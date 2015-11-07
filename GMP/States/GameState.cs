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
            { Menus.GUCMenus.Trade.RequestTradeKey, Menus.GUCMenus.Trade.RequestTrade},
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
        static double fvel = 0;
        public static void RenderTest()
        {
            /*if (npc == null)
            {
                NPCInstance inst = NPCInstance.Table.Get(3);
                npc = oCObjectFactory.GetFactory(Program.Process).CreateNPC("OTHERS_NPC");


                npc.Name.Set(inst.name);
                npc.SetVisual(inst.visual);

                npc.SetAdditionalVisuals(inst.bodyMesh, inst.bodyTex, 0, inst.headMesh, inst.headTex, 0, -1);
                using (zVec3 z = zVec3.Create(Program.Process))
                {
                    z.X = inst.bodyWidth;
                    z.Y = inst.bodyHeight;
                    z.Z = inst.bodyWidth;
                    npc.SetModelScale(z);
                }
                npc.SetFatness(inst.fatness);

                npc.Voice = inst.voice;

                npc.HPMax = 100;
                npc.HP = 90;
                npc.InitHumanAI();
                oCGame.Game(Program.Process).World.AddVob(npc);

            }

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


            /*GUI.GUCView.DebugText.Text = "";
            for (int i = 0; i < Player.VobControlledList.Count; i++)
            {
                GUI.GUCView.DebugText.Text += " " + Player.VobControlledList[i].ID;
            }*/

            for (int i = 0; i < World.AllVobs.Count; i++)
            {
                World.AllVobs[i].Update(ticks);
            }

            /*using (zVec3 start = zVec3.Create(Program.Process))
            using (zVec3 dir = zVec3.Create(Program.Process))
            {
                start.X = Player.Hero.Position.X;
                start.Y = Player.Hero.Position.Y;
                start.Z = Player.Hero.Position.Z;

                dir.X = start.X + Player.Hero.Direction.X * 9999f;
                dir.Y = start.Y + Player.Hero.Direction.Y * 9999f;
                dir.Z = start.Z + Player.Hero.Direction.Z * 9999f;

                zCWorld world = oCGame.Game(Program.Process).World;
                world.TraceRayNearestHit(start, dir, 0);

                double dist = (start.X - world.Raytrace_FoundIntersection.X) * (start.X - world.Raytrace_FoundIntersection.X);
                dist += (start.Y - world.Raytrace_FoundIntersection.Y) * (start.Y - world.Raytrace_FoundIntersection.Y);
                dist += (start.Z - world.Raytrace_FoundIntersection.Z) * (start.Z - world.Raytrace_FoundIntersection.Z);
                dist = Math.Sqrt(dist);

                GUI.GUCView.DebugText.Text = dist.ToString();

            }*/
        }
    }
}
