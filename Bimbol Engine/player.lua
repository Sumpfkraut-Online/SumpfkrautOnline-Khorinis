--[[
	
	Module: player.lua
	Autor: Bimbol
	
	Player System
	
]]--

local ENABLE_SAVE_PLAYER_AFTER_DEATH = false;
local Player = {};

for i = 0, MAX_SLOTS - 1 do
	Player[i] = {};
	
	-- Items
	Player[i].Items = {};
	Player[i].UseItem = { "NULL", 0 };
	
	-- Statistic
	Player[i].Acrobatic = 0
	Player[i].AdditionalVisual = { "Hum_Body_Naked0", 9, "Hum_Head_Pony", 18 };
	Player[i].Color = { 255, 250, 200 };
	Player[i].Dexterity = 10;
	Player[i].ExperienceNextLevel = 500;
	Player[i].Experience = 0;
	Player[i].Fatness = 0.0;
	Player[i].Instance = "PC_HERO";
	Player[i].LearnPoints = 0;
	Player[i].Level = 0;
	Player[i].MagicLevel = 0;
	Player[i].MaxHealth = 40;
	Player[i].MaxMana = 10;
	Player[i].Science = {};
	for j = 0, 6 do
		Player[i].Science[j] = 0;
	end
	Player[i].SkillWeapon = {};
	for j = 0, 3 do
		Player[i].SkillWeapon[j] = 10;
	end
	Player[i].Strength = 10;
	Player[i].World = "NEWWORLD\\NEWWORLD.ZEN";
	Player[i].Walk = "NULL";
	Player[i].WeaponMode = WEAPON_NONE;
	Player[i].Draw = {};
	Player[i].Connect = false;
end

function restart_Player(playerid, disconnect)
	removeAllItem(playerid);
	Player[playerid].UseItem = { "NULL", 0 };
	-- Statistic
	Player[playerid].Acrobatic = 0
	Player[playerid].AdditionalVisual = { "Hum_Body_Naked0", 9, "Hum_Head_Pony", 18 };
	Player[playerid].Color = { 255, 250, 200 };
	Player[playerid].Dexterity = 10;
	Player[playerid].ExperienceNextLevel = 500;
	Player[playerid].Experience = 0;
	Player[playerid].Fatness = 0.0;
	Player[playerid].Instance = "PC_HERO";
	Player[playerid].LearnPoints = 0;
	Player[playerid].Level = 0;
	Player[playerid].MagicLevel = 0;
	Player[playerid].MaxHealth = 40;
	Player[playerid].MaxMana = 10;
	Player[playerid].Science = {};
	for j = 0, 6 do
		Player[playerid].Science[j] = 0;
	end
	Player[playerid].SkillWeapon = {};
	for j = 0, 3 do
		Player[playerid].SkillWeapon[j] = 10;
	end
	Player[playerid].Strength = 10;
	if disconnect then
		Player[playerid].World = "NEWWORLD\\NEWWORLD.ZEN";
	end
	Player[playerid].Walk = "NULL";
	Player[playerid].WeaponMode = WEAPON_NONE;
end

function GetDraw(playerid)
	return Player[playerid].Draw;
end

function SetWorld(playerid, world)
	Player[playerid].World = world;
end

function RestartConnectControl(playerid)
	Player[playerid].Connect = false;
end

-- Set Functions
function SetPlayerAcrobatic(playerid, mode)
	--SetCheatControlAC(playerid, "acrobatic");
	_SetPlayerAcrobatic(playerid, mode);
	Player[playerid].Acrobatic = mode;
end

function SetPlayerAdditionalVisual(playerid, bodyModel, bodyTextureID, headModel, headTextureID)
	_SetPlayerAdditionalVisual(playerid, bodyModel, bodyTextureID, headModel, headTextureID);
	Player[playerid].AdditionalVisual = { bodyModel, bodyTextureID, headModel, headTextureID };
end

function SetPlayerAngle(playerid, angle)
	_SetPlayerAngle(playerid, angle);
end

function SetPlayerColor(playerid, r, g, b)
	_SetPlayerColor(playerid, r, g, b);
	Player[playerid].Color = { r, g, b };
end

function SetPlayerDexterity(playerid, dexterity)
	SetCheatControlAC(playerid, "dexterity");
	_SetPlayerDexterity(playerid, dexterity);
	Player[playerid].Dexterity = dexterity;
end

function SetPlayerExperienceNextLevel(playerid, experience)
	--SetCheatControlAC(playerid, "experience");
	_SetPlayerExperienceNextLevel(playerid, experience);
	Player[playerid].ExperienceNextLevel = experience;
end

function SetPlayerExperience(playerid, experience)
	--SetCheatControlAC(playerid, "next_experience");
	_SetPlayerExperience(playerid, experience);
	Player[playerid].Experience = experience;
end

function SetPlayerFatness(playerid, fatness)
	_SetPlayerFatness(playerid, fatness);
	Player[playerid].Fatness = fatness;
end

function SetPlayerHealth(playerid, health)
	_SetPlayerHealth(playerid, health);
end

function SetPlayerInstance(playerid, instance)
	--SetCheatControlAC(playerid, "instance");
	_SetPlayerInstance(playerid, instance);
	Player[playerid].Instance = instance;
end

function SetPlayerLearnPoints(playerid, learn_point)
	SetCheatControlAC(playerid, "learn_point");
	_SetPlayerLearnPoints(playerid, learn_point);
	Player[playerid].LearnPoints = learn_point;
end

function SetPlayerLevel(playerid, level)
	--SetCheatControlAC(playerid, "level");
	_SetPlayerLevel(playerid, level);
	Player[playerid].Level = level;
end

function SetPlayerMagicLevel(playerid, magic_lvl)
	--SetCheatControlAC(playerid, "magic_lvl");
	_SetPlayerMagicLevel(playerid, magic_lvl);
	Player[playerid].MagicLevel = magic_lvl;
end

function SetPlayerMana(playerid, mana)
	_SetPlayerMana(playerid, mana);
end

function SetPlayerMaxHealth(playerid, max_hp)
	--SetCheatControlAC(playerid, "max_hp");
	_SetPlayerMaxHealth(playerid, max_hp);
	Player[playerid].MaxHealth = max_hp;
end

function SetPlayerMaxMana(playerid, max_mp)
	--SetCheatControlAC(playerid, "max_mp");
	_SetPlayerMaxMana(playerid, max_mp);
	Player[playerid].MaxMana = max_mp;
end

function SetPlayerPos(playerid, x, y, z)
	_SetPlayerPos(playerid, x, y, z);
end

function SetPlayerScience(playerid, science_id, value)
	--SetCheatControlAC(playerid, "science");
	_SetPlayerScience(playerid, science_id, value);
	Player[playerid].Science[science_id] = value;
end

function SetPlayerSkillWeapon(playerid, skill_id, value)
	--SetCheatControlAC(playerid, "skill");
	_SetPlayerSkillWeapon(playerid, skill_id, value);
	Player[playerid].SkillWeapon[skill_id] = value;
end

function SetPlayerStrength(playerid, str)
	SetCheatControlAC(playerid, "strength");
	_SetPlayerStrength(playerid, str);
	Player[playerid].Strength = str;
end

function SetPlayerWalk(playerid, walk)
	_SetPlayerWalk(playerid, walk);
	Player[playerid].Walk = walk;
end

function SetPlayerWeaponMode(playerid, weaponmode)
	_SetPlayerWeaponMode(playerid, weaponmode);
	Player[playerid].WeaponMode = weaponmode;
end

function SetPlayerWorld(playerid, world, checkpoint)
	--SetCheatControlAC(playerid, "world");	
	_SetPlayerWorld(playerid, world, checkpoint);
	Player[playerid].World = world;
end

function SetPlayerGold(playerid, gold)
	SetCheatControlAC(playerid, "gold");
	_SetPlayerGold(playerid, gold);
end

-- Get Functions
function GetPlayerAcrobatic(playerid)
	return Player[playerid].Acrobatic;
end

function GetPlayerAdditionalVisual(playerid)
	return Player[playerid].AdditionalVisual[1], Player[playerid].AdditionalVisual[2], Player[playerid].AdditionalVisual[3], Player[playerid].AdditionalVisual[4];
end

function GetPlayerAngle(playerid)
	return _GetPlayerAngle(playerid);
end

function GetPlayerColor(playerid)
	return Player[playerid].Color[1], Player[playerid].Color[2], Player[playerid].Color[3];
end

function GetPlayerDexterity(playerid)
	return Player[playerid].Dexterity;
end

function GetPlayerExperienceNextLevel(playerid)
	return Player[playerid].ExperienceNextLevel;
end

function GetPlayerExperience(playerid)
	return Player[playerid].Experience;
end

function GetPlayerFatness(playerid)
	return Player[playerid].Fatness;
end

function GetPlayerHealth(playerid)
	return _GetPlayerHealth(playerid);
end

function GetPlayerInstance(playerid)
	return Player[playerid].Instance;
end

function GetPlayerLearnPoints(playerid)
	return Player[playerid].LearnPoints;
end

function GetPlayerLevel(playerid)
	return Player[playerid].Level;
end

function GetPlayerMagicLevel(playerid)
	return Player[playerid].MagicLevel;
end

function GetPlayerMana(playerid)
	return _GetPlayerMana(playerid);
end

function GetPlayerMaxHealth(playerid)
	return Player[playerid].MaxHealth;
end

function GetPlayerMaxMana(playerid)
	return Player[playerid].MaxMana;
end

function GetPlayerPos(playerid)
	return _GetPlayerPos(playerid);
end

function GetPlayerScience(playerid, science_id)
	return Player[playerid].Science[science_id];
end

function GetPlayerSkillWeapon(playerid, skill_id)
	return Player[playerid].SkillWeapon[skill_id];
end

function GetPlayerStrength(playerid)
	return Player[playerid].Strength;
end

function GetPlayerWalk(playerid)
	return Player[playerid].Walk;
end

function GetPlayerWeaponMode(playerid)
	return Player[playerid].WeaponMode;
end

function GetPlayerWorld(playerid)
	return Player[playerid].World;
end

function GetPlayerGold(playerid)
	local slot, amount = checkPlayerItem(playerid, "ITMI_GOLD");
	if slot then
		return amount;
	end
		return 0;
end

-- Another Function
function GiveItem(playerid, instance, amount)
	if string.lower(instance) == "itmi_gold" then
		SetCheatControlAC(playerid, "gold");
	end
	_GiveItem(playerid, instance, amount);
	giveItem(playerid, instance, amount);
end

function RemoveItem(playerid, instance, amount)
	_RemoveItem(playerid, instance, amount);
	removeItem(playerid, instance, amount);
end

function EquipItem(playerid, instance_item)
	_EquipItem(playerid, instance_item);
	giveItem(playerid, instance_item, 1);
end

function EquipMeleeWeapon(playerid, instance_meleeweapon)
	_EquipMeleeWeapon(playerid, instance_meleeweapon);
	giveItem(playerid, instance_meleeweapon, 1);
end

function EquipRangedWeapon(playerid, instance_rangedweapon)
	_EquipRangedWeapon(playerid, instance_rangedweapon);
	giveItem(playerid, instance_rangedweapon, 1);
end

function EquipArmor(playerid, instance_armor)
	_EquipArmor(playerid, instance_armor);
	giveItem(playerid, instance_armor, 1);
end

function ClearInventory(playerid)
	_ClearInventory(playerid);
	removeAllItem(playerid);
end

function DropItem(playerid, item_instance, amount)
	_DropItem(playerid, item_instance, amount);
	removeItem(playerid, item_instance, amount);
end

function DropItemBySlot(playerid, slot)
	_DropItemBySlot(playerid, slot);
	for i,k in ipairs(Player[playerid].Items) do
		if i == slot + 1 then
			table.remove(Player[playerid].Items, i);
			return;
		end
	end
end

-- Engine func
function setPlayerWeaponMode(playerid, weaponmode)
	if weaponmode == WEAPON_NONE
	and (Player[playerid].WeaponMode == WEAPON_BOW
	or Player[playerid].WeaponMode == WEAPON_CBOW)
	then
		removeBoltandArrow(playerid);
	end
	Player[playerid].WeaponMode = weaponmode;
end

-- Items
function enableSavePlayerAfterDeath(value)
	if type(value) == "boolean" then
		ENABLE_SAVE_PLAYER_AFTER_DEATH = value;
	end
end

function checkPlayerItem(playerid, instance)
	if playerid and instance then
		instance = string.upper(instance);
		for i, k in ipairs(Player[playerid].Items) do
			if k.instance == instance then
				return i, k.amount;
			end
		end
	end
		return false;
end

function checkPlayerSlot(playerid, slot)
	if playerid and slot then
		for i,k in ipairs(Player[playerid].Items) do
			if i == slot then
				return k.instance, k.amount;
			end
		end
	end
		return false;
end

function getPlayerItems(playerid)
	if #Player[playerid].Items > 0 then
		return Player[playerid].Items;
	end
		return false;
end

function givePlayerAllItems(playerid)
	if ENABLE_SAVE_PLAYER_AFTER_DEATH then
		_SetPlayerInstance(playerid, Player[playerid].Instance);
		_ClearInventory(playerid);
		_SetPlayerAcrobatic(playerid, Player[playerid].Acrobatic);
		_SetPlayerAdditionalVisual(playerid, Player[playerid].AdditionalVisual[1], Player[playerid].AdditionalVisual[2], Player[playerid].AdditionalVisual[3], Player[playerid].AdditionalVisual[4]);
		_SetPlayerDexterity(playerid, Player[playerid].Dexterity);
		_SetPlayerExperienceNextLevel(playerid, Player[playerid].ExperienceNextLevel);
		_SetPlayerExperience(playerid, Player[playerid].Experience);
		_SetPlayerFatness(playerid, Player[playerid].Fatness);
		_SetPlayerLearnPoints(playerid, Player[playerid].LearnPoints);
		_SetPlayerLevel(playerid, Player[playerid].Level);
		_SetPlayerMagicLevel(playerid, Player[playerid].MagicLevel);
		_SetPlayerMaxHealth(playerid, Player[playerid].MaxHealth);
		_SetPlayerHealth(playerid, Player[playerid].MaxHealth);
		_SetPlayerMaxMana(playerid, Player[playerid].MaxMana);
		_SetPlayerMana(playerid, Player[playerid].MaxMana);
		for j = 0, 6 do
			_SetPlayerScience(playerid, j, Player[playerid].Science[j]);
		end
		for j = 0, 3 do
			_SetPlayerSkillWeapon(playerid, j, Player[playerid].SkillWeapon[j]);
		end
		_SetPlayerStrength(playerid, Player[playerid].Strength);
		_SetPlayerWalk(playerid, Player[playerid].Walk);
		for i in pairs(Player[playerid].Items) do
			_GiveItem(playerid, Player[playerid].Items[i].instance, Player[playerid].Items[i].amount);
		end	
	else
		if Player[playerid].Connect then
			restart_Player(playerid);
		end
		Player[playerid].Connect = true;
	end
	Player[playerid].WeaponMode = WEAPON_NONE;
end

function giveItem(playerid, instance, amount)
	local slot = checkPlayerItem(playerid, instance);
	if slot then
		Player[playerid].Items[slot].amount = Player[playerid].Items[slot].amount + amount;
	else
		table.insert(Player[playerid].Items, { instance = string.upper(instance), amount = amount });
	end
end

function removeItem(playerid, instance, amount)
	local slot = checkPlayerItem(playerid, instance);
	if slot then
		Player[playerid].Items[slot].amount = Player[playerid].Items[slot].amount - amount;
		if Player[playerid].Items[slot].amount <= 0 then
			table.remove(Player[playerid].Items, slot);
		end
	end
end

function removeAllItem(playerid)
	for i = 1, #Player[playerid].Items do
		table.remove(Player[playerid].Items, 1);
	end
end

function removeDisposableItem(playerid, instance, amount)
	if instance ~= "NULL" then
		Player[playerid].UseItem = { instance, amount };
	else
		local anim = GetPlayerAnimationID(playerid);
		if anim == 994 or
		   anim == 999 or
		   anim == 764 or
		   anim == 768 or
		   anim == 968
		then
			removeItem(playerid, Player[playerid].UseItem[1], Player[playerid].UseItem[2]);
			OnPlayerConsumed(playerid, Player[playerid].UseItem[1], Player[playerid].UseItem[2]);
		end
	end
end

function removeBoltandArrow(playerid)
	if Player[playerid].WeaponMode == WEAPON_BOW then
		removeItem(playerid, "ITRW_ARROW", 1);
	elseif Player[playerid].WeaponMode == WEAPON_CBOW then
		removeItem(playerid, "ITRW_BOLT", 1);
	end
end

-- Callback
function OnPlayerConsumed(playerid, item_instance, amount)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");