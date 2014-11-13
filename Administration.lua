print("Loading Administration.lua");

-----
--GLOBAL VARS
-----


--is set to true if the server is shutting down
-- -> no more players can connect
serverIsShuttingDown = false;

instance_Administration = nil;

Administration = {};
Administration.__index = Administration;

---Creates an instance of Administration
function Administration.new()
	local newAdministration = {};
	
	setmetatable(newAdministration, Administration);
	
	return newAdministration;
end

---Catch Callback: OnPlayerCommandText
function Administration:OnPlayerCommandText(playerid, cmdtext)

	local character = getCharacterById(playerid);
	
	if not character then return; end;
	
	local cmd, params = GetCommand(cmdtext);
	
	--determines whether the command was recognized or not
	local recognized = 0;
	
	--check administrator commands
	if character.rights >= 2 then
		recognized = self:checkAdminCommands(playerid, cmd, params);
	end
	
	--check game manager commands
	if character.rights >= 1 and recognized == 0 then
		recognized = self:checkGameManagerCommands(playerid, cmd, params);
	end
	
	--check player commands
	if recognized == 0 then
		recognized = self:checkPlayerCommands(playerid, cmd, params);
	end
	
	if recognized == 1 then
		LogString("Administration", string.format("%s used command: %s", GetPlayerName(playerid), cmdtext));
		
	else
		LogString("Administration", string.format("[NOT RECOGNIZED] %s used command: %s", GetPlayerName(playerid), cmdtext));
		
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Der Befehl %s ist nicht bekannt.", cmdtext));
	end
end


---Checks the input for the commands an administrator can execute
function Administration:checkAdminCommands(playerid, cmd, params)
	if cmd == "/set_char_skills" then
		local result, value = sscanf(params,"d");
    
		local character = getCharacterById(playerid);
		
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
			character:setSkillValue(v.skillID, value)
		end
    
		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast jetzt in jedem Skill einen Wert von %d.", value));

	elseif cmd == "/set_all_char_skills" then
		local result, value = sscanf(params,"d");
    
		local character = nil;

		for i = 0, GetMaxPlayers() - 1 do
			if IsPlayerConnected(i) == 1 then
				character = getCharacterById(i);
				
				if character then
					for k, v in pairs(instance_TeachSystem.skillMan.skills) do
						character:setSkillValue(v.skillID, value)
					end
					
					SendPlayerMessage(i, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast jetzt in jedem Skill einen Wert von %d.", value));
				end
			end
		end
		
	
		
	elseif cmd == "/po" then
		
		local fho,err = io.open("position.txt","w");
		local fh;
		while true do
        local line = fho:read();
        if line == nil then
			local pos = GetPlayerPos(playerid);
			print(pos);
			fho:write(pos);
            fho:write("\n");
			break;
		end
      
	end
	
	

	elseif cmd == "/giveitem" then
		local result, value = sscanf(params,"s");
		if value == nil or not value then
		SendPlayerMessage(playerid,255,255,0," /giveitem itemname!");
		return;
		end
		GiveItem(playerid,value,1);
		
		
		
       elseif cmd == "/set_schatten" then
		local result, value = sscanf(params,"d");
		value = 45;
		GiveItem(playerid,"ItRw_Arrow",2000);	 --vllt zuviel?
		SetPlayerStrength(playerid,20);
		SetPlayerMaxMana(playerid,10);
		SetPlayerMana(playerid,10);
		SetPlayerDexterity(playerid,15);
		SetPlayerMaxHealth(playerid,75);
		SetPlayerHealth(playerid,75);
		SetPlayerSkillWeapon(playerid, 3, 10);
		SetPlayerSkillWeapon(playerid, 1, 20);
		local character = getCharacterById(playerid);
		
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end

       elseif cmd == "/set_jaeger" then
		local result, value = sscanf(params,"d");
		value = 45;
		GiveItem(playerid,"ItRw_Arrow",2000);	 --vllt zuviel?
		SetPlayerStrength(playerid,20);
		SetPlayerMaxMana(playerid,10);
		SetPlayerMana(playerid,10);
		SetPlayerDexterity(playerid,15);
		SetPlayerMaxHealth(playerid,75);
		SetPlayerHealth(playerid,75);
		SetPlayerSkillWeapon(playerid, 3, 10);
		SetPlayerSkillWeapon(playerid, 1, 20);
		local character = getCharacterById(playerid);
		
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		
	elseif cmd == "/set_duado" then
		local result, value = sscanf(params,"d");
		value = 45;
		GiveItem(playerid,"ItRw_Crossbow_M_01",1);
		GiveItem(playerid,"ItRw_Bolt",2000);
		SetPlayerStrength(playerid,30);
		SetPlayerMaxMana(playerid,10);
		SetPlayerMana(playerid,10);
		SetPlayerDexterity(playerid,20);
		SetPlayerMaxHealth(playerid,120);
		SetPlayerHealth(playerid,120);
		SetPlayerSkillWeapon(playerid, 0, 30);
		SetPlayerSkillWeapon(playerid, 1, 30);
		SetPlayerSkillWeapon(playerid, 2, 10);
		SetPlayerSkillWeapon(playerid, 3, 20);
		local character = getCharacterById(playerid);

		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		
		
		elseif cmd == "/set_templer" then
		local result, value = sscanf(params,"d");
		value = 45;
		GiveItem(playerid,"ItRw_Crossbow_M_01",1);
		GiveItem(playerid,"ItRw_Bolt",2000);
		SetPlayerStrength(playerid,30);
		SetPlayerMaxMana(playerid,10);
		SetPlayerMana(playerid,10);
		SetPlayerDexterity(playerid,20);
		SetPlayerMaxHealth(playerid,120);
		SetPlayerHealth(playerid,120);
		SetPlayerSkillWeapon(playerid, 0, 30);
		SetPlayerSkillWeapon(playerid, 1, 30);
		SetPlayerSkillWeapon(playerid, 2, 10);
		SetPlayerSkillWeapon(playerid, 3, 20);
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		
		
		elseif cmd == "/set_Banditen" then
        local result, value = sscanf(params,"d");
        value = 45;
		GiveItem(playerid,"ItMw_1H_Special_01",1);
		GiveItem(playerid,"ItRw_Arrow",2000);	 --vllt zuviel?
		SetPlayerStrength(playerid,20);
		SetPlayerMaxMana(playerid,10);
		SetPlayerMana(playerid,10);
		SetPlayerDexterity(playerid,15);
		SetPlayerMaxHealth(playerid,75);
		SetPlayerHealth(playerid,75);
		SetPlayerSkillWeapon(playerid, 3, 10);
		SetPlayerSkillWeapon(playerid, 1, 20);
		
        local character = getCharacterById(playerid);
				for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		
		
		elseif cmd == "/set_basti" then
        local result, value = sscanf(params,"d");
        value = 45;
       	GiveItem(playerid,"ItRw_Crossbow_M_01",1);
		GiveItem(playerid,"ItRw_Bolt",2000);
        SetPlayerStrength(playerid,45);
        SetPlayerMaxMana(playerid,10);
        SetPlayerMana(playerid,10);
        SetPlayerDexterity(playerid,30);
        SetPlayerMaxHealth(playerid,150);
        SetPlayerHealth(playerid,150);
        SetPlayerSkillWeapon(playerid, 0, 40);
        SetPlayerSkillWeapon(playerid, 1, 40);
        SetPlayerSkillWeapon(playerid, 2, 10);
        SetPlayerSkillWeapon(playerid, 3, 30);
        local character = getCharacterById(playerid);
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		
	
	 elseif cmd == "/set_soldner" then
        local result, value = sscanf(params,"d");
        value = 45;
		GiveItem(playerid,"ItRw_Bolt",2000);
        SetPlayerStrength(playerid,45);
        SetPlayerMaxMana(playerid,10);
        SetPlayerMana(playerid,10);
        SetPlayerDexterity(playerid,30);
        SetPlayerMaxHealth(playerid,150);
        SetPlayerHealth(playerid,150);
        SetPlayerSkillWeapon(playerid, 0, 40);
        SetPlayerSkillWeapon(playerid, 1, 40);
        SetPlayerSkillWeapon(playerid, 2, 30);
        SetPlayerSkillWeapon(playerid, 3, 10);
        local character = getCharacterById(playerid);
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		
		
		elseif cmd == "/set_Guru" then
        local result, value = sscanf(params,"d");
        value = 45;
        SetPlayerStrength(playerid,10);
        SetPlayerMaxMana(playerid,50);
        SetPlayerMana(playerid,50);
        SetPlayerMaxHealth(playerid,100);
        SetPlayerHealth(playerid,100);
        SetPlayerSkillWeapon(playerid, 0, 10);
        SetPlayerSkillWeapon(playerid, 1, 15);
	SetPlayerScience(playerid,1);
        local character = getCharacterById(playerid);
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		
		
		 elseif cmd == "/set_Wassermagier" then
        local result, value = sscanf(params,"d");
        value = 45;
        SetPlayerStrength(playerid,10);
        SetPlayerMaxMana(playerid,75);
        SetPlayerMana(playerid,75);
        SetPlayerMaxHealth(playerid,100);
        SetPlayerHealth(playerid,100);
	SetPlayerScience(playerid,1);
        SetPlayerSkillWeapon(playerid, 0, 0);
        SetPlayerSkillWeapon(playerid, 1, 15);
        local character = getCharacterById(playerid);
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		
		
		elseif cmd == "/set_feuermagier" then
        local result, value = sscanf(params,"d");
        value = 45;
        SetPlayerStrength(playerid,10);
        SetPlayerMaxMana(playerid,75);
        SetPlayerMana(playerid,75);
        SetPlayerMaxHealth(playerid,100);
        SetPlayerHealth(playerid,100);
	SetPlayerScience(playerid,1);
        SetPlayerSkillWeapon(playerid, 0, 0);
        SetPlayerSkillWeapon(playerid, 1, 15);
        local character = getCharacterById(playerid);
		for k, v in pairs(instance_TeachSystem.skillMan.skills) do
		if v.skillID == 5 or v.skillID == 6 or v.skillID == 7 then
		print("fickscheise");
			character:setSkillValue(v.skillID, value);
			end
		end
		

		elseif cmd == "/sumpflager" then
		GiveItem(playerid,"ItMi_Herbpaket",30);
		GiveItem(playerid,"ItMi_Addon_Joint_01",200);
		GiveItem(playerid,"ItMw_1H_Bau_Axe",30);
		GiveItem(playerid,"ItMi_SumpfTabak",200);
		
		elseif cmd == "/ration" then
		GiveItem(playerid,"ItMi_Nugget",1000);
		GiveItem(playerid,"ItFo_Apple",30);
		GiveItem(playerid,"ItFo_Beer",30);
		GiveItem(playerid,"ItFo_Fish",30);
		GiveItem(playerid,"ItFo_FishSoup",30);
		GiveItem(playerid,"ItFo_Gebratenerschinken",30);
		GiveItem(playerid,"ItFo_Cheese",30);
		GiveItem(playerid,"ItFo_Water",30);
		GiveItem(playerid,"ItFo_Kochwurst",30);
		GiveItem(playerid,"ItFo_Milk",30);
		GiveItem(playerid,"ItFoMuttonRaw",30);
		GiveItem(playerid,"ItFo_Bread",30);
		GiveItem(playerid,"ItFo_Wine",30);
		GiveItem(playerid,"ItMi_Broom",30);
		GiveItem(playerid,"ItMw_2H_Axe_L_01",30);
		GiveItem(playerid,"ItMi_Pan",30);
		GiveItem(playerid,"ItMw_1H_Mace_L_04",1);
		GiveItem(playerid,"ItMiSwordraw",1);
		GiveItem(playerid,"ItMiSwordrawhot",1);
		GiveItem(playerid,"ItMiSwordbladehot",1);
		GiveItem(playerid,"ItMiSwordblade",1);
		GiveItem(playerid,"ItMw_Kochloeffel",30);
		
	
	elseif cmd == "/reload_database" then
		instance_CraftingSystem:reload();
		instance_TeachSystem:reload();
	
	elseif cmd == "/shutdown_server" then
		SendMessageToAll(COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Wir fuehren einen Serverneustart durch. Du wirst in %d Sekunden automatisch abgemeldet. Der Server wird in wenigen Minuten wieder online sein.", GENERAL_SERVER_RESTART_ANNOUNCE_TIME));
		
		SetTimer("shutdownServer", GENERAL_SERVER_RESTART_ANNOUNCE_TIME * 1000, 0);
		
	elseif cmd == "/set_server_time" then
		local result, val = sscanf(params,"d");
		
		SetTime(val, 0);
		
	elseif cmd == "/set_server_password" then
		local result, pw1, pw2, pwold = sscanf(params,"sss");
		
		if pwold ~= self:getAdminPassword() then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du musst das korrekte aktuelle Password angeben.");
			return;
		end
		
		if pw1 ~= pw2 then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Das neue Passwort und seine Wiederholung muessen uebereinstimmen.");
			return;
		end
		
		self:changeAdminPassword(playerid, pw1);
		
	elseif cmd == "/give" then
		local result, amount = sscanf(params,"d");
		
		if not amount then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du musst eine Anzahl an Items angeben (/give <Anzahl>)");
			return;
		end
		
		for k, v in pairs(instance_InventorySystem.inventoryMan.inventoryCategories) do
			for k2, v2 in pairs(v.items) do
				GiveItem(playerid, v2.instanceName, amount);
			end
		end
	
		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast jedes Item %dx bekommen.", amount));

	elseif cmd == "/set_teacher_level" then
		local result, level = sscanf(params,"d");
		
		if not level then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du musst ein Level angeben (/set_teacher_level <Level>)");
			return;
		end
		
		if level < 0 or level > 1 then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Das Level muss zwischen 0 und 1 liegen.");
			return;
		end
		
		local character = getCharacterById(playerid);
		
		for k, v in pairs(instance_TeachSystem.teachMan.teachCategories) do
			character:setTeacherRank(v.categoryID, level)
		end
		
		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast jetzt Lehrerlevel %d in jeder Kategorie.", level));
		
	elseif cmd == "/spawner_switch" then
		if (SPAWNER_PLANTS_001:isRunning()) then
			SPAWNER_PLANTS_001:stop();
		else
			SPAWNER_PLANTS_001:resume();
		end
		SendPlayerMessage(playerid, 100, 255, 255, string.format("Spawner.running = %s", tostring(SPAWNER_PLANTS_001:isRunning())));
	elseif cmd == "/sa" then
		local x,y,z = GetPlayerPos(playerid);
		-- für knapp über dem Bodenniveau
		y = y - 80;
		SPAWNER_PLANTS_001:collInsert(0, 1, 1, {id=-1, x=x, y=y, z=z, properties={prob=1}}, false);
		local length = #SPAWNER_PLANTS_001.getPriv().Spatials[1]:getFeatures();
		SendPlayerMessage(playerid, 100, 255, 255, string.format("(%d) Punkt hinzugefügt bei x=%d y=%d z=%d.", length, x, y, z));
		-- SendPlayerMessage(playerid, 100, 255, 255, "Spawner-Status=" .. SPAWNER_PLANTS_001:isRunning());
	elseif cmd == "/srl" then
		local length = #SPAWNER_PLANTS_001.getPriv().Spatials[1]:getFeatures();
		local x = SPAWNER_PLANTS_001.getPriv().Spatials[1]:getFeatures()[length].x
		local y = SPAWNER_PLANTS_001.getPriv().Spatials[1]:getFeatures()[length].y
		local z = SPAWNER_PLANTS_001.getPriv().Spatials[1]:getFeatures()[length].z
		SPAWNER_PLANTS_001:collRemove(0, 1, length);
		length = #SPAWNER_PLANTS_001.getPriv().Spatials[1]:getFeatures();
		SendPlayerMessage(playerid, 100, 255, 255, string.format("(%d) Punkt gelöscht bei x=%d y=%d z=%d.", length, x, y, z));
	elseif cmd == "/sr" then
		local Spatials = SPAWNER_PLANTS_001:getSpatials();
		local x,y,z = GetPlayerPos(playerid);
		x = tonumber(x); y = tonumber(y); z = tonumber(z);
		if (x and y and z) then
			local nearest,lookupIndex,collIndex,index,nearestDist = SPAWNER_PLANTS_001:nearestSpatialMember(x, y, z);
			print(nearest,lookupIndex,collIndex,index,nearestDist);
			if (nearest) then
				SPAWNER_PLANTS_001:collRemove(0, collIndex, index);
				SendPlayerMessage(playerid, 100, 255, 255, string.format("(%d) Punkt gelöscht bei x=%d y=%d z=%d.", #Spatials[collIndex]:getFeatures(), nearest.x, nearest.y, nearest.z));
			end
		end
	elseif cmd == "/spawner_clear" then
		local running = SPAWNER_PLANTS_001:isRunning();
		SPAWNER_PLANTS_001:stop();
		SPAWNER_PLANTS_001:clearColl(0, 1);
		if (running) then
			SPAWNER_PLANTS_001:resume();
		end
		SendPlayerMessage(playerid, 100, 255, 255, "Alle Spawn-Punkte gelöscht.");
	elseif cmd == "/spawner_rh" then
		local running = SPAWNER_PLANTS_001:isRunning();
		SPAWNER_PLANTS_001:stop();
		SPAWNER_PLANTS_001:resetHistory();
		if (running) then
			SPAWNER_PLANTS_001:resume();
		end
		SendPlayerMessage(playerid, 100, 255, 255, "Spawn-Historie resetted.");
	elseif cmd == "/sh" then
		SendPlayerMessage(playerid, 100, 255, 255, "Spawner-Historie wird gespeichert...");
		local running = SPAWNER_PLANTS_001:isRunning();
		SPAWNER_PLANTS_001:stop();
		SPAWNER_PLANTS_001:saveHistory();
		if (running) then
			SPAWNER_PLANTS_001:resume();
		end
		SendPlayerMessage(playerid, 100, 255, 255, "... Speichervorgang gespeichert.");
	elseif cmd == "/spawner_save" then
		SendPlayerMessage(playerid, 100, 255, 255, "SPAWNER_PLANTS_001 wird speichert...");
		local running = SPAWNER_PLANTS_001:isRunning();
		SPAWNER_PLANTS_001:stop();
		-- SPAWNER_PLANTS_001:saveAll();
		SPAWNER_PLANTS_001:saveCollections(0);
		-- SPAWNER_PLANTS_001:saveCollections(1);
		SPAWNER_PLANTS_001:saveHistory();
		if (running) then
			SPAWNER_PLANTS_001:resume();
		end
		SendPlayerMessage(playerid, 100, 255, 255, "... Speichervorgang beendet.");
	elseif cmd == "/spawner_load" then
		local priv 		= SPAWNER_PLANTS_001.getPriv();
		local running 	= SPAWNER_PLANTS_001:isRunning();
		SendPlayerMessage(playerid, 100, 255, 255, "Spawn-Punkte werden geladen...");
		SPAWNER_PLANTS_001:stop();
		priv.Spatials 	= {};
		priv.Spawns 	= {};
		SPAWNER_PLANTS_001:loadCollections(0);
		SPAWNER_PLANTS_001:loadCollections(1);
		if (running) then
			SPAWNER_PLANTS_001:resume();
		end
		SendPlayerMessage(playerid, 100, 255, 255, "... Ladevorgang beendet.");
	elseif cmd == "/spawner_incit" then
		local running = SPAWNER_PLANTS_001:isRunning();
		SPAWNER_PLANTS_001:stop();
		SPAWNER_PLANTS_001:setIterTime(SPAWNER_PLANTS_001:getIterTime() + 500);
		if (running) then
			SPAWNER_PLANTS_001:resume();
		end
		SendPlayerMessage(playerid, 100, 255, 255, string.format("Spawn-Itervall=%d", SPAWNER_PLANTS_001:getIterTime()));
	elseif cmd == "/spawner_decit" then
		local running = SPAWNER_PLANTS_001:isRunning();
		SPAWNER_PLANTS_001:stop();
		SPAWNER_PLANTS_001:setIterTime(SPAWNER_PLANTS_001:getIterTime() - 500);
		if (running) then
			SPAWNER_PLANTS_001:resume();
		end
		SendPlayerMessage(playerid, 100, 255, 255, string.format("Spawn-Itervall=%d", SPAWNER_PLANTS_001:getIterTime()));
	elseif cmd == "/sm" then
		SendPlayerMessage(playerid, 100, 255, 255, "Marking Spatials...");
		SPAWNER_PLANTS_001:markSpatials();
		SendPlayerMessage(playerid, 100, 255, 255, "Done marking Spatials...");
	elseif cmd == "/sum" then
		SendPlayerMessage(playerid, 100, 255, 255, "Unmarking Spatials...");
		SPAWNER_PLANTS_001:unmarkSpatials();
		SendPlayerMessage(playerid, 100, 255, 255, "Done unmarking Spatials...");
	
	--Saves a Waypoint for the Position and angle of the Command-Sender  with the specified Waypoint name
	elseif cmd == "/savewp" then
		local waypointname = string.match(params, "([%w%p]+)");
		
		if SaveWaypointAtPlayer(playerid, waypointname) then
			SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Waypoint '" .. waypointname .. "' gespeichert");
		else
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Waypoint '" .. waypointname .. "' nicht gespeichert");
		end
	
	elseif cmd == "/deletewp" then
		local waypointname = string.match(params, "([%w%p]+)");
		
		if DeleteWaypoint(waypointname) then
			SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Waypoint '" .. waypointname .. "' gelöscht.");
		else
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Waypoint '" .. waypointname .. "' nicht gelöscht.");
		end
	
	elseif cmd == "/sheep" then
		local focusID = GetFocus(playerid);
		if focusID == -1 then return; end
		
		SetPlayerInstance(focusID, "SHEEP");
		
	elseif cmd == "/inst" then
		local result, instanceName = sscanf(params,"s");
		
		SetPlayerInstance(playerid, instanceName);
		
	elseif cmd == "/ork1" then
		SetPlayerInstance(playerid, "ORCELITE_REST");
		
	elseif cmd == "/ork2" then
		SetPlayerInstance(playerid, "orkelite_antipaladinorkoberst");
		
	elseif cmd == "/ork3" then
		SetPlayerInstance(playerid, "orcwarrior_rest");
		
	elseif cmd == "/pos" then
		local x,y,z = GetPlayerPos(playerid);
		SendPlayerMessage(playerid, 100, 255, 255, "Pos: ..x..y..z");
		
	elseif cmd == "/set_char_scale" then
		local id, scaleX, scaleY, scaleZ = string.match(params, "(%d) (%d) (%d) (%d)");
		id 		= tonumber(id);
		scaleX 	= tonumber(scaleX);
		scaleY 	= tonumber(scaleY);
		scaleZ 	= tonumber(scaleZ);
		if (id and scaleX and scaleY and scaleZ) then
			SetPlayerScale(id, scaleX, scaleY, scaleZ);
		end
	elseif cmd == "/npc" then
		local x, y, z 	= GetPlayerPos(playerid);
		local params 	= string.upper(params);
		local npc 		= CreateNPC(params);
		SetPlayerInstance(npc, params);
		SetPlayerPos(npc, x, y, z);
	elseif cmd == "/fanim" then
		local id, anim 	= string.match(params, "(%d+) ([%w_]+)");
		id				= tonumber(id) 
		anim 			= string.upper(anim);
		print(id, type(id), anim, type(anim));
		PlayAnimation(id, anim);
	else
		return 0;
	end
	
	return 1;
end


---Checks the input for the commands a gamemanager can execute
function Administration:checkGameManagerCommands(playerid, cmd, params)
	if cmd == "/kick" then
		local result, id, reason = sscanf(params,"ds");
		
		self:kickPlayer(playerid, id, reason);
    
	elseif cmd == "/ban" then
		local result, id, reason = sscanf(params,"ds");
		
		self:banPlayer(playerid, id, reason);
    
	elseif cmd == "/warn" then
		local result, id, reason = sscanf(params,"ds");
		
		self:warnPlayer(playerid, id, reason);

	elseif cmd == "/tp" then
		local to = tonumber(params);
		
		if to then
			local to_x, to_y, to_z = GetPlayerPos(to);
			
			if (to_x and to_y and to_z) then
				SetPlayerPos(playerid, to_x, to_y, to_z);
			end
		end
		
	elseif cmd == "/tpcoord" then
		local to_x,to_y,to_z = string.match(params, "([%w%p]+) ([%w%p]+) ([%w%p]+)");
		to_x = tonumber(to_x);
		to_y = tonumber(to_y);
		to_z = tonumber(to_z);
		
		if (to_x and to_y and to_z) then
			SetPlayerPos(playerid, to_x, to_y, to_z);
		end	
		
		print(params, to_x, to_y, to_z);
		
		
		
	--Teleports the Command-Sender to the specified waypoint
	elseif cmd == "/goto" then
		local waypointname = string.match(params, "([%w%p]+)");
		
		if TeleportPlayerToWayPoint(playerid, waypointname) then
			SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Zu '" .. waypointname .. "' teleportiert.");
		else
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Bitte einen gueltigen Waypointname eingeben.");
		end
	else
		return 0;
	end
	
	return 1;
end

---Checks the input for the commands a player can execute
function Administration:checkPlayerCommands(playerid, cmd, params)
	if cmd == "/set_char_rights" then
		local result, value, password = sscanf(params, "ds");
		
		if not password then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du musst das Passwort angeben.");
			return;
		end
		
		if password ~= self:getAdminPassword() then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Falsches Passwort.");
			return;
		end
		
		local character = getCharacterById(playerid);
		
		character:setRights(value);

		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast jetzt Nutzerrechte der Stufe %d", value));
		
	elseif cmd == "/getskills" then
		local focusID = GetFocus(playerid);
		local character;
		
		if focusID == -1 then
			--use player: ME
			character = getCharacterById(playerid);
			
			SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, string.format("Skills von dir: %s", character:skillsToString()));
		else
			--use player: focused player
			character = getCharacterById(focusID);
			
			SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, string.format("Skills von %s: %s", instance_NameSystem:getPlayerNameFromViewOf(playerid, character.playerid), character:skillsToString()));
		end
		
		elseif cmd == "/cam" then
		SetDefaultCamera(playerid);
		
		elseif cmd == "/erzhacken" then
			--FreezePlayer(playerid,1);
			
			if Erzhack[playerid].erzcount > 100 then
			SendPlayerMessage(playerid, 255, 0, 0, "Du hast heute genug Erz verdient.");
			return;
			elseif Erzhack[playerid].erzcond == true then
				Erzhack[playerid].erzcond = false;
				if GetPlayerAnimationID(playerid) == 248 then
					SendPlayerMessage(playerid, 0, 0, 255, "Du faengst an Erz zu hacken!");
					SetTimerEx("erzHacken",10000,0,playerid);
				else
					SendPlayerMessage(playerid, 255, 0, 0, "Du musst an einem Erzklumpen stehen!");
					Erzhack[playerid].erzcond = true;
				end
			
				else
					SendPlayerMessage(playerid, 255, 0, 0, "Du hackst schon.");
			end
			
		
	elseif cmd == "/charedit" then
		CharEditor:new(playerid);
		
		elseif cmd == "/guard1" then
 	PlayAnimation(playerid, "S_LGUARD");
    
 	
 		elseif cmd == "/guard2" then
 	PlayAnimation(playerid, "S_HGUARD");
	
	elseif cmd == "/sit" then
 	PlayAnimation(playerid, "S_SIT");
	
	
	--elseif cmd == "/saegen" then
 	--PlayAnimation(playerid, "S_BAUMSAEGE_S1");
    elseif cmd == "/chill" then
	
 	PlayAnimation(playerid, "S_BEDHIGH_FRONT_S1");
	
 	elseif cmd == "/sleep" then
	PlayAnimation(playerid, "S_SLEEPGROUND");
	--FreezePlayer(playerid, 1);
 	
 	
	--PlayAnimation(playerid, "S_SLEEPGROUND");
	elseif cmd == "/harken" then
 	PlayAnimation(playerid, "S_RAKE_S1");
    elseif cmd == "/wounded" then
 	PlayAnimation(playerid, "S_WOUNDED");
 	elseif cmd == "/cmere" then
 	PlayAnimation(playerid, "T_COMEOVERHERE");	
	
	elseif cmd == "/joint" then
	--EquipMeleeWeapon(playerid, "ItMi_Addon_Joint_01");
 	
	smoke(playerid);
   
	
    elseif cmd == "/pee" then
	--FreezePlayer(playerid, 1);
 	PlayAnimation(playerid, "T_STAND_2_PEE");
	
    elseif cmd == "/wash" then
	--FreezePlayer(playerid, 1);
 	PlayAnimation(playerid, " S_WASH");
 	elseif cmd == "/get" then
 	PlayAnimation(playerid, "T_PLUNDER");	
	
	
	elseif cmd == "/look" then
 	PlayAnimation(playerid, "T_SEARCH");
    elseif cmd == "/yeah" then
 	PlayAnimation(playerid, "T_WATCHFIGHT_YEAH");
	elseif cmd == "/ohno" then
 	PlayAnimation(playerid, "T_WATCHFIGHT_OHNO");
 	elseif cmd == "/greet1" then
 	PlayAnimation(playerid, "T_GREETCOOL");	
	local sound_id = CreateSound("SVM_15_SC_HEYTURNAROUND02.WAV");
	PlayPlayerSound(playerid, sound_id);
		
	elseif cmd == "/dance1" then
 	PlayAnimation(playerid, "T_DANCE_01");
	elseif cmd == "/dance2" then
 	PlayAnimation(playerid, "T_DANCE_02");
	elseif cmd == "/dance3" then
 	PlayAnimation(playerid, "T_DANCE_03");
	elseif cmd == "/dance4" then
 	PlayAnimation(playerid, "T_DANCE_04");
	elseif cmd == "/dance5" then
 	PlayAnimation(playerid, "T_DANCE_05");
	elseif cmd == "/dance6" then
 	PlayAnimation(playerid, "T_DANCE_06");
	elseif cmd == "/dance7" then
 	PlayAnimation(playerid, "T_DANCE_07");
	elseif cmd == "/dance8" then
 	PlayAnimation(playerid, "T_DANCE_08");
	elseif cmd == "/dance9" then
 	PlayAnimation(playerid, "T_DANCE_09");
	elseif cmd == "/dance10" then
 	PlayAnimation(playerid, "T_DANCE_10");
	
    elseif cmd == "/arm" then
 	PlayAnimation(playerid, "T_GREETLEFT");
 	elseif cmd == "/bow" then
 	PlayAnimation(playerid, "T_GREETNOV");		
		
		
		
		
		elseif cmd == "/greet" then
 	PlayAnimation(playerid, "T_LGUARD_GREET"); -- S_ORE_S0 ERZ ANI!!!!!
    
 	elseif cmd == "/kraut" then
 	PlayAnimation(playerid, "S_HERB_S2");
	
 		elseif cmd == "/repair" then
 	PlayAnimation(playerid, "S_REPAIR_S1");
	

 	
 		elseif cmd == "/stand" then
 	PlayAnimation(playerid, "S_STAND");
	elseif cmd == "/dunno" then
 	PlayAnimation(playerid, "T_DONTKNOW");
    
 	elseif cmd == "/drop" then
 	local result, name, amount = sscanf(params,"sd");
	if name == "erz" then
	DropItem(playerid, "ItMi_Nugget", amount);
	return;
	elseif name == "apfel" then
	DropItem(playerid, "ItFo_Apple", amount);
	return;
	elseif name == "brot" then
	DropItem(playerid, "ItFo_Bread", amount);
	return;
	elseif name == "kaese" then
	DropItem(playerid, "ItFo_Cheese", amount);
	return;
	elseif name == "bier" then
	DropItem(playerid, "ItFo_Beer", amount);
	return;
	elseif name == "wein" then
	DropItem(playerid, "ItFo_Wine", amount);
	return;
	elseif name == "wasser" then
	DropItem(playerid, "ItFo_Water", amount);
	return;
	elseif name == "spitzhacke" then
	DropItem(playerid, "ItMw_2H_Axe_L_01", amount);
	return;
	elseif name == "pfanne" then
	DropItem(playerid, "ItMi_Pan", amount);
	return;
	elseif name == "besen" then
	DropItem(playerid, "ItMi_Broom", amount);
	return;
	elseif name == "sichel" then
	DropItem(playerid, "ItMw_1H_Bau_Axe", amount);
	return;
	elseif name == "kochloeffel" then
	DropItem(playerid, "ItMi_Scoop", amount);
	return;
	elseif name == "kraut" then
	DropItem(playerid, "ItMi_Stomper", amount);
	return;
	elseif name == "lute" then
	DropItem(playerid, "ItMi_Lute", amount);
	return;
	elseif name == "fisch" then
	DropItem(playerid, "ItFo_Fish", amount);
	return;
	elseif name == "fischsuppe" then
	DropItem(playerid, "ItFo_FishSoup", amount);
	return;
	elseif name == "gschinken" then
	DropItem(playerid, "ItFo_Gebratenerschinken", amount);
	return;
	elseif name == "milch" then
	DropItem(playerid, "ItFo_Milk", amount);
	return;
	elseif name == "rfleisch" then
	DropItem(playerid, "ItFoMuttonRaw", amount);
	return;
	elseif name == "heiltrank" then
	DropItem(playerid, "ItPo_Health_01", amount);
	return;
	elseif name == "buddlerhose" then
	DropItem(playerid, "ITAR_VLK_ARMOR_L", amount);
	return;
	elseif name == "schuerferklamotten" then
	DropItem(playerid, "ITAR_SFB_ARMOR_L", amount);
	return;
	elseif name == "gbauernkleidung" then
	DropItem(playerid, "ITAR_BAU_M", amount);
	return;
	elseif name == "bauernkleidung" then
	DropItem(playerid, "ITAR_BAU_L", amount);
	return;
	
end
	
	
	
	--[[GiveItem(playerid,"ItFo_Apple",100);
		GiveItem(playerid,"ItFo_Beer",100);
		GiveItem(playerid,"ItFo_Fish",100);
		GiveItem(playerid,"ItFo_FishSoup",100);
		GiveItem(playerid,"ItFo_Gebratenerschinken",100);
		GiveItem(playerid,"ItFo_Cheese",100);
		GiveItem(playerid,"ItFo_Water",100);
		GiveItem(playerid,"ItFo_Kochwurst",100);
		GiveItem(playerid,"ItFo_Milk",100);
		GiveItem(playerid,"ItFoMuttonRaw",100);
		GiveItem(playerid,"ItFo_Bread",100);
		GiveItem(playerid,"ItFo_Wine",100);]]--
	

		--SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, string.format("Animation: %s", ani));
  
	elseif cmd == "/dice" then
		local result, numOfSides, numOfThrows = sscanf(params, "dd");
    
		self:dice(playerid, numOfSides, numOfThrows);
	elseif cmd == "/anim" then
		local anim = string.upper(tostring(params));
		PlayAnimation(playerid, anim);
	elseif cmd == "/walk" then
		SetPlayerWalk(playerid, params);   --TODO walk animations
	elseif cmd == "/config" then
		local keybinder = Keybinder.new(playerid);

		keybinder:start();
		
		playersInKeybinder[playerid] = keybinder;
	else
		return 0;
	end
	
	return 1;
end



function erzHacken(playerid)
--if erzhackcond == true then
--SendPlayerMessage(playerid, 255, 0, 0, "Du hackst schon.");
--return;
--else

if Erzhack[playerid].erzcount > 100 then
SendPlayerMessage(playerid, 255, 0, 0, "Du hast heute genug Erz verdient.");
--erzhackrespawn(playerid);
return;
else

if GetPlayerAnimationID(playerid) ~= 248 then
Erzhack[playerid].erzcond = true;
SendPlayerMessage(playerid, 0, 0, 255, "Du hoerst auf zu hacken!");
return;
end

if GetPlayerAnimationID(playerid) == 248 then
GiveItem(playerid,"ItMi_Nugget",1);
Erzhack[playerid].erzcount = Erzhack[playerid].erzcount + 1;
SendPlayerMessage(playerid, 0, 0, 255, "Du erhaelst 1 Erz!");
SetTimerEx("erzHacken",20000,0,playerid);
else
end
end
end
--end



--[[function erzhackrespawn(playerid)
if erzrespawn == true and firsterzcast == true then
erzcount = 0;
erzrespawn = false;
elseif 
SetTimer("erzhackrespawn",21600000, 0);
erzrespawn = true;
end
end]]--

--[[function kochen(playerid)


if GetPlayerAnimationID(playerid) == 123 then
if HasItem(playerid, "ItPl_Temp_Herb",3) == "ItPl_Temp_Herb" then
GetPlayerItem(playerid, slot)
end
GiveItem(playerid,"ItMi_Nugget",1);
SendPlayerMessage(playerid, 0, 0, 255, "Du erhaelst 1 Erz!");
SetTimerEx("erzHacken",20000,0,playerid);
else

end
end]]--

--[[function smoke(playerid)
local joints = 0;
if joints ~= 10 then
PlayAnimation(playerid, "T_JOINT_RANDOM_1");
joints = joints + 1;
SetTimerEx("smoke",2000,0,playerid);
end
end]]--

function retAni(playerid)
local ani = GetPlayerAnimationID(playerid);
		SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, string.format("Animation: %s", ani));
end

function Administration:dice(playerid, numOfSides, numOfThrows)
  if not numOfSides then numOfSides = 6; end;
  if not numOfThrows then numOfThrows = 1; end;
    
  
  if numOfSides < 2 then
    SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Der Wuerfel muss mindestens zwei Seiten haben. (/dice <Anzahl Seiten> <Anzahl Wuerfe>)");
    
    return;
    
  elseif numOfSides > 20 then
    SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Der Wuerfel darf maximal 20 Seiten haben. (/dice <Anzahl Seiten> <Anzahl Wuerfe>)");
    
    return;
  end
  
  if numOfThrows < 1 then
    SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du musst mindestens ein Mal wuerfeln. (/dice <Anzahl Seiten> <Anzahl Wuerfe>)");
    
    return;
    
  elseif numOfThrows > 5 then
    SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du darfst maximal fuenf Mal wuerfeln. (/dice <Anzahl Seiten> <Anzahl Wuerfe>)");
    
    return;
  end
  
  local seed = os.time();
  local result = "";
  
  for i = 1, numOfThrows do
    Permafrost.Util.quickRandom(seed);
    
    result = string.format("%s %d", result, math.random(1, numOfSides));
    
    seed = seed + 9873;
  end
  
  instance_ChatSystem:dice(playerid, result);
end







---Kicks all players and prepares shutdown
function shutdownServer()
	--let all connected players exit the game
	for i = 0, GetMaxPlayers() - 1 do
		if IsPlayerConnected(i) == 1 then
			ExitGame(i);
			LogString("Shutdown", string.format("Kicked player with id %d.", i));
		end
	end
	
	--destroy all vobs
	--Vob.DestroyAllVobs();
	
	--update player inventories to db
	--updateInventoriesInDB();
  
  --update plants to db
 -- updatePlantsInDB();
	
	--free unused RAM
	--collectGarbage();
	
	--disable connection requests
	serverIsShuttingDown = true;
	
	LogString("Shutdown", "--KICKED ALL PLAYERS--");
end


---Kicks a player from the server
function Administration:kickPlayer(playerid, id, reason)
	if not (id and reason) then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du musst eine Spieler-ID und Begruendung angeben. (/kick <ID> <Begruendung>)");
		return;
	end
	
	local playerName1 = GetPlayerName(playerid);
	local playerName2 = GetPlayerName(id);
	
	if id < GetMaxPlayers() and IsPlayerConnected(id) == 1 then
		local playerName = GetPlayerName(id);
			
		SendPlayerMessage(id, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du wirst vom Server gekickt. Begruendung: %s", reason));
			
		Kick(id);
			
		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s (%d) vom Server gekickt.", playerName, id));
		
		LogString("Kick", string.format("%s kicked %s from the server. Reason: %s", playerName1, playerName2, reason));
    
	else
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Bitte gib eine gueltige Spieler-ID ein.");
	end
end

---Bans a player from the 
function Administration:banPlayer(playerid, id, reason)
	if not (id and reason) then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du musst eine Spieler-ID und Begruendung angeben. (/ban <ID> <Begruendung>)");
		return;
	end
	
	local playerName1 = GetPlayerName(playerid);
	local playerName2 = GetPlayerName(id);
	
	if id < GetMaxPlayers() and IsPlayerConnected(id) == 1 then
		local playerName = GetPlayerName(id);
			
		SendPlayerMessage(id, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du wirst vom Server gebannt. Begruendung: %s", reason));
    
		Ban(id);
			
		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s (%d) vom Server gebannt.", playerName, id));
		
		LogString("Ban", string.format("%s banned %s from the server. Reason: %s", playerName1, playerName2, reason));
    
	else
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Bitte gib eine gueltige Spieler-ID ein.");
	end
end

---Warns a player (e. g. for trolling etc.)
function Administration:warnPlayer(playerid, id, reason)
	if not (id and reason) then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du musst eine Spieler-ID und Begruendung angeben. (/warn <ID> <Begruendung>)");
		return;
	end
	
	local playerName1 = GetPlayerName(playerid);
	local playerName2 = GetPlayerName(id);
	
	if id < GetMaxPlayers() and IsPlayerConnected(id) == 1 then
		local playerName = GetPlayerName(id);
    
    local character = getCharacterById(id);
    character:addWarning();
			
		SendPlayerMessage(id, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast eine Verwarnung erhalten. Du hast jetzt %d Verwarnungen. Begruendung: %s", character.warnings, reason));
			
		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s (%d) eine Verwarnung ausgesprochen. Der Spieler hat jetzt %d Verwarnungen.", playerName, id, character.warnings));
		
		LogString("Warning", string.format("%s warned %s. Reason: %s", playerName1, playerName2, reason));
    
	else
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Bitte gib eine gueltige Spieler-ID ein.");
	end
end

---Returns the current administrator password
function Administration:getAdminPassword()
	local currentPassword = nil;
	
	local file = io.open("adminpassword.txt");
	
	if not file then
		--return default admin password
		return GENERAL_DEFAULT_ADMIN_PASSWORD;
	end
	
	for line in io.lines("adminpassword.txt") do
		currentPassword = line;
		break;
	end
	
	return currentPassword;
end

---Changes the administrator password
function Administration:changeAdminPassword(playerid, password)
	local file = io.open("adminpassword.txt", "w");
	
	file:write(password);
	file:flush();
	file:close();
	
	LogString("Administration", string.format("Changed admin password to: %s", password));
	
	SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Du hast das Administratorpassword erfolgreich geaendert.");
end


--instantiate administration
if not instance_Administration then
	instance_Administration = Administration.new();
end