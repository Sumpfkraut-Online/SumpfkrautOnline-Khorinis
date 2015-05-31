/* DROP DATABASE `SOK_Gameserver`;

CREATE DATABASE IF NOT EXISTS `SOK_Gameserver`;

USE `SOK_Gameserver`; */

DROP TABLE IF EXISTS `World_inst`;
CREATE TABLE IF NOT EXISTS `World_inst` (
    `ID` INTEGER  NOT NULL,
    `Name` TEXT DEFAULT NULL,
    `Path` TEXT DEFAULT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- individual player accounts
DROP TABLE IF EXISTS `Account_inst`;
CREATE TABLE IF NOT EXISTS `Account_inst` (
    `ID` INTEGER  NOT NULL,
    `Name` TEXT NOT NULL,
    `Password` TEXT NOT NULL,
    `EMail` TEXT DEFAULT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- npcs which belong to their respective accounts
DROP TABLE IF EXISTS `AccountNPCs_inst`;
CREATE TABLE IF NOT EXISTS `AccountNPCs_inst` (
    `AccountInstID` INTEGER  NOT NULL,
    `NPCInstID` INTEGER  NOT NULL,
    FOREIGN KEY (`AccountInstID`) REFERENCES `Account_inst`(`ID`)
    FOREIGN KEY (`NPCInstID`) REFERENCES `NPC_inst`(`ID`)
);

-- general definition information for Mob-instantiation (static or usable)
DROP TABLE IF EXISTS `Mob_def`;
CREATE TABLE IF NOT EXISTS `Mob_def` (
    `ID` INTEGER  NOT NULL,
    `VobType` TEXT NOT NULL,
    `Visual` TEXT NOT NULL,
    `Material` INTEGER  NOT NULL DEFAULT 3,
    `HasEffects` INTEGER NOT NULL DEFAULT 0,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- general definition information for Item-instantiation
DROP TABLE IF EXISTS `Item_def`;
CREATE TABLE IF NOT EXISTS `Item_def` (
    `ID` INTEGER  NOT NULL,
    `VobType` TEXT NOT NULL,
    `Visual` TEXT NOT NULL,
    `Visual_Skin` INTEGER  NOT NULL DEFAULT 0,
    `InstanceName` TEXT NOT NULL,
    `Name` TEXT NOT NULL,
    `ScemeName` TEXT NOT NULL,
    `Protection` TEXT, -- should be of pattern 0=0,1=0,2=25
    `Damages` TEXT, -- should be of pattern 0=0,1=0,2=25
    `Value` INTEGER NOT NULL DEFAULT 0,
    `MainFlags` INTEGER NOT NULL DEFAULT 0,
    `Flags` INTEGER NOT NULL, -- defines type of item in Gothic 2
    `Wear` INTEGER DEFAULT NULL, -- ArmorFlags-entry (0 - 3)
    `Material` INTEGER  NOT NULL DEFAULT 3,
    `IsKeyInstance` INTEGER NOT NULL DEFAULT 0,
    `IsTorch` INTEGER NOT NULL DEFAULT 0,
    `IsTorchBurning` INTEGER NOT NULL DEFAULT 0,
    `IsTorchBurned` INTEGER NOT NULL DEFAULT 0,
    `IsGold` INTEGER NOT NULL DEFAULT 0,
    `HasEffects` INTEGER NOT NULL DEFAULT 0,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- general definition information for Spell-instantiation
DROP TABLE IF EXISTS `Spell_def`;
CREATE TABLE IF NOT EXISTS `Spell_def` (
    `ID` INTEGER  NOT NULL,
    `Name` TEXT NOT NULL,
    `FXName` TEXT NOT NULL,
    `DamageType` INTEGER NOT NULL DEFAULT 0,
    `SpellType` INTEGER NOT NULL DEFAULT 0,
    `IsMultiEffect` INTEGER NOT NULL DEFAULT 0,
    `TargetCollectionAlgo` INTEGER NOT NULL DEFAULT 0,
    `TargetCollectType` INTEGER NOT NULL DEFAULT 0,
    `TargetCollectRange` INTEGER NOT NULL DEFAULT 0,
    `TargetCollectAzi` INTEGER NOT NULL DEFAULT 0,
    `TargetCollectElev` INTEGER NOT NULL DEFAULT 0,
    `HasEffects` INTEGER NOT NULL DEFAULT 0,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- general definition information for NPC-instantiation
DROP TABLE IF EXISTS `NPC_def`;
CREATE TABLE IF NOT EXISTS `NPC_def` (
    `ID` INTEGER  NOT NULL,
    `VobType` TEXT NOT NULL,
    `Visual` TEXT NOT NULL,
    `Visual_Skin` INTEGER  NOT NULL DEFAULT 0,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- general definition information for effect-definitions
DROP TABLE IF EXISTS `Effect_def`;
CREATE TABLE IF NOT EXISTS `Effect_def` (
    `ID` INTEGER  NOT NULL,
    `Name` TEXT NOT NULL DEFAULT "",
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- general definition information for change definition that actually apply to the game
DROP TABLE IF EXISTS `Effect_Changes_def`;
CREATE TABLE IF NOT EXISTS `Effect_Changes_def` (
    `ID` INTEGER NOT NULL,
    `EventID` INTEGER  NOT NULL,
    `EffectDefID` INTEGER  NOT NULL,
    `ChangeType` INTEGER  NOT NULL,
    `Parameters` TEXT NOT NULL DEFAULT "",
    PRIMARY KEY (`ID`),
    FOREIGN KEY (`EffectDefID`) REFERENCES `Effect_def`(`ID`)
);

-- maps effect- to mob-definitions
DROP TABLE IF EXISTS `Mob_Effects_inst`;
CREATE TABLE IF NOT EXISTS `Mob_Effects_inst` (
    `MobDefID` INTEGER  NOT NULL,
    `EffectDefID` INTEGER  NOT NULL,
    FOREIGN KEY (`MobDefID`) REFERENCES `Mob_def`(`ID`),
    FOREIGN KEY (`EffectDefID`) REFERENCES `Effect_def`(`ID`)
);

-- maps effect- to spell-definitions
DROP TABLE IF EXISTS `Spell_Effects_inst`;
CREATE TABLE IF NOT EXISTS `Spell_Effects_inst` (
    `SpellDefID` INTEGER  NOT NULL,
    `EffectDefID` INTEGER  NOT NULL,
    FOREIGN KEY (`SpellDefID`) REFERENCES `Spell_def`(`ID`),
    FOREIGN KEY (`EffectDefID`) REFERENCES `Effect_def`(`ID`)
);

-- maps effect- to item-definitions
DROP TABLE IF EXISTS `Item_Effects_inst`;
CREATE TABLE IF NOT EXISTS `Item_Effects_inst` (
    `ItemDefID` INTEGER  NOT NULL,
    `EffectDefID` INTEGER  NOT NULL,
    FOREIGN KEY (`ItemDefID`) REFERENCES `Item_def`(`ID`),
    FOREIGN KEY (`EffectDefID`) REFERENCES `Effect_def`(`ID`)
);

-- maps effect- to npc-definitions
DROP TABLE IF EXISTS `NPC_Effects_inst`;
CREATE TABLE IF NOT EXISTS `NPC_Effects_inst` (
    `NPCDefID` INTEGER  NOT NULL,
    `EffectDefID` INTEGER  NOT NULL,
    FOREIGN KEY (`NPCDefID`) REFERENCES `NPC_def`(`ID`),
    FOREIGN KEY (`EffectDefID`) REFERENCES `Effect_def`(`ID`)
);

-- individual mob instances
DROP TABLE IF EXISTS `Mob_inst`;
CREATE TABLE IF NOT EXISTS `Mob_inst` (
    `ID` INTEGER  NOT NULL,
    `MobDefID` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- individual item instances
DROP TABLE IF EXISTS `Item_inst`;
CREATE TABLE IF NOT EXISTS `Item_inst` (
    `ID` INTEGER  NOT NULL,
    `ItemDefID` INTEGER  NOT NULL,
    `Amount` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- individual npc instances
DROP TABLE IF EXISTS `NPC_inst`;
CREATE TABLE IF NOT EXISTS `NPC_inst` (
    `ID` INTEGER  NOT NULL,
    `NPCDefID` INTEGER  NOT NULL,
    `IsSpawned` INTEGER  NOT NULL,
    `Fatness` INTEGER  NOT NULL,
    `ScaleX` INTEGER  NOT NULL,
    `ScaleY` INTEGER  NOT NULL,
    `ScaleZ` INTEGER  NOT NULL,
    `HeadTexture` TEXT DEFAULT "",
    `BodyTexture` TEXT DEFAULT "",
    `CurrWalk` INTEGER  NOT NULL,
    `CurrAnimation` TEXT DEFAULT "",
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ID`)
);

-- individual item instances which are positioned in npc inventories
DROP TABLE IF EXISTS `ItemInInventory_inst`;
CREATE TABLE IF NOT EXISTS `ItemInInventory_inst` (
    `NPCInstID` INTEGER  NOT NULL,
    `ItemInstID` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`NPCInstID`) REFERENCES `NPC_inst`(`ID`),
    FOREIGN KEY (`ItemInstID`) REFERENCES `Item_inst`(`ID`)
);

-- individual item instances which are positioned in containers/mobs
DROP TABLE IF EXISTS `ItemInContainer_inst`;
CREATE TABLE IF NOT EXISTS `ItemInInventory_inst` (
    `MobInstID` INTEGER  NOT NULL,
    `ItemInstID` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`MobInstID`) REFERENCES `Mob_inst`(`ID`),
    FOREIGN KEY (`ItemInstID`) REFERENCES `Item_inst`(`ID`)
);

-- individual mob instances which are positioned in one of the worlds
DROP TABLE IF EXISTS `MobInWorld_inst`;
CREATE TABLE IF NOT EXISTS `MobInWorld_inst` (
    `MobInstID` INTEGER  NOT NULL,
    `WorldInstID` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`MobInstID`) REFERENCES `Mob_inst`(`ID`),
    FOREIGN KEY (`WorldInstID`) REFERENCES `World_inst`(`ID`)
);

-- individual item instances which are positioned in one of the worlds
DROP TABLE IF EXISTS `ItemInWorld_inst`;
CREATE TABLE IF NOT EXISTS `ItemInWorld_inst` (
    `ItemInstID` INTEGER  NOT NULL,
    `WorldInstID` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`ItemInstID`) REFERENCES `Item_inst`(`ID`),
    FOREIGN KEY (`WorldInstID`) REFERENCES `World_inst`(`ID`)
);

-- individual npc instances which are positioned in one of the worlds
DROP TABLE IF EXISTS `NPCInWorld_inst`;
CREATE TABLE IF NOT EXISTS `NPCInWorld_inst` (
    `NPCInstID` INTEGER  NOT NULL,
    `WorldInstID` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`NPCInstID`) REFERENCES `NPC_inst`(`ID`),
    FOREIGN KEY (`WorldInstID`) REFERENCES `World_inst`(`ID`)
);