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
        public void SetIgTimeRate (double igTimeRate)
        {
            this.igTimeRate = igTimeRate;
            // forcefully update the clients to keep ticking igTime in sync
            UpdateWorldTime(true);
        }

        protected IgTime igTime;
        public IgTime GetIgTime () { return this.igTime; }
        public void SetIgTime (IgTime igTime)
        {
            this.igTime = igTime;
            // forcefully update the clients to keep ticking igTime in sync
            UpdateWorldTime(true);
        }

        protected DateTime lastTimeUpdate;
        public DateTime GetLastTimeUpdate () { return this.lastTimeUpdate; }

        protected object igTimeLock = new object();

        public delegate void OnTimeChangeEventHandler (IgTime igTime);
        public event OnTimeChangeEventHandler OnIgTimeChange;

        protected TimeSpan clientUpdateInterval;
        public TimeSpan GetClientUpdateInterval () { return this.clientUpdateInterval; }
        public void SetClientUpdateInterval (TimeSpan interval) { this.clientUpdateInterval = interval; }

        protected DateTime lastClientUpdate;
        public DateTime GetLastClientUpdate () { return this.lastClientUpdate; }
        protected void SetLastClientUpdate (DateTime lastClientUpdate)
        {
            this.lastClientUpdate = lastClientUpdate;
        }

        public delegate void OnClientUpdateEventHandler (IgTime igTime, double igTimeRate);
        public event OnClientUpdateEventHandler OnClientUpdate;



        public WorldClock (List<World> affectedWorlds, IgTime startIGTime, 
            double igTimeRate, TimeSpan clientUpdateInterval, bool startOnCreate, 
            TimeSpan tickTimeout)
            : base (false, tickTimeout, false)
        {
            SetObjName("WorldClock (default)");
            this.affectedWorlds = affectedWorlds;
            this.igTime = startIGTime;
            this.igTimeRate = igTimeRate;
            SetClientUpdateInterval(clientUpdateInterval);

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
            UpdateWorldTime(false);
        }

        public void UpdateWorldTime (bool forceUpdate)
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
                        + " due to unhandleable calculations: " + ex);
                }

                if (((long) igDiff == 0L) && !forceUpdate)
                {
                    // no signifcant difference made
                    return;
                }

                igTime = new IgTime(newTotalMinutes);
                lastTimeUpdate = now;

                if (OnIgTimeChange != null)
                {
                    OnIgTimeChange.Invoke(igTime);
                }

                if ((!((now - GetLastClientUpdate()) >= GetClientUpdateInterval()))
                    && !forceUpdate)
                {
                    // not need to update all clients igTime-information for now
                    return;
                }

                SetLastClientUpdate(now);

                for (int w = 0; w < affectedWorlds.Count; w++)
                {
                    affectedWorlds[w].ChangeIgTime(newIgTime, (float) igTimeRate);
                }

                if (OnClientUpdate != null)
                {
                    OnClientUpdate.Invoke(igTime, igTimeRate);
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
