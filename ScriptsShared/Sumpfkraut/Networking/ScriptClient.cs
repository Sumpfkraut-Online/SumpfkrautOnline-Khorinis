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

        /// <summary> Spawned or specating in a world. </summary>
        public bool IsIngame { get { return this.baseClient.IsIngame; } }
        public bool IsSpecating { get { return this.baseClient.IsSpectating; } }
        public bool IsCharacter { get { return this.baseClient.Character != null; } }

        #endregion

        #region Connection

        partial void pOnConnect();
        public virtual void OnConnection()
        {
            pOnConnect();
        }

        public virtual void OnDisconnection(int id)
        {
        }

        #endregion

        #region NPC Control

        public void SetControl(NPC npc)
        {
            this.SetControl((NPCInst)npc.ScriptObject);
        }

        partial void pSetControl(NPCInst npc);
        public virtual void SetControl(NPCInst npc)
        {
            this.baseClient.SetControl(npc.BaseInst);
            pSetControl(npc);
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
