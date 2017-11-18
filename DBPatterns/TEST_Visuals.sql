INSERT OR REPLACE 
INTO ScriptAni (ScriptAniID, FPS, StartFrame, EndFrame, SpecialFrames)
VALUES
    --- >> climbing << --
    (1, NULL, 0, 4, ""),
    (2, NULL, 0, 1, ""),
    (3, NULL, 0, 9, ""),
    
    (4, NULL, 0, 9, ""),
    (5, NULL, 0, 1, ""),
    (6, NULL, 0, 20, ""),
    
    (7, NULL, 0, 17, ""),
    (8, NULL, 0, 1, ""),
    (9, NULL, 0, 25, ""),
    
    --- >> fists << --
    (10, NULL, 0, 3, "Draw=2;"),
    (11, NULL, 0, 1, ""),
    (12, NULL, 0, 3, ""),
    (13, NULL, 0, 14, "Draw=5;"),
    
    (14, NULL, 0, 3, "Draw=2;"),
    (15, NULL, 0, 1, ""),
    (16, NULL, 0, 3, ""),
    (17, NULL, 0, 14, "Draw=5;"),
    
    (18, NULL, 0, 15, "Combo=9;"),
    (19, NULL, 15, 29, "Hit=9;"),
    (20, NULL, 0, 29, "Hit=19;"),
    (21, NULL, 0, 12, ""),
    (22, NULL, 0, 12, "")

INSERT OR REPLACE 
INTO ScriptOverlay (ScriptOverlayID, CodeName, ScriptOverlayName)
VALUES
    (1, "1HST1", "Humans_1hST1"),
    (2, "1HST2", "Humans_1hST2")
    
INSERT OR REPLACE 
INTO ScriptAniJob (ScriptAniJobID, DefaultAniID, AniName, CodeName, NextScriptAniJobID, Layer)
VALUES 
    --- >> jumping << ---
    (1, NULL, "t_Stand_2_Jump", "jump_fwd", NULL, NULL),
    (2, NULL, "t_RunL_2_Jump", "jump_run", NULL, NULL),
    (3, NULL, "t_Stand_2_JumpUp", "jump_up", NULL, NULL),
    
    --- >> climbing << --
    (4, 1, "t_Stand_2_JumpUpLow", "climb_low", 5, NULL),
    (5, 2, "s_JumpUpLow", "climb_low1", 6, NULL),
    (6, 3, "t_JumpUpLow_2_Stand", "climb_low2", NULL, NULL),
    
    (7, 4, "t_Stand_2_JumpUpMid", "climb_mid", 8, NULL),
    (8, 5, "s_JumpUpMid", "climb_mid1", 9, NULL),
    (9, 6, "t_JumpUpMid_2_Stand", "climb_mid2", NULL, NULL),
    
    (10, 7, "t_Jump_2_Hang", "climb_high", 11, NULL),
    (11, 8, "s_hang", "climb_high1", 12, NULL),
    (12, 9, "t_Hang_2_Stand", "climb_high2", NULL, NULL),
    
    --- >> fists << --
    (13, 10, "t_Run_2_Fist", "drawfists_part0", 14, NULL),
    (14, 11, "s_Fist", "drawfists_part1", 15, NULL),
    (15, 12, "t_Fist_2_FistRun", "drawfists_part2", NULL, NULL),
    (16, 13, "t_Move_2_FistMove", "drawfists_running", NULL, 2),
    
    (17, 14, "t_FistRun_2_Fist", "undrawfists_part0", 18, NULL),
    (18, 15, "s_Fist", "undrawfists_part1", 19, NULL),
    (19, 16, "t_Fist_2_Run", "undrawfists_part2", NULL, NULL),
    (20, 17, "t_FistMove_2_Move", "undrawfists_running", NULL, 2),
    
    (21, 18, "s_FistAttack", "fistattack_fwd0", NULL, NULL),
    (22, 19, "s_FistAttack", "fistattack_fwd1", NULL, NULL),
    (23, 20, "t_FistAttackMove", "fistattack_run", NULL, 2),
    (24, 21, "t_FistParade_0", "fist_parade", NULL, NULL),
    (25, 22, "t_FistParadeJumpB", "fist_jumpback", NULL, NULL),

INSERT OR REPLACE 
INTO ModelDef (ModelDefID, ModelDefName, Visual, AniCatalog, Radius, Height, FistRange)
VALUES 
    (1, "humans", "HUMANS.MDS", "NPCCatalog", 80, 180, 40)
    
INSERT OR REPLACE 
INTO OverlayAniJobRelation (ScriptOverlayID, ScriptAniJobID, ScriptAniID)
VALUES
    (XXX, XXX, XXX)
    
INSERT OR REPLACE 
INTO ScriptOverlayModelDef (ModelDefID, ScriptOverlayID)
VALUES
    (1, 1),
    (1, 2)
    
INSERT OR REPLACE 
INTO ScriptAniJobModelDef (ModelDefID, ScriptAniJobID)
VALUES
    --- >> jumping << ---
    (1, 1),
    (1, 2),
    (1, 3),
    
    --- >> climbing << --
    (1, 4),
    (1, 5),
    (1, 6),
    
    (1, 7),
    (1, 8),
    (1, 9),
    
    (1, 10),
    (1, 11),
    (1, 12),
    
    --- >> fists << --
    (1, 13),
    (1, 14),
    (1, 15),
    (1, 16),
    
    (1, 17),
    (1, 18),
    (1, 19),
    (1, 20),
    
    (1, 21),
    (1, 22),
    (1, 23),
    (1, 24),
    (1, 25)