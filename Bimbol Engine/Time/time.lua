--[[
	
	Module: time.lua
	Autor: Bimbol
	
	Time
	
]]--

local TIME = false;

function enable_OnTime(value)
	if type(value) == "boolean" and value then
		TIME = true;
		SetTimer("BE_OnTime", 4015, 1);
	end
end

function BE_OnTime()
	local hour, minute = GetTime();
	OnTime(hour, minute);
end

function OnTime(hour, minute)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");


