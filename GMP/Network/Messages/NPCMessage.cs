using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using GUC.Client.WorldObjects;
using Gothic.zStruct;
using WinApi;
using Gothic.zClasses;

namespace GUC.Client.Network.Messages
{
    static class NPCMessage
    {

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

        #region States

        public static void WriteState(NPC npc)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCStateMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((byte)npc.State);
            stream.mWrite(npc.Position);
            stream.mWrite(npc.Direction);
            Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED);
            npc.nextPosUpdate = DateTime.Now.Ticks + NPC.PositionUpdateTime; //set position update time
        }

        public static void ReadState(BitStream stream)
        {
            uint id = stream.mReadUInt();

            NPC npc;
            World.npcDict.TryGetValue(id, out npc);
            if (npc == null) return;

            if (npc.State == NPCState.Stand)
            {
                npc.StopTurnAnis();
            }

            npc.State = (NPCState)stream.mReadByte();
            npc.Position = stream.mReadVec();
            npc.Direction = stream.mReadVec();

            switch (npc.State)
            {
                default:
                    npc.Update(DateTime.Now.Ticks);
                    break;
            }
        }

        public static void WriteWeaponState()
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCWeaponStateMessage);
            stream.mWrite((byte)Player.Hero.WeaponState);
            stream.mWrite(Player.Hero.Position);
            stream.mWrite(Player.Hero.Direction);
            Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED);
            Player.Hero.nextPosUpdate = DateTime.Now.Ticks + NPC.PositionUpdateTime; //set position update time
        }

        public static void ReadWeaponState(BitStream stream)
        {
            NPC npc;
            World.npcDict.TryGetValue(stream.mReadUInt(), out npc);
            if (npc == null) return;

            npc.WeaponState = (NPCWeaponState)stream.mReadByte();
            npc.Position = stream.mReadVec();
            npc.Direction = stream.mReadVec();

            switch (npc.WeaponState)
            {
                case NPCWeaponState.Fists: //FIXME
                case NPCWeaponState.Melee:
                    npc.gNpc.DrawMeleeWeapon();
                    break;
                case NPCWeaponState.Ranged:
                case NPCWeaponState.Magic: //FIXME
                    npc.gNpc.DrawRangedWeapon();
                    break;
                default:
                    npc.gNpc.RemoveWeapon();
                    break;
            }
        }

        #endregion

        #region Combat

        public static void WriteAttack()
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCAttackMessage);
            stream.mWrite((byte)Player.Hero.State);
            stream.mWrite(Player.Hero.Position);
            stream.mWrite(Player.Hero.Direction);

            Vob target;
            World.vobAddr.TryGetValue(Player.Hero.gNpc.GetFocusNpc().Address, out target);
            if (target == null)
            {
                stream.mWrite(0);
            }
            else
            {
                stream.mWrite(target.ID);
            }

            Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED);
            Player.Hero.nextPosUpdate = DateTime.Now.Ticks + NPC.PositionUpdateTime; //set position update time
        }

        public static void ReadAttack(BitStream stream)
        {
            NPC attacker;
            World.npcDict.TryGetValue(stream.mReadUInt(), out attacker);
            if (attacker == null) return;

            NPCState state = (NPCState)stream.mReadByte();
            attacker.Position = stream.mReadVec();
            attacker.Direction = stream.mReadVec();

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
                targetVob = new oCNpc(null, 0);
            }

            attacker.gNpc.SetEnemy(targetVob);

            oCMsgAttack msg;
            switch (state)
            {
                case NPCState.AttackForward:
                    if (attacker.State == NPCState.AttackForward)
                    {
                        attacker.gNpc.AniCtrl.BitField |= oCAniCtrl_Human.BitFlag.canEnableNextCombo;
                        attacker.gNpc.AniCtrl.HitCombo(1);
                        return;
                    }
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.AttackForward, attacker.gNpc.AniCtrl._t_hitf, 1);
                    break;
                case NPCState.AttackLeft:
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.AttackLeft, attacker.gNpc.AniCtrl._t_hitl, 1);
                    break;
                case NPCState.AttackRight:
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.AttackRight, attacker.gNpc.AniCtrl._t_hitr, 1);
                    break;
                case NPCState.AttackRun:
                    msg = oCMsgAttack.Create(Program.Process, oCMsgAttack.SubTypes.AttackRun, attacker.gNpc.AniCtrl._t_hitfrun, 1);
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
            attacker.State = state;
            attacker.gVob.GetEM(0).KillMessages();
            attacker.gVob.GetEM(0).OnMessage(msg, attacker.gVob);
        }

        public static void WriteHits(NPC attacker)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCHitMessage);
            stream.mWrite(attacker.ID);
            Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED);
        }

        public static void ReadHitMessage(BitStream stream)
        {
            NPC attacker, target;
            World.npcDict.TryGetValue(stream.mReadUInt(), out attacker);
            World.npcDict.TryGetValue(stream.mReadUInt(), out target);
            if (target != null && attacker != null)
            {
                attacker.gNpc.AniCtrl.CreateHit(target.gVob);
            }
        }

        #endregion

        #region Appearance

        public static void ReadEquipMessage(BitStream stream)
        {
            NPC npc;
            World.npcDict.TryGetValue(stream.mReadUInt(), out npc);
            if (npc == null) return;

            ItemInstance inst;
            ItemInstance.InstanceDict.TryGetValue(stream.mReadUInt(), out inst);
            if (inst == null) return;

            if (inst == npc.EquippedMeleeWeapon)
            {
                npc.EquippedMeleeWeapon = null;
                npc.gNpc.UnequipItem(npc.gNpc.GetEquippedMeleeWeapon());
            }
            else if (inst == npc.EquippedRangedWeapon)
            {
                npc.EquippedRangedWeapon = null;
                npc.gNpc.UnequipItem(npc.gNpc.GetEquippedRangedWeapon());
            }
            else if (inst == npc.EquippedArmor)
            {
                npc.EquippedArmor = null;
                npc.gNpc.UnequipItem(npc.gNpc.GetEquippedArmor());
            }
            else
            {
                if (inst.MainFlags == MainFlags.ITEM_KAT_NF)
                {
                    npc.EquippedMeleeWeapon = inst;
                    npc.gNpc.UnequipItem(npc.gNpc.GetEquippedMeleeWeapon());
                }
                else if (inst.MainFlags == MainFlags.ITEM_KAT_FF)
                {
                    npc.EquippedRangedWeapon = inst;
                    npc.gNpc.UnequipItem(npc.gNpc.GetEquippedRangedWeapon());
                }
                else if (inst.MainFlags == MainFlags.ITEM_KAT_ARMOR)
                {
                    npc.EquippedArmor = inst;
                    npc.gNpc.UnequipItem(npc.gNpc.GetEquippedArmor());
                }
                else return;

                oCItem newItem = oCItem.Create(Program.Process);
                inst.InitItem(newItem);
                npc.gNpc.Equip(newItem);
            }
        }

        #endregion
    }
}
