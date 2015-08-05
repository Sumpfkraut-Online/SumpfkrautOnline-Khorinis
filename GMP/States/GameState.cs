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
        static Dictionary<VirtualKeys, Action> shortcuts = new Dictionary<VirtualKeys, Action>()
        {
            { VirtualKeys.Escape, Menus.GUCMenus.Main.Open },
            { VirtualKeys.Tab, Menus.GUCMenus.Inventory.Open },
            { VirtualKeys.X, Menus.GUCMenus.Animation.Open},
            { VirtualKeys.OEM5, DrawFists }, //^
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

        public static void DrawFists()
        {
            //send
            Player.Hero.DrawFists();
        }


        public static void RenderTest()
        {
            Program.Process.THISCALL<FloatArg>((uint)Player.Hero.gNpc.AniCtrl.Address, (uint)0x6AE540, new CallValue[] { (FloatArg)5.0f, (IntArg)0 });
        }

        public static void RenderTest2()
        {
            Program.Process.THISCALL<FloatArg>((uint)Player.Hero.gNpc.AniCtrl.Address, (uint)0x6AE540, new CallValue[] { (FloatArg)5.0f, (IntArg)1 });
        }

        public static void RenderTest3()
        {
            Program.Process.THISCALL<FloatArg>((uint)Player.Hero.gNpc.AniCtrl.Address, (uint)0x6AE540, new CallValue[] { (FloatArg)(-5.0f), (IntArg)0 });
        }

        public static void WriteTalent(byte num)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCTalentMessage);
            stream.mWrite(num);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void ReadTalent(BitStream stream)
        {
            uint id = stream.mReadUInt();
            byte talent = stream.mReadByte();
            NPC npc;

            World.npcDict.TryGetValue(id, out npc);

            if (npc != null)
            {
                npc.gNpc.SetTalentSkill(1, talent);
                npc.gNpc.SetTalentSkill(2, talent);
            }

        }

        public GameState()
        {
            hEventManager.AddHooks(Program.Process);
            hAniCtrl_Human.AddHooks(Program.Process);

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

            Vec3f dir = Player.Hero.Position.normalise() * -1.0f;
            GUI.GUCView.DebugText.Text = "" + (Player.Hero.Direction.Z*dir.X - dir.Z*Player.Hero.Direction.X);

            for (int i = 0; i < World.AllVobs.Count; i++)
            {
                World.AllVobs[i].Update(ticks);
            }
        }
    }
}
