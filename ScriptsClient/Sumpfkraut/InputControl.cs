using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Client.Scripts.Sumpfkraut.Menus;
using GUC.Enumeration;

namespace GUC.Client.Scripts.Sumpfkraut
{
    static class InputControl
    {
        static bool inited = false;
        public static void Init()
        {
            if (inited) {
                return;
            }
            inited = true;

            InputHandler.OnKeyDown += KeyDown;
            InputHandler.OnKeyUp += KeyUp;
        }

        static int fwdKeys = 0;
        static int strafeKeys = 0;
        static int turnKeys = 0;

        static void Fwd(bool inc)
        {
            fwdKeys += inc ? 1 : -1;

            if (fwdKeys > 0)
                GUC.Network.GameClient.Client.SetHeroState(NPCStates.MoveForward);
            else if (fwdKeys < 0)
                GUC.Network.GameClient.Client.SetHeroState(NPCStates.MoveBackward);
            else if (strafeKeys == 0)
                GUC.Network.GameClient.Client.SetHeroState(NPCStates.Stand);
        }

        static void Strafe(bool inc)
        {
            strafeKeys += inc ? 1 : -1;

            if (strafeKeys > 0)
                GUC.Network.GameClient.Client.SetHeroState(NPCStates.MoveRight);
            else if (strafeKeys < 0)
                GUC.Network.GameClient.Client.SetHeroState(NPCStates.MoveLeft);
            else if (fwdKeys == 0)
                GUC.Network.GameClient.Client.SetHeroState(NPCStates.Stand);
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

            if (key == VirtualKeys.O)
            {
                var stream = GUC.Network.GameClient.Client.GetIngameMsgStream();
                GUC.Network.GameClient.Client.SendIngameMsg(stream);
            }
            else if (key == VirtualKeys.F1)
            {
                GUC.Network.GameClient.Client.Character.gVob.GetModel().StartAni("S_FALL", 0);
                GUC.Network.GameClient.Client.Character.gVob.SetPhysicsEnabled(true);
            }
            else if (key == VirtualKeys.Up)
            {
                Fwd(true);
            }
            else if (key == VirtualKeys.Down)
            {
                Fwd(false);
            }
            else if (key == VirtualKeys.D)
            {
                Strafe(true);
            }
            else if (key == VirtualKeys.A)
            {
                Strafe(false);
            }
            else if (key == VirtualKeys.Right)
            {
                turnKeys++;
            }
            else if (key == VirtualKeys.Left)
            {
                turnKeys--;
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

            if (key == VirtualKeys.Up)
            {
                Fwd(false);
            }
            else if (key == VirtualKeys.Down)
            {
                Fwd(true);
            }
            else if (key == VirtualKeys.D)
            {
                Strafe(false);
            }
            else if (key == VirtualKeys.A)
            {
                Strafe(true);
            }
            else if (key == VirtualKeys.Right)
            {
                turnKeys--;
            }
            else if (key == VirtualKeys.Left)
            {
                turnKeys++;
            }
        }
        
        public static void Update(long now)
        {
            if (GUC.Network.GameClient.Client.Character == null)
                return;
            
            if (turnKeys > 0)
            {
                GUC.Network.GameClient.Client.Character.gVob.AniCtrl.Turn(3.0f, true);
            }
            else if (turnKeys < 0)
            {
                GUC.Network.GameClient.Client.Character.gVob.AniCtrl.Turn(-3.0f, true);
            }
            else
            {
                GUC.Network.GameClient.Client.Character.gVob.AniCtrl.StopTurnAnis();
            }
        }
    }
}
