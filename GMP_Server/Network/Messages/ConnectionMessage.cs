using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;
using GUC.WorldObjects.Instances;

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

            if (client.CheckValidation(signature, mac))
            {
                WriteInstances(client);
            }
        }

        public static void WriteInstances(Client client)
        {
            if (InstanceCollection.GetCountDynamics() > 0)
            {
                PacketWriter strm = GameServer.SetupStream(NetworkIDs.ConnectionMessage);
                for (int i = 0; i < (int)VobTypes.Maximum; i++)
                {
                    strm.Write((ushort)InstanceCollection.GetCountDynamics(i));
                    foreach (BaseVobInstance inst in InstanceCollection.GetAllDynamics(i))
                    {
                        inst.WriteStream(strm);
                    }
                }
                client.Send(strm, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0');

                using (var fs = new System.IO.FileStream("data", System.IO.FileMode.Create))
                    fs.Write(strm.GetData(), 0, strm.GetLength());
            }
        }
    }
}
