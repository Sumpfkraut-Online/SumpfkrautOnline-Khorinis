--[[
	
	Author: Bimbol
	Module: Split.lua
	
	Split string
	
]]--

function Split(str, pat)
	local str_split = {};
	pat = "(.-)" .. pat;
	for value in string.gmatch(str, pat) do
		table.insert(str_split, value);
	end
	return str_split;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");