// *******************************************************************
// Startup und Init Funktionen der Level-zen-files
// -----------------------------------------------
// Die STARTUP-Funktionen werden NUR beim ersten Betreten eines Levels 
// (nach NewGame) aufgerufen, die INIT-Funktionen jedesmal
// Die Funktionen müssen so heissen wie die zen-files
// *******************************************************************

// *********
// GLOBAL
// *********

func void STARTUP_GLOBAL()
{
	// wird fuer jede Welt aufgerufen (vor STARTUP_<LevelName>)
	Game_InitGerman();
};

func void INIT_GLOBAL()
{
	// wird fuer jede Welt aufgerufen (vor INIT_<LevelName>)
	Game_InitGerman();
};


FUNC VOID STARTUP_NewWorld()
{	

};
FUNC VOID INIT_NewWorld()
{

};

func void STARTUP_Testlevel ()
{

};

	func void INIT_SUB_Testlevel ()
	{
	};
	
	
	FUNC VOID STARTUP_AddonWorld ()
{	

};
FUNC VOID INIT_AddonWorld ()
{

};