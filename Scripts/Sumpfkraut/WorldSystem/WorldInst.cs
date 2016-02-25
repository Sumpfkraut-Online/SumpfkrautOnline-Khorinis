using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class WorldInst : ScriptObject, WorldObjects.World.IScriptWorld
    {
        WorldObjects.World baseWorld;
        public WorldObjects.World BaseWorld { get { return baseWorld; } }

        WorldDef definition;
        public WorldDef Definition { get { return definition; } }

        public WorldInst(PacketReader stream) : this(new WorldObjects.World())
        {
            baseWorld.ReadStream(stream);
        }

        private WorldInst(WorldObjects.World baseWorld)
        {
            if (baseWorld == null)
                throw new ArgumentNullException("BaseWorld is null!");

            this.baseWorld = baseWorld;
        }

        public void SpawnVob(BaseVobInst vob)
        {
            this.BaseWorld.SpawnVob(vob.BaseInst);
        }

        public void OnWriteProperties(PacketWriter stream)
        {
            // write definition id
        }

        public void OnReadProperties(PacketReader stream)
        {
            // read definition id
        }

        public void Create()
        {
            WorldObjects.Collections.WorldCollection.Add(this.BaseWorld);
        }

        public void Delete()
        {
            WorldObjects.Collections.WorldCollection.Remove(this.BaseWorld);
        }
    }
}
