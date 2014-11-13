--[[
	
	Module: require.lua
	Autor: Bimbol
	
	Requires
	
]]--

MAX_PLAYERS = GetMaxPlayers();
MAX_SLOTS = GetMaxSlots();

-- Hooks
require "Bimbol Engine/hooks"
-- Function
require "Bimbol Engine/Function/event-handler"
require "Bimbol Engine/Function/cmd-handler"
require "Bimbol Engine/Function/GetPlayeridByName"
require "Bimbol Engine/Function/GetTableValue"
require "Bimbol Engine/Function/RandomWithoutRepetition"
require "Bimbol Engine/Function/CheckPlayerInWater"
require "Bimbol Engine/Function/GetOnlinePlayers"
require "Bimbol Engine/Function/ColorHexToRGB"
require "Bimbol Engine/Function/Split"
-- Callbacks
require "Bimbol Engine/Callbacks/OnPlayerSpawn"
require "Bimbol Engine/Callbacks/OnPlayerDeath"
require "Bimbol Engine/Callbacks/OnPlayerHit"
require "Bimbol Engine/Callbacks/OnPlayerConnect"
require "Bimbol Engine/Callbacks/OnPlayerDisconnect"
require "Bimbol Engine/Callbacks/OnGamemodeInit"
require "Bimbol Engine/Callbacks/OnPlayerChangeStrength"
require "Bimbol Engine/Callbacks/OnPlayerChangeDexterity"
require "Bimbol Engine/Callbacks/OnPlayerChangeHealth"
require "Bimbol Engine/Callbacks/OnPlayerChangeMana"
require "Bimbol Engine/Callbacks/OnPlayerChangeLearnPoints"
require "Bimbol Engine/Callbacks/OnPlayerChangeGold"
require "Bimbol Engine/Callbacks/OnPlayerDropItem"
require "Bimbol Engine/Callbacks/OnPlayerTakeItem"
require "Bimbol Engine/Callbacks/OnPlayerUseItem"
require "Bimbol Engine/Callbacks/OnPlayerText"
require "Bimbol Engine/Callbacks/OnPlayerChangeWorld"
require "Bimbol Engine/Callbacks/OnPlayerKey"
require "Bimbol Engine/Callbacks/OnPlayerCommandText"
require "Bimbol Engine/Callbacks/OnPlayerFocus"
require "Bimbol Engine/Callbacks/OnPlayerSpellCast"
require "Bimbol Engine/Callbacks/OnPlayerWeaponMode"
-- Timer
require "Bimbol Engine/Time/timer"
-- AI
require "Bimbol Engine/AI/monster-add"
require "Bimbol Engine/AI/monster-spawn"
require "Bimbol Engine/AI/monster-ai"
-- Area
require "Bimbol Engine/Area/area"
-- Group
require "Bimbol Engine/Group/group"
-- Player
require "Bimbol Engine/player"
-- Anty-Cheat
require "Bimbol Engine/Anty-Cheat/anty-cheat"
-- File
require "Bimbol Engine/Save/File/file"
-- MySQL
require "Bimbol Engine/Save/MySQL/mysql"
-- GUI
require "Bimbol Engine/GUI/gui"
-- Key
require "Bimbol Engine/Key/key"
-- NPC
require "Bimbol Engine/NPC/npc"
-- WayPoints
require "Bimbol Engine/NPC/wp"
-- Time
require "Bimbol Engine/Time/time"
require "Bimbol Engine/Time/time_event"
-- Camera
require "Bimbol Engine/Camera/camera"

-- Timer

-- Checking command
function VersionBimbolEngine(playerid)
	SendPlayerMessage(playerid, 0, 255, 0, "== Version of Bimbol Engine: 0.1 beta ==");
end
addCommandHandler("/version_be", VersionBimbolEngine);								

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");
print("== Bimbol Engine v0.1 beta has been loaded ==");