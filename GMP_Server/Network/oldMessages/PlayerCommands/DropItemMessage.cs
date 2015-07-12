using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.PlayerCommands
{
    class DropItemMessage : IMessage
    {
        public void Read(BitStream stream, Client client)
        {
            int plID = 0, itemID = 0;
            stream.Read(out plID);
            stream.Read(out itemID);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found! ID:"+plID);
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPC))
                throw new Exception("Vob is not an NPC!");

            if (itemID == 0 || !sWorld.VobDict.ContainsKey(itemID))
                throw new Exception("Item not found! ID:"+itemID);
            Vob item = sWorld.VobDict[itemID];
            if (!(item is Item))
                throw new Exception("Vob is not an Item!");

            NPC proto = (NPC)vob;
            Item itm = (Item)item;
            proto.DropItem(itm);

            Scripting.Objects.Character.NPC.isOnDropItem(proto.ScriptingNPC, (Scripting.Objects.Item)itm.ScriptingProto, itm.Amount);

            using (RakNetGUID guid = new RakNetGUID(client.guid))
            Write(proto, itm, guid);
        }

        public static void Write(NPC proto, Item itm)
        {
            Write(proto, itm, null);
        }
        public static void Write(NPC proto, Item itm, AddressOrGUID guidExclude)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.DropItemMessage);

            stream.Write(proto.ID);
            stream.Write(itm.ID);

            if (guidExclude == null)
                guidExclude = RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS;
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guidExclude, true);

        }
    }
}
