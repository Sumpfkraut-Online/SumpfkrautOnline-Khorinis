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
            { KeyBind.Jump, d => { if (d) NPCInst.Requests.Jump(ScriptClient.Client.Character); } },
            { KeyBind.DrawFists, d => { if (d) NPCInst.Requests.DrawFists(ScriptClient.Client.Character); } },
            { KeyBind.MoveForward, d => CheckFightMove(d, FightMoves.Fwd) },
            { KeyBind.TurnLeft, d => CheckFightMove(d, FightMoves.Left) },
            { KeyBind.TurnRight, d => CheckFightMove(d, FightMoves.Right) },
            { KeyBind.MoveBack, d => CheckFightMove(d, FightMoves.Parry) },
            { KeyBind.Action, PlayerActionButton },
            { KeyBind.DrawWeapon, DrawWeapon },
            { KeyBind.OpenScoreBoard, ToggleScoreBoard },
            { KeyBind.OpenAllChat, d => { if (d) ChatMenu.Menu.OpenAllChat(); } },
            { KeyBind.OpenTeamChat, d => { if (d) ChatMenu.Menu.OpenTeamChat(); } },
            { KeyBind.Inventory, d => { if (d && TeamMode.TeamDef == null) Sumpfkraut.Menus.PlayerInventory.Menu.Open(); } },
            { VirtualKeys.P, PrintPosition },
            { VirtualKeys.F2, d => Menus.PlayerList.TogglePlayerList() },
            { VirtualKeys.F3, ToggleG1Camera },
        };

        static bool g1 = false;
        static void ToggleG1Camera(bool down)
        {
            if (down && zCCamera.ActiveCamera.Address != 0)
            {
                var screen = GUI.GUCView.GetScreenSize();
                if (screen.X != 0 && screen.Y != 0)
                {
                    const float FOV = 90.0f;
                    if (g1)
                    {
                        if (Gothic.System.zCParser.GetCameraParser().LoadDat("CAMERA.DAT"))
                        {
                            zCAICamera.CurrentCam.CreateInstance(zCAICamera.CurrentCam.CurrentMode);
                            zCCamera.ActiveCamera.SetFOV(FOV, (float)(FOV * 0.75d));
                            g1 = false;
                        }
                    }
                    else
                    {
                        if (Gothic.System.zCParser.GetCameraParser().LoadDat("CAMERA.DAT.G1"))
                        {
                            zCAICamera.CurrentCam.CreateInstance(zCAICamera.CurrentCam.CurrentMode);
                            double ratio = (double)screen.Y / screen.X;
                            zCCamera.ActiveCamera.SetFOV(FOV, (float)(FOV * ratio));
                            g1 = true;
                        }
                    }
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
            if (hero.IsInFightMode)
            {
                NPCInst.Requests.Attack(hero, move);
            }
        }

        static void PlayerActionButton(bool down)
        {
            var hero = ScriptClient.Client.Character;
            if (KeyBind.MoveForward.IsPressed())
            {
                if (down)
                    CheckFightMove(down, FightMoves.Run);
            }
            else if (!hero.IsDead)
            {
                if (hero.IsInFightMode)
                {
                    if (down)
                    {
                        var focus = hero.GetFocusVob();
                        enemy = focus is NPCInst ? (NPCInst)focus : null;
                    }
                    else
                    {
                        enemy = null;
                    }
                    if (enemy == null)
                        Gothic.Objects.oCNpcFocus.StopHighlightingFX();
                    else
                        Gothic.Objects.oCNpcFocus.StartHighlightingFX(enemy.BaseInst.gVob);
                }
                else if (down)
                {
                    var focusVob = hero.GetFocusVob();
                    if (focusVob is NPCInst)
                        DuelMode.SendRequest((NPCInst)focusVob);
                }
            }
        }

        long nextDodgeTime = 0;
        LockTimer strafeLock = new LockTimer(150);
        void PlayerUpdate()
        {
            fwdTelHelper.Update(GameTime.Ticks);
            upTelHelper.Update(GameTime.Ticks);

            NPCInst hero = ScriptClient.Client.Character;
            if (hero.IsDead)
                return;

            DoTurning(hero);

            NPCMovement state = hero.Movement;
            if (KeyBind.MoveLeft.IsPressed()) // strafe left
            {
                if (strafeLock.IsReady)
                    state = NPCMovement.Left;
            }
            else if (KeyBind.MoveRight.IsPressed()) // strafe right
            {
                if (strafeLock.IsReady)
                    state = NPCMovement.Right;
            }
            else if (!KeyBind.Action.IsPressed())
            {
                if (enemy != null)
                {
                    Gothic.Objects.oCNpcFocus.StopHighlightingFX();
                    enemy = null;
                }

                hero.BaseInst.gAI.CheckFocusVob(1);
                if (KeyBind.MoveForward.IsPressed()) // move forward
                {
                    state = NPCMovement.Forward;
                }
                else if (KeyBind.MoveBack.IsPressed()) // move backward
                {
                    if (hero.IsInFightMode)
                    {
                        if (nextDodgeTime < GameTime.Ticks) // don't spam
                        {
                            NPCInst.Requests.Attack(hero, FightMoves.Dodge);
                            nextDodgeTime = GameTime.Ticks + 50 * TimeSpan.TicksPerMillisecond;
                        }
                        return;
                    }
                    else
                    {
                        state = NPCMovement.Backward;
                    }
                }
                else
                {
                    state = NPCMovement.Stand;
                }
            }
            hero.SetMovement(state);
        }

        static NPCInst enemy;
        static void DoTurning(NPCInst hero)
        {
            if (enemy != null)
            {
                Vec3f heroPos = hero.GetPosition();
                Vec3f heroDir = hero.GetDirection();
                Vec3f enemyPos = enemy.GetPosition();

                Vec3f dir = (new Vec3f(enemyPos.X, 0, enemyPos.Z) - new Vec3f(heroPos.X, 0, heroPos.Z)).Normalise();
                Vec3f diff = new Vec3f(heroDir.X, 0, heroDir.Z) - dir;
                float len = diff.GetLength();
                const float maxSpeed = 0.075f;
                if (len > maxSpeed)
                    diff = new Vec3f(diff.X / len * maxSpeed, 0, diff.Z / len * maxSpeed);

                hero.SetDirection(heroDir - diff);
                return;
            }

            // Fixme: do own turning
            if (!KeyBind.Action.IsPressed())
            {
                if (KeyBind.TurnLeft.IsPressed())
                {
                    hero.BaseInst.gVob.AniCtrl.Turn(-2f, !hero.ModelInst.IsInAnimation());
                    return;
                }
                else if (KeyBind.TurnRight.IsPressed())
                {
                    hero.BaseInst.gVob.AniCtrl.Turn(2f, !hero.ModelInst.IsInAnimation());
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
