using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects;

namespace GUC.Scripts.Sumpfkraut
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
        }

        static void KeyUp(VirtualKeys key, long now)
        {
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                activeMenu.KeyUp(key, now);
                return;
            }
        }

        public static void Update(long now)
        {
            var hero = NPCInst.Hero;

            if (hero == null)
                return;

            if (!InputHandler.IsPressed(VirtualKeys.Control))
            {
                if (InputHandler.IsPressed(VirtualKeys.A)) // strafe left
                {
                    hero.SetMovement(NPCMovement.Left);
                }
                else if (InputHandler.IsPressed(VirtualKeys.D)) // strafe right
                {
                    hero.SetMovement(NPCMovement.Right);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Up) || InputHandler.IsPressed(VirtualKeys.W)) // move forward
                {
                    hero.SetMovement(NPCMovement.Forward);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Down) || InputHandler.IsPressed(VirtualKeys.S)) // move backward
                {
                    hero.SetMovement(NPCMovement.Backward);
                }
                else // not moving
                {
                    hero.SetMovement(NPCMovement.Stand);
                }

                // Do turning
                if (InputHandler.IsPressed(VirtualKeys.Left) || InputHandler.IsPressed(VirtualKeys.Q))
                {
                    hero.BaseInst.gVob.AniCtrl.Turn(-2.5f, true);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Right) || InputHandler.IsPressed(VirtualKeys.E))
                {
                    hero.BaseInst.gVob.AniCtrl.Turn(2.5f, true);
                }
                else
                {
                    hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
                }
            }
            else
            {
                hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
            }

            if (InputHandler.IsPressed(VirtualKeys.K))
            {
                if (nextFwdTeleport < now)
                {
                    Vec3f pos = hero.GetPosition();
                    Vec3f dir = hero.GetDirection();
                    pos += dir * 125.0f;
                    hero.SetPosition(pos);

                    nextFwdTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
                }
            }
            if (InputHandler.IsPressed(VirtualKeys.F8))
            {
                if (nextUpTeleport < now)
                {
                    Vec3f pos = hero.GetPosition();
                    pos.Y += 200.0f;
                    hero.SetPosition(pos);
                    hero.BaseInst.gVob.SetPhysicsEnabled(false);

                    nextUpTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
                }
            }
        }

        static long nextFwdTeleport = 0;
        static long nextUpTeleport = 0;
    }
}
