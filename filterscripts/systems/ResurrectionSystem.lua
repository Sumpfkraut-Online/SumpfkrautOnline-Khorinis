print("Loading ResurrectionSystem.lua");

instance_ResurrectionSystem = nil;

ResurrectionSystem = {};
ResurrectionSystem.__index = ResurrectionSystem;

---Creates an instance of ResurrectionSystem
function ResurrectionSystem.new()
	local newResurrectionSystem = {};
	
	newResurrectionSystem.gravestones = {};
	
	setmetatable(newResurrectionSystem, ResurrectionSystem);
	
	return newResurrectionSystem;
end

---Revive a player
--playerid1 = the reviving player
--playerid2 = the player that is revived
function ResurrectionSystem:resurrectPlayer(playerid1, playerid2)

	PlayAnimation(playerid1, "T_PLUNDER")
	
	--SpawnPlayer(playerid2)
	SetPlayerHealth(playerid2, 10);
	SetPlayerWeaponMode(playerid2, WEAPON_NONE);
	FreezePlayer(playerid2, 1);
					
	local msg = CreatePlayerDraw(playerid2, 700, 4400, "Um den Wiederbelebungsbug zu verhindern, wirst du nun ausgeloggt melde dich bitte neu an.", "Font_Default.tga", 255, 255, 255);
	ShowPlayerDraw(playerid2, msg);
	
	self.gravestones[playerid2]:Destroy();
	self.gravestones[playerid2] = nil;
	
	SendPlayerMessage(playerid1, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s erfolgreich wiederbelebt.", GetPlayerName(playerid2)));
	
	SetTimerEx("forcePlayerQuit", 4000, 0, playerid2)
end

---Callback: OnPlayerKey
function ResurrectionSystem:OnPlayerKey(playerid, keyDown)
	--Note: this consumes mana now to prevent spamming and serverlag
	if keyDown == KEY_V then
		local playerMana = GetPlayerMana(playerid);
		local character = getCharacterById(playerid);
		
		if playerMana >= RESURRECTION_MANA_COST or character.rights > 0 then
      if character.rights == 0 then
        local newPlayerMana = playerMana - RESURRECTION_MANA_COST;
        if newPlayerMana < 0 then newPlayerMana = 0; end;
			
        SetPlayerMana(playerid, newPlayerMana);
      end
		
			if GetPlayerHealth(playerid) > 0 then
				for k, v in pairs(self.gravestones) do
					if GetDistancePlayers(playerid, k) <= RESURRECTION_RADIUS then
            if playerid ~= k then
              self:resurrectPlayer(playerid, k);
					
              return;
             end
					end
				end
			end
		else
			--send msg: not enought mana
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du hast nicht ausreichend Mana, um den Spieler wiederzubeleben.");
		end
	end
end

---Callback: OnPlayerDeath
function ResurrectionSystem:OnPlayerDeath(playerid, p_classid, killerid, k_classid)
	if debugMode == 1 then
		SetPlayerHealth(playerid, 10);
		SetPlayerWeaponMode(playerid, WEAPON_NONE);
		FreezePlayer(playerid, 1);
		
		local msg = CreatePlayerDraw(playerid, 700, 4400, "Um den Wiederbelebungsbug zu verhindern, wirst du nun ausgeloggt melde dich bitte neu an.", "Font_Default.tga", 255, 255, 255);
		ShowPlayerDraw(playerid, msg);
		
		SetTimerEx("forcePlayerQuit", 4000, 0, playerid);
	else
		local x, y, z = GetPlayerPos(playerid);
	
		local gVob = Vob.Create(RESURRECTION_VOB, GetPlayerWorld(playerid), x, y, z);
		gVob:SetRotation(0, GetPlayerAngle(playerid) + 180, 0);
	
		self.gravestones[playerid] = gVob;
	end
end

---Forces a player to exit the game immediately
function forcePlayerQuit(playerid)
	ExitGame(playerid);
end


--instantiate system
if not instance_ResurrectionSystem then
	instance_ResurrectionSystem = ResurrectionSystem.new();
end