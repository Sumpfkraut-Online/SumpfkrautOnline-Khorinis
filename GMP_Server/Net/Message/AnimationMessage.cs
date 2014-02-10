using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using Network.Types;

namespace GMP_Server.Net.Message
{
    public class AnimationMessage : Message
    {
        public override void  Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            byte type = 0;
            int aniID = 0;
            int value = 0;
            float value2 = 0;

            

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out aniID);
            if (type == 1 || type == 5 || type == 4)
                stream.Read(out value);
            if(type == 6)
                stream.Read(out value2);

            Player pl = Player.getPlayerSort(id, Program.playerList);
            if (pl == null)
                return;
            if (pl != null)
            {
                pl.lastAniID = aniID;
                pl.lastAniValue = value;
            }

            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AnimationMessage);
            stream.Write(id);
            stream.Write(type);
            stream.Write(aniID);
            if (type == 1  || type == 5 || type == 4)
                stream.Write(value);
            if (type == 6)
                stream.Write(value2);


            foreach (Player toPL in Program.playList)
            {
                if (toPL.guid == packet.guid)
                    continue;
                Vec3f posToPL = new Vec3f(toPL.pos);
                Vec3f posPL = new Vec3f(pl.pos);
                if (posToPL.getDistance(posPL) < Program.updateControlDistance)
                {
                    server.server.Send(stream, RakNet.PacketPriority.IMMEDIATE_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, toPL.guid, false);
                }
            }
            //server.server.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);
        }
    }
}
