using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.Networking.Requests
{
    public class NPCRequestReceiver
    {
        public event Action<NPCInst, NPCInst.JumpMoves> OnJump;
        public event Action<NPCInst> OnDrawFists;
        public event Action<NPCInst, ItemInst> OnDrawWeapon;
        public event Action<NPCInst, NPCInst.FightMoves> OnFightMove;
        public event Action<NPCInst, ItemInst, int> OnDropItem;
        public event Action<NPCInst, ItemInst> OnEquipItem;
        public event Action<NPCInst, ItemInst> OnUnequipItem;
        public event Action<NPCInst, ItemInst> OnUseItem;


        public void ReadRequest(ScriptRequestMessageIDs id, PacketReader stream, NPCInst npc)
        {
            switch (id)
            {
                case ScriptRequestMessageIDs.JumpFwd:
                    if (OnJump != null)
                        OnJump(npc, NPCInst.JumpMoves.Fwd);
                    break;
                case ScriptRequestMessageIDs.JumpRun:
                    if (OnJump != null)
                        OnJump(npc, NPCInst.JumpMoves.Run);
                    break;
                case ScriptRequestMessageIDs.JumpUp:
                    if (OnJump != null)
                        OnJump(npc, NPCInst.JumpMoves.Up);
                    break;

                case ScriptRequestMessageIDs.DrawFists:
                    if (OnDrawFists != null)
                        OnDrawFists(npc);
                    break;
                case ScriptRequestMessageIDs.DrawWeapon:
                    if (OnDrawWeapon != null)
                        OnDrawWeapon(npc, npc.Inventory.GetItem(stream.ReadByte()));
                    break;

                case ScriptRequestMessageIDs.AttackForward:
                    if (OnFightMove != null)
                        OnFightMove(npc, NPCInst.FightMoves.Fwd);
                    break;
                case ScriptRequestMessageIDs.AttackLeft:
                    if (OnFightMove != null)
                        OnFightMove(npc, NPCInst.FightMoves.Left);
                    break;
                case ScriptRequestMessageIDs.AttackRight:
                    if (OnFightMove != null)
                        OnFightMove(npc, NPCInst.FightMoves.Right);
                    break;
                case ScriptRequestMessageIDs.Parry:
                    if (OnFightMove != null)
                        OnFightMove(npc, NPCInst.FightMoves.Parry);
                    break;
                case ScriptRequestMessageIDs.Dodge:
                    if (OnFightMove != null)
                        OnFightMove(npc, NPCInst.FightMoves.Dodge);
                    break;

                case ScriptRequestMessageIDs.DropItem:
                    if (OnDropItem != null)
                        OnDropItem(npc, npc.Inventory.GetItem(stream.ReadByte()), stream.ReadUShort());
                    break;

                case ScriptRequestMessageIDs.EquipItem:
                    if (OnEquipItem != null)
                        OnEquipItem(npc, npc.Inventory.GetItem(stream.ReadByte()));
                    break;
                case ScriptRequestMessageIDs.UnequipItem:
                    if (OnUnequipItem != null)
                        OnUnequipItem(npc, npc.Inventory.GetItem(stream.ReadByte()));
                    break;

                case ScriptRequestMessageIDs.UseItem:
                    if (OnUseItem != null)
                        OnUseItem(npc, npc.Inventory.GetItem(stream.ReadByte()));
                    break;
                default:
                    Log.Logger.Log("Received Script RequestMessage with invalid ID: " + id.ToString());
                    break;
            }
        }
    }
}
