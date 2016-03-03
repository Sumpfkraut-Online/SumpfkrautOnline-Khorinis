using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;
using GUC.WorldObjects.Instances;
using GUC.Scripting;

namespace GUC.Server.Network.Messages
{
    static class ConnectionMessage
    {
        public static bool Read(PacketReader stream, RakNetGUID guid, SystemAddress ip, out GameClient client)
        {
            byte[] signature = new byte[16];
            stream.Read(signature, 0, 16);

            byte[] mac = new byte[16];
            stream.Read(mac, 0, 16);

            client = new GameClient(guid, ip, mac, signature);
            if (ScriptManager.Interface.OnClientConnection(client))
            {
                WriteInstances(client);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void WriteInstances(GameClient client)
        {
            if (InstanceCollection.GetCountDynamics() > 0)
            {
                PacketWriter strm = GameServer.SetupStream(NetworkIDs.ConnectionMessage);
                for (int i = 0; i < (int)VobTypes.Maximum; i++)
                {
                    strm.Write((ushort)InstanceCollection.GetCountDynamics(i));
                    InstanceCollection.ForEachDynamic(i, inst =>
                    {
                        inst.WriteStream(strm);
                    });
                }
                client.Send(strm, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0');

                using (var fs = new System.IO.FileStream("instances", System.IO.FileMode.Create))
                    fs.Write(strm.GetData(), 0, strm.GetLength());
            }
        }
    }
}
