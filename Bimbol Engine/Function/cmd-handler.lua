--[[
	
	Module: cmd-handler.lua
	Autor: Bimbol
	
	CMD System
	
]]--

local cmd = {};

function addCommandHandler(cmdd, func)
	if cmdd and func then
		if type(cmdd) == "table" then
			for _, v in ipairs(cmdd) do
				table.insert(cmd, { name = v, func = func });
			end
		else
			table.insert(cmd, { name = cmdd, func = func });
		end
			return true;
	end
		return false;
end

function removeCommandHandler(cmdd)
	if cmdd then
		for i, k in ipairs(cmd) do
			if cmdd == k.name then
				table.remove(cmd, i);
				return true;
			end
		end
			return false;
	end
		return false;
end

function callCommandHandler(playerid, cmdd, params)
	if playerid and cmdd and params then
		for i, k in ipairs(cmd) do
			if cmdd == k.name then
				k.func(playerid, params);
				return true;
			end
		end
	end
		return false;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");
