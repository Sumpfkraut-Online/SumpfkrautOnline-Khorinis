--[[
	
	Module: GetTableValue.lua
	Autor: Bimbol
	
	Get Table value.
	
]]--

function GetTableValue(_table)
	local values = {};
	if _table and type(_table) == "table"  then
		resultTableValue(_table, values);
		return values;
	end
		return false;
end

function resultTableValue(_table, values)
	for i, k in pairs(_table) do
		if type(k) == "table" then
			resultTableValue(k, values);
		else
			table.insert(values, k);
		end
	end
		return values;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");