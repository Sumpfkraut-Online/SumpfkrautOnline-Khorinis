--[[
	
	Module: GetPlayeridByName.lua
	Autor: Bimbol
	
	Get Player id by name
	
]]--

function GetPlayeridByName(name)
	if name then
		for i = 0, MAX_SLOTS - 1 do
			if IsPlayerConnected(i) == 1 then
				if GetPlayerName(i) == name then
					return i;
				end
			end
		end
	end
		return -1;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");