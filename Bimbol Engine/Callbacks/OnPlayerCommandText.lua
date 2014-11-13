--[[
	
	Module: OnPlayerCommandText.lua
	Autor: Bimbol
	
	OnPlayerCommandText
	
]]--

function OnPlayerCommandText(playerid, cmdtext)
	local cmd, params = GetCommand(cmdtext);
	callCommandHandler(playerid, cmd, params);
	-- BE Callbacks --
	BE_OnPlayerCommandText(playerid, cmdtext);
end

function BE_OnPlayerCommandText(playerid, cmdtext)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");