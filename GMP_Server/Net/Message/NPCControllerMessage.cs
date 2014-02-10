using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class NPCControllerMessage : Message
    {
        public void Write(RakNet.BitStream stream, Server server,NPC npc, bool enabled)
        {
            if (npc == null || npc.controller == null || npc.npcPlayer == null)
                return;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.NPCControllerMessage);
            stream.Write(npc.npcPlayer.id);
            stream.Write(enabled);

            if (enabled)
            {
                Log.Logger.log(Log.Logger.LOG_INFO, npc.controller.guid + " " + npc.controller.systemAddress + " Player: " +npc.controller.id+" " + npc.controller.name + " Get Control of NPC: " + npc.npcPlayer.instance + " in Map: " + npc.npcPlayer.actualMap + " With Ping: " + server.server.GetAveragePing(npc.controller.guid));
            }
            

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, npc.controller.guid, false);
        }

        public override void Read(BitStream stream, Packet packet, Server server)
        {
            int npcid;
            bool enabled;
            stream.Read(out npcid);
            stream.Read(out enabled);

            foreach (NPC npc in Program.npcList)
            {
                if (npc.npcPlayer.id == npcid)
                {
                    npc.controller = null;
                    break;
                }
            }
        }
    }
}
