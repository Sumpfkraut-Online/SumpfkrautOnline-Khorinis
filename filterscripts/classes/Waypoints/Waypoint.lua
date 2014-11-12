--This class/table is used as a structure for waypointdata
Waypoint = {};
Waypoint.__index = Waypoint;

function Waypoint:new(waypointname,posX,posY,posZ,angle,world)
	local newWaypoint = {};	
	setmetatable(newWaypoint, Waypoint);
	if waypointname and posX and posY and posZ and angle and world then
	--Set all attributes
	newWaypoint.waypointname = waypointname;
	newWaypoint.posX=posX;
	newWaypoint.posY=posY;
	newWaypoint.posZ=posZ;
	newWaypoint.angle=angle;
	newWaypoint.world=world;
	
	return newWaypoint;
	else
		print("Waypoint:new(waypointname,posX,posY,posZ,angle) Missing Parameter")
		return nil;
	end
end


