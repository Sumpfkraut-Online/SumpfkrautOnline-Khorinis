--[[
	
	Module: OnPlayerSpellCast.lua
	Autor: Bimbol
	
	OnPlayerSpellCast
	
]]--

function OnPlayerSpellCast(playerid, spellInstance)
	if string.find(spellInstance, "ITSC") then
		removeItem(playerid, spellInstance, 1);
	end
	-- BE Callbacks --
	BE_OnPlayerSpellCast(playerid, spellInstance);
end

function BE_OnPlayerSpellCast(playerid, spellInstance)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");