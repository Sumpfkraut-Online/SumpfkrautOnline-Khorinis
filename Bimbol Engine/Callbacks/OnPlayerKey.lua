--[[
	
	Module: OnPlayerKey.lua
	Autor: Bimbol
	
	OnPlayerKey
	
]]--

function OnPlayerKey(playerid, keyDown, keyUp)
	callBindKeyUp(playerid, keyUp);
	callBindKeyDown(playerid, keyDown);
	callBindKeyGlobUp(playerid, keyUp);
	callBindKeyGlobDown(playerid, keyDown);
	-- BE_Callback --
	BE_OnPlayerKey(playerid, keyDown, keyUp);
end

function BE_OnPlayerKey(playerid, keyDown, keyUp)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");