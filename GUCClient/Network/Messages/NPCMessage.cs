using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using GUC.Animations;

namespace GUC.Client.Network.Messages
{
    static class NPCMessage
    {
        const int DelayBetweenMessages = 1500000; //150ms

        #region States

        public static void WriteState(NPC npc, NPCStates state)
        {
            PacketWriter stream = GameClient.SetupStream(NetworkIDs.NPCStateMessage);
            stream.Write((ushort)npc.ID);
            stream.Write((byte)state);
            GameClient.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);
        }

        public static void ReadState(PacketReader stream)
        {
            int id = stream.ReadUShort();
            NPCStates state = (NPCStates)stream.ReadByte();

            NPC npc;
            if (World.Current.TryGetVob(id, out npc))
            {
                if (npc.ScriptObject != null)
                    npc.ScriptObject.OnCmdMove(state);
            }
        }

        #endregion

        #region Jumping

        /*public static void WriteJump(NPC npc)
        {
            if (DateTime.UtcNow.Ticks > npc.nextJumpUpdate)
            {
                PacketWriter stream = GameClient.SetupStream(NetworkIDs.NPCJumpMessage);
                stream.Write((ushort)npc.ID);
                GameClient.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);

                npc.nextJumpUpdate = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }

        public static void ReadJump(PacketReader stream)
        {
            int id = stream.ReadUShort();

            NPC npc;
            if (World.Current.TryGetVob(id, out npc))
            {
                if (npc.ScriptObject != null)
                    npc.ScriptObject.OnCmdJump();
            }
        }*/

        #endregion

        #region Animations

        public static void ReadApplyOverlay(PacketReader stream)
        {
            int id = stream.ReadUShort();

            NPC npc;
            if (World.Current.TryGetVob(id, out npc))
            {
                Overlay ov;
                if (npc.Model.TryGetOverlay(stream.ReadByte(), out ov))
                {
                    npc.ScriptObject.OnCmdApplyOverlay(ov);
                }
                else
                {
                    throw new Exception("Overlay not found!");
                }
            }
            else
            {
                throw new Exception("NPC not found!");
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
                    npc.ScriptObject.OnCmdRemoveOverlay(ov);
                }
                else
                {
                    throw new Exception("Overlay not found!");
                }
            }
            else
            {
                throw new Exception("NPC not found!");
            }
        }

        #endregion

        /*public static void ReadTargetState(BitStream stream)
        {
            NPC npc;
            World.npcDict.TryGetValue(stream.mReadUInt(), out npc);
            if (npc == null) return;

            //Just in case the npc is turning
            npc.StopTurnAnis();

            NPCState state = (NPCState)stream.mReadByte();

            uint targetID = stream.mReadUInt();

            oCNpc targetVob = null;
            if (targetID != 0)
            {
                NPC target = null;
                World.npcDict.TryGetValue(targetID, out target);
                if (target != null) targetVob = target.gNpc;
            }
            if (targetVob == null)
            {
                targetVob = new oCNpc();
            }

            npc.gNpc.SetEnemy(targetVob);

            oCMsgAttack msg;
            switch (state)
            {
                case NPCState.AttackForward:

                    if (npc.State == NPCState.AttackForward)
                    {
                        zERROR.GetZErr(Program.Process).Report(2, 'G', "COMBO!", 0, "Program.cs", 0);
                        hAniCtrl_Human.DoCombo = true;
                        npc.gAniCtrl.HitCombo(1);
                        return;
                    }
                    zERROR.GetZErr(Program.Process).Report(2, 'G', "ATTACK!", 0, "Program.cs", 0);
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.AttackForward, npc.gAniCtrl._t_hitf, 1);
                    break;
                case NPCState.AttackLeft:
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.AttackLeft, npc.gAniCtrl._t_hitl, 1);
                    break;
                case NPCState.AttackRight:
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.AttackRight, npc.gAniCtrl._t_hitr, 1);
                    break;
                case NPCState.AttackRun:
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.AttackRun, npc.gAniCtrl._t_hitfrun, 1);
                    break;
                case NPCState.Parry:
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.Parade, targetVob, 0);
                    break;
                case NPCState.DodgeBack:
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.Parade, targetVob, 0);
                    msg.Bitfield |= oCMsgAttack.BitFlag.Dodge;
                    break;
                default:
                    return;
            }
            npc.State = state;
            npc.gVob.GetEM(0).KillMessages();
            npc.gVob.GetEM(0).StartMessage(msg, npc.gVob);
        }

        const int DelayBetweenMessages = 5000000; //500ms

        static long nextDrawItemTime = 0;
        public static void WriteDrawItem(byte slot)
        {
            if (DateTime.UtcNow.Ticks > nextDrawItemTime)
            {
                BitStream stream = Program.client.SetupSendStream(NetworkID.NPCDrawItemMessage);

                stream.mWrite(slot);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);

                nextDrawItemTime = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }

        public static void ReadDrawItem(BitStream stream)
        {
            uint ID = stream.mReadUInt();

            NPC npc;
            World.npcDict.TryGetValue(ID, out npc);
            if (npc == null) return;

            Item item = ReadStrmDrawItem(stream, npc);

            npc.DrawItem(item, stream.ReadBit());
        }

        public static Item ReadStrmDrawItem(BitStream stream, NPC npc)
        {
            uint itemID = stream.mReadUInt();

            Item item = null;
            if (itemID != 0)
            {
                ushort instanceID = stream.mReadUShort();
                if (npc == Player.Hero) //search in inventory for item
                {
                    Player.Inventory.TryGetValue(itemID, out item);
                }

                if (item == null) //search in Equipment
                {
                    item = npc.equippedSlots.Values.FirstOrDefault(x => x.ID == itemID);
                }

                if (item == null)
                {
                    item = new Item(itemID, instanceID);
                }

                byte talent = stream.mReadByte();
                if (item.Type == ItemType.Blunt_1H || item.Type == ItemType.Sword_1H)
                    npc.gNpc.SetTalentSkill(1, talent);
                else if (item.Type == ItemType.Blunt_2H || item.Type == ItemType.Sword_2H)
                    npc.gNpc.SetTalentSkill(2, talent);
            }
            else
            {
                item = Item.Fists;
            }

            return item;
        }

        public static void WriteUndrawItem()
        {
            if (DateTime.UtcNow.Ticks > nextDrawItemTime)
            {
                BitStream stream = Program.client.SetupSendStream(NetworkID.NPCUndrawItemMessage);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);

                nextDrawItemTime = DateTime.UtcNow.Ticks + DelayBetweenMessages;
            }
        }

        public static void ReadUndrawItem(BitStream stream)
        {
            uint ID = stream.mReadUInt();

            NPC npc;
            World.npcDict.TryGetValue(ID, out npc);
            if (npc == null) return;

            Item item = npc.DrawnItem;

            if (item == null)
                return;

            bool fast = stream.ReadBit();
            bool altRemove = stream.ReadBit();

            npc.UndrawItem(altRemove, fast);
        }*/

        /*
        #region Animation

        public static void WriteAnimationStart(Animations ani)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCAniStartMessage);
            stream.mWrite((ushort)ani);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void WriteAnimationStop(Animations ani, bool fadeout)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCAniStopMessage);
            stream.mWrite((ushort)ani);
            stream.mWrite(fadeout);
            Program.client.SendStream(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE);
        }

        public static void ReadAniStart(BitStream stream)
        {
            uint id = stream.mReadUInt();

            NPC npc;
            World.npcDict.TryGetValue(id, out npc);
            if (npc == null) return;

            Animations ani = (Animations)stream.mReadUShort();
            npc.AnimationStart(ani);
        }

        public static void ReadAniStop(BitStream stream)
        {
            uint id = stream.mReadUInt();

            NPC npc;
            World.npcDict.TryGetValue(id, out npc);
            if (npc == null) return;

            Animations ani = (Animations)stream.mReadUShort();
            bool fadeout = stream.ReadBit();
            if (fadeout)
            {
                npc.AnimationFade(ani);
            }
            else
            {
                npc.AnimationStop(ani);
            }
        }

        #endregion
        */

        /*
                public static void ReadHealthMessage(BitStream stream)
                {
                    NPC npc;
                    World.npcDict.TryGetValue(stream.mReadUInt(), out npc);
                    if (npc == null) return;

                    npc.HPMax = stream.mReadUShort();
                    npc.HP = stream.mReadUShort();
                }

                #region Appearance

                public static void ReadEquipMessage(BitStream stream)
                {
                    uint npcID = stream.mReadUInt();

                    NPC npc;
                    if (World.npcDict.TryGetValue(npcID, out npc))
                    {
                        uint itemID = stream.mReadUInt();
                        ushort itemInstanceID = stream.mReadUShort();
                        ushort itemCondition = stream.mReadUShort();
                        byte slot = stream.mReadByte();

                        Item item = new Item(itemID, itemInstanceID);
                        item.Condition = itemCondition;
                        npc.EquipSlot(slot, item);
                    }
                }

                public static void ReadUnequipMessage(BitStream stream)
                {
                    uint npcID = stream.mReadUInt();

                    NPC npc;
                    if (World.npcDict.TryGetValue(npcID, out npc))
                    {
                        byte slot = stream.mReadByte();
                        npc.UnequipSlot(slot);
                    }
                }

                #endregion*/
    }
}
