INSERT OR REPLACE 
INTO ScriptAni (ScriptAniID, FPS, StartFrame, EndFrame, SpecialFrames)
VALUES
    (XXX, XXX, XXX, XXX, XXX),
    (XXX, XXX, XXX, XXX, XXX)

INSERT OR REPLACE 
INTO ScriptOverlay (ScriptOverlayID, CodeName, ScriptOverlayName)
VALUES
    (XXX, XXX, XXX),
    (XXX, XXX, XXX)
    
INSERT OR REPLACE 
INTO ScriptAniJob (ScriptAniJobID, DefaultAniID, AniName, CodeName, NextScriptAniJobID, Layer)
VALUES 
    (XXX, XXX, XXX, XXX, XXX, XXX),
    (XXX, XXX, XXX, XXX, XXX, XXX)

INSERT OR REPLACE 
INTO ModelDef (ModelDefID, ModelDefName, Visual, AniCatalog, Radius, Height, FistRange)
VALUES 
    (XXX, XXX, XXX, XXX, XXX, XXX, XXX),
    (XXX, XXX, XXX, XXX, XXX, XXX, XXX)
    
INSERT OR REPLACE 
INTO OverlayAniJobRelation (ScriptOverlayID, ScriptAniJobID, ScriptAniID)
VALUES
    (XXX, XXX, XXX),
    (XXX, XXX, XXX)
    
INSERT OR REPLACE 
INTO ScriptOverlayModelDef (ScriptOverlayID, ModelDefID)
VALUES
    (XXX, XXX),
    (XXX, XXX)
    
INSERT OR REPLACE 
INTO ScriptAniJobModelDef (ModelDefID, ScriptAniJobID)
VALUES
    (XXX, XXX),
    (XXX, XXX)