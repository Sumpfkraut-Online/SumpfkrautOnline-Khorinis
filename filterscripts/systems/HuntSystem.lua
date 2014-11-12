require "filterscripts/classes/Hunt/Huntmanager"

print("Loading HuntSystem.lua");

instance_HuntSystem = nil;

HuntSystem = {};
HuntSystem.__index = HuntSystem;

---Creates an instance of HuntSystem
function HuntSystem.new()
	local newHuntSystem = {};
	
	newHuntSystem.huntMan = Huntmanager.new();
	newHuntSystem.isInHuntMenu = {};
	
	setmetatable(newHuntSystem, HuntSystem);
	
	return newHuntSystem;
end

---Reloads all hunts from the DB
function HuntSystem:reload()
	self.huntMan = Huntmanager.new();
end

---Checks if player is in a hunt menu at the moment
function HuntSystem:isPlayerInValidMenu(playerid)
	if self.isInHuntMenu[playerid] then
		if type(self.isInHuntMenu[playerid]) ~= "number" then
			return true;
		end
	end
	
	return false;
end

---React on navigation by the player
function HuntSystem:OnPlayerKey(playerid, keyDown)
	if self:isPlayerInValidMenu(playerid) == true then
		navigateMenu(playerid, keyDown, self.isInHuntMenu);
	end
end

--instantiate system
if not instance_HuntSystem then
	instance_HuntSystem = HuntSystem.new();
end