--[[
	
	Module: time_event.lua
	Autor: Bimbol
	
	Time Events
	
]]--

local TIME = false;
local TimeEvents = {};

local callTimeEvent;

function time_Init(value)
	if value and type(value) == "boolean" then
		timerAdd("BE_TIMEEVENT", callTimeEvent, 60, false, true);
		TIME = true;
	end
end

function addTimeEvent(func, time)
	if TIME and func and type(func) == "function" and time then
		table.insert(time, func);
		table.insert(TimeEvents, time);
		return true;
	end
		return false;
end

function callTimeEvent()
	local day = tonumber(os.date("%d"));
	local month = tonumber(os.date("%m"));
	local hour = tonumber(os.date("%H"));
	local minute = tonumber(os.date("%M"));
	local year = tonumber(os.date("%Y"));
	
	local event = true;
	
	for i, k in ipairs(TimeEvents) do
		if k.day and k.day ~= day
		or k.month and k.month ~= month
		or k.hour and k.hour ~= hour
		or k.min and k.min ~= minute
		then
			event = false;
		end

		if event then
			k[1](year, month, day, hour, minute);
		else
			event = true;
		end
	end
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");


