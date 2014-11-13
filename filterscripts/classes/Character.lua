print("Loading Character.lua");

Character = {};
Character.__index = Character;

PlayerCharacters = {};

---Creates an instance of Character
function Character:new(playerid, accountID)
	local newCharacter = {};
	
	setmetatable(newCharacter, Character);
	
	--the players account id
	newCharacter.accountID = accountID;
	
	--the players rights (0 = standard, 1 = gamemaster, 2 = admin)
	newCharacter.rights = newCharacter:loadRights();
	
	--the players id
	newCharacter.playerid = playerid;
	
	--the players name
	newCharacter.PlayerName = GetPlayerName(playerid);
	
	--table for all skills of this character
	newCharacter.skills = {};
	
	--table for all teacher ranks of this character
	newCharacter.teacherRanks = {};
	
	--table for all reading skills a character has
	newCharacter.readingSkills = {};
	
	--records the time a player is online on a day (for teachsystem)
	newCharacter.dayOnlineTime = 0;
	
	--determines whether a character can teach or not (only 1x / day, after at least 60 minutes online-time)
	newCharacter.canTeach = false;
	
	--sets the players voice volume (0 = whisper, 1 = normal, 2 = shout)
	--ATTENTION: This value has no need to be persistent -> no database entry!
	newCharacter.voiceVolume = 1;
	
	--table which contains the characters friends (-> so the player can see other player's names)
	newCharacter.friends = {};
	
	--contains the hunger of the character
	newCharacter.hungerLevel = 49;
	
	--contains the number of life-points that are regenerated in a hunger-system-interval
	newCharacter.hungerRegeneration = 0;
	
	--contains the time when the player was plundered last (in seconds)
	newCharacter.lastPlunderTime = 0;
	
	--contains a draw instance that shows the focused player's name
	newCharacter.otherPlayerNameDraw = nil;
  
  --contains the amount of received warnings
  newCharacter.warnings = 0;
	
	newCharacter.playerLastWeaponMode = WEAPON_NONE;
  
	local data = self:getData(accountID);
	
	local row = mysql_fetch_row (data)
	
	if row then
    local loadedWorld = row[31]:gsub("/", "\\");
    local playerWorld = GetPlayerWorld(playerid);
    
    if loadedWorld == "NEWWORLD\\NEWWORLD.ZEN" then
      if playerWorld ~= loadedWorld then
        SetPlayerWorld(playerid, "NEWWORLD\\NEWWORLD.ZEN", "MAGECAVE");
      end
	  
    elseif loadedWorld == "NEW\\MM_WORLD.ZEN" then
      if playerWorld ~= loadedWorld then
        SetPlayerWorld(playerid, "NEW\\MM_WORLD.ZEN", "TEST");
      end
      
    elseif loadedWorld == "DRAGONISLAND\\DRAGONISLAND.ZEN" then
      if playerWorld ~= loadedWorld then
        SetPlayerWorld(playerid, "DRAGONISLAND\\DRAGONISLAND.ZEN", "SHIP");
      end
      
    elseif loadedWorld == "OLDWORLD\\OLDWORLD.ZEN" then
      if playerWorld ~= loadedWorld then
        SetPlayerWorld(playerid, "OLDWORLD\\OLDWORLD.ZEN", "WP_INTRO13");
      end
      
    else
      SetPlayerWorld(playerid, "OLDWORLD\\OLDWORLD.ZEN", "WP_INTRO13");
			LogString("Account", "Error while loading charakter data : Unknow world (not spawn waypoint in script)");	
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Der letzte Standort deines Charakters konnte nicht wieder hergestellt werden (Unbekannte Map).");	
    end
    
    SetPlayerAngle(playerid, tonumber(row[3]));
		SetPlayerPos(playerid, tonumber(row[4]), tonumber(row[5]), tonumber(row[6]));
    
		SetPlayerAdditionalVisual(playerid,  row[7],  tonumber(row[8]),  row[9],  tonumber(row[10]));
		SetPlayerFatness(playerid,  tonumber(row[12]))
		SetPlayerColor(playerid, tonumber(row[13]), tonumber(row[14]), tonumber(row[15]));
    
    
    newCharacter.equippedArmor = row[16];
    newCharacter.equippedMeleeWeapon= row[17];
    newCharacter.equippedRangeWeapon = row[18];
    
    --equip items after loading
    SetTimerEx("equipCharacter", 500, 0, playerid);
    
		-----------------------------
		SetPlayerExperience(playerid, tonumber(row[19]));
		SetPlayerLevel(playerid, tonumber(row[20]));
		SetPlayerExperienceNextLevel(playerid, tonumber(row[21]));
		SetPlayerLearnPoints(playerid, tonumber(row[22]));
		SetPlayerHealth(playerid, tonumber(row[23]));
		SetPlayerMana(playerid, tonumber(row[24]));
		SetPlayerMagicLevel(playerid, tonumber(row[25]));
		SetPlayerAcrobatic(playerid, tonumber(row[26]));

		EquipItem(playerid, row[27]);
		EquipItem(playerid, row[28]);
		SetPlayerWalk(playerid,  row[29]);
		SetPlayerWeaponMode(playerid, tonumber(row[30]));

		--load hunger level
		newCharacter.hungerLevel = tonumber(row[36]);
		
		--set health regeneration value
		if newCharacter.hungerLevel < HUNGER_BORDER_DECREMENT then
			newCharacter.hungerRegeneration = -1;
		elseif newCharacter.hungerLevel < HUNGER_BORDER_INCREMENT1 then
			newCharacter.hungerRegeneration = 0;
		elseif newCharacter.hungerLevel < HUNGER_BORDER_INCREMENT2 then
			newCharacter.hungerRegeneration = 1;
		else
			newCharacter.hungerRegeneration = 2;
		end
		
		--load last plunder time
		newCharacter.lastPlunderTime = tonumber(row[37]);
    
    --load warnings
    newCharacter.warnings = tonumber(row[38]);

		mysql_free_result(data)  -- free the SQL-Result
		
		LogString("Account", string.format("Loaded character data for playerID: %d, accountID: %d", playerid, accountID));
    
		--load the character's skills
		newCharacter:loadSkills();
	
	--load the character's teacher ranks
		newCharacter:loadTeacherRanks();

	--load the character's reading skills
		newCharacter:loadReadingSkills();
	
	--load the character's day-online-time
		newCharacter:loadDayOnlineTime();
	
	--load the character's friends
		newCharacter:loadFriends();
    
	else
		LogString("Account", string.format("Creating new account for playerID: %d, accountID: %d", playerid, accountID));	
    
		newCharacter:CharToDB();
    
		CharEditor:new(playerid);
	end
	

	PlayerCharacters[playerid] = newCharacter;
  
	return newCharacter;
end

function equipCharacter(playerid)
    local character = getCharacterById(playerid);
    
    EquipItem(playerid, character.equippedArmor);
		EquipItem(playerid, character.equippedMeleeWeapon);
		EquipItem(playerid, character.equippedRangeWeapon);
end


function  Character:CharToDB()
	if not self then return end;
    
	local playerid = self.playerid;
	local accountID = self.accountID;

	--Alle Werte Abrufen und in Variablen speichern.
	local PlayerName = escapeString(self.PlayerName);
	local PlayerAngle = GetPlayerAngle(playerid);
	local PlayerPosX, PlayerPosY, PlayerPosZ = GetPlayerPos(playerid);
	local BodyModel, BodyTextureID, HeadModel, HeadTextureID = GetPlayerAdditionalVisual(playerid);
	print(BodyModel.."  "..HeadModel)
	local PlayerAnimationID = GetPlayerAnimationID(playerid);
	local PlayerFatness = GetPlayerFatness(playerid);
	local PlayerColorRed, PlayerColorGreen, PlayerColorBlue = GetPlayerColor(playerid)
	local EquippedArmor = GetEquippedArmor(playerid);
	local EquippedMeleeWeapon = GetEquippedMeleeWeapon(playerid);
	local EquippedRangedWeapon = GetEquippedRangedWeapon(playerid);
	local PlayerExperience = GetPlayerExperience(playerid);
	local PlayerLevel = GetPlayerLevel(playerid);
	local PlayerExperienceNextLevel = GetPlayerExperienceNextLevel(playerid);
	local PlayerLearnPoints = GetPlayerLearnPoints(playerid);
	local PlayerHealth = GetPlayerHealth(playerid);
	local PlayerMana = GetPlayerMana(playerid);
	local PlayerMagicLevel = GetPlayerMagicLevel(playerid);
	local PlayerAcrobatic = GetPlayerAcrobatic(playerid);
	local LeftHand = GetLeftHand(playerid);
	local RightHand = GetRightHand(playerid);
	local PlayerWalk = GetPlayerWalk(playerid);
	local PlayerWeaponMode = GetPlayerWeaponMode(playerid);
	local PlayerWorld = GetPlayerWorld(playerid):gsub("\\", "/");
	local isNPC = IsNPC(playerid);
	
	local playerIP = GetPlayerIP(playerid);
	if not playerIP then playerIP = "0"; end; --case: bot
	
	local MacAddress = GetMacAddress(playerid);
	if not MacAddress then MacAddress = "0"; end; --case: bot
    
  local sql = string.format("INSERT INTO `character` (PlayerName, PlayerAngle, PlayerPosX, PlayerPosY, PlayerPosZ, BodyModel, BodyTextureID, HeadModel, HeadTextureID, PlayerAnimationID, PlayerFatness, PlayerColorRed, PlayerColorGreen, PlayerColorBlue, EquippedArmor, EquippedMeleeWeapon, EquippedRangeWeapon, PlayerExperience, PlayerLevel, PlayerExperienceNextLevel, PlayerLearnPoints, PlayerHealth, PlayerMana, PlayerMagicLevel, PlayerAcrobatic, LeftHand, RightHand, PlayerWalk, PlayerWeaponMode, PlayerWorld, IsNPC, PlayerIP, MacAddress, accountID, hungerLevel, lastPlunderTime, warnings) VALUES ('%s', %d, %d, %d, %d, '%s', %d, '%s', %d, %d, %d, %d, %d, %d, '%s', '%s', '%s', %d, %d, %d, %d, %d, %d, %d, %d, '%s', '%s', '%s', %d, '%s', %d, '%s', '%s', %d, %d, %d, %d) ON DUPLICATE KEY UPDATE PlayerAngle = %d, PlayerPosX = %d, PlayerPosY = %d, PlayerPosZ = %d, EquippedArmor = '%s', EquippedMeleeWeapon = '%s', EquippedRangeWeapon = '%s', PlayerExperience = %d, PlayerLevel = %d, PlayerExperienceNextLevel = %d, PlayerLearnPoints = %d, PlayerHealth = %d, PlayerMana = %d, PlayerMagicLevel = %d, PlayerAcrobatic = %d, LeftHand = '%s', RightHand = '%s', PlayerWalk = '%s', PlayerWeaponMode = %d, PlayerWorld = '%s', PlayerIP = '%s', MacAddress = '%s', hungerLevel = %d, lastPlunderTime = %d, warnings = %d, BodyModel = '%s', BodyTextureID = %d, HeadModel = '%s', HeadTextureID = %d, PlayerFatness = %d;", PlayerName, PlayerAngle, PlayerPosX, PlayerPosY, PlayerPosZ, BodyModel, BodyTextureID, HeadModel, HeadTextureID, PlayerAnimationID, PlayerFatness, PlayerColorRed, PlayerColorGreen, PlayerColorBlue, EquippedArmor, EquippedMeleeWeapon, EquippedRangedWeapon, PlayerExperience, PlayerLevel, PlayerExperienceNextLevel, PlayerLearnPoints, PlayerHealth, PlayerMana, PlayerMagicLevel, PlayerAcrobatic, LeftHand, RightHand, PlayerWalk, PlayerWeaponMode, PlayerWorld, isNPC, playerIP, MacAddress, accountID, self.hungerLevel, self.lastPlunderTime, self.warnings, PlayerAngle, PlayerPosX, PlayerPosY, PlayerPosZ, EquippedArmor, EquippedMeleeWeapon, EquippedRangedWeapon, PlayerExperience, PlayerLevel, PlayerExperienceNextLevel, PlayerLearnPoints, PlayerHealth, PlayerMana, PlayerMagicLevel, PlayerAcrobatic, LeftHand, RightHand, PlayerWalk, PlayerWeaponMode, PlayerWorld, playerIP, MacAddress, self.hungerLevel, self.lastPlunderTime, self.warnings, BodyModel, BodyTextureID, HeadModel, HeadTextureID, PlayerFatness);

  runSQL(sql);
end

function Character:getData(accountID)
	local result = runSQL(string.format("SELECT char_id, PlayerName, PlayerAngle, PlayerPosX, PlayerPosY, PlayerPosZ, BodyModel, BodyTextureID, HeadModel, HeadTextureID, PlayerAnimationID, PlayerFatness, PlayerColorRed, PlayerColorGreen, PlayerColorBlue, EquippedArmor, EquippedMeleeWeapon, EquippedRangeWeapon, PlayerExperience, PlayerLevel, PlayerExperienceNextLevel, PlayerLearnPoints, PlayerHealth, PlayerMana, PlayerMagicLevel, PlayerAcrobatic, LeftHand, RightHand, PlayerWalk, PlayerWeaponMode, PlayerWorld, IsNPC, PlayerIP, MacAddress, accountID, hungerLevel, lastPlunderTime, warnings FROM `character` WHERE accountID = %s;", accountID));
  
  --[[ INFORMATION
1 char_id
2 PlayerName
3 PlayerAngle
4 PlayerPosX
5 PlayerPosY
6 PlayerPosZ
7 BodyModel
8 BodyTextureID
9 HeadModel
10 HeadTextureID
11 PlayerAnimationID
12 PlayerFatness
13 PlayerColorRed
14 PlayerColorGreen
15 PlayerColorBlue
16 EquippedArmor
17 EquippedMeleeWeapon
18 EquippedRangeWeapon
19 PlayerExperience
20 PlayerLevel
21 PlayerExperienceNextLevel
22 PlayerLearnPoints
23 PlayerHealth
24 PlayerMana
25 PlayerMagicLevel
26 PlayerAcrobatic
27 LeftHand
28 RightHand
29 PlayerWalk
30 PlayerWeaponMode
31 PlayerWorld
32 IsNPC
33 PlayerIP
34 MacAddress
35 accountID
36 hungerLevel
37 lastPlunderTime
38 warnings
  ]]
	
	return result;	
end


---Loads the character's skills
function Character:loadSkills()
	local result = runSQL(string.format("SELECT skill_id, skill_value FROM char_skill WHERE account_id = %s;", self.accountID));
	
	if result then
		local row = mysql_fetch_row(result);
		local skill_id;
		local skill_value;
		
		while row do
			skill_id = tonumber(row[1]);
			skill_value = tonumber(row[2]);
			
			--insert skill
			self.skills[skill_id] = skill_value;
			
			if skill_id == 1 then
				--maximale Lebensenergie
				SetPlayerMaxHealth(self.playerid, skill_value);
			elseif skill_id == 2 then
				--maximales Mana
				SetPlayerMaxMana(self.playerid, skill_value);
			elseif skill_id == 3 then
				--Staerke
				SetPlayerStrength(self.playerid, skill_value);
			elseif skill_id == 4 then
				--Geschick
				SetPlayerDexterity(self.playerid, skill_value);
			elseif skill_id == 5 then
				--Einhand
				SetPlayerSkillWeapon(self.playerid, 0, skill_value);
			elseif skill_id == 6 then
				--Zweihand
				SetPlayerSkillWeapon(self.playerid, 1, skill_value);
			elseif skill_id == 7 then
				--Bogen
				SetPlayerSkillWeapon(self.playerid, 2, skill_value);
			elseif skill_id == 8 then
				--Armbrust
				SetPlayerSkillWeapon(self.playerid, 3, skill_value);
			end
			
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the character's teacher ranks
function Character:loadTeacherRanks()
	local result = runSQL(string.format("SELECT cat_id, rank FROM tc_ranks WHERE account_id = %s;", self.accountID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
		
			--insert rank
			self.teacherRanks[tonumber(row[1])] = tonumber(row[2]);
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the character's reading skills
function Character:loadReadingSkills()
	local result = runSQL(string.format("SELECT teach_id FROM char_readingability WHERE account_id = %s;", self.accountID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
		
			--insert reading skill
			self.readingSkills[tonumber(row[1])] = 1;
				
			row = mysql_fetch_row(result);
		end
	end
end

---Loads the characters day-online-time from the database
--ATTENTION: The update to the database is executed each 5 min by Timerecorder.lua
function Character:loadDayOnlineTime()
	local result = runSQL(string.format("SELECT day, time, hadTeach FROM char_onlinetime WHERE account_id = %s;", self.accountID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		if row then
			if tonumber(row[1]) == tonumber(os.date("%d")) then
				--the player was already online today
				self.dayOnlineTime = tonumber(row[2]);
				
				if self.dayOnlineTime >= DAY_ONLINE_TIME_FOR_TEACH then
					local hadTeach = false;
					if tonumber(row[3]) == 1 then hadTeach = true; end;
					
					self.canTeach = not hadTeach;
				else
					self.canTeach = false;
				end
			else
				--the player has not been online today
				self.dayOnlineTime = 0;
				self.canTeach = false;
				
				--reset db entry
				self:updateDayOnlineTime(true, 0);
			end
			
			return;
		end
	end
	
	--handle error case (e.g. no entry in db)
	self.dayOnlineTime = 0;
	self.canTeach = false;
end


---Load the character's friends from DB
function Character:loadFriends()
	local result = runSQL(string.format("SELECT account_id_p2 FROM char_friends WHERE account_id_p1 = %s;", self.accountID ));
	
	if result then
		local row = mysql_fetch_row(result);
		
		while row do
			self.friends[row[1]] = 1;
			
			row = mysql_fetch_row(result);
		end
	end
end

---Add a character to this characters friend-list
--ATTENTION: This does not mean: "add a character to my friends-list" BUT "add me to a foreign character's friends-list"
function Character:addFriend(playerID)
	local character = getCharacterById(playerID);
	
	if not character then return; end;

	if not self.friends[character.accountID] then
		--add id to database
		runSQL(string.format("INSERT INTO char_friends (account_id_p1, account_id_p2) VALUES (%s, %s);", self.accountID, character.accountID));
		
		--add id to memory
		self.friends[character.accountID] = 1;
	end
end

---Returns whether the character knows the player with the given id
function Character:isFriend(playerID)
	local character = getCharacterById(playerID);
	
	if not character then return; end;
	
	if self.friends[character.accountID] then
		return true;
	end
	
	return false;
end

---Applies the charcters hunger and increments or decrements his health
function Character:applyHunger()
	if self.hungerRegeneration == 0 then return; end;
	
	local currentHealth = GetPlayerHealth(self.playerid);
	local maxHealth = GetPlayerMaxHealth(self.playerid);
	
	currentHealth = currentHealth + self.hungerRegeneration;
		
	if currentHealth < 1 then currentHealth = 1; end;
	if currentHealth > maxHealth then currentHealth = maxHealth; end;
	
	SetPlayerHealth(self.playerid, currentHealth);
end

---Updates the online time of a character
function Character:updateDayOnlineTime(newDay, interval)
	if newDay == true then
		self.dayOnlineTime = 0;
	else
		self.dayOnlineTime = self.dayOnlineTime + interval;
	end
	
	if self.dayOnlineTime >= DAY_ONLINE_TIME_FOR_TEACH then
		self.canTeach = true;
		SendPlayerMessage(self.playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Hmm.. Ich habe Lust auf ein kleines Training .");
	end
	
	runSQL(string.format("REPLACE INTO char_onlinetime (account_id, day, time, hadTeach) VALUES (%s, %s, %s, 0);", self.accountID, os.date("%d"), self.dayOnlineTime));
end

---Set the can teach value, that a character had his/her day-teach already
function Character:setHadTeach()
	self.canTeach = false;
	
	runSQL(string.format("INSERT INTO char_onlinetime (account_id, day, time, hadTeach) VALUES (%s, %s, 0, 1) ON DUPLICATE KEY UPDATE hadTeach = 1;", self.accountID, os.date("%d")));
end

---Returns a characters skill value for a certain skill id
function Character:getSkillValue(skillID)
	if self.skills[skillID] then
		return self.skills[skillID];
	else
		--skill is not trained.. so it has to be 0
		self.skills[skillID] = 0;
		self:setSkillValue(skillID, 0);
		
		return 0;
	end
end

---Sets the skill value for a certain skill
function Character:setSkillValue(skillID, value)
	--update skill value in database
	runSQL(string.format("INSERT INTO char_skill (account_id, skill_id, skill_value) VALUES (%s, %s, %s) ON DUPLICATE KEY UPDATE skill_value = %s;", self.accountID, skillID, value, value));

	--update skill value ingame
	if skillID <= 8 then
		if skillID == 1 then
			--maximale Lebensenergie
			SetPlayerMaxHealth__(self.playerid, value);
		elseif skillID == 2 then
			--maximales Mana
			SetPlayerMaxMana__(self.playerid, value);
		elseif skillID == 3 then
			--Staerke
			SetPlayerStrength__(self.playerid, value);
		elseif skillID == 4 then
			--Geschick
			SetPlayerDexterity__(self.playerid, value);
		elseif skillID == 5 then
			--Einhand
			SetPlayerSkillWeapon__(self.playerid, 0, value);
		elseif skillID == 6 then
			--Zweihand
			SetPlayerSkillWeapon__(self.playerid, 1, value);
		elseif skillID == 7 then
			--Bogen
			SetPlayerSkillWeapon__(self.playerid, 2, value);
		elseif skillID == 8 then
			--Armbrust
			SetPlayerSkillWeapon__(self.playerid, 3, value);
		end
	end		
	
	--unsere eigenen Skills (Kurzschwert, ...)
	self.skills[skillID] = value;
end

---Sets the teacher-rank of a character for a certain category
function Character:setTeacherRank(catID, value)
	--update skill value in database
	runSQL(string.format("INSERT INTO tc_ranks (account_id, rank, cat_id) VALUES (%s, %s, %s) ON DUPLICATE KEY UPDATE rank = %s;", self.accountID, value, catID, value));
	
	--update skill value ingame
	self.teacherRanks[catID] = value;
end

---Returns the teacher-rank of a character for a certain category
function Character:getTeacherRank(catID)
	if self.teacherRanks[catID] then
		return self.teacherRanks[catID];
	else
		--teacher rank does not exist -> has to be 0
	
		self.teacherRanks[catID] = 0;
		self:setTeacherRank(catID, 0);
		
		return 0;
	end
end



---Adds a book-teach that can be read to the character
function Character:addReadingSkill(teachID)
	runSQL(string.format("INSERT INTO char_readingability (account_id, teach_id) VALUES (%s, %s);", self.accountID, teachID));

	self.readingSkills[teachID] = 1;
end

---Checks whether a character can read an advanced book
function Character:getReadingSkill(teachID)
	if self.readingSkills[teachID] then
		return 1;
	end
	
	return 0;

end


---Returns a string that contains all skills (and values) of this character
function Character:skillsToString()
	local result = "";
	local skillCaption;
	
	for k, v in pairs(self.skills) do
		skillCaption = instance_TeachSystem.skillMan:getSkillNameByID(k);
		
		
		if skillCaption then
			result = string.format("%s%s = %s, ", result, skillCaption, tostring(v));
		end
	end

	return result;
end

---Loads the characters rights from DB
function Character:loadRights()
	
	local result = runSQL(string.format("SELECT rights FROM account WHERE accountID = %s;", self.accountID));
	
	if result then
		local row = mysql_fetch_row(result);
		
		if row then
			if row[1] == "gm" then
				return 1;
			elseif row[1] == "adm" then
				return 2;
			else
				--rights are standard (std)
				return 0;
			end
		end
	end
	
	--return standard rights
	return 0;
end

---Gives the player a new right
function Character:setRights(right)
	--get the string representation of the right value
	local rightStr = "std";
	
	if right == 1 then
		rightStr = "gm";
	elseif right == 2 then
		rightStr = "adm";
	end
	
	runSQL(string.format("UPDATE account SET rights = '%s' WHERE accountID = %s;", rightStr, self.accountID));
	
	LogString("UserRights", string.format("Set rights of %s from %s to %s", self.PlayerName, self.rights, right));
	
	self.rights = right;
end


---Adds a warning to the character
function Character:addWarning()
  self.warnings = self.warnings + 1;
  
  runSQL(string.format("UPDATE `character` SET warnings = %d WHERE accountID = %d;", self.warnings, self.accountID));
end

function Character:loadWarnings()
  local result = runSQL(string.format("SELECT warnings FROM `character` WHERE accountID = %d;", self.accountID));
    
  if result then
    local row = mysql_fetch_row(result);
		
    if row then
      self.warnings = tonumber(row[1]);
    end
  end
end



---Returns the character that belongs to the given playerid
function getCharacterById(playerid)
	if playerid then
		return PlayerCharacters[playerid];
	end
end


---Reacts on Disconnect-Try of a player
function CharacterOnPlayerDisconnect(playerid)
	if playerid then
		local character = PlayerCharacters[playerid];
		
		if character then 
			character:CharToDB();
		end
		
		PlayerCharacters[playerid] = nil;
	end
end




--OVERWRITE METHODS
function SetPlayerMaxHealth_(playerid, value)
	--write changes into DB
	local character = getCharacterById(playerid);
	if character then
		character:setSkillValue(1, value);
	else
		--call 'old' function
		SetPlayerMaxHealth__(playerid, value);
	end
end

SetPlayerMaxHealth__ = SetPlayerMaxHealth;
SetPlayerMaxHealth = SetPlayerMaxHealth_;



function SetPlayerMaxMana_(playerid, value)
	--write changes into DB
	local character = getCharacterById(playerid);
	if character then
		character:setSkillValue(2, value);
	else
		--call 'old' function
		SetPlayerMaxMana__(playerid, value);
	end
end

SetPlayerMaxMana__ = SetPlayerMaxMana;
SetPlayerMaxMana = SetPlayerMaxMana_;



function SetPlayerStrength_(playerid, value)
	--write changes into DB
	local character = getCharacterById(playerid);
	if character then
		character:setSkillValue(3, value);
	else
		--call 'old' function
		SetPlayerStrength__(playerid, value);
	end
end

SetPlayerStrength__ = SetPlayerStrength;
SetPlayerStrength = SetPlayerStrength_;



function SetPlayerDexterity_(playerid, value)
	--write changes into DB
	local character = getCharacterById(playerid);
	if character then
		character:setSkillValue(4, value);
	else
		--call 'old' function
		SetPlayerDexterity__(playerid, value);
	end
end

SetPlayerDexterity__ = SetPlayerDexterity;
SetPlayerDexterity = SetPlayerDexterity_;



function SetPlayerSkillWeapon_(playerid, skillID, value)
	--write changes into DB
	local character = getCharacterById(playerid);
	if character then
		character:setSkillValue(5 + skillID, value);
	else
		--call 'old' function
		SetPlayerSkillWeapon__(playerid, skillID, value);
	end
end

SetPlayerSkillWeapon__ = SetPlayerSkillWeapon;
SetPlayerSkillWeapon = SetPlayerSkillWeapon_;