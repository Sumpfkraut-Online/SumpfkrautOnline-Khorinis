using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        #region Properties

        GameClient baseClient;
        public GameClient BaseClient { get { return this.baseClient; } }

        public NPCInst Character { get { return (NPCInst)this.baseClient.Character.ScriptObject; } }
        
        #region dank rank

        public enum ClientRank
        {
            Statist,
            Supporter,
            Admin
        }

        public ClientRank Rank = ClientRank.Statist;

        #endregion

        #endregion

        #region Constructors

        public ScriptClient(GameClient baseClient)
        {
            if (baseClient == null)
                throw new ArgumentNullException("BaseClient is null!");

            this.baseClient = baseClient;
            this.baseClient.ScriptObject = this;
        }

        #endregion

        public void OnDisconnection()
        {

        }

        public void SetControl(WorldObjects.NPC npc)
        {
            this.SetControl((NPCInst)npc.ScriptObject);
        }

        public void SetControl(NPCInst npc)
        {
            this.baseClient.SetControl(npc.BaseInst);
        }

        #region Read & Write

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        #endregion
    }
}
