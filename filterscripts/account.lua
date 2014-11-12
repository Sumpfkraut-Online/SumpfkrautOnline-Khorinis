    print("account.lua loaded");
	
function OnFilterscriptInit()

	EnableExitGame(0);

	db = sqlite3.open("Jharkendarnew.db");
	
	local Playersave = false;
	local charakter = false;
	
	InitCharhelper();
	InitPlayerTable();
end

function InitPlayerTable()
	Players = {};
	
	for i = 0, GetMaxPlayers() - 1 do
		Players[i] = {};
		Players[i].loggedIn = nil;
		Players[i].id = nil;
	end
end

function InitCharhelper()
	Bodys =
	{
		[1] = "Hum_Body_Naked0",
		[2] = "Hum_Body_Babe0"
	};
	
	Heads = 
	{
		[1] = "Hum_Head_FatBald",
		[2] = "Hum_Head_Bald",
		[3] = "Hum_Head_Fighter",
		[4] = "Hum_Head_Pony",
		[5] = "Hum_Head_Thief",
		[6] = "Hum_Head_Psionic",
		[7] = "Hum_Head_Babe"
	};
	
	Gang = 
	{
		[1] = "HumanS_Normal.mds",
		[2] = "HumanS_Tired.mds",
		[3] = "HumanS_Relaxed.mds",
		[4] = "HumanS_Militia.mds",
		[5] = "HumanS_Arrogance.mds",
		[6] = "HumanS_Mage.mds",
		[7] = "HumanS_Babe.mds"
	};
end

function OnPlayerConnect(playerid)

if playerid >= 0 and IsNPC(playerid) == 0 then
	SetPlayerColor(playerid,139,69,19);
end

local charaktermenu = string.format("%s %s", "Willkommen,",GetPlayerName(playerid));

Playersave = false;
--FreezePlayer(playerid, 1);

	if CheckIfPlayerExists(GetPlayerName(playerid)) == 1 then
		SpawnPlayer(playerid);
		SendPlayerMessage(playerid,255,0,0,"");
		SendPlayerMessage(playerid,255,0,0,"Es existieren gespeicherte Daten zu deinem Nicknamen");
		SendPlayerMessage(playerid,255,0,0,"Bitte logge dich mit /login (passwort) ein oder wähle einen anderen Namen");
	else
		SendPlayerMessage(playerid,255,0,0,"");
		SendPlayerMessage(playerid,218,165,32,charaktermenu);
		SendPlayerMessage(playerid,184,134,11,"Bitte erstelle einen Charakter (Tippe /charhelp für Hilfe)");
		SendPlayerMessage(playerid,184,134,11,"Wenn du fertig bist, tippe /fertig");
	end
	
	SendPlayerMessage(playerid,148,0,211,"Das Spiel kann mit /exit beendet werden");
end

function OnPlayerDisconnect(playerid, reason)
	Players[playerid].loggedIn = nil;
	Players[playerid].id = nil;
end

function OnPlayerResponseItem(playerid, slot, item_instance, amount, equipped)
if not Playersave then return end
    if item_instance ~= "NULL" then
        print("ima yer");
        if WasItemSavedAlready(playerid, item_instance) == 1 then
            print("ima dar");
            db:exec("UPDATE PlayerItems SET amount="..amount..", equipped="..equipped.." WHERE player_id = "..Players[playerid].id.." AND item_instance = '"..item_instance.."'");
        else
            print("ima wfwg");
            db:exec("INSERT INTO PlayerItems (player_id, item_instance, amount, equipped) VALUES ("..Players[playerid].id..", '"..item_instance.."', "..amount..", "..equipped..")");
        end
        
        GetPlayerItem(playerid, slot + 1);
    else
        ExitGame(playerid);
    end
end

function OnPlayerDropItem(playerid, itemid, item_instance, amount, x, y, z, worldName)
	if WasItemSavedAlready(playerid, item_instance) == 1 then
		db:exec("DELETE FROM PlayerItems WHERE player_id = "..Players[playerid].id.." AND item_instance = '"..item_instance.."'");
	end
end

function OnPlayerUseItem(playerid, item_instance, amount, hand)
	if WasItemSavedAlready(playerid, item_instance) == 1 then
		if RemainingAmount(playerid, item_instance, amount) <= 0 then
			db:exec("DELETE FROM PlayerItems WHERE player_id = "..Players[playerid].id.." AND item_instance = '"..item_instance.."'");
		else
			db:exec("UPDATE PlayerItems SET amount="..RemainingAmount(playerid, item_instance, amount).." WHERE player_id = "..Players[playerid].id.." AND item_instance = '"..item_instance.."'");
		end
	end
end

function RemainingAmount(playerid, item_instance, amount)
	for TableRows in db:nrows ("SELECT amount FROM PlayerItems WHERE player_id = "..Players[playerid].id.." AND item_instance = '"..item_instance.."'") do
		return TableRows["amount"] - amount;
	end
end

function OnPlayerCommandText(playerid, cmdtext)

    local cmd,params = GetCommand(cmdtext);

	if cmdtext == "/charhelp" then
		CMD_CharHelp(playerid);
		
	elseif cmdtext == "/fertig" then
		CMD_CharFertig(playerid);
		
	elseif cmdtext == "/exit" or cmd == "/ende" or cmd == "/quit" then
		Playersave = true
		CMD_Exit(playerid);

	elseif cmd == "/head" then
		CMD_Head(playerid,params);

	elseif cmd == "/face" then
		CMD_Face(playerid,params);

	elseif cmd == "/body" then
		CMD_Body(playerid,params);

	elseif cmd == "/skin" then
		CMD_Skin(playerid,params);
		
	elseif cmd == "/gang" then
		CMD_Gang(playerid,params);
		
	elseif cmd == "/login" then
		CMD_Logmein(playerid,params);	
		
	elseif cmd == "/register" then
		CMD_Registerme(playerid,params);
	end
end

function CMD_CharHelp(playerid)

if CheckIfPlayerExists(GetPlayerName(playerid)) == 0 then
		SendPlayerMessage(playerid,255,255,255,"Kopfmodel: /head (1-7)");
		SendPlayerMessage(playerid,255,255,255,"Gesicht: /face (0-162)");
		SendPlayerMessage(playerid,255,255,255,"Koerpermodel: /body (1-2)");
		SendPlayerMessage(playerid,255,255,255,"Koerpertextur: /skin (1-8)");
		SendPlayerMessage(playerid,255,255,255,"Gangart: /gang (1-7)");
		else
		SendPlayerMessage(playerid,255,255,255,"Du bist bereits registriert!");
	end
end

function CMD_CharFertig(playerid)

if CheckIfPlayerExists(GetPlayerName(playerid)) == 0 then
		local bodyModel, bodyTexture, headModel, headTexture = GetPlayerAdditionalVisual(playerid);
		SetPlayerAdditionalVisual(playerid, bodyModel, bodyTexture, headModel, headTexture);
		--local gang = GetPlayerWalk(playerid);
		--SetPlayerWalk(playerid, gang);
		SendPlayerMessage(playerid,255,0,0,"Nun registriere dich mit: /register [Passwort]");
		charakter = true;
		else
		SendPlayerMessage(playerid,255,255,255,"Du bist bereits registriert!");
	end
end

function CMD_Exit(playerid)

		if Playersave == true then
		SetPlayerColor(playerid,205,51,51);
		FreezePlayer(playerid, 1);
		SendPlayerMessage(playerid,34,139,34,"Das Spiel wird in Kürze beendet");
		if Players[playerid].loggedIn ~= nil then
		OverwriteCharacterData(playerid);
		else
		ExitGame(playerid);
		end
	end
end

function CMD_Head(playerid, params)

if CheckIfPlayerExists(GetPlayerName(playerid)) == 0 then
		local result, headmesh = sscanf(params, "d");
		if result == 1 then
			if headmesh >= 1 and headmesh <= 7 then
				local bodyModel, bodyTexture, headModel, headTexture = GetPlayerAdditionalVisual(playerid);
				SetPlayerAdditionalVisual(playerid, bodyModel, bodyTexture, Heads[headmesh], headTexture);
				else
				SendPlayerMessage(playerid,255,255,255,"Du bist bereits registriert!");
			end
		end
	end
end

function CMD_Face(playerid, params)

if CheckIfPlayerExists(GetPlayerName(playerid)) == 0 then
		local result, headtex = sscanf(params, "d");
		if result == 1 then
			if headtex >= 0 and headtex <= 162 then
				local bodyModel, bodyTexture, headModel, headTexture = GetPlayerAdditionalVisual(playerid);
				SetPlayerAdditionalVisual(playerid, bodyModel, bodyTexture, headModel, headtex);
				else
				SendPlayerMessage(playerid,255,255,255,"Du bist bereits registriert!");
			end
		end
	end
end

function CMD_Body(playerid, params)

if CheckIfPlayerExists(GetPlayerName(playerid)) == 0 then
		local result, bodymesh = sscanf(params, "d");
		if result == 1 then
			if bodymesh == 1 or bodymesh == 2 then
				local bodyModel, bodyTexture, headModel, headTexture = GetPlayerAdditionalVisual(playerid);
				SetPlayerAdditionalVisual(playerid, Bodys[bodymesh], bodyTexture, headModel, headTexture);
				else
				SendPlayerMessage(playerid,255,255,255,"Du bist bereits registriert!");
			end
		end
	end
end

function CMD_Skin(playerid, params)

if CheckIfPlayerExists(GetPlayerName(playerid)) == 0 then
		local result, bodytex = sscanf(params, "d");
		if result == 1 then
			if bodytex >= 1 and bodytex <= 8 then
				local bodyModel, bodyTexture, headModel, headTexture = GetPlayerAdditionalVisual(playerid);
				SetPlayerAdditionalVisual(playerid, bodyModel, bodytex, headModel, headTexture);
				else
				SendPlayerMessage(playerid,255,255,255,"Du bist bereits registriert!");
			end
		end
	end
end
	
function CMD_Gang(playerid, params)

	local result, gang = sscanf(params, "d");
		if result == 1 then
			if gang >= 1 and gang <= 7 then
				SetPlayerWalk(playerid, Gang[gang]);
		end
	end
end

function CMD_Logmein(playerid, params)

		if Players[playerid].loggedIn == nil then
			if CheckIfPlayerExists(GetPlayerName(playerid)) == 1 then
				local result, password = sscanf(params, "s");
				if CheckIfPasswordIsValid(playerid, password) == 1 then
					LoadCharacterData(playerid);
					Players[playerid].loggedIn = 1;
					
					SendPlayerMessage(playerid,0,100,0,"Du hast dich erfolgreich eingeloggt!");
					
					SetPlayerColor(playerid,250,250,250);
					
					FreezePlayer(playerid, 0);
					
				else
					SendPlayerMessage(playerid,255,0,0,"Ungültiges Passwort!");
				end
			else
				SendPlayerMessage(playerid,255,0,0,"Es existieren keine gespeicherten Daten zu diesem Namen");
		end
	end
end

function CMD_Registerme(playerid, params)

if charakter == true then
		if Players[playerid].loggedIn == nil then
			local result, password = sscanf(params, "s");
			if result == 1 then
				if CheckIfPlayerExists(GetPlayerName(playerid)) == 0 then
					CreateCharacterData(playerid, password);
					Players[playerid].loggedIn = 1;
					
					for TableRows in db:nrows ("SELECT player_id FROM PlayerData WHERE name = '"..GetPlayerName(playerid).."'") do
						Players[playerid].id = TableRows["player_id"];
					end
					
					SendPlayerMessage(playerid,0,100,0,"Du hast dich erfolgreich registriert!");
					
					
					SetPlayerColor(playerid,250,250,250);
					
					SetPlayerWorld(playerid,"KENDARONLINE.ZEN","TOT");

					FreezePlayer(playerid, 0);
				else
					SendPlayerMessage(playerid,255,0,0,"Ein Spieler mit diesem Namen existiert bereits");
				end
			end
		else
			SendPlayerMessage(playerid,255,0,0,"Du bist bereits registriert!");
			SendPlayerMessage(playerid,255,0,0,"Wenn du einen neuen Charakter spielen willst, dann starte das");
			SendPlayerMessage(playerid,255,0,0,"Spiel neu und logge dich vor der Registrierung nicht ein!");
		end
	end
end

function WasItemSavedAlready(playerid, item_instance)
	for TableRows in db:nrows ("SELECT item_instance FROM PlayerItems WHERE player_id = "..Players[playerid].id.."") do
		if TableRows["item_instance"] == item_instance then
			return 1;
		end
	end
	
	return 0;
end

function CheckIfPlayerExists(name)
	for TableRows in db:nrows ("SELECT name FROM PlayerData") do
		if TableRows["name"] == name then
			return 1;
		end
	end
	
	return 0;
end

function CheckIfPasswordIsValid(playerid, password)
	for TableRows in db:nrows ("SELECT name, password FROM PlayerData") do
		if TableRows["name"] == GetPlayerName(playerid) and TableRows["password"] == (password) then
			return 1;
		end
	end
	
	return 0;
end

function CreateCharacterData(playerid, password)
	local bodyModel, bodyTexture, headModel, headTexture = GetPlayerAdditionalVisual(playerid);
	local x, y, z = GetPlayerPos(playerid);

	db:exec("INSERT INTO PlayerData (name, password) VALUES ('"..GetPlayerName(playerid).."', '"..(password).."')");
	db:exec("INSERT INTO PlayerLook (bodymesh, bodytex, headmesh, headtex) VALUES ('"..bodyModel.."', "..bodyTexture..", '"..headModel.."', "..headTexture..")");
	db:exec("INSERT INTO PlayerWalk (gang) VALUES ("..GetPlayerWalk(playerid)..")");
	db:exec("INSERT INTO PlayerStats (experiencelevel, experience, level, learnpoints, max_health, health, max_mana, mana, strength, dexterity, onehand, twohand, bow, crossbow) VALUES ("..GetPlayerExperienceNextLevel(playerid)..","..GetPlayerExperience(playerid)..","..GetPlayerLevel(playerid)..","..GetPlayerLearnPoints(playerid)..","..GetPlayerMaxHealth(playerid)..", "..GetPlayerHealth(playerid)..", "..GetPlayerMaxMana(playerid)..", "..GetPlayerMana(playerid)..", "..GetPlayerStrength(playerid)..", "..GetPlayerDexterity(playerid)..", "..GetPlayerSkillWeapon(playerid, 0)..", "..GetPlayerSkillWeapon(playerid, 1)..", "..GetPlayerSkillWeapon(playerid, 2)..", "..GetPlayerSkillWeapon(playerid, 3)..")");
	db:exec("INSERT INTO PlayerPos (world, x, y, z, angle) VALUES ('"..GetPlayerWorld(playerid).."', "..x..", "..y..", "..z..", "..GetPlayerAngle(playerid)..")");
end
	
function LoadCharacterData(playerid)
	
	for TableRows in db:nrows ("SELECT player_id FROM PlayerData WHERE name = '"..GetPlayerName(playerid).."'") do
		Players[playerid].id = TableRows["player_id"];
		print("id is: "..Players[playerid].id);
	end

	for TableRows in db:nrows ("SELECT bodymesh, bodytex, headmesh, headtex FROM PlayerLook WHERE player_id = "..Players[playerid].id.."") do
		SetPlayerAdditionalVisual(playerid, TableRows["bodymesh"], TableRows["bodytex"], TableRows["headmesh"], TableRows["headtex"]);
	end
	
	for TableRows in db:nrows ("SELECT gang FROM PlayerWalk WHERE player_id = "..Players[playerid].id.."") do
		SetPlayerWalk(playerid, TableRows["gang"]);
	end
	
	for TableRows in db:nrows ("SELECT experiencelevel, experience, level, learnpoints, max_health, health, max_mana, mana, strength, dexterity, onehand, twohand, bow, crossbow FROM PlayerStats WHERE player_id = "..Players[playerid].id.."") do
		SetPlayerExperienceNextLevel(playerid, TableRows["experiencelevel"]);
		SetPlayerExperience(playerid, TableRows["experience"]);
		SetPlayerLevel(playerid, TableRows["level"]);
		SetPlayerLearnPoints(playerid, TableRows["learnpoints"]);
		SetPlayerHealth(playerid, TableRows["health"]);
		SetPlayerMaxHealth(playerid, TableRows["max_health"]);
		SetPlayerMana(playerid, TableRows["mana"]);
		SetPlayerMaxMana(playerid, TableRows["max_mana"]);
		SetPlayerStrength(playerid, TableRows["strength"]);
		SetPlayerDexterity(playerid, TableRows["dexterity"]);
		SetPlayerSkillWeapon(playerid, 0, TableRows["onehand"]);
		SetPlayerSkillWeapon(playerid, 1, TableRows["twohand"]);
		SetPlayerSkillWeapon(playerid, 2, TableRows["bow"]);
		SetPlayerSkillWeapon(playerid, 3, TableRows["crossbow"]);
	end
	
	for TableRows in db:nrows ("SELECT world, x, y, z, angle FROM PlayerPos WHERE player_id = "..Players[playerid].id.."") do
		if TableRows["world"] ~= GetPlayerWorld(playerid) then
			SetPlayerWorld(playerid, TableRows["world"], "TOT");
		end
		
		SetPlayerPos(playerid, TableRows["x"], TableRows["y"], TableRows["z"]);
		SetPlayerAngle(playerid, TableRows["angle"]);
	end
	
	for TableRows in db:nrows ("SELECT item_instance, amount, equipped FROM PlayerItems WHERE player_id = "..Players[playerid].id.."") do
		if TableRows["equipped"] == 1 then
			EquipItem(playerid, TableRows["item_instance"]);
		else
			GiveItem(playerid, TableRows["item_instance"], TableRows["amount"]);
		end
	end
end

function OverwriteCharacterData(playerid)
	local x, y, z = GetPlayerPos(playerid);
	
	db:exec("UPDATE PlayerStats SET experiencelevel="..GetPlayerExperienceNextLevel(playerid)..", experience="..GetPlayerExperience(playerid)..", level="..GetPlayerLevel(playerid)..", learnpoints="..GetPlayerLearnPoints(playerid)..", max_health="..GetPlayerMaxHealth(playerid)..", health="..GetPlayerHealth(playerid)..", max_mana="..GetPlayerMaxMana(playerid)..", mana="..GetPlayerMana(playerid)..", strength="..GetPlayerStrength(playerid)..", dexterity="..GetPlayerDexterity(playerid)..", onehand="..GetPlayerSkillWeapon(playerid, 0)..", twohand="..GetPlayerSkillWeapon(playerid, 1)..", bow="..GetPlayerSkillWeapon(playerid, 2)..", crossbow="..GetPlayerSkillWeapon(playerid, 3).." WHERE player_id = "..Players[playerid].id.."");
	db:exec("UPDATE PlayerPos SET world='"..GetPlayerWorld(playerid).."', x="..x..", y="..y..", z="..z..", angle="..GetPlayerAngle(playerid).." WHERE player_id = "..Players[playerid].id.."");
	db:exec("UPDATE PlayerWalk SET gang='"..GetPlayerWalk(playerid).."' WHERE player_id = "..Players[playerid].id.."");
	
	GetPlayerItem(playerid, 0);
end