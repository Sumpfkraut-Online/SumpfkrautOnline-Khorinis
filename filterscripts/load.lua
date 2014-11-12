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
require("filterscripts.systems.ResurrectionSystem");
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
	OpenLocks(1);
	-- EnableExitGame(0);
	EnableDropAfterDeath(0);
	EnableNicknameID(0);
	--EnableNickname(0);
	SetRespawnTime(172800000);

	--load static classes
	loadTimerecorder(); --[[in Timerecorder.lua]]
	loadGarbageCollector(); --[[in GarbageCollector.lua]]
	loadHungerSystem(); --[[in HungerSystem.lua]]
	
	local tmp_id = CreateNPC("Anti-Cheat");
	createAccount("Anti-Cheat", "Anti-Cheat");
	
	Character:new(tmp_id,getAccountIdByName("Anti-Cheat"));
	SpawnPlayer(tmp_id);
	SetPlayerPos(tmp_id, 0, 0, 0);
	SetPlayerHealth(tmp_id, 80);
	PlayAnimation(tmp_id,"S_RUN");
		
	--enable teach and crafting menu check
	SetTimer("checkPlayerAnim", ANIM_CHECK_INTERVAL, 1);
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
	instance_ResurrectionSystem:OnPlayerDeath(playerid, p_classid, killerid, k_classid);
	
	SendMessageToAll(39,199,130, GetPlayerName(playerid) .. " unterlag im Kampf gegen " .. GetPlayerName(killerid) .. ".");
end

---Callback for OnPlayerKey
function OnPlayerKey(playerid, keyDown, keyUp)
	if not playerUpdateTime[playerid] then
		playerUpdateTime[playerid] = os.clock();
		return;
	end;
	

	if keyDown == KEY_F7 then
		local x,y,z = GetPlayerPos(playerid);
		local angle = tonumber(GetPlayerAngle(playerid));
		local x_dir = math.sin((angle * 3.141592653589793) / 180);
		local z_dir = math.cos((angle * 3.141592653589793) / 180);
		x			= x + (x_dir * 300);
		z			= z + (z_dir * 300);
		SetPlayerPos(playerid, x, y, z);
		
	elseif keyDown == KEY_F8 then
		local x,y,z = GetPlayerPos(playerid);
		y = y + 300;
		SetPlayerPos(playerid, x, y, z);
		
	elseif keyDown == KEY_F9 then
		if (GetPlayerHealth(playerid) <= 0) then
			SpawnPlayer(playerid);
		end
		local x,y,z = GetPlayerPos(playerid);
		local angle = tonumber(GetPlayerAngle(playerid));
		local x_dir = math.sin((angle * 3.141592653589793) / 180);
		local z_dir = math.cos((angle * 3.141592653589793) / 180);
		x			= x + (x_dir * 300);
		y			= y + 300;
		z			= z + (z_dir * 300);
		SetPlayerPos(playerid, x, y, z);
	elseif keyDown == KEY_F10 then
		PlayAnimation(playerid, "S_SNEAKL");
	elseif keyUp == KEY_F11 then
		PlayAnimation(playerid, "S_RUN");
	end
	
	
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
			instance_ResurrectionSystem:OnPlayerKey(playerid, keyDown);
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
	if item_instance == "NULL" then return; end;
    
  local item = instance_InventorySystem.inventoryMan:getItemForInstanceName(item_instance);
  
  if not item then
	print(string.format("ITEM_INSTANCE: %s is UNKNOWN", item_instance));
  end
  
  --ignore all item uses that are not from one of the following categories:
  ---Lebensmittel
  ---Pflanzen
  ---Traenke und Heilung
  if item.categoryID ~= 2 and item.categoryID ~= 3 and item.categoryID ~= 11 then
      return;
  end
    
	
	local inv = getPlayerInventory(playerid);
	if not inv then return; end;
    
	inv:OnPlayerUseItem(playerid, item_instance, amount, hand);
	
	instance_HungerSystem:OnPlayerUseItem(playerid, item_instance, amount, hand);
  --instance_FarmingSystem:OnPlayerUseItem(playerid, item_instance, amount, hand);
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