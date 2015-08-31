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
        static Item item;
        static uint num = 0;
        public static void RenderTest()
        {
            if (npc == null)
            {
                npc = new NPC(num++);
                NPCInstance.InstanceList[3].CreateNPC(npc.gNpc);
                npc.Spawn();
            }
            npc.Position = new Vec3f(0, 1000, 0);
        }

        static Random rand = new Random();
        public static void RenderTest2()
        {
            npc.gNpc.AniCtrl.StartFallDownAni();
           /* for (int i = 0; i < 25; i++)
            {
                item = new Item(num++, (ushort)rand.Next(0, 7));
                item.Position = new Vec3f(rand.Next(-700, 700), 1000, rand.Next(-700, 700));
                item.Spawn(item.Position, item.Direction, true);
            }*/
        }

        static int lop = 9;
        static List<string> anis = new List<string>() { "S_NEUTRAL", "S_FRIENDLY", "S_ANGRY", "S_HOSTILE", "S_FRIGHTENED",
            "S_EYESCLOSED", "R_EYESBLINK", "T_EAT", "T_HURT", "VISEME" };
        public static void RenderTest3()
        {
            Player.Hero.gNpc.StopFaceAni(anis[lop]);
            lop++;
            if (lop >= anis.Count)
                lop = 0;

            Player.Hero.gNpc.StartFaceAni(anis[lop], 1, -1);
            GUI.GUCView.DebugText.Text = anis[lop];
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

            GUI.GUCView.DebugText.Text = "";
            foreach (NPC npc in World.npcDict.Values)
            {
                if (npc == Player.Hero)
                    continue;
                GUI.GUCView.DebugText.Text += " " + npc.gNpc.Address.ToString("X4") + " " + npc.gNpc.AniCtrl.Address.ToString("X4") + ": " + npc.gNpc.GetModel().GetAniIDFromAniName("T_FALLEN_2_STAND");
            }

            /*GUI.GUCView.DebugText.Text = "";
            for (int i = 0; i < Player.VobControlledList.Count; i++)
            {
                GUI.GUCView.DebugText.Text += " " + Player.VobControlledList[i].ID;
            }*/

                for (int i = 0; i < World.AllVobs.Count; i++)
                {
                    World.AllVobs[i].Update(ticks);
                }
        }
    }
}
