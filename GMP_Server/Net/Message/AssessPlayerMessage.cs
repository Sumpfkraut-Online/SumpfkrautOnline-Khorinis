using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    class AssessPlayerMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int playerID = 0;
            int npcID = 0;
            stream.Read(out playerID);
            stream.Read(out npcID);

            NPC npcN = null;
            
            foreach (NPC npc in Program.npcList)
            {
                if (npc.npcPlayer.id == npcID)
                {
                    npcN = npc;
                    break;
                }
            }

            if (npcN == null || npcN.controller == null)
                return;
            
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AssessPlayerMessage);
            stream.Write(playerID);
            stream.Write(npcID);

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, npcN.controller.guid, true);
        }
    }
}
