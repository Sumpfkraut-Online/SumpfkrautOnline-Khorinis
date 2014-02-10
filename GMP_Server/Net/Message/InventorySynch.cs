using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class InventorySynch : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int senderid;
            int invid;
            long time;
            int count;

            stream.Read(out senderid);
            stream.Read(out invid);
            stream.Read(out time);
            stream.Read(out count);

            Player pl = Player.getPlayerSort(invid, Program.playerList);
            if (pl == null)
                return;
            if (pl.lastSendet[(int)Player.SendTime.itemList] > time)
                return;

            pl.lastSendet[(int)Player.SendTime.itemList] = time;

            List<item> items = new List<item>();
            for (int i = 0; i < count; i++)
            {
                item it = new item();
                stream.Read(out it.code);
                stream.Read(out it.Amount);

                items.Add(it);
            }

            pl.itemList = items;



            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.InventorySynch);
            stream.Write(senderid);
            stream.Write(invid);
            stream.Write(time);
            stream.Write(items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                stream.Write(items[i].code);
                stream.Write(items[i].Amount);
            }


            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_SEQUENCED, (char)0, packet.systemAddress, true);
        }
    }
}
