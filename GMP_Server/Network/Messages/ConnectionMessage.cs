using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;
using GUC.Network;

namespace GUC.Server.Network.Messages
{
    static class ConnectionMessage
    {
        public static void Read(PacketReader stream, Client client)
        {
            byte[] signature = new byte[16];
            stream.Read(signature, 0, 16);

            byte[] mac = new byte[16];
            stream.Read(mac, 0, 16);

            //client.CheckValidity(driveString, macString, instanceTableHash);
        }

        public static void WriteInstanceTables(Client client)
        {
            Log.Logger.Print("Write ConnectionMSG ???");
            if (client.instanceNeeded)
            {
                Network.GameServer.ServerInterface.Send(VobInstance.AllInstances.Data, VobInstance.AllInstances.Data.Length, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'G', client.guid, false);
                Log.Logger.Print("Written ConnectionMSG");
            }
        }
    }
}
