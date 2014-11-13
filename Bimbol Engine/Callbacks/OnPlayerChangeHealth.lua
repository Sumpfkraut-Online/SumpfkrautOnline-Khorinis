--[[
	
	Module: OnPlayerChangeHealth.lua
	Autor: Bimbol
	
	OnPlayerChangeHealth
	
]]--

function OnPlayerChangeHealth(playerid, currHealth, oldHealth)
	if IsNPC(playerid) == 0 then
		checkValueAC(playerid, "health", currHealth, oldHealth);
	end
	-- BE Callback --
	BE_OnPlayerChangeHealth(playerid, currHealth, oldHealth);
end

function BE_OnPlayerChangeHealth(playerid, currHealth, oldHealth)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");