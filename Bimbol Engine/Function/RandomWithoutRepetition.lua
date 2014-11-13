--[[
	
	Module: RandomWithoutRepetition.lua
	Autor: Bimbol
	
	Random value without repetitione.
	
]]--

function checkValueAlreadyExist(value, tab)
	if #tab < 1 then
		return true;
	end
	for i = 1, #tab do
		if tab[i] == value then
			return false;
		end
	end
		return true;
end

function RandomWithoutRepetition(min, max, amount)
	if min and max and min <= max then
		local reduce;
		if min < 0 then
			reduce = math.abs(min);
			max = max + reduce;
			min = 0;
		end
		
		min = min - 1;
		local diff = max - min;
		local value;
		local rand_value = {};
		
		if reduce then
			for i = 1, amount do
				repeat
					value = math.random(diff) + min - reduce;
				until checkValueAlreadyExist(value, rand_value);
				table.insert(rand_value, value);
			end
		else
			for i = 1, amount do
				repeat
					value = math.random(diff) + min;
				until checkValueAlreadyExist(value, rand_value);
				table.insert(rand_value, value);
			end
		end
			return rand_value;
	end
		return false;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");