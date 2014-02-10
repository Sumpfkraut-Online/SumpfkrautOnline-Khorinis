using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class ItemSynchro_DoDrop : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            String itemcode = "";
            byte type = 0;
            int amount = 0;

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out itemcode);
            stream.Read(out amount);
            stream.Reset();

            Player pl = Player.getPlayer(id, Program.playerList);
            item itm = new item() { code = itemcode, Amount = amount, inInv = false };
            Network.Savings.WorldItems wi = new Network.Savings.WorldItems() { itm = itm, pos = pl.pos, world = pl.actualMap };
            //Item-Listen aktualisieren:
            if (type == 1)//Rausschmeißen
            {
                pl.RemoveItem(itm);//Aus dem Inventar löschen...
                Program.World.InsertItem(wi);//In die Map inserten...
                Console.WriteLine(pl.name + " Inserted Item: " + itm.code + " in World: " + pl.actualMap);

                Program.scriptManager.OnDropItem(new Scripting.Player(pl), itm.code, itm.Amount);
            }
            else if (type == 2) // Aufheben!
            {
                pl.InsertItem(itm);
                Program.World.RemoveItem(wi);//Rausnehmen...
                Console.WriteLine(pl.name+" removed Item: "+itm.code+" in World: "+pl.actualMap);

                Program.scriptManager.OnTakeItem(new Scripting.Player(pl), itm.code, itm.Amount);
            }

            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.ItemSynchro_DoDrop);
            stream.Write(id);
            stream.Write(type);
            stream.Write(itemcode);
            stream.Write(amount);

            server.server.Send(stream, PacketPriority.MEDIUM_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);
        }
    }
}
