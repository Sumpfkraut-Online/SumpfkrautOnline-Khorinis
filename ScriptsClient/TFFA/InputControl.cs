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
            else if (key == VirtualKeys.Control)
            {
                if (TFFAClient.Client.Character.State == NPCStates.MoveForward)
                {
                    // run attack
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HRun, out job))
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
                        if (curID >= (int)SetAnis.Attack2HFwd1 && curID < (int)SetAnis.Attack2HFwd4
                            && TFFAClient.Client.Character.Model.TryGetAniJob(curID + 1, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                            return;
                        }
                    }
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HFwd1, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Left)
                {
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HLeft, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Right)
                {
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HRight, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Down)
                {
                    ScriptAniJob job;
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HParry, out job))
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
        }

        public static void Update(long now)
        {
            if (GUC.Network.GameClient.Client.Character == null)
                return;

            if (!InputHandler.IsPressed(VirtualKeys.Control))
            {
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
                    if (TFFAClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HDodge, out job))
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
    }
}
