INSERT OR REPLACE 
INTO ScriptAni (ScriptAniID, FPS, StartFrame, EndFrame, SpecialFrames)
VALUES
    (XXX, XXX, XXX, XXX, XXX)

INSERT OR REPLACE 
INTO ScriptOverlay (ScriptOverlayID, CodeName, ScriptOverlayName)
VALUES
    (XXX, XXX, XXX)
    
INSERT OR REPLACE 
INTO ScriptAniJob (ScriptAniJobID, DefaultAniID, AniName, CodeName, NextScriptAniJobID, Layer)
VALUES 
    (1, NULL, "t_Stand_2_Jump", "jump_fwd", NULL, NULL),
    (2, NULL, "t_RunL_2_Jump", "jump_run", NULL, NULL),
    (3, NULL, "t_Stand_2_JumpUp", "jump_up", NULL, NULL)

INSERT OR REPLACE 
INTO ModelDef (ModelDefID, ModelDefName, Visual, AniCatalog, Radius, Height, FistRange)
VALUES 
    (1, "humans", "HUMANS.MDS", "NPCCatalog", 80, 180, 40)
    
INSERT OR REPLACE 
INTO OverlayAniJobRelation (ScriptOverlayID, ScriptAniJobID, ScriptAniID)
VALUES
    (XXX, XXX, XXX)
    
INSERT OR REPLACE 
INTO ScriptOverlayModelDef (ScriptOverlayID, ModelDefID)
VALUES
    (XXX, XXX)
    
INSERT OR REPLACE 
INTO ScriptAniJobModelDef (ScriptAniJobID, ModelDefID)
VALUES
    (XXX, XXX)