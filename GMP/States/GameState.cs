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
            { VirtualKeys.OEM5, Player.DoFists }, //^
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

        static NPC npc;
        public static void RenderTest()
        {
            if (npc == null)
            {
                npc = new NPC(9999);
                NPCInstance.InstanceList[3].CreateNPC(npc.gNpc);
                npc.gNpc.SetToFistMode();
                npc.Spawn();
            }
            npc.Position = new Vec3f(0, 1000, 0);
        }

        public static void RenderTest2()
        {
            npc.State = NPCState.MoveRight;
        }

        public static void RenderTest3()
        {
        }

        public GameState()
        {
            hEventManager.AddHooks(Program.Process);
            hAniCtrl_Human.AddHooks(Program.Process);
            hNpc.AddHooks(Program.Process);
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
