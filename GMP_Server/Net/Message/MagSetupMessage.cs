using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class MagSetupMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            byte type = 0;
            int vobtype = 0;
            String name = "";
            float[] pos = new float[16];
            int x= 0,y=0,z=0;
            int plid=0;


            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out vobtype);
            stream.Read(out x);
            stream.Read(out y);
            stream.Read(out z);
            if (vobtype != 8640292)//NPC
            {
                stream.Read(out name);


                for (int i = 0; i < 16; i++)
                    stream.Read(out pos[i]);
            }
            else
                stream.Read(out plid);

            


            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.MagSetupSynch);
            stream.Write(id);
            stream.Write(type);
            stream.Write(vobtype);
            stream.Write(x);
            stream.Write(y);
            stream.Write(z);
            if (vobtype != 8640292)//NPC
            {
                stream.Write(name);

                for (int i = 0; i < 16; i++)
                    stream.Write(pos[i]);
            }
            else
                stream.Write(plid);

            server.server.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);
        }
    }
}
