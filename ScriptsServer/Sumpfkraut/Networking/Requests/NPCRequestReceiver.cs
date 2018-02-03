using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects;
using GUC.Types;

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
        public event Action<NPCInst, ItemInst> OnTakeItem;
        public event Action<NPCInst, ItemInst> OnEquipItem;
        public event Action<NPCInst, ItemInst> OnUnequipItem;
        public event Action<NPCInst, ItemInst> OnUseItem;
        public event Action<NPCInst> OnAim;
        public event Action<NPCInst> OnUnaim;
        public event Action<NPCInst, Vec3f, Vec3f> OnShoot;


        public void ReadRequest(RequestMessageIDs id, PacketReader stream, NPCInst npc)
        {
            switch (id)
            {
                case RequestMessageIDs.JumpFwd:
                    OnJump?.Invoke(npc, JumpMoves.Fwd);
                    break;
                case RequestMessageIDs.JumpRun:
                    OnJump?.Invoke(npc, JumpMoves.Run);
                    break;
                case RequestMessageIDs.JumpUp:
                    OnJump?.Invoke(npc, JumpMoves.Up);
                    break;

                case RequestMessageIDs.ClimbHigh:
                    OnClimb?.Invoke(npc, ClimbMoves.High, new NPC.ClimbingLedge(stream));
                    break;
                case RequestMessageIDs.ClimbMid:
                    OnClimb?.Invoke(npc, ClimbMoves.Mid, new NPC.ClimbingLedge(stream));
                    break;
                case RequestMessageIDs.ClimbLow:
                    OnClimb?.Invoke(npc, ClimbMoves.Low, new NPC.ClimbingLedge(stream));
                    break;

                case RequestMessageIDs.DrawFists:
                    OnDrawFists?.Invoke(npc);
                    break;
                case RequestMessageIDs.DrawWeapon:
                    OnDrawWeapon?.Invoke(npc, npc.Inventory.GetItem(stream.ReadByte()));
                    break;

                case RequestMessageIDs.AttackForward:
                    OnFightMove?.Invoke(npc, FightMoves.Fwd);
                    break;
                case RequestMessageIDs.AttackLeft:
                    OnFightMove?.Invoke(npc, FightMoves.Left);
                    break;
                case RequestMessageIDs.AttackRight:
                    OnFightMove?.Invoke(npc, FightMoves.Right);
                    break;
                case RequestMessageIDs.AttackRun:
                    OnFightMove?.Invoke(npc, FightMoves.Run);
                    break;
                case RequestMessageIDs.Parry:
                    OnFightMove?.Invoke(npc, FightMoves.Parry);
                    break;
                case RequestMessageIDs.Dodge:
                    OnFightMove?.Invoke(npc, FightMoves.Dodge);
                    break;

                case RequestMessageIDs.DropItem:
                    OnDropItem?.Invoke(npc, npc.Inventory.GetItem(stream.ReadByte()), stream.ReadUShort());
                    break;
                case RequestMessageIDs.TakeItem:
                    if (npc.World.TryGetVob(stream.ReadUShort(), out ItemInst item))
                        OnTakeItem?.Invoke(npc, item);
                    break;

                case RequestMessageIDs.EquipItem:
                    OnEquipItem?.Invoke(npc, npc.Inventory.GetItem(stream.ReadByte()));
                    break;
                case RequestMessageIDs.UnequipItem:
                    OnUnequipItem?.Invoke(npc, npc.Inventory.GetItem(stream.ReadByte()));
                    break;

                case RequestMessageIDs.UseItem:
                    OnUseItem?.Invoke(npc, npc.Inventory.GetItem(stream.ReadByte()));
                    break;

                case RequestMessageIDs.Aim:
                    OnAim?.Invoke(npc);
                    break;
                case RequestMessageIDs.Unaim:
                    OnUnaim?.Invoke(npc);
                    break;
                case RequestMessageIDs.Shoot:
                    OnShoot?.Invoke(npc, stream.ReadVec3f(), stream.ReadVec3f());
                    break;

                default:
                    Log.Logger.Log("Received Script RequestMessage with invalid ID: " + id.ToString());
                    break;
            }
        }
    }
}
