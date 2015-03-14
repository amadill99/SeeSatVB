// -----------------------------------------------------------------------------------------------
//
// These headers were rmoved form the source code I got from http://www.projectpluto.com/sat_code.htm
//
// This file is part of a free package distributed by Dr TS Kelso, tkelso@grove.net in his web site
// at http://www.grove.net/~tkelso/
//
// This system carries two-line orbital data (*.tle files with the master list being tle.new) 
// and the following key programs:
//
//* spacetrk.zip (LaTeX documentation and FORTRAN source code for the NORAD
//                              orbital models needed for using the two-line element sets),
//* sat-db13.zip (a complete database of all payloads launched into orbit),
//* sgp4-pl2.zip (a library of Turbo Pascal units to implement SGP4/SDP4),
//* trakstr2.zip (an implementation of sgp4-pl2 to output satellite ECI
//                              position, subpoint, look angles, and right ascension/declination),
//* passupdt.zip (a package for quickly and easily updating two-line element
//                              sets), and
//* unzip (a Sun executable for unzipping these programs).
//
// More information on the first five files is available in the file master.dir.
//
//
//- Dr TS Kelso, tkelso@grove.net
//      Adjunct Professor of Space Operations
//      Air Force Institute of Technology
//      SYSOP, Celestial WWW
//                                     http://www.grove.net/~tkelso/
//
// -----------------------------------------------------------------------------------------------

#ifndef NORAD_IN_H
#define NORAD_IN_H

/* Common "internal" arguments between deep-space functions;  users of  */
/* the satellite routines shouldn't need to bother with any of this     */

typedef struct
{
  double
  /* Common between SGP4 and SDP4: */
  aodp, cosio, sinio, omgdot, xmdot, xnodot, xnodp,
  /* Used by dpinit part of Deep() */
  eosq, betao, theta2, sing, cosg, betao2,

  /* Used by dpsec and dpper parts of Deep() */
  xll, omgadf, xnode, em, xinc, xn, t,

       /* 'd####' secular coeffs for 12-hour, e>.5 orbits: */
   d2201, d2211, d3210, d3222, d4410, d4422, d5220, d5232, d5421, d5433,
      /* formerly static to Deep( ),   but more logically part of this struct: */
   atime, del1, del2, del3, e3, ee2, omegaq, pe, pgh, ph, pinc, pl, preep,
   savtsn, se2, se3, sgh2, sgh3, sgh4, sh2, sh3, si2, si3, sl2, sl3,
   sl4, sse, ssg, ssh, ssi, ssl, thgr, xfact, xgh2, xgh3, xgh4, xh2,
   xh3, xi2, xi3, xl2, xl3, xl4, xlamo, xli, xni, xnq, xqncl, zcosgl,
   zcoshl, zcosil, zmol, zmos, zsingl, zsinhl, zsinil;

         /* Epoch offsets,  described by Rob Matson,  added by BJG, */
         /* then commented out;  I don't think they really ought to */
         /* be used... */
#ifdef RETAIN_PERTURBATION_VALUES_AT_EPOCH
    double pe0, pinc0, pl0, pgh0, ph0;
    int solar_lunar_init_flag;
#endif
    int resonance_flag, synchronous_flag;
} deep_arg_t;

double FMod2p( const double x);
void Deep_dpinit( const tle_t *tle, deep_arg_t *deep_arg);
void Deep_dpsec( const tle_t *tle, deep_arg_t *deep_arg);
void Deep_dpper( const tle_t *tle, deep_arg_t *deep_arg);

int sxpx_posn_vel( const double xnode, const double a, const double e,
      const double *params, const double cosio, const double sinio,
      const double xincl, const double omega,
      const double xl, double *pos, double *vel);

typedef struct
{
   double coef, coef1, tsi, s4, unused_a3ovk2, eta;
} init_t;

void sxpx_common_init(double *params, const tle_t *tle,
                                  init_t *init, deep_arg_t *deep_arg);

/* Table of constant values */
#define pi               3.141592653589793238462643383279502884197
#define twopi            (pi*2.)
#define e6a              1.0E-6
#define two_thirds       (2. / 3.)
#define xj3             -2.53881E-6
#define minus_xj3        2.53881E-6
#define earth_radius_in_km           6378.135
#define minutes_per_day  1440.
#define ae                  1.0
#define xj2                 1.082616e-3
#define ck2                 (.5 * xj2 * ae * ae)

#ifdef OLD_CONSTANTS
#define ck4      6.209887E-7
#define s        1.012229
#define qoms2t   1.880279E-09
#define xke      7.43669161E-2
#else
#define xj4      (-1.65597e-6)
#define ck4      (-.375 * xj4 * ae * ae * ae * ae)
//#define ck4    6.2098875E-7
#define s        (ae * (1. + 78. / earth_radius_in_km))
#define qoms2t   1.880279159015270643865e-9
#define xke      0.0743669161331734132
#endif

#define a3ovk2   (minus_xj3/ck2*ae*ae*ae)

#endif         /* #ifndef NORAD_IN_H */
