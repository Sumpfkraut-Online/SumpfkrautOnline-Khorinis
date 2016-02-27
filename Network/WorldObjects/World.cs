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
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        public readonly VobCollection Vobs = new VobCollection();
        
        public bool Added { get; internal set; }

        #endregion
        
        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write((byte)ID);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadByte();
        }

        #endregion

        #region Spawn

        partial void pSpawnVob(BaseVob vob);
        internal void AddVob(BaseVob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("World.SpawnVob: Vob is null!");

            if (vob.IsStatic)
            {
                if (vob.World == null)
                    vob.World = this;
            }
            else
            {
                if (vob.IsSpawned)
                {
                    if (vob.World != this)
                    {
                        vob.Despawn();
                    }
                }
                vob.World = this;
            }

            pSpawnVob(vob);

            Vobs.Add(vob);
        }

        partial void pDespawnVob(BaseVob vob);
        internal void RemoveVob(BaseVob vob)
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
