using System; 
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class ConnectionRequest : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            String a, b;
            String name = "";
            String world = "";
            stream.Read(out a);
            stream.Read(out b);
            stream.Read(out name);
            stream.Read(out world);

            Program.idCount += 1;
            int id = Program.idCount;
            Player pl = null;
            String oldname = name;
            int nameindex = 0;
            while (Player.getPlayerByName(name, Program.playList) != null)
            {
                name = oldname + nameindex;
                nameindex++;
            }

            pl = new Player(packet.guid, packet.systemAddress);
            
            pl.id = id;
            pl.name = name;
            pl.actualMap = Player.getMap(world);
            pl.guidStr = packet.guid.ToString();
            pl.a = a;
            pl.b = b;

            Log.Logger.log(Log.Logger.LOG_INFO, packet.guid + " " + packet.systemAddress + " Connection Request from: " + pl.name+" map: "+pl.actualMap);


            SetDefaultData(pl);
            
            Program.playerDict.Add(pl.id, pl);
            Program.playerList.Add(pl);
            Program.playerList.Sort(new Network.Player.PlayerComparer());
            Program.playList.Add(pl);
            

            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.ConnectionRequest);
            stream.Write(id);
            stream.Write(name);
            stream.Write(pl.guidStr);
            stream.Write(pl.actualMap);



            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, pl.guid, false);
        }

        public static void SetDefaultData(Player pl)
        {
            //pl.pos = Program.config.GetRandomSpawn();
            //Vec3 vec = (Vec3)Program.config.GetRandomSpawn();
            //pl.pos = new float[] { vec.x, vec.y, vec.z };
        }
    }
}
