--[[
	
	Module: monster-spawn.lua
	Autor: Bimbol
	
	Spawn System
	
]]--

MONSTER_AMOUNT = 0;

function spawnMonster(special_instance, x, y, z, amount, world, distance, aggressive)
	if special_instance and x and y and z and amount and world and distance and type(aggressive) == "boolean" then
		local k = getMonsterStats()[special_instance];
		if k then
			local mob_id = {};
			for i = 1, amount do
				local bot_id = CreateNPC(k.name);
				SpawnPlayer(bot_id);
				SetPlayerWorld(bot_id, world, "START");
				SetPlayerInstance(bot_id, k.instance);
				SetPlayerStrength(bot_id, k.str);
				SetPlayerDexterity(bot_id, k.dex);
				SetPlayerLevel(bot_id, k.lvl);
				SetPlayerHealth(bot_id, k.hp);
				SetPlayerMaxHealth(bot_id, k.hp);
				SetPlayerMana(bot_id, k.mp);
				SetPlayerMaxMana(bot_id, k.mp);
				local x_r, z_r = changeValueRandom(random(distance)), changeValueRandom(random(distance));
				SetPlayerPos(bot_id, x + x_r, y, z + z_r);
				SetPlayerAngle(bot_id, random(360));
				SetPlayerSkillWeapon(bot_id, 0, k.skill_1h);
				SetPlayerSkillWeapon(bot_id, 1, k.skill_2h);
				SetPlayerSkillWeapon(bot_id, 2, k.skill_bow);
				SetPlayerSkillWeapon(bot_id, 3, k.skill_cbow);
				if k.scale_x and k.scale_y and k.scale_z then
					SetPlayerScale(bot_id, k.scale_x, k.scale_y, k.scale_z);
				end
				if k.armor then
					EquipArmor(bot_id, k.armor);
				end
				if k.weapon then
					EquipMeleeWeapon(bot_id, k.weapon);
				end
				if k.rangedweapon then
					EquipRangedWeapon(bot_id, k.rangedweapon);
				end
				getMonsters()[bot_id] = { 
					special_instance = special_instance,
					blow_time = k.blow_time,
					focus = false,
					death = false,
					id = bot_id,
					world = world,
					x = x,
					y = y,
					z = z,
					rage = distance,
					aggressive = aggressive,
					warn = false,
					run = false,
					attack = false,
					blow = false,
					eat = 0,
				};
				table.insert(mob_id, bot_id);
				MONSTER_AMOUNT = MONSTER_AMOUNT + 1;
			end
				return mob_id;
		else
			print("Error: Special instance '"..special_instance.."' didn't exist"); 
		end
	else
		print("Error: Missing argument on function: spawnMonster");
	end
		return false;
end

function destroyMonster(id)
	if id then
		local m = getMonsters();
		if m[id] ~= nil then
			DestroyNPC(id);
			table.remove(m, id);
			return true;
		end
	else
		print("Error: Missing argument on function: destroyMonster");
	end
		return false;
end

function isMonster(id, ai)
	if id then
		local m = getMonsters();
		if m[id] ~= nil then
			if ai then
				return m[id];
			else
				return true;
			end
		end
	else
		print("Error: Missing argument on function: isMonster");
	end
		return false;
end

function changeValueRandom(value)
	if random(2) == 0 then
		return -value
	end
		return value;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");