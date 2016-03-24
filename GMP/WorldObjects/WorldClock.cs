using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Types;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{

    public class WorldClock : GUC.Utilities.Threading.AbstractRunnable
    {

        new public static readonly String _staticName = "WorldClock (static)";

        protected IgTime igTime;
        public IgTime GetIgTime () { return this.igTime; }
        public void SetIgTime (IgTime igTime)
        {
            this.igTime = igTime;
            originIgTime = igTime;
            this.lastCheckup = DateTime.Now;
            MakeLog("Set igTime to: " + this.igTime);
            ApplyIgTime();
        }

        // rate at which the ig-time clocks relative to realtime 
        // [ig-minutes / rl-minutes]
        protected float igTimeRate;
        public float GetIgTimeRate () { return this.igTimeRate; }
        public void SetIgTimeRate (float igTimeRate)
        {
            this.igTimeRate = igTimeRate;
            MakeLog("Set igTimeRate to: " + igTimeRate);
        }

        // ig time from where tick differences are applied to result in the
        // current igTime
        protected IgTime originIgTime;
        // last time the originIgTime was updated
        protected DateTime lastCheckup;
        object igTimeLock;

        public delegate void OnIgTimeChangeHandler (IgTime igTime);
        public event OnIgTimeChangeHandler OnIgTimeChange;



        public WorldClock ()
            : this (new IgTime(1, 9, 0), 4f)
        { }

        public WorldClock (IgTime igTime, float igTimeRate)
            : this (true, new TimeSpan(0, 0, 10), igTime, igTimeRate)
        { }

        protected WorldClock (bool startOnCreate, TimeSpan timeout, IgTime igTime, float igTimeRate)
            : base(false, timeout, false)
        {
            SetObjName("WorldClock (default)");
            printStateControls = true;
            lastCheckup = DateTime.Now;
            //originIgTime = igTime;
            igTimeLock = new object();
            SetIgTime(igTime);
            SetIgTimeRate(igTimeRate);
            
            if (startOnCreate)
            {
                Start();
            }
        }



        public void ApplyIgTime ()
        {
            if (!(Program._state is GUC.Client.States.GameState))
            {
                return;
            }
            lock (igTimeLock)
            {
                oCGame.Game(Program.Process).WorldTimer.SetTime(igTime.hour, igTime.minute);
                oCGame.Game(Program.Process).WorldTimer.SetDay(igTime.day);
            }

            MakeLog("Applied ingame-time: " + igTime);
            OnIgTimeChange.Invoke(igTime);
        }

        public void TickIgTime ()
        {
            TimeSpan tickDiffTS = DateTime.Now - this.lastCheckup;
            double tickDiff = tickDiffTS.TotalMinutes;
            long minuteJump = (long) Math.Round(tickDiff * igTimeRate);
            IgTime newIgTime = new IgTime(IgTime.ToMinutes(igTime) + minuteJump);

            if (newIgTime != igTime)
            {
                SetIgTime(newIgTime);
                //ApplyIgTime();
            }
        }



        public override void Run ()
        {
            base.Run();

            TickIgTime();
        }

    }

}
