--[[
	
	Module: camera.lua
	Autor: Bimbol
	
	Camera function
	
]]--

local CameraVob = {};

function setCameraBeforePlayer(playerid, distance)
	if CameraVob[playerid] then CameraVob[playerid]:Destroy(); end
	local x, y, z = GetPlayerPos(playerid);
	local angle = GetPlayerAngle(playerid);
	CameraVob[playerid] = Vob.Create("", GetPlayerWorld(playerid), x, y, z);
	CameraVob[playerid]:SetRotation(0, angle, 0);
	
	x = x + (math.sin(angle * 3.14 / 180.0) * distance);
	z = z + (math.cos(angle * 3.14 / 180.0) * distance);
	
	CameraVob[playerid]:SetPosition(x, y, z );
	CameraVob[playerid]:SetRotation(0, angle + 180, 0);
	SetCameraBehindVob(playerid, CameraVob[playerid]);
end

function SetDefaultCamera(playerid)
	_SetDefaultCamera(playerid);
	restartCamera(playerid);
end

function restartCamera(playerid)
	if CameraVob[playerid] then
		CameraVob[playerid]:Destroy();
		CameraVob[playerid] = nil;
	end
end

addCommandHandler("/camera", function(pid, param)
	local result, id = sscanf(param, "d");
	setCameraBehindPlayer(pid, id, 100);
end);

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");


