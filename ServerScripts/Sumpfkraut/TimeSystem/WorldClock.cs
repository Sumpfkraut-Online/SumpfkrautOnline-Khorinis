using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
using GUC.Server.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.TimeSystem
{
    public class WorldClock : Runnable
    {

        new public static readonly String _staticName = "WorldClock (static)";

        protected List<World> affectedWorlds;

        protected TimeSpan tickTimeout;
        public TimeSpan GetTickTimeout () { return this.timeout; }
        public void SetTickTimeout (TimeSpan tickTimeout)
        {
            this.timeout = tickTimeout;
        }

        // how fast goes ingametime goes by relative to realtime
        protected float igTimeRate;
        protected DateTime lastTimeUpdate;

        protected IGTime igTime;
        public IGTime GetIgTime () { return this.igTime; }
        public void SetIgTime (IGTime igTime)
        {
            this.igTime = igTime;
            UpdateWorldTime();
        }



        public WorldClock (List<World> affectedWorlds)
            : base(true, new TimeSpan(0, 0, 10), false)
        {
            this._objName = "WorldClock (default)";
        }



        //public 

        public void UpdateWorldTime ()
        {
            if (affectedWorlds == null)
            {
                return;
            }

            //lastTimeUpdate

            for (int w = 0; w < affectedWorlds.Count; w++)
            {
                
                //affectedWorlds[w].ChangeTime();
            }
        }



        public override void Run ()
        {
            base.Run();

            UpdateWorldTime();
        }

    }
}
