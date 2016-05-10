using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Client.Scripts.Sumpfkraut.Menus;
using GUC.Enumeration;
using GUC.Types;
using GUC.Scripts.TFFA;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Client.Scripts.TFFA
{
    static class InputControl
    {
        static bool inited = false;
        public static void Init()
        {
            if (inited)
            {
                return;
            }
            inited = true;

            InputHandler.OnKeyDown += KeyDown;
            InputHandler.OnKeyUp += KeyUp;
        }

        static Random random = new Random();
        static void KeyDown(VirtualKeys key, long now)
        {
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                activeMenu.KeyDown(key, now);
                return;
            }

            if (key == VirtualKeys.Escape)
            {
                MainMenu.Menu.Open();
            }
            else if (key == VirtualKeys.Tab)
            {
                Scoreboard.Menu.Open();
            }

            if (TFFAClient.Client.Character == null)
                return;

            NPCInst Hero = TFFAClient.Client.Character;

            if (key == VirtualKeys.P)
            {
                if (Hero.IsSpawned)
                {
                    string str = Hero.BaseInst.GetPosition() + " " + Hero.BaseInst.GetDirection() + "\r\n";
                    Log.Logger.Log(str);
                    System.IO.File.AppendAllText(Program.ProjectPath + "SavedLocations.txt", str);
                }
            }

            if (TFFAClient.Status == TFFAPhase.Waiting)
                return;

            if (key == VirtualKeys.Control || key == VirtualKeys.LeftButton)
            {
                Gothic.Objects.oCNpcFocus.StartHighlightingFX(Hero.BaseInst.gVob.GetFocusNpc());

                // RUN ATTACK
                if (Hero.Movement == MoveState.Forward)
                {
                    ScriptAniJob job;
                    if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Run, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
            }

            if (key == VirtualKeys.Menu) // JUMPING
            {
                var ledge = Hero.BaseInst.DetectClimbingLedge();
                if (ledge != null)
                {
                    float dist = ledge.Location.Y - Hero.BaseInst.gVob.HumanAI.FeetY;

                    if (dist > 205) // CLIMB HIGH
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.ClimbHigh, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, ledge);
                            return;
                        }
                    }
                    else if (dist > 105) // CLIMB MID
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.ClimbMid, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, ledge);
                            return;
                        }
                    }
                    else if (dist > 60) // CLIMB LOW
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.ClimbLow, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, ledge);
                            return;
                        }
                    }
                }

                if (Hero.Movement == MoveState.Forward)
                {
                    ScriptAniJob job;
                    if (Hero.Model.TryGetAniJob((int)SetAnis.JumpRun, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else
                {
                    ScriptAniJob job;
                    if (Hero.Model.TryGetAniJob((int)SetAnis.JumpFwd, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
            }
            else if (InputHandler.IsPressed(VirtualKeys.Control) || InputHandler.IsPressed(VirtualKeys.LeftButton))
            {
                if (key == VirtualKeys.Up || key == VirtualKeys.W)
                {
                    // FORWARD COMBOS
                    ScriptAniJob job;
                    if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd1, out job) && Hero.GetActiveAniFromAniID(job.ID) != null)
                    {
                        if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd2, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                        }
                    }
                    else if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd2, out job) && Hero.GetActiveAniFromAniID(job.ID) != null)
                    {
                        if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd3, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                        }
                    }
                    else if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd3, out job) && Hero.GetActiveAniFromAniID(job.ID) != null)
                    {
                        if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd4, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                        }
                    }
                    else if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd1, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Left || key == VirtualKeys.Q) // LEFT ATTACK
                {
                    ScriptAniJob job;
                    if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Left, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Right || key == VirtualKeys.D) // RIGHT ATTACK
                {
                    ScriptAniJob job;
                    if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Right, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Down || key == VirtualKeys.A) // PARADE
                {
                    ScriptAniJob job;
                    List<ScriptAniJob> parries = new List<ScriptAniJob>();
                    for (int i = (int)NPCInst.AttackMove.Parry1; i <= (int)NPCInst.AttackMove.Parry3; i++)
                        if (Hero.TryGetAttackFromMove((NPCInst.AttackMove)i, out job))
                            parries.Add(job);

                    TFFAClient.Client.BaseClient.DoStartAni(parries[random.Next(0, parries.Count)].BaseAniJob);
                }
            }
        }

        static void KeyUp(VirtualKeys key, long now)
        {
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                activeMenu.KeyUp(key, now);
                return;
            }

            if (key == VirtualKeys.Tab)
            {
                Scoreboard.Menu.Close();
            }

            if (GUC.Network.GameClient.Client.Character == null || TFFAClient.Status == TFFAPhase.Waiting)
                return;

            if (key == VirtualKeys.Control || key == VirtualKeys.LeftButton)
            {
                Gothic.Objects.oCNpcFocus.StopHighlightingFX();
            }
        }

        public static void Update(long now)
        {
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                return;
            }

            if (GUC.Network.GameClient.Client.IsSpectating)
            {
                if (InputHandler.MouseDistY != 0)
                {
                    Gothic.oCGame.GetCameraVob().RotateLocalX(InputHandler.MouseDistY * 0.2f);
                }
                if (InputHandler.MouseDistX != 0)
                {
                    Gothic.oCGame.GetCameraVob().RotateWorldY(InputHandler.MouseDistX * 0.2f);
                }

                if (InputHandler.IsPressed(VirtualKeys.Up) || InputHandler.IsPressed(VirtualKeys.W))
                {
                    var cam = Gothic.oCGame.GetCameraVob();
                    var dir = new Vec3f(cam.Direction) * 20.0f;
                    cam.MoveWorld(dir.X, dir.Y, dir.Z);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Down) || InputHandler.IsPressed(VirtualKeys.S))
                {
                    var cam = Gothic.oCGame.GetCameraVob();
                    var dir = new Vec3f(cam.Direction) * -20.0f;
                    cam.MoveWorld(dir.X, dir.Y, dir.Z);
                }

                return;
            }

            if (GUC.Network.GameClient.Client.Character == null || TFFAClient.Status == TFFAPhase.Waiting)
                return;

            NPCInst Hero = TFFAClient.Client.Character;

            if (InputHandler.MouseDistX != 0)
            {
                Hero.BaseInst.gVob.AniCtrl.Turn(InputHandler.MouseDistX * 0.05f, false);
            }

            if (!InputHandler.IsPressed(VirtualKeys.Control) && !InputHandler.IsPressed(VirtualKeys.LeftButton))
            {
                // Do turning
                if (InputHandler.IsPressed(VirtualKeys.Left) || InputHandler.IsPressed(VirtualKeys.Q))
                {
                    Hero.BaseInst.gVob.AniCtrl.Turn(-2.5f, true);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Right) || InputHandler.IsPressed(VirtualKeys.E))
                {
                    Hero.BaseInst.gVob.AniCtrl.Turn(2.5f, true);
                }
                else
                {
                    Hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
                }

                TFFAClient.Client.Character.BaseInst.gVob.HumanAI.CheckFocusVob(0);
                if (InputHandler.IsPressed(VirtualKeys.A)) // strafe left
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Left);
                }
                else if (InputHandler.IsPressed(VirtualKeys.D)) // strafe right
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Right);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Up) || InputHandler.IsPressed(VirtualKeys.W)) // move forward
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Forward);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Down) || InputHandler.IsPressed(VirtualKeys.S)) // move backward
                {
                    //GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveBackward)
                    ScriptAniJob job;
                    if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Dodge, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else // not moving
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Stand);
                }
            }
            else
            {
                var fnpc = Hero.BaseInst.gVob.GetFocusNpc();
                if (fnpc.Address == 0 || fnpc.HP <= 0)
                    Hero.BaseInst.gVob.HumanAI.CheckFocusVob(0);
                else
                    TurnToEnemy();
                Hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
            }

            if (InputHandler.IsPressed(VirtualKeys.K))
            {
                if (nextFwdTeleport < now)
                {
                    Vec3f pos = Hero.BaseInst.GetPosition();
                    Vec3f dir = Hero.BaseInst.GetDirection();
                    pos += dir * 125.0f;
                    Hero.BaseInst.SetPosition(pos);

                    nextFwdTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
                }
            }
            if (InputHandler.IsPressed(VirtualKeys.F8))
            {
                if (nextUpTeleport < now)
                {
                    Vec3f pos = GUC.Network.GameClient.Client.Character.GetPosition();
                    pos.Y += 200.0f;
                    Hero.BaseInst.SetPosition(pos);
                    Hero.BaseInst.gVob.SetPhysicsEnabled(false);

                    nextUpTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
                }
            }
        }

        static long nextFwdTeleport = 0;
        static long nextUpTeleport = 0;

        static void TurnToEnemy()
        {
            WorldObjects.NPC e;
            if (WorldObjects.World.Current.TryGetVobByAddress(TFFAClient.Client.Character.BaseInst.gVob.GetFocusNpc().Address, out e))
            {
                NPCInst npc = TFFAClient.Client.Character;
                NPCInst enemy = (NPCInst)e.ScriptObject;
                Vec3f npcDir = npc.BaseInst.GetDirection();
                Vec3f npcPos = npc.BaseInst.GetPosition();
                Vec3f enemyPos = e.GetPosition();

                Vec3f dir = (new Vec3f(enemyPos.X, 0, enemyPos.Z) - new Vec3f(npcPos.X, 0, npcPos.Z)).Normalise();
                Vec3f diff = new Vec3f(npcDir.X, 0, npcDir.Z) - dir;
                float len = diff.GetLength();
                const float maxSpeed = 0.1f;
                if (len > maxSpeed)
                    diff = new Vec3f(diff.X / len * maxSpeed, 0, diff.Z / len * maxSpeed);

                npc.BaseInst.SetDirection(npcDir - diff);
            }

        }
    }
}
