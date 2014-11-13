--[[
	
	Module: OnPlayerTakeItem.lua
	Autor: Bimbol
	
	OnPlayerTakeItem
	
]]--

function OnPlayerTakeItem(playerid, itemID, itemInstance, amount, x, y, z, worldName)
	if itemID >= 0 then
		giveItem(playerid, itemInstance, amount);
	end
	-- BE_Callback --
	BE_OnPlayerTakeItem(playerid, itemID, itemInstance, amount, x, y, z, worldName);
end

function BE_OnPlayerTakeItem(playerid, itemID, itemInstance, amount, x, y, z, worldName)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");