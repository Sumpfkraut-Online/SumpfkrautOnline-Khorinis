--[[
	
	Module: monster-add.lua
	Autor: Bimbol
	
	Monster System
	
]]--

local BLOW_TIME = 2;

function addMonster(special_instance, monster)
	if special_instance and type(monster) == "table" then
		if monster.blow_time ~= false and not monster.blow_time then
			monster.blow_time = BLOW_TIME;
		end
	
		if not monster.respawn then
			monster.respawn = 10;
		end
		
		getMonsterStats()[special_instance] = {
			ai = monster.ai or "ANIMAL", 
			name = monster.name or "Default", 
			instance = monster.instance or "PC_HERO", 
			str = monster.str or 0, 
			dex = monster.dex or 0, 
			lvl = monster.lvl or 0, 
			hp = monster.hp or 0, 
			mp = monster.mp or 0, 
			exp = monster.exp or 0, 
			skill_1h = monster.skill_1h or 0, 
			skill_2h = monster.skill_2h or 0, 
			skill_bow = monster.skill_bow or 0, 
			skill_cbow = monster.skill_cbow or 0, 
			min_dist = monster.min_dist or 200, 
			max_dist = monster.max_dist or 1000,
			bonus_dist = 0,
			respawn = monster.respawn * 2, 
			armor = monster.armor, 
			weapon = monster.weapon, 
			rangedweapon = monster.rangedweapon, 
			scale_x = monster.scale_x,
			scale_y = monster.scale_y, 
			scale_z = monster.scale_z,
			blow_time = monster.blow_time,
		};
	else
		print("Error: Missing argument on function: addMonster");
	end
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");