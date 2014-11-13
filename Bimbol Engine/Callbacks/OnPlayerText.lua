--[[

	Module: OnPlayerText.lua
	Author: Bimbol
	
	OnPlayerText

]]--

function OnPlayerText(playerid, text)
	updateGUIInput(playerid, text);
	-- BE_Callback --
	BE_OnPlayerText(playerid, text);
end

function BE_OnPlayerText(playerid, text)
end

-- Loaded
print(debug.getinfo(1).source .. " has been loaded.")

