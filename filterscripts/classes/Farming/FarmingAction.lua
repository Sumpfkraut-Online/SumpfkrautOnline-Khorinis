require "filterscripts/classes/Farming/FarmingPosition"

print("Loading FarmingAction.lua");

FarmingAction = {};
FarmingAction.__index = FarmingAction;


---Creates an instance of FarmingAction
function FarmingAction.new(actionID, caption, duration, radius, specialAction)
	local newFarmingAction = {};

	setmetatable(newFarmingAction, FarmingAction);
	
	newFarmingAction.actionID = actionID;
	newFarmingAction.caption = caption;
	newFarmingAction.duration2 = duration;
	
	newFarmingAction.parentMenu = nil;
	
	newFarmingAction.radius = radius;
	newFarmingAction.specialAction = specialAction;
	
	--needed items to start the farming action
	newFarmingAction.requiredItems = {};
	--needed items for the farming action (e.g. Ruebenstecher)
	newFarmingAction.requiredTools = {};
	--needed skills for the farming action
	newFarmingAction.requiredSkills = {};
	--items that result from the farming action
	newFarmingAction.resultingItems = {};
	
	--initialize
	newFarmingAction:loadRequiredItems();
	newFarmingAction:loadRequiredTools();
	newFarmingAction:loadRequiredSkills();
	newFarmingAction:loadResultingItems();
	
	return newFarmingAction;
end

---Checks whether the farming action can be executed by the player
function FarmingAction:checkExecConditions(playerid)
	if self:checkConditions(playerid) == false then		
		return false;
	end

	
	return true;
end



---Execute the farming action
--ATTENTION: This function does not check the requirements, this has to be done in the 'exec'-function of the parent MenuItem
function FarmingAction:exec(playerid)
	--remove the source items
	for k, v in pairs(self.requiredItems) do
		RemoveItem(playerid, v[1], tonumber(v[2]));
	end

	if self.specialAction == 0 then
		if #self.resultingItems < 1 then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Datenbankfehler. %s ist nicht ausfuehrbar.", self.caption));
			return;
		end
		--add vob
		local newPos = FarmingPosition.new(playerid, self.resultingItems[1][1], self.radius);
	
		if newPos then
			instance_FarmingSystem.farmingMan:addFarmingPosition(newPos);
		
			PlayAnimation(playerid, "S_RAKE_S1");
			SetTimerEx("resetAnimation", self.duration2, 0, playerid);
		end
	
	elseif self.specialAction == 1 then
		--watering
		
		local x, y, z = GetPlayerPos(playerid);
		
		instance_FarmingSystem.farmingMan:waterPlants(x, y, z, self.radius);
		
		PlayAnimation(playerid, "S_WASH");
		SetTimerEx("resetAnimation", self.duration2, 0, playerid)
		
	elseif self.specialAction == 2 then
		--fertilizing
	
		local x, y, z = GetPlayerPos(playerid);
		
		instance_FarmingSystem.farmingMan:fertilizePlants(x, y, z, self.radius);
		
		PlayAnimation(playerid, "S_WASH");
		SetTimerEx("resetAnimation", self.duration2, 0, playerid)
	end
	
	if self.parentMenu then
		self.parentMenu:exitMenuAndSuperiorMenus();
	end
end

function resetAnimation(playerid)
	PlayAnimation(playerid, "S_RUN");
end



---Checks whether the player fulfills all conditions for this farming action
function FarmingAction:checkConditions(playerid)
	local inventory = getPlayerInventory(playerid);
	
	if self:checkRequiredItems(inventory) == false then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast nicht die notwendigen Rohstoffe, um %s durchzufuehren.", self.caption));
		
		return false;
	end
	
	if self:checkRequiredTools(inventory) == false then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast nicht das notwendige Werkzeug, um %s durchzufuehren.", self.caption));
		return false;
	end
	
	if self:checkRequiredSkills(playerid) == false then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast nicht die notwendigen Faehigkeiten, um %s durchzufuehren.", self.caption));
		return false;
	end
	
	return true;
end

---Checks whether the player has enought resource items
function FarmingAction:checkRequiredItems(inventory)
	for k, v in pairs(self.requiredItems) do
		--player has not enought resources
		if inventory:getItemCount(v[1]) < v[2] then
			return false;
		end
	end
	
	return true;
end

---Checks whether the player has the required tools
function FarmingAction:checkRequiredTools(inventory)
	for k, v in pairs(self.requiredTools) do
		--player has not enought resources
		if inventory:getItemCount(v) < 1 then
			return false;
		end
	end
	
	return true;
end

---Checks whether the player has the required skills
function FarmingAction:checkRequiredSkills(playerid)
	local character = getCharacterById(playerid);
	
	for k, v in pairs(self.requiredSkills) do
		--player has not enought skill
		if character:getSkillValue(v[1]) < v[2] then
			return false;
		end
	end
	
	return true;
end





---Loads the required tools from DB
function FarmingAction:loadRequiredItems()
	local result = runSQL(string.format("SELECT item_id, cnt FROM fg_reqitems WHERE action_id = %s;", self.actionID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			table.insert(self.requiredItems, { instance_InventorySystem.inventoryMan:getItemByID(row[1]).instanceName, tonumber(row[2]) });
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the required tools from DB
function FarmingAction:loadRequiredTools()
	local result = runSQL(string.format("SELECT item_id FROM fg_reqtools WHERE action_id = %s;", self.actionID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			table.insert(self.requiredTools, instance_InventorySystem.inventoryMan:getItemByID(row[1]).instanceName);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the required skills from DB
function FarmingAction:loadRequiredSkills()
	local result = runSQL(string.format("SELECT skill_id, amount FROM fg_reqskills WHERE action_id = %s;", self.actionID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			
			table.insert(self.requiredSkills, { tonumber(row[1]), tonumber(row[2]) });
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the resulting items from the DB
function FarmingAction:loadResultingItems()
	local result = runSQL(string.format("SELECT item_id, cnt FROM fg_resitems WHERE action_id = %s;", self.actionID));
	
	if result then
		local row = mysql_fetch_row(result);
	
		while row do
			table.insert(self.resultingItems, { instance_InventorySystem.inventoryMan:getItemByID(row[1]).instanceName, tonumber(row[2]) });
				
			row = mysql_fetch_row(result);
		end
	end
end