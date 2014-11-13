--[[
	
	Module: npc.lua
	Autor: Bimbol
	
	NPC System
	
]]--

local NPC = {};

NPC_AMOUNT = 0;

function createNPC(npc_name, npc_body, npc_bodytex, npc_head, npc_headtex, anim, instance, x, y, z, angle, world, meleeweapon, rangedweapon, armor, callback)
	if npc_name and npc_body and npc_bodytex and npc_head and npc_headtex and anim and x and y and z and angle and world then
		local bot_id = CreateNPC(npc_name);
		if bot_id == -1 then return false end
		
		NPC[bot_id] = { name = npc_name, anim = anim };
		SpawnPlayer(bot_id);
		SetPlayerWorld(bot_id, world, "START");
		SetPlayerInstance(bot_id, instance);
		SetPlayerStrength(bot_id, 1000);
		SetPlayerDexterity(bot_id, 1000);
		SetPlayerLevel(bot_id, 100);
		SetPlayerHealth(bot_id, 999999);
		SetPlayerMaxHealth(bot_id, 999999);
		SetPlayerMana(bot_id, 0);
		SetPlayerMaxMana(bot_id, 0);
		SetPlayerPos(bot_id, x, y, z);
		SetPlayerAngle(bot_id, angle);
		SetPlayerAdditionalVisual(bot_id, npc_body, npc_bodytex, npc_head, npc_headtex);
		if anim then
			PlayAnimation(bot_id, anim);
		end
		if meleeweapon then
			EquipMeleeWeapon(bot_id, meleeweapon);
		end
		if rangedweapon then
			EquipRangedWeapon(bot_id, rangedweapon);
		end
		if armor then
			EquipArmor(bot_id, armor);
		end
		-- callback
		if callback then callback(bot_id); end
		NPC_AMOUNT = NPC_AMOUNT + 1;
			return bot_id;
	else
		print("Error: Missing argument on function: createNPC");
	end
		return false;
end

function destroyNPC(npc_id)
	if npc_id then
		if NPC[npc_id] then
			DestroyNPC(npc_id);
			NPC[npc_id] = nil;
			return true;
		end
	else
		print("Error: Missing argument on function: destroyNPC");
	end
		return false;
end

function isNPC(npc_id)
	if npc_id then
		if NPC[npc_id] then
			return true;
		end
	else
		print("Error: Missing argument on function: isNPC");
	end
		return false;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");