Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Globalization


Public Class SatIO

    Public Shared stars() As star_t

    Public Shared tle0Dict As New Dictionary(Of Integer, tle0_t)

    ' these are user defined parameters
    Public Shared CBRIGHT As Single = SeeSatVBmain.my_params.star_bright    'brightness factor
    Public Shared CTRANSP As Single = SeeSatVBmain.my_params.star_transp    'transparency factor

    ' global array for tle0 elements
    Public Shared Tle0Ndx As Integer = 0   ' Last satellite in array sats

    Shared Function ReadStars(ByVal fname As String) As Integer

        'fname = "E:\Source\Satellite\Data\hipstarcat - Copy.csv"
        'fname = "E:\Source\Satellite\Data\hipstarcat.csv"

        If fname.Length <= 1 Or fname = "foo" Then
            fname = My.Settings.user_star_path
        End If


        Dim increment As Integer = 100

        ReDim stars(increment)  ' set initial size
        Dim StarNdx As Integer = 1

        Using MyReader As New Microsoft.VisualBasic.
                      FileIO.TextFieldParser(fname)
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim currentRow As String()

            SeeSatVBmain.TextBox1.AppendText("Module ReadStars - reading " + fname + vbNewLine)

            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()

                    If currentRow(0) = "RRA" Then
                        Continue While
                    End If

                    Dim myStar As New star_t

                    'Dim currentField As String
                    'For Each currentField In currentRow
                    'SeeSatVBmain.TextBox1.AppendText(".")
                    'MsgBox(currentField)
                    'Next
                    'SeeSatVBmain.TextBox1.AppendText(vbNewLine)

                    myStar.ra = Double.Parse(currentRow(0), CultureInfo.InvariantCulture) '* DefConst.DE2RA
                    myStar.dec = Double.Parse(currentRow(1), CultureInfo.InvariantCulture) '* DefConst.DE2RA
                    myStar.mag = Double.Parse(currentRow(2), CultureInfo.InvariantCulture)
                    myStar.name = currentRow(3)
                    htmlColorToARGB(currentRow(4).Trim, myStar.colors)
                    myStar.cosdec = Math.Cos(myStar.dec)
                    myStar.sindec = Math.Sin(myStar.dec)

                    stars(StarNdx) = myStar

                    StarNdx += 1
                    If (StarNdx Mod increment) = 0 Then
                        ReDim Preserve stars(StarNdx + increment)
                    End If

                Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " is not valid and will be skipped. " + vbNewLine)
                Catch ex As System.IO.FileNotFoundException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " File not found fatal error reading star data." + vbNewLine)
                    StarNdx = -1
                    Exit While
                Catch ex As System.InvalidCastException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " Cast exception error reading star data." + vbNewLine)
                    StarNdx = -1
                    Exit While
                End Try
            End While
        End Using

        If StarNdx > 1 Then
            ReDim Preserve stars(StarNdx - 1)
            SatWindow.ShowStars = True
        Else
            ReDim stars(1)
            StarNdx = 0
            SatWindow.ShowStars = False
        End If

        SeeSatVBmain.TextBox1.AppendText("read " + CStr(StarNdx) + " stars." + vbNewLine)

        ReadStars = StarNdx

    End Function

    Private Shared Sub htmlColorToARGB(ByVal wcolor As String, ByRef colors() As Integer, Optional ByVal mag As Single = 4.0)
        ' convert and possibly adjust html style color to ARGB values
        Dim tcolor As Color = ColorTranslator.FromHtml(wcolor)  'scratchpad
        colors(0) = CInt(tcolor.A * CTRANSP)
        colors(1) = CInt(tcolor.R * CBRIGHT)
        colors(2) = CInt(tcolor.G * CBRIGHT)
        colors(3) = CInt(tcolor.B * CBRIGHT)
        tcolor = Color.FromArgb(colors(0), colors(0), colors(0), colors(0))
    End Sub

    Shared Function ReadMcname(ByVal fname As String, Optional ByVal clear As Boolean = False) As Integer
        ' read in the Mcnames visual elements and put them in an array

        ' Struct to hold the McCant style line 0 information
        'Columns
        '01-14  Name
        '17-20  Length, m  (1)
        '22-25  Width, m   (2)
        '27-30  Depth, m
        '31-35  Standard magnitude (at 1000 km range, and 50% illuminated) //mag = stdmag - 15.75 + 2.5 * log10 (range * range / fracil) //(fraction illuminated 0 to 1)
        '37-37  Standard magnitude source flag  //based on dimensions or visual obs, a letter "d" in column 37, the latter by a "v". Added "u" as unknown
        '39-42  Radar Cross Section value (4)
        Dim mnFormat1 As Integer() = {6, 16, 5, 5, 5, 5, 2, -1}  ' all parameters
        Dim mnFormat2 As Integer() = {6, -1}                     ' just the name
        Dim mnFormat3 As Integer() = {6, 16, 5, 5, 5, 5, -1}     ' no rxc
        '1234567890123456789012345678901234567890123456789012345678901234567890
        '   ID           Name   Len  Wid  Dep SMag F RadX
        '00005 Vanguard 1       0.2  0.0  0.0 11.0 v 0.12
        '25544 ISS             30.0 20.0  0.0 -0.5 v  348

        If fname.Length <= 1 Or fname = "foo" Then
            fname = My.Settings.user_mcnames_path
        End If

        Dim rval As Integer = 0
        Using MyReader As New Microsoft.VisualBasic.
              FileIO.TextFieldParser(fname)
            MyReader.TextFieldType = FileIO.FieldType.FixedWidth
            MyReader.FieldWidths = mnFormat1

            ' if we made it this far then re-intialize the array
            'ReDim Tle0array(0)
            Tle0Ndx = 1

            Dim ID As Integer = 0
            If clear Then
                tle0Dict.Clear()
            End If

            Dim currentRow As String()
            SeeSatVBmain.TextBox1.AppendText("Module ReadMcnames - reading mcnames visual data. " + fname + vbNewLine)

            While Not MyReader.EndOfData
                Try
                    Dim rowType = MyReader.PeekChars(49)
                    Dim toNext As Boolean = False

                    Select Case rowType.Length
                        Case Is >= 48    ' has rxc
                            MyReader.FieldWidths = mnFormat1
                            If Mid(rowType, 40, 1) <> "." Then
                                toNext = True   ' there is no valid mag
                            End If
                        Case Is < 24    ' just the name - not interested
                            MyReader.FieldWidths = mnFormat2
                            toNext = True
                        Case Is >= 42   ' has at least the mag
                            MyReader.FieldWidths = mnFormat3
                    End Select

                    currentRow = MyReader.ReadFields()

                    If toNext Then
                        Continue While
                    End If

                    'Dim currentField As String
                    'For Each currentField In currentRow
                    ' SeeSatVBmain.TextBox1.appendText ( currentField + "|")
                    'MsgBox(currentField)
                    'Next
                    'SeeSatVBmain.TextBox1.AppendText(".")
                    'SeeSatVBmain.TextBox1.Refresh()

                    If currentRow(4) <> "" Then

                        'Dim tle0a As New tle0_array
                        Dim tle0 As New tle0_t

                        ID = CInt(currentRow(0))
                        tle0.satname = currentRow(1)

                        If rowType.Length >= 42 Then
                            tle0.smag = Single.Parse(currentRow(5), CultureInfo.InvariantCulture)   ' already molcazan magnitudes
                            tle0.slength = Single.Parse(currentRow(2), CultureInfo.InvariantCulture)
                            tle0.swidth = Single.Parse(currentRow(3), CultureInfo.InvariantCulture)
                            tle0.sdepth = Single.Parse(currentRow(4), CultureInfo.InvariantCulture)
                            If rowType.Length >= 48 Then
                                tle0.rxsect = Single.Parse(currentRow(7), CultureInfo.InvariantCulture)
                            Else
                                tle0.rxsect = 0
                            End If
                        End If

                        ' still to do deal with mag flag
                        ' the mccants codes are d and v

                        Select Case CChar(currentRow(6))
                            Case Is = CChar("")
                                tle0.magflg = CChar("u")
                            Case Else
                                tle0.magflg = CChar(currentRow(6))
                        End Select

                        If Not tle0Dict.ContainsKey(ID) Then
                            tle0Dict.Add(ID, tle0)
                        Else
                            tle0Dict(ID) = tle0
                        End If

                        Tle0Ndx += 1

                    End If


                    'SeeSatVBmain.TextBox1.Text = SeeSatVBmain.TextBox1.Text + vbNewLine
                    'SeeSatVBmain.TextBox1.Refresh()

                Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " and is invalid - skipping. " + vbNewLine)
                Catch ex As System.InvalidCastException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " Cast exception error reading mcnames data." + vbNewLine)
                    Tle0Ndx = -1
                    Exit While
                Catch ex As System.IO.FileNotFoundException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " File not found fatal error reading mcnames data." + vbNewLine)
                    Tle0Ndx = -1
                    Exit While
                End Try
            End While
        End Using

        SeeSatVBmain.TextBox1.AppendText("Read " + CStr(Tle0Ndx) + " elements." + vbNewLine)

        ReadMcname = Tle0Ndx

    End Function

    Shared Function ReadQsat(ByVal fname As String, Optional ByVal clear As Boolean = False) As Integer
        ' read in the quicksat visual elements and put them in an array
        Dim qsFormat1 As Integer() = {5, 3, 8, 16, 6, 4, 4, 4, 4, -1}   ' all parameters and a comment
        Dim qsFormat2 As Integer() = {5, 3, 8, 16, 6, 4, 4, 4, 4}       ' all parameters
        Dim qsFormat3 As Integer() = {5, 3, 8, 16, 6, 4, 4, 4}          ' no radar cross sect
        Dim qsFormat4 As Integer() = {5, 3, 8, 16, 5}                   ' just the mag
        Dim qsFormat5 As Integer() = {5, -1}                            ' id number and name only - throw away
        '1234567890123456789012345678901234567890123456789012345678901234567890
        '00001 d Desig...  Name.......... Mag.  Sz1 Sz2 Sz3 RCS Comments 40108
        '00005   58 2B     Vanguard 1      9.5  0.2 0.0 0.0 .12
        '25544   98 67A    ISS            -2.0              348
        ' to....
        'Public Structure tle0_t
        '        Public slength, swidth, sdepth, smag, rxsect As Single
        '        Public magflg As Char
        '        Public satname As String      'initialize to 14 characters
        If fname.Length <= 1 Or fname = "foo" Then
            fname = My.Settings.user_qsat_path
        End If

        'Tle0Ndx = Tle0array.Length

        Using MyReader As New Microsoft.VisualBasic.
                      FileIO.TextFieldParser(fname)
            MyReader.TextFieldType = FileIO.FieldType.FixedWidth
            MyReader.FieldWidths = qsFormat1

            ' if we made it this far then re-intialize the array
            'ReDim Tle0array(0)
            Tle0Ndx = 1

            Dim ID As Integer = 0
            If clear Then
                tle0Dict.Clear()
            End If

            Dim currentRow As String()
            SeeSatVBmain.TextBox1.AppendText("Module ReadVisual - reading quicksat visual data. " + fname + vbNewLine)

            While Not MyReader.EndOfData
                Try
                    Dim rowType = MyReader.PeekChars(60)
                    Dim toNext As Boolean = False

                    Select Case rowType.Length
                        Case Is > 54    ' comment at the end
                            MyReader.FieldWidths = qsFormat1
                        Case Is = 54    ' no comment but has rxc
                            MyReader.FieldWidths = qsFormat2
                        Case Is = 50    ' no rxc
                            MyReader.FieldWidths = qsFormat3
                        Case Is = 37    ' mag only
                            MyReader.FieldWidths = qsFormat4
                        Case Is < 37    ' not interested
                            MyReader.FieldWidths = qsFormat5
                            toNext = True
                    End Select

                    currentRow = MyReader.ReadFields()

                    If toNext Then
                        Continue While
                    End If

                    'Dim currentField As String
                    'For Each currentField In currentRow
                    ' SeeSatVBmain.TextBox1.appendText ( currentField + "|")
                    'MsgBox(currentField)
                    'Next
                    'SeeSatVBmain.TextBox1.AppendText(".")
                    'SeeSatVBmain.TextBox1.Refresh()

                    If CInt(currentRow(0)) = 1 Or CInt(currentRow(0)) = 99999 Then
                        Continue While
                    End If

                    If currentRow(4) <> "" Then

                        'Dim tle0a As New tle0_array
                        Dim tle0 As New tle0_t

                        ID = CInt(currentRow(0))
                        tle0.satname = currentRow(3)
                        tle0.smag = str2sng(currentRow(4)) + 1.5!   ' convert to molcazan magnitudes
                        If rowType.Length >= 38 Then
                            tle0.slength = str2sng(currentRow(5))
                            tle0.swidth = str2sng(currentRow(6))
                            tle0.sdepth = str2sng(currentRow(7))
                            If rowType.Length >= 51 Then
                                tle0.rxsect = str2sng(currentRow(8))
                            Else
                                tle0.rxsect = 0
                            End If

                        Else
                            tle0.slength = 0
                            tle0.swidth = 0
                            tle0.sdepth = 0
                            tle0.rxsect = 0
                        End If

                        ' still to do deal with mag flag
                        ' the quicksat codes are c, f, t, h, d, g, J, s, l, and one i.

                        Select Case CChar(currentRow(1).Trim)
                            Case Is = CChar("")
                                tle0.magflg = CChar("q")
                            Case Else
                                tle0.magflg = CChar(currentRow(1).Trim)
                        End Select

                        If Not tle0Dict.ContainsKey(ID) Then
                            tle0Dict.Add(ID, tle0)
                        Else
                            tle0Dict(ID) = tle0
                        End If

                        Tle0Ndx += 1

                    End If


                    'SeeSatVBmain.TextBox1.Text = SeeSatVBmain.TextBox1.Text + vbNewLine
                    'SeeSatVBmain.TextBox1.Refresh()

                Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " and is invalid - skipping. " + vbNewLine)
                Catch ex As System.IO.FileNotFoundException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " File not found fatal error reading quicksat data." + vbNewLine)
                    Tle0Ndx = -1
                    Exit While
                Catch ex As InvalidCastException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " Cast exception error reading quicksat data." + vbNewLine)
                    Tle0Ndx = -1
                    Exit While
                End Try
            End While
        End Using

        SeeSatVBmain.TextBox1.AppendText("Read " + CStr(Tle0Ndx) + " elements." + vbNewLine)

        ReadQsat = Tle0Ndx
    End Function


    Private Shared Function str2sng(ByVal s As String) As Single
        ' convert a string to a single value, 0 if null
        Dim rval As Single = 0
        If s.Length > 0 Then
            rval = Single.Parse(s, CultureInfo.InvariantCulture)
        End If
        str2sng = rval
    End Function

    Shared Function ReadTLE(ByVal fname As String) As Integer
        ' read in the tle file, parse it, add it to the satellites array

        If fname.Length <= 1 Or fname = "foo" Then
            fname = My.Settings.user_tle_path
        End If

        Dim tlechk As Integer = 0   ' return flag for parse elements
        'Dim tle As tle_t
        'Dim tle0 As tle0_t
        Dim sndx As Integer = 0
        Dim tlecnt As Integer = 0

        Dim tleNFormat As Integer() = {-1}              ' name only
        'Columns
        '01-14  Name - 14
        '15-16  null
        '17-20  Length, m  (1)
        '21     null
        '22-25  Width, m   (2)
        '26     null
        '27-30  Depth, m
        '31-35  Standard magnitude (at 1000 km range, and 50% illuminated) //mag = stdmag - 15.75 + 2.5 * log10 (range * range / fracil) //(fraction illuminated 0 to 1)
        '36     null
        '37-37  Standard magnitude source flag  //based on dimensions or visual obs, a letter "d" in column 37, the latter by a "v".
        '38     null
        '39-42  Radar Cross Section value (4)
        Dim tle0Format As Integer() = {14, 6, 5, 5, 5, 2, -1}   ' -1 signifies variable width
        Dim have0, have1, have2 As Boolean
        have0 = False
        have1 = False
        have2 = False

        Using MyReader As New FileIO.TextFieldParser(fname)
            MyReader.TextFieldType = FileIO.FieldType.FixedWidth
            MyReader.FieldWidths = tle0Format

            'Dim currentRow As String()
            Dim line0(), line1(), line2() As String   'string buffers

            SeeSatVBmain.TextBox1.AppendText("Module ReadTLE - reading TLE file. " + fname + vbNewLine)
            While Not MyReader.EndOfData
                Try
                    MyReader.FieldWidths = tle0Format
                    Dim rowType = MyReader.PeekChars(40)
                    If InStr(rowType, "1 ") = 1 Then      ' try for line 1
                        ' If this line describes an error, the format of the row will be different.
                        MyReader.SetFieldWidths(tleNFormat)
                        line1 = MyReader.ReadFields
                        have1 = True
                    ElseIf InStr(rowType, "2 ") = 1 Then  ' try for line 2
                        MyReader.SetFieldWidths(tleNFormat)
                        line2 = MyReader.ReadFields
                        If Not (line2(0) Is Nothing) Then
                            have2 = True
                        End If
                    ElseIf (InStr(rowType, " v ") = 36) Or (InStr(rowType, " d ") = 36) Then  ' try molcolzan tle0
                        MyReader.SetFieldWidths(tle0Format)
                        line0 = MyReader.ReadFields
                        'For Each newString In line0
                        '    SeeSatVBmain.TextBox1.AppendText(newString + "|")
                        'Next
                        have0 = True
                        ' call ParseTLE0
                    Else  ' just the name or we don't know what it is
                        MyReader.SetFieldWidths(tleNFormat)
                        ' assume that we have the name of the satellite
                        MyReader.SetFieldWidths(tleNFormat)
                        line0 = MyReader.ReadFields
                        have0 = True   ' actually is just maybe
                        ' create a tle0 struct and hang on to it
                    End If
                    'currentRow = MyReader.ReadFields
                    'SeeSatVBmain.TextBox1.AppendText(vbNewLine)

                    If have1 And have2 Then
                        Dim tle As New tle_t
                        tle.intl_desig = Space(DefConst.INTL_SIZE)
                        SeeSatVBmain._parse_elements(tlechk, line1(0), line2(0), tle)
                        If tlechk >= 0 Then ' we have a valid tle
                            If have0 Then
                                Dim tle0 As New tle0_t
                                ' we can fill in the tle0 struct
                                If line0.Length = 6 Then ' we have a valid tle0 line, use it
                                    If InStr(line0(0), "0 ") = 1 Then
                                        tle0.satname = line0(0).Substring(1)
                                    Else
                                        tle0.satname = line0(0)
                                    End If
                                    tle0.slength = str2sng(line0(1))
                                    tle0.swidth = str2sng(line0(2))
                                    tle0.sdepth = str2sng(line0(3))
                                    tle0.smag = str2sng(line0(4))
                                    tle0.rxsect = str2sng(line0(6))
                                    tle0.magflg = CChar(line0(5))
                                ElseIf tle0Dict.ContainsKey(tle.norad_number) Then  ' fill it in from the dictionary of visual values
                                    tle0 = tle0Dict(tle.norad_number)
                                Else
                                    If InStr(line0(0), "0 ") = 1 Then
                                        tle0.satname = line0(0).Substring(1)
                                    Else
                                        tle0.satname = line0(0)
                                    End If
                                    'tle0.satname = line0(0).Substring(1)     ' fill in default values
                                    tle0.slength = 0
                                    tle0.swidth = 0
                                    tle0.sdepth = 0
                                    If InStr(tle0.satname, " DEB") > 1 Then      ' it is likely debris
                                        tle0.smag = DefConst.SATDEFMAG + 4 ' lets make it really dim
                                        tle0.magflg = CChar("z")   'unknown
                                    Else
                                        tle0.smag = DefConst.SATDEFMAG      ' set default - nominally 6
                                        tle0.magflg = CChar("x")   'unknown
                                    End If
                                    tle0.rxsect = 0

                                End If
                                ' create or find sat element and attach the tle structs
                                sndx = SeeSatVBmain.find_or_create_sat_element(tle.intl_desig)
                                If InStr(line0(0), "0 ") = 1 Then
                                    SeeSatVBmain.satellites(sndx).tlename = line0(0).Substring(1)
                                Else
                                    SeeSatVBmain.satellites(sndx).tlename = line0(0)
                                End If
                                SeeSatVBmain.satellites(sndx).tle = tle
                                SeeSatVBmain.satellites(sndx).tle0 = tle0
                                SeeSatVBmain.satellites(sndx).is_valid = True
                                SeeSatVBmain.satellites(sndx).elset = Path.GetFileName(fname)
                                SeeSatVBmain._select_ephemeris(SeeSatVBmain.satellites(sndx).is_deep, SeeSatVBmain.satellites(sndx).tle)
                                Select Case SeeSatVBmain.satellites(sndx).is_deep
                                    Case Is = 0  ' is SGP4 model
                                        SeeSatVBmain.satellites(sndx).Model = DefConst.SGP4M
                                        SeeSatVBmain._SGPX_init(SeeSatVBmain.satellites(sndx).sat_params(0), _
                                                                SeeSatVBmain.satellites(sndx).tle, SeeSatVBmain.satellites(sndx).Model) ' initialize model
                                        tlecnt += 1
                                    Case Is = 1  ' is SDP4 model
                                        SeeSatVBmain.satellites(sndx).Model = DefConst.SDP4M
                                        SeeSatVBmain._SGPX_init(SeeSatVBmain.satellites(sndx).sat_params(0), _
                                                                SeeSatVBmain.satellites(sndx).tle, SeeSatVBmain.satellites(sndx).Model) ' initialize model
                                        tlecnt += 1
                                    Case Else
                                        ' we have an error here and we need to throw this away
                                        SeeSatVBmain.satellites(sndx).is_valid = False
                                End Select
                            End If

                            'start over
                            have0 = False
                            have1 = False
                            have2 = False

                        End If

                    End If

                Catch ex As FileIO.MalformedLineException
                    SeeSatVBmain.TextBox1.AppendText(" error reading TLE element." + vbNewLine)
                Catch ex As System.ArgumentOutOfRangeException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + " arg out of range error reading TLE." + vbNewLine)
                    tlecnt = -1
                    Exit While
                Catch ex As System.IO.FileNotFoundException
                    SeeSatVBmain.TextBox1.AppendText(ex.Message + "File not found fatal error reading TLE." + vbNewLine)
                    tlecnt = -1
                    Exit While
                End Try
            End While
        End Using
        SeeSatVBmain.SatNdx = SeeSatVBmain.satellites.Length - 1

        SeeSatVBmain.TextBox1.AppendText("Read " + CStr(tlecnt) + " elements." + vbNewLine)

        ReadTLE = tlecnt
    End Function

End Class

