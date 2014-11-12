require "filterscripts/classes/Menu/Menu"
require "filterscripts/classes/Menu/MenuItem"
require "filterscripts/classes/Menu/StaticMenuItem"
require "filterscripts/classes/Farming/FarmingCategory"

print("Loading FarmingManager.lua");

FarmingManager = {};
FarmingManager.__index = FarmingManager;

---Creates an instance of FarmingManager
function FarmingManager.new()
	local newFarmingManager= {};
	
	setmetatable(newFarmingManager, FarmingManager);
	
	newFarmingManager.categories = {};
	newFarmingManager.farmingPositions = {};
	
	newFarmingManager:loadCategoriesFromDB();
	
	newFarmingManager:loadFarmingPoints();
	
	return newFarmingManager;
end

---Creates a hunt menu for a player
function FarmingManager:createFarmingMenu(playerid)
	local items = self.categories;
	
	if #items < 1 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du kannst hier nichts anpflanzen.");
		
		return nil;
	end
	
	local menu = Menu.new(playerid, nil, nil, MENU_X_STANDARD, MENU_Y_STANDARD, instance_FarmingSystem.isInFarmingMenu);
	
	--add headline
	local headlineItem = StaticMenuItem.new("Du kannst folgendes versuchen:");
	menu:setHeadline(MenuItem.new(headlineItem));
	
	--add content
	for k, v in pairs(items) do
		local menuItem = MenuItem.new(v);
		
		menu:addItem(menuItem);
	end
	
	--add exit
	local exitItem = MenuItem.new(StaticMenuItem.new("Ende"));
	menu:setExit(exitItem);
	
	--[[exitItem.exec = function ()
						menu:exitMenu();
					end]]
		
	menu:createDraws();
	menu:updateTop();
	
	return menu;
end




---Adds a category to this farming manager instance
function FarmingManager:addCategory(category)
	if self.categories[category.categoryID] then return false; end;
	
	self.categories[category.categoryID] = category;
end

---Loads all farming categories from the database
function FarmingManager:loadCategoriesFromDB()
	local result = runSQL("SELECT id, caption FROM fg_categories");
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local cat = FarmingCategory.new(tonumber(row[1]), row[2]);

			self:addCategory(cat);
				
			row = mysql_fetch_row(result);
		end
	end
end




---Adds a farming position to the manager
function FarmingManager:addFarmingPosition(farmingPosition)
	table.insert(self.farmingPositions, farmingPosition);
end

---Removes a farming position from the manager
function FarmingManager:removeFarmingPosition(x, y, z)
	local fp = nil;
	local removeAt = -1;
	
	local y_ = y - 100;
	
	for i = 1, #self.farmingPositions do
		fp = self.farmingPositions[i];
		
		if fp.x == x and fp.y == y_ and fp.z == z then
			removeAt = i;
			break;
		end
	end
	
	if removeAt ~= -1 then
		fp:deinit();
		
		table.remove(self.farmingPositions, removeAt);
		print("point removed");
	end
end

---Checks whether a plant has enought space to grow
function FarmingManager:checkPlantDistance(farmingPosition)
	local distance;
	
	for k, v in pairs(self.farmingPositions) do
		distance = math.sqrt(math.pow(farmingPosition.x - v.x, 2) + math.pow(farmingPosition.y - v.y, 2) + math.pow(farmingPosition.z - v.z, 2));
		print(distance);
		
		if distance < farmingPosition.radius then
			return 0;
		end
	end
	
	return 1;
end

---Pours all plants in the given radius
function FarmingManager:waterPlants(x, y, z, radius)
	local distance;
	
	local cnt = 0;
	
	for k, v in pairs(self:createFlatTableCopy(self.farmingPositions)) do
		distance = math.sqrt(math.pow(x - v.x, 2) + math.pow(y - v.y, 2) + math.pow(z - v.z, 2));
		print(distance);
		
		if distance <= radius then
			v:setWater(true);
			cnt = cnt + 1;
		end
		
	end
	
	print(cnt .. " Pflanzen bewaessert.");
end

---Fertilizes all plants in the given radius
function FarmingManager:fertilizePlants(x, y, z, radius)
	local distance;
	
	local cnt = 0;
	
	for k, v in pairs(self:createFlatTableCopy(self.farmingPositions)) do
		distance = math.sqrt(math.pow(x - v.x, 2) + math.pow(y - v.y, 2) + math.pow(z - v.z, 2));
		print(distance);
		
		if distance <= radius then
			v:setFertilizer(true);
			cnt = cnt + 1;
		end
	end
	
	print(cnt .. " Pflanzen geduengt.");
end

function FarmingManager:createFlatTableCopy(tbl)
	local result = {};
	
	for k, v in pairs(tbl) do
		result[k] = v;
	end
	
	return result;
end


function FarmingManager:loadFarmingPoints()
	local result = runSQL("SELECT x, y, z, world, instance_name, water, fertilizer, reported FROM fg_positions");
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local fp = FarmingPosition.new(nil, row[5], 0, tonumber(row[1]), tonumber(row[2]), tonumber(row[3]), row[4], tonumber(row[6]), tonumber(row[7]), tonumber(row[8]));

			self:addFarmingPosition(fp);
				
			row = mysql_fetch_row(result);
		end
	end
end

function FarmingManager:saveFarmingPoints()
	if #self.farmingPositions < 1 then return; end;

	local s = "";
	
	for k, v in pairs(self.farmingPositions) do
		local water = 0;
		if v.water == true then
			water = 1;
		end
		
		local fertilizer = 0;
		if v.fertilizer == true then
			fertilizer = 1;
		end
    
    local reported = 0;
    if v.reported == true then
      reported = 1;
    end
    
		
		s = string.format("%s (%d, %d, %d, '%s', '%s', %d, %d, %d),", s, v.x, v.y, v.z, v.world, v.itemInstance, water, fertilizer, reported);
	end
	
	s = s:sub(1, s:len() - 1);
	
	runSQL("TRUNCATE TABLE fg_positions");
	runSQL(string.format("INSERT INTO fg_positions(x, y, z, world, instance_name, water, fertilizer, reported) VALUES %s ON DUPLICATE KEY UPDATE instance_name = VALUES(instance_name), water = VALUES(water), fertilizer = VALUES(fertilizer), reported = VALUES(reported);", s));
end

--[[function FarmingManager:OnPlayerUseItem(playerid, item_instance, amount, hand)
  for k, v in pairs(self.categories) do
    for k2, v2 in pairs(v.actions) do
      if v2.specialAction == 0 and #v2.requiredItems > 0 then
        local instance = v2.requiredItems[1][1];
        
        if instance:upper() == item_instance:upper() then
          if v2:checkExecConditions(playerid) == true then
            v2:exec(playerid);
          else
            GiveItem(playerid, item_instance, 1);
          end
          
        end
      end
    end
  end
end]]

