require "Bimbol Engine/require"



function BE_OnGamemodeInit()
enableSavePlayerAfterDeath(true);
enableAntyCheat(true);
ai_Init(true,true,true);
  --Monster
addMonster("YSCAVENGER",
{
        ai = "ANIMAL",
        name = "Junger Scavanger",
        instance = "SCAVENGER",
        str = 10,
        dex = 5,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 3,
        hp = 100,
        mp = 0,
        exp = 5,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});
 
-- Create Instance of Monster: Orc Warrior
addMonster("ORCWARRIOR_HARAD",
{
        ai = "2H",
        name = "Krieger der Orks",
        instance = "ORCWARRIOR_HARAD",
        str = 75,
        dex = 100,
        skill_1h = 0,
        skill_2h = 30,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 30,
        hp = 600,
        mp = 0,
        exp = 40,
        min_dist = 350,
        max_dist = 1100,
        respawn = 3600,
        blow_time = false,
        weapon = "ITMW_2H_ORCAXE_01",
});
 
addMonster("Scavenger",
{
        ai = "ANIMAL",
        name = "Scavanger",
        instance = "SCAVENGER",
        str = 15,
        dex = 5,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 8,
        hp = 125,
        mp = 0,
        exp = 10,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});

addMonster("GiantBug",
{
        ai = "ANIMAL",
        name = "Feldraueber",
        instance = "GIANT_BUG",
        str = 10,
        dex = 29,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 100,
        mp = 0,
        exp = 1,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});
 
addMonster("WOLF",
{
        ai = "ANIMAL",
        name = "Wolf",
        instance = "WOLF",
        str = 30,
        dex = 30,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 10,
        hp = 150,
        mp = 0,
        exp = 15,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});

addMonster("Lurker",
{
        ai = "ANIMAL",
        name = "Lurker",
        instance = "LURKER",
        str = 30,
        dex = 30,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 10,
        hp = 150,
        mp = 0,
        exp = 15,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});
 
addMonster("RAZOR",
{
        ai = "ANIMAL",
        name = "Razor",
        instance = "RAZOR",
        str = 70,
        dex = 70,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 15,
        hp = 300,
        mp = 0,
        exp = 25,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});
 
addMonster("BLOODHOUND",
{
        ai = "ANIMAL",
        name = "Bluthund",
        instance = "BLOODHOUND",
        str = 65,
        dex = 65,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 15,
        hp = 300,
        mp = 0,
        exp = 25,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});
 
addMonster("SHADOWBEAST",
{
        ai = "ANIMAL",
        name = "Schattenlaufer",
        instance = "SHADOWBEAST",
        str = 80,
        dex = 80,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 500,
        mp = 0,
        exp = 50,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});
 
addMonster("SNAPPER",
{
        ai = "ANIMAL",
        name = "Snapper",
        instance = "SNAPPER",
        str = 50,
        dex = 50,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 12,
        hp = 200,
        mp = 0,
        exp = 25,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});
 
addMonster("BLOODFLY",
{
        ai = "ANIMAL",
        name = "Blutfliege",
        instance = "BLOODFLY",
        str = 25,
        dex = 25,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 5,
        hp = 100,
        mp = 0,
        exp = 10,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});
 
addMonster("ICEGOLEM",
{
        ai = "ANIMAL",
        name = "Eisgolem",
        instance = "ICEGOLEM",
        str = 80,
        dex = 5,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 30,
        hp = 500,
        mp = 0,
        exp = 60,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});
 
addMonster("FIREGOLEM",
{
        ai = "ANIMAL",
        name = "Feuergolem",
        instance = "FIREGOLEM",
        str = 90,
        dex = 5,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 30,
        hp = 500,
        mp = 0,
        exp = 60,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});
 
addMonster("STONEGOLEM",
{
        ai = "ANIMAL",
        name = "Steingolem",
        instance = "STONEGOLEM",
        str = 100,
        dex = 5,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 40,
        hp = 600,
        mp = 0,
        exp = 70,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});
 
addMonster("GOBBO_GREEN",
{
        ai = "1H",
        name = "Goblin",
        instance = "GOBBO_GREEN",
        str = 30,
        dex = 30,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 6,
        hp = 175,
        mp = 0,
        exp = 15,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
        weapon = "ItMw_1h_Bau_Mace",
});
 
addMonster("GOBBO_WARRIOR",
{
        ai = "1H",
        name = "Goblin Krieger",
        instance = "GOBBO_WARRIOR",
        str = 50,
        dex = 50,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 10,
        hp = 225,
        mp = 0,
        exp = 20,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
        weapon = "ItMw_Nagelknueppel",
});
 
addMonster("LURKER",
{
        ai = "ANIMAL",
        name = "Lurker",
        instance = "LURKER",
        str = 40,
        dex = 40,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 10,
        hp = 175,
        mp = 0,
        exp = 20,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});
 
addMonster("Molerat",
{
        ai = "ANIMAL",
        name = "Molerat",
        instance = "MOLERAT",
        str = 10,
        dex = 10,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 3,
        hp = 100,
        mp = 0,
        exp = 5,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});
 
addMonster("WARAN",
{
        ai = "ANIMAL",
        name = "Waran",
        instance = "WARAN",
        str = 30,
        dex = 30,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 3,
        hp = 500,
        mp = 0,
        exp = 15,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});
 
addMonster("SWAMPSHARK",
{
        ai = "ANIMAL",
        name = "Sumpfhai",
        instance = "SWAMPSHARK",
        str = 90,
        dex = 90,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 30,
        hp = 500,
        mp = 0,
        exp = 40,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});
 
addMonster("Zombie",
{
        ai = "ANIMAL",
        name = "Zombie",
        instance = "ZOMBIE",
        str = 60,
        dex = 69,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 300,
        mp = 0,
        exp = 30,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});

addMonster("JungerWolf",
{
        ai = "ANIMAL",
        name = "Junger Wolf",
        instance = "YWOLF",
        str = 60,
        dex = 69,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 300,
        mp = 0,
        exp = 30,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 2,
});

addMonster("Keiler",
{
        ai = "ANIMAL",
        name = "Wildschwein",
        instance = "KEILER",
        str = 20,
        dex = 29,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 100,
        mp = 0,
        exp = 1,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});

addMonster("Warg",
{
        ai = "ANIMAL",
        name = "Warg",
        instance = "WARG",
        str = 40,
        dex = 29,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 170,
        mp = 0,
        exp = 1,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});

addMonster("MinerCrawler",
{
        ai = "ANIMAL",
        name = "Minecrawler",
        instance = "MINECRAWLER",
        str = 30,
        dex = 29,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 170,
        mp = 0,
        exp = 1,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});


addMonster("MinecrawlerWarrior",
{
        ai = "ANIMAL",
        name = "Minecrawler Krieger",
        instance = "minecrawlerwarrior",
        str = 50,
        dex = 29,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 270,
        mp = 0,
        exp = 1,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});

addMonster("Troll",
{
        ai = "ANIMAL",
        name = "Troll",
        instance = "TROLL",
        str = 70,
        dex = 29,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 470,
        mp = 0,
        exp = 1,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});

addMonster("BlackTroll",
{
        ai = "ANIMAL",
        name = "Schwarzer Troll",
        instance = "TROLL_BLACK",
        str = 90,
        dex = 29,
        skill_1h = 0,
        skill_2h = 0,
        skill_bow = 0,
        skill_cbow = 0,
        lvl = 20,
        hp = 770,
        mp = 0,
        exp = 1,
        min_dist = 250,
        max_dist = 700,
        respawn = 3600,
        blow_time = 3,
});

   --NEWWORLD

	-- schaafe
	spawnMonster("Schaaf",49644.77734375,3222.1069335938,-19567.599609375,15,"NEWWORLD\\NEWWORLD.ZEN", 300, false);
--	spawnMonster("Schaaf",49075.17578125,2993.6472167969,-18652.880859375,15,"NEWWORLD\\NEWWORLD.ZEN", 300, false);
--	spawnMonster("Schaaf",58091.625,2155.3771972656,-3082.6833496094,15,"NEWWORLD\\NEWWORLD.ZEN", 300, false);
--	spawnMonster("Schaaf",75602.2109375,3743.2468261719,-15414.819335938,15,"NEWWORLD\\NEWWORLD.ZEN", 300, false);
--	spawnMonster("Schaaf",30518.69921875,3472.2150878906,9922.576171875,15,"NEWWORLD\\NEWWORLD.ZEN", 300, false);
	
	
	--JungerWolf
	spawnMonster("JungerWolf", 8859.1709 ,1475.45313 ,-23093.7285, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 9280.95508 ,781.698792 ,-10258.1553, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 27905.2012 ,3739.11914 ,-20546.1836, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 16857.5293 ,2020.52271 ,-23996.1152, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 26059.082 ,440.092377 ,-17869.1406, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 27569.084 ,526.761963 ,-18071.4082, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 29181.2012 ,706.134583 ,-17194.7695, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 25367.4473 ,521.388794 ,-14795.6807, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 26100.0684 ,856.895935 ,-12536.3887, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 32687.6543 ,3424.90015 ,-30119.4844, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 34357.5469 ,3874.19971 ,-34983.1563, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 30494.0527 ,5440.20215 ,-34398.5859, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 29738.0723 ,5457.13965 ,-35230.3594, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("JungerWolf", 29340.5195 ,5455.84375 ,-35619.4414, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--YoungGiantBug
	spawnMonster("YoungGiantBug", 10269.4736 ,966.838989 ,-8889.69629, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("YoungGiantBug", 13502.3535 ,1640.48022 ,-9478.18848, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("YoungGiantBug", 17692.1855 ,2514.22339 ,-9484.21191, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("YoungGiantBug", 32572.6836 ,3416.74316 ,-18376.7754, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--Snapper
	spawnMonster("SNAPPER", 367.927124 ,52.8669739 ,-8111.4751, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("SNAPPER", 45992.707 ,2505.69849 ,-32371.7539, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("SNAPPER", 13353.793 ,2332.28857 ,29184.8281, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("SNAPPER", 5942.68799 ,2993.11841 ,25311.6309, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("SNAPPER", 8996.9502 ,3095.80005 ,24052.0254, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("SNAPPER", 10395.2568 ,2803.12622 ,25016.9805, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("SNAPPER", 74921.4688 ,5069.17285 ,22943.5352, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("SNAPPER", 75886.9219 ,5011.13428 ,22567.2266, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("SNAPPER", 52160.6172 ,7951.87109 ,35618.1758, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);

	--Wolf
	spawnMonster("WOLF", 4004.46606 ,321.412109 ,-13818.0625, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 48532.4961 ,3546.2876 ,10731.4707, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 45803.3516 ,2571.08105 ,-3839.21289, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	
	spawnMonster("WOLF", 57271.9648 ,1889.34729 ,4568.39209, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 61939.1523 ,2267.7937 ,4671.66602, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 63400.0078 ,2273.09863 ,6352.79102, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 68081.2969 ,2348.59937 ,-1067.3645, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 67726.0156 ,1563.2876 ,-26994.2246, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 20329.8477 ,1996.0271 ,12655.7363, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 20953.5723 ,1979.10498 ,13376.1309, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 23779.0156 ,2501.68433 ,17059.1289, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 15904.7178 ,586.420837 ,4539.51709, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 17462.7637 ,1120.81921 ,2664.50269, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 30049.3555 ,2785.56372 ,4028.7229, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 20575.6094 ,868.285889 ,5291.64941, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 4476.76025 ,2486.42676 ,20873.2871, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 14814.8115 ,355.088379 ,4995.23096, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 19301.2617 ,2724.0127 ,10204.6406, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 31087.1191 ,4161.521 ,27164.6211, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("WOLF", 59046.625 ,3179.70386 ,12083.9229, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);

    --Shadowbeast
    spawnMonster("Shadowbeast", 3383.68408 ,366.023102 ,-15403.3271, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Shadowbeast", 65898.1641 ,2383.62988 ,3386.38721, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Shadowbeast", 23621.3887 ,3799.80737 ,24296.9941, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Shadowbeast", 29293.3281 ,-2009.04028 ,239.425552, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Shadowbeast", 1472.25317 ,2900.60718 ,18767.5664, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Shadowbeast", 30943.7773 ,-1636.06982 ,2602.33667, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Shadowbeast", 71623.4297 ,5241.03809 ,25810.8926, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Shadowbeast", 67387.0547 ,3270.01294 ,14878.4092, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Shadowbeast", 66470.1953 ,3269.28125 ,13256.4404, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--OrcWarrior
	spawnMonster("OrcWarrior", 2661.72021 ,621.533081 ,-19715.5352, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 33492.8477 ,4255.33203 ,-37849.5898, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 33423 ,4419.9082 ,-39118, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 31954.8848 ,4622.81445 ,-41078.6992, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 32411.0273 ,4550.95313 ,-40562.4844, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 31818.8691 ,4514.51465 ,-40254.0469, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 31566.9727 ,4434.26221 ,-39379.1367, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 30981.1973 ,4591.66357 ,-40407.4141, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 30885.9805 ,4724.05176 ,-41774.6289, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 30683.1738 ,4773.61914 ,-42238.457, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 30450.2012 ,4819.58643 ,-42743.1602, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 30625 ,4858.1416 ,-43986, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 31002.4512 ,4788.95752 ,-42654.2773, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 12157.9063 ,4034.91504 ,21244.2402, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 20847.9805 ,868.285889 ,5573.05713, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 9767.53223 ,3472.00342 ,22129.2695, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("OrcWarrior", 17284.2832 ,3877.9939 ,20493.9531, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);

	
	--GiantRat
	spawnMonster("GiantRat", 21550.3535 ,3569.08862 ,-11371.2959, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 25481.7266 ,755.155701 ,-13080.9414, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 24272.8125 ,1019.43945 ,-9577.85449, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 25917.1563 ,1018.65472 ,-9761.8457, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 40011.75 ,3720.92407 ,6013.75342, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	
	spawnMonster("GiantRat", 37381.293 ,3296.375 ,2306.87793, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 36247.8203 ,3839.25854 ,-30738.1914, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 36556.3984 ,3968.91968 ,-31375.9922, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 36545.5898 ,4310.31982 ,-32434.6836, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 36459.5156 ,4546.09326 ,-33202.4141, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 23318.7715 ,2456.12695 ,6860.17139, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 23534.3105 ,2555.74756 ,5791.30078, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 20418.9414 ,2691.85693 ,14853.6543, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 20645.6426 ,2611.85693 ,14454.2529, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", -10040.374 ,-616.083618 ,-6153.96484, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 20395.3516 ,1178.08081 ,2414.50122, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 21215.8086 ,1114.91235 ,3250.62622, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 7598.58154 ,490.028503 ,12288.6191, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 6954.86963 ,448.440247 ,12874.834, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 31764.8535 ,-2351.03589 ,-676.739136, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 31627.7246 ,-2369.2666 ,-522.277466, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 31916.3359 ,-2360.8042 ,-434.838287, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 28453.0664 ,3167.37964 ,28856.9961, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 2675.83374 ,150.401428 ,13739.5625, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 4269.05273 ,179.771515 ,15219.5791, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 5085.11621 ,56.6929207 ,11641.5156, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 33258.6563 ,-2125.79736 ,4864.2002, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	
	spawnMonster("GiantRat", 15139.0762 ,3957.55322 ,24917.5605, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 35270.9336 ,5174.13721 ,31190.4219, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 79922.3438 ,5003.32129 ,23384.666, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 79931.0625 ,5072.59033 ,21958.5039, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 80860.3125 ,3700.76904 ,20171.0684, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 79152.3594 ,3898.84058 ,20193.3301, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 77893.2109 ,3898.84058 ,20285.0156, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 77858.4453 ,3900.81299 ,22013.0645, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 57242.0234 ,7238.34863 ,39277.9336, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantRat", 57747.7969 ,6981.18555 ,26496.2656, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);

	--Scavenger
	spawnMonster("Scavenger", 37588.9883 ,4358.43604 ,12686.5479, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 39506.1602 ,3544.8291 ,4799.44336, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 40488.3086 ,3437.52075 ,699.107239, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 42378.1602 ,3422.22217 ,-1086.52954, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 66496.0625 ,2296.23633 ,-1004.2821, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 57027.5039 ,1717.67041 ,-24597.5801, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 56713.9453 ,1954.1145 ,-26814.6777, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 46507.5977 ,2781.20288 ,-25290.2676, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 41437.4727 ,2756.27222 ,-16981.0352, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 39632.0898 ,2794.19873 ,-18414.3594, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 15791.2969 ,4132.79639 ,11511.0664, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 15684.7949 ,4138.38037 ,11197.3047, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 20899.1973 ,3247.69849 ,9791.38574, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 20835.6816 ,3123.93677 ,7785.71924, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 2446.04321 ,2172.1814 ,15799.793, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 8590.79688 ,3439.35889 ,15269.4199, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 61691.6523 ,3450.87109 ,15332.71, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 69594.2969 ,4450.33594 ,25104.2188, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 68825.2109 ,5118.50732 ,27554.3164, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 60163.2656 ,6927.26123 ,28497.8809, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 63816.043 ,6549.73633 ,32065.0273, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 40070.2617 ,6769.01807 ,30273.8848, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Scavenger", 69119.5078 ,6663.58301 ,34647.0742, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--Wild Boar
	spawnMonster("Keiler", 24822.0938 ,1773.05151 ,-17361.1855, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 84111.9141 ,4373.51025 ,-16104.3281, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 69074.6797 ,2206.86133 ,1987.8988, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 46057.125 ,2781.29785 ,-28792.2051, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 39852.0977 ,3364.29346 ,-11127.1953, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 39412.3672 ,3104.75 ,-14794.5752, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 52078.7266 ,1746.89087 ,-5603.62939, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 20727.8652 ,2672.55298 ,15876.9912, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 3370.08154 ,2097.28174 ,14232.9678, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 19160.7539 ,2193.41333 ,27586.1543, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 18362.0527 ,2172.81348 ,30013.6035, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 4985.90479 ,2344.20044 ,18568.2383, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Keiler", 29491.332 ,2154.43115 ,14217.0352, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--GiantBug
	spawnMonster("GiantBug", 41162.5078 ,4257.55957 ,11371.334, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 64346.1406 ,2524.38281 ,-15128.875, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 64435.8555 ,2489.3064 ,-15875.2744, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 64709.6289 ,2463.74146 ,-16950.8105, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 66599.3984 ,2367.04053 ,-3848.11572, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 66925.4844 ,2393.80273 ,-3285.7124, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 63905.1563 ,2300.82837 ,-2211.00562, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 64192.6094 ,2328.00806 ,-2364.74048, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 60280.7461 ,2023.75696 ,-17338.5254, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 59157.207 ,2062.35986 ,-15820.4082, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 65274.8594 ,2496.0542 ,-6364.28418, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 47530.1914 ,3463.11792 ,4916.45703, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 64667.6641 ,2407.77539 ,5112.67578, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 73613.0234 ,3261.3103 ,485.881775, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 54099.6719 ,1643.62097 ,3611.12329, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 55479.0273 ,1484.70874 ,6685.80273, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 54119.3477 ,1324.33337 ,6889.5542, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	spawnMonster("GiantBug", 54834.2344 ,1719.45313 ,-13714.2256, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 55061.0977 ,1837.61865 ,-16130.749, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 56909.0078 ,1748.10889 ,-18641.7949, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 53201.3633 ,1683.80981 ,-3488.57129, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 27161.7793 ,2316.25391 ,17965.8613, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 23742.1387 ,-854.581238 ,-3945.13208, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 23098.4746 ,-752.387878 ,-3338.75269, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 22787.5645 ,-450.319061 ,-2373.78149, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 22746.4688 ,-627.098389 ,-2838.10645, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 20976.8887 ,-710.877808 ,-4122.68018, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 22081.2148 ,-532.190674 ,-2736.48755, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 30845.1074 ,4130.71191 ,-4398.48438, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 31670.6719 ,4158.51904 ,-5780.68311, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 32820.3789 ,4135.82813 ,-7150.71631, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 21354.4063 ,2860.90625 ,23048.6289, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 24649.0996 ,2625.10669 ,20134.416, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 11583.8525 ,2448.63818 ,27336.8535, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 16681.8203 ,2306.9397 ,29119.4785, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 11463.4219 ,3730.52759 ,22043.873, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 15031.2354 ,4165.4541 ,23807.3223, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 24237.668 ,2050.85986 ,8636.51563, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 24476.1973 ,1898.69727 ,9866.06543, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 24175.9199 ,1915.73254 ,11145.5742, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 27359.1719 ,2707.30664 ,9272.8291, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("GiantBug", 72661.9766 ,4766.19727 ,19735.1504, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
 
    --Lurker
    spawnMonster("Lurker", 57376.4805 ,1857.11414 ,-15622.6699, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 44511.9023 ,2129.50073 ,-6353.31787, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 47839.5586 ,1658.85876 ,-10433.6162, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 44159.9648 ,2637.41187 ,-13679.3271, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 43974.8828 ,2674.85181 ,-14872.8965, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 39405.418 ,2782.69849 ,-20376.8398, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 43358.8906 ,2684.06177 ,-17731.8711, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 45654.3711 ,2566.31201 ,-16080.2979, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 47080.4258 ,2561.69922 ,-14691.1309, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 43102.457 ,2928.14209 ,-24815.3184, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 44194 ,2978.30957 ,-27248.9004, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 58570.7461 ,6897.83398 ,29512.0723, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Lurker", 61159.1289 ,7021.24951 ,38028.8164, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
    
	--Zombie
	spawnMonster("Zombie", 59826.4219 ,4108.32178 ,-32721.4512, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 60875.2578 ,4308.54883 ,-36772.2539, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 14195.748 ,2637.31274 ,14190.0039, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 30923.6719 ,4175.49561 ,-12404.4404, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 60426.6523 ,4109.62793 ,-32776.5195, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 31460.8828 ,4175.49561 ,-12452.4814, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 59645.0859 ,4045.52295 ,-34743.1523, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 13983.8545 ,2692.23584 ,14443.2539, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 31214.8203 ,4175.46582 ,-12738.3428, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 59728.7617 ,4075.96533 ,-35283.6836, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Zombie", 13692.498 ,2722.23584 ,14470.666, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	
	--Warg
	spawnMonster("Warg", 60499.8047 ,2191.51953 ,3323.61328, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 25519.2969 ,3112.51123 ,15787.4336, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 25968.6113 ,2931.08545 ,16520.5898, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 27473.0469 ,2983.71582 ,16978.7695, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 10036.1064 ,3461.1604 ,19005.9102, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 14920.166 ,4419.41504 ,20448.291, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 15096.6504 ,4374.9375 ,20147.6211, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 26877.0684 ,-1053.91443 ,-1242.37268, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 27355.3086 ,-1157.0647 ,-947.698181, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 27620.0332 ,-1174.65491 ,-1632.82568, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 27978.5059 ,-1256.58179 ,-1463.72949, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 27118.0391 ,-1690.60559 ,1522.53296, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 27744.8477 ,-1852.05225 ,1531.79089, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 27850.1426 ,-1772.31689 ,1221.50366, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 8829.8584 ,3283.625 ,20823.3262, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 31235.3027 ,2624.6731 ,17269.0547, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 29607.6953 ,2576.78564 ,17988.1953, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 20163.4004 ,2608.59424 ,18330.3066, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 12941.4873 ,3782.22412 ,18571.1211, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 9767.53223 ,3472.00342 ,22129.2695, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 16995.1211 ,2669.47998 ,15073.1729, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 19529.0605 ,2619.8877 ,17520.7188, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Warg", 31951.7852 ,4215.70117 ,25900.7461, 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--Troll
	spawnMonster("Troll", 65899.0781 ,1473.42188 ,-31458.0117, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Troll", 67198.5078 ,1532.95215 ,-29773.2852, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--BlackTroll
	spawnMonster("BlackTroll", 48401.5977 ,7948.98779 ,39269.6797, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--MinerCrawler
	spawnMonster("MinerCrawler", 63245.7852 ,4118.00488 ,-27580.8809, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinerCrawler", 62875.5313 ,4118.00488 ,-28607.6543, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinerCrawler", 64372.0586 ,6918.03613 ,43350.8516, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinerCrawler", 81332.625 ,5292.22021 ,33440.4922, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinerCrawler", 82251.4063 ,5287.33447 ,33827.9141, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinerCrawler", 83089.7656 ,5323.07861 ,33690.1602, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	--MinecrawlerWarrior
	spawnMonster("MinecrawlerWarrior", 61205.9063 ,3872.14868 ,-27294.8965, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinecrawlerWarrior", 61881.1602 ,3996.36499 ,-26028.1797, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinecrawlerWarrior", 66391.5 ,6943.11963 ,43716.1563, 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinecrawlerWarrior", 39012.6719 ,5245.65186 ,30976.3047, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinecrawlerWarrior", 36387.3672 ,5293.35938 ,29799.7207, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinecrawlerWarrior", 84278.5156 ,5471.46289 ,33754.4023, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("MinecrawlerWarrior", 82993.6719 ,5273.19043 ,36198.0625, 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	
	
	--Molerat
	spawnMonster("Molerat", 31748.0059 ,3329.82959 ,1138.68774 , 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 32507.0078 ,3474.55835 ,1086.04919 , 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 32027.084 ,3149.30347 ,-647.127075 , 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 32090.625 ,3153.7334 ,-448.244598 , 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 34406.3008 ,-2302.64087 ,3851.09033 , 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 17456.4629 ,2771.91772 ,17248.9434 , 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 33758.7109 ,4170.29932 ,-8701.37012 , 3, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 22922.9082 ,2739.125 ,19182.4707 , 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 19836.0859 ,2477.21069 ,23794.0195 , 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 3423.73975 ,130.098343 ,10934.4316 , 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 14483.4326 ,4113.32031 ,18878.3926 , 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 64453.6211 ,3231.36108 ,18007.4199 , 1, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 46811.4141 ,7839.28418 ,32229.3379 , 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	spawnMonster("Molerat", 46304.2656 ,7850.34229 ,27711.707 , 2, "NEWWORLD\\NEWWORLD.ZEN", 300, true);
	end

function MonsterDeath(monsterid, playerid, instance, id, experience, ai)

 SetPlayerExperience(playerid, GetPlayerExperience(playerid) + experience);
 GameTextForPlayer(playerid,3000,3000,string.format("%s","Erfahrung +"..experience),"Font_Old_10_White_Hi.TGA",255,255,255,2000);

	
if GetPlayerInstance(monsterid) == "WOLF" then
if GetPlayerScience(playerid, 6) == 1 then
GiveItem(playerid,"ItAt_WolfFur",1);
GiveItem(playerid,"ItFoMuttonRaw",1);
SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 1);
SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +1 EXP");
end
end
		
		if GetPlayerInstance(monsterid)=="WARG" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItAt_WargFur",1);
		GiveItem(playerid,"ItFoMuttonRaw",1);
		SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 3);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +3 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="OrcWarrior_Roam" then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItMi_Gold",25);
		GiveItem(playerid,"ItMw_2H_OrcAxe_02",1);
		end
		
		if GetPlayerInstance(monsterid)=="Orc Shaman" then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItMw_2H_OrcAxe_01",1);
		GiveItem(playerid,"ItMi_Gold",45);
		end
		
		if GetPlayerInstance(monsterid)=="OrcElite_Roam" then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItMw_2H_OrcSword_02",1);
		GiveItem(playerid,"ItMi_Gold",100);
		end
		
		if GetPlayerInstance(monsterid)=="YWolf" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItAt_WolfFur",1);
		GiveItem(playerid,"ItFoMuttonRaw",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 1);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +1 EXP");
		end
		end

		if GetPlayerInstance(monsterid)=="Shadowbeast" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItFoMuttonRaw",4);
		GiveItem(playerid,"ITAT_SHADOWFUR",1);
		GiveItem(playerid,"ItAt_ShadowHorn",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 5);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +5 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="Keiler" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItFoMuttonRaw",2);
		GiveItem(playerid,"ItAt_Addon_KeilerFur",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 2);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +2 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="Lurker" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItAt_LurkerClaw",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 2);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +2 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="Troll" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItAt_TrollFur",1);
		GiveItem(playerid,"ItAt_TrollTooth",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 10);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +10 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="Troll_Black" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItAt_TrollBlackFur",1);
		GiveItem(playerid,"ItAt_TrollTooth",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 15);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +15 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="Minecrawler" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItAt_CrawlerPlate",1);
		GiveItem(playerid,"ItAt_CrawlerMandibles",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 2);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +2 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="MinecrawlerWarrior" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItAt_CrawlerPlate",1);
		GiveItem(playerid,"ItAt_CrawlerMandibles",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 3);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +3 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="Sheep" then
		if GetPlayerScience(playerid, 6) == 1 then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItAt_SheepFur",1);
		GiveItem(playerid,"ItFoMuttonRaw",1);
				SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 1);
		SendPlayerMessage(playerid,0,255,0,"Tier ausgenommen / +1 EXP");
		end
		end
		
		if GetPlayerInstance(monsterid)=="PC_HERO" then
		PlayAnimation(playerid,"T_PLUNDER");
		GiveItem(playerid,"ItMi_Gold",25);
		end
		
		
	
        if GetPlayerExperience(playerid) >= GetPlayerExperienceNextLevel(playerid) then
 
                SetPlayerExperienceNextLevel(playerid, GetPlayerExperienceNextLevel(playerid) + (GetPlayerLevel(playerid) + 1)*300);
 
                SetPlayerLevel(playerid, GetPlayerLevel(playerid) + 1);
 
                GameTextForPlayer(playerid,3000,3000,string.format("%s","Stufenaufstieg LP+5!"),"Font_Old_20_White_Hi.TGA",255,255,255,2000);
 
                SetPlayerMaxHealth(playerid, GetPlayerMaxHealth(playerid) + 5);
                CompleteHeal(playerid);
   
                SetPlayerLearnPoints(playerid, GetPlayerLearnPoints(playerid) + 5);
			
        end
end