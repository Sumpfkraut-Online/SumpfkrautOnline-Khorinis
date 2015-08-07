using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using Gothic.zClasses;
using WinApi;
using GUC.Types;
using WinApi.User.Enumeration;
using GUC.Client.WorldObjects;
using GUC.Client.Network.Messages;
using Gothic.zStruct;
using Gothic.zTypes;
using GUC.Client.Hooks;

namespace GUC.Client.States
{
    class GameState : AbstractState
    {
        Dictionary<VirtualKeys, Action> shortcuts = new Dictionary<VirtualKeys, Action>()
        {
            { VirtualKeys.Escape, Menus.GUCMenus.Main.Open },
            { VirtualKeys.Tab, Menus.GUCMenus.Inventory.Open },
            { Menus.GUCMenus.Animation.Hotkey, Menus.GUCMenus.Animation.Open},
            { VirtualKeys.OEM5, Fists }, //^
             { VirtualKeys.F1, RenderTest },
             { VirtualKeys.F2, RenderTest2 },
              { VirtualKeys.F3, RenderTest3 }

            /*
            { Ingame.Chat.GetChat().ActivationKey, Ingame.Chat.GetChat().Open },
            { Ingame.Chat.GetChat().ToggleKey, Ingame.Chat.GetChat().ToggleChat },
            { Ingame.Trade.GetTrade().ActivationKey, Ingame.Trade.GetTrade().Activate },
            { Ingame.AnimationMenu.GetMenu().ActivationKey, Ingame.AnimationMenu.GetMenu().Open }*/
        };
        public override Dictionary<VirtualKeys, Action> Shortcuts { get { return shortcuts; } }

        public static void Fists()
        {
            //send
            if (Player.Hero.gNpc.WeaponMode == 0)
            {
                Player.Hero.DrawFists();
            }
            else if (Player.Hero.gNpc.WeaponMode== 1)
            {
                oCMsgWeapon msg = oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.RemoveWeapon1, 0, 0);
                Player.Hero.gVob.GetEM(0).OnMessage(msg, Player.Hero.gVob);
            }
        }


        public static void RenderTest()
        {
        }

        public static void RenderTest2()
        {
        }

        public static void RenderTest3()
        {
        }

        public GameState()
        {
            hEventManager.AddHooks(Program.Process);
            hAniCtrl_Human.AddHooks(Program.Process);
            Program.Process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("hook_GetNextWeaponMode"), 0x739A30, 6, 4);

            Player.AniTurnLeft = oCNpc.Player(Program.Process).GetModel().GetAniIDFromAniName("T_RUNTURNL");
            Player.AniTurnRight = oCNpc.Player(Program.Process).GetModel().GetAniIDFromAniName("T_RUNTURNR");
            Player.AniStrafeLeft = oCNpc.Player(Program.Process).GetModel().GetAniIDFromAniName("S_1HATTACK");
            Player.AniRun = oCNpc.Player(Program.Process).GetModel().GetAniIDFromAniName("S_RUNL");

            

            /*if (oCNpc.Player(process).MagBook.Address == 0)
            {
                oCMag_Book magBook = oCMag_Book.Create(process);
                oCNpc.Player(process).MagBook = magBook;

                magBook.SetOwner(oCNpc.Player(process));
            }*/

            /*StealContainer sc = new StealContainer(Program.Process);
            sc.Enable();*/

            //Sumpfkraut.Ingame.IngameInterface.Init();
        }



        public override void Update()
        {
            long ticks = DateTime.Now.Ticks;
            InputHandler.Update();
            Program.client.Update();

            for (int i = 0; i < World.AllVobs.Count; i++)
            {
                World.AllVobs[i].Update(ticks);
            }
        }
    }
}
