using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using GUC.Log;

namespace GUC.WorldObjects
{
    public partial class World : WorldObject<World.IScriptWorld>
    {
        public const int MAX_VOBS = 65536; // ushort.MaxValue + 1

        public partial interface IScriptWorld
        {
        }

        public int ID { get; internal set; }

        BaseVob[] vobs = new BaseVob[MAX_VOBS];
        public BaseVob GetVobByID(int id)
        {
            if (id >= 0 && id < MAX_VOBS)
                return vobs[id];
            return null;
        }

        public World(IScriptWorld scriptObject, int id = -1) : base(scriptObject)
        {
        }

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
            
            vobs[vob.WorldID] = vob; 
        }

        partial void pDespawnVob(BaseVob vob);
        public void DespawnVob(BaseVob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("World.DespawnVob: Vob is null!");

            if (vob.World != this)
                return;

            vob.World = null;
        }
    }
}
