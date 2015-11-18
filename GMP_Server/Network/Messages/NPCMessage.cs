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
            WriteAniStart(client.character.cell.SurroundingClients(client), client.character, ani);
        }

        public static void ReadAniStop(BitStream stream, Client client)
        {
            Animations ani = (Animations)stream.mReadUShort();
            bool fadeout = stream.ReadBit();
            WriteAniStop(client.character.cell.SurroundingClients(client), client.character, ani, fadeout);
        }

        public static void WriteAniStart(IEnumerable<Client> list, NPC npc, Animations ani)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCAniStartMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((ushort)ani);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W', client.guid, false);
        }

        public static void WriteAniStop(IEnumerable<Client> list, NPC npc, Animations ani, bool fadeout)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCAniStartMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((ushort)ani);
            stream.mWrite(fadeout);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W', client.guid, false);
        }

        #endregion

        public static void WriteEquipMessage(IEnumerable<Client> list, NPC npc, Item item, byte slot)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCEquipMessage);
            stream.mWrite(npc.ID);
            stream.mWrite(item.ID);
            stream.mWrite(item.Instance.ID);
            stream.mWrite(item.Condition);
            stream.mWrite(slot);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        public static void WriteUnequipMessage(IEnumerable<Client> list, NPC npc, byte slot)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCUnequipMessage);
            stream.mWrite(npc.ID);
            stream.mWrite(slot);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        public static void WriteJump(IEnumerable<Client> list, NPC npc)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCJumpMessage);
            stream.mWrite(npc.ID);
            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        #region States

        public static void WriteState(IEnumerable<Client> list, NPC npc)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCStateMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((byte)npc.State);
            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        public static void WriteTargetState(IEnumerable<Client> list, NPC npc, NPC target)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCTargetStateMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((byte)npc.State);

            if (target == null)
            {
                stream.mWrite(0);
            }
            else
            {
                stream.mWrite(target.ID);
            }

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        public static void WriteDrawItem(IEnumerable<Client> list, NPC npc, Item item, bool fast)
        {
            if (npc == null) 
                return;

            if (item == null)
                return;

            BitStream stream = Program.server.SetupStream(NetworkID.NPCDrawItemMessage);
            stream.mWrite(npc.ID);
            WriteStrmDrawItem(stream, npc, item);
            stream.mWrite(fast);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        public static void WriteStrmDrawItem(BitStream stream, NPC npc, Item item)
        {
            stream.mWrite(item.ID);
            if (item != Item.Fists)
            {
                stream.mWrite(item.Instance.ID);
                if (item.Type == ItemType.Blunt_1H || item.Type == ItemType.Sword_1H)
                    stream.mWrite(npc.Talent1H);
                else if (item.Type == ItemType.Blunt_2H || item.Type == ItemType.Sword_2H)
                    stream.mWrite(npc.Talent2H);
                else
                    stream.mWrite(byte.MinValue);
            }
        }

        public static void WriteUndrawItem(IEnumerable<Client> list, NPC npc, bool fast, bool altRemove)
        {
            if (npc == null)
                return;

            BitStream stream = Program.server.SetupStream(NetworkID.NPCUndrawItemMessage);
            stream.mWrite(npc.ID);
            stream.mWrite(fast);
            stream.mWrite(altRemove);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        #endregion
    }
}
