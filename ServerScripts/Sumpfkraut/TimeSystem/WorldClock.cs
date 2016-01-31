using GUC.Server.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Server.Scripts.Sumpfkraut.TimeSystem
{
    public class WorldClock : GUC.Utilities.Threading.AbstractRunnable
    {

        new public static readonly String _staticName = "WorldClock (static)";

        protected List<World> affectedWorlds;

        public static readonly TimeSpan defaultTickTimeout = new TimeSpan(0, 0, 10);
        protected TimeSpan tickTimeout;
        public TimeSpan GetTickTimeout () { return this.timeout; }
        public void SetTickTimeout (TimeSpan tickTimeout)
        {
            this.timeout = tickTimeout;
        }

        // how fast goes ingametime goes by relative to realtime
        // note that the actual IGTime does not allow for high precision,
        // although the DateTime- and rate-calculations are performed with doubles
        protected double igTimeRate;
        public double GetIgTimeRate () { return this.igTimeRate; }
        public void SetIgTimeRate (double igTimeRate) { this.igTimeRate = igTimeRate; }

        protected DateTime lastTimeUpdate;
        public DateTime GetLastTimeUpdate () { return this.lastTimeUpdate; }

        protected IgTime igTime;
        public IgTime GetIgTime () { return this.igTime; }
        public void SetIgTime (IgTime igTime)
        {
            this.igTime = igTime;
            UpdateWorldTime();
        }

        protected object igTimeLock = new object();

        public delegate void OnTimeChangeEventHandler (IgTime igTime);
        public event OnTimeChangeEventHandler OnTimeChange; 



        public WorldClock (List<World> affectedWorlds, IgTime startIGTime, 
            double igTimeRate, bool startOnCreate, TimeSpan tickTimeout)
            : base (false, tickTimeout, false)
        {
            SetObjName("WorldClock (default)");
            this.affectedWorlds = affectedWorlds;
            this.igTime = startIGTime;
            this.igTimeRate = igTimeRate;

            if (startOnCreate)
            {
                Start();
            }
        }



        public override void Start ()
        {
            lastTimeUpdate = DateTime.Now;
            base.Start();
        }



        public void UpdateWorldTime ()
        {
            if (affectedWorlds == null)
            {
                return;
            }

            DateTime now = DateTime.Now;
            double rlDiff, igDiff;
            long newTotalMinutes;
            IgTime newIgTime;

            lock (igTimeLock)
            {
                try
                {
                    rlDiff = (now - lastTimeUpdate).TotalMinutes;
                    igDiff = rlDiff * igTimeRate;
                    newTotalMinutes = IgTime.ToMinutes(igTime) + (long) igDiff;
                    newIgTime = new IgTime(newTotalMinutes);
                }
                catch (Exception ex)
                {
                    // forcefully reset to day 0 to reduce number sizes in following calculations
                    newIgTime = igTime;
                    newIgTime.day = 0;
                    rlDiff = (now - lastTimeUpdate).TotalMinutes;
                    igDiff = rlDiff * igTimeRate;
                    newTotalMinutes = IgTime.ToMinutes(newIgTime) + (long) igDiff;
                    MakeLogError("Forcefully reset igTime to 0 days, preserving the daytime"
                        + " due to unhandleble calculations: " + ex);
                }

                if ((long) igDiff == 0)
                {
                    // no signifcant difference made
                    return;
                }

                igTime = new IgTime(newTotalMinutes);
                lastTimeUpdate = now;

                if (OnTimeChange != null)
                {
                    OnTimeChange.Invoke(igTime);
                }
            
                for (int w = 0; w < affectedWorlds.Count; w++)
                {
                    affectedWorlds[w].ChangeIgTime(newIgTime, World.NewWorld.GetIgTimeRate());
                }
            }
        }



        public override void Run ()
        {
            base.Run();

            UpdateWorldTime();
        }

    }
}
