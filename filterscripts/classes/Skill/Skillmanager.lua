require "filterscripts/classes/skill/Skill"

print("Loading Skillmanager.lua");

Skillmanager = {};
Skillmanager.__index = Skillmanager;

---Creates an instance of Skillmanager
function Skillmanager.new()
	local newSkillmanager = {};
	
	setmetatable(newSkillmanager, Skillmanager);
	
	newSkillmanager.skills = {};
	
	newSkillmanager:loadSkillsFromDB();
		
	return newSkillmanager;
end

---Adds a skill to this skill manager instance
function Skillmanager:addSkill(skill)
	if self.skills[skill.skillID] then return false; end;
	
	self.skills[skill.skillID] = skill;
end

---Loads all teach-categories from DB and adds them to this instance
function Skillmanager:loadSkillsFromDB()
	local result = runSQL("SELECT id, caption FROM skills");
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			local skill = Skill.new(tonumber(row[1]), row[2]);

			self:addSkill(skill);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Returns the caption of a skill by its id
function Skillmanager:getSkillNameByID(id)
	if self.skills[id] then
		return self.skills[id].caption;
	end
	
	return "NULL";
end