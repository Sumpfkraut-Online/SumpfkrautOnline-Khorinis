print("Loading ElementTellName.lua");

ElementTellName = {};
ElementTellName.__index = ElementTellName;

---Creates an instance of ElementTellName
function ElementTellName.new()
	local newElementTellName = {};
	
	newElementTellName.caption = "Meinen Name nennen";
	
	setmetatable(newElementTellName, ElementTellName);
	
	
	return newElementTellName;
end

---Executes this instance
function ElementTellName:exec(playerid)
	instance_InteractionSystem.isInInteractionMenu[playerid]:exitMenu();
	
	local focusID = GetFocus(playerid);
		
	if focusID ~= -1 then
		--get the other player's character and add 'my' id to his friend-list
		local character = getCharacterById(focusID);
		character:addFriend(playerid);
		
		instance_NameSystem:OnPlayerFocus(focusID, playerid);
		
		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s deinen Namen genannt.", instance_NameSystem:getPlayerNameFromViewOf(playerid, focusID)));
		SendPlayerMessage(focusID, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("%s hat dir seinen Namen genannt.", instance_NameSystem:getPlayerNameFromViewOf(focusID, playerid)));
	end
end

---Checks the execution conditions of this instance
function ElementTellName:checkExecConditions(playerid)
	return true;
end