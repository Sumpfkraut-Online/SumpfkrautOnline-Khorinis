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
    --- draw
    (10, NULL, 0, 3, "Draw=2;"),
    (11, NULL, 0, 1, ""),
    (12, NULL, 0, 3, ""),
    (13, NULL, 0, 14, "Draw=5;"),
    
    --- undraw
    (14, NULL, 0, 3, "Draw=2;"),
    (15, NULL, 0, 1, ""),
    (16, NULL, 0, 3, ""),
    (17, NULL, 0, 14, "Draw=5;"),
    
    --- fight
    (18, NULL, 0, 15, "Combo=9;"),
    (19, NULL, 15, 29, "Hit=9;"),
    (20, NULL, 0, 29, "Hit=19;"),
    (21, NULL, 0, 12, ""),
    (22, NULL, 0, 12, ""),
    
    --- >> 1H << ---
    --- draw
    (23, NULL, 0, 2, "Draw=2;"),
    (24, NULL, 0, 3, "Draw=3;"),
    (25, NULL, 0, 1, "Draw=1;"),
    
    (26, NULL, 0, 1, ""),
    (27, NULL, 0, 1, ""),
    (28, NULL, 0, 1, ""),
    
    (29, NULL, 0, 12, ""),
    (30, NULL, 0, 6, ""),
    (31, NULL, 0, 8, ""),
    
    (32, NULL, 0, 24, "Draw=6;"),
    
    --- undraw
    (33, NULL, 0, 12, "Draw=12;"),
    (34, NULL, 0, 6, "Draw=6;"),
    (35, NULL, 0, 8, "Draw=8;"),
    
    (36, NULL, 0, 1, ""),
    (37, NULL, 0, 1, ""),
    (38, NULL, 0, 1, ""),
    
    (39, NULL, 0, 2, ""),
    (40, NULL, 0, 3, ""),
    (41, NULL, 0, 1, ""),
    
    (42, NULL, 0, 24, "Draw=18;"),
    
    --- fight
    (43, NULL, 0, 23, "Hit=6;Combo=15;"),
    (44, NULL, 0, 33, "Hit=4;Combo=14;"),
    (45, NULL, 0, 29, "Hit=4;Combo=10;"),
    
    (46, NULL, 26, 40, "Hit=7;"),
    (47, NULL, 33, 68, "Hit=4;Combo=15;"),
    (48, NULL, 33, 60, "Hit=3;Combo=9;"),
    
    (49, NULL, 68, 103, "Hit=6;Combo=17;"),
    (50, NULL, 65, 92, "Hit=8;Combo=13;"),
    
    (51, NULL, 103, 120, "Hit=7;"),
    (52, NULL, 97, 113, "Hit=10;"),
    
    (53, NULL, 0, 30, "Hit=5;Combo=15;"),
    (54, NULL, 0, 40, "Hit=4;Combo=10;"),
    (55, NULL, 0, 20, "Hit=3;Combo=8;"),
    
    (56, NULL, 0, 30, "Hit=5;Combo=15;"),
    (57, NULL, 0, 40, "Hit=4;Combo=10;"),
    (58, NULL, 0, 20, "Hit=3;Combo=8;"),
    
    (59, NULL, 0, 29, "Hit=16;"),
    (60, NULL, 0, 15, ""),
    (61, NULL, 0, 15, ""),
    (62, NULL, 0, 15, ""),
    (63, NULL, 0, 14, "")
;

INSERT OR REPLACE 
INTO ScriptOverlay (ScriptOverlayID, CodeName, ScriptOverlayName)
VALUES
    --- >> 1H << ---
    (1, "1HST1", "Humans_1hST1"),
    (2, "1HST2", "Humans_1hST2")
;
    
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
    --- draw
    (13, 10, "t_Run_2_Fist", "drawfists_part0", 14, NULL),
    (14, 11, "s_Fist", "drawfists_part1", 15, NULL),
    (15, 12, "t_Fist_2_FistRun", "drawfists_part2", NULL, NULL),
    (16, 13, "t_Move_2_FistMove", "drawfists_running", NULL, 2),
    
    --- undraw
    (17, 14, "t_FistRun_2_Fist", "undrawfists_part0", 18, NULL),
    (18, 15, "s_Fist", "undrawfists_part1", 19, NULL),
    (19, 16, "t_Fist_2_Run", "undrawfists_part2", NULL, NULL),
    (20, 17, "t_FistMove_2_Move", "undrawfists_running", NULL, 2),
    
    --- fight
    (21, 18, "s_FistAttack", "fistattack_fwd0", NULL, NULL),
    (22, 19, "s_FistAttack", "fistattack_fwd1", NULL, NULL),
    (23, 20, "t_FistAttackMove", "fistattack_run", NULL, 2),
    (24, 21, "t_FistParade_0", "fist_parade", NULL, NULL),
    (25, 22, "t_FistParadeJumpB", "fist_jumpback", NULL, NULL),
    
    --- >> 1H << ---
    --- draw
    (26, 23, "t_Run_2_1h", "draw1h_part0", 27, NULL),
    (27, 26, "s_1h", "draw1h_part1", 28, NULL),
    (28, 29, "t_1h_2_1hRun", "draw1h_part2", NULL, NULL),
    
    (29, 32, "t_Move_2_1hMove", "draw1h_running", NULL, 2),
    
    --- undraw
    (30, 33, "t_1hRun_2_1h", "undraw1h_part0", 31, NULL),
    (31, 36, "s_1h", "undraw1h_part1", 32, NULL),
    (32, 39, "t_1h_2_Run", "undraw1h_part2", NULL, NULL),
    
    (33, 42, "t_1hMove_2_Move", "undraw1h_running", NULL, 2),
    
    --- fight
    (34, 43, "s_1hattack", "1hattack_fwd0", NULL, NULL),
    (35, 46, "s_1hattack", "1hattack_fwd1", NULL, NULL),
    (36, NULL, "s_1hattack", "1hattack_fwd2", NULL, NULL),
    (37, NULL, "s_1hattack", "1hattack_fwd3", NULL, NULL),
    (38, 53, "t_1HAttackL", "1hattack_left", NULL, NULL),
    (39, 56, "t_1HAttackR", "1hattack_right", NULL, NULL),
    
    (40, 59, "t_1HAttackMove", "1hattack_run", NULL, 2),
    (41, 60, "t_1HParade_0", "1h_parade0", NULL, NULL),
    (42, 61, "t_1HParade_0_A2", "1h_parade1", NULL, NULL),
    (43, 62, "t_1HParade_0_A3", "1h_parade2", NULL, NULL),
    (44, 63, "t_1HParadeJumpB", "1h_dodge", NULL, NULL)
;

INSERT OR REPLACE 
INTO ModelDef (ModelDefID, ModelDefName, Visual, AniCatalog, Radius, Height, FistRange)
VALUES 
    (1, "humans", "HUMANS.MDS", "NPCCatalog", 80, 180, 40)
;
    
INSERT OR REPLACE 
INTO OverlayAniJobRelation (ScriptOverlayID, ScriptAniJobID, ScriptAniID)
VALUES
    --- >> 1H << ---
    --- draw
    (1, 26, 24),
    (2, 26, 25),
    
    (1, 27, 27),
    (2, 27, 28),
    
    (1, 28, 30),
    (2, 28, 31),
    
    --- undraw
    (1, 30, 34),
    (2, 30, 35),
    
    (1, 31, 37),
    (2, 31, 38),
    
    (1, 32, 40),
    (2, 32, 41),
    
    --- fight
    (1, 34, 44),
    (2, 34, 45),
    
    (1, 35, 47),
    (2, 35, 48),
    
    (1, 36, 49),
    (2, 36, 50),
    
    (1, 37, 51),
    (2, 37, 52),
    
    (1, 38, 54),
    (2, 38, 55),
    
    (1, 39, 57),
    (2, 39, 58)
;
    
INSERT OR REPLACE 
INTO ScriptOverlayModelDef (ModelDefID, ScriptOverlayID)
VALUES
    --- >> 1H << ---
    (1, 1),
    (1, 2)
;
    
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
    --- draw
    (1, 13),
    (1, 14),
    (1, 15),
    (1, 16),
    
    --- undraw
    (1, 17),
    (1, 18),
    (1, 19),
    (1, 20),
    
    --- fight
    (1, 21),
    (1, 22),
    (1, 23),
    (1, 24),
    (1, 25),
    
    --- >> 1H << ---
    --- draw
    (1, 26),
    (1, 27),
    (1, 28),
    
    (1, 29),
    
    --- undraw
    (1, 30),
    (1, 31),
    (1, 32),
    
    (1, 33),
    
    --- fight
    (1, 34),
    (1, 35),    
    (1, 36),
    (1, 37),
    (1, 38),
    (1, 39),
    
    (1, 40),
    (1, 41),
    (1, 42),
    (1, 43),
    (1, 44)
;