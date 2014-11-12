require "filterscripts/classes/Crafting/CraftingSkill"

print("Loading Crafting.lua");

Crafting = {};
Crafting.__index = Crafting;

---Creates an instance of Crafting
function Crafting.new(craftID, caption, duration, propability)
	local newCrafting = {};
	
	setmetatable(newCrafting, Crafting);
	
	newCrafting.craftID = craftID;
	newCrafting.animID = {};
	
	newCrafting.caption = caption;
	newCrafting.duration = duration;
	newCrafting.propability = propability;
	
	--needed items to start the crafting process
	newCrafting.requiredItems = {};
	--needed items for the crafting process (e.g. Ruebenstecher)
	newCrafting.requiredTools = {};
	--needed skills for the crafting process
	newCrafting.requiredSkills = {};
	--items that result from the crafting process
	newCrafting.resultingItems = {};
	
	--initialize
	newCrafting:loadAnimation();
	newCrafting:loadRequiredItems();
	newCrafting:loadRequiredTools();
	newCrafting:loadRequiredSkills();
	newCrafting:loadResultingItems();
	
	return newCrafting;
end

---Checks whether the crafting can be executed by the player
function Crafting:checkExecConditions(playerid)
	if self:checkConditions(playerid) == false then		
		return false;
	end
	
	SendPlayerMessage(playerid, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, string.format("Du versuchst %s herzustellen / zu tun. Das wird circa %d s dauern.", self.caption, self.duration / 1000));
	
	return true;
end

---Execute the crafting
--ATTENTION: This function does not check the requirements, this has to be done in the 'exec'-function of the parent MenuItem
function Crafting:exec(playerid)

	--remove the source items
	for k, v in pairs(self.requiredItems) do
		RemoveItem(playerid, v[1], tonumber(v[2]))
	end

	--check propability
	--ATTENTION: if 0 or 100 -> ignore.
	if self.propability > 0 and self.propability < 100 then
		local rand = math.random(100);

		if rand > self.propability then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast beim Herstellen / beim  %s einen Fehler gemacht.", self.caption));
			return;
		end
	end

	--add the crafted items
	for k, v in pairs(self.resultingItems) do
		GiveItem(playerid, v[1], tonumber(v[2]))
	end
	
	SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s erfolgreich hergestellt / gemacht.", self.caption));
end

---Checks whether an animation id is affected by this crafting
function Crafting:containsAnimID(animID)
	for k, v in pairs(self.animID) do
		if v == animID then return true; end;
	end
	
	return false;
end

---Checks whether the player fulfills all conditions for this crafting
function Crafting:checkConditions(playerid)
	local inventory = getPlayerInventory(playerid);
	
	if self:checkRequiredItems(inventory) == false then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast nicht die notwendigen Rohstoffe, um %s herzustellen / zu können.", self.caption));
		return false;
	end
	
	if self:checkRequiredTools(inventory) == false then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast nicht das notwendige Werkzeug, um %s herzustellen.", self.caption));
		return false;
	end
	
	if self:checkRequiredSkills(playerid) == false then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast nicht die notwendigen Faehigkeiten, um %s herzustellen / zu tun.", self.caption));
		return false;
	end
	
	return true;
end

---Checks whether the player has enought resource items
function Crafting:checkRequiredItems(inventory)
	for k, v in pairs(self.requiredItems) do
		--player has not enought resources
		if inventory:getItemCount(v[1]) < v[2] then
			return false;
		end
	end
	
	return true;
end

---Checks whether the player has the required tools
function Crafting:checkRequiredTools(inventory)
	for k, v in pairs(self.requiredTools) do
		--player has not enought resources
		if inventory:getItemCount(v) < 1 then
			return false;
		end
	end
	
	return true;
end

---Checks whether the player has the required skills
function Crafting:checkRequiredSkills(playerid)
	local character = getCharacterById(playerid);
	
	for k, v in pairs(self.requiredSkills) do
		--player has not enought skill
		if character:getSkillValue(v.skillID) < v.amount then
			return false;
		end
	end
	
	return true;
end

---Loads the affected animation id's from the DB
function Crafting:loadAnimation()
	local result = runSQL(string.format("SELECT anim_id FROM cr_playeranim WHERE crafting_id = %s;", self.craftID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do				
			table.insert(self.animID, tonumber(row[1]));
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the required items from the DB
function Crafting:loadRequiredItems()
	local result = runSQL(string.format("SELECT item_id, cnt FROM cr_reqitems WHERE crafting_id = %s;", self.craftID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			
			table.insert(self.requiredItems, { instance_InventorySystem.inventoryMan:getItemByID(row[1]).instanceName, tonumber(row[2]) });
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the required tools from the DB
function Crafting:loadRequiredTools()
	local result = runSQL(string.format("SELECT item_id FROM cr_reqtools WHERE crafting_id = %s;", self.craftID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			table.insert(self.requiredTools, instance_InventorySystem.inventoryMan:getItemByID(row[1]).instanceName);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the required skills from the DB
function Crafting:loadRequiredSkills()
	local result = runSQL(string.format("SELECT skill_id, amount FROM cr_reqskills WHERE crafting_id = %s;", self.craftID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local crsk = CraftingSkill.new(tonumber(row[1]), tonumber(row[2]));
			table.insert(self.requiredSkills, crsk);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the resulting items from the DB
function Crafting:loadResultingItems()
	local result = runSQL(string.format("SELECT item_id, cnt FROM cr_resitems WHERE crafting_id = %s;", self.craftID));
	
	if result then
		local row = mysql_fetch_row(result);
	
		while row do
			table.insert(self.resultingItems, { instance_InventorySystem.inventoryMan:getItemByID(row[1]).instanceName, tonumber(row[2]) });
				
			row = mysql_fetch_row(result);
		end
	end
end