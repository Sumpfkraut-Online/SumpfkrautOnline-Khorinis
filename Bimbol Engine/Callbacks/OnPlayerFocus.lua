--[[
	
	Module: OnPlayerFocus.lua
	Autor: Bimbol
	
	OnPlayerFocus
	
]]--

local Focus = {};

for i = 0, MAX_PLAYERS - 1 do
	Focus[i] = -1;
end
	
function restartFocus(playerid)
	Focus[playerid] = -1;
end

function OnPlayerFocus(playerid, focusid)
	if focusid == -1 and Focus[playerid] ~= -1 then
		if isNPC(Focus[playerid]) then
			OnPlayerLostFocusNPC(playerid, Focus[playerid]);
		elseif isMonster(Focus[playerid]) then
			OnPlayerLostFocusMonster(playerid, Focus[playerid]);
		else
			if IsNPC(Focus[playerid]) == 0 then
				OnPlayerLostFocusPlayer(playerid, Focus[playerid]);
			else
				OnPlayerLostFocusBot(playerid, Focus[playerid]);
			end
		end
		restartFocus(playerid);
	else
		if Focus[playerid] ~= -1 then
			if isNPC(Focus[playerid]) then
				OnPlayerLostFocusNPC(playerid, Focus[playerid]);
			elseif isMonster(Focus[playerid]) then
				OnPlayerLostFocusMonster(playerid, Focus[playerid]);
			else
				if IsNPC(Focus[playerid]) == 0 then
					OnPlayerLostFocusPlayer(playerid, Focus[playerid]);
				else
					OnPlayerLostFocusBot(playerid, Focus[playerid]);
				end
			end
		end
		
		if isNPC(focusid) then
			OnPlayerFocusNPC(playerid, focusid);
		elseif isMonster(focusid) then
			OnPlayerFocusMonster(playerid, focusid);
		else
			if IsNPC(playerid) == 0 then
				OnPlayerFocusPlayer(playerid, focusid);
			else
				OnPlayerFocusBot(playerid, focusid);
			end
		end
		
		Focus[playerid] = focusid;
	end
	-- BE Callbacks --
	BE_OnPlayerFocus(playerid, focusid);
end

function BE_OnPlayerFocus(playerid, focusid)
end

-- Callbacks
function OnPlayerFocusNPC(playerid, focusid)
end

function OnPlayerFocusMonster(playerid, focusid)
end

function OnPlayerFocusPlayer(playerid, focusid)
end

function OnPlayerFocusBot(playerid, focusid)
end

function OnPlayerLostFocusNPC(playerid, last_focusid)
end

function OnPlayerLostFocusMonster(playerid, last_focusid)
end

function OnPlayerLostFocusPlayer(playerid, last_focusid)
end

function OnPlayerLostFocusBot(playerid, last_focusid)
end


-- Loaded
print(debug.getinfo(1).source.." has been loaded.");