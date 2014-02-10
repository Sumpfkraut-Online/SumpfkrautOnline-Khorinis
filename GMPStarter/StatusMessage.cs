using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;
using WinApi;
using System.Windows.Forms;

namespace GMP.Net.Messages
{
    public class StatusMessage
    {
        public void Read(BitStream stream, Packet packet, RakPeerInterface client)
        {
            ServerConfig serverconfig = new ServerConfig();


            stream.Read(out serverconfig.ServerName);
            stream.Read(out serverconfig.World);
            stream.Read(out serverconfig.Slots);
            stream.Read(out serverconfig.Port);
            int Count = 0;
            stream.Read(out Count);



            for (int i = 0; i < Count; i++)
            {
                float[] f = new float[3];
                stream.Read(out f[0]); stream.Read(out f[1]); stream.Read(out f[2]);
                serverconfig.Spawn.Add(f);
            }
            serverconfig.Save("server_client.config.tmp");
        }

        public void Write(RakNet.BitStream stream, RakPeerInterface client)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.Status);

            client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
