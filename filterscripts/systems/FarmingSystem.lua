require "filterscripts/classes/Farming/FarmingManager"

print("Loading FarmingSystem.lua");

instance_FarmingSystem = nil;

FarmingSystem = {};
FarmingSystem.__index = FarmingSystem;

---Creates an instance of FarmingSystem
function FarmingSystem.new()
	local newFarmingSystem = {};
	
	newFarmingSystem.farmingMan = FarmingManager.new();
	newFarmingSystem.isInFarmingMenu = {};
	
	setmetatable(newFarmingSystem, FarmingSystem);
  
  SetTimer("updatePlantsInDB", PLANT_UPDATE_INTERVAL * 1000, 1);
	
	return newFarmingSystem;
end

---Reloads all farming actions from the DB
function FarmingSystem:reload()
	self.farmingMan = FarmingManager.new();
end

---Checks if player is in a farming menu at the moment
function FarmingSystem:isPlayerInValidMenu(playerid)
	if self.isInFarmingMenu[playerid] then
		if type(self.isInFarmingMenu[playerid]) ~= "number" then
			return true;
		end
	end
	
	return false;
end

---React on navigation by the player
function FarmingSystem:OnPlayerKey(playerid, keyDown)
	if self:isPlayerInValidMenu(playerid) == true then
		navigateMenu(playerid, keyDown, self.isInFarmingMenu);
	end
end

--[[function FarmingSystem:OnPlayerUseItem(playerid, item_instance, amount, hand)
  self.farmingMan:OnPlayerUseItem(playerid, item_instance, amount, hand);
end]]

function updatePlantsInDB()
	instance_FarmingSystem.farmingMan:saveFarmingPoints();
end

--instantiate system
if not instance_FarmingSystem then
	instance_FarmingSystem = FarmingSystem.new();
end