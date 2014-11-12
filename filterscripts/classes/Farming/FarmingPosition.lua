print("Loading FarmingPosition.lua");

FarmingPosition = {};
FarmingPosition.__index = FarmingPosition;

---Creates an instance of FarmingPosition
function FarmingPosition.new(playerid, itemInstance, radius, x, y, z, world, water, fertilizer, reported)
	local newFarmingPosition = {};
	
	newFarmingPosition.itemInstance = itemInstance;
	newFarmingPosition.radius = radius;
	newFarmingPosition.vobInstance = nil;
	newFarmingPosition.x = x;
	newFarmingPosition.y = y;
	newFarmingPosition.z = z;
	newFarmingPosition.world = "NEWWORLD\\NEWWORLD.ZEN";
  
  newFarmingPosition.reported = false;
	newFarmingPosition.water = false;
  newFarmingPosition.fertilizer = false;
  
  setmetatable(newFarmingPosition, FarmingPosition);
	
	if playerid then
		if newFarmingPosition:init(playerid) == 1 then
      
			return newFarmingPosition;
		end
	else
		newFarmingPosition:reload();
    
    --set reported
    if reported then
      if reported == 1 then
        newFarmingPosition.reported = true;
      end
    end
  
    --set water
    if water then
      if water == 1 then
        newFarmingPosition:setWater(true);
      end
    end
	
    --set fertilizer
    if fertilizer then
      if fertilizer == 1 then
        newFarmingPosition:setFertilizer(true);
      end
    end
    
		return newFarmingPosition;
	end
	
	return nil;
end

---Initializes the FarmingPosition
function FarmingPosition:init(playerid)
	local x, y, z = GetPlayerPos(playerid);

	self.x = math.floor(x);
	self.y = math.floor(y) - 100;
	self.z = math.floor(z);
	self.world = GetPlayerWorld(playerid);
	
	if instance_FarmingSystem.farmingMan:checkPlantDistance(self) == 1 then
		self.vobInstance = Vob.Create("Erdetrocken.3DS", self.world, self.x, self.y, self.z);
		
		return 1;
	end
	
	--send error message
	SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du kannst hier nichts anpflanzen. Halte etwas mehr Abstand!");
	
	return 0;
end

function FarmingPosition:reload()
	if self.water == true then
		self.vobInstance = Vob.Create("Erdenass.3DS", self.world, self.x, self.y, self.z);
    
    if self.fertilizer == true then
      self.vobInstance = Vob.Create("Erdeduenger.3DS", self.world, self.x, self.y, self.z);
    end
    
    return;
	end
  
	
	self.vobInstance = Vob.Create("Erdetrocken.3DS", self.world, self.x, self.y, self.z);
end

---Clears the FarmingPosition
--ATTENTION: And spawns an item at the moment -> later: Permafrost
function FarmingPosition:deinit()
	self.vobInstance:Destroy();
end

---Sets the value for self.water and checks whether farming process is completed
function FarmingPosition:setWater(value)
	self.water = value;
	
	--'update' the vob
	self.vobInstance:Destroy();
	self.vobInstance = Vob.Create("Erdenass.3DS", self.world, self.x, self.y, self.z);
end

---Sets the value for self.fertilizer and checks whether farming process is completed
function FarmingPosition:setFertilizer(value)
  
  --watering has to be done first
  if self.water == false then return; end;
  
	self.fertilizer = value;
	
	--'update' the vob
	self.vobInstance:Destroy();
	self.vobInstance = Vob.Create("Erdeduenger.3DS", self.world, self.x, self.y, self.z);
	
	if value == true and self.water == true and self.reported == false then
		self.reported = true;
		
    print("Calling Permafrost...");
    SPAWNER_FARM_001:collInsert (0, 1, nil, {id=nil, x=self.x, y=self.y + 100, z=self.z, instance=self.itemInstance, amount=1, spawningTime=5}, nil);
	end
end

--[[function FarmingPosition:callPermafrost()
	print("Calling Permafrost");
	
  local arg = string.format("%d %d %d ", self.x, self.y, self.z);
  
	--self:callback();
  SetTimerEx("callback", 5000, 1, arg);	
end

function callback(arg)
  
  arg = tostring(arg);
  local pos = {};
  
  for w in arg:gmatch("[%-]-[0-9 ]+") do
    table.insert(pos, w);
  end
  
	instance_FarmingSystem.farmingMan:removeFarmingPosition(x, y, z);
end]]

---Returns a string representation of this instance
function FarmingPosition.toString(self)
	return string.format("Position: (%d, %d, %d)\nInstance: %s\nRadius: %d\nWater: %s\nFertilizer: %s\nReported: %s\n------------",self.x, self.y, self.z, self.itemInstance, self.radius, tostring(self.water), tostring(self.fertilizer), tostring(self.reported));
end

--add toString-function to metatable
FarmingPosition.__tostring = FarmingPosition.toString;