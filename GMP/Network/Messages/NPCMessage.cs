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
using GUC.Client.Hooks;

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

        public static void WriteState(NPCState state, NPC npc)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCStateMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((byte)state);
            stream.mWrite(npc.Position);
            stream.mWrite(npc.Direction);
            Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED);

            npc.nextPosUpdate = DateTime.Now.Ticks + NPC.PositionUpdateTime; //set position update time
        }

        public static void WriteTargetState(NPCState state) //only for self, attacks & strafing
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCTargetStateMessage);
            stream.mWrite((byte)state);
            stream.mWrite(Player.Hero.Position);
            stream.mWrite(Player.Hero.Direction);

            AbstractVob target;
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

        public static void ReadState(BitStream stream)
        {
            uint id = stream.mReadUInt();

            NPC npc;
            World.npcDict.TryGetValue(id, out npc);
            if (npc == null) return;

            if (npc.State == NPCState.Stand)
            {   //Just in case the npc is turning
                npc.StopTurnAnis();
            }

            npc.State = (NPCState)stream.mReadByte();
            npc.Position = stream.mReadVec();
            npc.Direction = stream.mReadVec();

            switch (npc.State)
            {
                case NPCState.Jump:
                    npc.gNpc.AniCtrl.PC_JumpForward();
                    break;
                case NPCState.Fall:
                    npc.gNpc.AniCtrl.StartFallDownAni();
                    break;
                default:
                    npc.Update(DateTime.Now.Ticks);
                    break;
            }
        }

        const int DelayBetweenMessages = 5000000; //500ms

        static long nextWeaponTime = 0;
        public static void WriteWeaponState(NPCWeaponState state, bool removeType1)
        {
            if (DateTime.Now.Ticks > nextWeaponTime)
            {
                BitStream stream = Program.client.SetupSendStream(NetworkID.NPCWeaponStateMessage);
                stream.mWrite((byte)state);
                stream.mWrite(removeType1);
                Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE);

                nextWeaponTime = DateTime.Now.Ticks + DelayBetweenMessages;
            }
        }

        public static void ReadWeaponState(BitStream stream)
        {
            uint ID = stream.mReadUInt();

            NPC npc;
            if (World.npcDict.TryGetValue(ID, out npc))
            {
                npc.WeaponState = (NPCWeaponState)stream.mReadByte();
                bool removeType1 = stream.ReadBit();

                oCMsgWeapon msg = null;
                switch (npc.WeaponState)
                {
                    case NPCWeaponState.Fists:
                    //npc.DrawFists();
                    //break;
                    case NPCWeaponState.Melee:
                        msg = oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.DrawWeapon, 0, 0);
                        break;
                    case NPCWeaponState.Ranged:
                    case NPCWeaponState.Magic: //FIXME
                        msg = oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.DrawWeapon, 4, 0);
                        break;
                    case NPCWeaponState.None:
                        if (removeType1)
                        {
                            msg = oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.RemoveWeapon1, 0, 0);
                            npc.gVob.GetEM(0).StartMessage(msg, npc.gVob);
                            msg = oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.RemoveWeapon2, 0, 0);
                        }
                        else
                        {
                            msg = oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.RemoveWeapon, 0, 0);
                        }
                        break;
                }

                if (msg != null)
                {
                    npc.gVob.GetEM(0).StartMessage(msg, npc.gVob);
                }
            }
        }

        #endregion

        #region Combat



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

        public static void WriteSelfHit(NPC attacker)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCHitMessage);
            stream.mWrite(attacker.ID);
            stream.mWrite((byte)0);
            Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED);
        }

        public static void WriteHits(NPC attacker, List<NPC> hitlist)
        {
            BitStream stream = Program.client.SetupSendStream(NetworkID.NPCHitMessage);
            stream.mWrite(attacker.ID);
            stream.mWrite((byte)hitlist.Count);
            for (int i = 0; i < hitlist.Count; i++)
            {
                stream.mWrite(hitlist[i].ID);
            }
            Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED);
        }

        public static void ReadHitMessage(BitStream stream)
        {
            NPC npc;
            World.npcDict.TryGetValue(stream.mReadUInt(), out npc);
            if (npc == null) return;

            npc.gNpc.HPMax = stream.mReadUShort();
            npc.gNpc.HP = stream.mReadUShort();
        }

        #endregion

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

        #endregion
    }
}
