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
    public partial class World : GameObject
    {
        #region ScriptObject

        public partial interface IScriptWorld : IScriptGameObject
        {
        }

        public new IScriptWorld ScriptObject
        {
            get { return (IScriptWorld)base.ScriptObject; }
        }

        #endregion

        #region Properties
        
        public readonly VobCollection Vobs = new VobCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new World with the given ID or [-1] a free ID.
        /// </summary>
        public World(IScriptWorld scriptObject, int id = -1) : base(scriptObject, id)
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
                vob.world = this;
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

            vob.world = null;
            Vobs.Remove(vob);
        }

        #endregion
    }
}
