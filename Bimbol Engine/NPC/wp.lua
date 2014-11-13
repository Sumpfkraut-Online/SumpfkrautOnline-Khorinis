--[[
	
	Module: npc.lua
	Autor: Bimbol
	
	NPC System
	
]]--

local Player = {};

local WayPoint = {};
local Path = {};
local CurrentlyPath = {};

local Distance = {};
local Step = {};
local Used = {};
local Node = {};
local NodeMT = {};

local WAYPOINT = false;
local MAX_WAYPOINT = -1;
local MAX_WAYPOINT_CONNECT = 0;
local AMOUNT_WAYPOINT = 0;

local WayPoint_Check;

function wp_Init(max_wp, timer)
	if type(max_wp) == "number" then
		WAYPOINT = true;
		MAX_WAYPOINT = max_wp;
		
		for i = 1, MAX_WAYPOINT do
			Distance[i] = 999999;
			Step[i] = 0;
			Used[i] = false;
		end
		
		if timer then timerAdd("BE_WAYPOINT", WayPoint_Check, 0.3, false, true); end
	else
		print("Error: Missing argument on function: wp_Init");
	end
end

function WayPoint_Check()
	for i, k in ipairs(CurrentlyPath) do
		if k.x and k.z then
			local x, y, z = GetPlayerPos(k.id);
			if GetDistance2D(x, z, k.x, k.z) <= 200 then
				if GetPlayerInstance(k.id) == "PC_HERO" then
					PlayAnimation(k.id, "S_RUN");
				else
					PlayAnimation(k.id, "S_FISTRUN");
				end
				OnPlayerReachedPoint(k.id, k.x, k.z);
				table.remove(CurrentlyPath, i);
			end
		else
			local wp_x, wp_z = getWayPointPos(k.path[k.curr_path]);
			local x, y, z = GetPlayerPos(k.id);
			
			local angle = GetAngleToPos(x, z, wp_x, wp_z); -- This function have some bugs :(
			SetPlayerAngle(k.id, math.ceil(angle));
			
			if GetDistance2D(x, z, wp_x, wp_z) <= 150 then
				k.curr_path = k.curr_path + 1;
				if k.curr_path <= #k.path then
					goToNextWayPoint(k.id, k.path[k.curr_path], k.anim);
					OnPlayerWayPoint(k.id, k.path[k.curr_path - 1]);
				else
					-- End of Path
					if GetPlayerInstance(k.id) == "PC_HERO" then
						PlayAnimation(k.id, "S_RUN");
					else
						PlayAnimation(k.id, "S_FISTRUN");
					end
					OnPlayerFinishPath(k.id, k.path_name, k.anim);
					OnPlayerWayPoint(k.id, k.path[k.curr_path - 1]);
					table.remove(CurrentlyPath, i);
				end
			end
		end
	end
end

function createWayPointPath(path_name, wp_list)
	if WAYPOINT and path_name and wp_list then
		for i, k in ipairs(Path) do
			if k.id == path_name then
				return false;
			end
		end
		table.insert(Path, { id = path_name, wp = wp_list });
			return true;
	else
		print("Error: Missing argument on function: createWayPointPath");
	end
		return false;
end

function destroyWayPointPath(path_name)
	if WAYPOINT and path_name and wp_list then
		for i, k in ipairs(Path) do
			if k.id == path_name then
				table.remove(Path, i);
				return true;
			end
		end
	else
		print("Error: Missing argument on function: createWayPointPath");
	end
		return false;
end

function followWayPointPath(playerid, path_name, anim)
	if WAYPOINT and playerid and path_name and anim then
		if IsNPC(playerid) == 1 then
			for i, k in ipairs(Path) do
				if k.id == path_name then
					for j, v in ipairs(CurrentlyPath) do
						if v.id == playerid then
							return false;
						end
					end
					table.insert(CurrentlyPath, { id = playerid, path_name = path_name, curr_path = 1, path = k.wp, anim = anim });
					OnPlayerStartedFollowPath(k.id, path_name, anim);
					goToNextWayPoint(playerid, k.wp[1]);
					PlayAnimation(playerid, anim);
						return true;
				end
			end
		else
			print("Error: Missing argument on function: followWayPointPath. Only Bots can follow path!");
		end
	else
		print("Error: Missing argument on function: followWayPointPath");
	end
		return false;
end

function stopFollowWayPointPath(playerid)
	if WAYPOINT and IsNPC(playerid) == 1 then
		for j, v in ipairs(CurrentlyPath) do
			if v.id == playerid then
				if GetPlayerInstance(v.id) == "PC_HERO" then
					PlayAnimation(v.id, "S_RUN");
				else
					PlayAnimation(v.id, "S_FISTRUN");
				end
				OnPlayerFinishPath(v.id, v.path_name, v.anim);
				table.remove(CurrentlyPath, j);
					return true;
			end
		end
	else
		print("Error: Missing argument on function: stopFollowWayPointPath");
	end
		return false;
end

local function findWayPointPath(near, id)
	Distance[WayPoint[near].nr] = 0;

	local u;
	for i = 1, MAX_WAYPOINT_CONNECT do
		u = -1
		for j = 1, MAX_WAYPOINT_CONNECT do
			if not Used[j] and (u == -1 or Distance[j] < Distance[u]) then u = j; end
		end
		Used[u] = true;
		
		while Node[u] do
			if not Used[Node[u].number] and (Distance[Node[u].number] > Distance[u] + Node[u].weight) then
				Distance[Node[u].number] = Distance[u] + Node[u].weight;
				Step[Node[u].number] = u;
			end
			Node[u] = Node[u].next;
		end
	end

	-- Returning way
	local result = {};
	local j = WayPoint[id].nr;

	while j do
		local wp = 0;
		for _, waypoint in pairs(WayPoint) do
			if waypoint.nr == j then
				wp = waypoint.id;
				break;
			end
		end
		
		table.insert(result, wp);
		j = Step[j];
	end

	table.remove(result, #result);
	
	for i = 1, #result / 2 do
		local temp = result[i];
		result[i] = result[#result - (i - 1)];
		result[#result - (i - 1)] = temp;
	end

	-- Restart
	for i = 1, MAX_WAYPOINT_CONNECT do
		Distance[i] = 999999;
		Step[i] = 0;
		Used[i] = false;
		if NodeMT[i] then
			Node[i] = NodeMT[i];
		end
	end
	
	return result, Distance[WayPoint[id].nr];
end

function getWayPointPos(id)
	if id then
		if WayPoint[id] then
			return WayPoint[id].x, WayPoint[id].z;
		end
	else
		print("Error: Missing argument on function: getWayPointPos");
	end
		return false;
end

function goToXZPoint(playerid, go_x, go_z, anim)
	if playerid and go_x and go_z and anim then
		local x, y, z = GetPlayerPos(playerid);
		SetPlayerAngle(playerid, GetAngleToPos(x, z, go_x, go_z));
		PlayAnimation(playerid, anim);
		table.insert(CurrentlyPath, { id = playerid, x = go_x, z = go_z });
			return true;
	else
		print("Error: Missing argument on function: goToXZPoint");
	end
		return false;
end

function goToNextWayPoint(playerid, id)
	local wp_x, wp_z = getWayPointPos(id);
	local x, y, z = GetPlayerPos(playerid);
	SetPlayerAngle(playerid, GetAngleToPos(x, z, wp_x, wp_z));
end

function goToWayPoint(playerid, id, anim)
	if WayPoint[id] then
		stopFollowWayPointPath(playerid);
		local pathname = "BE" .. id;
		
		local path = findWayPointPath(getNearestWayPoint(playerid), id);
		createWayPointPath(pathname, path);
		followWayPointPath(playerid, pathname, anim);
			return true;
	end
		return false;
end

function getWayPointNumber(id)
	if id then
		return WayPoint[id].nr;
	else
		print("Error: Missing argument on function: getWayPointNumber");
	end
		return false;
end

function getNearestWayPoint(playerid)
	local nearst = {
		id = false,
		dist = 999999,
	};
	
	local x, y, z = GetPlayerPos(playerid);
	
	for _, waypoint in pairs(WayPoint) do
		local dist = GetDistance2D(x, z, waypoint.x, waypoint.z);
		if dist < nearst.dist then
			nearst.id = waypoint.id;
			nearst.dist = dist;
		end
	end
		return nearst.id;
end

function connectWayPoints(id1, id2, mutually)
	if id1 and WayPoint[id1]
	and id2 and WayPoint[id2]
	then
		if WayPoint[id1].nr > MAX_WAYPOINT_CONNECT then MAX_WAYPOINT_CONNECT = WayPoint[id1].nr;
		elseif WayPoint[id2].nr > MAX_WAYPOINT_CONNECT then MAX_WAYPOINT_CONNECT = WayPoint[id2].nr; end
		
		local node = {};
		node.next = Node[WayPoint[id1].nr];
		node.number = WayPoint[id2].nr;
		node.weight = math.ceil(GetDistance2D(WayPoint[id1].x, WayPoint[id1].z, WayPoint[id2].x, WayPoint[id2].z));
		Node[WayPoint[id1].nr] = node;
		NodeMT[WayPoint[id1].nr] = node;
		
		if mutually then connectWayPoints(id2, id1, false); end
			return true;
	else
		print("Error: Missing argument on function: connectWayPoints");
	end
		return false;
end

function addWayPoint(id, x, z)
	if id and x and z then
		if AMOUNT_WAYPOINT >= MAX_WAYPOINT then return false; end
		if WayPoint[id] then
			return false;
		end
		
		AMOUNT_WAYPOINT = AMOUNT_WAYPOINT + 1;
		WayPoint[id] = { id = id, nr = AMOUNT_WAYPOINT, x = x, z = z };
			return true;
	else
		print("Error: Missing argument on function: addWayPoint");
	end
		return false;
end

function removeWayPoint(id)
	if id and x and y and z then
		if WayPoint[id] then
			WayPoint[id] = nil;
		end
	else
		print("Error: Missing argument on function: removeWayPoint");
	end
		return false;
end

-- Callbacks
function OnPlayerWayPoint(playerid, waypoint)
end

function OnPlayerStartedFollowPath(playerid, path_name, animation)
end

function OnPlayerFinishPath(playerid, path_name, animation)
end

function OnPlayerReachedPoint(playerid, x, z)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");