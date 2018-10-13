using Gothic.Objects;
using GUC.Scripts.Sumpfkraut.Controls;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Types;
using GUC.Utilities;
using System;
using WinApi.User.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Enumeration;
using GUC.Scripts.Arena.GameModes;
using GUC.Scripts.Arena.Duel;
using GUC.Scripts.Arena.GameModes.Horde;

namespace GUC.Scripts.Arena.Controls
{
    partial class ArenaControl
    {
        KeyDictionary playerControls = new KeyDictionary()
        {
            { KeyBind.Jump, Jump },
            { KeyBind.DrawFists, d => { if (d) NPCInst.Requests.DrawFists(ScriptClient.Client.Character); } },
            { KeyBind.MoveForward, d => CheckFightMove(d, FightMoves.Fwd) },
            { KeyBind.TurnLeft, d => CheckFightMove(d, FightMoves.Left) },
            { KeyBind.TurnRight, d => CheckFightMove(d, FightMoves.Right) },
            { KeyBind.MoveBack, d => CheckFightMove(d, FightMoves.Parry) },
            { KeyBind.MoveLeft, d => CheckFightMove(d, FightMoves.Left) },
            { KeyBind.MoveRight, d => CheckFightMove(d, FightMoves.Right) },
            { KeyBind.Action, PlayerActionButton },
            { KeyBind.DrawWeapon, DrawWeapon },
            { KeyBind.ScoreBoard, ToggleScoreBoard },
            { KeyBind.ChatAll, d => { if (d) ChatMenu.Menu.OpenAllChat(); } },
            { KeyBind.ChatTeam, d => { if (d) ChatMenu.Menu.OpenTeamChat(); } },
            { KeyBind.Inventory, d => { if (d) Sumpfkraut.Menus.PlayerInventory.Menu.Open(); } },
            { VirtualKeys.P, PrintPosition },
            { VirtualKeys.F2, d => Menus.PlayerList.TogglePlayerList() },
            { VirtualKeys.F3, ToggleG1Camera },
            { VirtualKeys.F5, ToggleScreenInfo },
            { VirtualKeys.N1, DrawMeleeWeapon },
            { VirtualKeys.N2, DrawRangedWeapon },
            { VirtualKeys.RightButton, FreeAim },
            {KeyBind.StatusMenu, OpenStatusMenu }
        };

        static void OpenStatusMenu(bool down)
        {
            if (!down) return;

            if (ArenaClient.Client.Character != null)
            {
                if (GameMode.IsActive && PlayerInfo.HeroInfo.TeamID >= TeamIdent.GMPlayer)
                    GameMode.ActiveMode.OpenStatusMenu();
                else
                    Arena.Menus.StatusMenu.Instance.Open();
            }
        }

        static void DrawMeleeWeapon(bool down)
        {
            if (!down) return;

            var hero = ScriptClient.Client.Character;
            if (hero.ModelInst.IsInAnimation() || hero.IsDead || hero.IsUnconscious)
                return;

            ItemInst weapon;
            if (((weapon = hero.GetDrawnWeapon()) != null && weapon.IsWepMelee) || // undraw
                 (weapon = hero.GetEquipmentBySlot(NPCSlots.OneHanded1)) != null || // draw
                 (weapon = hero.GetEquipmentBySlot(NPCSlots.TwoHanded)) != null) // draw
                NPCInst.Requests.DrawWeapon(hero, weapon);
        }

        static void DrawRangedWeapon(bool down)
        {
            if (!down) return;

            var hero = ScriptClient.Client.Character;
            if (hero.ModelInst.IsInAnimation() || hero.IsDead || hero.IsUnconscious)
                return;

            ItemInst weapon;
            if (((weapon = hero.GetDrawnWeapon()) != null && weapon.IsWepRanged) ||  // undraw
                 (weapon = hero.GetEquipmentBySlot(NPCSlots.Ranged)) != null) // draw
            {
                NPCInst.Requests.DrawWeapon(hero, weapon);
            }
        }

        static void DrawWeapon(bool down)
        {
            if (!down) return;

            var hero = ScriptClient.Client.Character;
            if (hero.ModelInst.IsInAnimation() || hero.IsDead || hero.IsUnconscious)
                return;

            ItemInst weapon;
            if ((weapon = hero.GetDrawnWeapon()) != null ||
               ((weapon = hero.LastUsedWeapon) != null && weapon.Container == hero && weapon.IsEquipped) ||
               (weapon = hero.GetEquipmentBySlot(NPCSlots.OneHanded1)) != null ||
               (weapon = hero.GetEquipmentBySlot(NPCSlots.TwoHanded)) != null ||
               (weapon = hero.GetEquipmentBySlot(NPCSlots.Ranged)) != null)
            {
                NPCInst.Requests.DrawWeapon(hero, weapon);
            }
            else
            {
                NPCInst.Requests.DrawFists(hero);
            }
        }

        static void Jump(bool d)
        {
            var hero = NPCInst.Hero;
            if (!d || hero == null || hero.IsDead || hero.IsUnconscious) return;

            if (hero.ModelInst.GetActiveAniFromLayer(1) != null || hero.BaseInst.gAI.GetFoundLedge() || IsWarmup())
                return;

            var ledge = hero.BaseInst.DetectClimbingLedge();
            if (ledge == null)
            {
                if (hero.BaseInst.gAI.CheckEnoughSpaceMoveForward(true))
                    NPCInst.Requests.Jump(hero);
            }
            else
            {
                NPCInst.Requests.Climb(hero, ledge);
            }
        }

        static Vec3f lastPos;
        static void PrintPosition(bool down)
        {
            var hero = NPCInst.Hero;
            if (!down || hero == null)
                return;

            var pos = hero.GetPosition();
            var ang = hero.GetAngles();

            Log.Logger.Log(pos + " " + ang + " Distance to last: " + lastPos.GetDistance(pos));
            System.IO.File.AppendAllText("positions.txt", string.Format(System.Globalization.CultureInfo.InvariantCulture, "{{ new Vec3f({0}f, {1}f, {2}f), new Angles({3}f, {4}f, {5}f) }},\n", pos.X, pos.Y, pos.Z, ang.Pitch, ang.Yaw, ang.Roll));
            lastPos = pos;
        }

        static void ToggleScoreBoard(bool down)
        {
            if (down)
            {
                if (ArenaClient.FFAJoined)
                {
                    DuelBoardScreen.Instance.Open();
                }
                else if (GameMode.IsActive && ArenaClient.GMJoined)
                {
                    GameMode.ActiveMode.ScoreBoard.Open();
                }
            }
            else
            {
                DuelBoardScreen.Instance.Close();
                if (GameMode.IsActive)
                    GameMode.ActiveMode.ScoreBoard.Close();
            }
        }


        static void CheckFightMove(bool down, FightMoves move)
        {
            if (!down)
                return;

            var hero = ScriptClient.Client.Character;
            if (hero.IsDead || hero.IsUnconscious || hero.Movement != NPCMovement.Stand || !hero.IsInFightMode || hero.Environment.InAir
                || IsWarmup())
                return;

            var drawnWeapon = hero.GetDrawnWeapon();
            if (drawnWeapon != null && drawnWeapon.IsWepRanged)
            {
                if (freeAim)
                    RequestShootFree(hero);
                else if (KeyBind.Action.IsPressed())
                    RequestShootAuto(hero);
            }
            else if (KeyBind.Action.IsPressed())
            {
                NPCInst.Requests.Attack(hero, move);
            }
        }

        static LockTimer screamLock = new LockTimer(3000);
        static void PlayerActionButton(bool down)
        {
            var hero = ScriptClient.Client.Character;
            if (hero.IsDead)
                return;

            if (hero.IsUnconscious)
            {
                if (down && HordeMode.IsActive && PlayerInfo.HeroInfo.TeamID >= TeamIdent.GMPlayer && screamLock.IsReady)
                {
                    NPCInst.Requests.Voice(hero, VoiceCmd.HELP);
                }
                return;
            }

            if (down && HordeMode.IsActive && PlayerInfo.HeroInfo.TeamID >= TeamIdent.GMPlayer
                     && hero.HP > 1 && !hero.ModelInst.IsInAnimation())
            {
                if (PlayerFocus.FocusVob is NPCInst npc && npc.IsUnconscious && npc.TeamID == hero.TeamID && npc.GetDistance(hero) < 300)
                {
                    NPCInst.Requests.HelpUp(hero, npc);
                    return;
                }
            }

            if (freeAim)
            {
                if (!IsWarmup())
                    RequestShootFree(hero);
                return;
            }

            if (hero.IsInFightMode)
            {
                var drawnWeapon = hero.GetDrawnWeapon();
                if (drawnWeapon != null && drawnWeapon.IsWepRanged)
                {
                    NPCInst.Requests.Aim(hero, down);
                }
                else if (down && KeyBind.MoveForward.IsPressed() && !IsWarmup())
                {
                    NPCInst.Requests.Attack(hero, FightMoves.Run);
                }
            }
            else if (down)
            {
                if (PlayerFocus.FocusVob is ItemInst item)
                {
                    NPCInst.Requests.TakeItem(hero, item);
                }
                else if (!ArenaClient.GMJoined && PlayerFocus.FocusVob is NPCInst npc)
                {
                    DuelMode.SendRequest(npc);
                }
            }
        }

        static LockTimer toWarmupTimer = new LockTimer(1000);
        static bool IsWarmup()
        {
            var hero = NPCInst.Hero;
            if (hero != null && ArenaClient.GMJoined && GameMode.ActiveMode.Phase == GamePhase.WarmUp && !HordeMode.IsActive)
            {
                if (toWarmupTimer.IsReady)
                    Sumpfkraut.Menus.ScreenScrollText.AddText("Noch wenige Sekunden!");

                return true;
            }
            return false;
        }

        void LookAround(NPCInst hero)
        {
            const float maxLookSpeed = 2f;

            float rotSpeed = 0;
            if (InputHandler.MouseDistY != 0)
            {
                rotSpeed = Alg.Clamp(-maxLookSpeed, InputHandler.MouseDistY * 0.35f * MouseSpeed, maxLookSpeed);

                var cam = zCAICamera.CurrentCam;
                cam.BestElevation = Alg.Clamp(-50, cam.BestElevation + rotSpeed, 85);
            }

            if (InputHandler.MouseDistX != 0)
            {
                rotSpeed = Alg.Clamp(-maxLookSpeed, InputHandler.MouseDistX * 0.35f * MouseSpeed, maxLookSpeed);

                var cam = zCAICamera.CurrentCam;
                cam.BestAzimuth -= rotSpeed;
            }
        }

        LockTimer dodgeLock = new LockTimer(200);
        const long StrafeInterval = 200 * TimeSpan.TicksPerMillisecond;
        long nextStrafeChange = 0;
        void PlayerUpdate()
        {
            if (ArenaClient.DetectSchinken)
            {
                fwdTelHelper.Update(GameTime.Ticks);
                upTelHelper.Update(GameTime.Ticks);
            }

            NPCInst hero = ScriptClient.Client.Character;
            var gAI = hero.BaseInst.gAI;
            if (hero.IsDead || hero.IsUnconscious)
            {
                LookAround(hero);
                return;
            }

            zCAICamera.CurrentCam.BestAzimuth = 0;

            if (freeAim)
            {
                FreeAiming(hero);
                return;
            }

            DoTurning(hero);

            if (KeyBind.Action.IsPressed())
            {
                if (hero.IsInFightMode)
                {
                    if (enemy == null || !enemy.IsSpawned || enemy.IsDead)
                    {
                        enemy = PlayerFocus.GetFocusNPC();
                        if (enemy == null || !enemy.IsSpawned || enemy.IsDead)
                        {
                            oCNpcFocus.StopHighlightingFX();
                            enemy = null;
                        }
                        else
                        {
                            oCNpcFocus.StartHighlightingFX(enemy.BaseInst.gVob);
                        }
                    }
                }
            }
            else
            {
                if (enemy != null)
                {
                    oCNpcFocus.StopHighlightingFX();
                    enemy = null;
                }

                //hero.BaseInst.gVob.CollectFocusVob(false);
            }

            NPCMovement state = NPCMovement.Stand;
            if (!KeyBind.Action.IsPressed() || hero.Movement != NPCMovement.Stand)
            {
                if (KeyBind.MoveForward.IsPressed()) // move forward
                {
                    state = NPCMovement.Forward;
                }
                else if (KeyBind.MoveBack.IsPressed()) // move backward
                {
                    var drawnWeapon = hero.GetDrawnWeapon();
                    if (hero.IsInFightMode && (drawnWeapon == null || drawnWeapon.IsWepMelee))
                    {
                        if (dodgeLock.IsReady) // don't spam
                        {
                            if (IsWarmup())
                                return;

                            NPCInst.Requests.Attack(hero, FightMoves.Dodge);
                        }
                        return;
                    }
                    else
                    {
                        state = NPCMovement.Backward;
                    }
                }
                else if (KeyBind.MoveLeft.IsPressed()) // strafe left
                {
                    state = NPCMovement.Left;
                }
                else if (KeyBind.MoveRight.IsPressed()) // strafe right
                {
                    state = NPCMovement.Right;
                }
                else
                {
                    state = NPCMovement.Stand;
                }
            }

            if (nextStrafeChange > GameTime.Ticks)
            {
                state = hero.Movement;
            }

            if (state == NPCMovement.Forward)
            {   // FIXME: use only a better CheckEnoughSpaceMoveForward
                if (hero.Movement == NPCMovement.Stand && !gAI.CheckEnoughSpaceMoveForward(true))
                {
                    state = NPCMovement.Stand;
                }
                else
                {
                    gAI.CalcForceModelHalt();
                    if ((gAI.Bitfield0 & zCAIPlayer.Flags.ForceModelHalt) != 0)
                    {
                        gAI.Bitfield0 &= ~zCAIPlayer.Flags.ForceModelHalt;
                        state = NPCMovement.Stand;
                    }
                }
            }
            else if (state == NPCMovement.Backward)
            {
                if (!gAI.CheckEnoughSpaceMoveBackward(true))
                    state = NPCMovement.Stand;
            }
            else if (state == NPCMovement.Left)
            {
                if (!gAI.CheckEnoughSpaceMoveLeft(true))
                    state = NPCMovement.Stand;
            }
            else if (state == NPCMovement.Right)
            {
                if (!gAI.CheckEnoughSpaceMoveRight(true))
                    state = NPCMovement.Stand;
            }

            if (state != NPCMovement.Stand && IsWarmup())
                state = NPCMovement.Stand;

            if (state == NPCMovement.Left || state == NPCMovement.Right || (state == NPCMovement.Forward && hero.IsInFightMode))
            {
                if (hero.Movement != state)
                    nextStrafeChange = GameTime.Ticks + StrafeInterval;
            }
            else
                nextStrafeChange = 0;

            hero.SetMovement(state);
        }

        static NPCInst enemy;
        static void DoTurning(NPCInst hero)
        {
            if (hero.BaseInst.gAI.GetFoundLedge())
                return;

            const float maxTurnFightSpeed = 0.07f;
            if (enemy != null)
            {
                Vec3f heroPos = hero.GetPosition();
                Vec3f enemyPos = enemy.GetPosition();

                float bestYaw = Angles.GetYawFromAtVector(enemyPos - heroPos);
                Angles curAngles = hero.GetAngles();

                float yawDiff = Angles.Difference(bestYaw, curAngles.Yaw);
                curAngles.Yaw += Alg.Clamp(-maxTurnFightSpeed, yawDiff, maxTurnFightSpeed);

                hero.SetAngles(curAngles);
                CheckCombineAim(hero, enemyPos - heroPos);
                return;
            }

            const float maxLookupSpeed = 2f;
            float rotSpeed = 0;
            if (InputHandler.MouseDistY != 0)
            {
                rotSpeed = Alg.Clamp(-maxLookupSpeed, InputHandler.MouseDistY * 0.35f * MouseSpeed, maxLookupSpeed);

                if (hero.Environment.WaterLevel >= 1)
                {
                    hero.BaseInst.gAI.DiveRotateX(rotSpeed);
                }
                else
                {
                    var cam = zCAICamera.CurrentCam;
                    cam.BestElevation = Alg.Clamp(-50, cam.BestElevation + rotSpeed, 85);
                }
            }

            // Fixme: do own turning
            const float maxTurnSpeed = 2f;
            if (!KeyBind.Action.IsPressed())
            {
                float turn = 0;
                if (KeyBind.TurnLeft.IsPressed())
                    turn = -maxTurnSpeed;
                else if (KeyBind.TurnRight.IsPressed())
                    turn = maxTurnSpeed;
                else if (Math.Abs(InputHandler.MouseDistX) > ((rotSpeed > 0.5f && hero.Movement == NPCMovement.Stand) ? 18 : 2))
                {
                    turn = Alg.Clamp(-maxTurnSpeed, InputHandler.MouseDistX * 0.45f * MouseSpeed, maxTurnSpeed);
                }

                if (turn != 0)
                {
                    hero.BaseInst.gAI.Turn(turn, !hero.ModelInst.IsInAnimation());
                    return;
                }
            }
            hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
        }

        static KeyHoldHelper fwdTelHelper = new KeyHoldHelper(500, 100)
        {
            { () =>
                {
                    NPCInst hero = ScriptClient.Client.Character;
                    Vec3f pos = hero.GetPosition();
                    Vec3f dir = (Vec3f)hero.BaseInst.gVob.Direction;
                    pos += dir * 150.0f;
                    hero.SetPosition(pos);
                }, VirtualKeys.K
            }
        };

        static KeyHoldHelper upTelHelper = new KeyHoldHelper(500, 100)
        {
            { () =>
                {
                    NPCInst hero = ScriptClient.Client.Character;
                    Vec3f pos = hero.GetPosition();
                    pos.Y += 200.0f;
                    hero.SetPosition(pos);
                    hero.BaseInst.gVob.SetPhysicsEnabled(false);
                }, VirtualKeys.F8
            }
        };

        static void RequestShootAuto(NPCInst hero)
        {
            Vec3f start;
            using (var matrix = Gothic.Types.zMat4.Create())
            {
                var weapon = hero.GetDrawnWeapon();
                var node = (weapon == null || weapon.ItemType == ItemTypes.WepBow) ? oCNpc.NPCNodes.RightHand : oCNpc.NPCNodes.LeftHand;
                hero.BaseInst.gVob.GetTrafoModelNodeToWorld(node, matrix);
                start = (Vec3f)matrix.Position;
            }

            const zCWorld.zTraceRay traceType = zCWorld.zTraceRay.Ignore_Alpha | zCWorld.zTraceRay.Ignore_Projectiles | zCWorld.zTraceRay.Ignore_Vob_No_Collision | zCWorld.zTraceRay.Ignore_NPC;

            Vec3f dir = enemy == null ? (Vec3f)hero.BaseInst.gVob.Direction : (enemy.GetPosition() - start).Normalise();
            Vec3f ray = 500000f * dir;

            Vec3f end;
            using (var zStart = start.CreateGVec())
            using (var zRay = ray.CreateGVec())
            {
                var gWorld = GothicGlobals.Game.GetWorld();

                if (gWorld.TraceRayNearestHit(zStart, zRay, traceType))
                {
                    end = (Vec3f)gWorld.Raytrace_FoundIntersection;
                }
                else
                {
                    end = start + ray;
                }
            }

            NPCInst.Requests.Shoot(hero, start, end);
        }

        static void RequestShootFree(NPCInst hero)
        {
            CalcRangedTrace(hero, out Vec3f start, out Vec3f end);
            NPCInst.Requests.Shoot(hero, start, end);
        }

        static void CalcRangedTrace(NPCInst npc, out Vec3f start, out Vec3f end)
        {
            Vec3f projStartPos;
            using (var matrix = Gothic.Types.zMat4.Create())
            {
                var weapon = npc.GetDrawnWeapon();
                var node = (weapon == null || weapon.ItemType == ItemTypes.WepBow) ? oCNpc.NPCNodes.RightHand : oCNpc.NPCNodes.LeftHand;
                npc.BaseInst.gVob.GetTrafoModelNodeToWorld(node, matrix);
                projStartPos = (Vec3f)matrix.Position;
            }

            const zCWorld.zTraceRay traceType = zCWorld.zTraceRay.Ignore_Alpha | zCWorld.zTraceRay.Ignore_Projectiles | zCWorld.zTraceRay.Ignore_Vob_No_Collision | zCWorld.zTraceRay.Ignore_NPC;

            var camVob = GothicGlobals.Game.GetCameraVob();
            start = (Vec3f)camVob.Position;
            Vec3f ray = 500000f * (Vec3f)camVob.Direction;
            end = start + ray;

            using (var zStart = start.CreateGVec())
            using (var zRay = ray.CreateGVec())
            {
                var gWorld = GothicGlobals.Game.GetWorld();

                if (gWorld.TraceRayNearestHit(zStart, zRay, traceType))
                {
                    end = (Vec3f)gWorld.Raytrace_FoundIntersection;
                }

                start = projStartPos;
                ray = end - start;

                start.SetGVec(zStart);
                ray.SetGVec(zRay);

                if (gWorld.TraceRayNearestHit(zStart, zRay, traceType))
                {
                    end = (Vec3f)gWorld.Raytrace_FoundIntersection;
                }
            }
        }

        static Sumpfkraut.GUI.GUCWorldSprite crosshair;
        static bool freeAim = false;
        static void FreeAim(bool down)
        {

            if (crosshair == null)
            {
                crosshair = new Sumpfkraut.GUI.GUCWorldSprite(10, 10);
                crosshair.SetBackTexture("crosshair.tga");
                crosshair.ShowOutOfScreen = false;
            }

            var hero = NPCInst.Hero;

            string CamModFreeAim = "CAMMODRANGED_FREEAIM";
            if (hero.ModelDef.Visual == "ORC.MDS" || hero.ModelDef.Visual == "DRACONIAN.MDS")
                CamModFreeAim += "_ORC";

            if (down && hero != null && !hero.IsDead && hero.IsInFightMode
                && !hero.Environment.InAir && !hero.ModelInst.IsInAnimation())
            {
                var drawnWeapon = hero.GetDrawnWeapon();
                if (drawnWeapon != null && drawnWeapon.IsWepRanged)
                {
                    if (!freeAim)
                    {

                        hero.SetMovement(NPCMovement.Stand);
                        NPCInst.Requests.Aim(hero, true);

                        // no auto-lock
                        oCNpcFocus.StopHighlightingFX();
                        enemy = null;

                        // zoom in
                        FOVTransition(60, TimeSpan.TicksPerSecond / 2);

                        zCAICamera.CamModRanged.Set(CamModFreeAim); // replace so gothic sets it to this while in bow mode
                        zCAICamera.CurrentCam.SetByScript(CamModFreeAim); // change camera
                        freeAim = true;
                    }
                    return;
                }
            }

            if (freeAim)
            {
                NPCInst.Requests.Aim(hero, false);
                crosshair.Hide();
                FOVTransition(90, TimeSpan.TicksPerSecond / 2);

                var cam = zCAICamera.CurrentCam;
                zCAICamera.CamModRanged.Set("CAMMODRANGED"); // reset
                if (cam.CurrentMode.ToString().Equals(CamModFreeAim, StringComparison.OrdinalIgnoreCase))
                    cam.SetByScript("CAMMODRANGED"); // change camera
                freeAim = false;
            }
        }

        static void FreeAiming(NPCInst hero)
        {
            if (InputHandler.MouseDistY != 0)
            {
                const float maxSpeed = 1.0f;
                float rotSpeed = Alg.Clamp(-maxSpeed, InputHandler.MouseDistY * 0.16f * MouseSpeed, maxSpeed);

                var cam = zCAICamera.CurrentCam;
                cam.BestElevation = Alg.Clamp(-65, cam.BestElevation + rotSpeed, 85);
            }

            if (InputHandler.MouseDistX != 0)
            {
                const float maxSpeed = 1.2f;

                float rotSpeed = Alg.Clamp(-maxSpeed, InputHandler.MouseDistX * 0.20f * MouseSpeed, maxSpeed);
                hero.BaseInst.gAI.Turn(rotSpeed, false);
            }

            CalcRangedTrace(hero, out Vec3f start, out Vec3f end);
            crosshair.SetTarget(end);

            CheckCombineAim(hero, (Vec3f)GothicGlobals.Game.GetCameraVob().Direction);
        }

        static void CheckCombineAim(NPCInst hero, Vec3f direction)
        {
            var gModel = hero.BaseInst.gModel;
            int aniID;
            if (gModel.IsAnimationActive("S_BOWAIM"))
            {
                aniID = gModel.GetAniIDFromAniName("S_BOWAIM");
            }
            else if (gModel.IsAnimationActive("S_CBOWAIM"))
            {
                aniID = gModel.GetAniIDFromAniName("S_CBOWAIM");
            }
            else
            {
                if (freeAim)
                    crosshair.Hide();
                return;
            }

            if (freeAim)
                crosshair.Show();

            Angles heroAngles = hero.GetAngles();
            Angles angles = Angles.FromAtVector(direction);
            float pitch = Angles.Difference(angles.Pitch, heroAngles.Pitch);
            float yaw = Angles.Difference(angles.Yaw, heroAngles.Yaw);

            float x = 0.6f;// Alg.Clamp(0, 0.5f - yaw / Angles.PI, 1);
            float y = Alg.Clamp(0, 0.47f - (pitch < 0 ? 1.2f : 1.0f) * pitch / Angles.PI, 1);

            hero.BaseInst.gAI.InterpolateCombineAni(x, y, aniID);
        }
    }
}
