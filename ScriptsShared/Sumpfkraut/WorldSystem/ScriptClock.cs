using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects.Time;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public class ScriptClock : WorldClock.IScriptWorldClock
    {
        WorldClock baseClock;
        public WorldClock BaseClock { get { return this.baseClock; } }

        public WorldTime Time { get { return this.baseClock.Time; } }
        public float Rate { get { return this.baseClock.Rate; } }

        public WorldInst World { get { return (WorldInst)this.baseClock.World.ScriptObject; } }

        // SetTime etc

        public ScriptClock(WorldInst world)
        {
            this.baseClock = world.BaseWorld.Clock;
            this.baseClock.ScriptObject = this;
        }

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        public void SetTime(WorldTime time, float rate)
        {
            this.baseClock.SetTime(time, rate);
        }

        public void Start()
        {
            this.baseClock.Start();
        }

        public void Stop()
        {
            this.baseClock.Stop();
        }
    }
}
