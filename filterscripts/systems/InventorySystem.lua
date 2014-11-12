require "filterscripts/classes/Inventory/InventoryManager"
require("filterscripts.classes.Queue");

print("Loading InventorySystem.lua");

instance_InventorySystem = nil;

InventorySystem = {};
InventorySystem.__index = InventorySystem;

---Creates an instance of InventorySystem
function InventorySystem.new()
	local newInventorySystem = {};
	
	newInventorySystem.inventoryMan = InventoryManager.new();
	newInventorySystem.isInTradeMenu = {};
	newInventorySystem.addQueue = Queue.new();
	newInventorySystem.removeQueue = Queue.new();
	
	setmetatable(newInventorySystem, InventorySystem);
	
	SetTimer("updateInventoriesInDB", INVENTORY_UPDATE_INTERVAL * 1000, 1);
	
	return newInventorySystem;
end

---Reloads inventory entries from DB
function InventorySystem:reload()
	self.inventoryMan = InventoryManager.new();
end

---Checks whether a player is in a trading menu
function InventorySystem:isPlayerInValidMenu(playerid)
	if self.isInTradeMenu[playerid] and type(self.isInTradeMenu[playerid]) ~= "number" then
		return true;
	end
	
	return false;
end

---Callback: OnPlayerKey
function InventorySystem:OnPlayerKey(playerid, keyDown)
	if self:isPlayerInValidMenu(playerid) == true then
		navigateMenu(playerid, keyDown, self.isInTradeMenu);
	end
end

---Adds a string to the add queue
function InventorySystem:addToDBAddQueue(str)
	self.addQueue:offer(str);
end

---Adds a string to the remove queue
function InventorySystem:addToDBRemoveQueue(str)
	self.removeQueue:offer(str);
end

---Updates all changes during the last N seconds to DB
function updateInventoriesInDB()
	local add = instance_InventorySystem.addQueue:poll();
	local rem = instance_InventorySystem.removeQueue:poll();
	
	if #add > 0 then
	
		local strAdd = "";
	
		for i = 1, #add do
			strAdd = string.format("%s%s,", strAdd, add[i]);
		end
	
		strAdd = strAdd:sub(1, strAdd:len() - 1);
	
		runSQL(string.format("INSERT INTO inv_entries (account_id, inventory_item_id, amount) VALUES %s ON DUPLICATE KEY UPDATE amount = amount + VALUES(amount);", strAdd));
		
		add = nil;
	end
	
	
	if #rem > 0 then
	
		local strRem = "";
	
		for i = 1, #rem do
			strRem = string.format("%s%s,", strRem, rem[i]);
		end
		
		strRem = strRem:sub(1, strRem:len() - 1);
		
		runSQL(string.format("INSERT INTO inv_entries (account_id, inventory_item_id, amount) VALUES %s ON DUPLICATE KEY UPDATE amount = amount - VALUES(amount);", strRem));
	
		--remove entries if amount <= 0
		runSQL("DELETE FROM inv_entries WHERE amount < 1;");
		
		rem = nil;
	end
	
	--print("Updated inventories in DB.");
end


--instantiate system
if not instance_InventorySystem then
	instance_InventorySystem = InventorySystem.new();
end