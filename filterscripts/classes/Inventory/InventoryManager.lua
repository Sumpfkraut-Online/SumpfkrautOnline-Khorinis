require "filterscripts/classes/Menu/Menu"
require "filterscripts/classes/Menu/MenuItem"
require "filterscripts/classes/Menu/StaticMenuItem"
require "filterscripts/classes/Inventory/InventoryCategory"

print("Loading InventoryManager.lua");

InventoryManager = {};
InventoryManager.__index = InventoryManager;

---Creates an instance of InventoryManager
function InventoryManager.new()
	local newInventoryManager = {};
	
	setmetatable(newInventoryManager, InventoryManager);
	
	newInventoryManager.inventoryCategories = {};
	
	newInventoryManager:loadCategoriesFromDB();
		
	return newInventoryManager;
end

---Adds a teach-category to this teach manager instance
function InventoryManager:addCategory(category)
	if self.inventoryCategories[category.categoryID] then return false; end;
	
	self.inventoryCategories[category.categoryID] = category;
end

---Returns the item instance for a given instance-name
function InventoryManager:getItemForInstanceName(instanceName)
	local instance_upper = instanceName:upper();
	
	for k, v in pairs(self.inventoryCategories) do
		if v.items[instance_upper] then
			return v.items[instance_upper];
		end
	end
	
	return nil;
end

---Returns the item instance for a given item id
function InventoryManager:getItemByID(id)
	local id_num = tonumber(id);
	
	for k, v in pairs(self.inventoryCategories) do
		for k2, v2 in pairs(v.items) do
			if v2.itemID == id_num then
				return v2;
			end
		end
	end
	return nil;
end

---Loads all teach-categories from DB and adds them to this instance
function InventoryManager:loadCategoriesFromDB()
	local result = runSQL("SELECT id, caption FROM inv_categories ORDER BY caption");
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local cat = InventoryCategory.new(tonumber(row[1]), row[2]);
			self:addCategory(cat);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Returns all categories a player can deal with
function InventoryManager:getTradeableCategories(playerid)
	local categoryIDs = {};
	local inventory = getPlayerInventory(playerid);
	
	if not inventory then return nil; end;
	
	for k, v in pairs(inventory.items) do
		if v.amount > 0 and not categoryIDs[v.inventoryItem.categoryID] then
			categoryIDs[v.inventoryItem.categoryID] = 1;
		end
	end
	
	local categories = {};
	
	for k, v in pairs(categoryIDs) do
		table.insert(categories, self.inventoryCategories[k]);
	end
	
	
	--sort the results
	 function comp(v1, v2)
        if v1.caption < v2.caption then
            return true;
        end
    end
    
    table.sort(categories, comp);
		
	return categories;
end

---Creates a trade menu between two players
function InventoryManager:createTradeMenu(playerid1, playerid2)
	instance_InventorySystem.isInTradeMenu[playerid1] = self:createTradeMenuFor(playerid1, playerid2);
	
	if not instance_InventorySystem.isInTradeMenu[playerid1] then return; end;
	
	instance_InventorySystem.isInTradeMenu[playerid2] = self:createTradeMenuFor(playerid2, playerid1);
	
	--set the second players angle -> looks at the first player
	SetPlayerAngle(playerid2, GetAngleToPlayer(playerid2,playerid1));
end

---Creates a trade menu for one (of two) player
function InventoryManager:createTradeMenuFor(playerid, secondPlayerID)
	local items = self:getTradeableCategories(playerid);
	
	if not items then return nil; end;
	--TODO: Check whether inventory is empty
	
	local menu = Menu.new(playerid, secondPlayerID, 0, MENU_X_STANDARD, MENU_Y_STANDARD, instance_InventorySystem.isInTradeMenu, nil, 0);
	
	--add headline
	local headlineItem = StaticMenuItem.new("INVENTAR");
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add content
	for k, v in pairs(items) do
		local menuItem = MenuItem.new(v);
		
		menu:addItem(menuItem);
	end
	
	--add exit
	local exitItem = MenuItem.new(StaticMenuItem.new("Ende"));
	menu:setExit(exitItem);
	
	exitItem.exec = function ()
						menu:exitMenu();
						
						if instance_InventorySystem:isPlayerInValidMenu(secondPlayerID) then
							instance_InventorySystem.isInTradeMenu[secondPlayerID]:exitMenuAndSubMenus();
							SendPlayerMessage(secondPlayerID, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, instance_NameSystem:getPlayerNameFromViewOf(secondPlayerID, playerid) .. " hat den Handel beendet.");
						end
					end
		
	menu:createDraws();
	menu:updateTop();
	
	return menu;
end