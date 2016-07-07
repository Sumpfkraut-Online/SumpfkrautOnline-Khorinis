-- >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
-- >>>>> main database (includes accounts, worlds) <<<<<<<<<<<
-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

-- >> worlds << --
--------------------------------------------------------------

-- list of WorldDef --
DROP TABLE IF EXISTS WorldDef;
CREATE TABLE IF NOT EXISTS WorldDef 
(
    WorldDefID INTEGER NOT NULL, -- unique primary key id
    WorldDefName Text NOT NULL, -- descriptive name
    FilePath Text NOT NULL, -- relative path to the corresponding SqLite-file
    Description Text DEFAULT "", -- optional description
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of last change made
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of creation
    CONSTRAINT WorlDef_PK PRIMARY KEY (WorldDefID) 
);

CREATE TRIGGER Update_WorldDef
    AFTER UPDATE
    ON WorldDef
BEGIN
    UPDATE WorldDef SET ChangeDate = CURRENT_TIMESTAMP WHERE WorldDefID = OLD.WorldDefID;
END;

-- list of WorldInst --
DROP TABLE IF EXISTS WorldInst;
CREATE TABLE IF NOT EXISTS WorldInst 
(
    WorldInstID INTEGER NOT NULL, -- unique primary key id
    WorldInstName Text NOT NULL, -- descriptive name
    WorldDefID INTEGER NOT NULL, -- origin, this WorldInst relates to (uses content of WorldDef)
    FilePath Text NOT NULL, -- relative path to the corresponding SqLite-file
    Description Text DEFAULT "", -- optional description
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of last change made
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of creation
    CONSTRAINT WorlDef_PK PRIMARY KEY (WorldInstID)
);

CREATE TRIGGER Update_WorldInst
    AFTER UPDATE
    ON WorldInst
BEGIN
    UPDATE WorldInst SET ChangeDate = CURRENT_TIMESTAMP WHERE WorldInstID = OLD.WorldInstID;
END;

-- >> vob-effect-system << --
--------------------------------------------------------------

-- ids of effects which are applied to world- or vob-definitions --
DROP TABLE IF EXISTS DefEffect;
CREATE TABLE IF NOT EXISTS DefEffect 
(
    DefEffectID INTEGER NOT NULL,
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT DefEffect_PK PRIMARY KEY (DefEffectID) 
);

CREATE TRIGGER Update_DefEffect
    AFTER UPDATE
    ON DefEffect
BEGIN
    UPDATE DefEffect SET ChangeDate = CURRENT_TIMESTAMP WHERE DefEffectID = OLD.DefEffectID;
END;

-- actual changes / attributes of vob-definitions --
DROP TABLE IF EXISTS DefChange;
CREATE TABLE IF NOT EXISTS DefChange 
(
    DefChangeID INTEGER NOT NULL,
    DefEffectID INTEGER NOT NULL,
    Func INTEGER  NOT NULL,
    Params TEXT NOT NULL DEFAULT "",
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT DefChange_PK PRIMARY KEY (DefChangeID) 
    FOREIGN KEY (DefEffectID) REFERENCES DefEffect(DefEffectID)
);

CREATE TRIGGER Update_DefChange
    AFTER UPDATE
    ON DefChange
BEGIN
    UPDATE DefChange SET ChangeDate = CURRENT_TIMESTAMP WHERE DefChangeID = OLD.DefChangeID;
END;

-- >> vob-definitions (used globally in multiple worlds) << --
--------------------------------------------------------------

-- list of general definitions for vobs (mobs, items, npcs, spells?) --
DROP TABLE IF EXISTS VobDef;
CREATE TABLE IF NOT EXISTS VobDef 
(
    VobDefID INTEGER NOT NULL,
    IsStatic INTEGER DEFAULT 0 CHECK ((IsStatic == 0) OR (IsStatic == 1)), -- static objects are already uploaded for the clients to download on their local hard drive !!! MIGHT AS WELL SAVE IT AS ANOTHER EFFECT !!!
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT VobDef_PK PRIMARY KEY (VobDefID) 
);

CREATE TRIGGER Update_VobDef
    AFTER UPDATE
    ON VobDef
BEGIN
    UPDATE VobDef SET ChangeDate = CURRENT_TIMESTAMP WHERE VobDefID = OLD.VobDefID;
END;

-- maps effect- to vob-definitions and vice-versa --
DROP TABLE IF EXISTS VobDefEffect;
CREATE TABLE IF NOT EXISTS VobDefEffect 
(
    VobDefID INTEGER NOT NULL,
    DefEffectID INTEGER NOT NULL,
    FOREIGN KEY (VobDefID) REFERENCES VobDef(VobDefID),
    FOREIGN KEY (DefEffectID) REFERENCES DefEffect(DefEffectID)
);

-- >> accounts << --
--------------------------------------------------------------

-- !!! TO DO !!!