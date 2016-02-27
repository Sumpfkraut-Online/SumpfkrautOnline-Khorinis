using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        #region Properties

        GameClient baseClient;
        public GameClient BaseClient { get { return this.baseClient; } }
        
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
