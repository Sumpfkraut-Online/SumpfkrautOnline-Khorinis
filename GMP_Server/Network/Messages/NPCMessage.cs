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

        public static void ReadAniStart(PacketReader stream, Client client, NPC character)
        {
            Animations ani = (Animations)stream.ReadUShort();
            WriteAniStart(character.cell.SurroundingClients(client), character, ani);
        }

        public static void ReadAniStop(PacketReader stream, Client client, NPC character)
        {
            Animations ani = (Animations)stream.ReadUShort();
            bool fadeout = stream.ReadBit();
            WriteAniStop(character.cell.SurroundingClients(client), character, ani, fadeout);
        }

        public static void WriteAniStart(IEnumerable<Client> list, NPC npc, Animations ani)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCAniStartMessage);
            stream.Write(npc.ID);
            stream.Write((ushort)ani);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
        }

        public static void WriteAniStop(IEnumerable<Client> list, NPC npc, Animations ani, bool fadeout)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCAniStartMessage);
            stream.Write(npc.ID);
            stream.Write((ushort)ani);
            stream.Write(fadeout);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
        }

        #endregion

        public static void WriteEquipMessage(IEnumerable<Client> list, NPC npc, Item item, byte slot)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCEquipMessage);

            stream.Write(npc.ID);
            stream.Write(slot);
            item.WriteEquipped(stream);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteUnequipMessage(IEnumerable<Client> list, NPC npc, byte slot)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCUnequipMessage);
            stream.Write(npc.ID);
            stream.Write(slot);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteJump(IEnumerable<Client> list, NPC npc)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCJumpMessage);
            stream.Write(npc.ID);
            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        #region States

        public static void WriteState(IEnumerable<Client> list, NPC npc)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCStateMessage);
            stream.Write(npc.ID);
            stream.Write((byte)npc.State);
            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteTargetState(IEnumerable<Client> list, NPC npc, NPC target)
        {
            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCTargetStateMessage);
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
            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCDrawItemMessage);
            stream.Write(npc.ID);
            stream.Write(fast);
            item.WriteEquipped(stream);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteUndrawItem(IEnumerable<Client> list, NPC npc, bool fast, bool altRemove)
        {
            if (npc == null)
                return;

            PacketWriter stream = Network.Server.SetupStream(NetworkIDs.NPCUndrawItemMessage);
            stream.Write(npc.ID);
            stream.Write(fast);
            stream.Write(altRemove);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        #endregion

    }
}
