using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.WorldObjects;
using GUC.Network;

namespace GUC.Server.Network.Messages
{
    static class NPCMessage
    {
        public static void ReadAnimation(BitStream stream, Client client)
        {
            short anim = stream.mReadShort();
            client.character.Animation = anim;

            if (client.character.Spawned && client.character.cell != null)
            {
                WriteAnimation(client.character.cell.SurroundingClients(client), client.character);
            }
        }

        public static void WriteAnimation(IEnumerable<Client> list, NPC npc)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCAnimationMessage);
            stream.mWrite(npc.ID);
            stream.mWrite(npc.Animation);

            foreach(Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE, (char)0, client.guid, false);
        }
    }
}
