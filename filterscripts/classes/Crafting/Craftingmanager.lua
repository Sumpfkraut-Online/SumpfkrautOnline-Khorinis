require "filterscripts/classes/Menu/Menu"
require "filterscripts/classes/Menu/MenuItem"
require "filterscripts/classes/Menu/StaticMenuItem"
require "filterscripts/classes/Crafting/Crafting"

print("Loading Craftingmanager.lua");

Craftingmanager = {};
Craftingmanager.__index = Craftingmanager;

---Creates an instance of Craftingmanager
function Craftingmanager.new()
	local newCraftingmanager = {};
	
	setmetatable(newCraftingmanager, Craftingmanager);
	
	newCraftingmanager.crafts = {};
	newCraftingmanager.knownAnimations = {};
	
	newCraftingmanager:loadCraftingsFromDB();
	newCraftingmanager:loadAllKnownAnimationsFromDB();
	
	return newCraftingmanager;
end

---Adds a crafting to this crafting manager instance
function Craftingmanager:addCrafting(crafting)
	if self.crafts[crafting.craftID] then return false; end;
	
	self.crafts[crafting.craftID] = crafting;
end

---Returns all craftings for a player and a certain animation id
function Craftingmanager:getCraftingsForPlayerAndAnimID(playerid, animID)
	local result = {};
	
	for k, v in pairs(self.crafts) do
		if v:containsAnimID(animID) == true and v:checkRequiredSkills(playerid) == true then
			table.insert(result, v);
		end
	end
	
	return result;
end

---Creates a crafting menu for a player
function Craftingmanager:createCraftingMenu(playerid, animID)
	local items = self:getCraftingsForPlayerAndAnimID(playerid, animID);
	
	if #items < 1 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du kannst hier nichts herstellen.");
		
		return animID;
	end
	
	local menu = Menu.new(playerid, nil, animID, MENU_X_STANDARD, MENU_Y_STANDARD, instance_CraftingSystem.isInCraftingMenu);
	
	--add headline
	local headlineItem = StaticMenuItem.new("Du kannst folgendes herstellen:");
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

---Loads all available craftings from DB
function Craftingmanager:loadCraftingsFromDB()
	local result = runSQL("SELECT id, caption, duration, propability FROM cr_crafting;");
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local cr = Crafting.new(tonumber(row[1]), row[2], tonumber(row[3]), tonumber(row[4]));
			
			self:addCrafting(cr);
			
			row = mysql_fetch_row(result);
		end
	end
end

---Loads all available animation ids from DB
function Craftingmanager:loadAllKnownAnimationsFromDB()
	local result = runSQL("SELECT DISTINCT(anim_id) FROM cr_playeranim ORDER BY anim_id;");
		
	if result then
		local row = mysql_fetch_row(result);
		while row do
			self.knownAnimations[tonumber(row[1])] = 1;
			
			row = mysql_fetch_row(result);
		end
	end
end