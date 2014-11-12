require "filterscripts/classes/Teach/TeachBorder"

print("Loading Teach.lua");

Teach = {};
Teach.__index = Teach;

---Creates an instance of Teach
function Teach.new(teachID, categoryID, caption, isBasic, masteringTeachID, duration)
	local newTeach = {};
	
	setmetatable(newTeach, Teach);
	
	newTeach.teachID = teachID;
	newTeach.categoryID = categoryID;
	newTeach.caption = caption;
	newTeach.isBasic = isBasic;
	newTeach.masteringTeachID = masteringTeachID;
	newTeach.duration = duration;
	
	--contains all skills that are trained by this teach
	newTeach.skills = {};
	
	--contains all skills (and values) that are required to get this teach
	newTeach.preconditions = {};
	
	--[[
	Border-example:
	Skill		Teaches needed
	0-20:		1
	21-30:		2
	31-40		3
	...			...
	
	IMPORTANT: NO OVERLAPPING VALUES ARE ALLOWED
	]]
	newTeach.borders = {};
	
	newTeach:loadSkills();
	newTeach:loadPreconditions();
	newTeach:loadBorders();

	--print(tostring(newTeach));
	
	return newTeach;
end

---Checks whether a player fulfills all preconditions of this teach
function Teach:checkPreconditions(characterStudent, dualPlayerTeach)
	
	if not dualPlayerTeach then dualPlayerTeach = 0; end;
	
	--check whether the student can be taught or not (-> one hour online/day)
	if characterStudent.canTeach == false then
		SendPlayerMessage(characterStudent.playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du hast Heute entweder schon etwas gelernt oder bist noch nicht bereit dafuer.");
		return false;
	end
	
	--check if this teach is a 'advanced' book teach
	--those teaches do not have any other conditions.
	if self.masteringTeachID then
		if characterStudent:getReadingSkill(self.masteringTeachID) == 1 then
			SendPlayerMessage(characterStudent.playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du kannst '%s' bereits.", self.caption));
			
			return false;
		end
		
		return true;
	end
	

	--check required skills
	return self:checkRequiredSkills(characterStudent, dualPlayerTeach, 1);
end

---Checks the required skills for a teach
function Teach:checkRequiredSkills(characterStudent, dualPlayerTeach, notify)
	--notify determines whether the player gets a message or not
	if not notify then notify = 0; end;
	
	--check if the student has reached the maximum skill value
	
	local studentSkillValue = characterStudent:getSkillValue(self.skills[1][1]);
	local skillIncrementCost = self:getCostForValue(studentSkillValue + 1);
	
	if skillIncrementCost == -1 then
		if notify == 1 then
			SendPlayerMessage(characterStudent.playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du hast den Maximalwert in %s erreicht und kannst diesen vorerst nicht weiter lernen.", self.caption));
		end
		
		return false;
	end
	
	if studentSkillValue >= MAXIMUM_TEACH_SKILL_IN_BOOK and dualPlayerTeach == 0 then
		if characterStudent:getReadingSkill(self.teachID) == 0 then
			if notify == 1 then
				SendPlayerMessage(characterStudent.playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du kannst dir kein weiteres Wissen zu %s am Buch aneignen. Gehe zu deinem Meister.", self.caption));
			end
			
			return false;
		end
	end

	--iterate over all preconditions (= skill & value)
	for k, v in pairs(self.preconditions) do
		if characterStudent:getSkillValue(v[1]) < v[2] then
			if notify == 1 then
				SendPlayerMessage(characterStudent.playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du erfuellst nicht die notwendigen Voraussetzungen, um %s zu erlernen.", self.caption));
			end
			
			return false;
		end
	end
	
	return true;
end

function Teach:checkTeachConditions(characterTeacher, characterStudent)
	for k, v in pairs(self.skills) do		
		if characterTeacher:getSkillValue(v[1]) <= characterStudent:getSkillValue(v[1]) then
			SendPlayerMessage(characterTeacher.playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Du kannst deinem Schueler nichts mehr beibringen. Er ist mindestens genauso gut wie du. (Skill: %s)", instance_TeachSystem.skillMan:getSkillNameByID(v[1])));
      
			SendPlayerMessage(characterStudent.playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Dein Meister kann dir nichts mehr beibringen. Du bist mindestens genauso gut wie er. (Skill: %s)", instance_TeachSystem.skillMan:getSkillNameByID(v[1])));
			return false;
		end
	end	
	
	return true;
end

---Checks whether this instance (as a MenuItem) can be executed
function Teach:checkExecConditions(playerid)
	local characterStudent = getCharacterById(playerid);
		
	if self:checkPreconditions(characterStudent, 0) == false then
	
		return false;
	end
	
	SendPlayerMessage(playerid, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, string.format("Du versuchst etwas ueber %s zu lernen. Das wird circa %d s dauern.", self.caption, self.duration / 1000));
	
	return true;
end

---Executes the teach
function Teach:exec(playerid, secondPlayerID)
	if secondPlayerID then
		self:execDualPlayer(playerid, secondPlayerID);
		
	else
		self:execSinglePlayer(playerid);
	end
end

---Executes a teach from a bookshelf
function Teach:execSinglePlayer(playerid)
	local characterStudent = getCharacterById(playerid);

	if self.masteringTeachID then
		characterStudent:addReadingSkill(self.masteringTeachID);
		
		SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast '%s' gelernt.", self.caption));
		
		LogString("Teach", string.format("%s learned reading (> %d): %s", characterStudent.PlayerName, MAXIMUM_TEACH_SKILL_IN_BOOK, self.caption));
	else
		local studentLevel = self:getPlayerLevel(playerid);
		local studentSkillValue = characterStudent:getSkillValue(self.skills[1][1]);
		local skillIncrementCost = self:getCostForValue(studentSkillValue + 1);
		local newStudentLevel = studentLevel + 1;
		
		if newStudentLevel >= skillIncrementCost then
			--student has enought teaches to increment the desired skill
			for k, v in pairs(self.skills) do
				characterStudent:setSkillValue(v[1], characterStudent:getSkillValue(v[1]) + v[2]);
			end
		
			self:setPlayerLevel(playerid, 0);
		
			SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast deinen Skill '%s' erhoeht.", self.caption));
		else
			--add one teach to the students teach level
			self:setPlayerLevel(playerid, newStudentLevel);
		
			SendPlayerMessage(playerid, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, string.format("Du hast benoetigst noch %d Trainingseinheiten, um '%s' zu erhoehen.", skillIncrementCost - newStudentLevel, self.caption));
		end
		
		LogString("Teach", string.format("%s learned: %s", characterStudent.PlayerName, self.caption));
	end
	
	--set canTeach to false --> player cannot teach again at this day
	if debugMode == 0 then
		characterStudent:setHadTeach();
	end
end

---Executes a teach between a teacher and a student
function Teach:execDualPlayer(teacherid, studentid)
	local characterStudent = getCharacterById(studentid);
	local characterTeacher = getCharacterById(teacherid);
	
	if self:checkPreconditions(characterStudent, 1) == false then
		--the student does not fulfill the requirements for this teach
		SendPlayerMessage(teacherid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Dein Schueler erfuellt nicht die notwendigen Voraussetzungen, ist bereits Meister oder kann gerade nichts lernen.");		

		return;
	end
		
	if self:checkTeachConditions(characterTeacher, characterStudent) == false then
		--either teachers rank is to low or the students skill value is >= the teachers skill value
		-- - - ->TEACHING NOT POSSIBLE
		
		return;
	end
	
	--FROM HERE: teacher is able to teach the student
	
	if self.masteringTeachID then
		characterStudent:addReadingSkill(self.masteringTeachID);
		
		SendPlayerMessage(studentid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast '%s' gelernt.", self.caption));
		SendPlayerMessage(teacherid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast deinem Schueler das Lesen von '%s' beigebracht.", self.caption));
		
		LogString("Teach", string.format("%s taught %s reading (> 20): %s", characterTeacher.PlayerName, characterStudent.PlayerName, self.caption));
	else
		local studentSkillValue = characterStudent:getSkillValue(self.skills[1][1]);
		local studentLevel = self:getPlayerLevel(studentid);
		local newStudentLevel = studentLevel + 1;
	
		local skillIncrementCost = self:getCostForValue(studentSkillValue + 1);
		
		if newStudentLevel >= skillIncrementCost then
			--student has enought teaches to increment the desired skill
		
			for k, v in pairs(self.skills) do
				characterStudent:setSkillValue(v[1], characterStudent:getSkillValue(v[1]) + v[2]);
			end
		
			self:setPlayerLevel(studentid, 0);
		
			SendPlayerMessage(studentid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast deinen Skill '%s' erhoeht.", self.caption));
			SendPlayerMessage(teacherid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast deinem Schueler etwas ueber %s beigebracht. Er ist eine Stufe aufgestiegen.", self.caption));
		else
			--add one teach to the students teach level
			self:setPlayerLevel(studentid, newStudentLevel);
		
			local remainingTeaches = tostring(skillIncrementCost - newStudentLevel);
		
			SendPlayerMessage(studentid, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, string.format("Du benoetigst noch %d Trainingseinheiten, um '%s' zu erhoehen.", remainingTeaches, self.caption));
			SendPlayerMessage(teacherid, COLOR_TRY.r, COLOR_TRY.g, COLOR_TRY.b, string.format("Du hast deinem Schueler etwas ueber %s beigebracht. Er benoetigt noch %d Unterrichtungen, um eine Stufe aufzusteigen.", self.caption, remainingTeaches));
		end
		
		LogString("Teach", string.format("%s taught %s: %s", characterTeacher.PlayerName, characterStudent.PlayerName, self.caption));
	end
	
	--set canTeach to false --> player cannot teach again at this day
	if debugMode == 0 then
		characterStudent:setHadTeach();
	end
end


---Returns the cost (in day-teaches) for a certain skill-value
function Teach:getCostForValue(value)
	for k, v in pairs(self.borders) do
		if v.lower <= value and value <= v.upper then
			return v.cost;
		end
	end
	
	--no valid entry found
	return -1;
end

---Sets the new player level (in day-teaches) in the DB
function Teach:setPlayerLevel(playerid, value)
	runSQL(string.format("INSERT INTO tc_level (account_id, teach_id, level) VALUES (%d, %d, %d) ON DUPLICATE KEY UPDATE level = %d;", getCharacterById(playerid).accountID, self.teachID, value, value));
end

---Returns the current player level (in day-teaches) from the DB
function Teach:getPlayerLevel(playerid)
	local result = runSQL(string.format("SELECT level FROM tc_level WHERE account_id = %d AND teach_id = %d;", getCharacterById(playerid).accountID, self.teachID));
	
	local playerLevel = 0;
	
	if result then
		local row = mysql_fetch_row(result);
		
		if row then
			playerLevel = tonumber(row[1]);
		else
			--create entry
			self:setPlayerLevel(playerid, 0);
		end
	end

	return playerLevel;
end

---Returns the teacher-rank of a teacher from the DB
function Teach:getTeacherRank(playerid)
	local result = runSQL(string.format("SELECT rank FROM tc_ranks WHERE account_id = %d AND cat_id = %d;", getCharacterById(playerid).accountID, self.categoryID));
	
	local teacherRank = -1;
	
	if result then
		local row = mysql_fetch_row(result);
		
		if row then
			teacherRank = tonumber(row[1]);
		else
			--create entry
			runSQL(string.format("INSERT INTO tc_ranks (account_id, cat_id, rank) VALUES (%d, %d, 0);", getCharacterById(playerid).accountID, self.categoryID));
		end
	end
	
	return teacherRank;
end

---Loads the skills from the DB that are tought by this teach
function Teach:loadSkills()
	local result = runSQL(string.format("SELECT skill_id, skill_value FROM tc_teachskills WHERE teach_id = %d;", self.teachID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
		
			table.insert(self.skills, {tonumber(row[1]), tonumber(row[2])});
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the preconditions from the DB that have to be fulfilled by the student to learn this teach
function Teach:loadPreconditions()
	local result = runSQL(string.format("SELECT skill_id, skill_value FROM tc_teachprecond WHERE teach_id = %d;", self.teachID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
		
			table.insert(self.preconditions, {tonumber(row[1]), tonumber(row[2])});
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the borders of a teach from the DB
function Teach:loadBorders()
	local result = runSQL(string.format("SELECT lower, upper, cost FROM tc_borders WHERE teach_id = %d;", self.teachID));

	if result then
		local row = mysql_fetch_row(result);
		
		while row do				
			local border = TeachBorder.new(tonumber(row[1]), tonumber(row[2]), tonumber(row[3]));
			
			table.insert(self.borders, border);
				
			row = mysql_fetch_row(result);
		end
	end
end



---Returns the string representation of the instance
function Teach.toString(self)
	local result = string.format("Teach %s (ID: %d)\nCategory: %s\nBasic: %s\n---SKILLS---\n", self.caption, self.teachID, self.categoryID, self.isBasic);
	
	for k, v in pairs(self.skills) do
		result = string.format("%sID: %s, += %s\n", result, v[1], v[2]);
	end
	
	result = string.format("%s---PRECONDITIONS---\n", result);
	
	for k, v in pairs(self.preconditions) do
		result = string.format("%sID: %s, >= %s\n", result, v[1], v[2]);
	end
	
	result = string.format("%s---BORDERS---\n", result);
	
	for k, v in pairs(self.borders) do
		result = string.format("%s%s\n", result, tostring(v));
	end

	return result;
end

--add toString-function to metatable
Teach.__tostring = Teach.toString;