using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Networking.Requests
{
    public class NPCRequestSender
    {
        public void DropItem(NPCInst npc, ItemInst item, int amount)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.DropItem);
            stream.Write((byte)item.ID);
            stream.Write((ushort)amount);
            NPC.SendScriptCommand(stream, NetPriority.Low);
        }

        public void TakeItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.TakeItem);
            stream.Write((ushort)item.ID);
            NPC.SendScriptCommand(stream, NetPriority.Low);
        }

        public void EquipItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.EquipItem);
            stream.Write((ushort)item.ID);
            NPC.SendScriptCommand(stream, NetPriority.Low);
        }

        public void UnequipItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.UnequipItem);
            stream.Write((ushort)item.ID);
            NPC.SendScriptCommand(stream, NetPriority.Low);
        }

        public void UseItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.UseItem);
            stream.Write((ushort)item.ID);
            NPC.SendScriptCommand(stream, NetPriority.Low);
        }

        public void Attack(NPCInst npc, FightMoves move)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            switch (move)
            {
                case FightMoves.Fwd:
                    stream.Write((byte)ScriptRequestMessageIDs.AttackForward);
                    break;
                case FightMoves.Left:
                    stream.Write((byte)ScriptRequestMessageIDs.AttackLeft);
                    break;
                case FightMoves.Right:
                    stream.Write((byte)ScriptRequestMessageIDs.AttackRight);
                    break;
                case FightMoves.Run:
                    stream.Write((byte)ScriptRequestMessageIDs.AttackRun);
                    break;
                case FightMoves.Parry:
                    stream.Write((byte)ScriptRequestMessageIDs.Parry);
                    break;
                case FightMoves.Dodge:
                    stream.Write((byte)ScriptRequestMessageIDs.Dodge);
                    break;
                default:
                    return;
            }
            NPC.SendScriptCommand(stream, NetPriority.Immediate);
        }

        LockTimer jumpTimer = new LockTimer(500);
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
                    stream.Write((byte)ScriptRequestMessageIDs.JumpFwd);
                    break;
                case JumpMoves.Run:
                    stream.Write((byte)ScriptRequestMessageIDs.JumpRun);
                    break;
                case JumpMoves.Up:
                    stream.Write((byte)ScriptRequestMessageIDs.JumpUp);
                    break;
                default:
                    return;
            }
            NPC.SendScriptCommand(stream, NetPriority.Medium);
        }

        public void DrawWeapon(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.DrawWeapon);
            stream.Write((byte)item.ID);
            NPC.SendScriptCommand(stream, NetPriority.Medium);
        }

        public void DrawFists(NPCInst npc)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.DrawFists);
            NPC.SendScriptCommand(stream, NetPriority.Medium);
        }
        
        public void Climb(NPCInst npc, NPC.ClimbingLedge ledge)
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
                    stream.Write((byte)ScriptRequestMessageIDs.ClimbHigh);
                    break;
                case ClimbMoves.Mid:
                    stream.Write((byte)ScriptRequestMessageIDs.ClimbMid);
                    break;
                case ClimbMoves.Low:
                    stream.Write((byte)ScriptRequestMessageIDs.ClimbLow);
                    break;
                default:
                    return;
            }

            ledge.WriteStream(stream);

            NPC.SendScriptCommand(stream, NetPriority.Medium);
        }
    }
}
