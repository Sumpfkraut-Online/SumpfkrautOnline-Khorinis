using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

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

            ScriptCommandMessageIDs id = (ScriptCommandMessageIDs)stream.ReadByte();
            switch (id)
            {
                case ScriptCommandMessageIDs.JumpFwd:
                    npc.DoJump();
                    break;
                case ScriptCommandMessageIDs.AttackForward:
                    npc.DoFightMove(NPCInst.FightMoves.Fwd);
                    break;
                case ScriptCommandMessageIDs.DropItem:
                    byte itemID = stream.ReadByte();
                    ushort amount = stream.ReadUShort();
                    npc.DropItem(itemID, amount);
                    break;
            }
        }
    }
}
