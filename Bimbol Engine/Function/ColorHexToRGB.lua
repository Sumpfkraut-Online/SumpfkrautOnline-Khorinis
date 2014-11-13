--[[
	
	Module: ColorHexToRGB.lua
	Autor: Bimbol
	
	Transform hex color to r g b.
	
]]--

local mapHB = {
	["0"] = "0000",
	["1"] = "0001",
	["2"] = "0010",
	["3"] = "0011",
	["4"] = "0100",
	["5"] = "0101",
	["6"] = "0110",
	["7"] = "0111",
	["8"] = "1000",
	["9"] = "1001",
	["a"] = "1010",
	["b"] = "1011",
	["c"] = "1100",
	["d"] = "1101",
	["e"] = "1110",
	["f"] = "1111",
};

local function toBin(hex)
	local result = "";
	for i in string.gfind(hex, ".") do
		if mapHB[i] then
			result = result .. mapHB[i];
		else
			result = result .. "0000";
		end
	end
		return result;
end

local function toDec(bin)
	local result = 0;
	local lenght = string.len(bin) - 1;
	for i in string.gfind(bin, ".") do
		result = result + tonumber(i) * 2^lenght;
		lenght = lenght - 1;
	end
		return result;
end

function ColorHexToRGB(hex)
	if type(hex) == "string" then
		hex = string.lower(hex);
		local char = {};
		local RGB = {};
		
		for i = 1, 6 do
			char[i] = string.sub(hex, i, i);
			if char[i] == "" then char[i] = 0 end
		end
		
		for i = 1, 6, 2 do
			RGB[math.ceil(i/2)] = char[i] .. char[i + 1];
		end
		
		local r = toBin(RGB[1]);
		local g = toBin(RGB[2]);
		local b = toBin(RGB[3]);
		
		return toDec(r), toDec(g), toDec(b);
	else
		print("Error: Missing argument on function: ColorHexToRGB");
	end
		return 0, 0, 0;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");