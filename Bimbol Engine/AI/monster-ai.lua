--[[
	
	Module: monster-add.lua
	Autor: Bimbol
	
	AI System
	
]]--

local Monsters = {};
local Monsters_Stats = {};

function getMonsters()
	return Monsters;
end

function getMonsterStats()
	return Monsters_Stats;
end

local WARN_TIME = 6;

function ai_Init(value)
	if value then
		SetTimer("OnAI", 600, 1);
	else
		print("Error: Missing argument on function: ai_Init");
	end
end

function setMonsterFocus(monsterid, focusid)
	if monsterid and focusid then
		local mob = isMonster(monsterid, true);
		if mob then
			mob.focus = focusid;
				return true;
		end
	else
		print("Error: Missing argument on function: setMonsterFocus");
	end
		return false;
end

function setMonsterAggressive(monsterid, value)
	if monsterid and type(value) == "boolean" then
		local mob = isMonster(monsterid, true);
		if mob then
			mob.aggressive = value;
				return true;
		end
	else
		print("Error: Missing argument on function: setMonsterAggressive");
	end
		return false;
end

function setMonsterRespawn(monsterid, respawn)
	if monsterid and type(respawn) == "number" then
		local mob = isMonster(monsterid, true);
		if mob then
			mob.death = respawn;
				return true;
		end
	else
		print("Error: Missing argument on function: setMonsterRespawn");
	end
		return false;
end

local function ATTACK_AI(mob, ai, id)
	mob.attack = true;
	if ai == "ANIMAL" then
		PlayAnimation(id, "S_FISTATTACK");
	elseif ai == "1H" then
		if mob.run then
			PlayAnimation(id, "T_1HATTACKMOVE");
			mob.run = false;
		else
			local direct = math.random(3);
			if direct == 1 then
				PlayAnimation(id, "S_1HATTACK");
			elseif direct == 2 then
				PlayAnimation(id, "T_1HATTACKR");
			elseif direct == 3 then
				PlayAnimation(id, "T_1HATTACKL");
			end
		end
	elseif ai == "2H" then
		if mob.run then
			PlayAnimation(id, "T_2HATTACKMOVE");
			mob.run = false;
		else
			local direct = math.random(3);
			if direct == 1 then
				PlayAnimation(id, "S_2HATTACK");
			elseif direct == 2 then
				PlayAnimation(id, "T_2HATTACKR");
			elseif direct == 3 then
				PlayAnimation(id, "T_2HATTACKL");
			end
		end
	end
	HitPlayer(id, mob.focus);
end

function BLOW_AI(id, killerid)
	local mob = isMonster(id, true);
	if mob then
		if not(mob.focus) then
			mob.focus = killerid;
			OnMonsterTakeFocus(id, killerid);
			if Monsters_Stats[mob.special_instance].ai == "ANIMAL" then
				mob.warn = true;
			end
			PlayAnimation(id, "T_FISTPARADEJUMPB");
			local distance = GetDistancePlayers(mob.focus, id);
			if distance > Monsters_Stats[mob.special_instance].max_dist then
				Monsters_Stats[mob.special_instance].bonus_dist = distance - Monsters_Stats[mob.special_instance].max_dist + 300;
			end
		elseif mob.blow_time then
			if killerid ~= mob.focus and math.random(2) == 1 then
				mob.focus = killerid;
				OnMonsterTakeFocus(id, killerid);
			end
			if not(mob.blow) then
				if math.random(2) == 1 then
					if mob.blow_time > 0 then
						mob.blow = math.random(1, mob.blow_time);
					else
						mob.blow = true;
					end
					if Monsters_Stats[mob.special_instance].ai == "1H" or Monsters_Stats[mob.special_instance].ai == "2H" then
						if math.random(2) == 1 then
							PlayAnimation(id, "T_" .. Monsters_Stats[mob.special_instance].ai .. "PARADEJUMPB");
						else
							PlayAnimation(id, "T_" .. Monsters_Stats[mob.special_instance].ai .. "PARADE_0");
						end
					else
						PlayAnimation(id, "T_FISTPARADEJUMPB");
					end
				end
			else
				mob.blow = false;
			end
		end
	end
end

function DEATH_AI(id, playerid)
	local mob = isMonster(id, true);
	if mob then
		if playerid ~= -1 then
			if Monsters_Stats[mob.special_instance].ai == "1H" or Monsters_Stats[mob.special_instance].ai == "2H" then
				SetPlayerWeaponMode(id, WEAPON_NONE);
			end
			OnMonsterLostFocus(id, mob.focus or -1);
			OnMonsterDeath(id, 
						   playerid, 
						   Monsters_Stats[mob.special_instance].instance, 
						   mob.special_instance, 
						   Monsters_Stats[mob.special_instance].exp, 
						   Monsters_Stats[mob.special_instance].ai);
			mob.death = Monsters_Stats[mob.special_instance].respawn;
			mob.focus = false;
			mob.warn = false;
			mob.run = false;
			mob.attack = false;
			PlayAnimation(id, "S_FISTRUN");
		end
	end
end

function SPAWN_AI(id)
	local mob = isMonster(id, true);
	if mob then
		if mob.death then
			SetPlayerPos(id, 5000, -6000, 5000);
		end
	end
end

local OnAnimalAI;
local On1HAI;
local On2HAI;

function OnAI()
	for id = MAX_SLOTS - 1, MAX_SLOTS - MONSTER_AMOUNT - NPC_AMOUNT, -1 do
		if isMonster(id) then
			if Monsters_Stats[Monsters[id].special_instance].ai == "ANIMAL" then
				OnAnimalAI(Monsters[id], id);
			elseif Monsters_Stats[Monsters[id].special_instance].ai == "1H" then
				On1HAI(Monsters[id], id);
			elseif Monsters_Stats[Monsters[id].special_instance].ai == "2H" then
				On2HAI(Monsters[id], id);
			end
		end
	end
end

function OnAnimalAI(bot, id)
	if bot.death then
		if bot.death > 0 then
			bot.death = bot.death - 1;
		else
			SetPlayerPos(id, bot.x + changeValueRandom(math.random(bot.rage)), bot.y, bot.z + changeValueRandom(math.random(bot.rage)));
			bot.death = false;
		end
	else
		if not(bot.focus) and bot.aggressive then
			if bot.eat > 0 then
				bot.eat = bot.eat - 1;
				if bot.eat == 0 then PlayAnimation(id, "S_FISTRUN"); end
			end
			for _, player in pairs(GetOnlinePlayers()) do
				if IsDead(player) == 0 and IsDead(id) == 0 
				and GetDistancePlayers(player, id) <= Monsters_Stats[bot.special_instance].max_dist
				then
					bot.focus = player;
					OnMonsterTakeFocus(id, player);
				end
			end
		elseif bot.focus then
			local foucus_dead = IsDead(bot.focus);
			if IsPlayerConnected(bot.focus) == 1 and foucus_dead == 0 and IsDead(id) == 0 
			and GetDistancePlayers(bot.focus, id) <= Monsters_Stats[bot.special_instance].max_dist + Monsters_Stats[bot.special_instance].bonus_dist
			then
				local angle = GetAngleToPlayer(id, bot.focus);
				if angle ~= GetPlayerAngle(id) then
					SetPlayerAngle(id, angle);
				end
				if not(bot.warn) then
					PlayAnimation(id, "T_WARN");
					bot.warn = 1;
				else
					if not(bot.warn == true) then
						if bot.warn <= WARN_TIME then
							bot.warn = bot.warn + 1;
							if GetDistancePlayers(bot.focus, id) <= Monsters_Stats[bot.special_instance].min_dist + 400 then
								bot.warn = true;
								bot.run = true;
								PlayAnimation(id, "S_FISTRUNL");
							end
						else
							bot.warn = true;
							bot.run = true;
							PlayAnimation(id, "S_FISTRUNL");
						end
					else
						if bot.blow and type(bot.blow) == "number" then
							if bot.blow <= bot.blow_time then
								bot.blow = bot.blow + 1;
								local direct = math.random(2);
								if direct == 1 then
									PlayAnimation(id, "T_FISTRUNSTRAFER");
								elseif direct == 2 then
									PlayAnimation(id, "T_FISTRUNSTRAFEL");
								end
							else
								bot.blow = false;
								PlayAnimation(id, "S_FISTRUNL");
							end
						elseif bot.blow then
							bot.blow = false;
							PlayAnimation(id, "S_FISTRUNL");
						end
						if GetDistancePlayers(bot.focus, id) <= Monsters_Stats[bot.special_instance].min_dist then
							if not(bot.blow) then
								if not(bot.attacbot) then
									if bot.run then
										bot.run = false;
										PlayAnimation(id, "S_FISTRUN");
									end
									ATTACK_AI(bot, Monsters_Stats[bot.special_instance].ai, id);
								else
									bot.attacbot = false;
								end
							end
						else
							if not(bot.run) then
								bot.run = true;
								PlayAnimation(id, "S_FISTRUNL");
							end
						end
					end
				end		
			else
				Monsters_Stats[bot.special_instance].bonus_dist = 0;
				OnMonsterLostFocus(id, bot.focus);
				if foucus_dead == 1 then
					PlayAnimation(id, "S_EAT");
					bot.eat = 9;
				else
					PlayAnimation(id, "S_FISTRUN");
				end
				bot.focus = false;
				bot.warn = false;
				bot.run = false;
				bot.attacbot = false;
				bot.blow = false;
			end
		end
	end
end

function On1HAI(bot, id)
	if bot.death then
		if bot.death > 0 then
			bot.death = bot.death - 1;
		else
			if Monsters_Stats[bot.special_instance].instance == "PC_HERO" then
				PlayAnimation(id, "S_RUN");
			end
			SetPlayerPos(id, bot.x + changeValueRandom(math.random(bot.rage)), bot.y, bot.z + changeValueRandom(math.random(bot.rage)));
			bot.death = false;
		end
	else
		if not(bot.focus) and bot.aggressive then
			for _, player in pairs(GetOnlinePlayers()) do
				if IsDead(player) == 0 and IsDead(id) == 0
				and GetDistancePlayers(player, id) <= Monsters_Stats[bot.special_instance].max_dist
				then
					bot.focus = player;
					OnMonsterTakeFocus(id, player);
				end
			end
		elseif bot.focus then
			local uncusin_bot = IsUnconscious(id);
			local uncusin_player = IsUnconscious(bot.focus);
			if IsPlayerConnected(bot.focus) == 1 and IsDead(bot.focus) == 0 and IsDead(id) == 0 and uncusin_player == 0 and uncusin_bot == 0
			and GetDistancePlayers(bot.focus, id) <= Monsters_Stats[bot.special_instance].max_dist + Monsters_Stats[bot.special_instance].bonus_dist
			then
				local angle = GetAngleToPlayer(id, bot.focus);
				if angle ~= GetPlayerAngle(id) then
					SetPlayerAngle(id, angle);
				end
				if not(bot.warn) then
					SetPlayerWeaponMode(id, WEAPON_1H);
					PlayAnimation(id, "T_1H_2_1HRUN");
					bot.warn = 1;
				else
					if not(bot.warn == true) then
						bot.warn = true;
						bot.run = true;
						PlayAnimation(id, "S_1HRUNL");
					else
						if bot.blow and type(bot.blow) == "number" then
							if bot.blow <= bot.blow_time then
								bot.blow = bot.blow + 1;
								local direct = math.random(2);
								if direct == 1 then
									PlayAnimation(id, "T_FISTRUNSTRAFER");
								elseif direct == 2 then
									PlayAnimation(id, "T_FISTRUNSTRAFEL");
								end
							else
								bot.blow = false;
								PlayAnimation(id, "S_1HRUNL");
							end
						elseif bot.blow then
							bot.blow = false;
							PlayAnimation(id, "S_1HRUNL");
						end
						if GetDistancePlayers(bot.focus, id) <= Monsters_Stats[bot.special_instance].min_dist then
							if not(bot.blow) then
								if not(bot.attack) then
									ATTACK_AI(bot, Monsters_Stats[bot.special_instance].ai, id);
								else
									bot.attack = false;
								end
							end
						else
							if not(bot.run) then
								bot.run = true;
								PlayAnimation(id, "S_1HRUNL");
							end
						end
					end
				end		
			else
				if Monsters_Stats[bot.special_instance].instance == "PC_HERO" then
					if uncusin_bot == 0 and uncusin_player == 1 then
						SetPlayerHealth(bot.focus, 0);
					end
				end
				PlayAnimation(id, "S_RUN");
				OnMonsterLostFocus(id, bot.focus);
				bot.focus = false;
				bot.warn = false;
				bot.run = false;
				bot.attack = false;
				bot.blow = false;
				SetPlayerWeaponMode(id, WEAPON_NONE);
			end
		end
	end
end

function On2HAI(bot, id)
	if bot.death then
		if bot.death > 0 then
			bot.death = bot.death - 1;
		else
			if Monsters_Stats[bot.special_instance].instance == "PC_HERO" then
				PlayAnimation(id, "S_RUN");
			end
			SetPlayerPos(id, bot.x + changeValueRandom(math.random(bot.rage)), bot.y, bot.z + changeValueRandom(math.random(bot.rage)));
			bot.death = false;
		end
	else
		if not(bot.focus) and bot.aggressive then
			if bot.eat > 0 then
				bot.eat = bot.eat - 1;
				if bot.eat == 0 then PlayAnimation(id, "S_RUN"); end
			end
			for _, player in pairs(GetOnlinePlayers()) do
				if IsDead(player) == 0 and IsDead(id) == 0
				and GetDistancePlayers(player, id) <= Monsters_Stats[bot.special_instance].max_dist
				then
					bot.focus = player;
					OnMonsterTakeFocus(id, player);
				end
			end
		elseif bot.focus then
			local uncusin_bot = IsUnconscious(id);
			local uncusin_player = IsUnconscious(bot.focus);
			local foucus_dead = IsDead(bot.focus);
			if IsPlayerConnected(bot.focus) == 1 and foucus_dead == 0 and IsDead(id) == 0 and uncusin_player == 0 and uncusin_bot == 0 
			and GetDistancePlayers(bot.focus, id) <= Monsters_Stats[bot.special_instance].max_dist + Monsters_Stats[bot.special_instance].bonus_dist
			then
				local angle = GetAngleToPlayer(id, bot.focus);
				if angle ~= GetPlayerAngle(id) then
					SetPlayerAngle(id, angle);
				end
				if not(bot.warn) then
					SetPlayerWeaponMode(id, WEAPON_2H);
					PlayAnimation(id, "T_2H_2_1HRUN");
					bot.warn = 1;
				else
					if not(bot.warn == true) then
						bot.warn = true;
						bot.run = true;
						PlayAnimation(id, "S_2HRUNL");
					else
						if bot.blow and type(bot.blow) == "number" then
							if bot.blow <= bot.blow_time then
								bot.blow = bot.blow + 1;
								local direct = math.random(2);
								if direct == 1 then
									PlayAnimation(id, "T_FISTRUNSTRAFER");
								elseif direct == 2 then
									PlayAnimation(id, "T_FISTRUNSTRAFEL");
								end
							else
								bot.blow = false;
								PlayAnimation(id, "S_2HRUNL");
							end
						elseif bot.blow then
							bot.blow = false;
							PlayAnimation(id, "S_2HRUNL");
						end
						if GetDistancePlayers(bot.focus, id) <= Monsters_Stats[bot.special_instance].min_dist then
							if not(bot.blow) then
								if not(bot.attack) then
									ATTACK_AI(bot, Monsters_Stats[bot.special_instance].ai, id);
								else
									bot.attack = false;
								end
							end
						else
							if not(bot.run) then
								bot.run = true;
								PlayAnimation(id, "S_2HRUNL");
							end
						end
					end
				end		
			else
				if Monsters_Stats[bot.special_instance].instance == "PC_HERO" then
					if uncusin_bot == 0 and uncusin_player == 1 then
						SetPlayerHealth(bot.focus, 0);
					end
				end
				if foucus_dead == 1 then
					PlayAnimation(id, "T_WARN");
					bot.eat = 2;
				else
					PlayAnimation(id, "S_RUN");
				end
				OnMonsterLostFocus(id, bot.focus);
				bot.focus = false;
				bot.warn = false;
				bot.run = false;
				bot.attack = false;
				bot.blow = false;
				SetPlayerWeaponMode(id, WEAPON_NONE);
			end
		end
	end
end

--Callbacks
function OnMonsterDeath(monsterid, playerid, instance, id, exp, ai)
end

function OnMonsterTakeFocus(monsterid, focusid)
end

function OnMonsterLostFocus(monsterid, focusid)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");