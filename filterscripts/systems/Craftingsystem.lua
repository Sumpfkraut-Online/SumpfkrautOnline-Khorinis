require "filterscripts/classes/Crafting/Craftingmanager"

print("Loading Craftingsystem.lua");

instance_CraftingSystem = nil;

CraftingSystem = {};
CraftingSystem.__index = CraftingSystem;

---Creates an instance of CraftingSystem
function CraftingSystem.new()
	local newCraftingSystem = {};
	
	newCraftingSystem.craftingMan = Craftingmanager.new();
	newCraftingSystem.isInCraftingMenu = {};
	
	setmetatable(newCraftingSystem, CraftingSystem);
	
	return newCraftingSystem;
end

---Reloads crafting entries from DB
function CraftingSystem:reload()
	self.craftingMan = Craftingmanager.new();
end

---Checks whether a player is in a crafting menu
function CraftingSystem:isPlayerInValidMenu(playerid)
	return self.isInCraftingMenu[playerid] and type(self.isInCraftingMenu[playerid]) ~= "number";
end

---Callback: OnPlayerKey
function CraftingSystem:OnPlayerKey(playerid, keyDown)
	if self:isPlayerInValidMenu(playerid) == true then
		navigateMenu(playerid, keyDown, self.isInCraftingMenu);
	end
end

---Checks whether a player is an a crafting menu, if not: create one if possible
function CraftingSystem:check(playerid)
	local animID = tonumber(GetPlayerAnimationID(playerid));
	
	if self.isInCraftingMenu[playerid] then
		if type(self.isInCraftingMenu[playerid]) == "number" and self.isInCraftingMenu[playerid] ~= animID then
			if self.craftingMan.knownAnimations[animID] then
				--create a Menu for the player
				self.isInCraftingMenu[playerid] = self.craftingMan:createCraftingMenu(playerid, animID);
			else
				self.isInCraftingMenu[playerid] = animID;
			end
		end
	else
		if self.craftingMan.knownAnimations[animID] then
			--create a Menu for the player
			self.isInCraftingMenu[playerid] = self.craftingMan:createCraftingMenu(playerid, animID);
		end
	end
end

--instantiate system
if not instance_CraftingSystem then
	instance_CraftingSystem = CraftingSystem.new();
end