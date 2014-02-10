using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using Injection;
using RakNet;
using GMP.Helper;
using GMP.Modules;

namespace GMP.Network.Messages
{
    public class ItemStealMessage : Message
    {
        public void Write(RakNet.BitStream stream, Client client, Player pl, String item, int amount)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.ItemStealMessage);
            stream.Write(Program.Player.id);
            stream.Write(pl.id);
            stream.Write(item);
            stream.Write(amount);

            if (pl.isNPC)
                return;//TODO: AUCH NPC's erlauben ;)

            client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.ItemStealMessage))
                StaticVars.sStats[(int)NetWorkIDS.ItemStealMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.ItemStealMessage] += 1;
        }

        public override void Read(BitStream stream, Packet packet, Client client)
        {
            int thiefid = 0;
            int plid = 0;
            String item = "";
            int amount = 0;

            stream.Read(out thiefid);
            stream.Read(out plid);
            stream.Read(out item);
            stream.Read(out amount);

            Player thief = StaticVars.AllPlayerDict[thiefid];
            Player pl = StaticVars.AllPlayerDict[plid];

            if (thief == null || pl == null || thief.isSpawned == false || pl.isSpawned == false)
                return;

            pl.RemoveItem(new item() { code = item, Amount = amount });
            InventoryHelper.RemoveItem(pl, item, amount);

            thief.InsertItem(new item() { code = item, Amount = amount });
            InventoryHelper.InsertItem(thief, item, amount);
        }
    }
}
