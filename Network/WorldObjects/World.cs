using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using GUC.Log;
using GUC.WorldObjects.Instances;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class World : WorldObject
    {
        #region ScriptObject

        public partial interface IScriptWorld : IScriptWorldObject
        {
        }

        public new IScriptWorld ScriptObject
        {
            get { return (IScriptWorld)base.ScriptObject; }
        }

        #endregion

        #region Properties

        new public WorldInstance Instance { get { return (WorldInstance)Instance; } }

        public readonly VobCollection Vobs = new VobCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new World with the given ID or [-1] a free ID.
        /// </summary>
        public World(IScriptWorld scriptObject, WorldInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new World by reading a networking stream.
        /// </summary>
        public World(IScriptWorld scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
        }

        #endregion

        #region Spawn

        partial void pSpawnVob(BaseVob vob);
        public void SpawnVob(BaseVob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("World.SpawnVob: Vob is null!");

            if (vob.IsSpawned)
            {
                if (vob.World != this)
                {
                    vob.Despawn();
                }
            }
            else
            {
                vob.World = this;
            }

            pSpawnVob(vob);

            Vobs.Add(vob);
        }

        partial void pDespawnVob(BaseVob vob);
        public void DespawnVob(BaseVob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("World.DespawnVob: Vob is null!");

            if (vob.World != this)
                return;
            
            pDespawnVob(vob);

            vob.World = null;
            Vobs.Remove(vob);
        }

        #endregion
    }
}
