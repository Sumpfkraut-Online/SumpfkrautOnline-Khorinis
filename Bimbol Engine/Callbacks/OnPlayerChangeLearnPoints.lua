--[[
	
	Module: OnPlayerChangeLearnPoints.lua
	Autor: Bimbol
	
	OnPlayerChangeLearnPoints
	
]]--

function OnPlayerChangeLearnPoints(playerid, currLP, oldLP)
	if IsNPC(playerid) == 0 then
		checkPlayerCheatAC(playerid, "learn_point", currLP, oldLP);
	end
	-- BE Callback --
	BE_OnPlayerChangeLearnPoints(playerid, currLP, oldLP);
end

function BE_OnPlayerChangeLearnPoints(playerid, currLP, oldLP)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");