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
            ScriptRequestMessageIDs id = (ScriptRequestMessageIDs)stream.ReadByte();

            if (id > ScriptRequestMessageIDs.MaxGuidedMessages && vob != this.Character.BaseInst)
            {
                return; // client sent a request for a bot which is only allowed for players
            }

            if (id < ScriptRequestMessageIDs.MaxNPCRequests)
            {
                if (vob is WorldObjects.NPC)
                    NPCInst.Requests.ReadRequest(id, stream, ((NPCInst)vob.ScriptObject));
            }
        }
    }
}
