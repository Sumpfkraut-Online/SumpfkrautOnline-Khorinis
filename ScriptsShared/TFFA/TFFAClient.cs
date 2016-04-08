using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.TFFA
{
    public partial class TFFAClient : GameClient.IScriptClient
    {
        public PlayerClass Class = PlayerClass.None;
        public Team Team = Team.Spec;
        public int Kills = 0;
        public int Deaths = 0;

        GameClient baseClient;
        public GameClient BaseClient { get { return this.baseClient; } }

        public NPCInst Character { get { return this.baseClient.Character == null ? null : (NPCInst)this.baseClient.Character.ScriptObject; } }

        partial void pConstruct();
        public TFFAClient(GameClient client)
        {
            this.baseClient = client;
            client.ScriptObject = this;
            pConstruct();
        }

        public void SetControl(NPC npc)
        {
            this.SetControl((NPCInst)npc.ScriptObject);
        }

        public void SetControl(NPCInst npc)
        {
            this.baseClient.SetControl(npc.BaseInst);
        }

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }
    }
}
