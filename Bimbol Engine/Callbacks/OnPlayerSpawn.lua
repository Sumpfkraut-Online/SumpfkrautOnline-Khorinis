--[[
	
	Module: OnPlayerHit.lua
	Autor: Bimbol
	
	OnPlayerHit
	
]]--

function OnPlayerSpawn(playerid, classid)
	SPAWN_AI(playerid);
	if IsNPC(playerid) == 0 then
		givePlayerAllItems(playerid);
	end
	-- BE_Callback --
	BE_OnPlayerSpawn(playerid, classid);
end

function BE_OnPlayerSpawn(playerid, classid)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");