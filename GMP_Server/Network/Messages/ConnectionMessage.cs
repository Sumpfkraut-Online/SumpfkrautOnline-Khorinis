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

            client.CheckValidity(driveString, macString);
        }

        public static void Write(Client client)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.ConnectionMessage);
            stream.mWrite(ItemInstance.InstanceList.Count);
            for (int i = 0; i < ItemInstance.InstanceList.Count; i++ )
            {
                ItemInstance.InstanceList[i].Write(stream);
            }
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, client.guid, false);
        }
    }
}
