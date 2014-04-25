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
using WinApi.User.Enumeration;
using Gothic.zTypes;
using Gothic.zStruct;

namespace GUC.States
{
    class GameState : AbstractState
    {
        protected long lastPlayerPosUpdate = 0;
        
        public override void Init()
        {
            if (_init)
                return;

            NPCSpawnMessage.Write();

            StealContainer sc = new StealContainer(Process.ThisProcess());
            sc.Enable();

            _init = true;
        }


        public override void update()
        {
            Process process = Process.ThisProcess();

            long now = DateTime.Now.Ticks;

            if (Program.newWorld)
            {
                sWorld.getWorld(oCGame.Game(process).World.WorldFileName.Value).SpawnWorld();
                Program.newWorld = false;
            }



            if (lastPlayerPosUpdate + 10000 * 200 < now )
            {
                SendPlayerPosition(process, Player.Hero);
                NPCUpdateMessage.Write(Player.Hero);

                foreach (NPC iNPC in Player.Hero.NPCControlledList)
                {
                    SendPlayerPosition(process, iNPC);
                    NPCUpdateMessage.Write(iNPC);
                }
                lastPlayerPosUpdate = now;
            }

            Program.client.Update();
        }

        protected void SendPlayerPosition(Process process, NPCProto proto)
        {
            if (proto.Address == 0)
                return;
            oCNpc npc = new oCNpc(process, proto.Address);
            proto.Position = (Vec3f)npc.TrafoObjToWorld.getPosition();
            proto.Direction = (Vec3f)npc.TrafoObjToWorld.getDirection();

            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.SetVobPosDirMessage);
            stream.Write(proto.ID);

            stream.Write(proto.Position);
            stream.Write(proto.Direction);

            Program.client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

    }
}
