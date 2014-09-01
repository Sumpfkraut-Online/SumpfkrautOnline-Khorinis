using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network.Messages.Connection;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using Gothic.zClasses;
using WinApi;
using GUC.Types;
using GUC.WorldObjects;
using GUC.Network.Messages.NpcCommands;
using Gothic.mClasses;
using Gothic.zTypes;
using Gothic.zStruct;
using GUC.Network.Messages.PlayerCommands;

namespace GUC.States
{
    class GameState : AbstractState
    {
        protected long lastPlayerPosUpdate = 0;
        protected long lastNPCPosUpdate = 0;

        protected PlayerKeyMessage pkm;
        public override void Init()
        {
            if (_init)
                return;
            
            Process process = Process.ThisProcess();
            if (oCNpc.Player(process).MagBook.Address == 0)
            {
                oCMag_Book magBook = oCMag_Book.Create(process);
                oCNpc.Player(process).MagBook = magBook;

                magBook.SetOwner(oCNpc.Player(process));
            }

            NPCSpawnMessage.Write();

            StealContainer sc = new StealContainer(Process.ThisProcess());
            sc.Enable();

            pkm = PlayerKeyMessage.getPlayerKeyMessage();


            

            _init = true;
        }

        static zString SoundStr = null;
        static zTSound3DParams SoundParam = null;
        static int soundInt = 0;

        static bool startGS = false;


        static long lastKeyPressed = 0;
        public override void update()
        {
            Process process = Process.ThisProcess();

            long now = DateTime.Now.Ticks;

            if (Program.newWorld)
            {
                sWorld.getWorld(oCGame.Game(process).World.WorldFileName.Value).SpawnWorld();
                Program.newWorld = false;
            }

            //if (lastKeyPressed + 10000*1000*2 < now)
            //{
            //    if (InputHooked.IsPressed(process, (int)VirtualKeys.F7))
            //    {
            //        oCNpc npc = oCNpc.Player(process);
            //        oCMobInter mI = new oCMobInter(process, npc.FocusVob.Address);
            //        mI.StartStateChange(npc, 0, 1);
                    
            //        zERROR.GetZErr(process).Report(2, 'G', "Start-Ani -1", 0, "Program.cs", 0);


            //        lastKeyPressed = now;
            //    }

            //    if (InputHooked.IsPressed(process, (int)VirtualKeys.F8))
            //    {
            //        oCNpc npc = oCNpc.Player(process);
            //        oCMobInter mI = new oCMobInter(process, npc.FocusVob.Address);
            //        mI.StartStateChange(npc, 1, 0);

            //        zERROR.GetZErr(process).Report(2, 'G', "Start-Ani -2", 0, "Program.cs", 0);


            //        lastKeyPressed = now;
            //    }

            //    if (InputHooked.IsPressed(process, (int)VirtualKeys.F9))
            //    {
            //        oCNpc npc = oCNpc.Player(process);
            //        oCMobInter mI = new oCMobInter(process, npc.FocusVob.Address);
            //        mI.GetModel().StartAnimation("T_S0_2_S1");

            //        zERROR.GetZErr(process).Report(2, 'G', "Start-Ani 1", 0, "Program.cs", 0);
                    

            //        lastKeyPressed = now;
            //    }

            //    if (InputHooked.IsPressed(process, (int)VirtualKeys.F10))
            //    {
            //        oCNpc npc = oCNpc.Player(process);
            //        oCMobInter mI = new oCMobInter(process, npc.FocusVob.Address);
            //        mI.GetModel().StartAnimation("S_S1");

            //        zERROR.GetZErr(process).Report(2, 'G', "Start-Ani 2", 0, "Program.cs", 0);
                    
            //        lastKeyPressed = now;
            //    }

            //    if (InputHooked.IsPressed(process, (int)VirtualKeys.F11))
            //    {
            //        oCNpc npc = oCNpc.Player(process);
            //        oCMobInter mI = new oCMobInter(process, npc.FocusVob.Address);
            //        mI.GetModel().StartAnimation("T_S1_2_S0");

            //        zERROR.GetZErr(process).Report(2, 'G', "Start-Ani 3", 0, "Program.cs", 0);
                    
            //        lastKeyPressed = now;
            //    }

            //    if (InputHooked.IsPressed(process, (int)VirtualKeys.F12))
            //    {
            //        oCNpc npc = oCNpc.Player(process);
            //        oCMobInter mI = new oCMobInter(process, npc.FocusVob.Address);
            //        mI.GetModel().StartAnimation("S_S0");

            //        zERROR.GetZErr(process).Report(2, 'G', "Start-Ani 4", 0, "Program.cs", 0);
                    
            //        lastKeyPressed = now;
            //    }
                
            //}



            if (lastPlayerPosUpdate + 10000 * 200 < now )
            {
                SendPlayerPosition(process, Player.Hero);
                NPCUpdateMessage.Write(Player.Hero);

                foreach (NPC iNPC in Player.Hero.NPCControlledList)
                {
                    if (!iNPC.enabled)
                        continue;
                    if (!iNPC.IsSpawned)
                        continue;
                    SendPlayerPosition(process, iNPC);
                    NPCUpdateMessage.Write(iNPC);
                }
                lastPlayerPosUpdate = now;
            }

            //if (lastNPCPosUpdate + 10000 * 500 < now)
            //{
            //    //SendPlayerPosition(process, Player.Hero);
            //    //NPCUpdateMessage.Write(Player.Hero);

            //    foreach (NPC iNPC in Player.Hero.NPCControlledList)
            //    {
            //        SendPlayerPosition(process, iNPC);
            //        NPCUpdateMessage.Write(iNPC);
            //    }
            //    lastNPCPosUpdate = now;
            //}

            pkm.update();

            Program.client.Update();
        }

        protected void SendPlayerPosition(Process process, NPCProto proto)
        {
            if (proto.Address != 0)
            {
                oCNpc npc = new oCNpc(process, proto.Address);
                proto.Position = (Vec3f)npc.TrafoObjToWorld.getPosition();
                proto.Direction = (Vec3f)npc.TrafoObjToWorld.getDirection();
            }
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVobPosDirMessage);
            stream.Write(proto.ID);

            stream.Write(proto.Position);
            stream.Write(proto.Direction);

            Program.client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

    }
}
