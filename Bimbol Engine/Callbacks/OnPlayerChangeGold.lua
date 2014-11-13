--[[
	
	Module: OnPlayerChangeGold.lua
	Autor: Bimbol
	
	OnPlayerChangeGold
	
]]--

function OnPlayerChangeGold(playerid, currGold, oldGold)
	if IsNPC(playerid) == 0 then
		checkPlayerCheatAC(playerid, "gold", currGold, oldGold);
	end
	-- BE Callback --
	BE_OnPlayerChangeGold(playerid, currGold, oldGold);
end

function BE_OnPlayerChangeGold(playerid, currGold, oldGold)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");