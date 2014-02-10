using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class ItemStealMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int thiefid = 0;
            int plid = 0;
            String item = "";
            int amount = 0;

            stream.Read(out thiefid);
            stream.Read(out plid);
            stream.Read(out item);
            stream.Read(out amount);

            Player thief = Player.getPlayerSort(thiefid, Program.playerList);
            Player pl = Player.getPlayerSort(plid, Program.playerList);

            if (thief == null || pl == null)
                return;

            pl.RemoveItem(new item() { code = item, Amount = amount });
            thief.InsertItem(new item() { code = item, Amount = amount });

            Console.WriteLine("ItemStealMessage Player: " + pl.name + " thief: " + thief.name + " item:" + item + " amount:" + amount);

            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.ItemStealMessage);
            stream.Write(thiefid);
            stream.Write(pl.id);
            stream.Write(item);
            stream.Write(amount);

            server.server.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, true);
        }
    }
}
