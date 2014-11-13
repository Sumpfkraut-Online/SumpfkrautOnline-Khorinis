--[[
	
	Module: OnPlayerHit.lua
	Autor: Bimbol
	
	OnPlayerHit
	
]]--

function OnPlayerHit(playerid, killerid)
	BLOW_AI(playerid, killerid);
	removeBoltandArrow(killerid);
	-- BE Callback --
	BE_OnPlayerHit(playerid, killerid);
end

function BE_OnPlayerHit(playerid, killerid)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");