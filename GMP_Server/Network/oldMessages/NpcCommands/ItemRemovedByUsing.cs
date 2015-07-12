using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.NpcCommands
{
    class ItemRemovedByUsing : IMessage
    {

        public void Read(BitStream stream, Client client)
        {
            int itemID = 0;
            int amount = 0;

            stream.Read(out itemID);
            stream.Read(out amount);


            if (itemID == 0 || !sWorld.VobDict.ContainsKey(itemID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[itemID];
            if (!(vob is Item))
                throw new Exception("Vob is not an item");

            Item item = (Item)vob;

            if (item.Container is NPC)
            {
                NPC proto = (NPC)item.Container;
                proto.removeItem(item, amount);

                Scripting.Objects.Character.NPC.isOnUseItem(proto.ScriptingNPC, item.ScriptingProto, -1, -2);
            }

            Write(item, client.systemAddress);
        }

        public void Write(Item item, AddressOrGUID addGuild)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ItemChangeAmount);
            stream.Write(item.ID);
            stream.Write(item.Amount);

            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, addGuild, true);
        }
    }
}
