// if using a def file for exports uncomment the following
#define USE_DEF_FILE

#ifdef _WIN32
#define DLL_FUNC __stdcall
//#define DLL_FUNC __cdecl

// noradstub.h defines importing
#ifndef IMPORTING
#define _EXPORTING
#endif

#ifdef _EXPORTING

#ifdef USE_DEF_FILE
#define DLL_Export
#else
#define DLL_Export __declspec(dllexport)
#endif //USE_DEF_FILE

#else
#define DLL_Export __declspec(dllimport)
//#define DLL_Export
#endif //_EXPORTING

#else
#define DLL_FUNC
#endif //WIN32

#ifdef __cplusplus
extern "C" {
#endif  //_cplusplus

DLL_Export void DLL_FUNC lat_alt_to_parallax(const double lat, const double ht_in_meters,
                    double *rho_cos_phi, double *rho_sin_phi);
DLL_Export void DLL_FUNC observer_cartesian_coords(const double jd, const double lon,
              const double rho_cos_phi, const double rho_sin_phi,
              double *vect);
DLL_Export void DLL_FUNC get_satellite_ra_dec_delta(const double *observer_loc,
                                 const double *satellite_loc, double *ra,
                                 double *dec, double *delta);
DLL_Export void DLL_FUNC epoch_of_date_to_j2000(const double jd, double *ra, double *dec);

#ifdef __cplusplus
}                       /* end of 'extern "C"' section */
#endif
