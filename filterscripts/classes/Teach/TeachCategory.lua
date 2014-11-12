require "filterscripts/classes/Teach/Teach"

print("Loading TeachCategory.lua");


TeachCategory = {};
TeachCategory.__index = TeachCategory;

---Creates an instance of TeachCategory
function TeachCategory.new(categoryID, caption)
	local newTeachCategory = {};
	
	setmetatable(newTeachCategory, TeachCategory);
	
	newTeachCategory.categoryID = categoryID;
	newTeachCategory.caption = caption:upper();
	newTeachCategory.teaches = {};
	
	newTeachCategory.containsBasicTeaches = nil;
	
	newTeachCategory:loadTeachesFromDB();
		
	return newTeachCategory;
end

---Adds a teach to this category instance
function TeachCategory:addTeach(teach)
	if self.teaches[teach.teachID] then return false; end;
	
	self.teaches[teach.teachID] = teach;
end

---Checks whether this instance (as a MenuItem) can be executed
function TeachCategory:checkExecConditions(playerid)
	return true;
end

---Creates a sub menu for the teaches of this teach-category
function TeachCategory:exec(playerid, studentid)
	if studentid then
		return self:execDual(playerid, studentid);
	else
		return self:execSingle(playerid);
	end
end

---Creates a sub menu with the basic teaches of this category as items (single player teach)
function TeachCategory:execSingle(playerid)
	local menu = Menu.new(playerid, nil, 0, MENU_X_STANDARD, MENU_Y_STANDARD, nil, nil); --the parent menu is set in Menu:accept()
	
	--add headline
	local headlineItem = StaticMenuItem.new(self.caption:upper());
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add content
	for k, v in pairs(self.teaches) do
		if v.isBasic == true then
			local menuItem = MenuItem.new(v);
		
			menu:addItem(menuItem);
		end
	end
	
	--add exit
	local exitItem = StaticMenuItem.new("Zurueck");
	menu:setExit(MenuItem.new(exitItem));
		
	menu:createDraws();
	menu:updateTop();
	
	return menu;
end

---Creates a sub menu with the teaches of this category as items (dual player teach)
function TeachCategory:execDual(playerid, studentid)
	local menu = Menu.new(playerid, studentid, 0, MENU_X_STANDARD, MENU_Y_STANDARD, nil, nil); --the parent menu is set in Menu:accept()
	
	--add headline
	local headlineItem = StaticMenuItem.new(self.caption:upper());
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add content
	for k, v in pairs(self.teaches) do
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

---Loads this category's teaches from DB
function TeachCategory:loadTeachesFromDB()
	local result = runSQL(string.format("SELECT id, cat_id, caption, basic, mastering_tc, duration FROM tc_teach WHERE cat_id = %d;", self.categoryID));

	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local isBasic = false;
			local masteringTeachID = -1;
			
			if tonumber(row[4]) == 1 then isBasic = true; end;
			
			if row[5] ~= "NULL" then
				masteringTeachID = tonumber(row[5]);
			end
			
			local tc = Teach.new(tonumber(row[1]), tonumber(row[2]), row[3], isBasic, masteringTeachID, tonumber(row[6]));
			
			self:addTeach(tc);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Returns whether a player (character) is able to teach all teaches of this category to another character
function TeachCategory:canPlayerTeachCategory(playerid)
	local characterTeacher = getCharacterById(playerid);
	
	if characterTeacher:getTeacherRank(self.categoryID) > 0 then return true; end;
	
	return false;
end

---Returns whether this category contains basic (book) teaches or not
function TeachCategory:getContainsBasicTeaches()
	if not self.containsBasicTeaches then
		for k, v in pairs(self.teaches) do
			if v.isBasic == true then
				self.containsBasicTeaches = true;
				return self.containsBasicTeaches;
			end
		end
		
		self.containsBasicTeaches = false;
	end
	
	return self.containsBasicTeaches;
end