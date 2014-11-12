require("filterscripts.classes.Inventory.InventoryItem");
require("filterscripts.classes.Inventory.InventoryEntry");

print("Loading Inventory.lua");

Inventory = {};
Inventory.__index = Inventory;

PlayerInventories = {};

---Creates an instance of Inventory
---ATTENTION: Must not be called before there Account was logged in and the right username was set
function Inventory.new(playerid, accountID)
	local newInventory = {};
	
	setmetatable(newInventory, Inventory);
	
	newInventory.playerid = tonumber(playerid);
	newInventory.accountID = tonumber(accountID);
  
  PlayerInventories[newInventory.playerid] = newInventory;
  
  newInventory.initialized = false;
	newInventory.items = {};
	
	--initialize
	newInventory:restorePlayerInventory();
	
	return newInventory;
end

---Adds an item to the players inventory instance and DB
function Inventory:addItem(item_instance, count, noSync)
	local instance_upper = item_instance:upper();
	
	if self.items[instance_upper] then
		self.items[instance_upper]:addAmount(count);
	else
		self.items[instance_upper] = InventoryEntry.new(instance_upper, count);
	end
	
	if self.initialized == true then
		--add item to DB
		self:addItemDB(self.items[instance_upper], count);
	end
end

---Removes an item from the players inventory instance and DB
function Inventory:removeItem(item_instance, count, noSync)
	local instance_upper = item_instance:upper();
	
	if self.items[instance_upper] then
		--remove item from DB
		self:removeItemDB(self.items[instance_upper], count);
		--(the entry is removed from DB first, because else it could appear that the referenced instance is nil)
		
		if self.items[instance_upper]:removeAmount(count) == 0 then
			self.items[instance_upper] = nil;
		end
	end
end

---Returns the amount a player has of a certain item_instance
function Inventory:getItemCount(item_instance)
	local instance_upper = item_instance:upper();
	
	if self.items[instance_upper] then
		return self.items[instance_upper].amount;
	end
	
	return 0;
end

---Sets the amount of an inventory entry
function Inventory:setItemCount(item_instance, amount)
  local instance_upper = item_instance:upper();
	
	if self.items[instance_upper] then
    self.items[instance_upper]:setAmount(amount);
  end
end


--Restores the items on the Player Client
function Inventory:restorePlayerInventory()
	ClearInventory(self.playerid);
	
	for k,v in pairs(self:getCharacterInventory()) do
		if v[2] > 0 then --give only items with amount > 0
			GiveItem(self.playerid, instance_InventorySystem.inventoryMan:getItemByID(v[1]).instanceName, v[2]);
		end
	end
	
	self.initialized = true;
end

---Returns all items a player owns of a certain category
function Inventory:getItemsByCategoryID(categoryID)
	local result = {};
	
	for k, v in pairs(self.items) do
		if v.inventoryItem.categoryID == categoryID and v.amount > 0 then
			table.insert(result, v.inventoryItem);
		end
	end
	
	--sort the results
	 function comp(v1, v2)
        if v1.caption < v2.caption then
            return true;
        end
    end
    
    table.sort(result, comp);
	
	return result;
end



----------------------------------------------------------------------
--CALLBACKS
----------------------------------------------------------------------

---Callback: OnPlayerHasItem
function Inventory:OnPlayerHasItem(playerid, item_instance, amount, equipped, checkid)
  local character = getCharacterById(playerid);
  
  if not character then return; end;  
  
  if checkid == tonumber(character.accountID) then
    local inventory = getPlayerInventory(playerid);
    
    if not inventory then return; end;
    
    inventory:setItemCount(item_instance, amount);
  end
end	

---Callback: OnPlayerWeaponMode
function Inventory:OnPlayerWeaponMode(playerid, weaponmode)
	local character = getCharacterById(playerid);
  
  if not character then return; end;

  if weaponmode ~= character.playerLastWeaponMode then
    if character.playerLastWeaponMode == WEAPON_BOW then
      GiveItem(playerid, "ITRW_ARROW", 1);
      
      HasItem(playerid, "ITRW_ARROW", tonumber(character.accountID));
      
    elseif character.playerLastWeaponMode == WEAPON_CBOW then
      GiveItem(playerid, "ITRW_BOLT", 1);
      
      HasItem(playerid, "ITRW_BOLT", tonumber(character.accountID));
    end
  end
  
  character.playerLastWeaponMode = weaponmode;
end

---Callback: OnPlayerTakeItem
function Inventory:OnPlayerTakeItem(playerid, itemid, item_instance, amount, x, y, z, worldName)
	if itemID ~= ITEM_UNSYNCHRONIZED and item_instance ~= "NULL" then
		self:addItem(item_instance:upper(), amount, true);
	end
end

---Callback: OnPlayerDropItem
function Inventory:OnPlayerDropItem(playerid, itemid, item_instance, amount, x, y, z, worldName)
	if itemID ~= ITEM_UNSYNCHRONIZED and item_instance ~= "NULL" then
		self:removeItem(item_instance:upper(), amount, true);
	end
end

---Callback: OnPlayerUseItem
function Inventory:OnPlayerUseItem(playerid, item_instance, amount, hand)
	if item_instance ~= "NULL" then
		self:removeItem(item_instance:upper(), amount, true);
	end
end

---Callback: OnPlayerSpellCast
function Inventory:OnPlayerSpellCast(playerid, spellInstance) 
	if spellInstance ~= "NULL" then
		if string.match(spellInstance, "ITSC_") then
			self:removeItem(spellInstance:upper(), 1, true);
		end
	end
end

--Delete the Instance of Player Inventory on Player Disconnect
function Inventory:OnPlayerDisconnect(playerid)
	if PlayerInventories[playerid] then
		PlayerInventories[playerid] = nil;
	end
end




----------------------------------------------------------------------
--DATABASE INTERACTION
----------------------------------------------------------------------

---Loads the Player Inventory from DB
function Inventory:getCharacterInventory()
	local items = {};
	
	local result = runSQL(string.format("SELECT inventory_item_id, amount FROM inv_entries WHERE account_id = %s;", self.accountID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			table.insert(items, { tonumber(row[1]), tonumber(row[2]) });
				
			row = mysql_fetch_row(result);
		end
	end
	
	return items;
end
 
---Adds Item to DataBase PlayerInventory
function Inventory:addItemDB(entry, amount)
	if not entry then return; end;
	if not entry.inventoryItem then return; end;
	
	instance_InventorySystem:addToDBAddQueue(string.format("(%d, %d, %d)", self.accountID, entry.inventoryItem.itemID, amount));
	
	---
	--update to DB is in InventorySystem.lua
	---
	
	LogString("Database", string.format("Added %ix %s to the inventory of account: %s", amount, entry.inventoryItem.instanceName, self.accountID));
end

---Removes Item from DataBase PlayerInventory
function Inventory:removeItemDB(entry, amount)
	if not entry then return; end;
	if not entry.inventoryItem then return; end;
	
	instance_InventorySystem:addToDBRemoveQueue(string.format("(%d, %d, %d)", self.accountID, entry.inventoryItem.itemID, amount));
	
	---
	--update to DB is in InventorySystem.lua
	---
	
	LogString("Database", string.format("Removed %ix %s from the inventory of account: %s", amount, entry.inventoryItem.instanceName, self.accountID));
end

----------------------------------------------------------------------
--GLOBAL FUNCTIONS
----------------------------------------------------------------------

--Returns the Inventory of the given player. returns nil if there isn't a instance of Inventory for that Player yet
function getPlayerInventory(playerid)
	return PlayerInventories[playerid];
end

---Calls RemoveItem but additionally calls the inventory removeItem
function RemoveItem_(playerid, instance_item, amount)
	RemoveItem__(playerid, instance_item, amount);
	
	getPlayerInventory(playerid):removeItem(instance_item, amount);
end


---OVERWRITE THE ORIGINAL RemoveItem-function
RemoveItem__ = RemoveItem;
RemoveItem = RemoveItem_




---Returns the string representation of this instance
function Inventory.toString(self)
	local result = string.format("Inventory with id: %d\n", self.playerid);

	for k, v in pairs(self.items) do
		result = string.format("%s   %s (%s times).\n", result, k, v.amount);
	end

	return result;
end

--add toString-function to metatable
Inventory.__tostring = Inventory.toString;