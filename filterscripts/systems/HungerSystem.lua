print("Loading HungerSystem.lua");

instance_HungerSystem = nil;

HungerSystem = {};
HungerSystem.__index = HungerSystem;

---Creates an instance of HungerSystem
function HungerSystem.new()
	local newHungerSystem = {};
	
	setmetatable(newHungerSystem, HungerSystem);
	
	return newHungerSystem;
end

---Start the hunger system
function loadHungerSystem()
	SetTimer("updateCharacterHunger", HUNGER_SYSTEM_INTERVAL * 1000, 1);
end

---Updates the players hunger level
function updateCharacterHunger()
	for k, v in pairs(PlayerCharacters) do	
		
		updateHungerMessage(v);
		
		v:applyHunger();
		
		if v.hungerLevel > 0 then
			v.hungerLevel = v.hungerLevel - 1;
		end
	end
end

---Send a message to the player if the player's hunger level [category] changed
function updateHungerMessage(character)
	if character.hungerLevel < HUNGER_BORDER_DECREMENT then
		if character.hungerRegeneration ~= -1 then
			SendPlayerMessage(character.playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du bist extrem hungrig und verlierst nun Lebensenergie. Gehe etwas essen!");
		end
			
		character.hungerRegeneration = -1;
			
	elseif character.hungerLevel < HUNGER_BORDER_INCREMENT1 then
		if character.hungerRegeneration ~= 0 then
			SendPlayerMessage(character.playerid, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, "Du bist hungrig. Gehe etwas essen!");
		end
		
		character.hungerRegeneration = 0;
	
	elseif character.hungerLevel < HUNGER_BORDER_INCREMENT2 then
		if character.hungerRegeneration ~= 1 then
			SendPlayerMessage(character.playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Du bist gut gesaettigt und regenerierst dich langsam.");
		end
		
		character.hungerRegeneration = 1;
	else
		if character.hungerRegeneration ~= 2 then
			SendPlayerMessage(character.playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Du bist sehr gut gesaettigt und regenerierst dich schnell.");
		end
		
		character.hungerRegeneration = 2;
	end
end


function HungerSystem:OnPlayerUseItem(playerid, item_instance, amount, hand)
	local item = instance_InventorySystem.inventoryMan:getItemForInstanceName(item_instance);
	
	if not item then return; end;
	
	--Saettigungswert eines Items
	if item.saturation then
		if item.saturation > 0 then
			local character = getCharacterById(playerid);
		
			local newValue = character.hungerLevel + item.saturation;
			if newValue > 100 then newValue = 100; end;
		
			character.hungerLevel = newValue;
		
			SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du erhaeltst +%d Saettigung.", item.saturation));
		
			--return a message to the player
			updateHungerMessage(character);
		end
	end
end


--instantiate hunger system
if not instance_HungerSystem then
	instance_HungerSystem = HungerSystem.new();
end