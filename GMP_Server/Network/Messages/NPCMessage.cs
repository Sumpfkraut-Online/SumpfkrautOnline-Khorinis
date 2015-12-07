using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.WorldObjects;
using GUC.Network;
using GUC.Types;

namespace GUC.Server.Network.Messages
{
    static class NPCMessage
    {
        #region Animation

        public static void ReadAniStart(BitStream stream, Client client)
        {
            Animations ani = (Animations)stream.mReadUShort();
            WriteAniStart(client.Character.cell.SurroundingClients(client), client.Character, ani);
        }

        public static void ReadAniStop(BitStream stream, Client client)
        {
            Animations ani = (Animations)stream.mReadUShort();
            bool fadeout = stream.ReadBit();
            WriteAniStop(client.Character.cell.SurroundingClients(client), client.Character, ani, fadeout);
        }

        public static void WriteAniStart(IEnumerable<Client> list, NPC npc, Animations ani)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCAniStartMessage);
            stream.Write(npc.ID);
            stream.Write((ushort)ani);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
        }

        public static void WriteAniStop(IEnumerable<Client> list, NPC npc, Animations ani, bool fadeout)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCAniStartMessage);
            stream.Write(npc.ID);
            stream.Write((ushort)ani);
            stream.Write(fadeout);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
        }

        #endregion

        public static void WriteEquipMessage(IEnumerable<Client> list, NPC npc, Item item, byte slot)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCEquipMessage);
            stream.Write(npc.ID);
            stream.Write(item.ID);
            stream.Write(item.Instance.ID);
            stream.Write(item.Condition);
            stream.Write(slot);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteUnequipMessage(IEnumerable<Client> list, NPC npc, byte slot)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCUnequipMessage);
            stream.Write(npc.ID);
            stream.Write(slot);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteJump(IEnumerable<Client> list, NPC npc)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCJumpMessage);
            stream.Write(npc.ID);
            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        #region States

        public static void WriteState(IEnumerable<Client> list, NPC npc)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCStateMessage);
            stream.Write(npc.ID);
            stream.Write((byte)npc.State);
            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteTargetState(IEnumerable<Client> list, NPC npc, NPC target)
        {
            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCTargetStateMessage);
            stream.Write(npc.ID);
            stream.Write((byte)npc.State);

            if (target == null)
            {
                stream.Write(0);
            }
            else
            {
                stream.Write(target.ID);
            }

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteDrawItem(IEnumerable<Client> list, NPC npc, Item item, bool fast)
        {
            if (npc == null) 
                return;

            if (item == null)
                return;

            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCDrawItemMessage);
            stream.Write(npc.ID);
            WriteStrmDrawItem(stream, npc, item);
            stream.Write(fast);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteStrmDrawItem(PacketWriter stream, NPC npc, Item item)
        {
            stream.Write(item.ID);
            if (item != Item.Fists)
            {
                stream.Write(item.Instance.ID);
                if (item.Type == ItemType.Blunt_1H || item.Type == ItemType.Sword_1H)
                    stream.Write(npc.Talent1H);
                else if (item.Type == ItemType.Blunt_2H || item.Type == ItemType.Sword_2H)
                    stream.Write(npc.Talent2H);
                else
                    stream.Write(byte.MinValue);
            }
        }

        public static void WriteUndrawItem(IEnumerable<Client> list, NPC npc, bool fast, bool altRemove)
        {
            if (npc == null)
                return;

            PacketWriter stream = Program.server.SetupStream(NetworkID.NPCUndrawItemMessage);
            stream.Write(npc.ID);
            stream.Write(fast);
            stream.Write(altRemove);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        #endregion

    }
}
