using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Network;
using GUC.Types;

namespace GUC.Server.Network.Messages
{
    static class NPCMessage
    {
        #region States

        public static void ReadState(PacketReader stream, GameClient client, NPC character, World world)
        {
            int id = stream.ReadUShort();
            NPC npc;
            if (world.TryGetVob(id, out npc))
            {
                NPCStates state = (NPCStates)stream.ReadByte();
                if (npc == character /*|| (client.VobControlledList.Contains(npc) && state <= NPCStates.MoveBackward)*/) //is it a controlled NPC?
                {
                    if (npc.ScriptObject != null)
                        npc.ScriptObject.OnCmdMove(state);
                }
            }
        }

        public static void WriteState(NPC npc)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCStateMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((byte)npc.State);

            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        #endregion

        #region Jumping

        public static void ReadJump(PacketReader stream, GameClient client, NPC character, World world)
        {
            int id = stream.ReadUShort();
            NPC npc;
            if (world.TryGetVob(id, out npc))
            {
                if (npc == character /*|| (client.VobControlledList.Contains(npc) && state <= NPCStates.MoveBackward)*/) //is it a controlled NPC?
                {
                    if (npc.ScriptObject != null)
                        npc.ScriptObject.OnCmdJump();
                }
            }
        }

        public static void WriteJump(NPC npc)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCJumpMessage);
            stream.Write((ushort)npc.ID);

            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        #endregion

        public static void WriteDrawItem(IEnumerable<GameClient> list, NPC npc, Item item, bool fast)
        {
            PacketWriter stream = Network.GameServer.SetupStream(NetworkIDs.NPCDrawItemMessage);
            stream.Write(npc.ID);
            stream.Write(fast);
            //item.WriteEquipped(stream);

            foreach (GameClient client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }

        public static void WriteUndrawItem(IEnumerable<GameClient> list, NPC npc, bool fast, bool altRemove)
        {
            if (npc == null)
                return;

            /*PacketWriter stream = Network.GameServer.SetupStream(NetworkIDs.NPCUndrawItemMessage);
            stream.Write(npc.ID);
            stream.Write(fast);
            stream.Write(altRemove);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');*/
        }

        #region Animation

        /*public static void ReadAniStart(PacketReader stream, Client client, NPC character)
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
            PacketWriter stream = Network.GameServer.SetupStream(NetworkIDs.NPCAniStartMessage);
            stream.Write(npc.ID);
            stream.Write((ushort)ani);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
        }

        public static void WriteAniStop(IEnumerable<Client> list, NPC npc, Animations ani, bool fadeout)
        {
            PacketWriter stream = Network.GameServer.SetupStream(NetworkIDs.NPCAniStartMessage);
            stream.Write(npc.ID);
            stream.Write((ushort)ani);
            stream.Write(fadeout);

            foreach (Client client in list)
                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W');
        }*/

        #endregion

        public static void WriteEquipMessage(NPC npc, Item item)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCEquipMessage);

            stream.Write((ushort)npc.ID);
            stream.Write((byte)item.Slot);
            item.WriteEquipProperties(stream);

            npc.Cell.ForEachSurroundingClient(client =>
            {
                if (client != npc.client)
                    client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            });
        }

        public static void WriteUnequipMessage(NPC npc, int slot)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCUnequipMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((byte)slot);

            npc.Cell.ForEachSurroundingClient(client =>
            {
                if (client != npc.client)
                    client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
            });
        }
    }
}
