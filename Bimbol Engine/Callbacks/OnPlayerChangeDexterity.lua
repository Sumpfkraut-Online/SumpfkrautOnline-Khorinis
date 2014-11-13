--[[
	
	Module: OnPlayerChangeDexterity.lua
	Autor: Bimbol
	
	OnPlayerChangeDexterity
	
]]--

function OnPlayerChangeDexterity(playerid, currDexterity, oldDexterity)
	if IsNPC(playerid) == 0 then
		checkPlayerCheatAC(playerid, "dexterity", currDexterity, oldDexterity);
	end
	-- BE Callback --
	BE_OnPlayerChangeDexterity(playerid, currDexterity, oldDexterity);
end

function BE_OnPlayerChangeDexterity(playerid, currDexterity, oldDexterity)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");