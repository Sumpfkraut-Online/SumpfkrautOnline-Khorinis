using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects;

namespace GUC.Scripts.Sumpfkraut.Networking.Requests
{
    public class NPCRequestReceiver
    {
        public event Action<NPCInst, JumpMoves> OnJump;
        public event Action<NPCInst, ClimbMoves, NPC.ClimbingLedge> OnClimb;
        public event Action<NPCInst> OnDrawFists;
        public event Action<NPCInst, ItemInst> OnDrawWeapon;
        public event Action<NPCInst, FightMoves> OnFightMove;
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
                        OnJump(npc, JumpMoves.Fwd);
                    break;
                case ScriptRequestMessageIDs.JumpRun:
                    if (OnJump != null)
                        OnJump(npc, JumpMoves.Run);
                    break;
                case ScriptRequestMessageIDs.JumpUp:
                    if (OnJump != null)
                        OnJump(npc, JumpMoves.Up);
                    break;

                case ScriptRequestMessageIDs.ClimbHigh:
                    if (OnClimb != null)
                        OnClimb(npc, ClimbMoves.High, new NPC.ClimbingLedge(stream));
                    break;
                case ScriptRequestMessageIDs.ClimbMid:
                    if (OnClimb != null)
                        OnClimb(npc, ClimbMoves.Mid, new NPC.ClimbingLedge(stream));
                    break;
                case ScriptRequestMessageIDs.ClimbLow:
                    if (OnClimb != null)
                        OnClimb(npc, ClimbMoves.Low, new NPC.ClimbingLedge(stream));
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
                        OnFightMove(npc, FightMoves.Fwd);
                    break;
                case ScriptRequestMessageIDs.AttackLeft:
                    if (OnFightMove != null)
                        OnFightMove(npc, FightMoves.Left);
                    break;
                case ScriptRequestMessageIDs.AttackRight:
                    if (OnFightMove != null)
                        OnFightMove(npc, FightMoves.Right);
                    break;
                case ScriptRequestMessageIDs.AttackRun:
                    if (OnFightMove != null)
                        OnFightMove(npc, FightMoves.Run);
                    break;
                case ScriptRequestMessageIDs.Parry:
                    if (OnFightMove != null)
                        OnFightMove(npc, FightMoves.Parry);
                    break;
                case ScriptRequestMessageIDs.Dodge:
                    if (OnFightMove != null)
                        OnFightMove(npc, FightMoves.Dodge);
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
