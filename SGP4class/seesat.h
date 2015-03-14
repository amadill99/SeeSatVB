/*
SEESAT.H
by Paul S. Hirose, Edwards AFB, Calif.
Declarations & definitions for globals in the SEESAT satellite tracking
program.
modified Alan Madill 03/03/90
*/

/* printed out at program startup */
#define VERS "7 May 1989"

//#include <ctype.h>
//#include <stdio.h>
//#include <math.h>

/* #defining MAIN before #including this header will generate
definitions.  If MAIN has not been #defined, only declarations will be
generated. */
//#undef MAIN
#ifdef MAIN     /* generate DEFINITIONS */
#define CLASS
#define EQUALS(N) = N
#define DRIVER

#else           /* generate DECLARATIONS */
#define CLASS extern
#define EQUALS(N)
#endif

// if using a def file for exports uncomment the following
//#define USE_DEF_FILE
#define DLL_FUNC __stdcall
#define _EXPORTING
#define DLL_Export __declspec(dllexport)


/*####################################################################
            MISC DEPENDENCIES
####################################################################*/

/* args to fopen() a file for ASCII read or write */
#define R "r"
#define W "w"

/* Some functions may be declared type "void".  If your compiler doesn't
support this keyword, "typedef int void;" or "#define void int" can be
inserted here to fix the problem. */

/* POW(x, y) raises x to the y power. */
#define POW(x, y) pow(x, y)
//extern double pow();

//extern double dint();

/* dint(3.6) == 3.0, dint(-3.6) == -3.0.
If your library has floor() and ceil() instead of dint(), retain the
preceding dint() declaration and un-comment the following code: */

/*
#ifdef MAIN

double dint(x)
double x;
{
    if (x >= 0) return floor(x);
    return ceil(x);
}
#endif
*/

/*####################################################################
            Mathematical Constants
####################################################################*/

CLASS double
	PI EQUALS(3.141592653589793238462643383279502884197),
    E6A EQUALS(1e-6),

    /* initialized by init() at program startup: */
	PIO2 EQUALS(PI / 2.0),       /* pi/2 */
    TOTHRD EQUALS(2.0 / 3.0),     /* 2/3 */
	TWOPI EQUALS(PI * 2.0),      /* 2pi */
	X3PIO2 EQUALS(3.0 * PI / 2.0),     /* 3pi/2 */
	DE2RA EQUALS(PI / 180.0),      /* radians per degree */
	RA2DE EQUALS(180.0 / PI),      /* deg per radian */
	XJ3 EQUALS(-.253881e-5),
	XJ4 EQUALS(-1.65597e-6);   /* 3rd & 4th gravitational zonal harmonic */

    /* XJ3 & XJ4 belong with the physical constants, but I initialize them
    at run-time because my compiler (Ecosoft C v. 3.5 for CP/M) won't take
    negative floating point initializers */


/*####################################################################
            Physical Constants
####################################################################*/

/* dimensions & gravity of earth, World Geodetic System 1972 values */

CLASS double
XJ2 EQUALS(1.082616e-3),    /* 2nd gravitational zonal harmonic */
                /* XJ3 & XJ4 are with the math constants */
XKE EQUALS(.743669161e-1),
XKMPER EQUALS(6378.135),    /* equatorial earth radius, km */
MEAN_R EQUALS(.998882406),  /* mean radius, in units of XKMPER */

/* misc */
XMNPDA EQUALS(1440.0),  /* time units/day */
AE EQUALS(1.0),     /* distance units, earth radii */
SIDSOL EQUALS(1.002737908); /* sidereal/solar time ratio */


/*####################################################################
            Units & Conventions
####################################################################*/
/*
Unless the otherwise indicated, throughout this program
quantities are measured in the following units:

time (interval)     minutes
time (epoch)        minutes since 4713 B.C.
angle           radians
length          equatorial earth radii (1 unit = XKMPER km)

South declinations & south latitudes are negative.
Elevations below the horizon are negative.
East longitude is positive, west negative.
Azimuth is measured starting at north, increases as one turns
eastward, and 0 <= azimuth < 2pi radians.
*/


/*####################################################################
                Structures
####################################################################*/

/* rectangular coordinates */

typedef struct { 
	double x, y, z; 
} xyz_t;

/* data for observer's location */
typedef struct {
    double
    r,      /* altitude above sea level */
    lambda,     /* longitude */
    sinlat, coslat, /* latitude sin & cos */
    zg,     /* north distance from observatory to
            equatorial plane */
    xc;     /* distance between observatory & polar
            axis of earth */
} obs_t;

/* Spherical coordinates.  R is used for a linear measurement (e.g.,
radius), phi for an angle measured with respect to the principal
plane (e.g., latitude), and lambda for an angle measured IN the
principal plane (e.g., longitude). */

typedef struct { 
	double r, phi, lambda; 
} sph_t;

/*####################################################################
                Data
####################################################################*/

/* The satellite's mean orbital elements.  These are read from the
orbital element file, and remain constant during program execution. */
CLASS double
    xmo,    /* mean anomaly */
    xnodeo, /* right ascension of ascending node */
    omegao, /* argument of the perigee */
    eo, /* eccentricity */
    xincl,  /* inclination */
    xno,    /* mean motion, radians/min */
    xndt2o, /* 1st time derivative of mean motion, or ballistic
        coefficient (depending on ephemeris type) */
    xndd6o, /* 2nd time derivative of mean motion */
    bstar,  /* BSTAR drag term if GP4 theory was used;
        otherwise, radiation pressure coefficient */
    epoch;  /* epoch of mean elements */

CLASS char name[13];    /* of satellite - also from element file */

CLASS int  mgntd;       /* magnitude of sat */

/* Set by thetag(), used only in thetag() & deep space initialization. */
CLASS double ds50;

/* satellite geocentric coordinates output by sgp4() */
CLASS xyz_t sat;

/* satellite topocentric coordinates.  These all come from xyztop(). */
CLASS sph_t
    azel,   /* slant range, azimuth, elevation */
    radec,  /* slant range, R.A., dec. */
    latlon; /* altitude, latitude, longitude */

/* elevation (rounded to the nearest int) of center of sun above the
satellite's horizon.  Adjusted for dip due to height of satellite, but no
allowance for refraction.  Value comes from xyztop(). */
CLASS int elsusa;

/* Constants set at program startup by init(), and used by sgp4() */
CLASS double qoms2t, s, ck2, ck4;


// c style functions in c++

extern "C" {
	/*####################################################################
					Functions
					####################################################################*/

	/* module READEL:  gets orbital elements from file */

	DLL_Export int DLL_FUNC getel(FILE *fp); /* (fp)
	FILE *fp;

	Loads the mean orbital element variables with data from fp.  Initializes
	precession by calling inpre() with the epoch of the orbital elements.
	Returns 1 if period of satellite >= 225 minutes, 0 if period < 225 min., -1
	if error (e.g., checksum or format bad). */

	/*==================================================================*/

	/* module ASTRO:  astronomical time, coordinate transformation */
	/* ASTROGR.C */
	DLL_Export double DLL_FUNC julday(int y, int m, int d);
	/* (year, month, day)
	int year, month, day;
	Returns the Julian Day (measured in days, not minutes) at 0h UT on the
	given date, Gregorian calendar.  No error checking for illegal dates. */

	DLL_Export  int DLL_FUNC xyztop(int *iflag2, double tp);
	/* (iflag2, tp)
	int *iflag2;
	double tp;      epoch, used to compute the sun's position

	Satellite elevation is computed, based on coordinates in struct sat.
	If elevation is less than zero, xyztop() immediately returns 0; in
	this case, elsusa, radec, azel, and latlon are undefined.  On the
	other hand, if elevation is 0 or more, 1 is returned and the output
	variables are filled in.

	If *iflag2 == 1, xyztop() will assume that subsequent calls will have tp at
	equally spaced intervals.  This saves the trouble of recomputing sidereal time
	from scratch with each call.  *iflag2 is reset to 0 on second call. */

	DLL_Export  int DLL_FUNC dusk(double ep);
	/* (ep)
	double ep;      epoch
	Returns elevation of sun (in degrees) at observer. */

	DLL_Export  int DLL_FUNC parall(double t);
	/* (ep)
	double ep;          epoch
	Returns the parallactic angle in degrees: the angle on the celestial sphere
	whose vertex is the satellite, initial side contains the north pole, and
	terminal side contains the zenith of the observer.  Angle increases east. */

	DLL_Export  double DLL_FUNC topos(void);
	/* no args
	Responsible for filling in all data in struct "obs" in module ASTRO.  Must be
	called when SEESAT is started up, before any predictions are run.  Returns
	zone description (local time - Greenwich time) in minutes. */

	DLL_Export  void DLL_FUNC inpre(double epoch);
	/* (ep)
	double ep;
	Sets up struct "dircos" (direction cosines) in module ASTRO to precess
	satellite R.A/dec.  Initial epoch will be "ep" and final epoch is
	determined by REFEP in ASTRO. */

	DLL_Export  double DLL_FUNC fmod2p(double x);
	/* (x)
	double x;
	Returns x, such that 0 <= x < 2pi, i.e., takes out multiples of 2pi. */

	DLL_Export  double DLL_FUNC thetag(double ep);
	/* (ep)
	Returns Greenwich hour angle of Aries at epoch ep.
	Side effect: sets ds50 to minutes between 1950 Jan 0 0h UT & ep. */

	DLL_Export  void DLL_FUNC zrot(xyz_t *cor, double sint, double cost);
	DLL_Export  void DLL_FUNC yrot(xyz_t *cor, double sint, double cost);
	DLL_Export  void DLL_FUNC preces(xyz_t *cor);
	DLL_Export  void DLL_FUNC sunref(xyz_t *pos, double ep);
	DLL_Export  double DLL_FUNC sunlon(double epoch);
	DLL_Export  void DLL_FUNC sun(xyz_t *pos, double ep);

	/*==================================================================*/

} // end of extern C