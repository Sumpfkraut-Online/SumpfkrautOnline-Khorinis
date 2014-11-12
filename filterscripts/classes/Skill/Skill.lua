print("Loading Skill.lua");

Skill = {};
Skill.__index = Skill;

---Creates an instance of Teach
function Skill.new(skillID, caption)
	local newSkill = {};
	
	setmetatable(newSkill, Skill);
	
	newSkill.skillID = skillID;
	newSkill.caption = caption;

	
	return newSkill;
end

---Returns the string representation of the instance
function Skill.toString(self)	
	return string.format("Skill %s (ID: %d)", self.caption, self.skillID);
end

--add toString-function to metatable
Skill.__tostring = Skill.toString;