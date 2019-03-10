using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ExtendedObject, GameClient.IScriptClient
    {
        public bool IsConnected { get { return this.baseClient.IsConnected; } }

        public bool IsAllowedToConnect()
        {
            return true;
        }

        public static void ForEach(Action<ScriptClient> action)
        {
            GameClient.ForEach(client => action((ScriptClient)client.ScriptObject));
        }

        public static int GetCount()
        {
            return GameClient.Count;
        }

        partial void pBeforeSetControl(NPCInst npc)
        {
            // old npc
            if (npc.IsSpawned && npc.IsDead)
                npc.World.DespawnList_NPC.AddVob(npc);
        }

        partial void pAfterSetControl(NPCInst npc)
        {
            // new npc
            if (npc.IsSpawned && npc.IsDead)
                npc.World.DespawnList_NPC.RemoveVob(npc);
        }

        public virtual void ReadScriptMessage(PacketReader stream)
        {
        }

        public virtual void ReadScriptRequestMessage(PacketReader stream, WorldObjects.VobGuiding.GuidedVob vob)
        {
            RequestMessageIDs id = (RequestMessageIDs)stream.ReadByte();

            if (id > RequestMessageIDs.MaxGuidedMessages && vob != this.Character.BaseInst)
            {
                return; // client sent a request for a bot which is only allowed for players
            }

            if (id < RequestMessageIDs.MaxNPCRequests)
            {
                if (vob is WorldObjects.NPC)
                    NPCInst.Requests.ReadRequest(id, stream, ((NPCInst)vob.ScriptObject));
            }
        }

        public static PacketWriter GetScriptMessageStream()
        {
            return GameClient.GetScriptMessageStream();
        }

        public void SendScriptMessage(PacketWriter stream, NetPriority priority, NetReliability reliability)
        {
            this.BaseClient.SendScriptMessage(stream, priority, reliability);
        }

        /// <summary> Only use if you know what you're doing. </summary>
        public void SendScriptMessage(byte[] data, int length, NetPriority priority, NetReliability reliability)
        {
            this.BaseClient.SendScriptMessage(data, length, priority, reliability);
        }
    }
}
