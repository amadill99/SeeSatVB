Imports System.Runtime.InteropServices

<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> _
Public Structure tle_t
    Public epoch, xndt2o, xndd6o, bstar, xincl, xnodeo, eo, omegao, xmo, xno As Double  ' epoch is in julian days
    Public norad_number, bulletin_number, revolution_number As Integer
    Public classification, ephemeris_type As Char
    '<MarshalAs(UnmanagedType.ByValTStr, SizeConst:=9)> _
    Public intl_desig As String        ' must initialize sat.tle.intl_desig = Space(INTL_SIZE) (9)
End Structure

' Struct to hold the McCant style line 0 information
'Columns
'01-14  Name
'17-20  Length, m  (1)
'22-25  Width, m   (2)
'27-30  Depth, m
'31-35  Standard magnitude (at 1000 km range, and 50% illuminated) //mag = stdmag - 15.75 + 2.5 * log10 (range * range / fracil) //(fraction illuminated 0 to 1)
'37-37  Standard magnitude source flag  //based on dimensions or visual obs, a letter "d" in column 37, the latter by a "v". Added "u" as unknown
'39-42  Radar Cross Section value (4)

<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> _
Public Structure tle0_t
    Public slength, swidth, sdepth, smag, rxsect As Single
    Public magflg As Char
    Public satname As String      'initialize to 14 characters
End Structure

' sample qs.mag.txt format
' from https://www.prismnet.com/~mmccants/programs/qsmag.zip
' Uses "full phase" and "brightest likely magnitude"
'1234567890123456789012345678901234567890123456789012345678901234567890
'00001 d Desig...  Name.......... Mag.  Sz1 Sz2 Sz3 RCS Comments 40108
'00005   58 2B     Vanguard 1      9.5  0.2 0.0 0.0 .12
'25544   98 67A    ISS            -2.0              348

' sample mcnames.zip format
' from https://www.prismnet.com/~mmccants/tles/mcnames.zip
'Columns
'01-14  Name
'17-20  Length, m  (1)
'22-25  Width, m   (2)
'27-30  Depth, m
'31-35  Standard magnitude (at 1000 km range, and 50% illuminated) (3)
'37-37  Standard magnitude source flag
'39-42  Radar Cross Section value (4)
'(1) If width and depth are zero, then the object is a sphere, and the
'    length is its diameter. Objects with unknown dimensions have been
'    assumed to be spherical, and a value of diameter has been
'    "guesstimated".
'(2) If depth is zero, then the object is a cylinder, and width is its
'    diameter.
'(3) The standard magnitude may be an estimate based on the mean cross-
'    sectional area derived from its dimensions, or it may be a mean
'    value derived from visual observations. The former are denoted by a
'    letter "d" in column 37; the latter by a "v". To estimate the
'    magnitude at other ranges and illuminations, use the following formula:
'    mag = stdmag - 15.75 + 2.5 * log10 (range * range / fracil)
'    where : stdmag = standard magnitude as defined above
'            range = distance from observer to satellite, km
'            fracil = fraction of satellite illuminated,
'                     [ 0 <= fracil <= 1 ]

'1234567890123456789012345678901234567890123456789012345678901234567890
'   ID           Name   Len  Wid  Dep SMag F RadX
'00005 Vanguard 1       0.2  0.0  0.0 11.0 v 0.12
'25544 ISS             30.0 20.0  0.0 -0.5 v  348

'so the mcnames is the Molcazan format with the identifier in front

' translated to tle0
'1234567890123456789012345678901234567890123456789012345678901234567890
'          Name   Len  Wid  Dep SMag F RadX
'ISS             30.0 20.0  0.0 -0.5 v  348
'ISIS 1           1.1  1.3  0.0  9.0 v  2.2

'The differences are: 
'1) A phase angle definition difference causes Ted's intrinsic magnitude to be about 0.7 magnitudes fainter than a quicksat.mag file intrinsic magnitude. 
'2) The "average" magnitude also causes Ted's intrinsic magnitude to be about 0.7 magnitudes fainter than Quicksat's "brightest likely magnitude"
' (best possible object orientation). 
'So, the actual intrinsic magnitudes should differ by about 1.4 magnitudes between what is in the mcnames file and what is in the quicksat.mag file. 
'Half of that is due to the phase defintion difference, so if a program is compensating for that different definition, 
'the predicted magnitude would be the same. But the other half is a fundamental philosophical difference in definition. 
'So a program should always generate a prediction that is about 0.7 magnitudes fainter using Ted's intrinsic magnitude compared to the quicksat.mag intrinsic magnitude. 

'The "Molczan" magnitude values are based on 50% illumination; the "Quicksat" values are based on 100% illumination. 
'The difference mathematically is 0.8 magnitude. The "Molczan" magnitudes are 0.8 magnitudes fainter due to this assumption. 
'The "Molczan" values are based on "mean" magnitudes, while the "Quicksat" values are based on "maximum" magnitudes. 
'Typically (for a cylinder) the "mean" magnitude is about 0.7 magnitudes fainter than the "maximum" magnitude. On average, 
'the cylinder is tilted about 45 degrees relative to the observer compared to its maximum possible attitude. 
'The sum of the two differences in magnitude is about 1.5 magnitudes. So a typical Cosmos rocket would appear in the Quicksat 
'magnitude file as intrinsic 4.0 and in the Molczan file as intrinsic 5.5 , that is, its value of brightness in magnitude would be dimmer compared to the Quicksat value. 

'observers location parameters
' not in use so far so let's use the Hirose style
'<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> _
'Public Structure obs_loc_t
'Public olat, olon, ohgt, ojd, otz As Double
'Public oname As String      ' initialize to at least 50 chars
'End Structure

' structure to hold all of the items calculated in the Hirose functions in AstroVB
' this is a bit sloppy - most of these are classes and I'm not sure if one should create stucts of classes
Public Structure satxyz_t
    Public posn As xyz_t
    ' satellite topocentric coordinates.  These all come from xyztop(). 
    Public azel As sph_t   ' slant range, azimuth, elevation
    Public radec As sph_t  ' slant range, R.A., dec.
    Public latlon As sph_t ' altitude, latitude, longitude
    ' elevation (rounded to the nearest int) of center of sun above the
    'satellite's horizon.  Adjusted for dip due to height of satellite, but no
    'allowance for refraction.  Value comes from xyztop(). 
    Public elsusa As Integer
    Public phase As Double
    Public illum As Double
    Public truemag As Double
End Structure

' master structure for holding all of the satellites in a ring buffer
' sat(0) is always unused
<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> _
Public Structure sat_t
    Public sat_params() As Double       ' SGP model parameters
    'Public sdp_sat_params() As Double  ' SDP model parameters
    Public pos() As Double              ' pos and vel arrays
    Public vel() As Double
    Public is_deep As Integer           ' deep space object flag
    Public tag As Integer               ' tag / counter for flagging if below horizon
    Public elset As String              ' the name of the file it was read from
    Public tlename As String            ' the name as it is in the TLE file as opposed to the one that comes from the visual data
    Public is_valid As Boolean          ' flag to reuse or display
    Public Model As Integer             ' model SGP4, SGP8, SDP4, SDP8
    Public view As satxyz_t             ' azel, rade, latlon and such
    Public tle As tle_t                 ' put anything with a chance of changing in size last
    Public tle0 As tle0_t
End Structure

' deg, min, sec structure - used in AstroVB degdms
Public Structure dms_t
    Public deg As String
    Public min As String
    Public sec As String
End Structure


Public Class SeeSatVBmain

    ' data for observer's location 
    Public Shared obs As New obs_t()
    Public Shared my_loc As New obs_input

    'file paths
    Public Shared my_files As New file_names

    'program parameters
    Public Shared my_params As New prog_params

    ' current working Julian Date/Time shared 
    Public Shared JDPUB As Double
    ' offset to the time that we are working in
    Public Shared JDOFFSET As Double = 0  ' measured in decimal days

    ' ATLAS CENTAUR 2 
    'Dim tle1 As String = "1 00694U 63047A   14359.71852084  .00002470  00000-0  31909-3 0  6700"
    'Dim tle2 As String = "2 00694  30.3564  96.2656 0595383 111.9600  39.6867 14.00230273555744"


    'lat 44.01
    'lon -69.9
    '# Western longitudes are negative,  eastern positive
    'ht 100
    'JD 2452541.5         /* 24 Sep 2002 0h UT */
    ' ISS
    '                     1234567890123456789012345678901234567890123456789012345678901234567890
    '                     ISIS 1           1.1  1.3  0.0  9.0 v  2.2
    'Dim tle0 As String = "ISS             30.0 20.0  0.0 -2.0 v  348"
    'Dim tle1 As String = "1 25544U 98067A   02256.70033192  .00045618  00000-0  57184-3 0  1499"
    'Dim tle2 As String = "2 25544  51.6396 328.6851 0018421 253.2171 244.7656 15.59086742217834"
    'Above should give RA = 350.1615 deg, dec = -24.0241, dist = 1867.97542 km sept 24 2002

    '                     1234567890123456789012345678901234567890123456789012345678901234567890
    Dim tle0 As String = "ISS (ZARYA)     30.0 20.0  0.0 -0.5 v  348"
    Dim tle1 As String = "1 25544U 98067A   15043.32811632  .00024333  00000-0  36613-3 0  9996"
    Dim tle2 As String = "2 25544  51.6483 347.5817 0005937 359.5524 149.6677 15.54676862928606"
    ' up to date iss tle

    'Now compute a second,  higher satellite for the same place/time:
    'Dim tle0 As String = "Cosmos 1966 Rk"
    'Dim tle1 As String = "1 19448U 88076D   02255.52918163 -.00000002  00000-0  10000-3 0  4873"
    'Dim tle2 As String = "2 19448  65.7943 338.1906 7142558 193.4853 125.7046  2.04085818104610"
    'Above should give RA = 3.5743, dec = 30.4293, dist = 32114.83063 km

    ' toggle between testdate and UTC
    Public Shared REALTIME As Boolean = False
    ' How long to sleep in milisec if realtime
    Dim SLEEPTIME As Integer = 1000


    ' use this style of declarations if using the decorated style names
    'Public Declare Sub SGP4_init Lib "SGP4dll.dll" Alias "_SGP4_init@8" (ByRef params As Double, ByRef tle As tle_t)
    'Public Declare Function SGP4 Lib "SGP4dll.dll" Alias "_SGP4@24" (ByVal tsince As Double, ByRef params As Double, ByRef tle As tle_t, _
    '                                                                 ByRef pos As Double, ByRef vel As Double) As Integer
    'Public Declare Function select_ephemeris Lib "SGP4dll.dll" Alias "_select_ephemeris@4" (ByRef tle As tle_t) As Integer
    'Public Declare Function parse_elements Lib "SGP4dll.dll" Alias "_parse_elements@12" (ByRef sat As tle_t) As Integer
    'Public Declare Function sxpx_library_version Lib "SGP4dll.dll" Alias "_sxpx_library_version@0" () As Integer

    ' in order for this to work on an XP system it has to have the SGP4class.dll in the runtime folder or in the search path
    ' It also requires msvcr120.dll or msvcr120d.dll (debug)

    ' use this style of declarations if using the undecorated style names ie compiled using a .def file

    Public Declare Sub _select_ephemeris Lib "SGP4class.dll" _
        Alias "?select_ephemeris_cpp@SGP4class@SGP4space@@SGXPAHPBUtle_t@@@Z" _
        (ByRef is_deep As Integer, <MarshalAs(UnmanagedType.Struct)> ByRef tle As tle_t)
    '(ByVal tle As tle_t) As Integer
    '(<MarshalAs(UnmanagedType.Struct)> ByVal tle As tle_t) As Integer

    Public Declare Sub _lat_alt_to_parallax Lib "SGP4class.dll" Alias "_lat_alt_to_parallax@24" _
        (ByVal lat As Double, ByVal ht_in_meters As Double, ByRef rho_cos_phi As Double, ByRef rho_sin_phi As Double)

    Public Declare Sub _observer_cartesian_coords Lib "SGP4class.dll" Alias "_observer_cartesian_coords@36" _
            (ByVal jd As Double, ByVal lon As Double, ByVal rho_cos_phi As Double, ByVal rho_sin_phi As Double, ByRef vect As Double)

    Public Declare Sub _parse_elements Lib "SGP4class.dll" _
        Alias "?parse_elements_cpp@SGP4class@SGP4space@@SGXPAHPBD1PAUtle_t@@@Z" _
        (ByRef tlechk As Integer, <MarshalAs(UnmanagedType.LPStr)> ByVal line1 As String, <MarshalAs(UnmanagedType.LPStr)> ByVal line2 As String, _
         <MarshalAs(UnmanagedType.Struct)> ByRef sat As tle_t)

    'Public Declare Function _sxpx_library_version Lib "SGP4class.dll" Alias "_sxpx_library_version@0" () As Integer
    Public Declare Function _sxpx_library_version Lib "SGP4class.dll" Alias "#18" () As Integer

    Public Declare Function _SGPX Lib "SGP4class.dll" Alias "?SGPX@SGP4class@SGP4space@@SGHNPBUtle_t@@PBNPAN2H@Z" _
        (ByVal tsince As Double, <MarshalAs(UnmanagedType.Struct)> ByRef tle As tle_t, ByRef params As Double, _
         ByRef pos As Double, ByRef vel As Double, ByVal Model As Integer) As Integer

    Public Declare Sub _SGPX_init Lib "SGP4class.dll" Alias "?SGPX_init@SGP4class@SGP4space@@SGXPANPBUtle_t@@H@Z" _
        (ByRef params As Double, <MarshalAs(UnmanagedType.Struct)> ByRef tle As tle_t, ByVal Model As Integer)
    '(ByRef params As Double, <MarshalAs(UnmanagedType.Struct)> ByRef tle As tle_t, <MarshalAs(UnmanagedType.LPStr)> ByVal Model As String)

    'Public Declare Function _tle_test Lib "SGP4class.dll" Alias "?tle_test@SGP4class@SGP4space@@SGHPANPAUtle_t@@PAPAD@Z" _
    '    (ByRef params As Double, <MarshalAs(UnmanagedType.Struct)> ByRef tle As tle_t, <MarshalAs(UnmanagedType.LPStr)> ByVal Model As String) As Integer

    Public Declare Sub _get_satellite_ra_dec_delta Lib "SGP4class.dll" Alias "_get_satellite_ra_dec_delta@20" _
        (ByVal observer_loc() As Double, ByVal satellite_loc() As Double, ByRef ra As Double, ByRef dec As Double, ByRef delta As Double)

    Public Declare Sub _epoch_of_date_to_j2000 Lib "SGP4class.dll" Alias "_epoch_of_date_to_j2000@16" _
        (ByVal jd As Double, ByRef ra As Double, ByRef dec As Double)

    'moved from c dll to vb AstroVB
    'Public Declare Function _julday Lib "user_dll_pathSGP4class.dll" Alias "_julday@12" _
    '   (ByVal year As Integer, ByVal month As Integer, ByVal day As Integer) As Double
    'year, month, day

    '<DllImport("user_dll_pathSGP4class.dll", CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.Cdecl)> _
    'Public Shared Function _tle_test Alias "?tle_test@SGP4@SGP4space@@SGHPANPAUtle_t@@PAD@Z" (ByRef params As Double, <MarshalAs(UnmanagedType.Struct)> ByRef tle As tle_t, <MarshalAs(UnmanagedType.LPStr)> ByVal Model As String) As Integer
    'Public Shared Function tle_test(ByVal resultBreakDown As IntPtr, <MarshalAs(UnmanagedType.LPStr)> ByVal szFilename As StringBuilder)
    'End Function

    ' this works but there isn't any real advantage to this calling convention AFAIK (using the ordinal as entrypoint does though!)
    'E:\Source\Satellite\VisualBasic\SeeSatVB\Debug\
    ', CharSet:=CharSet.Unicode, CallingConvention:=CallingConvention.Cdecl _
    '<DllImport("SGP4class.dll", EntryPoint:="?tle_test@SGP4class@SGP4space@@SGHPANPAUtle_t@@H@Z")> _
    <DllImport("SGP4class.dll", EntryPoint:="#5")> _
    Public Shared Function tle_test _
        (ByRef params As Double, <MarshalAs(UnmanagedType.Struct)> ByRef tle As tle_t, ByVal Model As Integer) As Integer
    End Function

    Public Shared SatNdx As Integer = 0   ' Last satellite in array sats
    Public Shared satellites(SatNdx) As sat_t

    Public SXXP() As String = {"", "SGP4", "SGP8", "SDP4", "SDP8"}

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'initialize the user location data from user application settings
        my_loc.ht_in_meters = My.Settings.user_ht_in_meters
        my_loc.lat_deg = My.Settings.user_lat
        my_loc.lat2rad()    '= My.Settings.user_lat * DefConst.DE2RA
        my_loc.lon_deg = My.Settings.user_lon
        my_loc.lon2rad()    '= My.Settings.user_lon * DefConst.DE2RA
        my_loc.loc_name = My.Settings.user_loc_name
        my_loc.run_date = My.Settings.user_rundate
        my_loc.tz_offset = My.Settings.user_tz

        'initialize the file paths from user application settings
        my_files.mcnames_path = My.Settings.user_mcnames_path
        my_files.qsat_path = My.Settings.user_qsat_path
        my_files.star_path = My.Settings.user_star_path
        my_files.tle_path = My.Settings.user_tle_path

        'initialize the program parameters 
        ' - refresh timer intervals
        my_params.sat_time_int = My.Settings.user_sat_int
        my_params.star_time_int = My.Settings.user_star_int

        'visual settings
        my_params.view_stereo = My.Settings.user_view_stereo

    End Sub

    ' save the application settings back to the application user defaults
    Public Sub save_user_defaults()

        'save the location defaults
        My.Settings.user_ht_in_meters = my_loc.ht_in_meters
        My.Settings.user_lat = my_loc.lat_deg
        My.Settings.user_lon = my_loc.lon_deg
        My.Settings.user_loc_name = my_loc.loc_name
        My.Settings.user_rundate = my_loc.run_date
        My.Settings.user_tz = my_loc.tz_offset

        'save the file paths from user application settings
        My.Settings.user_mcnames_path = my_files.mcnames_path
        My.Settings.user_qsat_path = my_files.qsat_path
        My.Settings.user_star_path = my_files.star_path
        My.Settings.user_tle_path = my_files.tle_path

        'save the user defined program parameters
        My.Settings.user_star_int = my_params.star_time_int
        My.Settings.user_sat_int = my_params.sat_time_int

        'visual settings
        My.Settings.user_view_stereo = my_params.view_stereo

        My.Settings.Save()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim version As Double
        Dim outs As String

        version = _sxpx_library_version()
        outs = CStr(version)

        TextBox1.AppendText("library version returned " + outs)

        'chk_rval(SatIO.ReadTLE(my_files.tle_path), my_files.tle_path)
        'tle_test(satellites(1).sat_params(0), satellites(1).tle, CInt(version))

    End Sub

    Private Function new_sat_element(ByRef SatArray() As sat_t) As Integer
        Dim ndx As Integer

        ndx = SatArray.Length       ' always one more than actual number of elements

        Dim sat = New sat_t
        sat.tle.intl_desig = Space(DefConst.INTL_SIZE)
        ReDim sat.sat_params(DefConst.N_SAT_PARAMS)
        'ReDim sat.sgp_sat_params(N_SAT_PARAMS)
        ReDim sat.pos(DefConst.N_POS_SIZE)
        ReDim sat.vel(DefConst.N_POS_SIZE)
        sat.is_deep = 0
        sat.Model = DefConst.SGP4M
        sat.tag = 0
        sat.is_valid = False
        sat.view = New satxyz_t

        ReDim Preserve SatArray(ndx)

        SatArray(ndx) = sat
        new_sat_element = ndx

    End Function

    Public Function find_or_create_sat_element(intl_desig As String) As Integer
        ' locate intl_desig or create an new sat element
        Dim ndx As Integer = 0
        Dim rval As Integer = 0

        For Each sat In satellites
            If (sat.tle.intl_desig = intl_desig) Or (sat.is_valid = False) Then
                rval = ndx
                Exit For
            End If
            ndx += 1
        Next

        If rval = 0 Then
            rval = new_sat_element(satellites)
        End If

        find_or_create_sat_element = rval

    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'TestSat(108)    'index of ISS
        'TestStar() ' calls readstar

        If REALTIME = False Then
            InitRealTime()
            REALTIME = True
            TimerR.Interval = my_params.sat_time_int
            TimerR.Enabled = True

            If SatWindow.SHOWSTARS Then
                TimerS.Interval = my_params.star_time_int
                TimerS.Enabled = True
            End If

            Button2.Text = "Stop Test"

        Else
            REALTIME = False
            TimerR.Enabled = False
            TimerS.Enabled = False

            Button2.Text = "Test"

        End If

    End Sub


    Private Sub TestStar()

        'Dim lat, lon, ht_in_meters, tz As Double
        Dim testDate As Date
        Dim tz_offset, jd, tz As Double     ' time zone offset in minutes returned by topos - 0 if all we do is in UTC/GMT

        Dim ndx As Integer

        'lat = 54.0
        'lon = -124.0
        'ht_in_meters = 640
        'testDate = #2/14/2015 2:40:00 AM#   ' UTC - set tz to 0
        'testDate = #2/13/2015 6:40:00 PM#   ' PST - set tz to -8
        'testDate = #2/14/2015 5:40:00 PM#   ' PST - set tz to -8
        tz = my_loc.tz_offset
        testDate = my_loc.run_date

        jd = Date2Julian(testDate)

        ' todo - the routines in xyztop have to be called in order to initialize some of the globals - cleanup!!

        tz_offset = AstroGR.topos(my_loc.ht_in_meters, my_loc.lat_deg, my_loc.lon_deg, tz) / DefConst.MINPERDAY
        jd = jd - tz_offset

        'SatIO.ReadStars("E:\Source\Satellite\Data\teststar.csv")
        SatIO.ReadStars("E:\Source\Satellite\Data\hipstarcat.csv")
        'SatIO.ReadStars("E:\Source\Satellite\Data\orion.csv")

        TextBox1.Text = "Starry sky at " + CStr(testDate) + vbNewLine + "Alt, Azm, Mag" + vbNewLine

        AstroGR.initstar()
        SatWindow.initStarD()

        'Dim starxy As New star_xy

        'For ndx = 1 To SatIO.stars.Length - 1
        '    AstroGR.calcstar(ndx, starxy)
        '    TextBox1.AppendText(CStr(starxy.lambda * DefConst.RA2DE) + ", " + CStr(starxy.phi * DefConst.RA2DE) + ", " + CStr(starxy.mag) + ", " + vbNewLine)
        'Next

    End Sub

    ' initialize the realtime mode
    Public Sub InitRealTime()
        Dim ephem As Integer = 1
        Dim iflag2 As Integer = 1

        'Dim sndx, rval As Integer      'counters and flags

        Dim tz_offset, tz As Double     ' time zone offset in minutes returned by topos - 0 if all we do is in UTC/GMT
        Dim testDate As Date

        Dim observer_loc(3) As Double
        'Dim rho_sin_phi, rho_cos_phi, ra, dec, dist_to_satellite, t_since As Double
        Dim rho_sin_phi, rho_cos_phi As Double

        ' we will work in universal time
        tz = 0
        testDate = DateTime.UtcNow

        JDPUB = Date2Julian(testDate) + JDOFFSET

        ' does some initializing in AstroVB
        tz_offset = AstroGR.topos(my_loc.ht_in_meters, my_loc.lat_deg, my_loc.lon_deg, tz) / DefConst.MINPERDAY

        If SatNdx <= 1 Then      'we need to read in the files
            ReadAllFiles()
        End If

        ' set the display buffer to 10 positions
        SatWindow.sizeofSatI = 10
        ' and initialize the display list
        SatWindow.initSatsD()

        ' initialize the dll parameters
        _lat_alt_to_parallax(my_loc.lat_rad, my_loc.ht_in_meters, rho_cos_phi, rho_sin_phi)
        _observer_cartesian_coords(JDPUB, my_loc.lon_rad, rho_cos_phi, rho_sin_phi, observer_loc(0))
        ' have to call this once before initstar
        'AstroGR.xyztop(1, JDPUB * DefConst.MINPERDAY, 1)

        If SatWindow.SHOWSTARS Then
            AstroGR.initstar()
            If SatWindow.initStarD() = 0 Then
                SatWindow.SHOWSTARS = False
            End If
        End If

    End Sub

    ' show all of the satellites in a realtime display
    Private Sub RealTimeDisplay()

        Dim ephem As Integer = 1
        Dim iflag2 As Integer = 1
        Dim counterR As Integer = 1     ' counter and interval to refresh observer coordinates
        Dim intervalR As Integer = 300  ' timerR is set to once a second so about 5 minutes

        Dim sndx, rval As Integer      'counters and flags

        'Dim tz_offset, tz, jd As Double     ' time zone offset in minutes returned by topos - 0 if all we do is in UTC/GMT
        'Dim JDPUB As Double            ' shared public
        'Dim testDate As Date

        Dim observer_loc(3) As Double
        Dim rho_sin_phi, rho_cos_phi, t_since As Double

        TimerR.Enabled = False  ' don't interrupt ourselves

        ' todo - implement the logic that tags sat below the horizon with a counter flag so they are not calculated each time

        For sndx = 1 To SatNdx  'go once through the list

            If satellites(sndx).tag < 0 Then    ' it has been flagged as below the horizon
                satellites(sndx).tag += 1
                Continue For
            End If

            JDPUB = Date2Julian(DateTime.UtcNow) + JDOFFSET

            ' not sure if the next line is required
            _lat_alt_to_parallax(my_loc.lat_rad, my_loc.ht_in_meters, rho_cos_phi, rho_sin_phi)
            _observer_cartesian_coords(JDPUB, my_loc.lon_rad, rho_cos_phi, rho_sin_phi, observer_loc(0))

            t_since = (JDPUB - satellites(sndx).tle.epoch) * DefConst.MINPERDAY

            _SGPX(t_since, satellites(sndx).tle, satellites(sndx).sat_params(0), _
                  satellites(sndx).pos(0), satellites(sndx).vel(0), satellites(sndx).Model)

            ' compute positions as per AstroVB
            ' transfer the position values - each time the satellite changes or change astrovb to look at the global vars 
            AstroGR.set_sat_xyz(satellites(sndx).pos)
            ' set the precession vectors for this satellite - again each time the satellite changes
            AstroGR.inpre(satellites(sndx).tle.epoch * DefConst.MINPERDAY)
            ' compute a bunch of stuff - reset iflag to 1 if satellite changes
            ' rval can be 0 - sat below horizon, 1 - sat should be visable, 2 - sat more than 2 earth radii away
            iflag2 = 1
            rval = AstroGR.xyztop(iflag2, JDPUB * DefConst.MINPERDAY, sndx)

            Select Case rval
                Case Is = 0     ' sat is below horizon
                    satellites(sndx).tag = CInt(satellites(sndx).view.azel.phi * DefConst.RA2DE)    ' catch the case where we have just added more TLE
                    If SatWindow.SatsD.Length > sndx AndAlso SatWindow.SatsD(sndx).isActive Then  ' it just slipped below the horizon
                        SatWindow.SatsD(sndx).isActive = False
                    End If
                Case Is = 1     ' sat is within AstroVB.MAXDIST earth radii (nominally 2)
                    SatWindow.plotazel(sndx, satellites(sndx).view.azel, satellites(sndx).view.truemag, satellites(sndx).view.elsusa)
                Case Is = 2     ' sat is further than AstroVB.MAXDIST earth radii (nominally 2)
                    ' plot it anyway - put some selective logic in here later
                    SatWindow.plotazel(sndx, satellites(sndx).view.azel, satellites(sndx).view.truemag, satellites(sndx).view.elsusa)
            End Select

            counterR += 1
            If counterR = intervalR Then
                counterR = 1
                ' do stuff every 300 ticks
            End If

            ' to fix a bug where the stars are out of place until xyztop has been called once - ll
            If SatWindow.SHOWSTARS And AstroGR.FIRSTIME Then
                AstroGR.initstar()
                If SatWindow.initStarD() = 0 Then
                    SatWindow.SHOWSTARS = False
                End If
                AstroGR.FIRSTIME = False
            End If
            'Application.DoEvents()
            'sndx = SatWindow.CListNext(sndx, SatNdx) 'next element in the circular list

        Next

        TimerR.Enabled = True

    End Sub


    ' pass the satellite ndx
    Private Sub TestSat(satid As Integer)
        ' if satid is -1 then go into realtime mode with all satellites

        Dim posn2(3), delta, d2 As Double

        Dim t0 As Date

        Dim ephem As Integer = 1
        Dim iflag2 As Integer = 1

        Dim tz_offset, tz As Double     ' time zone offset in minutes returned by topos - 0 if all we do is in UTC/GMT

        'Dim lat, lon, ht_in_meters, jd, tz As Double
        Dim jd As Double

        Dim testDate As Date

        Dim i, j, N_RUNS, n_found, tlechk As Integer

        Dim observer_loc(3) As Double
        Dim rho_sin_phi, rho_cos_phi, ra, dec, dist_to_satellite, t_since As Double

        'Dim realtime As Boolean = False

        N_RUNS = 100
        n_found = 0

        ' toggle to switch between test cases and actual
        If satid = -1 Then
            REALTIME = True
            SatWindow.sizeofSatI = 10
        Else
            REALTIME = False
            SatWindow.sizeofSatI = 100
        End If
        SLEEPTIME = 2000    ' milisec
        Dim verbose As Boolean = False
        Dim mStep As Double = 10 / 60 ' the step in minutes between calls in non realtime

        If REALTIME = True Then
            'lat = my_loc.lat_deg
            'lon = my_loc.lon_deg
            'ht_in_meters = my_loc.ht_in_meters
            tz = 0
            testDate = DateTime.UtcNow
        Else
            'lat = 44.01
            'lon = -69.9
            'ht_in_meters = 100
            'testDate = #9/24/2002#
            'tz = -5
            'testDate = #2/14/2015 2:40:00 AM#   ' UTC - set tz to 0
            'testDate = #2/14/2015 5:40:00 PM#   ' PST - set tz to -8
            testDate = my_loc.run_date   ' PST - set tz to -8
            tz = my_loc.tz_offset
        End If

        jd = Date2Julian(testDate)

        tz_offset = AstroGR.topos(my_loc.ht_in_meters, my_loc.lat_deg, my_loc.lon_deg, tz) / DefConst.MINPERDAY

        If REALTIME = False Then
            jd = jd - tz_offset
        End If

        'satid = new_sat_element(satellites)

        ' call to SatWindow and initalize the display array (normally it would add after reading all of the TLE files)
        SatWindow.initSatsD()

        '_parse_elements(tlechk, tle1, tle2, satellites(satid).tle)

        'Select Case tlechk
        '    Case Is = 0     'if the elements are parsed without error
        '        TextBox1.Text = "Parsed TLE" + vbNewLine
        '    Case Is = 1     'if they're OK except the first line has a checksum error;
        '        TextBox1.Text = "line 1 checksum error in TLE - continuing" + vbNewLine
        '    Case Is = 2     'if they're OK except the second line has a checksum error;
        '        TextBox1.Text = "line 2 checksum error in TLE - continuing" + vbNewLine
        '    Case Is = 3     'if they're OK except both lines have checksum errors;
        '        TextBox1.Text = "line 1 & 2 checksum error in TLE - continuing" + vbNewLine
        '    Case Is < 0     'a negative value if the lines aren't at all parseable */
        '        TextBox1.Text = "TLE not parseable - aborting" + vbNewLine
        '        Exit Sub
        'End Select

        '_select_ephemeris(satellites(satid).is_deep, satellites(satid).tle)

        'Select Case satellites(satid).is_deep
        '    Case Is = 0  ' is SGP4 model
        '        satellites(satid).Model = DefConst.SGP4M
        '    Case Is = 1  ' is SDP4 model
        '        satellites(satid).Model = DefConst.SDP4M
        '    Case Is = -1 ' error in tle or function
        '        TextBox1.Text = "_select_ephemeris found an error in TLE" + vbNewLine
        '        Exit Sub
        '    Case Else
        '        TextBox1.Text = "_select_ephemeris returned an unknown value " + CStr(satellites(satid).is_deep) + vbNewLine
        '        Exit Sub
        'End Select

        '_SGPX_init(satellites(satid).sat_params(0), satellites(satid).tle, satellites(satid).Model) ' initialize model

        ' JUST TESTING!
        ' normal call would look like
        SatWindow.addSatsE(satid)


        'foo = satellites(satid).tle
        'i = (Marshal.SizeOf(foo))

        'parse elements is breaking the tle structure somehow
        '_parse_elements(tlechk, tle1, tle2, foo)
        'satellites(satid).tle = foo
        'i = (Marshal.SizeOf(foo))
        '_parse_elements(tlechk, tle1, tle2, satellites(satid).tle)

        '_select_ephemeris(is_deep, satellites(satid).tle)

        'i = _select_ephemeris(satellites(satid).tle)

        _lat_alt_to_parallax(my_loc.lat_rad, my_loc.ht_in_meters, rho_cos_phi, rho_sin_phi)


        'perigee = satellites(satid).sgp_sat_params(9) * (1.0 - satellites(satid).tle.eo) - 1.0

        'TextBox1.Text = ""
        TextBox1.Text = "Now (UTC):" + CStr(testDate)
        TextBox1.AppendText("Julian Date in minutes:" + CStr(jd) + vbNewLine)

        'TextBox1.appendText ("JD back to Greg: " + CStr(AstroGR.Julian2Gregorian(jd + tz_offset)) + vbNewLine)

        _observer_cartesian_coords(jd, my_loc.lon_rad, rho_cos_phi, rho_sin_phi, observer_loc(0))


        For j = 0 To N_RUNS Step 1

            If REALTIME = True Then
                jd = Date2Julian(DateTime.UtcNow)
            Else
                jd = jd + mStep / DefConst.MINPERDAY
            End If

            _observer_cartesian_coords(jd, my_loc.lon_rad, rho_cos_phi, rho_sin_phi, observer_loc(0))

            t_since = (jd - satellites(satid).tle.epoch) * DefConst.MINPERDAY

            _SGPX(t_since, satellites(satid).tle, satellites(satid).sat_params(0), _
                  satellites(satid).pos(0), satellites(satid).vel(0), satellites(satid).Model)

            TextBox1.AppendText("Model " + SXXP(satellites(satid).Model) + " Object " _
                + CStr(satellites(satid).tle.norad_number) + " as seen from lat " _
                + CStr(my_loc.lat_deg) + " lon " + CStr(my_loc.lon_deg) + " JD " + CStr(Math.Round(jd, 6)) _
                + " Date " + CStr(AstroGR.Julian2Gregorian(jd + tz_offset)) + vbNewLine)

            ' compute positions as per AstroVB
            ' transfer the position values - each time the satellite changes or change astrovb to look at the global vars 
            AstroGR.set_sat_xyz(satellites(satid).pos)
            ' set the precession vectors for this satellite - again each time the satellite changes
            AstroGR.inpre(satellites(satid).tle.epoch * DefConst.MINPERDAY)
            ' compute a bunch of stuff - reset iflag to 1 if satellite changes
            AstroGR.xyztop(iflag2, jd * DefConst.MINPERDAY, satid)

            If verbose = True Then
                TextBox1.AppendText("AstroVB  RA " + CStr(satellites(satid).view.radec.lambda * DefConst.RA2DE) + _
                    " (J2000) dec " + CStr(satellites(satid).view.radec.phi * DefConst.RA2DE) + _
                    " dist " + CStr(satellites(satid).view.radec.r * DefConst.EARTHR2KM) + " km" + vbNewLine)
                TextBox1.AppendText("AstroVB lat " + CStr(satellites(satid).view.latlon.lambda * DefConst.RA2DE) + _
                    " (J2000) lon " + CStr(satellites(satid).view.latlon.phi * DefConst.RA2DE) + _
                    " elev " + CStr(satellites(satid).view.latlon.r * DefConst.EARTHR2KM) + " km" + vbNewLine)
            End If

            ' for some reason the sign on alt is reversed

            TextBox1.AppendText("AstroVB alt " + CStr(Math.Round(satellites(satid).view.azel.phi * DefConst.RA2DE, 4)) + _
                " (J2000) azm " + CStr(Math.Round(satellites(satid).view.azel.lambda * DefConst.RA2DE, 4)) + _
                " dist " + CStr(Math.Round(satellites(satid).view.azel.r * DefConst.EARTHR2KM, 4)) + " km" + vbNewLine)
            TextBox1.AppendText("AstroVB sun< " + CStr(satellites(satid).view.elsusa) + " %illum " + _
                CStr(satellites(satid).view.illum) + " true mag " + CStr(satellites(satid).view.truemag) + vbNewLine)


            'TextBox1.AppendText("AstroVB alt " + CStr(Math.Round(satellites(satid).view.azel.phi * DefConst.RA2DE, 4)) + _
            '    " (J2000) azm " + CStr(Math.Round(satellites(satid).view.azel.lambda * DefConst.RA2DE, 4)) + _
            '    " dist " + CStr(Math.Round(satellites(satid).view.azel.r * DefConst.EARTHR2KM, 4)) + " km" + vbNewLine)
            'TextBox1.AppendText("AstroVB sun< " + CStr(satellites(satid).view.elsusa) + " %illum " + _
            'CStr(satellites(satid).view.illumination()) + " true mag " + CStr(satellites(satid).view.true_mag()) + vbNewLine)

            'TimerS.Enabled = True
            If satellites(satid).view.azel.phi >= 0 Then
                SatWindow.plotazel(satid, satellites(satid).view.azel, satellites(satid).view.truemag, satellites(satid).view.elsusa)
            End If

            Application.DoEvents()


            'TextBox1.appendText ("AstroVB  RA " + CStr(satellites(satid).view.radec.r * 180 / DefConst.PI) + _
            '    " (J2000) dec " + CStr(satellites(satid).view.radec.phi * 180 / DefConst.PI) + _
            '    " dist " + CStr(satellites(satid).view.radec.lambda * EARTHR2KM) + " km" + vbNewLine)

            ' same using routines in DLL
            _get_satellite_ra_dec_delta(observer_loc, satellites(satid).pos, ra, dec, dist_to_satellite)
            _epoch_of_date_to_j2000(jd, ra, dec)

            If verbose = True Then
                TextBox1.AppendText("SGP4.dll RA " + CStr(ra * DefConst.RA2DE) + " (J2000) dec " + CStr(dec * DefConst.RA2DE) + _
                    " dist " + CStr(dist_to_satellite) + " km" + vbNewLine)
            End If

            If REALTIME = True Then
                'TextBox1.Update()
                'TextBox1.Select(TextBox1.TextLength, 0)
                'TextBox1.ScrollToCaret()
                System.Threading.Thread.Sleep(SLEEPTIME)
                'TextBox1.Text = ""
            End If
        Next

    End Sub

    Public Function Date2Julian(ByVal vDate As DateTime) As Double
        ' calls _julday which returns the julian day in fractional days
        ' returns julian day with the time factor added in

        Dim jday As Double

        'jday = _julday(DatePart(DateInterval.Year, vDate), DatePart(DateInterval.Month, vDate), DatePart(DateInterval.Day, vDate)) _
        '    + DatePart(DateInterval.Hour, vDate) / 24 + DatePart(DateInterval.Minute, vDate) / MINPERDAY + DatePart(DateInterval.Second, vDate) / (60 * MINPERDAY)
        jday = AstroGR.julday(DatePart(DateInterval.Year, vDate), DatePart(DateInterval.Month, vDate), DatePart(DateInterval.Day, vDate)) _
            + DatePart(DateInterval.Hour, vDate) / 24 + DatePart(DateInterval.Minute, vDate) / DefConst.MINPERDAY + DatePart(DateInterval.Second, vDate) / (60 * DefConst.MINPERDAY)

        Return jday  ' julian dateTime in fractional days

    End Function

    Private Sub Testit()

        Dim ndx As Integer
        Dim arr(DefConst.N_SAT_PARAMS) As Double
        Dim retval As Integer

        ndx = new_sat_element(satellites)

        TextBox1.Text = "Created new satellite " + CStr(ndx) + vbNewLine


        '_parse_elements(tle1, tle2, satellites(ndx).tle)

        _SGPX_init(satellites(ndx).sat_params(0), satellites(ndx).tle, 1)
        '_SGPX_init(satellites(ndx).tle)
        '_SGP4_init(arr(0), satellites(ndx).tle)

        'TextBox1.Text = "Testing Add dll function 2+5= " + CStr(_Add(2.0, 5.0))

        arr(0) = 2
        arr(1) = 5

        'TextBox1.Text = "Testing AddArrary dll function 2+5= " + CStr(_AddArray(arr(0)))

        'retval = _tle_test(satellites(ndx).sdp_sat_params(0), satellites(ndx).tle, "SGP4")

        TextBox1.Text = "_SGPXinit returned " + CStr(retval) + vbNewLine + _
            "and changed intl_desig to model parameter " + satellites(ndx).tle.intl_desig
    End Sub

    Private Sub SeeSatVBmain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ShowSky_Click(sender As Object, e As EventArgs) Handles ShowSky.Click

        Dim frmCollection = System.Windows.Forms.Application.OpenForms
        If frmCollection.OfType(Of SatWindow).Any Then
            frmCollection.Item("SatWindow").Activate()
        Else
            Dim SkyView As New SatWindow
            SkyView.Show()
        End If


    End Sub

    ' Menu file dialogs
    ' set TLE file name
    Private Sub TLEFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TLEFileToolStripMenuItem.Click
        Dim OFD As New OpenFileDialog

        OFD.InitialDirectory = System.IO.Path.GetDirectoryName(my_files.tle_path)
        OFD.CheckFileExists = True
        OFD.RestoreDirectory = True
        OFD.Title = "Open TLE file"
        OFD.CheckPathExists = True
        OFD.DefaultExt = "tle"
        OFD.Filter = "TLE files (*.tle, *.txt)|*.tle; *.txt|All files (*.*)|*.*"
        Dim active As Boolean = False

        If TimerR.Enabled Then
            TimerR.Enabled = False  ' we don't wish to be interrupted if running realtime
            active = True
        End If

        Dim result As DialogResult = OFD.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then
            Dim rval As New Integer
            rval = SatIO.ReadTLE(OFD.FileName)
            Select Case rval
                Case Is > 0
                    TextBox1.AppendText("ReadTLE read " + CStr(rval) + " TLE elements." + vbNewLine)
                    my_files.tle_path = OFD.FileName
                    'SatWindow.initSatsD()   ' reinitialize the display list
                Case Is = 0
                    TextBox1.AppendText("ReadTLE did not add any TLE elements." + vbNewLine)
                Case Is = -1
                    TextBox1.AppendText("Error reading TLE elements in ReadTLE." + vbNewLine)
            End Select

        End If

        If active Then
            TimerR.Enabled = True
        End If

        OFD.Dispose()

    End Sub

    ' set McCants mag file name
    Private Sub McCantsMagFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles McCantsMagFileToolStripMenuItem.Click
        Dim OFD As New OpenFileDialog

        OFD.InitialDirectory = System.IO.Path.GetDirectoryName(my_files.mcnames_path)
        OFD.CheckFileExists = True
        OFD.RestoreDirectory = True
        OFD.Title = "Open McNames file"
        OFD.CheckPathExists = True
        OFD.DefaultExt = "txt"
        OFD.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*"

        Dim result As DialogResult = OFD.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then
            Dim rval As New Integer
            rval = SatIO.ReadMcname(OFD.FileName)
            Select Case rval
                Case Is > 0
                    TextBox1.AppendText("ReadMcname read " + CStr(rval) + " Visual elements." + vbNewLine)
                    my_files.mcnames_path = OFD.FileName
                Case Is = 0
                    TextBox1.AppendText("ReadMcname did not add any Visual elements." + vbNewLine)
                Case Is = -1
                    TextBox1.AppendText("Error reading Visual elements in ReadMcname." + vbNewLine)
            End Select

        End If

        OFD.Dispose()
    End Sub

    ' set visual mag file name
    Private Sub VisualMagFileToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles VisualMagFileToolStripMenuItem1.Click
        Dim OFD As New OpenFileDialog

        OFD.InitialDirectory = System.IO.Path.GetDirectoryName(my_files.qsat_path)
        OFD.CheckFileExists = True
        OFD.RestoreDirectory = True
        OFD.Title = "Open QuickSat file"
        OFD.CheckPathExists = True
        OFD.DefaultExt = "txt"
        OFD.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*"

        Dim result As DialogResult = OFD.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then
            Dim rval As New Integer
            rval = SatIO.ReadQsat(OFD.FileName)
            Select Case rval
                Case Is > 0
                    TextBox1.AppendText("ReadQsat read " + CStr(rval) + " Visual elements." + vbNewLine)
                    my_files.qsat_path = OFD.FileName
                Case Is = 0
                    TextBox1.AppendText("ReadQsat did not add any Visual elements." + vbNewLine)
                Case Is = -1
                    TextBox1.AppendText("Error reading Visual elements in ReadQsat." + vbNewLine)
            End Select

        End If

        OFD.Dispose()
    End Sub

    ' set star data file name
    Private Sub StarDataFileToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles StarDataFileToolStripMenuItem1.Click
        Dim OFD As New OpenFileDialog

        OFD.InitialDirectory = System.IO.Path.GetDirectoryName(my_files.star_path)
        OFD.CheckFileExists = True
        OFD.RestoreDirectory = True
        OFD.Title = "Open Star Data file"
        OFD.CheckPathExists = True
        OFD.DefaultExt = "csv"
        OFD.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"

        Dim result As DialogResult = OFD.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then
            Dim rval As New Integer
            rval = SatIO.ReadStars(OFD.FileName)
            Select Case rval
                Case Is > 1
                    TextBox1.AppendText("ReadStars read " + CStr(rval) + " star elements." + vbNewLine)
                    my_files.star_path = OFD.FileName
                Case Is = 0
                    TextBox1.AppendText("ReadStars did not add any Star elements." + vbNewLine)
                Case Is = -1
                    TextBox1.AppendText("Error reading Star elements in ReadStars." + vbNewLine)
            End Select

        End If

        OFD.Dispose()
    End Sub

    'load in all of the user default files in the proper order
    Private Sub AllDefaultFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllDefaultFilesToolStripMenuItem.Click

        ReadAllFiles()

    End Sub

    Private Sub ReadAllFiles()

        ' do some checking on the return values for rval
        chk_rval(SatIO.ReadStars(my_files.star_path), my_files.star_path)
        chk_rval(SatIO.ReadQsat(my_files.qsat_path), my_files.qsat_path)
        chk_rval(SatIO.ReadTLE(my_files.tle_path), my_files.tle_path)
        chk_rval(SatIO.ReadStars(my_files.star_path), my_files.star_path)

    End Sub

    'check the return value from a file operation and display a message box if error
    Private Sub chk_rval(rval As Integer, operation As String)

        Select Case rval
            Case Is = -1
                MessageBox.Show("Error opening or reading file " + operation + " no data returned.")
            Case 0, 1
                MessageBox.Show("Error reading file " + operation + " no data returned.")
            Case Else   ' all is well don't do anything

        End Select

    End Sub

    Private Sub SaveDefaultsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveDefaultsToolStripMenuItem.Click
        ' save all defaults
        save_user_defaults()

    End Sub


    Private Sub UserLocationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserLocationToolStripMenuItem.Click
        'open the user location dialog
        Dim frmCollection = System.Windows.Forms.Application.OpenForms
        If frmCollection.OfType(Of UserSettings).Any Then
            frmCollection.Item("UserSettings").Activate()
        Else
            Dim UserSet As New UserSettings
            UserSet.ShowDialog(Me)
        End If

    End Sub

    ' realtime timer - on each tick calls the realtimedisplay routine
    Private Sub TimerR_Tick(sender As Object, e As EventArgs) Handles TimerR.Tick
        'TimerR.Enabled = False
        RealTimeDisplay()

    End Sub

    Private Sub TimerS_Tick(sender As Object, e As EventArgs) Handles TimerS.Tick

        If SatWindow.SHOWSTARS Then
            'AstroGR.initstar()
            SatWindow.initStarD()
        End If
        RealTimeDisplay()

    End Sub

End Class


'the same structure as used in astro.ccp
' data for observer's location 
' had to move this class to the end to enable designer

Public Class obs_t
    Public r As Double      'altitude above sea level
    Public lambda As Double 'longitude radians
    Public sinlat As Double 'latitude sin & cos
    Public coslat As Double
    Public zg As Double     'north distance from observatory to equatorial plane
    Public xc As Double     'distance between observatory & polar axis of earth 
    Public Function Clone() As obs_t
        Return DirectCast(Me.MemberwiseClone(), obs_t)
    End Function
End Class

' this structure holds information about the observer location
' initialized from application user settings
Public Class obs_input
    Public ht_in_meters As Double   'altitude above sea level in meters
    Public lat_deg As Double        'latitude in decimal degrees
    Public lon_deg As Double        'longitude in decimal degrees
    Public lat_rad As Double        'latitude in decimal radians
    Public lon_rad As Double        'longitude in decimal radians
    Public loc_name As String       'the name of the location
    Public tz_offset As Double      'users time zone
    Public run_date As Date         'run date for predict mode
    Public Sub lat2rad()
        Me.lat_rad = Me.lat_deg * DefConst.DE2RA
    End Sub
    Public Sub lon2rad()
        Me.lon_rad = Me.lon_deg * DefConst.DE2RA
    End Sub
End Class

Public Class file_names
    Public mcnames_path As String   'the magnitude data in McCants format
    Public qsat_path As String      'the magnitude data in QuickSat format
    Public star_path As String      'the CSV file holding RA,DEC,MAG star data
    Public tle_path As String       'the TLE data for satellite 
End Class

Public Class prog_params
    Public star_time_int As Integer 'update interval for the star timer
    Public sat_time_int As Integer  'update interval for the satellite timer
    Public view_stereo As Boolean   'use a sterographic projection
End Class
