require "filterscripts/classes/Menu/Menu"
require "filterscripts/classes/Menu/MenuItem"
require "filterscripts/classes/Menu/StaticMenuItem"
require "filterscripts/classes/Hunt/Hunt"

print("Loading Huntmanager.lua");

Huntmanager = {};
Huntmanager.__index = Huntmanager;

---Creates an instance of Huntmanager
function Huntmanager.new()
	local newHuntmanager = {};
	
	setmetatable(newHuntmanager, Huntmanager);
	
	newHuntmanager.hunts = {};
	
	newHuntmanager:loadHuntsFromDB();
	
	return newHuntmanager;
end

---Adds a hunt to this hunt manager instance
function Huntmanager:addHunt(hunt)
	if self.hunts[hunt.huntID] then return false; end;
	
	self.hunts[hunt.huntID] = hunt;
end

---Creates a hunt menu for a player
function Huntmanager:createHuntMenu(playerid, instanceName, isDead)
	local items = self:getHuntsForPlayerAndInstance(playerid, instanceName, isDead);
	
	if #items < 1 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du hast hier keine Interaktionsmoeglichkeit.");
		
		return nil;
	end
	
	local menu = Menu.new(playerid, nil, nil, MENU_X_STANDARD, MENU_Y_STANDARD, instance_HuntSystem.isInHuntMenu);
	
	--add headline
	local headlineItem = StaticMenuItem.new("Du kannst folgendes versuchen:");
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add content
	for k, v in pairs(items) do
		v.executesLeft = 1;
		
		local menuItem = MenuItem.new(v);
		
		menu:addItem(menuItem);
	end
	
	--add exit
	local exitItem = MenuItem.new(StaticMenuItem.new("Ende"));
	menu:setExit(exitItem);
	
	exitItem.exec = function ()
						menu:exitMenu();

						--TODO: Focus? Rather set focused id when opening menu
						--TODO: Set flag 'plundered' to 'true'
						--DestroyNPC(GetFocus(playerid));
					end
		
	menu:createDraws();
	menu:updateTop();
	
	return menu;
end

---Returns the hunts that are available for the focused instance and the players skills
function Huntmanager:getHuntsForPlayerAndInstance(playerid, instanceName, isDead)
	local instanceID = -1;
	local res = {};
	
	--TODO: Load instance names - ids in RAM
	local result = runSQL(string.format("SELECT id FROM ht_instance WHERE name = '%s';", instanceName));
	
	if result then
		local row = mysql_fetch_row(result);
		
		if row then
			instanceID = tonumber(row[1]);
		end
	end
	
	if instanceID == -1 then return res; end;
	
	for k, v in pairs(self.hunts) do
		if v:containsInstanceID(instanceID) == true and v.hasToBeDead == isDead and v:checkRequiredSkills(playerid) == true then
			table.insert(res, v);
		end
	end
	
	return res;
end

---Loads all available hunts from DB
function Huntmanager:loadHuntsFromDB()
	local result = runSQL("SELECT id, caption, duration, propability, dead FROM ht_hunt;");
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local ht = Hunt.new(tonumber(row[1]), row[2], tonumber(row[3]), tonumber(row[4]), tonumber(row[5]));
			
			self:addHunt(ht);
			
			row = mysql_fetch_row(result);
		end
	end
end