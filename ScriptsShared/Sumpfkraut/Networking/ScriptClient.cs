using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        #region Properties

        public int ID { get { return this.baseClient.ID; } }

        GameClient baseClient;
        public GameClient BaseClient { get { return this.baseClient; } }

        public NPCInst Character { get { return (NPCInst)this.baseClient.Character?.ScriptObject; } }
        
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

        public void SetControl(NPC npc)
        {
            this.SetControl((NPCInst)npc.ScriptObject);
        }

        public void SetControl(NPCInst npc)
        {
            this.baseClient.SetControl(npc.BaseInst);
        }

        public void SetToSpectator(World world, Vec3f pos, Vec3f dir)
        {
            this.SetToSpectator((WorldInst)world.ScriptObject, pos, dir);
        }

        partial void pSetToSpectator(WorldInst world, Vec3f pos, Vec3f dir);
        public void SetToSpectator(WorldInst world, Vec3f pos, Vec3f dir)
        {
            this.baseClient.SetToSpectate(world.BaseWorld, pos, dir);
            pSetToSpectator(world, pos, dir);
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
