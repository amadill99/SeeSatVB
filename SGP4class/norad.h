/*
 *  norad.h v. 01.beta 03/17/2001
 *
 *  Header file for norad.c
 */
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


#ifndef NORAD_H
#define NORAD_H 1
#endif

/* #define RETAIN_PERTURBATION_VALUES_AT_EPOCH 1 */

/* Two-line-element satellite orbital data */
typedef struct
{
  double epoch, xndt2o, xndd6o, bstar;
  double xincl, xnodeo, eo, omegao, xmo, xno;
  int norad_number, bulletin_number, revolution_number;
  char classification;    /* "U" = unclassified;  only type I've seen */
  char ephemeris_type;
  char *intl_desig;
} tle_t;

   /* NOTE: xndt2o and xndt6o are used only in the "classic" SGP, */
   /* not in SxP4 or SxP8. */
   /* xmo = mean anomaly at epoch */
   /* xno = mean motion at epoch */

#define DEEP_ARG_T_PARAMS     94

#define N_SGP_PARAMS          11
#define N_SGP4_PARAMS         30
#define N_SGP8_PARAMS         25
#define N_SDP4_PARAMS        (10 + DEEP_ARG_T_PARAMS)
#define N_SDP8_PARAMS        (11 + DEEP_ARG_T_PARAMS)

/* 94 = maximum possible size of the 'deep_arg_t' structure,  in 8-byte units */
/* You can use the above constants to minimize the amount of memory used,
   but if you use the following constant,  you can be assured of having
   enough memory for any of the five models: */

#define N_SAT_PARAMS         (11 + DEEP_ARG_T_PARAMS)

/* Byte 63 of the first line of a TLE contains the ephemeris type.  The */
/* following five values are recommended,  but it seems the non-zero    */
/* values are only used internally;  "published" TLEs all have type 0.  */

#define TLE_EPHEMERIS_TYPE_DEFAULT           0
#define TLE_EPHEMERIS_TYPE_SGP               1
#define TLE_EPHEMERIS_TYPE_SGP4              2
#define TLE_EPHEMERIS_TYPE_SDP4              3
#define TLE_EPHEMERIS_TYPE_SGP8              4
#define TLE_EPHEMERIS_TYPE_SDP8              5

#define SXPX_DPSEC_INTEGRATION_ORDER         0
#define SXPX_DUNDEE_COMPLIANCE               1
#define SXPX_ZERO_PERTURBATIONS_AT_EPOCH     2


/* SDP4 and SGP4 can return zero,  or any of the following error/warning codes.
The 'warnings' result in a mathematically reasonable value being returned,
and perigee within the earth is completely reasonable for an object that's
just left the earth or is about to hit it.  The 'errors' mean that no
reasonable position/velocity was determined.       */

#define SXPX_ERR_NEARLY_PARABOLIC         -1
#define SXPX_ERR_NEGATIVE_MAJOR_AXIS      -2
#define SXPX_WARN_ORBIT_WITHIN_EARTH      -3
#define SXPX_WARN_PERIGEE_WITHIN_EARTH    -4
#define SXPX_ERR_NEGATIVE_XN              -5

/* Function prototypes */
/* norad.c */

         /* The Win32 version can be compiled to make a .DLL,  if the     */
         /* functions are declared to be of type __stdcall... _and_ the   */
         /* functions must be declared to be extern "C",  something I     */
         /* overlooked and added 24 Sep 2002.  The DLL_FUNC macro lets    */
         /* this coexist peacefully with other OSes.                      */

// if using a def file for exports uncomment the following
//#define USE_DEF_FILE
#define DLL_FUNC __stdcall
#define _EXPORTING
#define DLL_Export __declspec(dllexport)

extern "C" {

	/*generic common code*/
DLL_Export void DLL_FUNC SGP_init(double *params, const tle_t *tle);
DLL_Export void DLL_FUNC SGP(const double tsince, const tle_t *tle, const double *params,
                                     double *pos, double *vel);
/*low orbit routines*/
DLL_Export void DLL_FUNC SGP4_init(double *params, const tle_t *tle);
DLL_Export int  DLL_FUNC SGP4(const double tsince, const tle_t *tle, const double *params,
                                     double *pos, double *vel);

/*deep space routines*/
DLL_Export void DLL_FUNC SDP4_init(double *params, const tle_t *tle);
DLL_Export int  DLL_FUNC SDP4(const double tsince, const tle_t *tle, const double *params,
									double *pos, double *vel);

/* Selects the type of ephemeris to be used (SGP*-SDP*) - returns
   1 - it should be a deep-space (SDPx) ephemeris 
   0 - you can go with an SGPx ephemeris 
   -1 - error in input data*/
DLL_Export int DLL_FUNC select_ephemeris(const tle_t *tle);
DLL_Export int DLL_FUNC parse_elements(const char *line1, const char *line2, tle_t *sat);

DLL_Export int DLL_FUNC tle_checksum(const char *buff);
DLL_Export void DLL_FUNC write_elements_in_tle_format(char *buff, const tle_t *tle);

DLL_Export void DLL_FUNC sxpx_set_implementation_param(const int param_index,
                                              const int new_param);
DLL_Export void DLL_FUNC sxpx_set_dpsec_integration_step(const double new_step_size);

DLL_Export long DLL_FUNC sxpx_library_version(void);

}                       // end of 'extern "C"' section
