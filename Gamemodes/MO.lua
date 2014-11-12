ForumNames = {};

chatmode = 0; --global
maintance = 0;

ClassItems = {};
ClassItemCount = 0;
-- copyright bei mir.
bodyMeshes =
{
	[1] = "Hum_Body_Naked0",
	[2] = "Hum_Body_Babe0"
};

headMeshes =
{
	[1] = "Hum_Head_FatBald",
	[2] = "Hum_Head_Fighter",
	[3] = "Hum_Head_Pony",
	[4] = "Hum_Head_Bald",
	[5] = "Hum_Head_Thief",
	[6] = "Hum_Head_Psionic",
	[7] = "Hum_Head_Babe"
}

Animations =
{
	["stand"] = "T_SIT_2_STAND",
	["sit"] = "T_STAND_2_SIT",
	["search"] = "T_SEARCH",
	["plunder"] = "T_PLUNDER",
	["kick"] = "T_BORINGKICK",
	["inspect"] = "T_1HSINSPECT",
	["train"] = "T_1HSFREE",
	["wash"] = "T_STAND_2_WASH",
	["pee"] = "T_STAND_2_PEE",
	["sleep"] = "T_STAND_2_SLEEPGROUND",
	["guardL"] = "T_STAND_2_LGUARD",
	["guardH"] = "T_STAND_2_HGUARD"
};

Walkstyles =
{
	[1] = "HumanS_Normal.mds",
	[2] = "HumanS_Tired.mds",
	[3] = "HumanS_Relaxed.mds",
	[4] = "HumanS_Militia.mds",
	[5] = "HumanS_Arrogance.mds",
	[6] = "HumanS_Mage.mds",
	[7] = "HumanS_Babe.mds"
};

function OnGamemodeInit()
	print("Server Gestartet..");

	SetRespawnTime(242535346);
	EnableChat(0);

	db = sqlite3.open("SimpleRP.db");
	print("Datenbank Geladen..");

	db:exec("CREATE TABLE IF NOT EXISTS Player ( 'playerName' TEXT NOT NULL, 'charName' TEXT NOT NULL, 'world' TEXT NOT NULL, 'posX' INTEGER NOT NULL, 'posY' INTEGER NOT NULL, 'posZ' INTEGER NOT NULL, 'angle' INTEGER NOT NULL, 'bodyModel' TEXT NOT NULL, 'bodyTextureID' INTEGER NOT NULL, 'headModel' TEXT NOT NULL, 'headTextureID' INTEGER NOT NULL, 'walkstyle' TEXT NOT NULL, 'meleeWeapon' TEXT NOT NULL, 'rangedWeapon' TEXT NOT NULL, 'armor' TEXT NOT NULL, 'trashID' INTEGER NOT NULL PRIMARY KEY)");

	Classes = io.open("class.txt", "r+");

	for line in Classes:lines() do
		local result, instance, amount = sscanf(line, "sd");

		if result == 1 then
			ClassItemCount = ClassItemCount + 1;

			ClassItems[ClassItemCount] = {};
			ClassItems[ClassItemCount]["instance"] = instance;
			ClassItems[ClassItemCount]["amount"] = amount;
		end
	end

	print("Classes Geladen..");
end

function SendMessageToPlayer(playerid, message)
	SendPlayerMessage(playerid, 255, 255, 255, message);
end

function SendMessageToAllPlayers(message)
	SendMessageToAll(255, 255, 255, message);
end

function SendMessageToAdmins(playerid, message)
	for player = 0, GetMaxPlayers() - 1, 1 do
		if IsPlayerAdmin(player) == 1 then
			SendPlayerMessage(playerid, 255, 50, 0, message);
		end
	end
end

function SendServerMessage(message)
	SendMessageToAll(200, 50, 0, message);
end

function OnPlayerConnect(playerid)
	if maintance == 1 and GetPlayerName(playerid) ~= "Amnestie" then
		SendMessageToPlayer(playerid, "Der Server fuehrt gerade Wartungsarbeiten durch.");
		Kick(playerid);

		return;
	end

	ForumNames[playerid] = GetPlayerName(playerid);

	SpawnPlayer(playerid);

	SetPlayerPos(playerid, -270, -78, -950);
	SetPlayerAngle(playerid, 112);

	SetPlayerStrength(playerid, 100);
	SetPlayerDexterity(playerid, 100);
	SetPlayerHealth(playerid, 500);
	SetPlayerMaxHealth(playerid, 500);
	SetPlayerSkillWeapon(playerid, 0, 50);
	SetPlayerSkillWeapon(playerid, 1, 50);
	SetPlayerSkillWeapon(playerid, 2, 50);
	SetPlayerSkillWeapon(playerid, 3, 50);
	SetPlayerScience(playerid, 0, 1);

	registered = 0;

	for results in db:nrows("SELECT charName, posX, posY, posZ, angle, bodyModel, bodyTextureID, headModel, headTextureID, walkstyle, meleeWeapon, rangedWeapon, armor FROM Player WHERE playerName = '"..GetPlayerName(playerid).."'") do
		SetPlayerName(playerid, results["charName"]);
		SetPlayerPos(playerid, results["posX"], results["posY"], results["posZ"]);
		SetPlayerAngle(playerid, results["angle"]);
		SetPlayerAdditionalVisual(playerid, results["bodyModel"], results["bodyTextureID"], results["headModel"], results["headTextureID"]);
		SetPlayerWalk(playerid, results["walkstyle"]);

		if results["meleeWeapon"] ~= "NULL" then
			EquipItem(playerid, results["meleeWeapon"]);
		end

		if results["rangedWeapon"] ~= "NULL" then
			EquipItem(playerid, results["rangedWeapon"]);
		end

		if results["armor"] ~= "NULL" then
			EquipItem(playerid, results["armor"]);
		end

		registered = 1;
	end

	if registered == 0 then
		local bodyModel, bodyTextureID, headModel, headTextureID = GetPlayerAdditionalVisual(playerid);
		db:exec("INSERT INTO Player (playerName, charName, world, posX, posY, posZ, angle, bodyModel, bodyTextureID, headModel, headTextureID, walkstyle, meleeWeapon, rangedWeapon, armor) VALUES ('"..GetPlayerName(playerid).."', '"..GetPlayerName(playerid).."', 'NEWWORLD\\NEWWORLD.ZEN', -270, -78, -950, 112, '"..bodyModel.."', "..bodyTextureID..", '"..headModel.."', "..headTextureID..", 'HumanS_Normal.mds', 'NULL', 'NULL', 'NULL')");
	end

	for itemCounter = 1, ClassItemCount, 1 do
		GiveItem(playerid, ClassItems[itemCounter]["instance"], ClassItems[itemCounter]["amount"]);
	end

	SendServerMessage(GetPlayerName(playerid).." Verbunden zu Minental-Online.net");
	SendMessageToPlayer(playerid, "Willkommen auf Minental-Online.net");
	SendMessageToPlayer(playerid, "Benutze /help fuer Commandos");
	SendMessageToPlayer(playerid, 'Bitte gebe " //" ein. Bevor du schreibst.');
end

function OnPlayerDisconnect(playerid, reason)
	if ForumNames[playerid] == nil then
		return;
	end

	local x, y, z = GetPlayerPos(playerid);
	local angle = GetPlayerAngle(playerid);
	local world = GetPlayerWorld(playerid);

	db:exec("UPDATE Player SET world='"..world.."', posX="..x..", posY="..y..", posZ="..z..", angle="..angle.." WHERE playerName = '"..ForumNames[playerid].."'");

	if reason == 0 then
		SendServerMessage(GetPlayerName(playerid).." Verbindung Getrennt");
	elseif reason == 1 then
		SendServerMessage(GetPlayerName(playerid).." Abgebrochen..");
	end

	ForumNames[playerid] = nil;
end

function OnPlayerChangeWalk(playerid, currWalk, oldWalk)
	db:exec("UPDATE Player SET walkstyle='"..currWalk.."' WHERE playerName = '"..ForumNames[playerid].."'");

	SetPlayerWalk(playerid, "Humans_1hST1.mds"); --Humans_1hST2.mds
end

function OnPlayerChangeMeleeWeapon(playerid, currMelee, oldMelee)
	db:exec("UPDATE Player SET meleeWeapon='"..currMelee.."' WHERE playerName = '"..ForumNames[playerid].."'");
end

function OnPlayerChangeRangedWeapon(playerid, currRanged, oldRanged)
	db:exec("UPDATE Player SET rangedWeapon='"..currRanged.."' WHERE playerName = '"..ForumNames[playerid].."'");
end

function OnPlayerChangeArmor(playerid, currArmor, oldArmor)
	db:exec("UPDATE Player SET armor='"..currArmor.."' WHERE playerName = '"..ForumNames[playerid].."'");
end

function Log(filename, text)
	local logfile = io.open(filename..".txt", "a+");
	logfile:write(os.date("%d.%m.%Y %H:%M:%S ")..text.."\n");
	logfile:close();
end

function OnPlayerDeath(playerid, killerid)
	SendMessageToPlayer(playerid, "Du bist tot. Bitte neu Einloggen");
	Kick(playerid);
end

function OnPlayerText(playerid, text)
	if chatmode == 0 then
		SendMessageToAllPlayers(GetPlayerName(playerid)..": "..text);
		Log("OOC", GetPlayerName(playerid)..": "..text);

	elseif chatmode == 1 then
		SendMessageToPlayer(playerid, GetPlayerName(playerid)..": "..text);

		for player = 0, GetMaxPlayers() - 1, 1 do
			if IsPlayerConnected(player) == 1 then
				if GetDistancePlayers(playerid, player) < 500 and playerid ~= player then
					SendMessageToPlayer(player, GetPlayerName(playerid)..": "..text);
				end
			end
		end

		Log("RoleplayChat", GetPlayerName(playerid)..": "..text);
	end
end

function OnPlayerCommandText(playerid, cmdtext)
	local cmd, text = GetCommand(cmdtext);

	if cmd == "/help" then
		SendMessageToPlayer(playerid, "/setname (name) -> Deine Namen aendern.");
		SendMessageToPlayer(playerid, "/face (id), /head (id), /skin (id), /body (id) -> Dein Aussehen");
		SendMessageToPlayer(playerid, "/dice -> Dices a number between 1 and 6");
		SendMessageToPlayer(playerid, "/ani -> Animations liste");
		SendMessageToPlayer(playerid, "/ani (name) -> Animation abspielen");
		SendMessageToPlayer(playerid, "/walk -> dein Walkstyle");
		SendMessageToPlayer(playerid, "/walk (id) -> Walkstyle aendern.");
		SendMessageToPlayer(playerid, "/me (text) -> Animation");
		SendMessageToPlayer(playerid, "/x (text) -> ");
		SendMessageToPlayer(playerid, "/s (text) -> ");

	elseif cmd == "/s" then
		local result, message = sscanf(text, "s");

		if chatmode == 1 then
			SendMessageToPlayer(playerid, GetPlayerName(playerid).." shouts: "..message);

			for player = 0, GetMaxPlayers() - 1, 1 do
				if IsPlayerConnected(player) == 1 and playerid ~= player then
					if GetDistancePlayers(playerid, player) < 2000 then
						SendMessageToPlayer(player, GetPlayerName(playerid).." shouts: "..message);
					end
				end
			end

			Log("RoleplayChat", GetPlayerName(playerid).." shouts: "..message);
		else
			SendMessageToPlayer(playerid, "Shouts are only necessary in roleplay mode which isn't active at the moment");
		end

	elseif cmd == "/me" then
		local result, message = sscanf(text, "s");

		if result == 1 then
			if chatmode == 0 then
				SendMessageToAllPlayers(GetPlayerName(playerid).. " "..message);
				Log("GlobalChat", GetPlayerName(playerid).." "..message);

			elseif chatmode == 1 then
				SendMessageToPlayer(playerid, GetPlayerName(playerid).. " "..message);

				for player = 0, GetMaxPlayers() - 1, 1 do
					if IsPlayerConnected(player) == 1 and playerid ~= player then
						if GetDistancePlayers(playerid, player) < 2000 then
							SendMessageToPlayer(player, GetPlayerName(playerid).. " "..message);
						end
					end
				end

				Log("RoleplayChat", GetPlayerName(playerid).." "..message);
			end
		end

	elseif cmd == "/x" then
		local result, message = sscanf(text, "s");

		if result == 1 then
			if chatmode == 0 then
				SendMessageToAllPlayers(message);
				Log("GlobalChat", message);

			elseif chatmode == 1 then
				SendMessageToPlayer(playerid, message);

				for player = 0, GetMaxPlayers() - 1, 1 do
					if IsPlayerConnected(player) == 1 and playerid ~= player then
						if GetDistancePlayers(playerid, player) < 2000 then
							SendMessageToPlayer(player, message);
						end
					end
				end

			Log("RoleplayChat", message);
		end
	end

	elseif cmd == "/dice" then
		local rand = math.random(6);

		SendMessageToAllPlayers(GetPlayerName(playerid).." diced a "..rand);

		if chatmode == 0 then
			Log("GlobalChat", GetPlayerName(playerid).." diced a "..rand);
		elseif chatmode == 1 then
			Log("GlobalChat", GetPlayerName(playerid).." diced a "..rand);
		end

	elseif cmd == "/globalchat" then
		if IsPlayerAdmin(playerid) == 1 then
			chatmode = 0;

			for line = 1, 30, 1 do
				SendPlayerMessage(playerid, 255, 255, 255, "");
			end

			SendMessageToAllPlayers(GetPlayerName(playerid).." activated the global chat");
		end

	elseif cmd == "/roleplaychat" then
		if IsPlayerAdmin(playerid) == 1 then
			chatmode = 1;

			for line = 1, 30, 1 do
				SendPlayerMessage(playerid, 255, 255, 255, "");
			end

			SendMessageToAllPlayers(GetPlayerName(playerid).." activated the roleplay chat");
		end

	elseif cmd == "/srvmsg" then
		if IsPlayerAdmin(playerid) == 1 then
			local result, message = sscanf(text, "s");

			if result == 1 then
				SendMessageToAllPlayers("[Server]".." "..message);
				Log("ServerChat", "Message by "..GetPlayerName(playerid)..": "..message);
			end
		end

	elseif cmd == "/setname" then
		local result, name = sscanf(text, "s");

		if result == 1 then
			for results in db:nrows("SELECT charName FROM 'Player' WHERE charName='"..name.."' AND playerName = '"..ForumNames[playerid].."'") do
				SendMessageToPlayer(playerid, "A character with this name exists already");
			end

			SetPlayerName(playerid, name);
			SendMessageToPlayer(playerid, "Changed your name to: "..GetPlayerName(playerid));

			db:exec("UPDATE Player SET charName='"..name.."' WHERE playerName = '"..ForumNames[playerid].."'");
		end

	elseif cmd == "/giveitem" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end

		local result, id, instance, amount = sscanf(text, "dsd");

		if result == 1 then
			if IsPlayerConnected(id) ~= 1 then
				SendMessageToPlayer(playerid, "No player online at slot id");
				return;
			end

			GiveItem(id, instance, amount);

			SendMessageToPlayer(id, "You've received "..amount.." "..instance.." from "..GetPlayerName(playerid));
			SendMessageToPlayer(playerid, GetPlayerName(id).." received "..amount.." "..instance.." from you");
		end

	elseif cmd == "/heal" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end

		local result, id = sscanf(text, "d");

		if result == 1 then
			if IsPlayerConnected(id) ~= 1 then
				SendMessageToPlayer(playerid, "The specified player isn't online");
				return;
			end

			SetPlayerHealth(id, GetPlayerMaxHealth(id));

			SendMessageToAllPlayers(GetPlayerName(playerid).." healed "..GetPlayerName(id));
		else
			SendMessageToPlayer(playerid, "Wrong format. Please use: /heal (playerID)");
		end

	elseif cmd == "/teleport" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end

		local result, fromID, toID = sscanf(text, "dd");

		if result == 1 then
			if IsPlayerConnected(fromID) ~= 1 or IsPlayerConnected(toID) ~= 1 then
				SendMessageToPlayer(playerid, "One or both players aren't online");
				return;
			end

			local x, y, z = GetPlayerPos(toID);

			SetPlayerPos(fromID, x, y, z);
			SendMessageToAllPlayers(GetPlayerName(playerid).." teleported "..GetPlayerName(fromID).." to "..GetPlayerName(toID));
		else
			SendMessageToPlayer(playerid, "Wrong format. Please use: /teleport (fromID), (toID)");
		end

	elseif cmd == "/kick" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end

		local result, id, reason = sscanf(text, "ds");

		if result == 1 then
			if IsPlayerConnected(id) ~= 1 then
				SendMessageToPlayer(playerid, "The specified player isn't online");
				return;
			end

			SendMessageToAllPlayers(GetPlayerName(playerid).." kicked player "..GetPlayerName(id)..". Reason: "..reason);
			Kick(id);
		end


		local result, id = sscanf(text, "d");

		if result == 1 then
			if IsPlayerConnected(id) ~= 1 then
				SendMessageToPlayer(playerid, "The specified player isn't online");
				return;
			end

			SendMessageToAllPlayers(GetPlayerName(playerid).." kicked player "..GetPlayerName(id)..". No reason specified");
			Kick(id);
			return;
		end

		SendMessageToPlayer(playerid, "Wrong format. Please use: /kick (playerID)");

	elseif cmd == "/ban" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end


		local result, id, reason = sscanf(text, "ds");

		if result == 1 then
			if IsPlayerConnected(id) ~= 1 then
				SendMessageToPlayer(playerid, "The specified player isn't online");
				return;
			end

			SendMessageToAllPlayers(GetPlayerName(playerid).." banned player "..GetPlayerName(id)..". Reason: "..reason);
			Ban(id);
			return;
		end


		local result, id = sscanf(text, "d");

		if result == 1 then
			if IsPlayerConnected(id) ~= 1 then
				SendMessageToPlayer(playerid, "The specified player isn't online");
				return;
			end

			SendMessageToAllPlayers(GetPlayerName(playerid).." banned player "..GetPlayerName(id)..". No reason specified");
			Ban(id);
			return;
		end

		SendMessageToPlayer(playerid, "Wrong format. Please use: /ban (playerID)");

	elseif cmd == "/maintance" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end

		SendMessageToAllPlayers("A server maintance was initiated. Please check back later");

		maintance = 1;

		for player = 0, GetMaxPlayers() - 1, 1 do
			if IsPlayerConnected(player) == 1 and player ~= playerid then
				Kick(player);
			end
		end

	elseif cmd == "/stopmaintance" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end

		SendMessageToPlayer(playerid, "The server maintance ended. From now on players are free to join again");

		maintance = 0;
		
	elseif cmd == "/setpos" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end
	
		local result, x, y, z = sscanf(text, "ddd");
		
		if result == 1 then
			SetPlayerPos(playerid, x, y, z);
			SendMessageToPlayer(playerid, "Your position was changed");
		else
			SendMessageToPlayer(playerid, "Wrong format. Please use: /position (posX) (posY) (posZ)");
		end
		
	elseif cmd == "/getpos" then
		if IsPlayerAdmin(playerid) ~= 1 then
			return;
		end
	
		local x, y, z = GetPlayerPos(playerid);
		local angle = GetPlayerAngle(playerid);
		
		posFile = io.open("positions", "a+");
		
		posFile:write("position: "..x..", "..y..", "..z.." angle: "..angle);
		posFile:close();
		
		SendMessageToPlayer(playerid, "Your position was saved");

	elseif cmd == "/ani" then
		local result, aniname = sscanf(text, "s");

		if result == 1 then
			if Animations[aniname] ~= nil then
				PlayAnimation(playerid, Animations[aniname]);
			else
				SendMessageToPlayer(playerid, "No animation is assigned to the name "..aniname);
			end
		else
			SendMessageToPlayer(playerid, "Following animations are available (/ani (aniName))");

			local aniList = "";
			local aniCount = 0;

			for animation in pairs(Animations) do
				aniList = aniList.." "..animation;
				aniCount = aniCount + 1;

				if aniCount == 5 then
					SendMessageToPlayer(playerid, aniList);

					aniList = "";
					aniCount = 0;
				end
			end

			SendMessageToPlayer(playerid, aniList);
		end

	elseif cmd == "/walk" then
		local result, walkid = sscanf(text, "d");

		if result == 1 then
			if Walkstyles[walkid] ~= nil then
				SetPlayerWalk(playerid, Walkstyles[walkid]);
			else
				SendMessageToPlayer(playerid, "No walkstyle is assigned with the ID "..walkid.." (available are 1-7)");
			end
		else
			SendMessageToPlayer(playerid, "Following IDs can be used to set your walkstyle (/walk (walkID))");

			walkList = "";
			walkCount = 0;

			for walkstyle in pairs(Walkstyles) do
				walkList = walkList.." "..walkstyle.." = "..Walkstyles[walkstyle];
				walkCount = walkCount + 1;

				if walkCount == 5 then
					SendMessageToPlayer(playerid, walkList);

					walkList = "";
					walkCount = 0;
				end
			end

			SendMessageToPlayer(playerid, walkList);
		end

	elseif cmd == "/face" then
		local result, faceid = sscanf(text, "d");

		if result == 1 then
			if faceid >= 0 and faceid <= 162 then
				local bodyModel, bodyTextureID, headModel, headTextureID = GetPlayerAdditionalVisual(playerid);

				SetPlayerAdditionalVisual(playerid, bodyModel, bodyTextureID, headModel, faceid);
				db:exec("UPDATE Player SET headTextureID="..faceid.." WHERE playerName = '"..ForumNames[playerid].."'");
			else
				SendMessageToPlayer(playerid, "No face found. Face IDs: 0-162");
			end
		else
			SendMessageToPlayer(playerid, "Wrong format. Please use: /face (faceID)");
		end

	elseif cmd == "/skin" then
		local result, skinid = sscanf(text, "d");

		if result == 1 then
			if skinid >= 0 and skinid <= 7 then
				local bodyModel, bodyTextureID, headModel, headTextureID = GetPlayerAdditionalVisual(playerid);

				SetPlayerAdditionalVisual(playerid, bodyModel, skinid, headModel, headTextureID);
				db:exec("UPDATE Player SET bodyTextureID="..skinid.." WHERE playerName = '"..ForumNames[playerid].."'");
			else
				SendMessageToPlayer(playerid, "No skin found. Skin IDs: 0-7");
			end
		else
			SendMessageToPlayer(playerid, "Wrong format. Please use: /skin (skinID)");
		end

	elseif cmd == "/body" then
		local result, bodyid = sscanf(text, "d");

		if result == 1 then
			if bodyMeshes[bodyid] ~= nil then
				local bodyModel, bodyTextureID, headModel, headTextureID = GetPlayerAdditionalVisual(playerid);

				SetPlayerAdditionalVisual(playerid, bodyMeshes[bodyid], bodyTextureID, headModel, headTextureID);
				db:exec("UPDATE Player SET bodyModel='"..bodyMeshes[bodyid].."' WHERE playerName = '"..ForumNames[playerid].."'");
			else
				SendMessageToPlayer(playerid, "No body found. Body IDs: 1-2");
			end
		else
			SendMessageToPlayer(playerid, "Wrong format. Please use: /body (bodyID)");
		end

	elseif cmd == "/head" then
		local result, headid = sscanf(text, "d");

		if result == 1 then
			if headMeshes[headid] ~= nil then
				local bodyModel, bodyTextureID, headModel, headTextureID = GetPlayerAdditionalVisual(playerid);

				SetPlayerAdditionalVisual(playerid, bodyModel, bodyTextureID, headMeshes[headid], headTextureID);
				db:exec("UPDATE Player SET headModel='"..headMeshes[headid].."' WHERE playerName = '"..ForumNames[playerid].."'");
			else
				SendMessageToPlayer(playerid, "No head found. Head IDs: 1-7");
			end
		else
			SendMessageToPlayer(playerid, "Wrong format. Please use: /head (headID)");
		end
	end
end