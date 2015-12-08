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
        public static void Read(PacketReader stream, Client client, NPC character)
        {
            String driveString = stream.ReadString();
            String macString = stream.ReadString();

            byte[] instanceTableHash = new byte[16];
            stream.Read(instanceTableHash, 0, 16);

            client.CheckValidity(driveString, macString, instanceTableHash);
        }

        public static void WriteInstanceTables(Client client)
        {
            if (client.instanceNeeded)
            {
                PacketWriter stream = Program.server.SetupStream(NetworkID.ConnectionMessage);
                stream.Write(InstanceManager.instanceData, 0, InstanceManager.instanceData.Length);
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'G');
            }
        }
    }
}
