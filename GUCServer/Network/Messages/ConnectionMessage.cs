using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Models;
using GUC.WorldObjects.Instances;
using GUC.Scripting;

namespace GUC.Network.Messages
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
                Write(client);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Write(GameClient client)
        {
            if (BaseVobInstance.GetCountDynamics() > 0 && Model.GetCountDynamics() > 0)
            {
                PacketWriter strm = GameServer.SetupStream(NetworkIDs.ConnectionMessage);
                
                // MODELS
                if (Model.GetCountDynamics() > 0)
                {
                    strm.Write(true);
                    strm.Write((ushort)Model.GetCountDynamics());
                    Model.ForEachDynamic(model =>
                    {
                        model.WriteStream(strm);
                    });
                }
                else
                {
                    strm.Write(false);
                }

                // INSTANCES
                if (BaseVobInstance.GetCountDynamics() > 0)
                {
                    strm.Write(true);
                    for (int i = 0; i < (int)VobTypes.Maximum; i++)
                    {
                        strm.Write((ushort)BaseVobInstance.GetCountDynamicsOfType((VobTypes)i));
                        BaseVobInstance.ForEachDynamicOfType((VobTypes)i, inst =>
                        {
                            inst.WriteStream(strm);
                        });
                    }
                }
                else
                {
                    strm.Write(false);
                }

                client.Send(strm, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, '\0');
            }
        }
    }
}
