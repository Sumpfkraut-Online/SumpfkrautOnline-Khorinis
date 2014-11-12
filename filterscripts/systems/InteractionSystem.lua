require "filterscripts/classes/Menu/Elements/ElementTellName"
require "filterscripts/classes/Menu/Elements/ElementTeach"
require "filterscripts/classes/Menu/Elements/ElementTrading"
require "filterscripts/classes/Menu/Elements/ElementPlunder"

print("Loading InteractionSystem.lua");

instance_InteractionSystem = nil;

InteractionSystem = {};
InteractionSystem.__index = InteractionSystem;

---Creates an instance of InteractionSystem
function InteractionSystem.new()
	local newInteractionSystem = {};
	
	newInteractionSystem.isInInteractionMenu = {};
	
	setmetatable(newInteractionSystem, InteractionSystem);
	
	return newInteractionSystem;
end

---Creates an interaction menu
function InteractionSystem:createInteractionMenu(playerid)
	local focusID = GetFocus(playerid);
	
	if GetPlayerInstance(focusID) == "PC_HERO" then
		return self:createInteractionMenuForPlayer(playerid, focusID);
	else
		self:createInteractionMenuForMonster(playerid, focusID);
		
		--no return value
	end
end

---Creates an interaction menu for a focused monster
function InteractionSystem:createInteractionMenuForMonster(playerid, focusID)
	--hunting
	if instance_HuntSystem:isPlayerInValidMenu(playerid) == false then
		instance_HuntSystem.isInHuntMenu[playerid] = instance_HuntSystem.huntMan:createHuntMenu(playerid, GetPlayerInstance(focusID):upper(), IsDead(focusID));
	end
end

---Creates the interaction menu for a player
function InteractionSystem:createInteractionMenuForPlayer(playerid, focusID)
	local items = self:getInteractionPossibilities(playerid, focusID);
	
	if not items then return nil; end;
	
	--there are no interaction possibilites between the two player
	if #items < 1 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du kannst mit diesem Spieler nicht interagieren.");
		return;
	end
	local menu = Menu.new(playerid, nil, 0, MENU_X_STANDARD, MENU_Y_STANDARD, instance_InteractionSystem.isInInteractionMenu);
	
	--add headline
	local headlineItem = StaticMenuItem.new("Interaktionsmoeglichkeiten:");
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


---Returns all possible interactions for a player
function InteractionSystem:getInteractionPossibilities(playerid, focusID)
	local result = {};
	
	
	
	if focusID == -1 then
		--no player is focused
		
		--open farming menu
		if instance_FarmingSystem:isPlayerInValidMenu(playerid) == false then
			instance_FarmingSystem.isInFarmingMenu[playerid] = instance_FarmingSystem.farmingMan:createFarmingMenu(playerid);
		end
		
		return nil;
		
	else
		--players can not interact with NPCs
		if IsNPC(focusID) == 1 then return nil; end;
		
		local character = getCharacterById(focusID);
	
		if not character then return nil; end;
	
		--actions that can only be done when the other player is concious
		if IsUnconscious(focusID) == 0 and IsDead(focusID) == 0 then
			--teaching
			if instance_TeachSystem.teachMan:isTeacher(playerid) then
				table.insert(result, ElementTeach.new());
			end
	
			--trading
			table.insert(result, ElementTrading.new());
			
		else
		--actions that can only be done when the other player is unconcious
			--plundering
			table.insert(result, ElementPlunder.new());
		end
	
	
		--actions that can always be done
	
		--tell name
		if character:isFriend(playerid) == false then
			table.insert(result, ElementTellName.new());
		end
	end
	
	return result;
end


---React on navigation by the player
function InteractionSystem:OnPlayerKey(playerid, keyDown)
	if self:isPlayerInValidMenu(playerid) == true then
		navigateMenu(playerid, keyDown, self.isInInteractionMenu);

	elseif isInAnyMenu(playerid) == false then
		if keyDown == KEY_X then
			self.isInInteractionMenu[playerid] = instance_InteractionSystem:createInteractionMenu(playerid);
		end
	end
end


---Checks if player is in a interaction menu at the moment
function InteractionSystem:isPlayerInValidMenu(playerid)
	if self.isInInteractionMenu[playerid] then
		if type(self.isInInteractionMenu[playerid]) ~= "number" then
			return true;
		end
	end
	
	return false;
end


--instantiate interaction system
if not instance_InteractionSystem then
	instance_InteractionSystem = InteractionSystem.new();
end
