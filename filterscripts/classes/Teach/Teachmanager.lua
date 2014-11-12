require "filterscripts/classes/Menu/Menu"
require "filterscripts/classes/Menu/MenuItem"
require "filterscripts/classes/Menu/StaticMenuItem"
require "filterscripts/classes/Teach/TeachCategory"

print("Loading Teachmanager.lua");

Teachmanager = {};
Teachmanager.__index = Teachmanager;

---Creates an instance of Teachmanager
function Teachmanager.new()
	local newTeachmanager = {};
	
	setmetatable(newTeachmanager, Teachmanager);
	
	newTeachmanager.teachCategories = {};
	
	newTeachmanager:loadCategoriesFromDB();
		
	return newTeachmanager;
end

---Adds a teach-category to this teach manager instance
function Teachmanager:addCategory(category)
	if self.teachCategories[category.categoryID] then return false; end;
	
	self.teachCategories[category.categoryID] = category;
end

---Loads all teach-categories from DB and adds them to this instance
function Teachmanager:loadCategoriesFromDB()
	local result = runSQL("SELECT id, caption FROM tc_categories");
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local cat = TeachCategory.new(tonumber(row[1]), row[2]);

			self:addCategory(cat);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Returns all teaches (in all categories) that are marked as 'basic'
--[[function Teachmanager:getBasicTeaches()
	local result = {};
	
	for k, v in pairs(self.teachCategories) do
		for k2, v2 in pairs(v.teaches) do
			if v2.isBasic == true then
				table.insert(result, v2);
			end
		end
	end
	
	return result;
end]]

---Returns all teach-categories that are available for a player
function Teachmanager:getBasicCategoriesForPlayer(playerid)
	local result = {};
	local character = getCharacterById(playerid);
	
	for k, v in pairs(self.teachCategories) do
		if v:getContainsBasicTeaches() == true then
			table.insert(result, v);
		end
	end
	
	return result;
end

---creates the bookshelf-teach-menu
function Teachmanager:createTeachMenu(playerid, animID)
	local items = self:getBasicCategoriesForPlayer(playerid)
	
	if #items < 1 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du kannst hier nichts lernen.");
		
		return animID;
	end
	
	local menu = Menu.new(playerid, nil, animID, MENU_X_STANDARD, MENU_Y_STANDARD, instance_TeachSystem.isInTeachMenu);
	
	--add headline
	local headlineItem = StaticMenuItem.new("Du kannst folgendes lernen:");
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add content
	for k, v in pairs(items) do
		local menuItem = MenuItem.new(v);
		
		menu:addItem(menuItem);
	end
	
	--add exit
	local exitItem = StaticMenuItem.new("Ende");
	menu:setExit(MenuItem.new(exitItem));
		
	menu:createDraws();
	menu:updateTop();
	
	return menu;
end

---Creates the dual teach menu
--ATTENTION: items = teach categories
function Teachmanager:createDualTeachMenu(teacherid, studentid)
	instance_TeachSystem.isInTeachMenu[teacherid] = self:createTeacherMenu(teacherid, studentid);
	
	if not instance_TeachSystem.isInTeachMenu[teacherid] then return; end;
	
	instance_TeachSystem.isInTeachMenu[studentid] = self:createStudentMenu(studentid, teacherid);
	
	--set the second players angle -> looks at the first player
	SetPlayerAngle(studentid, GetAngleToPlayer(studentid,teacherid));
end

---Creates the teacher's part of the menu
function Teachmanager:createTeacherMenu(playerid, secondPlayerID)
	local items = self:getTeachableCategories(playerid);
	
	if #items < 1 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du kannst nichts lehren.");
		SendPlayerMessage(secondPlayerID, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("%s kann dir nichts beibringen.", instance_NameSystem:getPlayerNameFromViewOf(secondPlayerID, playerid)));
		
		return nil;
	end
	
	local menu = Menu.new(playerid, secondPlayerID, 0, MENU_X_STANDARD, MENU_Y_STANDARD, instance_TeachSystem.isInTeachMenu, nil, 1);
	
	--add headline
	local headlineItem = StaticMenuItem.new(string.format("Du kannst %s folgendes beibringen:", instance_NameSystem:getPlayerNameFromViewOf(playerid, secondPlayerID)));
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
						
						if instance_TeachSystem:isPlayerInValidMenu(secondPlayerID) then
							instance_TeachSystem.isInTeachMenu[secondPlayerID]:exitMenuAndSubMenus();
							--no message if the teacher finishes the teaching
						end
					end
		
	menu:createDraws();
	menu:updateTop();
	
	return menu;
end

---Creates the students's part of the menu
function Teachmanager:createStudentMenu(playerid, secondPlayerID)
	local menu = Menu.new(playerid, secondPlayerID, 0, MENU_X_STANDARD, MENU_Y_STANDARD, instance_TeachSystem.isInTeachMenu, nil, 0);
	
	--add headline
	local headlineItem = StaticMenuItem.new(string.format("%s bringt dir gerade etwas bei.", instance_NameSystem:getPlayerNameFromViewOf(playerid, secondPlayerID)));
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add exit
	local exitItem = MenuItem.new(StaticMenuItem.new("Ende"));
	menu:setExit(exitItem);
	
	exitItem.exec = function ()
						menu:exitMenu();
						
						if instance_TeachSystem:isPlayerInValidMenu(secondPlayerID) then
							instance_TeachSystem.isInTeachMenu[secondPlayerID]:exitMenuAndSubMenus();
							SendPlayerMessage(secondPlayerID, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, string.format("%s hat das Lernen abgebrochen.", instance_NameSystem:getPlayerNameFromViewOf(secondPlayerID, playerid)));
						end
					end
		
	menu:createDraws();
	menu:updateTop();
	
	return menu;
end

---Checks whether a player can teach ANY category
function Teachmanager:isTeacher(playerid)
	for k, v in pairs(self.teachCategories) do
		if v:canPlayerTeachCategory(playerid) then return true; end;
	end
	
	return false;
end

---Returns the categories a player can teach
function Teachmanager:getTeachableCategories(playerid)
	local result = {};
	
	for k, v in pairs(self.teachCategories) do
		if v:canPlayerTeachCategory(playerid) then			
			table.insert(result, v);
		end
	end
	
	return result;
end