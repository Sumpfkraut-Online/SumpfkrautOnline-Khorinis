require("filterscripts.classes.Waypoints.Waypoint");

WaypointSystem = {};
WaypointSystem.__index = WaypointSystem;
--Array with all Waypoints in it.

--Global Instance of this Modul.
WaypointSystemInstance=nil;

--Creates a new instance of WaypointSystem doesnt return anything instance is saved in WaypointSystemInstance
--@params nil
--@params nil
function WaypointSystem:new()
	local newWaypointSystem = {};	
	setmetatable(newWaypointSystem, WaypointSystem);
	newWaypointSystem.Waypoints={};
	local sqlCMD ="SELECT * FROM gothic_multiplayer.wp_waypoints;";
	local result = runSQL(sqlCMD);
	local amount = 0;
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			newWaypoint= Waypoint:new(row[1],tonumber(row[2]),tonumber(row[3]),tonumber(row[4]),tonumber(row[5]),row[6]);
			newWaypointSystem.Waypoints[row[1]]=newWaypoint;	
			amount=amount+1;
			row = mysql_fetch_row(result);
		end
	end
	mysql_free_result (result);
	runSQL(sqlCMD);
	WaypointSystemInstance =newWaypointSystem;
	return amount;
end

--Save a Waypoint with the given values
--@params waypointname,x,y,z,winkel
--@params nil
function SaveWaypoint(waypointname,posX,posY,posZ,angle,world)
	if not WaypointSystemInstance.Waypoints[string.lower(waypointname)] then 
		newWaypoint= Waypoint:new(string.lower(waypointname),posX,posY,posZ,angle,world);
		if newWaypoint then	
			WaypointSystemInstance.Waypoints[string.lower(waypointname)]=newWaypoint;
			local sqlCMD = "INSERT INTO `gothic_multiplayer`.`wp_waypoints` (`name`, `posX`, `posY`, `posZ`, `angle`, `world`) VALUES ('"..string.lower(waypointname).."', '"..posX.."', '"..posY.."', '"..posZ.."', '"..angle.."', '"..world.."');";
			runSQL(sqlCMD);
			return true ;
		else
			print("Could not save Waypoint!");
			return false;
		end
	else
		print("Could not save Waypoint! Because it already exists");
	end
end

--Save a Waypoint at the Position of an Player.
--@params playerid of the player for whos position the waypoint should be created. Name of the new Waypoint
--@params nil
function SaveWaypointAtPlayer(playerid, waypointname)
	local posX,posY,posZ = GetPlayerPos(playerid);
	local angle = GetPlayerAngle(playerid);
	local world = GetPlayerWorld(playerid);
	return SaveWaypoint(waypointname,posX,posY,posZ,angle,world);
end

--Deletes a Waypoint with the given name
--@params waypointname,x,y,z,winkel
--@params nil
function DeleteWaypoint(waypointname)
	if waypointname then
		if WaypointSystemInstance.Waypoints[string.lower(waypointname)] then
			local sqlCMD ="DELETE FROM `gothic_multiplayer`.`wp_waypoints` WHERE `name`='"..string.lower(waypointname).."';";
			runSQL(sqlCMD);
			WaypointSystemInstance.Waypoints[string.lower(waypointname)]=nil;
			return true;
		else
			print("Tryed to delete Waypoint with name "..string.lower(waypointname).. " which does not even exists");
			return false;
		end
	else
		print("Missing Parameter: 'waypointname'");
		return false;
	end
end

--teleports the given player to the waypoint with the given name
--@params playerid,waypointname
--@params nil
function TeleportPlayerToWayPoint(playerid,waypointname)
		if WaypointSystemInstance.Waypoints[string.lower(waypointname)] and playerid then
			local tempWaypoint = WaypointSystemInstance.Waypoints[string.lower(waypointname)];
			if GetPlayerWorld(playerid) ~= tempWaypoint.world then			
				if	 tempWaypoint.world ~= "NEWWORLD\\NEWWORLD.ZEN" then
					if	GetPlayerWorld(playerid)~="NEWWORLD\\NEWWORLD.ZEN" then			
						SetPlayerWorld(playerid,"NEWWORLD\\NEWWORLD.ZEN","MAGECAVE")
					end							
				elseif tempWaypoint.world ~="DRAGONISLAND\\DRAGONISLAND.ZEN"  then
					if GetPlayerWorld(playerid)~="DRAGONISLAND\\DRAGONISLAND.ZEN" then		
						SetPlayerWorld(playerid, "DRAGONISLAND\\DRAGONISLAND.ZEN", "SHIP")
					end
				elseif tempWaypoint.world ~="OLDWORLD\\OLDWORLD.ZEN"  then
					if GetPlayerWorld(playerid)~="OLDWORLD\\OLDWORLD.ZEN"  then		
						SetPlayerWorld(playerid,"OLDWORLD\\OLDWORLD.ZEN","WP_INTRO13");
					end
				end		
			end
			--SetPlayerPos(playerid,tempWaypoint.posX,tempWaypoint.posY ,tempWaypoint.posZ);
			SetPlayerAngle(playerid, tempWaypoint.angle)
			return true;
	else
		if not waypointname then
			print("Missing Parameter: 'waypointname' or 'playerid'");
		else
			print("There is no Waypoint with the name: '"..string.lower(waypointname).."'");
		end
		return false;
	end
end
--Instantiating the WaypointSystem
if not WaypointSystemInstance then
	local amount = WaypointSystem:new();
	print("WaypointSystem loaded and instantiated "..amount.." Waypoint(s) loaded from DB.");
end
