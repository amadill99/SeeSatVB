Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class UserSettings

    Public Shared HasChanged As Boolean = False     ' settings have changed 
    Public Shared loc_lat, loc_lon, loc_elev, loc_tz As Double
    Public Shared loc_name As String

    Private Shared fstyle As NumberStyles = NumberStyles.Float
    Private Shared iculture As CultureInfo = CultureInfo.InvariantCulture


    Private Sub UserSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        loc_lat = SeeSatVBmain.my_loc.lat_deg
        loc_lon = SeeSatVBmain.my_loc.lon_deg
        loc_elev = SeeSatVBmain.my_loc.ht_in_meters
        loc_tz = SeeSatVBmain.my_loc.tz_offset
        loc_name = SeeSatVBmain.my_loc.loc_name

        TBLat.Text = SeeSatVBmain.my_loc.lat_deg.ToString(iculture)               '= My.Settings.user_lat
        TBLatDisp.Text = Parser.DecDegToDMSString(loc_lat, "N")
        'SeeSatVBmain.my_loc.lat_rad = My.Settings.user_lat * DefConst.DE2RA
        TBLong.Text = SeeSatVBmain.my_loc.lon_deg.ToString(iculture)         '= My.Settings.user_lon
        TBLongDisp.Text = Parser.DecDegToDMSString(loc_lon, "E")
        'SeeSatVBmain.my_loc.lon_rad = My.Settings.user_lon * DefConst.DE2RA
        TBElev.Text = SeeSatVBmain.my_loc.ht_in_meters.ToString(iculture)    '= My.Settings.user_ht_in_meters
        TBLName.Text = SeeSatVBmain.my_loc.loc_name              '= My.Settings.user_loc_name
        TBTZoffset.Text = SeeSatVBmain.my_loc.tz_offset.ToString(iculture)   '= My.Settings.user_tz

    End Sub

    Private Sub TBLat_Leave(sender As Object, e As EventArgs) Handles TBLat.Leave
        Dim lat As Double
        lat = Parser.ParseAll(TBLat.Text)
        TBLatDisp.Text = Parser.DecDegToDMSString(lat, "N")
        If lat <> SeeSatVBmain.my_loc.lat_deg Then
            HasChanged = True
            loc_lat = lat
        End If
    End Sub

    Private Sub TBlong_Leave(sender As Object, e As EventArgs) Handles TBLong.Leave
        Dim lon As Double
        lon = Parser.ParseAll(TBLong.Text)
        TBLongDisp.Text = Parser.DecDegToDMSString(lon, "E")
        If lon <> SeeSatVBmain.my_loc.lon_deg Then
            HasChanged = True
            loc_lon = lon
        End If
    End Sub

    Private Sub TBElev_Leave(sender As Object, e As EventArgs) Handles TBElev.Leave
        Dim elev As Double
        If (String.IsNullOrWhiteSpace(TBElev.Text)) Then
            Exit Sub
        End If
        If Double.TryParse(TBElev.Text, elev) Then
            If elev <> SeeSatVBmain.my_loc.ht_in_meters Then
                HasChanged = True
                loc_elev = elev
            End If
        End If
    End Sub

    Private Sub TBTZoffset_Leave(sender As Object, e As EventArgs) Handles TBTZoffset.Leave
        Dim tz As Double
        If (String.IsNullOrWhiteSpace(TBTZoffset.Text)) Then
            Exit Sub
        End If
        If Double.TryParse(TBTZoffset.Text, tz) OrElse CBool(TBTZoffset.Text.CompareTo("0")) Then
            If tz <> SeeSatVBmain.my_loc.ht_in_meters Then
                HasChanged = True
                loc_tz = tz
            End If
        End If
    End Sub

    Private Sub TBLName_Leave(sender As Object, e As EventArgs) Handles TBLName.Leave
        'Dim name As String
        If (String.IsNullOrWhiteSpace(TBLName.Text)) Then
            Exit Sub
        End If
        If TBLName.Text <> SeeSatVBmain.my_loc.loc_name Then
            HasChanged = True
            loc_name = TBLName.Text
        End If
    End Sub


    Private Sub BExit_Click(sender As Object, e As EventArgs) Handles BExit.Click
        If HasChanged Then
            SeeSatVBmain.my_loc.lat_deg = loc_lat
            SeeSatVBmain.my_loc.lat2rad()

            SeeSatVBmain.my_loc.lon_deg = loc_lon
            SeeSatVBmain.my_loc.lon2rad()

            SeeSatVBmain.my_loc.ht_in_meters = loc_elev
            SeeSatVBmain.my_loc.tz_offset = loc_tz
            SeeSatVBmain.my_loc.loc_name = loc_name

            If SeeSatVBmain.REALTIME Then
                SeeSatVBmain.InitRealTime()
            End If
        End If

        Me.Dispose()

    End Sub
End Class



' use the VB.Net regular expression utilities
' bout time they caught up to perl :-)
' from http://www.codeproject.com/KB/recipes/geospatial/Geospatial.zip
Public Class Parser

    '    ^\s*                 # Ignore any whitespace at the start of the string
    '(?<latSuf>[NS])?     # Optional suffix
    '(?<latDeg>.+?)       # Match anything and we'll try to parse it later
    '[D\*\u00B0]?\s*      # Degree symbol ([D|*|°] optional) followed by optional whitespace
    '(?<latSuf>[NS])?\s+  # Suffix could also be here. Need some whitespace to separate
    '(?<lonSuf>[EW])?     # Now try the longitude
    '(?<lonDeg>.+?)       # Degrees
    '[D\*\u00B0]?\s*      # Degree symbol + whitespace
    '(?<lonSuf>[EW])?     # Optional suffix
    '\s*$                 # Match the end of the string (ignoring whitespace)";

    Public Const DegPattern As String = _
        "^\s*" + _
        "(?<Suf>[NSEW])?" + _
        "(?<Deg>.+?)" + _
        "[D\*\u00B0]?\s*" + _
        "(?<Suf>[NSEW])?\s*$"

    '    ^\s*                 # Ignore any whitespace at the start of the string
    '(?<latSuf>[NS])?     # Optional suffix
    '(?<latDeg>.+?)       # Match anything
    '[D\*\u00B0\s]        # Degree symbol or whitespace
    '[D\*\u00B0\s]       # Now look for minutes
    '[M'\u2032\u2019]?\s* # Minute symbol [single quote, prime, smart quote, M] + whitespace
    '(?<latSuf>[NS])?\s+  # Optional suffix + whitespace
    '(?<lonSuf>[EW])?      # Now try the longitude
    '(?<lonDeg>.+?)        # Degrees
    '[D\*\u00B0?\s]        # Degree symbol or whitespace
    '(?<lonMin>.+?)        # Minutes
    '[M'\u2032\u2019]?\s*  # Minute symbol
    '(?<lonSuf>[EW])?      # Optional suffix
    '\s*$                  # Match the end of the string (ignoring whitespace)";

    Public Const DegMinPattern As String = _
        "^\s*" + _
        "(?<Suf>[NSEW])?" + _
        "(?<Deg>.+?)" + _
        "[D\*\u00B0\s]" + _
        "(?<Min>.+?)" + _
        "[M'\u2032\u2019]?\s*" + _
        "(?<Suf>[NSEW])?\s*$"

    '    ^\s*                  # Ignore any whitespace at the start of the string
    '(?<latSuf>[NS])?      # Optional suffix
    '(?<latDeg>.+?)        # Match anything
    '[D\*\u00B0\s]         # Degree symbol/whitespace
    '(?<latMin>.+?)        # Now look for minutes
    '[M'\u2032\u2019\s]    # Minute symbol/whitespace
    '(?<latSec>.+?)        # Look for seconds
    '[""\u2033\u201D]?\s*  # Second symbol [double quote (c# escaped), double prime or smart doube quote] + whitespace
    '(?<latSuf>[NS])?\s+   # Optional suffix + whitespace
    '(?<lonSuf>[EW])?      # Now try the longitude
    '(?<lonDeg>.+?)        # Degrees
    '[D\*\u00B0\s]         # Degree symbol/whitespace
    '(?<lonMin>.+?)        # Minutes
    '[M'\u2032\u2019\s]    # Minute symbol/whitespace
    '(?<lonSec>.+?)        # Seconds
    '[""\u2033\u201D]?\s*  # Second symbol
    '(?<lonSuf>[EW])?      # Optional suffix    
    '\s*$                  # Match the end of the string (ignoring whitespace)";

    Public Const DegMinSecPattern As String = _
        "^\s*" + _
        "(?<Suf>[NSEW])?" + _
        "(?<Deg>.+?)" + _
        "[D\*\u00B0\s]" + _
        "(?<Min>.+?)" + _
        "[M'\u2032\u2019\s]" + _
        "(?<Sec>.+?)" + _
        "[""\u2033\u201D]?\s*" + _
        "(?<Suf>[NSEW])?\s*$"

    Private Const RegexOptions As RegexOptions = RegexOptions.Compiled Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase

    Private Shared DegRegex As New Regex(DegPattern, RegexOptions)
    Private Shared DegMinRegex As New Regex(DegMinPattern, RegexOptions)
    Private Shared DegMinSecRegex As New Regex(DegMinSecPattern, RegexOptions)

    Private Shared fstyle As NumberStyles = NumberStyles.Float
    Private Shared iculture As CultureInfo = CultureInfo.InvariantCulture


    Public Shared Function ParseAll(value As String) As Double
        ' try each of the parsers in turn
        Dim rval As Double
        If (String.IsNullOrWhiteSpace(value)) Then
            Return Nothing
        End If

        rval = Parse(value, DegRegex)
        If rval = 0 Then
            rval = Parse(value, DegMinRegex)
        End If

        If rval = 0 Then
            rval = Parse(value, DegMinSecRegex)
        End If

        Return rval

    End Function

    Public Shared Function ParseDeg(value As String) As Double
        If (String.IsNullOrWhiteSpace(value)) Then
            Return Nothing
        End If
        Return Parse(value, DegRegex)
    End Function

    Public Shared Function ParseDegMin(value As String) As Double
        If (String.IsNullOrWhiteSpace(value)) Then
            Return Nothing
        End If
        Return Parse(value, DegMinRegex)
    End Function

    Public Shared Function ParseDegMinSec(value As String) As Double
        If (String.IsNullOrWhiteSpace(value)) Then
            Return Nothing
        End If
        Return Parse(value, DegMinSecRegex)
    End Function

    Friend Shared Function Parse(ByVal input As String, ByVal regex As Regex) As Double
        Dim match As Match = regex.Match(input.Replace(", ", " "))
        If match.Success Then
            '    Dim latitude As Angle = ParseAngle(provider, TryGetValue(match, "latSuf"), TryGetValue(match, "latDeg"), TryGetValue(match, "latMin"), TryGetValue(match, "latSec"))
            Dim decDegrees As Double = ParseAngle(TryGetValue(match, "Suf"), TryGetValue(match, "Deg"), TryGetValue(match, "Min"), TryGetValue(match, "Sec"))
            'Dim longitude As Double = ParseAngle(TryGetValue(match, "lonSuf"), TryGetValue(match, "lonDeg"), TryGetValue(match, "lonMin"), TryGetValue(match, "lonSec"))
            '    Dim longitude As Angle = ParseAngle(provider, TryGetValue(match, "lonSuf"), TryGetValue(match, "lonDeg"), TryGetValue(match, "lonMin"), TryGetValue(match, "lonSec"))

            Return decDegrees
        End If
        Return Nothing

    End Function

    Friend Shared Function ParseAngle(ByVal suffix As String, ByVal degrees As String, Optional ByVal minutes As String = Nothing, Optional ByVal seconds As String = Nothing) As Double
        Dim degreeValue As Double = 0
        Dim minuteValue As Double = 0
        Dim secondValue As Double = 0

        '      ' First try parsing the values (minutes and seconds are optional)
        If Not Double.TryParse(degrees, fstyle, iculture, degreeValue) OrElse ((minutes IsNot Nothing) _
                AndAlso Not Double.TryParse(minutes, fstyle, iculture, minuteValue)) OrElse ((seconds IsNot Nothing) _
                AndAlso Not Double.TryParse(seconds, fstyle, iculture, secondValue)) Then
            Return Nothing
        End If

        '      ' We've parsed all the information! Make everything the same
        '      ' sign.
        minuteValue = Math.Abs(minuteValue)
        secondValue = Math.Abs(secondValue)

        '      ' Check the suffix (takes priority over positive/negtive sign).
        If Not String.IsNullOrEmpty(suffix) Then
            ' Change degreeValue into a known sign
            degreeValue = Math.Abs(degreeValue)

            If suffix.Equals("S", StringComparison.OrdinalIgnoreCase) OrElse suffix.Equals("W", StringComparison.OrdinalIgnoreCase) Then
                Return FromDegrees(-degreeValue, -minuteValue, -secondValue)
            End If

            ' Else assume it's N/E and return positive angles.
            Return FromDegrees(degreeValue, minuteValue, secondValue)
        End If

        '      ' Check if we need to negate to match the degrees (if we type
        '      ' "-6° 12.3'" we expect the whole thing to be negative).
        '      ' We can't just check if degreeValue is negative, as we could
        '      ' have "-0° 12.3'".
        Dim negativeSign As String = "-"
        If degrees.StartsWith(negativeSign, StringComparison.Ordinal) Then
            minuteValue = -minuteValue
            secondValue = -secondValue
        End If
        Return FromDegrees(degreeValue, minuteValue, secondValue)
    End Function

    Friend Shared Function FromDegrees(d As Double, Optional m As Double = 0, Optional s As Double = 0) As Double
        Return d + (m / 60) + (s / 3600)
    End Function

    Friend Shared Function TryGetValue(ByVal match As System.Text.RegularExpressions.Match, ByVal groupName As String) As String
        Dim group As Group = match.Groups(groupName)

        ' Need to check that only a single capture occured, as the suffixes are used more than once
        If group.Success AndAlso (group.Captures.Count = 1) Then
            Return group.Value
        End If
        Return Nothing
    End Function

    'pass a decimal angle in degrees and a flag N or E and get back a DD MM SS.S with (optional) N/S or E/W qualifer 
    Friend Shared Function DecDegToDMSString(ByVal Deg As Double, Optional ByVal Flag As String = "") As String
        Dim sign As String
        Dim d, m As Integer
        Dim s As Double
        Dim format As String = "{0}{1}° {2}' {3}"""

        Select Case Flag
            Case Is = "N"   ' we want lat
                If Math.Sign(Deg) < 0 Then
                    sign = "S"
                Else
                    sign = "N"
                End If
            Case Is = "E"   ' we want long
                If Math.Sign(Deg) < 0 Then
                    sign = "W"
                Else
                    sign = "E"
                End If
            Case Else   ' no flag or unrecognized
                If Math.Sign(Deg) < 0 Then
                    sign = "-"
                Else
                    sign = " "
                End If
        End Select

        Deg = Math.Abs(Deg)
        d = CInt(Fix(Deg))
        m = CInt(Fix((Deg - d) * 60))
        s = Math.Round((Deg - d - m / 60) * 3600, 2)

        Return String.Format(Format, sign, d, m, s)

    End Function

    'pass a decimal angle in degrees and get back a HH MM SS.S
    Friend Shared Function DecDegToHrString(ByVal Deg As Double, Optional ByVal Flag As String = "") As String
        Dim sign As String
        Dim d, h, m As Integer
        Dim s As Double
        Dim format As String = "{0}{1}h {2}m {3}s"

        Select Case Flag
            Case Is = "E"   ' we want long
                If Math.Sign(Deg) < 0 Then
                    sign = "W"
                Else
                    sign = "E"
                End If
            Case Else   ' no flag or unrecognized
                If Math.Sign(Deg) < 0 Then
                    sign = "-"
                Else
                    sign = " "
                End If
        End Select

        Deg = Math.Abs(Deg)
        h = CInt(Fix(Deg / DefConst.HRS2DEG))
        'd = CInt(Fix(Deg))
        m = CInt(Fix((Deg / DefConst.HRS2DEG - h) * 60))
        s = Math.Round((Deg / DefConst.HRS2DEG - h - m / 60) * 3600, 2)
        'h = CInt(Fix(d / DefConst.HRS2DEG))
        Return String.Format(format, sign, h, m, s)

    End Function
End Class