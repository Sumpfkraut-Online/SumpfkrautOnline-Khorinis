using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects;
using GUC.Utilities;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs;

namespace GUC.Scripts.Sumpfkraut.Networking.Requests
{
    public class NPCRequestSender
    {

        public void Voice(NPCInst npc, VoiceCmd cmd)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.Voice);
            stream.Write((byte)cmd);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }

        public void HelpUp(NPCInst npc, NPCInst target)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.HelpUp);
            stream.Write((ushort)target.ID);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }


        public void DropItem(NPCInst npc, ItemInst item, int amount)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.DropItem);
            stream.Write((byte)item.ID);
            stream.Write((ushort)amount);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }

        public void TakeItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.TakeItem);
            stream.Write((ushort)item.ID);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }

        public void EquipItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.EquipItem);
            stream.Write((ushort)item.ID);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }

        public void UnequipItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.UnequipItem);
            stream.Write((ushort)item.ID);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }

        public void UseItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.UseItem);
            stream.Write((ushort)item.ID);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }

        public void StartUseMob(NPCInst npc, MobInst mobInst)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.StartUseMob);
            stream.Write((ushort)mobInst.ID);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }

        public void StopUseMob(NPCInst npc)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.StopUseMob);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Low);
        }

        public void Attack(NPCInst npc, FightMoves move)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            switch (move)
            {
                case FightMoves.Fwd:
                    stream.Write((byte)RequestMessageIDs.AttackForward);
                    break;
                case FightMoves.Left:
                    stream.Write((byte)RequestMessageIDs.AttackLeft);
                    break;
                case FightMoves.Right:
                    stream.Write((byte)RequestMessageIDs.AttackRight);
                    break;
                case FightMoves.Run:
                    stream.Write((byte)RequestMessageIDs.AttackRun);
                    break;
                case FightMoves.Parry:
                    stream.Write((byte)RequestMessageIDs.Parry);
                    break;
                case FightMoves.Dodge:
                    stream.Write((byte)RequestMessageIDs.Dodge);
                    break;
                default:
                    return;
            }
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Immediate);
        }

        LockTimer jumpTimer = new LockTimer(200);
        public void Jump(NPCInst npc)
        {
            if (!jumpTimer.IsReady)
                return;

            JumpMoves move;
            if (npc.Movement == Types.NPCMovement.Forward)
            {
                move = JumpMoves.Run;
            }
            else
            {
                move = JumpMoves.Fwd;
            }


            var stream = npc.BaseInst.GetScriptCommandStream();
            switch (move)
            {
                case JumpMoves.Fwd:
                    stream.Write((byte)RequestMessageIDs.JumpFwd);
                    break;
                case JumpMoves.Run:
                    stream.Write((byte)RequestMessageIDs.JumpRun);
                    break;
                case JumpMoves.Up:
                    stream.Write((byte)RequestMessageIDs.JumpUp);
                    break;
                default:
                    return;
            }
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Medium);
        }

        public void DrawWeapon(NPCInst npc, ItemInst item)
        {
            if (npc == null || item == null)
                return;

            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.DrawWeapon);
            stream.Write((byte)item.ID);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Medium);
        }

        public void DrawFists(NPCInst npc)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.DrawFists);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Medium);
        }
        
        public void Climb(NPCInst npc, GUCNPCInst.ClimbingLedge ledge)
        {
            if (!jumpTimer.IsReady)
                return;

            ClimbMoves move;
            var gAI = npc.BaseInst.gAI;
            float dist = ledge.Location.Y - gAI.FeetY;
            if (dist < gAI.StepHeight)
            {
                return;
            }
            else if (dist < gAI.YMaxJumpLow)
            {
                move = ClimbMoves.Low;
            }
            else if (dist < gAI.YMaxJumpMid)
            {
                move = ClimbMoves.Mid;
            }
            else
            {
                move = ClimbMoves.High;
            }

            var stream = npc.BaseInst.GetScriptCommandStream();
            switch (move)
            {
                case ClimbMoves.High:
                    stream.Write((byte)RequestMessageIDs.ClimbHigh);
                    break;
                case ClimbMoves.Mid:
                    stream.Write((byte)RequestMessageIDs.ClimbMid);
                    break;
                case ClimbMoves.Low:
                    stream.Write((byte)RequestMessageIDs.ClimbLow);
                    break;
                default:
                    return;
            }

            ledge.WriteStream(stream);

            GUCNPCInst.SendScriptCommand(stream, NetPriority.Medium);
        }
        
        public void Aim(NPCInst npc, bool doAim)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)(doAim ? RequestMessageIDs.Aim : RequestMessageIDs.Unaim));
            GUCNPCInst.SendScriptCommand(stream, NetPriority.High);
        }

        public void Shoot(NPCInst npc, Vec3f start, Vec3f end)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)RequestMessageIDs.Shoot);
            stream.Write(start);
            stream.Write(end);
            GUCNPCInst.SendScriptCommand(stream, NetPriority.Immediate);
        }
    }
}
