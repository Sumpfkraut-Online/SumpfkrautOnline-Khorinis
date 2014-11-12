print("Loading ElementTeach.lua");

ElementTeach = {};
ElementTeach.__index = ElementTeach;

---Creates an instance of ElementTeach
function ElementTeach.new()
	local newElementTeach = {};
	
	newElementTeach.caption = "Ausbilden";
	
	setmetatable(newElementTeach, ElementTeach);
	
	
	return newElementTeach;
end

---Executes this instance
function ElementTeach:exec(playerid)
	instance_InteractionSystem.isInInteractionMenu[playerid]:exitMenu();

	local focusID = GetFocus(playerid);
	
	if focusID ~= -1 then
		if isInAnyMenu(focusID) == true then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("%s ist gerade beschaeftigt.", instance_NameSystem:getPlayerNameFromViewOf(playerid, focusID)));
			
			return;
		end
		
		if instance_TeachSystem:isPlayerInValidMenu(playerid) == false then
			instance_TeachSystem.teachMan:createDualTeachMenu(playerid, focusID);
		end
	end
end

---Checks the execution conditions of this instance
function ElementTeach:checkExecConditions(playerid)
	return true;
end