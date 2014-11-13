--[[
	
	Module: OnPlayerChangeStrength.lua
	Autor: Bimbol
	
	OnPlayerChangeStrength
	
]]--

function OnPlayerChangeStrength(playerid, currStrength, oldStrength)
	if IsNPC(playerid) == 0 then
		checkPlayerCheatAC(playerid, "strength", currStrength, oldStrength);
	end
	-- BE Callback --
	BE_OnPlayerChangeStrength(playerid, currStrength, oldStrength);
end

function BE_OnPlayerChangeStrength(playerid, currStrength, oldStrength)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");