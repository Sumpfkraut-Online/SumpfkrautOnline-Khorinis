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

        static int inc { get { return TFFAClient.Client.Character.DrawnWeapon.Definition.ItemType == ItemTypes.Wep1H ? 9 : 0; } }

        static void KeyDown(VirtualKeys key, long now)
        {
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                activeMenu.KeyDown(key, now);
                return;
            }

            if (GUC.Network.GameClient.Client.Character == null)
                return;

            if (key == VirtualKeys.Escape)
            {
                MainMenu.Menu.Open();
            }
            else if (key == VirtualKeys.Tab)
            {
                Scoreboard.Menu.Open();
            }
            else if (key == VirtualKeys.P)
            {
                if (TFFAClient.Client.Character != null && TFFAClient.Client.Character.IsSpawned)
                {
                    string str = TFFAClient.Client.Character.BaseInst.GetPosition() + " " + TFFAClient.Client.Character.BaseInst.GetDirection() + "\r\n";
                    Log.Logger.Log(str);
                    System.IO.File.AppendAllText(Program.ProjectPath + "SavedLocations.txt", str);
                }
            }
            else if (key == VirtualKeys.Control)
            {
                Gothic.Objects.oCNpcFocus.StartHighlightingFX(TFFAClient.Client.Character.BaseInst.gVob.GetFocusNpc());

                if (TFFAClient.Client.Character.State == NPCStates.MoveForward)
                {
                    // run attack
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HRun + inc, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
            }
            else if (InputHandler.IsPressed(VirtualKeys.Control))
            {
                if (key == VirtualKeys.Up)
                {
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.CurrentAni != null)
                    {
                        int curID = TFFAClient.Client.Character.CurrentAni.AniJob.ID;
                        if (((curID >= (int)SetAnis.Attack2HFwd1 && curID < (int)SetAnis.Attack2HFwd4) || (curID >= (int)SetAnis.Attack1HFwd1 && curID < (int)SetAnis.Attack1HFwd4))
                            && TFFAClient.Client.Character.Model.TryGetAniJob(curID + 1, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                            return;
                        }
                    }
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HFwd1 + inc, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Left)
                {
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HLeft + inc, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Right)
                {
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HRight + inc, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Down)
                {
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HParry + inc, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
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

            if (GUC.Network.GameClient.Client.Character == null)
                return;


            if (key == VirtualKeys.Tab)
            {
                Scoreboard.Menu.Close();
            }
            else if (key == VirtualKeys.Control)
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

            if (GUC.Network.GameClient.Client.Character == null)
                return;



            if (!InputHandler.IsPressed(VirtualKeys.Control))
            {
                TFFAClient.Client.Character.BaseInst.gVob.HumanAI.CheckFocusVob(0);
                if (InputHandler.IsPressed(VirtualKeys.A)) // strafe left
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveLeft);
                }
                else if (InputHandler.IsPressed(VirtualKeys.D)) // strafe right
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveRight);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Up) || InputHandler.IsPressed(VirtualKeys.W)) // move forward
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveForward);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Down) || InputHandler.IsPressed(VirtualKeys.S)) // move backward
                {
                    //GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveBackward)
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HDodge + inc, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else // not moving
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.Stand);
                }

                // Do turning
                if (InputHandler.IsPressed(VirtualKeys.Left) || InputHandler.IsPressed(VirtualKeys.Q))
                {
                    GUC.Network.GameClient.Client.Character.gVob.AniCtrl.Turn(-2.5f, true);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Right) || InputHandler.IsPressed(VirtualKeys.E))
                {
                    GUC.Network.GameClient.Client.Character.gVob.AniCtrl.Turn(2.5f, true);
                }
                else
                {
                    GUC.Network.GameClient.Client.Character.gVob.AniCtrl.StopTurnAnis();
                }
            }
            else
            {
                var fnpc = TFFAClient.Client.Character.BaseInst.gVob.GetFocusNpc();
                if (fnpc.Address == 0 || fnpc.HP <= 0)
                    TFFAClient.Client.Character.BaseInst.gVob.HumanAI.CheckFocusVob(0);
                else
                    TurnToEnemy();
                GUC.Network.GameClient.Client.Character.gVob.AniCtrl.StopTurnAnis();
            }

            if (InputHandler.IsPressed(VirtualKeys.K))
            {
                if (nextFwdTeleport < now)
                {
                    Vec3f pos = GUC.Network.GameClient.Client.Character.GetPosition();
                    Vec3f dir = GUC.Network.GameClient.Client.Character.GetDirection();
                    pos += dir * 125.0f;
                    GUC.Network.GameClient.Client.Character.SetPosition(pos);

                    nextFwdTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
                }
            }
            if (InputHandler.IsPressed(VirtualKeys.F8))
            {
                if (nextUpTeleport < now)
                {
                    Vec3f pos = GUC.Network.GameClient.Client.Character.GetPosition();
                    pos.Y += 200.0f;
                    GUC.Network.GameClient.Client.Character.SetPosition(pos);
                    GUC.Network.GameClient.Client.Character.gVob.SetPhysicsEnabled(false);

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
                //Log.Logger.Log(dot);
            }

        }
    }
}
