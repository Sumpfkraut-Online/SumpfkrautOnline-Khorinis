using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Network;
using GUC.Animations;

namespace GUC.Server.Network.Messages
{
    static class NPCMessage
    {
        #region States

        public static void ReadMoveState(PacketReader stream, GameClient client, NPC character, World world)
        {
            int id = stream.ReadUShort();
            NPC npc;
            if (world.TryGetVob(id, out npc))
            {
                MoveState state = (MoveState)stream.ReadByte();
                if (npc == character /*|| (client.VobControlledList.Contains(npc) && state <= NPCStates.MoveBackward)*/) //is it a controlled NPC?
                {
                    if (npc.ScriptObject != null)
                        npc.ScriptObject.OnCmdMove(state);
                }
            }
        }

        public static void WriteMoveState(NPC npc, MoveState state)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCStateMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((byte)state);

            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        #endregion

        #region Jumping

        public static void ReadJump(PacketReader stream, NPC character)
        {
            AniJob job;
            if (character.Model.TryGetAni(stream.ReadUShort(), out job))
            {
                Animation ani;
                if (character.TryGetAniFromJob(job, out ani))
                    character.ScriptObject.OnCmdAniJump(ani);
            }
        }

        public static void WriteJump(NPC npc, Animation ani)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCJumpMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((ushort)ani.AniJob.ID);
            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        public static void WriteJump(NPC npc, Animation ani, int upVel, int fwdVel)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCJumpVelMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((ushort)ani.AniJob.ID);
            stream.Write((ushort)upVel);
            stream.Write((ushort)fwdVel);
            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        #endregion

        #region Item drawing

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

        #endregion

        #region Animation

        #region Overlays

        public static void ReadApplyOverlay(PacketReader stream, NPC npc)
        {
            Overlay ov;
            if (npc.Model.TryGetOverlay(stream.ReadByte(), out ov))
            {
                npc.ScriptObject.ApplyOverlay(ov);
            }
        }

        public static void ReadRemoveOverlay(PacketReader stream, NPC npc)
        {
            Overlay ov;
            if (npc.Model.TryGetOverlay(stream.ReadByte(), out ov))
            {
                npc.ScriptObject.RemoveOverlay(ov);
            }
        }

        public static void WriteApplyOverlayMessage(NPC npc, Overlay overlay)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCApplyOverlayMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((byte)overlay.ID);

            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W'));
        }

        public static void WriteRemoveOverlayMessage(NPC npc, Overlay overlay)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCRemoveOverlayMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((byte)overlay.ID);

            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W'));
        }

        #endregion

        public static void ReadAniStart(PacketReader stream, NPC character)
        {
            AniJob job;
            if (character.Model.TryGetAni(stream.ReadUShort(), out job))
            {
                Animation ani;
                if (character.TryGetAniFromJob(job, out ani))
                    character.ScriptObject.OnCmdAniStart(ani);
            }
        }

        public static void ReadAniStop(PacketReader stream, NPC character)
        {
            character.ScriptObject.OnCmdAniStop(stream.ReadBit());
        }


        public static void WriteAniStart(NPC npc, Animation ani)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCAniStartMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((ushort)ani.AniJob.ID);

            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        public static void WriteAniStop(NPC npc, Animation ani, bool fadeout)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.NPCAniStopMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((byte)ani.LayerID);
            stream.Write(fadeout);

            npc.Cell.ForEachSurroundingClient(c => c.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        #endregion

        #region Equipment

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

        #endregion

        #region Properties

        public static void WriteHealthMessage(NPC npc)
        {
            var stream = GameServer.SetupStream(NetworkIDs.NPCHealthMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((ushort)npc.HPMax);
            stream.Write((ushort)npc.HP);
            npc.Cell.ForEachSurroundingClient(client => client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W'));
        }

        #endregion

    }
}
