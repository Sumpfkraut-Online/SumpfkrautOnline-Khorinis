--[[
	
	Module: OnPlayerWeaponMode.lua
	Autor: Bimbol
	
	OnPlayerWeaponMode
	
]]--

function OnPlayerWeaponMode(playerid, weaponmode)
	setPlayerWeaponMode(playerid, weaponmode);
	-- BE Callbacks --
	BE_OnPlayerWeaponMode(playerid, weaponmode);
end

function BE_OnPlayerWeaponMode(playerid, weaponmode)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");