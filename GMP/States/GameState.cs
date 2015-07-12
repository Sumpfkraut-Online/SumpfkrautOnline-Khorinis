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

namespace GUC.Client.States
{
    class GameState : AbstractState
    {
        public const int UpdateTimeMS = 120;

        static Dictionary<VirtualKeys, Action> shortcuts = new Dictionary<VirtualKeys, Action>()
        {
            { VirtualKeys.Escape, Menus.GUCMenus.Main.Open },
            { VirtualKeys.Tab, Menus.GUCMenus.Inventory.Open },
             { VirtualKeys.F1, RenderTest }
            /*
            { Ingame.Chat.GetChat().ActivationKey, Ingame.Chat.GetChat().Open },
            { Ingame.Chat.GetChat().ToggleKey, Ingame.Chat.GetChat().ToggleChat },
            { Ingame.Trade.GetTrade().ActivationKey, Ingame.Trade.GetTrade().Activate },
            { Ingame.AnimationMenu.GetMenu().ActivationKey, Ingame.AnimationMenu.GetMenu().Open }*/
        };
        public override Dictionary<VirtualKeys, Action> Shortcuts { get { return shortcuts; } }

        static oCItem item;
        public static void RenderTest()
        {
            if (item == null)
            {
                item = oCItem.Create(Program.Process);
                item.SetVisual("ITFO_APPLE.3DS");
                Player.Hero.gNpc.DoDropVob(item);
                //item.TrafoObjToWorld.setPosition(new float[] {0, 500, 0});
                //oCGame.Game(Program.Process).World.AddVob(item);
            }
            else
            {
                //item.TrafoObjToWorld.setPosition(new float[] { 0, 500, 0 });
                //item.BitField1 &= ~(int)oCItem.BitFlag0.physicsEnabled;
            }
            /*Player.Hero.gNpc.GetModel().StartAni(Player.AniRun, 0);
            zCModelAni ani = Player.Hero.gNpc.GetModel().GetAniFromAniID(Player.AniRun);
            Player.Hero.gNpc.GetModel().GetActiveAni(ani).SetActFrame(1000.0f);*/
            foreach(Vob vob in World.VobDict.Values)
            {
                if (vob is NPC)
                    ((NPC)vob).gNpc.SetBodyState(1);
            }
        }

        public GameState()
        {
            Player.AniTurnLeft = oCNpc.Player(Program.Process).GetModel().GetAniIDFromAniName("T_RUNTURNL");
            Player.AniTurnRight = oCNpc.Player(Program.Process).GetModel().GetAniIDFromAniName("T_RUNTURNR");
            Player.AniRun = oCNpc.Player(Program.Process).GetModel().GetAniIDFromAniName("S_JUMPUP");
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

        const long updateTime = UpdateTimeMS * TimeSpan.TicksPerMillisecond;
        protected long lastPosUpdate = 0;


        Vec3f lastPos;
        Vec3f lastDir;
        public override void Update()
        {
            InputHandler.Update();
            Program.client.Update();

            if (lastPosUpdate < DateTime.Now.Ticks)
            {
                if (Player.Hero.Position.getDistance(lastPos) > 0)
                {
                    VobMessage.WritePosition(Player.Hero);
                    lastPos = Player.Hero.Position;
                }
                if (Player.Hero.Direction.getDistance(lastDir) > 0)
                {
                    VobMessage.WriteDirection(Player.Hero);
                    lastDir = Player.Hero.Direction;
                }
                
                Vob ctrl;
                for (int i = 0; i < Player.VobControlledList.Count; i++)
                {
                    ctrl = null;
                    World.VobDict.TryGetValue(Player.VobControlledList[i], out ctrl);
                    if (ctrl == null) continue;

                    VobMessage.WritePosition(ctrl);
                }
                lastPosUpdate = DateTime.Now.Ticks + updateTime;
            }
            
            foreach (Vob vob in World.VobDict.Values)
            {
                if (vob is NPC)
                {
                    if (((NPC)vob).Animation == Player.AniTurnRight && DateTime.Now.Ticks >= ((NPC)vob).AnimationStartTime + (long)(1.25f*UpdateTimeMS*TimeSpan.TicksPerMillisecond))
                    {
                        ((NPC)vob).gNpc.GetModel().FadeOutAni(Player.AniTurnRight);
                    }
                }
            }
        }
    }
}
