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
    static class ConnectionMessage
    {
        public static void Read(BitStream stream, Client client)
        {
            String driveString = stream.mReadString();
            String macString = stream.mReadString();
            byte[] npcTableHash = new byte[16];
            stream.Read(npcTableHash, 16);

            client.CheckValidity(driveString, macString, npcTableHash);
        }

        public static void WriteInstanceTables(Client client)
        {
            if (client.instanceNPCNeeded || client.instanceItemNeeded)
            {
                BitStream stream = Program.server.SetupStream(NetworkID.ConnectionMessage);
                stream.mWrite(client.instanceNPCNeeded);
                if (client.instanceNPCNeeded)
                {
                    stream.Write(NPCInstance.data, (uint)NPCInstance.data.Length);
                }
                stream.mWrite(client.instanceItemNeeded);
                if (client.instanceItemNeeded)
                {
                    //stream.Write((ushort)NPCInstance.data.Length);
                    //stream.Write(NPCInstance.data, (uint)NPCInstance.data.Length);
                }
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'G', client.guid, false);
            }
        }
    }
}
