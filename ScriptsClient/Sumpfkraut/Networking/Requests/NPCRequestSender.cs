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

        public void Attack(NPCInst npc, NPCInst.FightMoves move)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            switch (move)
            {
                case NPCInst.FightMoves.Fwd:
                    stream.Write((byte)ScriptRequestMessageIDs.AttackForward);
                    break;
                case NPCInst.FightMoves.Left:
                    stream.Write((byte)ScriptRequestMessageIDs.AttackLeft);
                    break;
                case NPCInst.FightMoves.Right:
                    stream.Write((byte)ScriptRequestMessageIDs.AttackRight);
                    break;
                case NPCInst.FightMoves.Run:
                    stream.Write((byte)ScriptRequestMessageIDs.AttackRun);
                    break;
                case NPCInst.FightMoves.Parry:
                    stream.Write((byte)ScriptRequestMessageIDs.Parry);
                    break;
                case NPCInst.FightMoves.Dodge:
                    stream.Write((byte)ScriptRequestMessageIDs.Dodge);
                    break;
                default:
                    return;
            }
            NPC.SendScriptCommand(stream, PktPriority.Immediate);
        }

        public void Jump(NPCInst npc, NPCInst.JumpMoves move)
        {
            var stream = npc.BaseInst.GetScriptCommandStream();
            switch (move)
            {
                case NPCInst.JumpMoves.Fwd:
                    stream.Write((byte)ScriptRequestMessageIDs.JumpFwd);
                    break;
                case NPCInst.JumpMoves.Run:
                    stream.Write((byte)ScriptRequestMessageIDs.JumpRun);
                    break;
                case NPCInst.JumpMoves.Up:
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
