
Partial Friend NotInheritable Class DefConst

    Public Const VERS As String = "V0.5 02/18/2015"
    Public Const R As String = "r"
    Public Const W As String = "w"
    Public Const D_ALT As String = "640.0"
    Public Const D_LAT As String = "54.00563"
    Public Const D_LON As String = "-123.97506"
    Public Const D_TZ As String = "8"
    Public Const REFEP As Double = 3530224800.0
    Public Const EPMSG As String = "2000.0"
    Public Const TEST As Integer = 1

    '####################################################################
    '            Mathematical Constants
    '####################################################################

    Public Const PI As Double = 3.1415926535897931 'pumpkin
    Public Const E6A As Double = 0.000001          '1.0e-6
    Public Const PIO2 As Double = PI / 2.0         'pi/2
    Public Const TOTHRD As Double = 2.0 / 3.0      '2/3
    Public Const TWOPI As Double = PI * 2.0        '2pi
    Public Const X3PIO2 As Double = 3.0 * PI / 2.0 '3pi/2
    Public Const DE2RA As Double = PI / 180.0      'radians per degree
    Public Const RA2DE As Double = 180.0 / PI      'deg per radian
    Public Const XJ3 As Double = -0.00000253881    '3rd & 
    Public Const XJ4 As Double = -0.00000165597    '4th gravitational zonal harmonic

    '####################################################################
    '            Physical Constants
    '####################################################################

    ' dimensions & gravity of earth, World Geodetic System 1972 values 

    Public Const XJ2 As Double = 0.001082616 ' sidereal/solar time ratio -  distance units, earth radii -  time units/day
    Public Const XKE As Double = 0.0743669161
    Public Const XMNPDA As Double = 1440.0
    Public Const AE As Double = 1.0
    Public Const SIDSOL As Double = 1.002737908
    'Public const  earth_radius_in_km = 6378.135

    ' sat_t structure size_t info
    Public Const INTL_SIZE As Integer = 9
    Public Const N_POS_SIZE As Integer = 2

    Public Const SECPERDAY As Double = 86400.0
    Public Const MINPERDAY As Double = 1440.0
    Public Const HRPERDAY As Double = 24.0
    Public Const HRS2DEG As Double = 15.0
    Public Const EARTHR2KM As Double = 6378.135
    Public Const KM2EARTHR As Double = 1 / EARTHR2KM
    'Public Const XKMPER As Double = 6378.135
    Public Const MEAN_R As Double = 0.998882406

    '94 = maximum possible size of the 'deep_arg_t' structure,  in 8-byte units
    'You can use the above constants to minimize the amount of memory used,
    'but if you use the following constant,  you can be assured of having
    'enough memory for any of the five models:

    Public Const DEEP_ARG_T_PARAMS As Integer = 94
    Public Const N_SGP_PARAMS As Integer = 11
    Public Const N_SGP4_PARAMS As Integer = 30
    Public Const N_SGP8_PARAMS As Integer = 25
    Public Const N_SAT_PARAMS = (11 + DEEP_ARG_T_PARAMS)

    'Byte 63 of the first line of a TLE contains the ephemeris type.  The
    'following five values are recommended,  but it seems the non-zero
    'values are only used internally;  "published" TLEs all have type 0.

    Public Const TLE_EPHEMERIS_TYPE_DEFAULT As Integer = 0
    Public Const TLE_EPHEMERIS_TYPE_SGP As Integer = 1
    Public Const TLE_EPHEMERIS_TYPE_SGP4 As Integer = 2
    Public Const TLE_EPHEMERIS_TYPE_SDP4 As Integer = 3
    Public Const TLE_EPHEMERIS_TYPE_SGP8 As Integer = 4
    Public Const TLE_EPHEMERIS_TYPE_SDP8 As Integer = 5
    Public Const SXPX_DPSEC_INTEGRATION_ORDER As Integer = 0
    Public Const SXPX_DUNDEE_COMPLIANCE As Integer = 1
    Public Const SXPX_ZERO_PERTURBATIONS_AT_EPOCH As Integer = 2

    'SDP4 and SGP4 can return zero,  or any of the following error/warning codes.
    'The 'warnings' result in a mathematically reasonable value being returned,
    'and perigee within the earth is completely reasonable for an object that's
    'just left the earth or is about to hit it.  The 'errors' mean that no
    'reasonable position/velocity was determined.
    Public Const SXPX_ERR_NEARLY_PARABOLIC As Integer = -1
    Public Const SXPX_ERR_NEGATIVE_MAJOR_AXIS As Integer = -2
    Public Const SXPX_WARN_ORBIT_WITHIN_EARTH As Integer = -3
    Public Const SXPX_WARN_PERIGEE_WITHIN_EARTH As Integer = -4
    Public Const SXPX_ERR_NEGATIVE_XN As Integer = -5

    ' master struct to hold all info about a particular satellite
    Public Const SGP4M = 1
    Public Const SGP8M = 2
    Public Const SDP4M = 3
    Public Const SDP8M = 4

    ' unknown satellite magnitude
    Public Const SATDEFMAG = 6

End Class