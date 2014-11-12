require "filterscripts/classes/Inventory/InventoryItem"

print("Loading InventoryCategory.lua");

InventoryCategory = {};
InventoryCategory.__index = InventoryCategory;

---Creates an instance of InventoryCategory
function InventoryCategory.new(categoryID, caption)
	local newInventoryCategory = {};
	
	setmetatable(newInventoryCategory, InventoryCategory);
	
	newInventoryCategory.categoryID = categoryID;
	newInventoryCategory.caption = caption;
	newInventoryCategory.items = {};
	
	newInventoryCategory:loadItemsFromDB();
		
	return newInventoryCategory;
end

---Adds an item to this category instance
function InventoryCategory:addItem(item)
	if self.items[item.instanceName] then return false; end;
	
	self.items[item.instanceName] = item;
end

---Checks whether this instance (as a MenuItem) can be executed
function InventoryCategory:checkExecConditions(playerid)
	return true;
end

---Creates a sub menu with the inventory of this category as items
function InventoryCategory:exec(playerid, secondPlayerID)
	local inventory = getPlayerInventory(playerid);
	local items = inventory:getItemsByCategoryID(self.categoryID);
	
	
	local menu = Menu.new(playerid, secondPlayerID, 0, MENU_X_STANDARD, MENU_Y_STANDARD, nil, nil, 0); --the parent menu is set later
	
	--add headline
	local headlineItem = StaticMenuItem.new(self.caption:upper());
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add content
	for k, v in pairs(items) do
		local menuItem = MenuItem.new(v);
		
		menu:addItem(menuItem);
	end
	
	--add exit
	local exitItem = StaticMenuItem.new("Zurueck");
	menu:setExit(MenuItem.new(exitItem));
		
	menu:createDraws();
	menu:updateTop();
	
	return menu;
end

---Loads this category's items from DB
function InventoryCategory:loadItemsFromDB()
	local result = runSQL(string.format("SELECT id, instance_name, caption, saturation FROM inv_items WHERE cat_id = %s ORDER BY caption;", self.categoryID));

	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local it = InventoryItem.new(tonumber(row[1]), row[2], row[3], self.categoryID, tonumber(row[4]));
			
			self:addItem(it);
				
			row = mysql_fetch_row(result);
		end
	end
end