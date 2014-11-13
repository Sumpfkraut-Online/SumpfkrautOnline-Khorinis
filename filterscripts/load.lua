print("-----------------------------------------------------------");
print("-----------------------CONSTANTS---------------------------");
require("filterscripts.constants.Global");
print("-----------------------------------------------------------");
print("-------------------------CLASSES---------------------------");
require("filterscripts.classes.DBConnection");
require("filterscripts.classes.CharEditor");
require("filterscripts.classes.Menu.MenuFunctions");
require("filterscripts.systems.InventorySystem");
require("filterscripts.classes.Inventory.Inventory");
require("filterscripts.classes.Character");
require("filterscripts.classes.Animation");
require("filterscripts.classes.LoginSystem");
require("filterscripts.classes.Keybinder");
require("filterscripts.systems.Craftingsystem");
require("filterscripts.systems.Teachsystem");
require("filterscripts.systems.HuntSystem");
require("filterscripts.systems.FarmingSystem");
require("filterscripts.systems.ChatSystem");
require("filterscripts.systems.NameSystem");
require("filterscripts.systems.HungerSystem");
require("filterscripts.systems.InteractionSystem");
--require("filterscripts.systems.ResurrectionSystem");
require("filterscripts.classes.Timerecorder");
require("filterscripts.classes.GarbageCollector");
require("filterscripts.classes.Waypoints.WaypointSystem");
require("filterscripts.classes.Administration");
print("-----------------------------------------------------------");

-------------
---VARIABLES
-------------

--contains the timestamp the player hit a key
local playerUpdateTime = {};

--determines whether the server is running in debug mode
debugMode = GENERAL_DEBUG_MODE;



-------------
---CALLBACKS
-------------

--Initialisation of the filterscript
function OnFilterscriptInit()
	--set general server information
	SetServerHostname(GENERAL_HOSTNAME);
	SetGamemodeName(GENERAL_GAMEMODE_NAME);
	SetServerDescription(GENERAL_DESCRIPTION);
	OpenLocks(0);
	EnableExitGame(0);
	EnableDropAfterDeath(0);
	EnableNicknameID(0);
	--EnableNickname(0);
	SetRespawnTime(172800000);

	--load static classes
	loadTimerecorder(); --[[in Timerecorder.lua]]
	loadGarbageCollector(); --[[in GarbageCollector.lua]]
	loadHungerSystem(); --[[in HungerSystem.lua]]
	
	local tmp_id = CreateNPC("Sklave");
	createAccount("Sklave", "Sklave");
	
	Character:new(tmp_id,getAccountIdByName("Sklave"));
	SpawnPlayer(tmp_id);
	SetPlayerPos(tmp_id, 321312412, 4124123123, 3213123213);
	SetPlayerHealth(tmp_id, 80);
	PlayAnimation(tmp_id,"S_RUN");
		
	--enable teach and crafting menu check
	SetTimer("checkPlayerAnim", ANIM_CHECK_INTERVAL, 1);
	SetTimer("EXPBONUS", 600000, 1);
	
end
 
--Unloading of this script free resources here
function OnFilterScriptExit()
end

---Callback for a connecting player
function OnPlayerConnect(playerid)
	--enable nickname only for NPCs
	EnablePlayerNickname(playerid, IsNPC(playerid))

	--do only accept player connections if the server is not shutting down
	if serverIsShuttingDown == false then
		instance_LoginSystem:OnPlayerConnect(playerid);
		--SetPlayerEnable_OnPlayerKey(playerid, 1);
	else
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Der Server wird momentan heruntergefahren. Bitte versuche es spaeter erneut.");
		Kick(playerid);
	end
end

---Callback for player disconnect
function OnPlayerDisconnect(playerid, reason)
	CharacterOnPlayerDisconnect(playerid)
	instance_LoginSystem:logoutUser(playerid)
	CharEditor:OnPlayerDisconnect(playerid)
end
---Callback for player spawn
function OnPlayerSpawn(playerid, classid)
	instance_LoginSystem:OnPlayerSpawn(playerid)
	--Maybe put this after the LoginSystem login
end

---Callback for player commandText written in chat
function OnPlayerCommandText(playerid, cmdtext)
	--determines whether the command was recognized or not
	local recognized = 0;
	
	recognized = instance_LoginSystem:OnPlayerCommandText(playerid, cmdtext);
	
	if recognized == 0 then
		recognized = instance_ChatSystem:OnPlayerCommandText(playerid, cmdtext);
	end
	
	if recognized == 0 then
		recognized = instance_Administration:OnPlayerCommandText(playerid, cmdtext);
	end
end

---Callback for player text written in chat
function OnPlayerText(playerid, text)
	instance_ChatSystem:OnPlayerText(playerid, text);
end


---Callback for player change focus
function OnPlayerFocus(playerid, focusid)
	instance_NameSystem:OnPlayerFocus(playerid, focusid);
end

---Callback for player death
function OnPlayerDeath(playerid, p_classid, killerid, k_classid)
--	instance_ResurrectionSystem:OnPlayerDeath(playerid, p_classid, killerid, k_classid);
	
--	SendMessageToAll(39,199,130, GetPlayerName(playerid) .. " unterlag im Kampf gegen " .. GetPlayerName(killerid) .. ".");
end

---Callback for OnPlayerKey
function OnPlayerKey(playerid, keyDown, keyUp)
	if not playerUpdateTime[playerid] then
		playerUpdateTime[playerid] = os.clock();
		return;
	end;
	
	
	--delay: 0.2 seconds
	-- -> max 5 keyboard hits per second
	if playerUpdateTime[playerid] then
		if os.clock() - playerUpdateTime[playerid] >= 0.2 then
			CharEditor:OnPlayerKey(playerid, keyDown, keyUp);
			instance_LoginSystem:OnPlayerKey(playerid);
			--ATTENTION: interaction sys has to be called after teach and inventory!
			instance_CraftingSystem:OnPlayerKey(playerid, keyDown);
			instance_TeachSystem:OnPlayerKey(playerid, keyDown);
			instance_HuntSystem:OnPlayerKey(playerid, keyDown);
			instance_ChatSystem:OnPlayerKey(playerid, keyDown);
			instance_InventorySystem:OnPlayerKey(playerid, keyDown);
			instance_InteractionSystem:OnPlayerKey(playerid, keyDown);
--			instance_ResurrectionSystem:OnPlayerKey(playerid, keyDown);
			instance_FarmingSystem:OnPlayerKey(playerid, keyDown);
			
			if playersInKeybinder[playerid] then
				playersInKeybinder[playerid]:OnPlayerKey(playerid, keyDown, keyUp);
			end
		
			playerUpdateTime[playerid] = os.clock();
		end
	end
end



-------------
--CALLBACKS:
--INVENTORY
-------------

---Callback for OnPlayerHasItem
function OnPlayerHasItem(playerid, item_instance, amount, equipped, checkid)
  Inventory:OnPlayerHasItem(playerid, item_instance, amount, equipped, checkid);
end	

---Callback for player weaponmode change
function OnPlayerWeaponMode(playerid, weaponmode)	
	Inventory:OnPlayerWeaponMode(playerid, weaponmode)
end

---Callback for player using an item
function OnPlayerUseItem(playerid, item_instance, amount, hand)
	local inv = getPlayerInventory(playerid);
	if not inv then return; end;
    
	inv:OnPlayerUseItem(playerid, item_instance, amount, hand);
	
	instance_HungerSystem:OnPlayerUseItem(playerid, item_instance, amount, hand);
	instance_FarmingSystem:OnPlayerUseItem(playerid, item_instance, amount, hand);
end

---Callback for player take item
function OnPlayerTakeItem(playerid, itemid, item_instance, amount, x, y, z, worldName)
	local inv = getPlayerInventory(playerid);
	if not inv then return; end;
	
	inv:OnPlayerTakeItem(playerid, itemid, item_instance, amount, x, y, z, worldName);
end

---Callback for player item drop
function OnPlayerDropItem(playerid, itemid, item_instance, amount, x, y, z, worldName)
	local inv = getPlayerInventory(playerid)
	if not inv then return; end;
	
	inv:OnPlayerDropItem(playerid, itemid, item_instance, amount, x, y, z, worldName);
end

---Callback for OnPlayerSpellCast
function OnPlayerSpellCast(playerid, spellInstance) 
	local inv = getPlayerInventory(playerid);
	if not inv then return; end;
	
	inv:OnPlayerSpellCast(playerid, spellInstance);
end

---Callback for player closing inventory
function OnPlayerCloseInventory(playerid)
	-- Enable the Key Tracking after the player Closed the inventory 
	--Tracking was disabled to prevent inventory from not being used
	SetPlayerEnable_OnPlayerKey(playerid, 1);
end

---Callback for player opening inventory
function OnPlayerOpenInventory(playerid)
	-- Disable the Key Tracking when the player opens the inventory 
	--Tracking is disabled to prevent inventory from not being used
	SetPlayerEnable_OnPlayerKey(playerid, 0);
end




-------------
---OTHER FUNCTIONS
-------------

---Checks for all connected players whether a player is in an animation for a crafting or teach menu
function checkPlayerAnim()
	for playerid = 0, GetMaxPlayers() - 1 do
		if IsSpawned(playerid) == 1 then
			instance_CraftingSystem:check(playerid);
			instance_TeachSystem:check(playerid);
		end
	end
end

    function EXPBONUS()
            local i = 0;
            for i = 0, GetMaxPlayers() -1 do
                    if IsPlayerConnected(i) == 1 then
                            local bonuswert = GetPlayerMaxHealth(i) -10;
                            if GetPlayerHealth(i) >= bonuswert then
                                    SetPlayerExperience(i, GetPlayerExperience(i) +10);
                                    SendPlayerMessage(i, 0, 255, 0, "Ich bin gesund! (+10EXP)");
                            else
                                    SendPlayerMessage(i, 255, 0, 0, "Ich sollte etwas essen.");
                            end
                    end
            end
    end
	