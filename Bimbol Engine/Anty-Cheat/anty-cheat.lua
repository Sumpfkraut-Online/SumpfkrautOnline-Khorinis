--[[
	
	Module: anty-cheat.lua
	Autor: Bimbol
	
	Anty Cheat
	
]]--

local Player = {};
local ENABLE_ANTYCHEAT = false;

local AntyCheat = 
{
	{ block = "strength", func = GetPlayerStrength, func_check = _GetPlayerStrength },
	{ block = "dexterity", func = GetPlayerDexterity, func_check = _GetPlayerDexterity },
	{ block = "health", func = GetPlayerMaxHealth, func_check = _GetPlayerMaxHealth },
	{ block = "mana", func = GetPlayerMaxMana, func_check = _GetPlayerMaxMana },
	{ block = "gold", func = GetPlayerGold, func_check = _GetPlayerGold },
	{ block = "learn_point", func = GetPlayerLearnPoints, func_check = _GetPlayerLearnPoints },
};

function enableAntyCheat(value)
	if value == true then
		ENABLE_ANTYCHEAT = true;
		for i = 0, MAX_PLAYERS - 1 do
			Player[i] = {};
			Player[i].CheatControl = {};
		end
	end
end

function restartAntyCheat(playerid)
	if ENABLE_ANTYCHEAT then
		for i, k in ipairs(AntyCheat) do
			Player[playerid].CheatControl[k.block] = nil;
		end
	end
end

function checkValueAC(playerid, block, valuecurr, valueold, arg1, arg2, arg3, arg4)
	if ENABLE_ANTYCHEAT then
		for i, k in ipairs(AntyCheat) do
			if k.block == block then
				if valuecurr <= k.func(playerid, arg1, arg2, arg3, arg4) or valuecurr == 0 and valueold == 0 then
					return;
				end
				OnPlayerUseCheats(playerid, block, valuecurr, valueold);
			end
		end
	end
end

function checkPlayerCheatAC(playerid, block, valuecurr, valueold, arg1, arg2, arg3, arg4)
	if ENABLE_ANTYCHEAT then
		if valuecurr <= valueold or valuecurr <= 10 or Player[playerid].CheatControl[block] or Player[playerid].CheatControl[block] == nil then
			Player[playerid].CheatControl[block] = false;
			checkValueAC(playerid, block, valuecurr, valueold, arg1, arg2, arg3, arg4);
		else
			OnPlayerUseCheats(playerid, block, valuecurr, valueold);
		end
	end
end

function checkPlayerAllValueAC(playerid)
	if ENABLE_ANTYCHEAT then
		if IsNPC(playerid) == 0 then
			for i, k in ipairs(AntyCheat) do
				local func, func2 = k.func(playerid), k.func_check(playerid);
				if func2 > func then
					OnPlayerUseCheats(playerid, k.block, func2, func);
				end
			end
		end
	end
end

-- Set
function SetCheatControlAC(playerid, block)
	if ENABLE_ANTYCHEAT then
		if IsNPC(playerid) == 0 then
			Player[playerid].CheatControl[block] = true;
		end
	end
end

-- Callbacks
function OnPlayerUseCheats(playerid, block, valuecurr, valueold)
end


-- Loaded
print(debug.getinfo(1).source.." has been loaded.");
