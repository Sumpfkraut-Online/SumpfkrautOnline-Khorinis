print("Loading Hunt.lua");

Hunt = {};
Hunt.__index = Hunt;

---Creates an instance of Hunt
function Hunt.new(huntID, caption, duration, propability, hasToBeDead)
	local newHunt = {};
	
	setmetatable(newHunt, Hunt);
	
	newHunt.huntID = huntID;
	newHunt.caption = caption;
	newHunt.duration = duration;
	newHunt.propability = propability;
  newHunt.hasToBeDead = hasToBeDead;
	
	--contains the amount this item can be executed by the player
	-- 0 means <-> not executable anymore
	-- -1 means <-> infinite
	newHunt.executesLeft = -1;
	
	newHunt.instances = {};
	
	--needed items for the hunting process (e.g. Ruebenstecher)
	newHunt.requiredTools = {};
	--needed skills for the hunting process
	newHunt.requiredSkills = {};
	--items that result from the hunting process
	newHunt.resultingItems = {};
	
	--initialize
	newHunt:loadInstances();
	newHunt:loadRequiredTools();
	newHunt:loadRequiredSkills();
	newHunt:loadResultingItems();
	
	return newHunt;
end

---Checks whether this hunt contains a certain instance id
function Hunt:containsInstanceID(instanceID)
	if self.instances[instanceID] then
		return true;
	end

	return false;
end

---Checks whether the hunt can be executed by the player or not
function Hunt:checkExecConditions(playerid)
	if self:checkConditions(playerid) == false then		
		return false;
	end
	
	--check whether the player can execute this option
	if self.executesLeft == 0 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Das hast du bereits versucht.");
		return false;
	end
	
	SendPlayerMessage(playerid, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, string.format("Du versuchst %s. Das wird circa %d s dauern.", self.caption, self.duration / 1000));
	
	--play plunder animation
	PlayAnimation(playerid, "T_PLUNDER");
	
	return true;
end

---Executes the hunt
--ATTENTION: This function does not check the requirements, this has to be done in the 'exec'-function of the parent MenuItem
function Hunt:exec(playerid)
	--check propability
	--ATTENTION: if 0 or 100 -> ignore.
	if self.propability > 0 and self.propability < 100 then
		local rand = math.random(100);

		if rand > self.propability then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast beim %s einen Fehler gemacht.", self.caption));
			return;
		end
	end

	--add the items
	for k, v in pairs(self.resultingItems) do
		GiveItem(playerid, v[1], tonumber(v[2]));
	end
	
	--reduce the remaining executes
	self.executesLeft = self.executesLeft - 1;
	
	SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s erfolgreich durchgefuehrt.", self.caption));
end

---Checks whether the player fulfills all conditions for this hunt
function Hunt:checkConditions(playerid)
	local inventory = getPlayerInventory(playerid);
	
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

---Checks whether the player has the required tools
function Hunt:checkRequiredTools(inventory)
	for k, v in pairs(self.requiredTools) do
		--player has not enought resources
		if inventory:getItemCount(v) < 1 then
			return false;
		end
	end
	
	return true;
end

---Checks whether the player has the required skills
function Hunt:checkRequiredSkills(playerid)
	local character = getCharacterById(playerid);
	
	for k, v in pairs(self.requiredSkills) do
		--player has to low skillvalue
		if character:getSkillValue(v[1]) < v[2] then
			return false;
		end
	end
	
	return true;
end


---Loads the affected (player)-instances from the DB
function Hunt:loadInstances()
	local result = runSQL(string.format("SELECT inst_id FROM ht_targetinst WHERE hunt_id = %s;", self.huntID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			self.instances[tonumber(row[1])] = 1;
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the required tools from DB
function Hunt:loadRequiredTools()
	local result = runSQL(string.format("SELECT item_id FROM ht_reqtools WHERE hunt_id = %s;", self.huntID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			table.insert(self.requiredTools, instance_InventorySystem.inventoryMan:getItemByID(row[1]).instanceName);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the required skills from DB
function Hunt:loadRequiredSkills()
	local result = runSQL(string.format("SELECT skill_id, value FROM ht_reqskills WHERE hunt_id = %s;", self.huntID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			
			table.insert(self.requiredSkills, { tonumber(row[1]), tonumber(row[2]) });
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the resulting items from the DB
function Hunt:loadResultingItems()
	local result = runSQL(string.format("SELECT item_id, amount FROM ht_resitems WHERE hunt_id = %s;", self.huntID));
	
	if result then
		local row = mysql_fetch_row(result);
	
		while row do
			table.insert(self.resultingItems, { instance_InventorySystem.inventoryMan:getItemByID(row[1]).instanceName, tonumber(row[2]) });
				
			row = mysql_fetch_row(result);
		end
	end
end