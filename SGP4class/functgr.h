/*  functgr.h   - function definitions for seesatgr
        generated with /Zg option - Microsoft C V5.0
        cl /AL /Zg *.c >> functgr.out
*/

 /* SEESATGR.C */ 
extern  double dint(double x);
 /* SGP4GR.C */ 
extern  void sgp4(int *iflag,double tsince);
 /* STARGR.C */ 
extern  int loadstar(void );
extern  int initstar(void );
extern  int calcstar(int ndx);
 /* ASTROGR.C */ 
extern  double julday(int y,int m,int d);
extern  int xyztop(int *iflag2,double tp);
extern  int dusk(double ep);
extern  int parall(double t);
extern  double topos(void );
extern  void inpre(double epoch);
extern  double fmod2p(double x);
extern  double thetag(double ep);
static  void zrot(struct xyz *cor,double sint,double cost);
static  void yrot(struct xyz *cor,double sint,double cost);
static  void preces(struct xyz *cor);
static  void sunref(struct xyz *pos,double ep);
static  double sunlon(double epoch);
static  void sun(struct xyz *pos,double ep);
 /* DRIVE2GR.C */ 
static  char *s_in(char *prompt,char *buffer);
static  char *stoup(char *string);
extern  char * *tok(char * *argv);
extern  char * *tokm(char * *argv);
extern  double din(char *str);
extern  int prnval(double tph,int id);
extern  int prnval2(double tph,int id);
extern  double atomin(char *string);
extern  void tokdat(int *dmy,char *dest);
extern  void degdms(int pre,double x);
extern  char *dmstim(void );
extern  char *skip0(char *string);
extern  int *dday(char * * *pargv);
 /* PALETTE.C */ 
extern  int main(void );
 /* READELGR.C */ 
extern  int getel(struct _iobuf *fp,struct ORBELS *s);
static  int getlin(char *dest,struct _iobuf *fp,int size);
static  int csum(char *line);
static  double epjd(char *buf);
static  void nullsp(char *buf);
static  double angle(char *string);
static  char *sknull(char *cptr);
static  double sci(char *string);
 /* SCREENGR.C */ 
extern  int setgr(void );
extern  int setega(void );
extern  int setvga(void );
extern  int isdarkcolor(void );
extern  int issuncolor(void );
extern  int istarcolor(void );
extern  int ibackcolor(void );
extern  int itextcolor(void );
extern  int resetgr(void );
extern  int gprintf(char *buf,...);
extern  int egprintf(int cl,short line,char *buf,...);
extern  int gbackspace(void );
extern  int plotazel(int mag);
extern  int tclear(void );
extern  int gclear(void );
extern  int radial(double angle);
extern  int plotallstars(void );
extern  int teststar(void );
extern  int plotstar(int i);
 /* MAINGR.C */ 
extern  int main(int argc,char * *argv);
extern  int realtime(double zd);
extern  int predict(char * *argv,double zd);
extern  int allpredict(char * *argv,double zd);
extern  int compsat(char *pargv,struct ORBELS *onesat);
extern  int getsat(int i,char * *argv);
extern  int userkey2(void );
extern  int userkey(void );
extern  int helpme1(void );
extern  int helpmemain(void );
extern  int helpme2(void );
extern  int helpme3(void );
static  int load(char *name);
static  void setsat(int i);
static  void init(void );
static  double dosdate(int * *iarray);
static  double dostime(void );
static  double qtime(char * * *pargv,int * *iarray);
extern  void beep(unsigned int duration,unsigned int frequency);
extern  void delay(long wait);
