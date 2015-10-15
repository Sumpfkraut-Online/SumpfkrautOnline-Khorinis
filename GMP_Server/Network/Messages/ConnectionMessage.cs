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

            byte[] itemTableHash = new byte[16];
            stream.Read(itemTableHash, 16);

            client.CheckValidity(driveString, macString, npcTableHash, itemTableHash);
        }

        public static void WriteInstanceTables(Client client)
        {
            if (client.instanceNPCNeeded || client.instanceItemNeeded)
            {
                BitStream stream = Program.server.SetupStream(NetworkID.ConnectionMessage);

                if (client.instanceNPCNeeded)
                {
                    stream.Write1();
                    stream.mWrite(NPCInstance.Table.data.Length);
                    stream.Write(NPCInstance.Table.data, (uint)NPCInstance.Table.data.Length);
                }
                else
                {
                    stream.Write0();
                }

                if (client.instanceItemNeeded)
                {
                    stream.Write1();
                    stream.mWrite(ItemInstance.Table.data.Length);
                    stream.Write(ItemInstance.Table.data, (uint)ItemInstance.Table.data.Length);
                }
                else
                {
                    stream.Write0();
                }

                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'G', client.guid, false);
            }
        }
    }
}
