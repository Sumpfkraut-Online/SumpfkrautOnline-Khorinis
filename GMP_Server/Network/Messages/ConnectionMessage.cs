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
        public static void Read(PacketReader stream, Client client, NPC character, World world)
        {
            String driveString = stream.ReadString();
            String macString = stream.ReadString();

            byte[] instanceTableHash = new byte[16];
            stream.Read(instanceTableHash, 0, 16);

            client.CheckValidity(driveString, macString, instanceTableHash);

            Log.Logger.Print("Read ConnectionMSG");

            WriteInstanceTables(client);
        }

        public static void WriteInstanceTables(Client client)
        {
            Log.Logger.Print("Write ConnectionMSG ???");
            if (client.instanceNeeded)
            {
                Network.Server.ServerInterface.Send(Server.Instances.Data, Server.Instances.Data.Length, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'G', client.guid, false);
                Log.Logger.Print("Written ConnectionMSG");
            }
        }
    }
}
