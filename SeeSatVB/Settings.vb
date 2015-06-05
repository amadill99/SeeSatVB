Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class UserSettings

    Public Shared LocHasChanged As Boolean = False     'location settings have changed 
    Public Shared FOVHasChanged As Boolean = False     'FOV settings have changed 

    Public Shared loc_lat, loc_lon, loc_elev, loc_tz As Double
    Public Shared loc_name As String

    Public Shared fov_rotation, fov_azm, fov_alt, fov_ra, fov_dec, fov_width, fov_height, fov_temp_height As Double

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

        'FOV settings
        fov_alt = My.Settings.user_fov_alt
        fov_azm = My.Settings.user_fov_azm
        fov_dec = My.Settings.user_fov_dec
        fov_ra = My.Settings.user_fov_ra
        fov_width = My.Settings.user_fov_width
        fov_height = My.Settings.user_fov_height
        fov_temp_height = fov_height
        fov_rotation = My.Settings.user_fov_rotation

        CBFOViscircle.Checked = My.Settings.user_fov_iscircle
        CBFOVTrack.Checked = My.Settings.user_fov_track
        CBFOVRotate.Checked = My.Settings.user_fov_rotate
        RBFOVradec.Checked = My.Settings.user_fov_useRA
        RBFOValtazm.Checked = Not My.Settings.user_fov_useRA
        CBFOVShow.Checked = My.Settings.user_fov_show

        TBFOVWidth.Text = fov_width.ToString(iculture)
        TBFOVHeight.Text = fov_height.ToString(iculture)
        TBFOVRotation.Text = fov_rotation.ToString(iculture)
        TBFOVAlt.Text = fov_alt.ToString(iculture)
        TBFOVAltDisp.Text = Parser.DecDegToDMSString(fov_alt)
        TBFOVAzm.Text = fov_azm.ToString(iculture)
        TBFOVAzmDisp.Text = Parser.DecDegToDMSString(fov_azm)
        TBFOVRA.Text = fov_ra.ToString(iculture)
        TBFOVRADisp.Text = Parser.DecDegToHrString(fov_ra)
        TBFOVDec.Text = fov_dec.ToString(iculture)
        TBFOVDecDisp.Text = Parser.DecDegToDMSString(fov_dec)

        If CBFOViscircle.Checked = True Then
            FOVcircleChanged()
        End If

        FOVpointmodeChanged()

        Select Case SeeSatVBmain.SettingsTabNdx
            Case "location"
                TabControlSettings.SelectedTab = LocationTab
            Case "fov"
                TabControlSettings.SelectedTab = FOVTab
        End Select

    End Sub

    Private Sub TBLat_Leave(sender As Object, e As EventArgs) Handles TBLat.Leave
        Dim lat As Double
        lat = Parser.ParseAll(TBLat.Text)
        TBLatDisp.Text = Parser.DecDegToDMSString(lat, "N")
        If lat <> SeeSatVBmain.my_loc.lat_deg Then
            LocHasChanged = True
            loc_lat = lat
        End If
    End Sub

    Private Sub TBlong_Leave(sender As Object, e As EventArgs) Handles TBLong.Leave
        Dim lon As Double
        lon = Parser.ParseAll(TBLong.Text)
        TBLongDisp.Text = Parser.DecDegToDMSString(lon, "E")
        If lon <> SeeSatVBmain.my_loc.lon_deg Then
            LocHasChanged = True
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
                LocHasChanged = True
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
                LocHasChanged = True
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
            LocHasChanged = True
            loc_name = TBLName.Text
        End If
    End Sub



    Private Sub CBFOV_Leave(sender As Object, e As EventArgs) Handles CBFOVShow.Leave, CBFOVTrack.Leave, CBFOVRotate.Leave
        'check to see if the state has changed
        If sender Is CBFOVShow And My.Settings.user_fov_show <> CBFOVShow.Checked Then
            FOVHasChanged = True
        End If
        If sender Is CBFOVRotate And My.Settings.user_fov_rotate <> CBFOVRotate.Checked Then
            FOVHasChanged = True
        End If
        If sender Is CBFOVTrack And My.Settings.user_fov_track <> CBFOVTrack.Checked Then
            FOVHasChanged = True
        End If
    End Sub

    Private Sub CBFOViscircle_Leave(sender As Object, e As EventArgs) Handles CBFOViscircle.Leave
        If My.Settings.user_fov_iscircle <> CBFOViscircle.Checked Then
            FOVHasChanged = True
        End If
        FOVcircleChanged()
    End Sub

    Private Sub CBFOViscircle_CheckedChanged(sender As Object, e As EventArgs) Handles CBFOViscircle.CheckedChanged
        If sender Is CBFOViscircle Then
            FOVcircleChanged()
        End If
    End Sub

    Private Sub FOVcircleChanged()
        If CBFOViscircle.Checked = True Then        'disable the height tb and set to the width
            TBFOVHeight.Text = TBFOVWidth.Text
            TBFOVHeight.Enabled = False
            CBFOVRotate.Enabled = False
            TBFOVRotation.Enabled = False
            fov_temp_height = fov_height
            fov_height = fov_width
        Else                                        'restore the old setting
            fov_height = fov_temp_height
            TBFOVHeight.Enabled = True
            CBFOVRotate.Enabled = True
            TBFOVRotation.Enabled = True
            TBFOVHeight.Text = fov_height.ToString(iculture)
        End If
    End Sub

    Private Sub TBFOVWidth_Leave(sender As Object, e As EventArgs) Handles TBFOVWidth.Leave
        Dim width As Double
        If (String.IsNullOrWhiteSpace(TBFOVWidth.Text)) Then
            Exit Sub
        End If
        If Double.TryParse(TBFOVWidth.Text, width) OrElse CBool(TBFOVWidth.Text.CompareTo("0")) Then
            If width <> My.Settings.user_fov_width Then
                FOVHasChanged = True
                fov_width = width
            End If
        End If
    End Sub

    Private Sub TBFOVHeight_Leave(sender As Object, e As EventArgs) Handles TBFOVHeight.Leave
        Dim height As Double
        If (String.IsNullOrWhiteSpace(TBFOVHeight.Text)) Then
            Exit Sub
        End If
        If Double.TryParse(TBFOVHeight.Text, height) OrElse CBool(TBFOVHeight.Text.CompareTo("0")) Then
            If height <> My.Settings.user_fov_height Then
                FOVHasChanged = True
                fov_height = height
            End If
        End If
    End Sub

    Private Sub TBFOVRotation_Leave(sender As Object, e As EventArgs) Handles TBFOVRotation.Leave
        Dim rotation As Double
        If (String.IsNullOrWhiteSpace(TBFOVRotation.Text)) Then
            Exit Sub
        End If
        If Double.TryParse(TBFOVRotation.Text, rotation) OrElse CBool(TBFOVRotation.Text.CompareTo("0")) Then
            If rotation <> My.Settings.user_fov_rotation Then
                FOVHasChanged = True
                fov_rotation = rotation
            End If
        End If
    End Sub

    Private Sub FOVpointmodeChanged()
        If RBFOValtazm.Checked = True Then
            TBFOVRA.Enabled = False
            TBFOVDec.Enabled = False
            TBFOVAlt.Enabled = True
            TBFOVAzm.Enabled = True
        Else
            TBFOVRA.Enabled = True
            TBFOVDec.Enabled = True
            TBFOVAlt.Enabled = False
            TBFOVAzm.Enabled = False
        End If
    End Sub

    Private Sub RBFOValtazm_CheckedChanged(sender As Object, e As EventArgs) Handles RBFOValtazm.CheckedChanged
        FOVpointmodeChanged()
    End Sub

    Private Sub TBFOVAlt_Leave(sender As Object, e As EventArgs) Handles TBFOVAlt.Leave
        Dim alt As Double
        alt = Parser.ParseAll(TBFOVAlt.Text)
        TBFOVAltDisp.Text = Parser.DecDegToDMSString(alt)
        If alt <> My.Settings.user_fov_alt Then
            FOVHasChanged = True
            fov_alt = alt
        End If
    End Sub

    Private Sub TBFOVAzm_Leave(sender As Object, e As EventArgs) Handles TBFOVAzm.Leave
        Dim azm As Double
        azm = Parser.ParseAll(TBFOVAzm.Text)
        TBFOVAzmDisp.Text = Parser.DecDegToDMSString(azm)
        If azm <> My.Settings.user_fov_azm Then
            FOVHasChanged = True
            fov_azm = azm
        End If
    End Sub

    Private Sub TBFOVDec_Leave(sender As Object, e As EventArgs) Handles TBFOVDec.Leave
        Dim dec As Double
        dec = Parser.ParseAll(TBFOVDec.Text)
        TBFOVDecDisp.Text = Parser.DecDegToDMSString(dec)
        If dec <> My.Settings.user_fov_dec Then
            FOVHasChanged = True
            fov_dec = dec
        End If
    End Sub

    Private Sub TBFOVRA_Leave(sender As Object, e As EventArgs) Handles TBFOVRA.Leave
        Dim ra As Double
        ra = Parser.ParseAll(TBFOVRA.Text)      ' lets try a dec degree, dm, dms pattern first
        If ra = 0 Then
            ra = Parser.ParseHrMinSec(TBFOVRA.Text) 'that didn't work so maybe they are using hms
        End If

        TBFOVRADisp.Text = Parser.DecDegToHrString(ra)
        If ra <> My.Settings.user_fov_ra Then
            FOVHasChanged = True
            fov_ra = ra
        End If
    End Sub

    Private Sub BExit_Click(sender As Object, e As EventArgs) Handles BLocExit.Click, BFOVExit.Click
        If LocHasChanged Then
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

        If FOVHasChanged Then
            My.Settings.user_fov_iscircle = CBFOViscircle.Checked
            My.Settings.user_fov_track = CBFOVTrack.Checked
            My.Settings.user_fov_rotate = CBFOVRotate.Checked
            My.Settings.user_fov_useRA = RBFOVradec.Checked
            My.Settings.user_fov_show = CBFOVShow.Checked

            My.Settings.user_fov_rotation = fov_rotation
            My.Settings.user_fov_width = fov_width
            My.Settings.user_fov_height = fov_height
            My.Settings.user_fov_alt = fov_alt
            My.Settings.user_fov_azm = fov_azm
            My.Settings.user_fov_ra = fov_ra
            My.Settings.user_fov_dec = fov_dec

            If SeeSatVBmain.REALTIME Then
                FOV.initialize()
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

    Public Const HrMinSecPattern As String = _
        "^\s*" + _
        "(?<Suf>[EW])?" + _
        "(?<Hr>.+?)" + _
        "[h\*\s]" + _
        "(?<Min>.+?)" + _
        "[M'\u2032\u2019\s]" + _
        "(?<Sec>.+?)" + _
        "[""\u2033\u201D]?\s*" + _
        "(?<Suf>[NSEW])?\s*$"

    Private Const RegexOptions As RegexOptions = RegexOptions.Compiled Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase

    Private Shared DegRegex As New Regex(DegPattern, RegexOptions)
    Private Shared DegMinRegex As New Regex(DegMinPattern, RegexOptions)
    Private Shared DegMinSecRegex As New Regex(DegMinSecPattern, RegexOptions)
    Private Shared HrMinSecRegex As New Regex(HrMinSecPattern, RegexOptions)

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

    Public Shared Function ParseHrMinSec(value As String) As Double
        If (String.IsNullOrWhiteSpace(value)) Then
            Return Nothing
        End If

        Dim match As Match = HrMinSecRegex.Match(value.Replace(", ", " "))
        If match.Success Then
            Dim decDegrees As Double = ParseAngle(TryGetValue(match, "Suf"), TryGetValue(match, "Hr"), TryGetValue(match, "Min"), TryGetValue(match, "Sec"))
            decDegrees *= 15
            Return decDegrees
        End If
        Return Nothing

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
    Friend Shared Function DecDegToDMSString(ByVal Deg As Double, Optional ByVal Flag As String = "", Optional ByVal precis As Integer = 2) As String
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
        s = Math.Round((Deg - d - m / 60) * 3600, precis)

        Return String.Format(format, sign, d, m, s)

    End Function

    'pass a decimal angle in degrees and get back a HH MM SS.S
    Friend Shared Function DecDegToHrString(ByVal Deg As Double, Optional ByVal Flag As String = "", Optional ByVal precis As Integer = 2) As String
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
        s = Math.Round((Deg / DefConst.HRS2DEG - h - m / 60) * 3600, precis)
        'h = CInt(Fix(d / DefConst.HRS2DEG))
        Return String.Format(format, sign, h, m, s)

    End Function
End Class