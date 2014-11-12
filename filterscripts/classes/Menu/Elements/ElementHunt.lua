print("Loading ElementHunt.lua");

ElementHunt = {};
ElementHunt.__index = ElementHunt;

---Creates an instance of ElementHunt
function ElementHunt.new()
	local newElementHunt = {};
	
	newElementHunt.caption = "Beute nehmen";
	
	setmetatable(newElementHunt, ElementHunt);
	
	
	return newElementHunt;
end

---Executes this instance
function ElementHunt:exec(playerid)
	instance_InteractionSystem.isInInteractionMenu[playerid]:exitMenu();

	local focusID = GetFocus(playerid);
	
	if focusID ~= -1 then	
		if instance_HuntSystem:isPlayerInValidMenu(playerid) == false then
			instance_HuntSystem.isInHuntMenu[playerid] = instance_HuntSystem.huntMan:createHuntMenu(playerid, GetPlayerInstance(focusID):upper());
		end
	end
end

---Checks the execution conditions of this instance
function ElementHunt:checkExecConditions(playerid)
	return true;
end