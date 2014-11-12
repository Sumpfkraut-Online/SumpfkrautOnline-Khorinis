require "filterscripts/classes/Teach/Teachmanager"
require "filterscripts/classes/Skill/Skillmanager"

print("Loading Teachsystem.lua");

instance_TeachSystem = nil;

TeachSystem = {};
TeachSystem.__index = TeachSystem;

---Creates an instance of TeachSystem
function TeachSystem.new()
	local newTeachSystem = {};
	
	newTeachSystem.teachMan = Teachmanager.new();
	newTeachSystem.skillMan = Skillmanager.new();
	newTeachSystem.isInTeachMenu = {};
	
	setmetatable(newTeachSystem, TeachSystem);
	
	return newTeachSystem;
end

---Reloads all teaches from the DB
function TeachSystem:reload()
	self.teachMan = Teachmanager.new();
end

---Checks if player is in a teach menu at the moment
function TeachSystem:isPlayerInValidMenu(playerid)
	if self.isInTeachMenu[playerid] then
		if type(self.isInTeachMenu[playerid]) ~= "number" then
			return true;
		end
	end
	
	return false;
end

---Callback: OnPlayerKey
function TeachSystem:OnPlayerKey(playerid, keyDown)
	if self:isPlayerInValidMenu(playerid) == true then
		navigateMenu(playerid, keyDown, self.isInTeachMenu);
	end
end

---Checks whether a player is an a teach menu, if not: create one if possible
function TeachSystem:check(playerid)
	local animID = tonumber(GetPlayerAnimationID(playerid));
	
	
	if self.isInTeachMenu[playerid] then
		if type(self.isInTeachMenu[playerid]) == "number" and self.isInTeachMenu[playerid] ~= animID then
			if animID == 98 then
				--create a Menu for the player
				self.isInTeachMenu[playerid] = self.teachMan:createTeachMenu(playerid, animID);
			else
				self.isInTeachMenu[playerid] = animID;
			end
		end
	else
		if animID == 98 then --[[98 = animation id for reading in a book]]
			--create a Menu for the player
			self.isInTeachMenu[playerid] = self.teachMan:createTeachMenu(playerid, animID);
		end
	end
end

--instantiate system
if not instance_TeachSystem then
	instance_TeachSystem = TeachSystem.new();
end