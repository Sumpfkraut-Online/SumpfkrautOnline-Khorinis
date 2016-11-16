using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects.WorldGlobals;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class ScriptClock : WorldClock.IScriptWorldClock
    {
        #region Constructors

        partial void pConstruct();
        public ScriptClock(WorldInst world)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");
            this.world = world;
            pConstruct();
        }

        #endregion

        #region Properties

        WorldInst world;
        public WorldInst World { get { return this.world; } }
        public WorldClock BaseClock { get { return this.world.BaseWorld.Clock; } }

        public WorldTime Time { get { return BaseClock.Time; } }
        public float Rate { get { return BaseClock.Rate; } }

        #endregion

        public void SetTime(WorldTime time, float rate)
        {
            BaseClock.SetTime(time, rate);
        }

        public void Start()
        {
            BaseClock.Start();
        }

        public void Stop()
        {
            BaseClock.Stop();
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
