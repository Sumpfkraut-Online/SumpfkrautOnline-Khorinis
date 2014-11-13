--[[
	
	Module: OnPlayerConnect.lua
	Autor: Bimbol
	
	OnPlayerConnect
	
]]--

function OnPlayerConnect(playerid)
	group_Connect(playerid);
	addToOnline(playerid);
	-- BE Callback --
	BE_OnPlayerConnect(playerid);
end

function BE_OnPlayerConnect(playerid)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");