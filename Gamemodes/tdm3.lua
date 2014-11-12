
-----Player--Max---Players-----

local Player; 
local Player = {};
local MAX_PLAYERS = GetMaxPlayers();
local MAX_PLAYER_FIELD = 2;

local Server_Name = CreateDraw(6700, 7600, "Minental-Online.net RP/TDM 1.0", "Font_Old_10_White_Hi.TGA", 0,255,0);
local Server_Link = CreateDraw(6700, 7800, "", "Font_Old_10_White_Hi.TGA", 236,236,236);
local GameTime = CreateDraw(6700, 400, "Spielzeit: 00:00", "Font_Old_10_White_Hi.TGA", 255,255,0);

function ServerInfo(playerid)

    ShowDraw(playerid, GameTime);	
	
end

function TimeGame()

	local hour, minute = GetTime();
	UpdateDraw(GameTime ,6700, 400, string.format("%s %02d:%02d","Spielzeit:", hour, minute), "Font_Old_10_White_Hi.TGA", 255,255,0);

end

function ServerInfo(playerid)
	
	ShowDraw(playerid, Server_Name);
	ShowDraw(playerid, Server_Link);
    ShowDraw(playerid, GameTime);	
	
end

function OnGamemodeInit()
    --Gamemod----
print("------------------------------------------------------");
print("              Gamemode by Amnesite and -AVATAR-       ");
print("------------------------------------------------------");

----Menschliche--klassen-----------------------------
AddPlayerClass("PC_HERO",4001,-1374,3451,0,4001,-1374,3451,0); 
AddPlayerClass("PC_HERO",4001,-1374,3451,0,4001,-1374,3451,0);
AddPlayerClass("PC_HERO",4001,-1374,3451,0,4001,-1374,3451,0);
AddPlayerClass("PC_HERO",4001,-1374,3451,0,4001,-1374,3451,0);
AddPlayerClass("PC_HERO",4169,-751,-4941,0,4001,-1374,3451,0);
AddPlayerClass("PC_HERO",4169,-751,-4941,0,4001,-1374,3451,0);

---Ork---Klassen----------------------------------------------------------
AddPlayerClass("ORCWARRIOR_ROAM",11393,207,-3253,0,4001,-1374,3451,0);
AddPlayerClass("ORCWARRIOR_ROAM",11393,207,-3253,0,4001,-1374,3451,0);
AddPlayerClass("ORCELITE_ROAM",11393,207,-3253,0,4001,-1374,3451,0);

-----locale---nachrichten----
local score1Draw;
local score2Draw;
local TimeleftDraw;

-----Spieler--strukturen----
StructPlayers();
StructTeams();
EnableChat(0);
guiTeams();

------Gamemodname----Serverbeschreibung----------------
SetGamemodeName("Minental-Online.net RP/TDM 1.0");
SetServerDescription("Rollenspiel mit Deathmatch");

SetTimer("OnTimer1Sec",1000,1);
SetRespawnTime(1 * 1000); --3 secunden
SetTimer("TimeGame", 3500, 1);
end
 
function OnGamemodeExit()
 
print("-------------------");
print("Gamemode Beenden...");
print("-------------------");

end 
 
local Timer = {};
Timer.Minutes = 9;
Timer.Seconds = 0;
Timer.Freezed = false;
 
function StructPlayers()
local MAX_PLAYERS = GetMaxPlayers();
for i = 0 , MAX_PLAYERS - 1 do
Player[i] = {};
Player[i].tid = 0;
Player[i].gold = 0;
Player[i].classid = 0;
end
end
 
function guiTimeleft()
local timeleft = string.format("%d:%02d",Timer.Minutes,Timer.Seconds);
TimeleftDraw = CreateDraw(6500,5700,timeleft,"Font_Old_10_White_Hi.TGA",255,255,255);
end
 
 
local Team = {};
function StructTeams()
local MAX_TEAMS = 3;
for i = 0 , MAX_TEAMS - 1 do
Team[i] = {};
Team[i].score = 0;
Team[i].noplayers = 0;
end
end
 
function OnTimer1Sec()
updateGuiTeams();
updateTimer();
end
 
 
function updateTimer()
 
if Timer.Freezed == false then
if Timer.Seconds > 0 then
        Timer.Seconds = Timer.Seconds - 1;
               
elseif Timer.Seconds == 0 then
       
if Timer.Minutes > 0 then
        Timer.Minutes = Timer.Minutes - 1;
        Timer.Seconds = 59;
elseif Timer.Minutes == 0 and Timer.Minutes == 0 then
        OnTeamsRestart();
        Timer.Minutes = 9;
        Timer.Seconds = 0;
                local MAX_TEAMS = 3;
               
                        for i = 0, MAX_TEAMS - 1 do
                                Team[i].score = 0;
                        end
end
end
end
 
local timeleft = string.format("%d:%02d",Timer.Minutes,Timer.Seconds);
UpdateDraw(TimeleftDraw,6500,5700,timeleft,"Font_Old_10_White_Hi.TGA",255,255,255);
end
 
function updateGuiTeams()
local score1 = Team[1].score;
local score2 = Team[2].score;
 
UpdateDraw(score2Draw,6500,6000,string.format("%s %d", "Team Innos points:", score1),"Font_Old_10_White_Hi.TGA",15,232,102);
UpdateDraw(score1Draw,6500,6300,string.format("%s %d", "Team Beliar points:", score2),"Font_Old_10_White_Hi.TGA",232,15,37);
end
 
function ShowGui(playerid)
ShowDraw(playerid,score1Draw);
ShowDraw(playerid,score2Draw);
ShowDraw(playerid,TimeleftDraw);
end
 
function HideGui(playerid)
HideDraw(playerid,score1Draw);
HideDraw(playerid,score2Draw);
HideDraw(playerid,TimeleftDraw);
end
 
function WhatIsMyClassAndTeam(playerid, classid)
if classid == 0 then
Guard(playerid);
Player[playerid].tid = 1;
GameTextForPlayer(playerid,3000,3000,"Guard","Font_Old_20_White_Hi.TGA",15,232,102,3500);
elseif classid == 1 then
HeavyGuard(playerid)
Player[playerid].tid = 1;
GameTextForPlayer(playerid,3000,3000,"Knight","Font_Old_20_White_Hi.TGA",15,232,102,3500);
elseif classid == 2 then
HeavyGuard2(playerid)
Player[playerid].tid = 1;
GameTextForPlayer(playerid,3000,3000,"Paladin","Font_Old_20_White_Hi.TGA",15,232,102,3500);
elseif classid == 3 then
Mercenary(playerid)
Player[playerid].tid = 1;
GameTextForPlayer(playerid,3000,3000,"Mercenary","Font_Old_20_White_Hi.TGA",15,232,102,3500);
elseif classid == 4 then
Dragonhunter(playerid)
Player[playerid].tid = 2;
GameTextForPlayer(playerid,3000,3000,"Avatar Beliar","Font_Old_20_White_Hi.TGA",232,15,37,3500);
elseif classid == 5 then
HeavyDragonhunter(playerid)
Player[playerid].tid = 2;
GameTextForPlayer(playerid,3000,3000,"Dark Paladin","Font_Old_20_White_Hi.TGA",232,15,37,3500);
elseif classid == 6 then
Ork(playerid)
Player[playerid].tid = 2;
GameTextForPlayer(playerid,3000,3000,"Ork Scout","Font_Old_20_White_Hi.TGA",232,15,37,3500);
elseif classid == 7 then
Orkwarrior(playerid)
Player[playerid].tid = 2;
GameTextForPlayer(playerid,3000,3000,"Ork Warrior","Font_Old_20_White_Hi.TGA",232,15,37,3500);
elseif classid == 8 then
Orkelite(playerid)
Player[playerid].tid = 2;
GameTextForPlayer(playerid,3000,3000,"Ork Elite","Font_Old_20_White_Hi.TGA",232,15,37,3500);
end
end
 
function guiTeams()
local score1 = Team[1].score;
local score2 = Team[2].score;
 
 
score1Draw = CreateDraw(6500,6000,string.format("%s %d", "Team Alpha:", score1),"Font_Old_10_White_Hi.TGA",232,15,37);
score2Draw = CreateDraw(6500,6300,string.format("%s %d", "Team Beta:", score2),"Font_Old_10_White_Hi.TGA",15,232,102);
 
guiTimeleft();
end
 
 
function OnPlayerConnect(playerid)
SetPlayerWorld(playerid,"OLDWORLD\\OLDWORLD.ZEN","WP_INTRO13");
SendMessageToAll(0,255,0,string.format("%s %s",GetPlayerName(playerid),"ist beigetreten."));
ShowGui(playerid);
ServerInfo(playerid);
end
 
function OnPlayerChangeClass(playerid,classid)
WhatIsMyClassAndTeam(playerid, classid);
end
 
function PlaySpawnSound(playerid)
local SpawnSound = CreateSound("CHAPTER_01.WAV");
PlayPlayerSound(playerid,SpawnSound);
end
 
 
function OnPlayerSpawn(playerid, classid)
WhatIsMyClassAndTeam(playerid, classid);
PlaySpawnSound(playerid);
end
 
function OnPlayerText(playerid, text)
local MAX_PLAYERS = GetMaxPlayers();
local pname = GetPlayerName(playerid);
for i = 0, MAX_PLAYERS -1 do
if IsPlayerConnected(i) == 1 then
if IsPlayerAdmin(playerid) == 1 then
SendPlayerMessage(i,196,165,165,string.format("%s%s: %s","(Admin)",pname,text));
else
SendPlayerMessage(i,191,196,162,string.format("%s: %s",pname,text));
end
end
end
end
 
function OnPlayerHit(playerid, killerid)
local pteam = Player[playerid].tid;
local kteam = Player[killerid].tid;
if kteam == pteam then
GameTextForPlayer(killerid,3000,3000,"Greife nicht deine Kameraden an!","Font_Old_20_White_Hi.TGA",255,0,0,3400);
SetPlayerHealth(killerid,0);
end
end
 
function OnPlayerCommandText(playerid, cmdtext)
local cmd, params = GetCommand(cmdtext);
if cmd == "/pm" then
CMD_PM(playerid, params);
elseif cmd == "/kick" then
Kick(playerid);
elseif cmd == "/freeze" then
CMD_FTIME(playerid, params);
else
SendPlayerMessage(playerid,255,0,0,"Diesen Befehl gibt es nicht!")
end
end
 
function OnTeamsRestart()
local MAX_TEAMS = 3;
for i = 0, MAX_TEAMS - 1 do
Team[i].score = 0;
end
end
 
 
function OnPlayerDisconnect(playerid)
Player[playerid].tid = 0;
Player[playerid].gold = 0;
HideGui(playerid);
SendMessageToAll(255,0,0,string.format("%s %s",GetPlayerName(playerid),"hat uns Verlassen."));
end
 
function OnPlayerUnconscious(playerid, p_classid, killerid, k_classid)
local pname = GetPlayerName(playerid);
local kname = GetPlayerName(killerid);
GameTextForPlayer(killerid,3000,3000,string.format("%s %s","Du hast Umgebracht",pname),"Font_Old_20_White_Hi.TGA",0,255,0,3400);
GameTextForPlayer(playerid,3000,3000,string.format("%s %s", "Du wurdest gekillt.", kname),"Font_Old_20_White_Hi.TGA",255,0,0,3400);
local ktid = Player[killerid].tid;
local ktscore = Team[ktid].score;
Team[ktid].score = ktscore + 1;
SetPlayerHealth(playerid,0);

end

function CMD_PM(playerid, params)
local result, pid, pmsg = sscanf(params, "ds");
local pname = GetPlayerName(pid);
local sname = GetPlayerName(playerid);
if result == 1 then
        if IsPlayerConnected(pid) == 1 then
        SendPlayerMessage(pid,128,255,0,string.format("%s %s %s %s %s","(",sname,">>",pmsg,")"));
        SendPlayerMessage(playerid,114,186,41,string.format("%s %s %s %s %s","(",pname,"<<",pmsg,")"));
        else
        SendPlayerMessage(playerid,255,0,0,"Dieser Spieler ist nicht Online.");
        end
else
        SendPlayerMessage(playerid, 255, 0, 0, "Benutze : /pm id (Nachricht)");
end
end
 
function CMD_FTIME(playerid, params)
local TimerState = Timer.Freezed;
if IsPlayerAdmin(playerid) == 1 then
if TimerState == false then
Timer.Freezed = true;
        SendPlayerMessage(playerid,0,0,255,"Zeit freezed!");
elseif TimerState == true then
Timer.Freezed = false;
        SendPlayerMessage(playerid,0,0,255,"Zeit unfreezed!");
end
else
        SendPlayerMessage(playerid,255,0,0,"Du bist kein Entwickler.");
end
end
 
function Guard(playerid)
    
    SetPlayerLevel(playerid, 12)
    SetPlayerPos(playerid,-1030,186,1682);     
    SetPlayerInstance(playerid,"PC_HERO");
    SetPlayerAdditionalVisual(playerid,"Hum_Body_Naked0",1,"Hum_Head_Fighter",37);
    SetPlayerMaxHealth(playerid,800);
    SetPlayerHealth(playerid,800);
    SetPlayerStrength(playerid,140);
    SetPlayerDexterity(playerid,90);
    SetPlayerSkillWeapon(playerid,0,55);
    SetPlayerSkillWeapon(playerid,1,35);
    SetPlayerSkillWeapon(playerid,2,45);
    SetPlayerSkillWeapon(playerid,3,70);
    EquipArmor(playerid,"ITAR_MIL_M");
    EquipMeleeWeapon(playerid,"ItMw_Schwert2");
    EquipRangedWeapon(playerid,"ItRw_Crossbow_M_01");
    GiveItem(playerid,"ItRw_Bolt",43);
    GiveItem(playerid,"ItFoMutton",3);
    GiveItem(playerid,"ItFo_Bread",3);
    GiveItem(playerid,"ItFo_Apple",2);
    GiveItem(playerid,"ItFo_Cheese",2);
    GiveItem(playerid,"ItFo_Stew",2);
    GiveItem(playerid,"ItPo_Health_01",2);
    GiveItem(playerid,"ItPo_Health_02",1);
    GiveItem(playerid,"ItFo_Bread",2);
    GiveItem(playerid,"ItFo_Wine",3);
    GiveItem(playerid,"ItMi_Nugget",62);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
end

function HeavyGuard(playerid)

    SetPlayerPos(playerid,-2095,216,-2171);
    SetPlayerInstance(playerid,"PC_HERO");
    SetPlayerAdditionalVisual(playerid,"Hum_Body_Naked0",1,"Hum_Head_Fighter",35);
    SetPlayerMaxHealth(playerid,750);
    SetPlayerHealth(playerid,750);
    SetPlayerStrength(playerid,120);
    SetPlayerDexterity(playerid,50);
    SetPlayerSkillWeapon(playerid,0,45);
    SetPlayerSkillWeapon(playerid,1,65);
    SetPlayerSkillWeapon(playerid,2,45);
    SetPlayerSkillWeapon(playerid,3,55);
    EquipArmor(playerid,"ITAR_Pal_M");
    EquipMeleeWeapon(playerid,"itmw_2H_Pal_Sword");
    EquipRangedWeapon(playerid,"ItRw_Crossbow_L_02");
    GiveItem(playerid,"ItRw_Bolt",53);
    GiveItem(playerid,"ItFoMutton",3);
    GiveItem(playerid,"ItFo_Bread",3);
    GiveItem(playerid,"ItFo_Cheese",2);
    GiveItem(playerid,"ItFo_Stew",2);
    GiveItem(playerid,"ItPo_Health_01",2);
    GiveItem(playerid,"ItPl_Blueplant",2);
    GiveItem(playerid,"ItPo_Health_02",1);
    GiveItem(playerid,"ItFo_SmellyFish",2);
    GiveItem(playerid,"ItFo_Wine",3);
    GiveItem(playerid,"ItMi_Nugget",73);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
    GiveItem(playerid,"ItPo_Health_01",3);
    GiveItem(playerid,"ItPo_Health_02",2);
end

function HeavyGuard2(playerid)

    SetPlayerPos(playerid,-2095,216,-2171);
    SetPlayerInstance(playerid,"PC_HERO");
    SetPlayerAdditionalVisual(playerid,"Hum_Body_Naked0",1,"Hum_Head_Fighter",38);
    SetPlayerMaxHealth(playerid,550);
    SetPlayerHealth(playerid,550);
    SetPlayerStrength(playerid,135);
    SetPlayerDexterity(playerid,60);
    SetPlayerSkillWeapon(playerid,0,55);
    SetPlayerSkillWeapon(playerid,1,65);
    SetPlayerSkillWeapon(playerid,2,45);
    SetPlayerSkillWeapon(playerid,3,45);
    EquipArmor(playerid,"ITAR_Pal_H");
    EquipMeleeWeapon(playerid,"ItMw_2H_Blessed_02");
    GiveItem(playerid,"ItFoMutton",3);
    GiveItem(playerid,"ItFo_Bread",3);
    GiveItem(playerid,"ItFo_Cheese",2);
    GiveItem(playerid,"ItFo_Stew",2);
    GiveItem(playerid,"ItPo_Health_01",4);
    GiveItem(playerid,"ItFo_Sausage",3);
    GiveItem(playerid,"ItPo_Health_02",3);
    GiveItem(playerid,"ItFo_Wine",3);
    GiveItem(playerid,"ItMi_Nugget",84);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
    GiveItem(playerid,"ItPo_Health_01",3);
    GiveItem(playerid,"ItPo_Health_02",2);
end

function Mercenary(playerid)
   
    SetPlayerLevel(playerid, 12)
    SetPlayerPos(playerid,4212,-828,-5132);
    SetPlayerInstance(playerid,"PC_HERO");
    SetPlayerAdditionalVisual(playerid,"Hum_Body_Naked0",1,"Hum_Head_Pony",15);
    SetPlayerMaxHealth(playerid,720);
    SetPlayerHealth(playerid,720);
    SetPlayerStrength(playerid,120);
    SetPlayerDexterity(playerid,90);
    SetPlayerSkillWeapon(playerid,0,55);
    SetPlayerSkillWeapon(playerid,1,35);
    SetPlayerSkillWeapon(playerid,2,70);
    SetPlayerSkillWeapon(playerid,3,45);
    EquipArmor(playerid,"ItAr_SLD_H");
    EquipMeleeWeapon(playerid,"ItMw_Doppelaxt");
    EquipRangedWeapon(playerid,"ItRw_Bow_M_03");
    GiveItem(playerid,"ItRw_Arrow",32);
    GiveItem(playerid,"ItFoMutton",4);
    GiveItem(playerid,"ItFo_Bread",3);
    GiveItem(playerid,"ItFo_Apple",2);
    GiveItem(playerid,"ItFo_Cheese",3);
    GiveItem(playerid,"ItFo_Stew",2);
    GiveItem(playerid,"ItPo_Health_01",3);
    GiveItem(playerid,"ItPo_Health_02",2);
    GiveItem(playerid,"ItFo_Bread",2);
    GiveItem(playerid,"ItFo_Wine",3);
    GiveItem(playerid,"ItMi_Nugget",56);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
    GiveItem(playerid,"ItPo_Health_01",4);
    GiveItem(playerid,"ItPo_Health_02",3);
end

function Dragonhunter(playerid)
    
    SetPlayerPos(playerid,4212,-828,-5132); 
    SetPlayerInstance(playerid,"PC_HERO");
    SetPlayerAdditionalVisual(playerid,"Hum_Body_Naked0",1,"Hum_Head_Thief",2);
    SetPlayerMaxHealth(playerid,700);
    SetPlayerHealth(playerid,700);
    SetPlayerStrength(playerid,120);
    SetPlayerSkillWeapon(playerid,0,45);
    SetPlayerSkillWeapon(playerid,1,65);
    SetPlayerSkillWeapon(playerid,2,45);
    SetPlayerSkillWeapon(playerid,3,55);
    EquipArmor(playerid,"ItAr_Raven_Addon");
    EquipMeleeWeapon(playerid,"ItMw_BeliarWeapon_Fire");
    GiveItem(playerid,"ItFoMutton",3);
    GiveItem(playerid,"ItFo_Bread",3);
    GiveItem(playerid,"ItFo_Apple",2);
    GiveItem(playerid,"ItFo_Cheese",2);
    GiveItem(playerid,"ItFo_Stew",2);
    GiveItem(playerid,"ItPo_Health_01",2);
    GiveItem(playerid,"ItPo_Health_02",1);
    GiveItem(playerid,"ItFo_Bread",2);
    GiveItem(playerid,"ItFo_Wine",3);
    GiveItem(playerid,"ItMi_Nugget",62);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
end

function HeavyDragonhunter(playerid)
    
    SetPlayerPos(playerid,4212,-828,-5132);  
    SetPlayerInstance(playerid,"PC_HERO");
    SetPlayerAdditionalVisual(playerid,"Hum_Body_Naked0",1,"Hum_Head_Pony",35);
    SetPlayerMaxHealth(playerid,700);
    SetPlayerHealth(playerid,700);
    SetPlayerStrength(playerid,140);
    SetPlayerDexterity(playerid,60);
    SetPlayerSkillWeapon(playerid,0,55);
    SetPlayerSkillWeapon(playerid,1,70);
    SetPlayerSkillWeapon(playerid,2,45);
    SetPlayerSkillWeapon(playerid,3,45);
    EquipArmor(playerid,"ItAr_Pal_Skel");
    EquipMeleeWeapon(playerid,"ItMw_Zweihaender3");
    GiveItem(playerid,"ItFoMutton",3);
    GiveItem(playerid,"ItFo_Bread",3);
    GiveItem(playerid,"ItFo_Cheese",2);
    GiveItem(playerid,"ItFo_Stew",2);
    GiveItem(playerid,"ItPo_Health_01",2);
    GiveItem(playerid,"ItPl_Blueplant",2);
    GiveItem(playerid,"ItPo_Health_02",1);
    GiveItem(playerid,"ItFo_SmellyFish",2);
    GiveItem(playerid,"ItFo_Wine",3);
    GiveItem(playerid,"ItMi_Nugget",73);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
    GiveItem(playerid,"ItPo_Health_01",3);
    GiveItem(playerid,"ItPo_Health_02",2);
end

function Ork(playerid)
    
    SetPlayerPos(playerid,12936,348,-2126);
    SetPlayerInstance(playerid,"ORCWARRIOR_ROAM");
    SetPlayerMaxHealth(playerid,700);
    SetPlayerHealth(playerid,700);
    SetPlayerStrength(playerid,140);
    SetPlayerDexterity(playerid,60);
    SetPlayerSkillWeapon(playerid,0,45);
    SetPlayerSkillWeapon(playerid,1,45);
    EquipMeleeWeapon(playerid,"ItMw_2H_Orcaxe_02");
    GiveItem(playerid,"ItFoMutton",2);
    GiveItem(playerid,"ItAt_WolfFur",1);
    GiveItem(playerid,"ItLsTorch",2);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
    GiveItem(playerid,"ItPo_Health_01",4);
    GiveItem(playerid,"ItPo_Health_02",3);
end

function Orkwarrior(playerid)
    
    SetPlayerPos(playerid,12936,348,-2126);
    SetPlayerInstance(playerid,"ORCWARRIOR_ROAM");
    SetPlayerMaxHealth(playerid,620);
    SetPlayerHealth(playerid,620);
    SetPlayerStrength(playerid,140);
    SetPlayerDexterity(playerid,60);
    SetPlayerSkillWeapon(playerid,0,55);
    SetPlayerSkillWeapon(playerid,1,55);
    EquipMeleeWeapon(playerid,"ItMw_2H_Orcaxe_03");
    GiveItem(playerid,"ItFoMutton",2);
    GiveItem(playerid,"ItAt_WolfFur",1);
    GiveItem(playerid,"ItLsTorch",2);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
    GiveItem(playerid,"ItPo_Health_01",4);
    GiveItem(playerid,"ItPo_Health_02",3);
end

function Orkelite(playerid)
    
    SetPlayerPos(playerid,12936,348,-2126);
    SetPlayerInstance(playerid,"ORCELITE_ROAM");
    SetPlayerMaxHealth(playerid,580);
    SetPlayerHealth(playerid,580);
    SetPlayerStrength(playerid,140);
    SetPlayerDexterity(playerid,60);
    SetPlayerSkillWeapon(playerid,0,65);
    SetPlayerSkillWeapon(playerid,1,65);
    EquipMeleeWeapon(playerid,"ItMw_2H_OrcSword_02");
    GiveItem(playerid,"ItFoMutton",2);
    GiveItem(playerid,"ItAt_WolfFur",1);
    GiveItem(playerid,"ItLsTorch",2);
    GiveItem(playerid,"ItPo_Health_Addon_04",3);
    GiveItem(playerid,"ItPo_Health_01",4);
    GiveItem(playerid,"ItPo_Health_02",3);
end