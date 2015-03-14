#define DLL_Export __declspec(dllexport)

#define SGP4M  1
#define SGP8M  2
#define SDP4M  3
#define SDP8M  4

namespace SGP4space
{
	// This class is exported from the SGP4class.dll
	class SGP4class
	{
	public:
		
		// dispatch function for SGPX_init
		static DLL_Export void SGPX_init(double *params, const tle_t *tle, const int model);
		//static DLL_Export void SGPX_init(const tle_t *tle);

		// dispatch function for SGPX
		static DLL_Export int SGPX(const double tsince, const tle_t *tle, const double *params, double *pos, double *vel, const int model);

		// test function to see if tle struct is being passed correctly
		static DLL_Export int tle_test(double *params, tle_t *tle, int model);

		// wrapper for select_ephemeris
		/* Selects the type of ephemeris to be used (SGP*-SDP*) - returns flag
		 1	- it should be a deep-space (SDPx) ephemeris
		 0	- you can go with an SGPx ephemeris
		-1	- error in input data*/
		static DLL_Export void select_ephemeris_cpp(int *flag, const tle_t *tle);
		
		/* Takes the Line1 and Line2 TLE and parses them and fills in the TLE struct and returns flag
		parse_elements returns:
		0 if the elements are parsed without error;
		1 if they're OK except the first line has a checksum error;
		2 if they're OK except the second line has a checksum error;
		3 if they're OK except both lines have checksum errors;
		a negative value if the lines aren't at all parseable */
		static DLL_Export void parse_elements_cpp(int *flag, const char *line1, const char *line2, tle_t *sat);
	};
}
