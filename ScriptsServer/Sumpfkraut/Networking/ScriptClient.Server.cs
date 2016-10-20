using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using static GUC.Scripts.Sumpfkraut.VobSystem.Instances.NPCInst;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        public bool IsAllowedToConnect()
        {
            return true;
        }

        partial void pOnConnect()
        {
            this.SetToSpectator(WorldInst.Current, new Vec3f(), new Vec3f(0, 0, 1));
        }

        public static int GetCount()
        {
            return GameClient.Count;
        }

        public virtual void ReadScriptMessage(PacketReader stream)
        {
        }

        public virtual void ReadScriptRequestMessage(PacketReader stream, WorldObjects.VobGuiding.GuidedVob vob)
        {
            if (!(vob is WorldObjects.NPC))
                return;

            NPCInst npc = (NPCInst)vob.ScriptObject;

            ScriptRequestMessageIDs id = (ScriptRequestMessageIDs)stream.ReadByte();
            switch (id)
            {
                case ScriptRequestMessageIDs.JumpFwd:
                    npc.DoJump();
                    break;
                case ScriptRequestMessageIDs.DrawFists:
                    npc.DrawFists();
                    break;
                case ScriptRequestMessageIDs.DrawWeapon:
                    npc.DrawWeapon(stream.ReadByte());
                    break;
                case ScriptRequestMessageIDs.AttackForward:
                    npc.DoFightMove(NPCInst.FightMoves.Fwd);
                    break;
                case ScriptRequestMessageIDs.AttackLeft:
                    npc.DoFightMove(NPCInst.FightMoves.Left);
                    break;
                    case ScriptRequestMessageIDs.AttackRight:
                    npc.DoFightMove(NPCInst.FightMoves.Right);
                    break;
                case ScriptRequestMessageIDs.Parry:
                    npc.DoFightMove(NPCInst.FightMoves.Parry);
                    break;
                case ScriptRequestMessageIDs.Dodge:
                    npc.DoFightMove(NPCInst.FightMoves.Dodge);
                    break;
                case ScriptRequestMessageIDs.DropItem:
                    byte itemID = stream.ReadByte();
                    ushort amount = stream.ReadUShort();
                    npc.DropItem(itemID, amount);
                    break;
                case ScriptRequestMessageIDs.EquipItem:
                    npc.EquipItem(stream.ReadByte());
                    break;
                case ScriptRequestMessageIDs.UnequipItem:
                    npc.UnequipItem(stream.ReadByte());
                    break;
                case ScriptRequestMessageIDs.UseItem:
                    npc.UseItem(stream.ReadByte());
                    break;
            }
        }
    }
}
