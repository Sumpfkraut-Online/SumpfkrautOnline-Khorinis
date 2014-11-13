--[[
	
	Module: event-handler.lua
	Autor: Bimbol
	
	Event System
	
]]--

local event = {};

function addEvent(eventname, func)
	if eventname and type(func) == "function" then
		if not event[eventname] then
			event[eventname] = {};
		end
		if type(eventname) == "table" then
			for _, v in ipairs(eventname) do
				table.insert(event[eventname], { func = func });
			end
		else
			table.insert(event[eventname], { func = func });
		end
			return true;
	end
		return false;
end

function removeEvent(eventname)
	if eventname then
		event[eventname] = nil;
		return true;
	end
		return false;
end

function callEvent(eventname, ...)
	if eventname then
		local var = false;
		for _, event in ipairs(event[eventname] or {}) do
			event.func(unpack(arg));
			var = true;
		end
			return var;
	end
		return false;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");