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
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ExtendedObject, GameClient.IScriptClient
    {
        #region Constructors

        public ScriptClient()
        {
            this.baseClient = new GameClient(this);
        }

        #endregion

        #region Properties

        public int ID { get { return this.baseClient.ID; } }

        GameClient baseClient;
        public GameClient BaseClient { get { return this.baseClient; } }

        public NPCInst Character { get { return (NPCInst)this.baseClient.Character?.ScriptObject; } }

        #endregion

        #region Connection

        partial void pOnConnect();
        public virtual void OnConnection()
        {
            pOnConnect();
        }

        public virtual void OnDisconnection()
        {

        }

        #endregion

        #region NPC Control

        public void SetControl(NPC npc)
        {
            this.SetControl((NPCInst)npc.ScriptObject);
        }

        public virtual void SetControl(NPCInst npc)
        {
            this.baseClient.SetControl(npc.BaseInst);
        }

        #endregion

        #region Spectator

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
