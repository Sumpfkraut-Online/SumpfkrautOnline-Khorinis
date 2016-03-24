/* DROP DATABASE `SOK_Gameserver`;

CREATE DATABASE IF NOT EXISTS `SOK_Gameserver`;

USE `SOK_Gameserver`; */

DROP TABLE IF EXISTS `WorldInst`;
CREATE TABLE IF NOT EXISTS `WorldInst` (
    `WorldInstId` INTEGER  NOT NULL,
    `Name` TEXT DEFAULT NULL,
    `Path` TEXT DEFAULT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`WorldInstId`)
);

/* -- individual player accounts
DROP TABLE IF EXISTS `AccountInst`;
CREATE TABLE IF NOT EXISTS `AccountInst` (
    `AccountInstId` INTEGER  NOT NULL,
    `Name` TEXT NOT NULL,
    `Password` TEXT NOT NULL,
    `EMail` TEXT DEFAULT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`AccountInstId`)
);

-- npcs which belong to their respective accounts
DROP TABLE IF EXISTS `AccountNPCInst`;
CREATE TABLE IF NOT EXISTS `AccountNPCInst` (
    `AccountNPCInstId` INTEGER  NOT NULL,
    `NPCInstID` INTEGER  NOT NULL,
    FOREIGN KEY (`AccountNPCInstId`) REFERENCES `AccountInst`(`AccountInstId`)
    FOREIGN KEY (`NPCInstId`) REFERENCES `NPCInst`(`NPCInstId`)
); */

-- general definition information for Mob-instantiation (static or usable)
DROP TABLE IF EXISTS `MobDef`;
CREATE TABLE IF NOT EXISTS `MobDef` (
    `MobDefId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`MobDefId`)
);

-- general definition information for Item-instantiation
DROP TABLE IF EXISTS `ItemDef`;
CREATE TABLE IF NOT EXISTS `ItemDef` (
    `ItemDefId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ItemDefId`)
);

-- general definition information for NPC-instantiation
DROP TABLE IF EXISTS `NpcDef`;
CREATE TABLE IF NOT EXISTS `NpcDef` (
    `NpcDefId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`NpcDefId`)
);

-- general definition information for effect-definitions
DROP TABLE IF EXISTS `DefEffect`;
CREATE TABLE IF NOT EXISTS `DefEffect` (
    `DefEffectId` INTEGER  NOT NULL,
    `Name` TEXT NOT NULL DEFAULT "",
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`DefEffectId`)
);

-- general definition information for change definition that actually apply to the game
DROP TABLE IF EXISTS `DefChange`;
CREATE TABLE IF NOT EXISTS `DefChange` (
    `DefChangeId` INTEGER NOT NULL,
    `DefEffectId` INTEGER  NOT NULL,
    `ChangeType` INTEGER  NOT NULL,
    `Parameters` TEXT NOT NULL DEFAULT "",
    PRIMARY KEY (`DefChangeId`),
    FOREIGN KEY (`DefEffectId`) REFERENCES `DefEffect`(`DefEffectId`)
);

-- maps effect- to mob-definitions
DROP TABLE IF EXISTS `MobDefToDefEffect`;
CREATE TABLE IF NOT EXISTS `MobDefToDefEffect` (
    `MobDefId` INTEGER  NOT NULL,
    `DefEffectId` INTEGER  NOT NULL,
    FOREIGN KEY (`MobDefId`) REFERENCES `MobDef`(`MobDefId`),
    FOREIGN KEY (`DefEffectId`) REFERENCES `DefEffect`(`DefEffectId`)
);

-- maps effect- to item-definitions
DROP TABLE IF EXISTS `ItemDefToDefEffect`;
CREATE TABLE IF NOT EXISTS `ItemDefToDefEffect` (
    `ItemDefId` INTEGER  NOT NULL,
    `DefEffectId` INTEGER  NOT NULL,
    FOREIGN KEY (`ItemDefID`) REFERENCES `ItemDef`(`ItemDefId`),
    FOREIGN KEY (`DefEffectId`) REFERENCES `DefEffect`(`DefEffectId`)
);

-- maps effect- to npc-definitions
DROP TABLE IF EXISTS `NpcDefToDefEffect`;
CREATE TABLE IF NOT EXISTS `NpcDefToDefEffect` (
    `NPCDefId` INTEGER  NOT NULL,
    `DefEffectId` INTEGER  NOT NULL,
    FOREIGN KEY (`NPCDefID`) REFERENCES `NPCDef`(`NPCDefId`),
    FOREIGN KEY (`DefEffectId`) REFERENCES `DefEffect`(`DefEffectId`)
);

-- individual mob instances
DROP TABLE IF EXISTS `MobInst`;
CREATE TABLE IF NOT EXISTS `MobInst` (
    `MobInstId` INTEGER  NOT NULL,
    `MobDefId` INTEGER NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`MobInstId`),
    FOREIGN KEY (`MobDefId`) REFERENCES `MobDef`(`MobDefId`)
);

-- individual item instances
DROP TABLE IF EXISTS `ItemInst`;
CREATE TABLE IF NOT EXISTS `ItemInst` (
    `ItemInstId` INTEGER  NOT NULL,
    `ItemDefId` INTEGER  NOT NULL,
    `Amount` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`ItemInstId`),
    FOREIGN KEY (`ItemDefId`) REFERENCES `ItemDef`(`ItemDefId`)
);

-- individual npc instances
DROP TABLE IF EXISTS `NpcInst`;
CREATE TABLE IF NOT EXISTS `NpcInst` (
    `NpcInstId` INTEGER  NOT NULL,
    `NpcDefId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    PRIMARY KEY (`NpcInstId`),
    FOREIGN KEY (`NpcDefId`) REFERENCES `NpcDef`(`NpcDefId`)
);
`/* IsSpawned` INTEGER  NOT NULL,
    `Fatness` INTEGER  NOT NULL,
    `ScaleX` INTEGER  NOT NULL,
    `ScaleY` INTEGER  NOT NULL,
    `ScaleZ` INTEGER  NOT NULL,
    `HeadMesh` TEXT NOT NULL,
    `HeadTexture` INTEGER NOT NULL,
    `BodyMesh` TEXT NOT NULL,
    `BodyTexture` INTEGER NOT NULL,
    `CurrWalk` INTEGER  NOT NULL,
    `CurrAnimation` TEXT DEFAULT "", */

-- individual item instances which are positioned in npc inventories
DROP TABLE IF EXISTS `ItemInInventoryInst`;
CREATE TABLE IF NOT EXISTS `ItemInInventoryInst` (
    `NpcInstId` INTEGER  NOT NULL,
    `ItemInstId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`NpcInstId`) REFERENCES `NpcInst`(`NpcInstId`),
    FOREIGN KEY (`ItemInstId`) REFERENCES `ItemInst`(`ItemInstId`)
);

-- individual item instances which are positioned in containers/mobs
DROP TABLE IF EXISTS `ItemInContainerInst`;
CREATE TABLE IF NOT EXISTS `ItemInInventory_inst` (
    `MobInstId` INTEGER  NOT NULL,
    `ItemInstId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`MobInstId`) REFERENCES `MobInst`(`MobInstId`),
    FOREIGN KEY (`ItemInstId`) REFERENCES `ItemInst`(`ItemInstId`)
);

-- individual mob instances which are positioned in one of the worlds
DROP TABLE IF EXISTS `MobInWorldInst`;
CREATE TABLE IF NOT EXISTS `MobInWorldInst` (
    `MobInstId` INTEGER  NOT NULL,
    `WorldInstId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`MobInstId`) REFERENCES `MobInst`(`MobInstId`),
    FOREIGN KEY (`WorldInstId`) REFERENCES `WorldInst`(`WorldInstId`)
);

-- individual item instances which are positioned in one of the worlds
DROP TABLE IF EXISTS `ItemInWorldInst`;
CREATE TABLE IF NOT EXISTS `ItemInWorldInst` (
    `ItemInstId` INTEGER  NOT NULL,
    `WorldInstId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`ItemInstId`) REFERENCES `ItemInst`(`ItemInstId`),
    FOREIGN KEY (`WorldInstId`) REFERENCES `WorldInst`(`WorldInstId`)
);

-- individual npc instances which are positioned in one of the worlds
DROP TABLE IF EXISTS `NpcInWorldInst`;
CREATE TABLE IF NOT EXISTS `NpcInWorldInst` (
    `NpcInstId` INTEGER  NOT NULL,
    `WorldInstId` INTEGER  NOT NULL,
    `ChangeDate` TEXT NOT NULL,
    `CreationDate` TEXT NOT NULL,
    FOREIGN KEY (`NpcInstId`) REFERENCES `NpcInst`(`NpcInstId`),
    FOREIGN KEY (`WorldInstId`) REFERENCES `WorldInst`(`WorldInstId`)
);