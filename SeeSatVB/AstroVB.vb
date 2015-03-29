Imports Microsoft.VisualBasic
Imports System

Public Class AstroGR

#Const POW = True
    'extern double pow();

    'extern double dint();

    ' dint(3.6) == 3.0, dint(-3.6) == -3.0.
    'If your library has floor() and ceil() instead of dint(), retain the
    'preceding dint() declaration and un-comment the following code: 

    'double dint(x)
    'double x;
    '{
    '    if (x >= 0) return floor(x);
    '    return ceil(x);
    '}
    '#endif
    '

    '####################################################################
    '                Data
    '####################################################################

    ' The satellite's mean orbital elements.  These are read from the
    'orbital element file, and remain constant during program execution. 
    ' these are not used at all
    Public Shared xmo As Double ' mean anomaly
    Public Shared xnodeo As Double ' right ascension of ascending node
    Public Shared omegao As Double ' argument of the perigee
    Public Shared eo As Double ' eccentricity
    Public Shared xincl As Double ' inclination
    Public Shared xno As Double '   mean motion, radians/min
    Public Shared xndt2o As Double ' 1st time derivative of mean motion, or ballistic coefficient (depending on ephemeris type)
    Public Shared xndd6o As Double ' 2nd time derivative of mean motion
    Public Shared bstar As Double ' BSTAR drag term if GP4 theory was used otherwise, radiation pressure coefficient 
    Public Shared epoch As Double ' epoch of mean elements

    Public Shared decmax As Double ' maximum declination for stars routines

    Public Shared name As New String(New Char(12) {}) ' of satellite - also from element file

    'Public Shared mgntd As Integer ' magnitude of sat

    ' Set by thetag(), used only in thetag() & deep space initialization. 
    Public Shared ds50 As Double

    ' satellite geocentric coordinates output by sgp4() 
    ' these have to be initialized or localized
    ' for now use set_sat_xyz(pos()) with each call
    Public Shared sat As New xyz_t()

    ' satellite topocentric coordinates.  These all come from xyztop(). 
    Public Shared azel As New sph_t()   ' slant range, azimuth, elevation
    Public Shared radec As New sph_t()  ' slant range, R.A., dec.
    Public Shared latlon As New sph_t() ' altitude, latitude, longitude

    ' elevation (rounded to the nearest int) of center of sun above the
    'satellite's horizon.  Adjusted for dip due to height of satellite, but no
    'allowance for refraction.  Value comes from xyztop(). 
    Public Shared elsusa As Integer

    ' Constants set at program startup by init(), and used by sgp4() 
    Public Shared qoms2t As Double
    Public Shared s As Double
    Public Shared ck2 As Double
    Public Shared ck4 As Double

    Public Shared MAXDIST As Integer = 2    ' max dist in earth radii before xyztop flags it as too far away

    Public Shared FIRSTIME As Boolean = True    ' some parameters are not available until xyztop has been called once - affects the star display

    ' c style functions in c++

    '	####################################################################
    '					Functions
    '					####################################################################

    ' module READEL:  gets orbital elements from file 

    'C++ TO VB CONVERTER TODO TASK: The implementation of the following method could not be found:
    '	 Function getel(ByVal fp As FILE) As Integer ' (fp)
    '	FILE *fp;
    '
    '	Loads the mean orbital element variables with data from fp.  Initializes
    '	precession by calling inpre() with the epoch of the orbital elements.
    '	Returns 1 if period of satellite >= 225 minutes, 0 if period < 225 min., -1
    '	if error (e.g., checksum or format bad). 

    '==================================================================
    ' initialize the sat xyz structure with the values from SPGX
    ' the pos(x,y,z) array is in km and it appears that the sat struct is in earth radii
    '==================================================================
    Public Shared Sub set_sat_xyz(ByVal pos() As Double)

        sat.x = pos(0) / DefConst.EARTHR2KM
        sat.y = pos(1) / DefConst.EARTHR2KM
        sat.z = pos(2) / DefConst.EARTHR2KM

    End Sub

    '==================================================================

    ' module ASTRO:  astronomical time, coordinate transformation 
    ' ASTROGR.C 

    '==================================================================

    '	 (year, month, day)
    '	int year, month, day;
    '	Returns the Julian Day (measured in days, not minutes) at 0h UT on the
    '	given date, Gregorian calendar.  No error checking for illegal dates. 

    '==================================================================
    Public Shared Function julday(ByVal y As Integer, ByVal m As Integer, ByVal d As Integer) As Double
        Static difm() As Integer = {0, 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334}

        If y Mod 4 = 0 AndAlso CBool(y Mod 100) OrElse y Mod 400 = 0 Then ' leap year
            If m > 2 Then ' after Feb
                d += 1 ' Feb had an extra day
            End If
        End If

        If y >= 0 Then
            y -= 1
        End If

        Return (AstroGR.dint(y * 365.25) - AstroGR.dint(y * 0.01) + AstroGR.dint(y * 0.0025) + difm(m) + d + 1721424.5)

        ' try this a different way from http://scienceworld.wolfram.com/astronomy/JulianDate.html
        ' both ways work correctly if you pass the day of month instead of the day of year phhhht :-(

        'return (double)(367. * y - floor(7. * (y + floor((m + 9.) / 12.)) / 4.) + floor(275. * m / 9.) + d + 1721013.5);
    End Function

    Public Shared Function Julian2Gregorian(ByVal JDdt As Double) As DateTime

        Dim JD, j, y, d, m As Long
        Dim dt As Double

        Dim Year, Month, Day, Hour, Min, Sec As Integer

        'while calculating we consider the following :  
        'for Years -> 4 centuries (146067 days)  
        'for Months -> 4 years and 5 consecutive months  
        'for days -> 5 consecutive months  

        ' the julian calendar starts at noon so we add 12 hours to the time
        JDdt = JDdt + 0.5

        JD = CLng(Int(JDdt)) ' convert to long int
        dt = JDdt - JD  ' the time portion of the julian date

        j = JD - 1721119 '1721119 is the number of days since JD 0 to the start of March 2 1BC. //hence j are the no of days from March 2 1BC

        'C++ TO VB CONVERTER WARNING: C++ to VB Converter cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
        y = CLng(Int((4 * j - 1) / 146097)) '146097 is the number of days in four centuries, time it takes for the cycle of leap years to repeat.
        j = 4 * j - 1 - 146097 * y '4 is used to denote leap year
        d = CLng(Int(j / 4))

        'C++ TO VB CONVERTER WARNING: C++ to VB Converter cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
        j = CLng(Int((4 * d + 3) / 1461)) 'number of days in four years.
        d = 4 * d + 3 - 1461 * j
        d = CLng(Int((d + 4) / 4))

        'C++ TO VB CONVERTER WARNING: C++ to VB Converter cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
        m = CLng(Int((5 * d - 3) / 153)) 'number of days in 5 consecutive months alternating between 31 and 30 days (either Mar-Jul or Aug-Dec).
        d = 5 * d - 3 - 153 * m
        d = CLng(Int((d + 5) / 5))
        y = 100 * y + j

        If m < 10 Then
            m = m + 3 'additions and subtractions of 3 and 9 are to restore the "start" of the year to January.
        Else
            m = m - 9 'additions and subtractions of 3 and 9 are to restore the "start" of the year to January.
            y = y + 1
        End If

        Year = CInt(Int(y))
        Month = CInt(Int(m))
        Day = CInt(Int(d))

        ' the julian calendar starts at noon so we add 12 hours to the time
        'dt = dt + 0.5
        'If dt > 1 Then
        'dt = dt - Int(dt)
        'Day = Day + 1
        'End If

        dt = dt * 24
        Hour = CInt(Int(dt))
        dt = (dt - Hour) * 60
        Min = CInt(Int(dt))
        dt = (dt - Min) * 60
        Sec = CInt(Int(dt))

        Julian2Gregorian = New System.DateTime(Year, Month, Day, Hour, Min, Sec)


    End Function

    '==================================================================
    '	 (iflag2, tp)
    '	int *iflag2;
    '	double tp;      epoch, used to compute the sun's position ' in minutes?
    '
    '	Satellite elevation is computed, based on coordinates in struct sat.
    '	If elevation is less than zero, xyztop() immediately returns 0; in
    '	this case, elsusa, radec, azel, and latlon are undefined.  On the
    '	other hand, if elevation is 0 or more, 1 is returned and the output
    '	variables are filled in.
    '
    '	If *iflag2 == 1, xyztop() will assume that subsequent calls will have tp at
    '	equally spaced intervals.  This saves the trouble of recomputing sidereal time
    '	from scratch with each call.  *iflag2 is reset to 0 on second call. 
    '==================================================================

    Public Shared Function xyztop(ByRef iflag2 As Integer, ByVal tp As Double, Optional sndx As Integer = 0) As Integer
        'C++ TO VB CONVERTER NOTE: 'extern' variable declarations are not required in VB:
        '		  extern Double localhra, phase ' needed for stars.c
        Dim g As New xyz_t()    ' geocentric satellite position
        Dim l As New xyz_t()    ' topocentric (equatorial 
        Dim lh As New xyz_t()   ' & horizontal,
        Dim ls As New xyz_t()   ' respectively) satellite position
        Dim suneq As New xyz_t() ' equatorial sun position
        Dim sunl As New xyz_t() ' to determine phase angle

        Dim rho As Double       ' distance to satellite
        Dim temp As Double      ' scratchpad
        Dim sinlha As Double    ' sin & cos of lhaa
        Dim coslha As Double
        Static lhaa As Double   ' local hr angle of Aries
        Static dlhaa As Double  ' delta lhaa between calls
        Static i As Integer     ' signals 2nd call in a prediction run

        Dim rval As Integer = 1 ' 0 - below horizon, 1 - should be visible, 2 - more than 2 earth radii away

        If iflag2 <> 0 Then
            iflag2 = 0
            i = 1
            lhaa = AstroGR.thetag(tp) + SeeSatVBmain.obs.lambda
            dlhaa = 0.0
        ElseIf i <> 0 Then
            i = 0
            dlhaa = AstroGR.thetag(tp) + SeeSatVBmain.obs.lambda - lhaa
        End If
        lhaa += dlhaa
        sinlha = Math.Sin(lhaa)
        coslha = Math.Cos(lhaa)
        localhra = lhaa

        '    Generate topocentric equatorial satellite coordinates by adding the
        '    topocentric coordinates of the geocenter to the geocentric satellite
        '    position.
        l.x = sat.x - SeeSatVBmain.obs.xc * coslha
        lh.x = l.x
        ls.x = lh.x
        l.y = sat.y - SeeSatVBmain.obs.xc * sinlha
        lh.y = l.y
        ls.y = lh.y
        l.z = sat.z + SeeSatVBmain.obs.zg
        lh.z = l.z
        ls.z = lh.z
        rho = Math.Sqrt(l.x * l.x + l.y * l.y + l.z * l.z)

        '    if to far away (2 earth radi) to be visible then return.  If it is close
        '    to the horizon it should get checked again soon.

        '#define MAXDIST 2.0
        ' geosynchronous orbits are 6.5 radii
        If rho > MAXDIST Then
            rval = 2
        End If

        '#undef  MAXDIST

        '     The lh struct will become a horizontal coordinate system, with
        '    x, y, and z pointing to south, east, and zenith, respectively.
        '    To accomplish this, I'll z-rotate it so the negative x-axis
        '    intersects earth's polar axis, then y-rotate it so the positive
        '    z-axis points to the zenith.  The required y-rotation is the
        '    complement of the latitude; we can get it by reversing the sine
        '    and cosine arguments to yrot().
        AstroGR.zrot(lh, sinlha, coslha)
        AstroGR.yrot(lh, SeeSatVBmain.obs.coslat, SeeSatVBmain.obs.sinlat)

        '      If elevation < 0, the satellite's coordinates are of no
        '    interest, so return immediately with a value of zero.  Otherwise,
        '    compute azimuth.  Sign of x is reversed so az starts at north and
        '    increases east.
        If lh.z < 0.0 Then
            rval = 0
            'Else       ' continue on 
        End If
            azel.phi = Math.Asin(lh.z / rho)
            azel.lambda = AstroGR.fmod2p(Math.Atan2(lh.y, -lh.x))
            azel.r = rho

            '    Precess the topocentric equatorial coordinates to epoch REFEP,
            '    compute right ascension & declination.
            AstroGR.preces(l)
            radec.r = rho
            radec.phi = Math.Asin(l.z / rho)
            radec.lambda = AstroGR.fmod2p(Math.Atan2(l.y, l.x))

            ' altitude, latitude, longitude
            rho = Math.Sqrt(sat.x * sat.x + sat.y * sat.y + sat.z * sat.z)
            g.x = sat.x / rho
            g.y = sat.y / rho
            g.z = sat.z / rho
            latlon.r = rho - DefConst.MEAN_R
            latlon.phi = Math.Asin(g.z)
            temp = AstroGR.fmod2p(Math.Atan2(g.y, g.x) - lhaa + SeeSatVBmain.obs.lambda)
            If temp > DefConst.PI Then
                temp -= DefConst.TWOPI ' west lon. is negative
            End If
            latlon.lambda = temp

            '    Sun elevation at satellite.  Get sun position, rotate the
            '    axes so the satellite is on the positive z-axis.  Sun elevation =
            '    arc sin z.  Add dip of horizon due to height of satellite.

            AstroGR.sun(suneq, tp) ' suneq = xyz sun position

            ' copy sun coords
            sunl.x = suneq.x
            sunl.y = suneq.y
            sunl.z = suneq.z

            ' make ls into a unit vector
            rho = Math.Sqrt(ls.x * ls.x + ls.y * ls.y + ls.z * ls.z)
            ls.x = ls.x / rho
            ls.y = ls.y / rho
            ls.z = ls.z / rho

            ' do the rotation for the geocenter
            temp = Math.Sqrt(1 - g.z * g.z) ' dist. to z axis
            AstroGR.zrot(suneq, g.y / temp, g.x / temp)
            AstroGR.yrot(suneq, temp, g.z)

            ' the same for the topocenter
            temp = Math.Sqrt(1 - ls.z * ls.z) ' dist. to z axis
            AstroGR.zrot(sunl, ls.y / temp, ls.x / temp)
            AstroGR.yrot(sunl, temp, ls.z)
            phase = Math.Asin(sunl.z) + DefConst.PIO2

            ' right here it was blowing up with negative square root 
            ' latlon.r is < 0 and > -2
            'temp = (Math.Asin(suneq.z) + Math.Atan(Math.Sqrt((2 + latlon.r) * latlon.r))) * DefConst.RA2DE
            temp = (Math.Asin(suneq.z) + Math.Atan(Math.Sqrt(Math.Abs((2 + latlon.r)) * Math.Abs(latlon.r)))) * DefConst.RA2DE

            ' ensure temp truncates correctly when converted to int
            If temp >= 0.0 Then
                temp += 0.5
            Else
                temp -= 0.5
            End If

            elsusa = CInt(Fix(temp))
        'End If

        save_values(sndx)

        Return rval ' 0 - below horizon, 1 - should be visible, 2 - more than 2 earth radii away
    End Function

    ' stash all of the local vars back to the main Sats structure in SeesSatVBMain
    ' sloppy mess to clean up 
    Private Shared Sub save_values(ByVal sndx As Integer)

        If sndx > 0 AndAlso Not (IsNothing(SeeSatVBmain.satellites)) AndAlso SeeSatVBmain.satellites.Length > 1 Then
            SeeSatVBmain.satellites(sndx).view.elsusa = elsusa
            SeeSatVBmain.satellites(sndx).view.radec = radec.Clone
            'SeeSatVBmain.satellites(sndx).view.radec.phi = radec.phi
            'SeeSatVBmain.satellites(sndx).view.radec.r = radec.r
            SeeSatVBmain.satellites(sndx).view.azel = azel.Clone
            'SeeSatVBmain.satellites(sndx).view.azel.phi = azel.phi
            'SeeSatVBmain.satellites(sndx).view.azel.r = azel.r
            SeeSatVBmain.satellites(sndx).view.latlon = latlon.Clone
            'SeeSatVBmain.satellites(sndx).view.latlon.phi = latlon.phi
            'SeeSatVBmain.satellites(sndx).view.latlon.r = latlon.r
            SeeSatVBmain.satellites(sndx).view.posn = sat.Clone
            'SeeSatVBmain.satellites(sndx).view.posn.y = sat.y
            'SeeSatVBmain.satellites(sndx).view.posn.z = sat.z
            SeeSatVBmain.satellites(sndx).view.phase = phase
            SeeSatVBmain.satellites(sndx).view.illum = illumination()
            SeeSatVBmain.satellites(sndx).view.truemag = true_mag(SeeSatVBmain.satellites(sndx).tle0.smag)
        End If

    End Sub

    '==================================================================
    '	 (ep)
    '	double ep;      epoch
    '	Returns elevation of sun (in degrees) at observer. 
    '==================================================================

    Public Shared Function dusk(ByVal ep As Double) As Integer
        Dim lst As Double ' local sidereal time
        Dim elev As Double ' elevation of sun, deg
        Dim csun As New xyz_t() ' coordinates of sun

        lst = AstroGR.thetag(ep) + SeeSatVBmain.obs.lambda
        AstroGR.sun(csun, ep) ' get sun's position

        ' point the positive z-axis at zenith
        AstroGR.zrot(csun, Math.Sin(lst), Math.Cos(lst))
        AstroGR.yrot(csun, SeeSatVBmain.obs.coslat, SeeSatVBmain.obs.sinlat)

        elev = Math.Asin(csun.z) * DefConst.RA2DE
        '     If desired, the sun's azimuth (deg) can be found here with the
        '    expression fmod2p(atan2(csun.y, -csun.x)) * RA2DE

        ' ensure truncation occurs in correct direction
        If elev >= 0.0 Then
            elev += 0.5
        Else
            elev -= 0.5
        End If
        Return (CInt(Fix(elev)))
    End Function

    '==================================================================
    '	 (ep)
    '	double ep;          epoch
    '	Returns the parallactic angle in degrees: the angle on the celestial sphere
    '	whose vertex is the satellite, initial side contains the north pole, and
    '	terminal side contains the zenith of the observer.  Angle increases east. 
    '==================================================================

    Public Shared Function parall(ByVal t As Double) As Integer
        Dim zenith As New xyz_t()
        Dim lst As Double
        Dim iflag As Integer
        Dim iflag2 As Integer

        iflag = 1
        iflag2 = iflag

#If (TEST = 0) Then
        '     Get R.A., dec. of satellite at time t.  In TEST mode, the values
        '    for struct radec are set by the test software

        ' ATM TODO fix this
        'sgp4(iflag, t - epoch)
        AstroGR.xyztop(iflag2, t)
#End If

        lst = AstroGR.thetag(t) + SeeSatVBmain.obs.lambda ' local sidereal time

        '     Load struct "zenith" with the topocentric equatorial xyz
        '    components of a unit vector directed from the observer to the
        '    zenith.
        zenith.z = SeeSatVBmain.obs.sinlat
        zenith.x = Math.Cos(lst) * SeeSatVBmain.obs.coslat
        zenith.y = Math.Sin(lst) * SeeSatVBmain.obs.coslat

        '     z-rotate the axes to bring the satellite into the x-z
        '    plane, such that the satellite has a positive x coordinate.
        '    Bear in mind that struct "zenith" ALWAYS has the topocentric
        '    coordinates of the ZENITH.  We are rotating its axes into a
        '    particular relationship with the satellite.

        AstroGR.zrot(zenith, Math.Sin(radec.lambda), Math.Cos(radec.lambda))

        '     Satellite y-coordinate == 0.  x & y of north pole == 0.
        '    y-rotate so the positive z-axis passes through satellite.
        AstroGR.yrot(zenith, Math.Cos(radec.phi), Math.Sin(radec.phi))

        '     The z-axis passes through the satellite.  The north pole
        '    has a y-coordinate of 0 and a negative x-coordinate.  The
        '    origin of the coordinate system is still at the observer.

        Return (CInt(Fix(AstroGR.fmod2p(Math.Atan2(zenith.y, -zenith.x)) * DefConst.RA2DE + 0.5)))
    End Function


    '==================================================================
    '	Responsible for filling in all data in struct "obs" in module ASTRO.  Must be
    '	called when SEESAT is started up, before any predictions are run.  Returns
    '	zone description (local time - Greenwich time) in minutes. 
    '==================================================================
    Public Shared Function topos(ByVal d_alt As Double, ByVal d_lat As Double, ByVal d_lon As Double, ByVal d_tz As Double) As Double
        'C++ TO VB CONVERTER NOTE: The following #define macro was replaced in-line:
        'ORIGINAL LINE: #define din(x) atof(x)
        '#Const din = True

        Dim lat As Double
        Dim C As Double
        Dim S As Double
        Static f As Double = 0.00335278 ' WGS-72 value for flattening of earth

        ' potential problem here - we are using meters
        ' and I don't think it takes into account the elipsoid - yes it does but with the WGS72 model
        'SeeSatVBmain.obs.r = d_alt * 0.00000004778826 ' ft->earth radii

        SeeSatVBmain.obs.r = d_alt * DefConst.KM2EARTHR / 1000 ' m->earth radii
        'printf("NORTH LATITUDE\n");
        lat = d_lat * DefConst.DE2RA
        'printf("EAST LONGITUDE\n");
        SeeSatVBmain.obs.lambda = d_lon * DefConst.DE2RA

        ' sin & cos of observer's latitude
        SeeSatVBmain.obs.sinlat = Math.Sin(lat)
        SeeSatVBmain.obs.coslat = Math.Cos(lat)


        '     observer's position with respect to geocenter (formulas
        '    from Baker & Makemson)
        'C++ TO VB CONVERTER WARNING: C++ to VB Converter cannot determine whether both operands of this division are integer types
        ' - if they are then you should use the VB integer division operator:
        C = 1.0 / Math.Sqrt(1.0 - (f + f - f * f) * SeeSatVBmain.obs.sinlat * SeeSatVBmain.obs.sinlat)
        S = C * (1.0 - f) * (1.0 - f)
        SeeSatVBmain.obs.xc = (C + SeeSatVBmain.obs.r) * SeeSatVBmain.obs.coslat
        SeeSatVBmain.obs.zg = -(S + SeeSatVBmain.obs.r) * SeeSatVBmain.obs.sinlat

        'printf("TIME ZONE OFFSET\n(GMT - LOCAL TIME)\nHOURS ");
        Return Convert.ToDouble(d_tz) * 60.0
        'return (-8.0 * 60.0);
    End Function

    '==================================================================
    '	 (ep)
    '	double ep;
    '	Sets up struct "dircos" (direction cosines) in module ASTRO to precess
    '	satellite R.A/dec.  Initial epoch will be "ep" and final epoch is
    '	determined by REFEP in ASTRO. 
    '==================================================================
    Public Shared Sub inpre(ByVal epoch As Double)
        ' Set up struct "dircos" to precess from initial epoch "epoch" to final epoch
        '"REFEP".  Used in conjunction with preces().  Formulae are the "rigorous
        'method" given by Meeus, which he in turn attributes to Newcomb.
        ' epoch is satellite epoch in julian days expressed in minutes
        ' SGP4class uses epoch julian days expressed in days

        Dim temp As Double
        Dim sinzet As Double
        Dim coszet As Double
        Dim sinz As Double
        Dim cosz As Double
        Dim sinthe As Double
        Dim costhe As Double
        Dim tau0 As Double
        Dim tau As Double

        Static trocen As Double = 52594876.7 ' min. per trop. century

        tau0 = (epoch - 3477629251.0) / trocen
        'C++ TO VB CONVERTER WARNING: C++ to VB Converter cannot determine whether both operands of this division are integer types
        ' - if they are then you should use the VB integer division operator:
        tau = (DefConst.REFEP - epoch) / trocen

        temp = tau * ((0.011171319 + 0.000006768 * tau0) + tau * (0.000001464 + tau * 0.000000087))
        sinzet = Math.Sin(temp)
        coszet = Math.Cos(temp)

        temp = temp + tau * tau * (0.000003835 + 0.000000005 * tau)
        sinz = Math.Sin(temp)
        cosz = Math.Cos(temp)

        '	  temp = tau * ((9.7189726e-3 - 4.135e-6 * tau0) + tau * (-2.065e-6 2.04e-7 * tau))
        '	  -2.065e-6 -= 1

        temp = tau * ((0.0097189726 - 0.000004135 * tau0) + tau * (-0.000002065 - -0.000000204 * tau))

        sinthe = Math.Sin(temp)
        costhe = Math.Cos(temp)

        dircos.xx = coszet * cosz * costhe - sinzet * sinz
        dircos.xy = sinzet * cosz + coszet * sinz * costhe
        dircos.xz = coszet * sinthe
        dircos.yx = -coszet * sinz - sinzet * cosz * costhe
        dircos.yy = coszet * cosz - sinzet * sinz * costhe
        dircos.yz = -sinzet * sinthe
        dircos.zx = -cosz * sinthe
        dircos.zy = -sinz * sinthe
        dircos.zz = costhe
    End Sub

    '==================================================================
    '	 (x)
    '	double x;
    '	Returns x, such that 0 <= x < 2pi, i.e., takes out multiples of 2pi. 
    '==================================================================

    Public Shared Function fmod2p(ByVal x As Double) As Double
        x /= DefConst.TWOPI
        x = (x - AstroGR.dint(x)) * DefConst.TWOPI
        If x < 0.0 Then
            x += DefConst.TWOPI
        End If
        Return x
    End Function

    '==================================================================
    '	 (ep)
    '	Returns Greenwich hour angle of Aries at epoch ep.
    '	Side effect: sets ds50 to minutes between 1950 Jan 0 0h UT & ep. 
    '==================================================================
    Public Shared Function thetag(ByVal ep As Double) As Double
        ' Returns Greenwich hour angle of the mean equinox at ep.
        'As a side effect, sets ds50 to minutes since 1950 Jan 0 0h UT.
        Dim theta As Double
        Dim temp As Double
        'Dim i As Integer

        ds50 = (ep - 3503925360.0) / DefConst.XMNPDA
        theta = 1.72944494 + 6.3003880987 * ds50
        temp = CInt(Fix(theta / DefConst.TWOPI))
        ' temp = i = (int)(temp);     drop fractional part of temp
        Return theta - temp * DefConst.TWOPI
    End Function

    '==================================================================
    ' changed to by ref ATM
    '==================================================================
    Public Shared Sub zrot(ByRef cor As xyz_t, ByVal sint As Double, ByVal cost As Double)
        ' Z-rotates coordinate axes for "cor" by angle t, where "sint"
        'and "cost" are the sine and cosine of t.

        Dim tempx As Double
        Dim tempy As Double

        tempx = cor.x * cost + cor.y * sint
        tempy = cor.y * cost - cor.x * sint
        cor.x = tempx
        cor.y = tempy
    End Sub

    '==================================================================
    ' similar to zrot(), except rotate about y axis
    ' changed to by ref ATM
    '==================================================================
    Public Shared Sub yrot(ByRef cor As xyz_t, ByVal sint As Double, ByVal cost As Double)
        Dim tempx As Double
        Dim tempz As Double
        tempz = cor.z * cost + cor.x * sint
        tempx = cor.x * cost - cor.z * sint
        cor.x = tempx
        cor.z = tempz
    End Sub

    '==================================================================
    ' Uses direction cosines "dircos" (external struct) to rotate axes of
    'struct "cor".
    ' changed to by ref ATM
    '==================================================================
    Public Shared Sub preces(ByRef cor As xyz_t)

        Dim tempx As Double
        Dim tempy As Double
        Dim tempz As Double
        tempx = cor.x * dircos.xx + cor.y * dircos.yx + cor.z * dircos.zx
        tempy = cor.x * dircos.xy + cor.y * dircos.yy + cor.z * dircos.zy
        tempz = cor.x * dircos.xz + cor.y * dircos.yz + cor.z * dircos.zz
        cor.x = tempx
        cor.y = tempy
        cor.z = tempz
    End Sub

    '  set up parameters for calcstar
    Public Shared Function initstar() As Integer
        'extern struct OBS obs;
        'extern double decmax;
        Dim lat As Double
        'extern double twopi;
        Dim rval As Integer = 0

        If SeeSatVBmain.obs Is Nothing Then
            MessageBox.Show("Tilt - obs struct not initialized - call topos")
            rval = -1
        Else
            lat = Math.Asin(SeeSatVBmain.obs.sinlat)

            If lat > 0.0 Then
                decmax = -DefConst.TWOPI + lat
            Else
                decmax = DefConst.TWOPI + lat
            End If

        End If
        initstar = rval
    End Function

    '  translate ra, dec to alt, azm at observers time and location
    '
    Public Shared Function calcstar(ByVal ndx As Integer, ByRef starxy As star_xy) As Integer

        'extern struct OBS obs;
        'extern struct STAR * stars;
        'extern struct sph starxy;
        'extern double decmax;      /* from initstar */
        'extern double localhra;   /* local hour angle of aries from xyztop() */
        '	extern double DEC2RA, localra, PI, twopi;

        'Dim starxy As New star_xy

        ' MUST CALL XYZTOP FIRST for llhaa

        Dim dhra As Double ' temporary terms
        Dim cosdhra As Double
        Dim cosz As Double
        Dim sinzsinA As Double
        Dim sinzcosA As Double

        If SatIO.stars(ndx).mag = 0.18 Then
            'we have rigel
            Dim rval As Integer = 0
        End If

        If decmax < 0.0 AndAlso SatIO.stars(ndx).dec < CSng(decmax) Then
            Return (0) ' star below southern horizon
        End If

        If decmax > 0.0 AndAlso SatIO.stars(ndx).dec > CSng(decmax) Then
            Return (0) ' star below northern horizon
        End If

        dhra = localhra - SatIO.stars(ndx).ra
        cosdhra = Math.Cos(dhra)

        cosz = (CType(SatIO.stars(ndx).sindec, Double) * SeeSatVBmain.obs.sinlat) + _
            (CType(SatIO.stars(ndx).cosdec, Double) * SeeSatVBmain.obs.coslat * cosdhra)

        starxy.alt = DefConst.PIO2 - Math.Acos(cosz)
        If starxy.alt < 0.0 Then ' is below the horizon
            'If starxy.alt < -0.15 Then ' is 8 degrees below the horizon
            Return (0)
        End If

        sinzcosA = (CType(SatIO.stars(ndx).sindec, Double) * SeeSatVBmain.obs.coslat - _
                    CType(SatIO.stars(ndx).cosdec, Double) * cosdhra * SeeSatVBmain.obs.sinlat)

        sinzsinA = -CType(SatIO.stars(ndx).cosdec, Double) * Math.Sin(dhra)

        starxy.azm = Math.Atan(sinzsinA / sinzcosA)
        If sinzcosA < 0.0 Then
            starxy.azm = DefConst.PI + starxy.azm
        End If

        starxy.mag = CType(SatIO.stars(ndx).mag, Double)

        Return (1)

    End Function

    ' ATM - ref https://groups.google.com/forum/#!topic/sara-list/1isnv33wSoU
    'convert alt azm to ra dec at current llhra
    'checks to see if we are in an active run and returns false if not
    'angles in radians
    Public Shared Function AltAzmtoRaDec(ByVal Alt As Double, ByVal Azm As Double, ByRef Ra As Double, ByRef Dec As Double) As Boolean

        If SatWindow.SHOWSTARS = False Then
            Return False
        End If

        If SeeSatVBmain.obs Is Nothing Then
            MessageBox.Show("Tilt - obs struct not initialized - call topos")
            Return False
        End If

        Dim sinDec As Double = SeeSatVBmain.obs.sinlat * Math.Sin(Alt) + SeeSatVBmain.obs.coslat * Math.Cos(Alt) * Math.Cos(Azm)
        Dec = Math.Asin(sinDec)
        Dim cosH As Double = (Math.Sin(Alt) - sinDec * SeeSatVBmain.obs.sinlat) / (Math.Cos(Dec) * SeeSatVBmain.obs.coslat)

        ' rare case where cosH is fractionally less than -1 or more than 1
        If cosH < -1 Then
            cosH = -1
        End If

        If cosH > 1 Then
            cosH = 1
        End If

        ' local hour of aries for the time and observers position
        Ra = localhra - Math.Acos(cosH)

        If Ra < 0 Then
            Ra += DefConst.TWOPI
        End If
        
        Return True

        ' http://mathematica.stackexchange.com/questions/69330/astronomy-transform-coordinates-from-horizon-to-equatorial
        '       AzElToRADec[{azimuth_, elevation_}, lattitude_, LST_] := 
        'Module[{sinDec, cosLHA, sinLHA, Dec, RA, LHA}, 
        ' sinDec = Sin[elevation Degree] Sin[lattitude Degree] + 
        ' Cos[elevation Degree] Cos[lattitude Degree] Cos[azimuth Degree];
        ' Dec = ArcSin[sinDec]/Degree;
        ' sinLHA = -((Sin[azimuth Degree] Cos[elevation Degree])/Cos[Dec Degree]);
        ' cosLHA = (Sin[elevation Degree] - Sin[lattitude Degree] Sin[Dec Degree])/(Cos[Dec Degree] Cos[lattitude Degree]);
        ' LHA = ArcTan[cosLHA, sinLHA]/Degree;
        ' RA = Mod[LST - LHA, 360];
        ' {RA, Dec}]   
    End Function


    '==================================================================
    ' Put the mean equatorial xyz components of a unit vector to sun at
    'epoch "ep" into struct "pos".  Good to about .01 deg.
    ' changed to by ref ATM
    '==================================================================
    Public Shared Sub sunref(ByRef pos As xyz_t, ByVal ep As Double)
        Dim lon As Double
        Dim sinlon As Double

        '     sin & cos of obliq. of ecliptic.  Since the obliquity of the
        '    ecliptic changes but .013 deg/century, I use a constant value,
        '    that of 0h 16 Feb 1988 ET.
        Static sineps As Double = 0.397802
        Static coseps As Double = 0.917471

        lon = AstroGR.sunlon(ep)
        sinlon = Math.Sin(lon)

        pos.x = Math.Cos(lon)
        pos.y = sinlon * coseps
        pos.z = sinlon * sineps
    End Sub

    '==================================================================
    ' Return geometric mean longitude of sun at "epoch", accurate to
    'about .01 deg.  This is ample for our purpose.  Formulas from Meeus,
    'simplified to correspond to our required accuracy.  The returned value
    'may be off by a small multiple of 2pi.  That should cause no problem if
    'you have good sin() & cos(), since in this program the longitude is only
    'used by those functions.
    '==================================================================
    Public Shared Function sunlon(ByVal epoch As Double) As Double
        Dim T As Double
        Dim L As Double
        Dim e As Double
        Dim M As Double
        Dim Enew As Double
        Dim Eold As Double
        Dim v As Double

        T = (epoch / DefConst.XMNPDA - 2415020.0) / 36525.0

        ' geometric mean long.
        L = AstroGR.fmod2p((279.69668 + T * 36000.76892) * DefConst.DE2RA)

        ' mean anomaly
        M = AstroGR.fmod2p((358.47583 + T * 35999.04975) * DefConst.DE2RA)

        ' eccentricity
        e = 0.01675104 - T * 0.0000418

        ' solve Kepler's equation to .01 deg.
        Enew = M
        Do
            Eold = Enew
            Enew = M + e * Math.Sin(Eold)
        Loop While Enew - Eold >= 0.00017

        '     true anomaly.  If the eccentric anomaly (Enew) is near pi, there
        '    will be trouble in the true anomaly formula since tan is asymptotic at
        '    pi/2.  So if eccentric anomaly is within .01 deg of pi radians, we'll
        '    just say v is pi.  Yes, e also affects v, but if Enew is near pi e's
        '    effect is negligible for our purposes.
        If Math.Abs(Enew - DefConst.PI) <= 0.00017 Then
            v = DefConst.PI
        Else
            v = 2.0 * Math.Atan(Math.Sqrt((1.0 + e) / (1.0 - e)) * Math.Tan(Enew / 2.0))
        End If

        Return L + v - M
    End Function

    '==================================================================
    ' corrected pos to be byref ATM
    '==================================================================
    Public Shared Sub sun(ByRef pos As xyz_t, ByVal ep As Double)
        ' Equatorial xyz components of a unit vector to sun at epoch "ep" are
        'returned in struct "pos".  Accuracy about .1 deg.

        Static t0 As Double ' change in x, y, z during 6 days -  start epoch of a 6-day interval
        Static dx As Double
        Static dy As Double
        Static dz As Double
        Static sun0 As New xyz_t() ' sun position at t0

        Dim t As Double ' e.g. .5 if ep = t0 + 3 days

        'C++ TO VB CONVERTER WARNING: An assignment within expression was extracted from the following statement:
        'ORIGINAL LINE: if ((t = (ep - t0) / 8640.0) > 1.0 || t < 0.0)
        t = (ep - t0) / 8640.0
        If t > 1.0 OrElse t < 0.0 Then
            '     Position was requested for a time outside the current 6-day
            '    interval.  So set up a new 6-day interval centered at "ep".
            Dim sun1 As New xyz_t()

            t0 = ep - 4320.0 ' 3 days before "ep"
            AstroGR.sunref(sun0, t0)
            AstroGR.sunref(sun1, ep + 4320.0) ' 3 days after "ep"
            dx = sun1.x - sun0.x
            dy = sun1.y - sun0.y
            dz = sun1.z - sun0.z
            t = 0.5
        End If
        ' get position by linear interpolation in the 6-day period
        pos.x = sun0.x + t * dx
        pos.y = sun0.y + t * dy
        pos.z = sun0.z + t * dz
    End Sub

    '==================================================================

    ' REFEP is the epoch to which R.A./dec. will be precessed.
    'E.g. (per Meeus p. 101):
    '3477629251. = epoch 1900.0, 3503926689. = 1950.0, 3530224800. = 2000.0.
    'EPMSG is part of the message (printed at startup) which states what epoch
    'will be used for R.A./dec.  Be sure REFEP & EPMSG agree. 

    '#Const REFEP = True
    '#Const EPMSG = True

    '######################## LOCAL FUNCTIONS ########################


    'C++ TO VB CONVERTER TODO TASK: The implementation of the following method could not be found:
    'Sub zrot() ' z- & y-rotation
    'C++ TO VB CONVERTER TODO TASK: The implementation of the following method could not be found:
    'Sub yrot()
    'C++ TO VB CONVERTER TODO TASK: The implementation of the following method could not be found:
    'Sub preces() ' corrects coordinates for precession
    'C++ TO VB CONVERTER TODO TASK: The implementation of the following method could not be found:
    'Sub sunref() ' xyz of sun (non-interpol.)
    'C++ TO VB CONVERTER TODO TASK: The implementation of the following method could not be found:
    'Function sunlon() As Double ' true longitude of sun
    'C++ TO VB CONVERTER TODO TASK: The implementation of the following method could not be found:
    'Sub sun() ' xyz of sun (interpol.)
    Public Shared dircos As New AnonymousClass()

    Public Shared localhra As Double
    Public Shared phase As Double

    '############################## CODE ##############################
    Public Shared Function dint(ByVal x As Double) As Double
        If x >= 0 Then
            Return Math.Floor(x)
        End If
        Return Math.Ceiling(x)
    End Function

    '##################################################################
    ' these routines are from Hirose's drive2.c
    '##################################################################

    Private Shared Function illumination() As Double
        ' Must be called after xyztop which sets phase for this particular satellite

        Dim illum As Double

        ' percent illumination - make sure if isn't 0 
        'C++ TO VB CONVERTER WARNING: C++ to VB Converter cannot determine whether both operands of this division are integer types
        ' - if they are then you should use the VB integer division operator:

        illum = (0.5 * (Math.Sin(DefConst.PI - phase) - (DefConst.PI - phase) * Math.Cos(DefConst.PI - phase))) / DefConst.PIO2
        If illum < 0.001 Then
            illum = 0.001
        End If

        ' egprintf(13, id*2+1, "il%3d", (int)(illum*100) ); 

        Return illum

    End Function

    '==================================================================
    '    mag = stdmag - 15.8 + 2.5 * log10 (range * range / fracil)
    '    where : stdmag = standard magnitude as defined above
    '            range = distance from observer to satellite, km
    '            fracil = fraction of satellite illuminated, [ 0 <= fracil <= 1 ]
    '==================================================================
    ' this module should be private and not called from outside
    Private Shared Function true_mag(ByVal mgntd As Single) As Double
        ' Must be called after xyztop which sets phase and mgntd for this particular satellite
        ' the calculation for the Molcazan estimate is  mag = stdmag - 15.75 + 2.5 * log10 (range * range / fracil)
        ' so this is the Molcazan formula. If using the McCants values the mag should be adjusted up by a factor of 1.5 to 2

        Return (Fix(Math.Abs(mgntd - 15.8 + 2.5 * Math.Log10(Math.Pow(radec.r * DefConst.EARTHR2KM, 2) / illumination())) * 10) / 10)
        'Return ((Fix(Math.Abs(mgntd - 15.8 + 2.5 * Math.Log10(Math.Pow(dist * DefConst.EARTHR2KM, 2) / illumination(phase))))) * 10) / 10

    End Function

    '==================================================================
    ' Pass precision, value, dms_t string struct which is the return value
    '==================================================================
    Public Shared Sub degdms(ByVal pre As Integer, ByVal x As Double, ByRef timstr As dms_t)

        Dim d As Double
        Dim i As Integer
        Dim sign As Integer

        If x >= 0.0 Then
            sign = 0
        Else
            sign = 1
        End If
        x = Math.Abs(x)

        '     compute value of least significant digit
        '    specified by pre 
        If pre <= 2 Then
            If pre = 1 Then
                d = 0.16666666666666666 ' 1/6
            Else
                d = 0.016666666666666666 ' 1/60
            End If
        Else
            d = Math.Pow(10.0, CDbl(4 - pre)) / 3600.0
        End If

        ' Round x to nearest multiple of d. 

        x = dint(x / d + 0.5)
        x *= d

        '     x may not be an exact multiple of d.  This will cause difficulties
        '    if (for example) we want rounding to nearest 10 min. but x is
        '    equivalent to 34 deg. 49.9999999 sec.  Because dint() truncates the
        '    fractional part of its argument, we will get 49 minutes in this
        '    example rather than the obviously correct 50 unless I do some
        '    defensive coding.
        '
        '    The trick is to add some small quantity to x, enough to roll it
        '    over if it's low, but not so much that the wrong number comes
        '    out. 

        Select Case pre
            Case 1, 2
                d = 0.0083333333333333332 ' 1/2 minute
            Case 3, 4
                d = 0.00013888888888888889 ' 1/2 second
            Case Else
                d = 0.0

        End Select
        x += d

        ' get degrees 
        i = CInt(Fix(x))
        If sign <> 0 Then
            timstr.deg = String.Format("-{0:D}", i)
        Else
            timstr.deg = String.Format("{0:D}", i)
        End If

        ' get minutes 
        x = (x - i) * 60.0 ' x = minutes remainder
        i = CInt(Fix(x))
        timstr.min = String.Format("{0:D}", i)

        If pre < 3 Then
            Return
        End If

        ' get seconds 
        x = (x - i) * 60.0

        If pre < 5 Then
            timstr.sec = String.Format("{0:D}", CInt(Fix(x)))
            Return

        End If
        'C++ TO VB CONVERTER TODO TASK: The following line has a C format specifier which cannot be directly translated to VB:
        'ORIGINAL LINE: sprintf(timstr.sec, "%.*f", pre - 4, x);
        timstr.sec = String.Format("%.*f", pre - 4, x)
    End Sub

    '==================================================================
    '==================================================================

End Class

'
'ASTROGR.C
'7 May 1989 by Paul S. Hirose
'Coordinate transformation and astronomical functions for SEESAT satellite
'tracking program.
'
'SEESAT.H
'by Paul S. Hirose, Edwards AFB, Calif.
'Declarations & definitions for globals in the SEESAT satellite tracking
'program.

'C++ TO VB CONVERTER TODO TASK: #define macros defined in multiple preprocessor conditionals can only be replaced within the scope of the preprocessor conditional:
'C++ TO VB CONVERTER NOTE: The following #define macro was replaced in-line:
'ORIGINAL LINE: #define EQUALS(N) = N
'#Const EQUALS = True
'C++ TO VB CONVERTER TODO TASK: #define macros defined in multiple preprocessor conditionals can only be replaced within the scope of the preprocessor conditional:
'#Const DRIVER = True

'#define EQUALS(N)

'####################################################################
'            Units & Conventions
'####################################################################
'
'Unless the otherwise indicated, throughout this program
'quantities are measured in the following units:
'
'time (interval)     minutes
'time (epoch)        minutes since 4713 B.C.
'angle           radians
'length          equatorial earth radii (1 unit = XKMPER km)
'
'South declinations & south latitudes are negative.
'Elevations below the horizon are negative.
'East longitude is positive, west negative.
'Azimuth is measured starting at north, increases as one turns
'eastward, and 0 <= azimuth < 2pi radians.
'


'####################################################################
'                Structures
'####################################################################
' star RA, DE, Mag
Public Class star_t
    Public ra As Double
    Public dec As Double
    Public mag As Double
    Public cosdec As Double
    Public sindec As Double
    Public Function Clone() As star_t
        Return DirectCast(Me.MemberwiseClone(), star_t)
    End Function
End Class

Public Class star_xy
    Public alt As Double        ' elevation
    Public azm As Double        ' azimuth
    Public mag As Double        ' magnitude
    Public Function Clone() As star_xy
        Return DirectCast(Me.MemberwiseClone(), star_xy)
    End Function
End Class


' rectangular coordinates 

Public Class xyz_t
    Public x As Double
    Public y As Double
    Public z As Double
    Public Function Clone() As xyz_t
        Return DirectCast(Me.MemberwiseClone(), xyz_t)
    End Function
End Class

'defined in the main program
' data for observer's location 
'Public Class obs_t
'Public r As Double      'altitude above sea level
'Public lambda As Double 'longitude
'Public sinlat As Double 'latitude sin & cos
'Public coslat As Double
'Public zg As Double     'north distance from observatory to equatorial plane
'Public xc As Double     'distance between observatory & polar axis of earth 
'End Class

' Spherical coordinates.  R is used for a linear measurement (e.g.,
'radius), phi for an angle measured with respect to the principal
'plane (e.g., latitude), and lambda for an angle measured IN the
'principal plane (e.g., longitude). 

Public Class sph_t
    Public r As Double      'radius
    Public phi As Double    'angle (lat)
    Public lambda As Double 'angle (lon)
    Public Function Clone() As sph_t
        Return DirectCast(Me.MemberwiseClone(), sph_t)
    End Function
End Class ' 1st call ' 2nd call

'########################### LOCAL DATA ###########################

' direction cosines, i.e., a rotation matrix.  Initialized by inpre() & used
'by preces() to correct satellite Right Ascension & declination for precession
'of earth's polar axis 
'C++ TO VB CONVERTER NOTE: Classes must be named in VB, so the following class has been named AnonymousClass:
'Public Class AnonymousClass
Public Class AnonymousClass
    Public xx As Double
    Public xy As Double
    Public xz As Double
    Public yx As Double
    Public yy As Double
    Public yz As Double
    Public zx As Double
    Public zy As Double
    Public zz As Double
End Class

