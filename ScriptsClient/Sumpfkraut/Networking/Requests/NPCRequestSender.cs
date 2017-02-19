using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Network;
using GUC.WorldObjects;

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
            NPC.SendScriptCommand(stream, PktPriority.Low);
        }

        public void TakeItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.TakeItem);
            stream.Write((ushort)item.ID);
            NPC.SendScriptCommand(stream, PktPriority.Low);
        }

        public void EquipItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.EquipItem);
            stream.Write((ushort)item.ID);
            NPC.SendScriptCommand(stream, PktPriority.Low);
        }

        public void UnequipItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.UnequipItem);
            stream.Write((ushort)item.ID);
            NPC.SendScriptCommand(stream, PktPriority.Low);
        }

        public void UseItem(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.UseItem);
            stream.Write((ushort)item.ID);
            NPC.SendScriptCommand(stream, PktPriority.Low);
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
            NPC.SendScriptCommand(stream, PktPriority.Immediate);
        }

        public void Jump(NPCInst npc, JumpMoves move)
        {
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
            NPC.SendScriptCommand(stream, PktPriority.High);
        }

        public void DrawWeapon(NPCInst npc, ItemInst item)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.DrawWeapon);
            stream.Write((byte)item.ID);
            NPC.SendScriptCommand(stream, PktPriority.Medium);
        }

        public void DrawFists(NPCInst npc)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            stream.Write((byte)ScriptRequestMessageIDs.DrawFists);
            NPC.SendScriptCommand(stream, PktPriority.Medium);
        }
    }
}
