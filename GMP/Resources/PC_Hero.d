instance PC_Hero (NPC_DEFAULT)
{
	// ------ SC ------
	name 		= "Ich";
	guild		= GIL_NONE;
	id			= 0;
	voice		= 15;
	level		= 0;
	Npctype		= NPCTYPE_MAIN;
	
	//***************************************************
	bodyStateInterruptableOverride 	= TRUE;
	//***************************************************
	
	// ------ XP Setup ------
	exp				= 0;
	exp_next		= 500;
	lp				= 0;
	
	// ------ Attribute ------
	attribute[ATR_STRENGTH] 		= 0;
	attribute[ATR_DEXTERITY] 		= 0;
	attribute[ATR_MANA_MAX] 		= 0;
	attribute[ATR_MANA] 			= 0;
	attribute[ATR_HITPOINTS_MAX]	= 1;
	attribute[ATR_HITPOINTS] 		= 1;
	
	// ------ visuals ------
	//B_SetNpcVisual 		(self, MALE, "Hum_Head_Pony", FACE_N_Player, BodyTex_N, NO_ARMOR);
	
	Mdl_SetVisual (self,"HUMANS.MDS");
	// ------ Visual ------ "body_Mesh",		bodyTex			SkinColor	headMesh,			faceTex,		teethTex,	armorInstance	
	Mdl_SetVisualBody (self, "hum_body_Naked0", 9,				0,			"Hum_Head_Pony", 	FACE_N_Player,	0, 			NO_ARMOR);
	
	// ------ Kampf-Talente ------
	//B_SetFightSkills 	(self, 10); 
};

instance OTHERS_NPC (NPC_DEFAULT)
{
	// ------ SC ------
	name 		= "Ich";
	guild		= GIL_NONE;
	id			= 0;
	voice		= 15;
	level		= 0;
	Npctype		= NPCTYPE_MAIN;
	
	//***************************************************
	bodyStateInterruptableOverride 	= TRUE;
	//***************************************************
	
	// ------ XP Setup ------
	exp				= 0;
	exp_next		= 500;
	lp				= 0;
	
	// ------ Attribute ------
	attribute[ATR_STRENGTH] 		= 0;
	attribute[ATR_DEXTERITY] 		= 0;
	attribute[ATR_MANA_MAX] 		= 0;
	attribute[ATR_MANA] 			= 0;
	attribute[ATR_HITPOINTS_MAX]	= 1;
	attribute[ATR_HITPOINTS] 		= 1;
	
	// ------ Kampf-Talente ------
	//B_SetFightSkills 	(self, 10); 
};


func int G_CanSteal()
{
	return TRUE;
};

func int C_DropUnconscious()
{
	return True;
};




func void ZS_Unconscious ()
{	
	
	AI_StopPointAt	(self);
	
	AI_UnequipWeapons (self);
};
	
func int ZS_Unconscious_Loop ()
{
	if (Npc_GetStateTime (self) < HAI_TIME_UNCONSCIOUS)
	{
		return LOOP_CONTINUE;
	}
	else
	{
		return LOOP_END;
	};
};

func void ZS_Unconscious_End ()
{	
	AI_StandUp(self);
	return;
};


func void ZS_Dead ()
{	
	AI_StopPointAt	(self);
	AI_UnequipWeapons (self);
};

func int ZS_Dead_loop ()
{
	return LOOP_CONTINUE;
};

INSTANCE ItMi_Gold (C_Item)
{
	name 				=	"Gold";

	mainflag 			=	1;
	flags 				=	1 << 21;

	value 				=	1;

	visual 				=	"ItMi_Gold.3DS";
	material 			=	1;

	description			= 	name;
	
	TEXT[5]				= 	"";	
	COUNT[5]			= 	value;
	
	INV_ZBIAS				= 250;
};