--[[
	
	Module: OnPlayerDropItem.lua
	Autor: Bimbol
	
	OnPlayerDropItem
	
]]--

function OnPlayerDropItem(playerid, itemid, item_instance, amount, x, y, z, worldName)
	removeItem(playerid, item_instance, amount);
	-- BE Callback --
	BE_OnPlayerDropItem(playerid, itemid, item_instance, amount, x, y, z, worldName);
end

function BE_OnPlayerDropItem(playerid, itemid, item_instance, amount, x, y, z, worldName)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");