print("Loading ElementTrading.lua");

ElementTrading = {};
ElementTrading.__index = ElementTrading;

---Creates an instance of ElementTrading
function ElementTrading.new()
	local newElementTrading = {};
	
	newElementTrading.caption = "Handeln";
	
	setmetatable(newElementTrading, ElementTrading);
	
	
	return newElementTrading;
end

---Executes this instance
function ElementTrading:exec(playerid)
	instance_InteractionSystem.isInInteractionMenu[playerid]:exitMenu();
	
	local focusID = GetFocus(playerid);
	
	if focusID ~= -1 then
		if isInAnyMenu(focusID) == true then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("%s ist gerade beschaeftigt.", instance_NameSystem:getPlayerNameFromViewOf(playerid, focusID)));
			
			return;
		end
		
		if instance_InventorySystem:isPlayerInValidMenu(playerid) == false then
			--simply call this function -> the entry in the managing table is set in the function
			instance_InventorySystem.inventoryMan:createTradeMenu(playerid, focusID);
		end
	end
end

---Checks the execution conditions of this instance
function ElementTrading:checkExecConditions(playerid)
	return true;
end