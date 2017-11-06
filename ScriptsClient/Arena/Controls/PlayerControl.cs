using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Controls;
using GUC.Utilities;
using Gothic.Objects;

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
            { KeyBind.OpenScoreBoard, ToggleScoreBoard },
            { KeyBind.OpenAllChat, d => { if (d) ChatMenu.Menu.OpenAllChat(); } },
            { KeyBind.OpenTeamChat, d => { if (d) ChatMenu.Menu.OpenTeamChat(); } },
            { KeyBind.Inventory, d => { if (d && TeamMode.TeamDef == null) Sumpfkraut.Menus.PlayerInventory.Menu.Open(); } },
            { VirtualKeys.P, PrintPosition },
            { VirtualKeys.F2, d => Menus.PlayerList.TogglePlayerList() },
            { VirtualKeys.F3, ToggleG1Camera },
            { VirtualKeys.F5, ToggleScreenInfo },
        };

        static void Jump(bool d)
        {
            var hero = NPCInst.Hero;
            if (!d || hero == null) return;

            if (hero.ModelInst.GetActiveAniFromLayer(1) == null && !CheckWarmup())
            {
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
        }

        static void PrintPosition(bool down)
        {
            var hero = NPCInst.Hero;
            if (!down || hero == null)
                return;

            var pos = hero.GetPosition();
            var dir = hero.GetDirection();

            Log.Logger.Log(pos + " " + dir);
            System.IO.File.AppendAllText("positions.txt", string.Format(System.Globalization.CultureInfo.InvariantCulture, "{{ new Vec3f({0}f, {1}f, {2}f), new Vec3f({3}f, {4}f, {5}f) }},\n", pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z));
        }

        static void ToggleScoreBoard(bool down)
        {
            if (down)
            {
                if (TeamMode.IsRunning && TeamMode.TeamDef != null)
                {
                    TOBoardScreen.Instance.Open();
                }
                else
                {
                    DuelBoardScreen.Instance.Open();
                }
            }
            else
            {
                DuelBoardScreen.Instance.Close();
                TOBoardScreen.Instance.Close();
            }
        }

        static void DrawWeapon(bool down)
        {
            if (!down) return;

            var hero = ScriptClient.Client.Character;
            if (hero.ModelInst.IsInAnimation())
                return;

            if (hero.MeleeWeapon != null)
                NPCInst.Requests.DrawWeapon(hero, hero.MeleeWeapon);
            else if (hero.DrawnWeapon != null)
                NPCInst.Requests.DrawWeapon(hero, hero.DrawnWeapon);
            else
                NPCInst.Requests.DrawFists(hero);
        }

        static void CheckFightMove(bool down, FightMoves move)
        {
            if (!down || !KeyBind.Action.IsPressed())
                return;

            var hero = ScriptClient.Client.Character;
            if (hero.Movement == NPCMovement.Stand && hero.IsInFightMode)
            {
                if (CheckWarmup())
                    return;

                NPCInst.Requests.Attack(hero, move);
            }
        }

        static void PlayerActionButton(bool down)
        {
            if (!down) return;

            var hero = ScriptClient.Client.Character;
            if (hero.IsDead) return;

            if (hero.IsInFightMode)
            {
                if (KeyBind.MoveForward.IsPressed() && !CheckWarmup())
                {
                    NPCInst.Requests.Attack(hero, FightMoves.Run);
                }
            }
            else
            {
                if (hero.TeamID == -1)
                {
                    var focusVob = hero.GetFocusVob();
                    if (focusVob is NPCInst)
                        DuelMode.SendRequest((NPCInst)focusVob);
                }
            }
        }

        static LockTimer toWarmupTimer = new LockTimer(1000);
        static bool CheckWarmup()
        {
            var hero = NPCInst.Hero;
            if (hero != null && hero.TeamID != -1 && TeamMode.Phase == TOPhases.Warmup)
            {
                if (toWarmupTimer.IsReady)
                    Sumpfkraut.Menus.ScreenScrollText.AddText("Noch wenige Sekunden!");

                return true;
            }
            return false;
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
            if (hero.IsDead)
                return;

            DoTurning(hero);

            if (KeyBind.Action.IsPressed())
            {
                if (hero.IsInFightMode)
                {
                    if (enemy == null || !enemy.IsSpawned || enemy.IsDead)
                    {
                        var focus = hero.GetFocusVob();
                        enemy = focus is NPCInst ? (NPCInst)focus : null;
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
                gAI.CheckFocusVob(1);
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
                    if (hero.IsInFightMode)
                    {
                        if (dodgeLock.IsReady) // don't spam
                        {
                            if (CheckWarmup())
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

            if (state != NPCMovement.Stand && CheckWarmup())
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
            const float maxTurnFightSpeed = 0.075f;
            if (enemy != null)
            {
                Vec3f heroPos = hero.GetPosition();
                Vec3f heroDir = hero.GetDirection();
                Vec3f enemyPos = enemy.GetPosition();

                Vec3f dir = (new Vec3f(enemyPos.X, 0, enemyPos.Z) - new Vec3f(heroPos.X, 0, heroPos.Z)).Normalise();
                Vec3f diff = new Vec3f(heroDir.X, 0, heroDir.Z) - dir;
                float len = diff.GetLength();
                if (len > maxTurnFightSpeed)
                    diff = new Vec3f(diff.X / len * maxTurnFightSpeed, 0, diff.Z / len * maxTurnFightSpeed);

                hero.SetDirection(heroDir - diff);
                return;
            }

            const float maxLookupSpeed = 2f;
            float rotSpeed = 0;
            if (InputHandler.MouseDistY != 0)
            {
                rotSpeed = InputHandler.MouseDistY * 0.1f;
                if (rotSpeed > maxLookupSpeed) rotSpeed = maxLookupSpeed;
                else if (rotSpeed < -maxLookupSpeed) rotSpeed = -maxLookupSpeed;
                zCAICamera.CurrentCam.BestRotX += rotSpeed;
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
                    turn = InputHandler.MouseDistX * 0.15f;
                    if (turn > maxTurnSpeed) turn = maxTurnSpeed;
                    else if (turn < -maxTurnSpeed) turn = -maxTurnSpeed;
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
                    Vec3f dir = hero.GetDirection();
                    pos += dir * 125.0f;
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
    }

}
