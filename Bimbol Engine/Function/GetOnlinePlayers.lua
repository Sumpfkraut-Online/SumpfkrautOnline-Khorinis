--[[
	
	Module: GetOnlinePlayers.lua
	Autor: Bimbol
	
	Get all online players on the server.
	
]]--

local p = {};

function addToOnline(playerid)
	if IsNPC(playerid) == 0 then
		table.insert(p, playerid);
	end
end

function removeFromOnline(playerid)
	if IsNPC(playerid) == 0 then
		for i in pairs(p) do
			if p[i] == playerid then
				table.remove(p, i);
				return;
			end
		end
	end
end

function GetOnlinePlayers()
	return p;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");