/*
ASTROGR.C
7 May 1989 by Paul S. Hirose
Coordinate transformation and astronomical functions for SEESAT satellite
tracking program.

Breakpoints 200 - 299 reserved for this module
Modified 02/2015 Alan Madill
*/

#include "stdafx.h"
#include <string.h>
#include <math.h>
#define MAIN

#include "seesat.h" /* global header */

//#include "functgr.h"    /* our appologies */

/* library functions:
    asin atan atan2 cos fabs gprintf sin sqrt tan
are used in this module. */

/* library function declarations */
//extern double asin(), atan(), atan2(), cos(), fabs(), sin(), sqrt(), tan();

/* default values for observer's position 
   changed to Vanderhoof supposed center of B. C.*/
#define D_ALT "2296.0"       /* FEET above sea level */
#define D_LAT "54.00563"     /* north latitude (54 00.350' N) */
#define D_LON "-123.97506"   /* east longitude (123 58.505' W) */
#define D_TZ  "8"            /* PDT -Greenwich - local time, hours */

/* REFEP is the epoch to which R.A./dec. will be precessed.
E.g. (per Meeus p. 101):
3477629251. = epoch 1900.0, 3503926689. = 1950.0, 3530224800. = 2000.0.
EPMSG is part of the message (printed at startup) which states what epoch
will be used for R.A./dec.  Be sure REFEP & EPMSG agree. */

#define REFEP 3530224800.
#define EPMSG "2000.0"

/* normally 0.  Non-zero will make static functions extern, and alter the
operation of parall(). */
#define TEST 1

/*######################## LOCAL FUNCTIONS ########################*/

#if TEST
#define STATIC      /* precedes static func definitions */
#define SC extern   /* precedes static func declarations */

#else
#define STATIC static
#define SC static
#endif

SC void zrot(), yrot();     /* z- & y-rotation */
SC void preces();       /* corrects coordinates for precession */
SC void sunref();       /* xyz of sun (non-interpol.) */
SC double sunlon();     /* true longitude of sun */
SC void sun();          /* xyz of sun (interpol.) */

/*########################### LOCAL DATA ###########################*/

/* direction cosines, i.e., a rotation matrix.  Initialized by inpre() & used
by preces() to correct satellite Right Ascension & declination for precession
of earth's polar axis */
STATIC struct {
    double xx, xy, xz, yx, yy, yz, zx, zy, zz;
} dircos;

/* data for observer's location */

obs_t obs;
double  localhra,
        phase;

/*############################## CODE ##############################*/
double DLL_FUNC  dint(double x)
{
	if (x >= 0) return floor(x);
	return ceil(x);
}

double DLL_FUNC julday(int y, int m, int d) /* year, month, day */
{   
	static int difm[] = {0, 0, 31, 59, 90, 120, 151,
            181, 212, 243, 273, 304, 334};

    if (y%4 == 0 && y%100 || y%400 == 0)    /* leap year */
        if (m > 2)  /* after Feb */
            ++d;    /* Feb had an extra day */

    if (y >= 0)
		--y;

    return (dint(y * 365.25) - dint(y * .01) + dint(y * .0025) + difm[m] + d + 1721424.5);

	// try this a different way from http://scienceworld.wolfram.com/astronomy/JulianDate.html
	// both ways work correctly if you pass the day of month instead of the day of year phhhht :-(

	//return (double)(367. * y - floor(7. * (y + floor((m + 9.) / 12.)) / 4.) + floor(275. * m / 9.) + d + 1721013.5);
}

/*==================================================================*/

int DLL_FUNC xyztop(int *iflag2, double tp)	/* *iflag2 TRUE for 1st or 2nd call in a run */
{
    extern double
        localhra, /* needed for stars.c */
        phase;
	xyz_t
		g,       /* geocentric satellite position */
			l, lh,      /* topocentric (equatorial & horizontal, */
			ls,         /*   respectively) satellite position */
			suneq,      /* equatorial sun position */
			sunl;       /* to determine phase angle */
	
    double  rho,        /* distance to satellite */
        temp,           /* scratchpad */
        sinlha, coslha; /* sin & cos of lhaa */
    static double
        lhaa,       /* local hr angle of Aries */
        dlhaa;      /* delta lhaa between calls */
    static int i;       /* signals 2nd call in a prediction run */

    if (*iflag2) {      /* 1st call */
        *iflag2 = 0;
        i = 1;
        lhaa = thetag(tp) + obs.lambda;
        dlhaa = 0.;
    } else if (i) {     /* 2nd call */
        i = 0;
        dlhaa = thetag(tp) + obs.lambda - lhaa;
    } lhaa += dlhaa;
    sinlha = sin(lhaa);
    coslha = cos(lhaa);
    localhra = lhaa;

    /* Generate topocentric equatorial satellite coordinates by adding the
    topocentric coordinates of the geocenter to the geocentric satellite
    position. */
    ls.x = lh.x = l.x = sat.x - obs.xc * coslha;
    ls.y = lh.y = l.y = sat.y - obs.xc * sinlha;
    ls.z = lh.z = l.z = sat.z + obs.zg;
    rho = sqrt(l.x*l.x + l.y*l.y + l.z*l.z);

    /* if to far away (2 earth radi) to be visible then return.  If it is close 
    to the horizon it should get checked again soon. */

#define MAXDIST 2.0

    if (rho>MAXDIST)
        return 0;

#undef  MAXDIST

    /* The lh struct will become a horizontal coordinate system, with
    x, y, and z pointing to south, east, and zenith, respectively.
    To accomplish this, I'll z-rotate it so the negative x-axis
    intersects earth's polar axis, then y-rotate it so the positive
    z-axis points to the zenith.  The required y-rotation is the
    complement of the latitude; we can get it by reversing the sine
    and cosine arguments to yrot(). */
    zrot(&lh, sinlha, coslha);
    yrot(&lh, obs.coslat, obs.sinlat);

    /*  If elevation < 0, the satellite's coordinates are of no
    interest, so return immediately with a value of zero.  Otherwise,
    compute azimuth.  Sign of x is reversed so az starts at north and
    increases east. */
    if (lh.z < 0.)
        return 0;
    azel.phi = asin(lh.z / rho);
    azel.lambda = fmod2p(atan2(lh.y, -lh.x));
    azel.r = rho;

    /* Precess the topocentric equatorial coordinates to epoch REFEP,
    compute right ascension & declination. */
    preces(&l);
    radec.r = rho;
    radec.phi = asin(l.z / rho);
    radec.lambda = fmod2p(atan2(l.y, l.x));

    /* altitude, latitude, longitude */
    rho = sqrt(sat.x*sat.x + sat.y*sat.y + sat.z*sat.z);
    g.x = sat.x / rho;
    g.y = sat.y / rho;
    g.z = sat.z / rho;
    latlon.r = rho - MEAN_R;
    latlon.phi = asin(g.z);
    temp = fmod2p(atan2(g.y, g.x) - lhaa + obs.lambda);
    if (temp > PI)
        temp -= TWOPI;      /* west lon. is negative */
    latlon.lambda = temp;

    /* Sun elevation at satellite.  Get sun position, rotate the
    axes so the satellite is on the positive z-axis.  Sun elevation =
    arc sin z.  Add dip of horizon due to height of satellite. */

    sun(&suneq, tp);    /* suneq = xyz sun position */

    /* copy sun coords */
    sunl.x = suneq.x;
    sunl.y = suneq.y;
    sunl.z = suneq.z;

    /* make ls into a unit vector */
    rho = sqrt(ls.x*ls.x + ls.y*ls.y + ls.z*ls.z);
    ls.x = ls.x / rho;
    ls.y = ls.y / rho;
    ls.z = ls.z / rho;

    /* do the rotation for the geocenter */
    temp = sqrt(1 - g.z * g.z);     /* dist. to z axis */
    zrot(&suneq, g.y / temp, g.x / temp);
    yrot(&suneq, temp, g.z);

    /* the same for the topocenter */
    temp = sqrt(1 - ls.z * ls.z);     /* dist. to z axis */
    zrot(&sunl, ls.y / temp, ls.x / temp);
    yrot(&sunl, temp, ls.z);
    phase = asin(sunl.z) + PIO2;

    temp = (asin(suneq.z) + atan(sqrt((2 + latlon.r) * latlon.r)))
     * RA2DE;

    /* ensure temp truncates correctly when converted to int */
    if (temp >= 0.)
        temp += .5;
    else
        temp -= .5;
    elsusa = (int)(temp);

    return 1;   /* signifies sat elev > 0 */
}

/*==================================================================*/

int DLL_FUNC dusk(double ep) /* epoch */
{
    double lst;     /* local sidereal time */
    double elev;        /* elevation of sun, deg */
    xyz_t csun;    /* coordinates of sun */

    lst = thetag(ep) + obs.lambda;
    sun(&csun, ep);     /* get sun's position */

    /* point the positive z-axis at zenith */
    zrot(&csun, sin(lst), cos(lst));
    yrot(&csun, obs.coslat, obs.sinlat);

    elev = asin(csun.z) * RA2DE;
    /* If desired, the sun's azimuth (deg) can be found here with the
    expression fmod2p(atan2(csun.y, -csun.x)) * RA2DE */

    /* ensure truncation occurs in correct direction */
    if (elev >= 0.)
        elev += .5;
    else
        elev -= .5;
    return ((int)(elev));
}

/*------------------------------------------------------------------*/

int DLL_FUNC  parall(double t)
{
    xyz_t zenith;
    double lst;
    int iflag, iflag2;

    iflag2 = iflag = 1;

#if (TEST == 0)
    /* Get R.A., dec. of satellite at time t.  In TEST mode, the values
    for struct radec are set by the test software */
    sgp4(&iflag, t - epoch);
    xyztop(&iflag2, t);
#endif

    lst = thetag(t) + obs.lambda;   /* local sidereal time */

    /* Load struct "zenith" with the topocentric equatorial xyz
    components of a unit vector directed from the observer to the
    zenith. */
    zenith.z = obs.sinlat;
    zenith.x = cos(lst) * obs.coslat;
    zenith.y = sin(lst) * obs.coslat;

    /* z-rotate the axes to bring the satellite into the x-z
    plane, such that the satellite has a positive x coordinate. 
    Bear in mind that struct "zenith" ALWAYS has the topocentric
    coordinates of the ZENITH.  We are rotating its axes into a
    particular relationship with the satellite. */

    zrot(&zenith, sin(radec.lambda), cos(radec.lambda));

    /* Satellite y-coordinate == 0.  x & y of north pole == 0.
    y-rotate so the positive z-axis passes through satellite. */
    yrot(&zenith, cos(radec.phi), sin(radec.phi));

    /* The z-axis passes through the satellite.  The north pole
    has a y-coordinate of 0 and a negative x-coordinate.  The
    origin of the coordinate system is still at the observer. */

    return ((int)(fmod2p(atan2(zenith.y, -zenith.x)) * RA2DE + .5));
}

/*==================================================================*/

double DLL_FUNC topos()
{
#define din(x) atof(x)

    double lat, C, S;
    static double f = 3.35278e-3;   /* WGS-72 value for flattening
                    of earth */

    char    d_alt[20], d_lat[20], d_lon[20], d_tz[20];

    strcpy_s( d_alt, D_ALT );
    strcpy_s( d_lat, D_LAT );
    strcpy_s( d_lon, D_LON );
    strcpy_s( d_tz, D_TZ );
    //getobs( d_alt, d_lat, d_lon, d_tz );

    //printf("R.A. & dec. will be epoch %s\n", EPMSG);

    //printf("OBSERVER'S LOCATION\n");
    //printf("---------- --------\n");
    //printf("ALTITUDE ABOVE SEA\nLEVEL, FEET\n");
    obs.r = din(d_alt) * 4.778826e-8;   /* ft->earth radii */
    //printf("NORTH LATITUDE\n");
    lat = din(d_lat) * DE2RA;
    //printf("EAST LONGITUDE\n");
    //obs.lambda = din(d_lon) * DE2RA;

    /* sin & cos of observer's latitude */
	obs.sinlat = sin(lat);
	obs.coslat = cos(lat);


    /* observer's position with respect to geocenter (formulas
    from Baker & Makemson) */
    C = 1. / sqrt(1. - (f + f - f * f) * obs.sinlat * obs.sinlat);
    S = C * (1. - f) * (1. - f);
    obs.xc = (C + obs.r) * obs.coslat;
    obs.zg = -(S + obs.r) * obs.sinlat;

    //printf("TIME ZONE OFFSET\n(GMT - LOCAL TIME)\nHOURS ");
    return din(d_tz) * 60.0;
	//return (-8.0 * 60.0);
}

/*==================================================================*/

void DLL_FUNC inpre(double epoch)
{
/* Set up struct "dircos" to precess from initial epoch "epoch" to final epoch
"REFEP".  Used in conjunction with preces().  Formulae are the "rigorous
method" given by Meeus, which he in turn attributes to Newcomb. */

    double
    temp, sinzet, coszet, sinz, cosz, sinthe, costhe, tau0, tau;

    static double trocen = 52594876.7;  /* min. per trop. century */

    tau0 = (epoch - 3477629251.) / trocen;
    tau = (REFEP - epoch) / trocen;

    temp = tau * ((1.1171319e-2 + 6.768e-6 * tau0) + tau * (1.464e-6 +
     tau * 8.7e-8));
    sinzet = sin(temp);
    coszet = cos(temp);

    temp = temp + tau * tau * (3.835e-6 + 5e-9 * tau);
    sinz = sin(temp);
    cosz = cos(temp);

    temp = tau * ((9.7189726e-3 - 4.135e-6 * tau0) + tau * (-2.065e-6 -
     -2.04e-7 * tau));
    sinthe = sin(temp);
    costhe = cos(temp);

    dircos.xx = coszet * cosz * costhe - sinzet * sinz;
    dircos.xy = sinzet * cosz + coszet * sinz * costhe;
    dircos.xz = coszet * sinthe;
    dircos.yx = -coszet * sinz - sinzet * cosz * costhe;
    dircos.yy = coszet * cosz - sinzet * sinz * costhe;
    dircos.yz = -sinzet * sinthe;
    dircos.zx = -cosz * sinthe;
    dircos.zy = -sinz * sinthe;
    dircos.zz = costhe;
}

/*==================================================================*/

double DLL_FUNC  fmod2p(double x)
{
    x /= TWOPI;
    x = (x - dint(x)) * TWOPI;
    if (x < 0.0)
        x += TWOPI;
    return x;
}

/*==================================================================*/

double DLL_FUNC  thetag(double ep)
/* Returns Greenwich hour angle of the mean equinox at ep.
As a side effect, sets ds50 to minutes since 1950 Jan 0 0h UT. */
{
    double theta, temp;
    int i;

    ds50 = (ep - 3503925360.) / XMNPDA;
    theta = 1.72944494 + 6.3003880987 * ds50;
    temp = (int)(theta/TWOPI);
    /* temp = i = (int)(temp);     drop fractional part of temp */
    return theta - temp * TWOPI;
}

/*==================================================================*/

STATIC void DLL_FUNC zrot(xyz_t *cor, double sint, double cost)
/* Z-rotates coordinate axes for "cor" by angle t, where "sint"
and "cost" are the sine and cosine of t. */

{   double tempx, tempy;

    tempx = cor->x * cost + cor->y * sint;
    tempy = cor->y * cost - cor->x * sint;
    cor->x = tempx;
    cor->y = tempy;
}

/*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

STATIC void DLL_FUNC yrot(xyz_t *cor, double sint, double cost)
/* similar to zrot(), except rotate about y axis */
{   double tempx, tempz;
    tempz = cor->z * cost + cor->x * sint;
    tempx = cor->x * cost - cor->z * sint;
    cor->x = tempx;
    cor->z = tempz;
}

/*==================================================================*/

STATIC void DLL_FUNC  preces(xyz_t *cor) /* structure to rotate */
{
/* Uses direction cosines "dircos" (external struct) to rotate axes of
struct "cor". */

    double tempx, tempy, tempz;
    tempx = cor->x * dircos.xx + cor->y * dircos.yx + cor->z * dircos.zx;
    tempy = cor->x * dircos.xy + cor->y * dircos.yy + cor->z * dircos.zy;
    tempz = cor->x * dircos.xz + cor->y * dircos.yz + cor->z * dircos.zz;
    cor->x = tempx;
    cor->y = tempy;
    cor->z = tempz;
}

/*==================================================================*/

STATIC void DLL_FUNC sunref(xyz_t *pos, double ep)
/* Put the mean equatorial xyz components of a unit vector to sun at
epoch "ep" into struct "pos".  Good to about .01 deg. */
{
    double lon, sinlon;

    /* sin & cos of obliq. of ecliptic.  Since the obliquity of the
    ecliptic changes but .013 deg/century, I use a constant value,
    that of 0h 16 Feb 1988 ET. */
    static double sineps = .397802, coseps = .917471;

    lon = sunlon(ep);
    sinlon = sin(lon);

    pos->x = cos(lon);
    pos->y = sinlon * coseps;
    pos->z = sinlon * sineps;
}

/*==================================================================*/

STATIC double DLL_FUNC sunlon(double epoch)
/* Return geometric mean longitude of sun at "epoch", accurate to
about .01 deg.  This is ample for our purpose.  Formulas from Meeus,
simplified to correspond to our required accuracy.  The returned value
may be off by a small multiple of 2pi.  That should cause no problem if
you have good sin() & cos(), since in this program the longitude is only
used by those functions. */
{
    double T, L, e, M, Enew, Eold, v;

    T = (epoch / XMNPDA - 2415020.) / 36525.;

    /* geometric mean long. */
    L = fmod2p((279.69668 + T * 36000.76892) * DE2RA);

    /* mean anomaly */
    M = fmod2p((358.47583 + T * 35999.04975) * DE2RA);

    /* eccentricity */
    e = .01675104 - T * 4.18e-5;

    /* solve Kepler's equation to .01 deg. */
    Enew = M;
    do {
        Eold = Enew;
        Enew = M + e * sin(Eold);
    } while (Enew - Eold >= 1.7e-4);

    /* true anomaly.  If the eccentric anomaly (Enew) is near pi, there
    will be trouble in the true anomaly formula since tan is asymptotic at
    pi/2.  So if eccentric anomaly is within .01 deg of pi radians, we'll
    just say v is pi.  Yes, e also affects v, but if Enew is near pi e's
    effect is negligible for our purposes. */
    if (fabs(Enew - PI) <= 1.7e-4)
        v = PI;
    else
        v = 2. * atan(sqrt((1. + e) / (1. - e)) * tan(Enew / 2.));

    return L + v - M;
}

/*==================================================================*/

STATIC void DLL_FUNC  sun(xyz_t *pos, double ep)
{
/* Equatorial xyz components of a unit vector to sun at epoch "ep" are
returned in struct "pos".  Accuracy about .1 deg. */

    static double t0,   /* start epoch of a 6-day interval */
        dx, dy, dz; /* change in x, y, z during 6 days */
    static xyz_t sun0; /* sun position at t0 */

    double t;   /* e.g. .5 if ep = t0 + 3 days */

    if ((t = (ep - t0) / 8640.) > 1.0  ||  t < 0.) {
    /* Position was requested for a time outside the current 6-day
    interval.  So set up a new 6-day interval centered at "ep". */
        xyz_t sun1;

        t0 = ep - 4320.;    /* 3 days before "ep" */
        sunref(&sun0, t0);
        sunref(&sun1, ep + 4320.);  /* 3 days after "ep" */
        dx = sun1.x - sun0.x;
        dy = sun1.y - sun0.y;
        dz = sun1.z - sun0.z;
        t = .5;
    }
    /* get position by linear interpolation in the 6-day period */
    pos->x = sun0.x + t * dx;
    pos->y = sun0.y + t * dy;
    pos->z = sun0.z + t * dz;
}


