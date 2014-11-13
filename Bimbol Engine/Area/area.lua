--[[
	
	Module: area.lua
	Autor: Bimbol
	
	Area System
	
]]--

local Area = {};
local Player = {};
local CALLBACK_AREA = false;

local autoCheckArea;

function enableAreaAutoCheck(value, _time)
	if value == true then
		for i = 0, MAX_PLAYERS - 1 do
			Player[i] = {};
			Player[i].places = {};
		end
		CALLBACK_AREA = true;
		timerAdd("BE_AREA", autoCheckArea, _time, true, true);
	end
end

function restartPlayerArea(playerid)
	if CALLBACK_AREA then
		for i in pairs(Area) do
			Player[playerid].places[i] = nil;
		end
	end
end

function autoCheckArea(playerid)
	local x, y, z = GetPlayerPos(playerid);
	for j, k in ipairs(Area) do
		if checkAreaPoint(k.id, x, z) then
			if k.id ~= Player[playerid].places[j] then
				OnPlayerEnterArea(playerid, k.id);
				Player[playerid].places[j] = k.id;
			end
		else
			if k.id == Player[playerid].places[j] then
				OnPlayerExitArea(playerid, k.id);
				Player[playerid].places[j] = nil;
			end
		end
	end
end

function addArea(id, x, y)
	if id and x and y then
		table.insert(Area, { id = id, polyX = { x }, polyY = { y } });
		return true;
	else
		print("Error: Missing argument on function: addArea");
	end
		return false;
end

function removeArea(id)
	if id then
		for i, k in ipairs(Area) do
			if k.id == id then
				table.remove(Area, i);
			end
		end
			return false;
	else
		print("Error: Missing argument on function: removeArea");
	end
		return false;
end

function addAreaPoint(id, x, y)
	if id and x and y then
		for i, k in ipairs(Area) do
			if k.id == id then
				table.insert(k.polyX, x);
				table.insert(k.polyY, y);
				return true;
			end
		end
			return false;
	else
		print("Error: Missing argument on function: addAreaPoint");
	end
		return false;
end

function checkAreaPoint(id, x, y)
	if id and x and y then
		for v, k in ipairs(Area) do
			if k.id == id then
				local i;
				local j = #k.polyX;
				local oddNodes = false;
				for i = 1, #k.polyX do
					if ((k.polyY[i] < y and k.polyY[j] >= y
					or   k.polyY[j] < y and k.polyY[i] >= y)
					and (k.polyX[i] <= x or k.polyX[j] <= x))
					then
						if (k.polyX[i] + (y - k.polyY[i]) / (k.polyY[j] - k.polyY[i]) * (k.polyX[j] - k.polyX[i]) < x) then
							oddNodes = not oddNodes;
						end
					end
					j = i;
				end
				return oddNodes;
			end
		end
			return false;
	else
		print("Error: Missing argument on function: checkAreaPoint");
	end
		return false;
end

-- Callbacks
function OnPlayerEnterArea(playerid, area)
end

function OnPlayerExitArea(playerid, area)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");