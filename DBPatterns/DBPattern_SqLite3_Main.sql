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
    WorldDefName TEXT NOT NULL, -- descriptive name
    FilePath TEXT NOT NULL, -- relative path to the corresponding SqLite-file
    Description TEXT DEFAULT "", -- optional description
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
    WorldInstName TEXT NOT NULL, -- descriptive name
    WorldDefID INTEGER NOT NULL, -- origin, this WorldInst relates to (uses content of WorldDef)
    FilePath TEXT NOT NULL, -- relative path to the corresponding SqLite-file
    Description TEXT DEFAULT "", -- optional description
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of last change made
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of creation
    CONSTRAINT WorldInst_PK PRIMARY KEY (WorldInstID)
);

CREATE TRIGGER Update_WorldInst
    AFTER UPDATE
    ON WorldInst
BEGIN
    UPDATE WorldInst SET ChangeDate = CURRENT_TIMESTAMP WHERE WorldInstID = OLD.WorldInstID;
END;

-- >> effect-system (for VobDef-objects) << --
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
    ChangeType TEXT  NOT NULL,
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
    IsStatic INTEGER DEFAULT 0 CHECK ((IsStatic == 0) OR (IsStatic == 1)), -- static objects are already uploaded for the clients to download on their local hard drive !!! MIGHT AS WELL SAVE IT AS ANOTHER EFFECT ?!?
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

-- >> models and animations << --
--------------------------------------------------------------

-- list of ModelDef --
DROP TABLE IF EXISTS ModelDef;
CREATE TABLE IF NOT EXISTS ModelDef 
(
    ModelDefID INTEGER NOT NULL, -- unique primary key id
    ModelDefName TEXT NOT NULL, -- descriptive name
    IsStatic INTEGER DEFAULT 0 CHECK ((IsStatic == 0) OR (IsStatic == 1)), -- static objects are already uploaded for the clients to download on their local hard drive !!! MIGHT AS WELL SAVE IT AS ANOTHER EFFECT ?!?
    Visual TEXT NOT NULL, -- Gothic visual name
    Radius REAL DEFAULT 0, -- radius for collision detection
    Height REAL DEFAULT 0, -- height for collision detection
    FistRange REAL DEFAULT 0, -- fist range for hit detection without weapon
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of last change made
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of creation
    CONSTRAINT ModelDef_PK PRIMARY KEY (ModelDefID)
);

CREATE TRIGGER Update_ModelDef
    AFTER UPDATE
    ON ModelDef
BEGIN
    UPDATE ModelDef SET ChangeDate = CURRENT_TIMESTAMP WHERE ModelDef = OLD.ModelDef;
END;

-- list of ScriptOverlay --
DROP TABLE IF EXISTS ScriptOverlay;
CREATE TABLE IF NOT EXISTS ScriptOverlay 
(
    ScriptOverlayID INTEGER NOT NULL, -- unique primary key id
    ModelDefID INTEGER NOT NULL,
    CodeName TEXT NOT NULL,
    ScriptOverlayName TEXT NOT NULL, -- descriptive name
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of last change made
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of creation
    CONSTRAINT ScriptOverlay_PK PRIMARY KEY (ScriptOverlayID),
    FOREIGN KEY (ModelDefID) REFERENCES ModelDef(ModelDefID)
);

CREATE TRIGGER Update_ScriptOverlay
    AFTER UPDATE
    ON ScriptOverlay
BEGIN
    UPDATE ScriptOverlay SET ChangeDate = CURRENT_TIMESTAMP WHERE ScriptOverlayID = OLD.ScriptOverlayID;
END;

-- list of ScriptAniJob--
DROP TABLE IF EXISTS ScriptAniJob;
CREATE TABLE IF NOT EXISTS ScriptAniJob 
(
    ScriptAniJobID INTEGER NOT NULL, -- unique primary key id
    ScriptAniID INTEGER NOT NULL,
	AniName TEXT NOT NULL,
	CodeName TEXT NOT NULL,
    AniJobType INTEGER NOT NULL,
	PrevCodeName TEXT DEFAULT NULL,
	NextCodeName TEXT DEFAULT NULL,
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of last change made
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of creation
    CONSTRAINT ScriptAniJob_PK PRIMARY KEY (ScriptAniJobID),
    FOREIGN KEY (ScriptAniID) REFERENCES ScriptAni(ScriptAniID)
);

CREATE TRIGGER Update_ScriptAniJob
    AFTER UPDATE
    ON ScriptAniJob
BEGIN
    UPDATE ScriptAniJob SET ChangeDate = CURRENT_TIMESTAMP WHERE ScriptAniJobID = OLD.ScriptAniJobID;
END;

-- list of ScriptAni --
DROP TABLE IF EXISTS ScriptAni;
CREATE TABLE IF NOT EXISTS ScriptAni 
(
    ScriptAniID INTEGER NOT NULL, -- unique primary key id
    ScriptOverlayID INTEGER NOT NULL,
    ScriptAniJobID INTEGER NOT NULL,
    Layer INTEGER NOT NULL,
    Duration INTEGER NOT NULL,
    StartFrame INTEGER NOT NULL,
    EndFrame INTEGER NOT NULL,
    SpecialFrames TEXT NOT NULL,
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of last change made
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, -- date of creation
    CONSTRAINT ScriptAni_PK PRIMARY KEY (ScriptAniID)
);

CREATE TRIGGER Update_ScriptAni
    AFTER UPDATE
    ON ScriptAni
BEGIN
    UPDATE ScriptAni SET ChangeDate = CURRENT_TIMESTAMP WHERE ScriptAniID = OLD.ScriptAniID;
END;

-- >> static and dynamic content management << --
--------------------------------------------------------------

-- list of "jobs" concerning switches between statis and dynamic content --
DROP TABLE IF EXISTS StaticDynamicJob;
CREATE TABLE IF NOT EXISTS StaticDynamicJob
(
    StaticDynamicJobID INTEGER NOT NULL,
    TableName TEXT NOT NULL,
    Task TEXT NOT NULL,
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT StaticDynamicJob_PK PRIMARY KEY (StaticDynamicJobID)
);

CREATE TRIGGER Update_StaticDynamicJob
    AFTER UPDATE
    ON StaticDynamicJob
BEGIN
    UPDATE StaticDynamicJob SET ChangeDate = CURRENT_TIMESTAMP 
        WHERE StaticDynamicJobID = OLD.StaticDynamicJobID;
END;

-- >> accounts << --
--------------------------------------------------------------

-- !!! TO DO !!!