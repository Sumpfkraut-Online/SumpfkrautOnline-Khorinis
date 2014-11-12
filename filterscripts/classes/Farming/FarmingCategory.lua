require "filterscripts/classes/Farming/FarmingAction"

print("Loading FarmingCategory.lua");

FarmingCategory = {};
FarmingCategory.__index = FarmingCategory;

---Creates an instance of FarmingCategory
function FarmingCategory.new(categoryID, caption)
	local newFarmingCategory = {};
	
	setmetatable(newFarmingCategory, FarmingCategory);
	
	newFarmingCategory.categoryID = categoryID;
	newFarmingCategory.caption = caption;
	newFarmingCategory.actions = {};
	
	newFarmingCategory:loadActionsFromDB();
		
	return newFarmingCategory;
end

---Adds a teach to this category instance
function FarmingCategory:addAction(action)
	if self.actions[action.actionID] then return false; end;
	
	self.actions[action.actionID] = action;
end



---Checks whether this instance (as a MenuItem) can be executed
function FarmingCategory:checkExecConditions(playerid)
	return true;
end

---Creates a sub menu with the teaches of this category as items
function FarmingCategory:exec(playerid)
	local menu = Menu.new(playerid, nil, nil, MENU_X_STANDARD, MENU_Y_STANDARD, nil, nil); --the parent menu is set in Menu:accept()
	
	--add headline
	local headlineItem = StaticMenuItem.new(self.caption:upper());
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add content
	for k, v in pairs(self.actions) do
		
		v.parentMenu = menu;
		
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

---Loads this category's actions from DB
function FarmingCategory:loadActionsFromDB()
	local result = runSQL(string.format("SELECT id, caption, duration, radius, specialAction FROM fg_actions WHERE cat_id = %s;", self.categoryID));

	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			
			local act = FarmingAction.new(tonumber(row[1]), row[2], tonumber(row[3]), tonumber(row[4]), tonumber(row[5]));
			
			self:addAction(act);
				
			row = mysql_fetch_row(result);
		end
	end
end