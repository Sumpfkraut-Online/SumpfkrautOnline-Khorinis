-- >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
-- >>>>> database pattern for worlds (WorldDef + WorldInst) <<
-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

-- >> world-wide settings / effects << --
--------------------------------------------------------------

-- effects concerning the whole world (real changes in WorldChange-table) --
DROP TABLE IF EXISTS WorldEffect;
CREATE TABLE IF NOT EXISTS WorldEffect
(
    WorldEffectID INTEGER NOT NULL,
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT WorldEffect_PK PRIMARY KEY (WorldEffectID)
);

CREATE TRIGGER Update_WorldEffect
    AFTER UPDATE
    ON WorldEffect
BEGIN
    UPDATE WorldEffect SET ChangeDate = CURRENT_TIMESTAMP WHERE WorldEffectID = OLD.WorldEffectID;
END;

-- actual changes / attributes concernign the current world --
DROP TABLE IF EXISTS WorldChange;
CREATE TABLE IF NOT EXISTS WorldChange 
(
    WorldChangeID INTEGER NOT NULL,
    WorldEffectID INTEGER NOT NULL,
    Func INTEGER  NOT NULL,
    Params TEXT NOT NULL DEFAULT "",
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT WorldChange_PK PRIMARY KEY (WorldChangeID) 
    FOREIGN KEY (WorldEffectID) REFERENCES WorldEffect(WorldEffectID)
);

CREATE TRIGGER Update_WorldChange
    AFTER UPDATE
    ON WorldChange
BEGIN
    UPDATE WorldChange SET ChangeDate = CURRENT_TIMESTAMP WHERE WorldChangeID = OLD.WorldChangeID;
END;

-- >> vob-effect-system << --
--------------------------------------------------------------

-- ids of effects which are applied to world- or vob-instinitions --
DROP TABLE IF EXISTS InstEffect;
CREATE TABLE IF NOT EXISTS InstEffect 
(
    InstEffectID INTEGER NOT NULL,
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT InstEffect_PK PRIMARY KEY (InstEffectID) 
);

CREATE TRIGGER Update_InstEffect
    AFTER UPDATE
    ON InstEffect
BEGIN
    UPDATE InstEffect SET ChangeDate = CURRENT_TIMESTAMP WHERE InstEffectID = OLD.InstEffectID;
END;

-- actual changes / attributes of vob-instinitions --
DROP TABLE IF EXISTS InstChange;
CREATE TABLE IF NOT EXISTS InstChange 
(
    InstChangeID INTEGER NOT NULL,
    InstEffectID INTEGER NOT NULL,
    Func INTEGER  NOT NULL,
    Params TEXT NOT NULL DEFAULT "",
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT InstChange_PK PRIMARY KEY (InstChangeID) 
    FOREIGN KEY (InstEffectID) REFERENCES InstEffect(InstEffectID)
);

CREATE TRIGGER Update_InstChange
    AFTER UPDATE
    ON InstChange
BEGIN
    UPDATE InstChange SET ChangeDate = CURRENT_TIMESTAMP WHERE InstChangeID = OLD.InstChangeID;
END;

-- >> VobInst-related << --
--------------------------------------------------------------

-- ids of vob instances (mobs, items, npcs, spells?) --
DROP TABLE IF EXISTS VobInst;
CREATE TABLE IF NOT EXISTS VobInst 
(
    VobInstID INTEGER NOT NULL,
    IsStatic INTEGER DEFAULT 0 CHECK ((IsStatic == 0) OR (IsStatic == 1)), -- static objects are already uploaded for the clients to download on their local hard drive !!! MIGHT AS WELL SAVE IT AS ANOTHER EFFECT !!!
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT VobInst_PK PRIMARY KEY (VobInstID) 
);

CREATE TRIGGER Update_VobInst
    AFTER UPDATE
    ON VobInst
BEGIN
    UPDATE VobInst SET ChangeDate = CURRENT_TIMESTAMP WHERE VobInstID = OLD.VobInstID;
END;

-- maps effect- to vob-instinitions and vice-versa --
DROP TABLE IF EXISTS VobInstEffect;
CREATE TABLE IF NOT EXISTS VobInstEffect 
(
    VobInstID INTEGER NOT NULL,
    InstEffectID INTEGER NOT NULL,
    FOREIGN KEY (VobInstID) REFERENCES VobInst(VobInstID),
    FOREIGN KEY (InstEffectID) REFERENCES InstEffect(InstEffectID)
);

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