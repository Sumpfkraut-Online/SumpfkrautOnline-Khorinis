--[[
	
	Module: OnPlayerDeath.lua
	Autor: Bimbol
	
	OnPlayerDeath
	
]]--

function OnPlayerDeath(playerid, p_classid, killerid, k_classid)
	if IsNPC(playerid) == 1 then
		DEATH_AI(playerid, killerid);
	end
	checkPlayerAllValueAC(playerid);
	-- BE Callback --
	BE_OnPlayerDeath(playerid, p_classid, killerid, k_classid);
end

function BE_OnPlayerDeath(playerid, p_classid, killerid, k_classid)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");