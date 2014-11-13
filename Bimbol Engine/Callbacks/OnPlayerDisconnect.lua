--[[
	
	Module: OnPlayerDisconnect.lua
	Autor: Bimbol
	
	OnPlayerDisconnect
	
]]--

function OnPlayerDisconnect(playerid, reason)
	removeFromOnline(playerid);
	-- BE Callback --
	BE_OnPlayerDisconnect(playerid, reason);
	-----------------
	restartPlayerArea(playerid);
	restart_Player(playerid, true);
	RestartConnectControl(playerid);
	if IsNPC(playerid) == 0 then
		restartGroup(playerid);
		restartGUI(playerid);
		restartAntyCheat(playerid);
		restartKey(playerid);
		restartFocus(playerid);
		restartCamera(playerid);
	end
end

function BE_OnPlayerDisconnect(playerid, reason)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");