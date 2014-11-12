print("Loading Animation.lua");

Animation = {};
Animation.__index = Animation;

---Creates an instance of Animation
function Animation.new()
	local newAnimation = {};
	
	setmetatable(newAnimation, Animation);
	
	newAnimation.animations = {};
	
	newAnimation:loadAnimationsFromDB();
	
	return newAnimation;
end

function Animation:playAnimation(playerid, command)
	if self.animations[command] then
		local name = self.animations[command][1];
		local duration = self.animations[command][2];
		
		PlayAnimation(playerid, name);
		SetTimerEx("resetAnimation", duration, 0, playerid);
		
		return 1;
	else
		return 0;
	end
end

---Adds an animation to the list of known animations
function Animation:addAnimation(anim, command)
	self.animations[command] = anim;
end

---Loads all known animations from DB
function Animation:loadAnimationsFromDB()
	local result = runSQL("SELECT anim_name, anim_command, duration FROM animations;");
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			self:addAnimation({ row[1], tonumber(row[3]) }, row[2]);
			
			row = mysql_fetch_row(result);
		end
	end
end


--instantiate
if not instance_Animation then
	instance_Animation = Animation.new();
end