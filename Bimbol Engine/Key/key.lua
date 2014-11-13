--[[
	
	Module: key.lua
	Autor: Bimbol
	
	Key System
	
]]--

local Key = {};
local ENTER = {};
for i = 0, MAX_PLAYERS - 1 do
	Key[i] = {};
	ENTER[i] = {};
	ENTER[i].Value = false;
end

local KeyGlob = {};

function DisableEnterKey(playerid)
	ENTER[playerid].Value = true;
end

local function checkEnterKey(playerid, key)
	if key == KEY_RETURN and ENTER[playerid].Value then
		--ENTER[playerid].Enable = not ENTER[playerid].Enable;
		--return not ENTER[playerid].Enable;
		return false;
	end
		return true;
end

-- Key
function bindKey(playerid, key, keyState, handlerFunction, ...)
	if IsNPC(playerid) == 0 then
		if playerid and key and keyState and type(handlerFunction) == "function" then
			for i, k in ipairs(Key[playerid]) do
				if k.key == key and k.keystate == keyState then
					return false;
				end
			end
			table.insert(Key[playerid], { key = key, keystate = keyState, func = handlerFunction, arg = arg });
				return true;
		else
			print("Error: Missing argument on function: bindKey");
		end
	end
		return false;
end

function removeKey(playerid, key, keyState)
	if IsNPC(playerid) == 0 then
		if playerid and key and keyState then
			for i, k in ipairs(Key[playerid]) do
				if k.key == key and k.keystate == keyState then
					table.remove(Key[playerid], i);
				end
			end
				return true;
		else
			print("Error: Missing argument on function: removeKey");
		end
	end
		return false;
end

function callBindKeyUp(playerid, key)
	for i, k in ipairs(Key[playerid]) do
		if k.key == key and k.keystate == "UP" then
			k.func(playerid, key, "UP", unpack(k.arg));
		end
	end
end

function callBindKeyDown(playerid, key)
	for i, k in ipairs(Key[playerid]) do
		if k.key == key and k.keystate == "DOWN" and checkEnterKey(playerid, key) then
			k.func(playerid, key, "DOWN", unpack(k.arg));
		end
	end
	ENTER[playerid].Value = false;
end

-- Key Global
function bindKeyGlobal(key, keyState, handlerFunction, ...)
	if key and keyState and type(handlerFunction) == "function" then
		for i, k in ipairs(KeyGlob) do
			if k.key == key and k.keystate == keyState then
				return false;
			end
		end
		table.insert(KeyGlob, { key = key, keystate = keyState, func = handlerFunction, arg = arg });
			return true;
	else
		print("Error: Missing argument on function: bindKeyGlobal");
	end
		return false;
end

function removeKeyGlobal(key, keyState)
	if key and keyState then
		for i, k in ipairs(KeyGlob) do
			if k.key == key and k.keystate == keyState then
				table.remove(KeyGlob, i);
				return true;
			end
		end
	else
		print("Error: Missing argument on function: removeKeyGlobal");
	end
		return false;
end

function callBindKeyGlobUp(playerid, key)
	for i, k in ipairs(KeyGlob) do
		if k.key == key and k.keystate == "UP" then
			k.func(playerid, key, "UP", unpack(k.arg));
		end
	end
end

function callBindKeyGlobDown(playerid, key)
	for i, k in ipairs(KeyGlob) do
		if k.key == key and k.keystate == "DOWN" and checkEnterKey(playerid, key) then
			k.func(playerid, key, "DOWN", unpack(k.arg));
		end
	end
	ENTER[playerid].Value = false;	
end

function restartKey(playerid)
	for i = 1, #Key[playerid] do
		table.remove(Key[playerid], 1);
	end
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");


