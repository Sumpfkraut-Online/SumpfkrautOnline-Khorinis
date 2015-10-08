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
            { Menus.GUCMenus.Status.Hotkey, Menus.GUCMenus.Status.Open },
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

        static oCNpc npc;
        public static void RenderTest()
        {
            oCMob mob = oCMob.Create(Program.Process);
            mob.VobType = zCObject.VobTypes.Mob;
            mob.Name.Set("FONT_Screen");
            mob.SetName("FONT_Screen");

            mob.SetVisual("ITFO_APPLE.3DS");

            Vec3f newPos = Player.Hero.Position;
            newPos.X += 20;
            mob.TrafoObjToWorld.setPosition(newPos.Data);
            mob.SetPositionWorld(newPos.Data);
            mob.TrafoObjToWorld.setPosition(newPos.Data);

            mob.BitField1 |= (int)zCVob.BitFlag0.collDetectionDynamic;
            mob.BitField1 |= (int)zCVob.BitFlag0.collDetectionStatic;
            mob.BitField1 |= (int)zCVob.BitFlag0.staticVob;

            oCGame.Game(Program.Process).World.AddVob(mob);

            //Player.AddItem(ItemInstance.InstanceList[0], 10);
            //Player.AddItem(ItemInstance.InstanceList[1], 10);
            /*if (npc == null)
            {
                npc = NPCInstance.InstanceList[0].CreateNPC();
                npc.Name.Set("Testcharakter");
                npc.SetAdditionalVisuals(HumBodyMesh.HUM_BODY_NAKED0.ToString(), (int)HumBodyTex.G1Hero, 0, HumHeadMesh.HUM_HEAD_PONY.ToString(), (int)HumHeadTex.Face_N_Player, 0, -1);
                npc.InitHumanAI();
                oCGame.Game(Program.Process).World.AddVob(npc);
                npc.HPMax = 100;
            }
            npc.HP = 100;

            Vec3f newPos = Player.Hero.Position;
            newPos.X += 20;
            npc.TrafoObjToWorld.setPosition(newPos.Data);
            npc.SetPositionWorld(newPos.Data);
            npc.TrafoObjToWorld.setPosition(newPos.Data);*/
        }

        static Random rand = new Random();
        static oCMsgMovement msg = null;
        public static void RenderTest2()
        {
            if (npc != null)
            {
                npc.GetEM(0).KillMessages();
                msg = oCMsgMovement.Create(Program.Process, oCMsgMovement.SubTypes.RobustTrace, Player.Hero.gNpc);
                npc.GetEM(0).OnMessage(msg, npc);
            }
            //npc.gNpc.AniCtrl.StartFallDownAni();
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

        public static zString str = zString.Create(Program.Process, "TESTMOBSI");
        public static Int32 GetNameHook(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                zERROR.GetZErr(Program.Process).Report(2, 'G', str.Address.ToString("X4"), 0, "Program.cs", 0);
                Program.Process.Write(str.Address, address + 8);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', "Exception: " + ex.Message + " " + ex.StackTrace + " " + ex.Source, 0, "Program.cs", 0);
            }
            return 0;
        }

        public GameState()
        {
            hEventManager.AddHooks(Program.Process);
            hAniCtrl_Human.AddHooks(Program.Process);
            hNpc.AddHooks(Program.Process);

            //Program.Process.Replace("UntoldChapter\\DLL\\GUC.dll", typeof(GameState).GetMethod("GetNameHook"), 0x71BC30, 5, 1);
        }

        public override void Update()
        {
            long ticks = DateTime.Now.Ticks;
            InputHandler.Update();
            Program.client.Update();

            if (npc != null)
                if ((new Vec3f(npc.TrafoObjToWorld.getPosition())).getDistance(Player.Hero.Position) < 150)
                {
                    npc.GetEM(0).KillMessages();
                    npc.AniCtrl._Stand();
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
