print("Loading ElementPlunder.lua");

ElementPlunder = {};
ElementPlunder.__index = ElementPlunder;

---Creates an instance of ElementPlunder
function ElementPlunder.new()
	local newElementPlunder = {};
	
	newElementPlunder.caption = "Ausrauben";
	
	setmetatable(newElementPlunder, ElementPlunder);
	
	
	return newElementPlunder;
end

---Executes this instance
function ElementPlunder:exec(playerid)
	instance_InteractionSystem.isInInteractionMenu[playerid]:exitMenu();
	
	local focusID = GetFocus(playerid);
	
	if focusID == -1 then return nil; end;
	
	
	if IsUnconscious(focusID) == 0 and IsDead(focusID) == 0 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("%s ist nicht mehr bewusstlos.", instance_NameSystem:getPlayerNameFromViewOf(playerid, focusID)));
		
		return;
	end
	
	local victim = getCharacterById(focusID);
	
	--check if plunder time interval is exceeded
	if os.difftime(os.time(), victim.lastPlunderTime) < MIN_PLUNDER_SAFETY_TIME then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("%s wurde vor kurzem Ausgeraubt.", instance_NameSystem:getPlayerNameFromViewOf(playerid, focusID)));
		return;
	end
	
	local inventoryVictim = getPlayerInventory(focusID);
	
	if not inventoryVictim then
		LogString("Plunder", string.format("Player with playerid %s has no inventory.", focusID));
		
		return;
	end
	
	local robbedGold = math.floor(inventoryVictim:getItemCount("ItMi_Gold") * PLUNDER_PERCENT);
	
	GiveItem(playerid, "ItMi_Gold", robbedGold);
	RemoveItem(focusID, "ItMi_Gold", robbedGold)
	
	--play plunder animation
	PlayAnimation(playerid, "T_PLUNDER");
	
	SendPlayerMessage(focusID, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("%s hat dir %d Gold gestohlen.", instance_NameSystem:getPlayerNameFromViewOf(focusID, playerid), robbedGold));
	SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s %d Gold gestohlen.", instance_NameSystem:getPlayerNameFromViewOf(playerid, focusID), robbedGold));
	
	--set last plundered time
	victim.lastPlunderTime = os.time();
end

---Checks the execution conditions of this instance
function ElementPlunder:checkExecConditions(playerid)
	return true;
end