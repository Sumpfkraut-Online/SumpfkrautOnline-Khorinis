require "filterscripts/classes/Inventory/Elements/InventoryAmount"
require "filterscripts/classes/Inventory/Elements/InventorySend"

print("Loading InventoryItem.lua");

InventoryItem = {};
InventoryItem.__index = InventoryItem;

---Creates an instance of InventoryItem
function InventoryItem.new(itemID, instanceName, caption, categoryID, saturation)
	local newInventoryItem = {};
	
	setmetatable(newInventoryItem, InventoryItem);
	
	newInventoryItem.itemID = tonumber(itemID);
	newInventoryItem.caption = caption;
	newInventoryItem.instanceName = instanceName:upper();
	newInventoryItem.categoryID = tonumber(categoryID);
	
	
	--GER: Saettigungswert eines Items
	newInventoryItem.saturation = saturation;
	
	return newInventoryItem;
end


---Checks whether this instance (as a MenuItem) can be executed
function InventoryItem:checkExecConditions(playerid)
	return true;
end

---Creates a sub menu with the inventory of this category as items
function InventoryItem:exec(playerid, secondPlayerID)
	local inventory = getPlayerInventory(playerid);
	local itemCount = inventory:getItemCount(self.instanceName);	
	
	local menu = Menu.new(playerid, secondPlayerID, 0, MENU_X_STANDARD, MENU_Y_STANDARD, nil, nil, 0); --the parent menu is set later
	
	--add headline
	local headlineItem = StaticMenuItem.new(self.caption:upper());
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add OK button
	menu:addItem(MenuItem.new(InventorySend.new(menu)));
	
	local availableAmounts = {5, 1, -1, -5};
	--add content
	for k, v in pairs(availableAmounts) do
		local menuItem = MenuItem.new(InventoryAmount.new(v, menu));
		
		menu:addItem(menuItem);
	end
	
	--add exit
	local exitItem = StaticMenuItem.new("Zurueck");
	menu:setExit(MenuItem.new(exitItem));
	
	--set the start amount and instance name
	menu.amount = 0;
	menu.maxAmount = itemCount;
	menu.instanceName = self.instanceName;
	menu.instanceCaption = self.caption;
	
	menu:createDraws();
	menu:updateTop();
	
	--update the menu headline. ATTIONTION: First draw, then update.
	menu:updateHeadline(playerid, string.format("%s (0 / %d)", menu.headlineItem.caption, itemCount));
	
	return menu;
end