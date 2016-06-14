using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using GUC.Animations;
using GUC.Scripting;

namespace GUC.Client.Network.Messages
{
    static class NPCMessage
    {
        const int DelayBetweenMessages = 1500000; //150ms

        #region States

        public static void WriteMoveState(NPC npc, MoveState state)
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.NPCStateMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((byte)state);
            GameClient.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void ReadMoveState(PacketReader stream)
        {
            int id = stream.ReadUShort();
            MoveState state = (MoveState)stream.ReadByte();

            NPC npc;
            if (World.Current.TryGetVob(id, out npc))
            {
                if (npc.ScriptObject != null)
                    npc.ScriptObject.SetState(state);
            }
        }

        #endregion
        
        #region Animations

        #region Overlays

        public static void ReadApplyOverlay(PacketReader stream)
        {
            int id = stream.ReadUShort();

            NPC npc;
            if (World.Current.TryGetVob(id, out npc))
            {
                Overlay ov;
                if (npc.Model.TryGetOverlay(stream.ReadByte(), out ov))
                {
                    npc.ScriptObject.ApplyOverlay(ov);
                }
            }
        }

        public static void ReadRemoveOverlay(PacketReader stream)
        {
            int id = stream.ReadUShort();

            NPC npc;
            if (World.Current.TryGetVob(id, out npc))
            {
                Overlay ov;
                if (npc.Model.TryGetOverlay(stream.ReadByte(), out ov))
                {
                    npc.ScriptObject.RemoveOverlay(ov);
                }
            }
        }

        #endregion

        public static void ReadAniStart(PacketReader stream)
        {
            NPC npc;
            if (World.Current.TryGetVob(stream.ReadUShort(), out npc))
            {
                AniJob job;
                if (npc.Model.TryGetAni(stream.ReadUShort(), out job))
                {
                    Animation ani;
                    if (npc.TryGetAniFromJob(job, out ani))
                        npc.ScriptObject.StartAnimation(ani);
                }
            }
        }

        public static void ReadAniStartWithArgs(PacketReader stream)
        {
            NPC npc;
            if (World.Current.TryGetVob(stream.ReadUShort(), out npc))
            {
                AniJob job;
                if (npc.Model.TryGetAni(stream.ReadUShort(), out job))
                {
                    Animation ani;
                    if (npc.TryGetAniFromJob(job, out ani))
                    {
                        object[] netArgs;
                        npc.ScriptObject.OnReadAniStartArgs(stream, job, out netArgs);
                        npc.ScriptObject.StartAnimation(ani, netArgs);
                    }
                }
            }
        }

        public static void ReadAniStop(PacketReader stream)
        {
            NPC npc;
            if (World.Current.TryGetVob(stream.ReadUShort(), out npc))
            {
                int layerID = stream.ReadByte();
                var ani = npc.GetActiveAniFromLayerID(layerID);
                if (ani != null)
                {
                    npc.ScriptObject.StopAnimation(ani, stream.ReadBit());
                }
            }
        }

        public static void WriteAniStart(AniJob job)
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.NPCAniStartMessage);
            stream.Write((ushort)job.ID);
            GameClient.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void WriteAniStart(AniJob job, object[] netArgs)
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.NPCAniStartWithArgsMessage);
            stream.Write((ushort)job.ID);
            GameClient.Client.character.ScriptObject.OnWriteAniStartArgs(stream, job, netArgs);
            GameClient.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
        }

        #endregion

        #region Equipment

        public static void ReadEquipMessage(PacketReader stream)
        {
            NPC npc;
            if (World.Current.TryGetVob(stream.ReadUShort(), out npc))
            {
                int slot = stream.ReadByte();

                Item item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
                item.ReadEquipProperties(stream);
                npc.ScriptObject.EquipItem(slot, item);
            }
        }

        public static void ReadUnequipMessage(PacketReader stream)
        {
            NPC npc;
            if (World.Current.TryGetVob(stream.ReadUShort(), out npc))
            {
                Item item;
                if (npc.Inventory.TryGetItem(stream.ReadByte(), out item))
                {
                    npc.ScriptObject.UnequipItem(item);
                }
            }
        }

        #endregion
        
        #region Properties

        public static void ReadHealthMessage(PacketReader stream)
        {
            int id = stream.ReadUShort();
            NPC npc;
            if (World.current.TryGetVob(id, out npc))
            {
                int hpmax = stream.ReadUShort();
                int hp = stream.ReadUShort();
                npc.ScriptObject.SetHealth(hp, hpmax);
            }
        }

        #endregion
    }
}
