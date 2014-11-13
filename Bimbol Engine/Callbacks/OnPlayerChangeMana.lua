--[[
	
	Module: OnPlayerChangeMana.lua
	Autor: Bimbol
	
	OnPlayerChangeMana
	
]]--

function OnPlayerChangeMana(playerid, currMana, oldMana)
	if IsNPC(playerid) == 0 then
		checkValueAC(playerid, "mana", currMana, oldMana);
	end
	-- BE Callback --
	BE_OnPlayerChangeMana(playerid, currMana, oldMana);
end

function BE_OnPlayerChangeMana(playerid, currMana, oldMana)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");