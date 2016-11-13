using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class WorldInst : ScriptObject, WorldObjects.World.IScriptWorld
    {
        WorldObjects.World baseWorld;
        public WorldObjects.World BaseWorld { get { return baseWorld; } }

        WorldDef definition;
        public WorldDef Definition
        {
            get { return this.definition; }
            set
            {
                if (this.IsCreated)
                    throw new ArgumentNullException("Can't change the definition when the object is already added to the static collection!");
                this.definition = value;
            }
        }

        public bool IsCreated { get { return this.baseWorld.IsCreated; } }
        public ScriptClock Clock { get { return (ScriptClock)this.baseWorld.Clock.ScriptObject; } }
        public ScriptWeatherCtrl Weather { get { return (ScriptWeatherCtrl)this.baseWorld.WeatherCtrl.ScriptObject; } }
        public ScriptBarrierCtrl Barrier { get { return (ScriptBarrierCtrl)this.baseWorld.BarrierCtrl.ScriptObject; } }

        public WorldInst()
        {
            this.baseWorld = new WorldObjects.World(new ScriptClock(this), new ScriptWeatherCtrl(this), new ScriptBarrierCtrl(this), this);
        }

        public void OnWriteProperties(PacketWriter stream)
        {
            // write definition id
        }

        public void OnReadProperties(PacketReader stream)
        {
            // read definition id
        }

        partial void pCreate();
        public void Create()
        {
            this.baseWorld.Create();
            pCreate();
        }

        partial void pDelete();
        public void Delete()
        {
            this.baseWorld.Delete();
            pDelete();
        }
    }
}