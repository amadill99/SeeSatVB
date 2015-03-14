// attempt to build a wrapper for the c routines in the dll
#include "stdafx.h"
#include <stdexcept>
#include <unordered_map> 
#include "norad.h"
#include "dispatch.h"

using namespace std;

namespace SGP4space
{

	// dispatch function for SGPX_init
	//void SGP4class::SGPX_init(const tle_t *tle)
	void SGP4class::SGPX_init(double *params, const tle_t *tle, const int model)
	{
		if (model == SGP4M)
			SGP4_init(params, tle);
		/*else if (model == SGP8M)
		SGP8(tsince, tle, params, pos, vel);*/
		else if (model == SDP4M)
			SDP4_init(params, tle);
		/*else if (model == SDP8M)
		SDP8(tsince, tle, params, pos, vel);*/
		
	}

	// dispatch function for SGPX
	int SGP4class::SGPX(const double tsince, const tle_t *tle, const double *params, double *pos, double *vel, const int model)
	{

		if (model == SGP4M)
			SGP4(tsince, tle, params, pos, vel);
		/*else if (model == SGP8M)
			SGP8(tsince, tle, params, pos, vel);*/
		else if (model == SDP4M)
			SDP4(tsince, tle, params, pos, vel);
		/*else if (model == SDP8M)
			SDP8(tsince, tle, params, pos, vel);*/
		else return -1;
		
		return 0;
	}


	// test function to test tle structure
	//int SGP4class::tle_test(double *params, tle_t *tle, LPSTR * model)
	int SGP4class::tle_test(double *params, tle_t *tle, int model)
	{
		//memcpy(tle->intl_desig, model, (size_t) sizeof(tle->intl_desig));

		params[0] = 1; params[2] = 2;

		return 0;
	}

	/* Selects the type of ephemeris to be used (SGP*-SDP*) - returns flag
	1	- it should be a deep-space (SDPx) ephemeris
	0	- you can go with an SGPx ephemeris
	-1	- error in input data*/
	void SGP4class::select_ephemeris_cpp(int *flag, const tle_t *tle)
	{
				
		*flag = select_ephemeris(tle);
		
	}

	/* Takes the Line1 and Line2 TLE and parses them and fills in the TLE struct and returns flag
		 parse_elements returns:
         0 if the elements are parsed without error;
         1 if they're OK except the first line has a checksum error;
         2 if they're OK except the second line has a checksum error;
         3 if they're OK except both lines have checksum errors;
		 a negative value if the lines aren't at all parseable */
	void SGP4class::parse_elements_cpp(int *flag, const char *line1, const char *line2, tle_t *sat)
	{
		
		*flag = parse_elements(line1, line2, sat);

	}
}


