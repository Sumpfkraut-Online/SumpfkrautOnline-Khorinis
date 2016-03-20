using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.Scripting
{
    public class GUCTimer
    {
        static DynamicCollection<GUCTimer> timerList = new DynamicCollection<GUCTimer>();

        internal static void Update(long now)
        {
            timerList.ForEach(timer =>
            {
                if (timer.nextCallTime <= now)
                {
                    timer.callback();
                    if (timer.started) // still running?
                    {
                        timer.SetNextCallTime(now);
                    }
                }
            });
        }

        #region Properties

        int id;

        long interval;
        /// <summary>
        /// The interval in ticks in which the callback should be called. Setting this while the timer is running restarts it with the new value.
        /// </summary>
        public long Interval { get { return interval; } }

        long nextCallTime;
        /// <summary>
        /// DateTime.UtcNow.Ticks + Interval from start/last fire
        /// </summary>
        public long NextCallTime { get { return nextCallTime; } }

        bool started = false;
        /// <summary>
        /// Returns whether the timer is running.
        /// </summary>
        public bool Started { get { return started; } }
        
        Action callback;

        #endregion

        #region Set Methods

        public void SetInterval(long interval)
        {
            if (interval < 0)
                throw new ArgumentOutOfRangeException("Interval is < 0!");

            this.interval = interval;
        }

        public void SetCallback(Action callback)
        {
            if (callback == null)
                throw new ArgumentNullException("Callback is null!");

            this.callback = callback;
        }

        void SetNextCallTime(long now)
        {
            nextCallTime = now + interval;
        }

        #endregion

        #region Constructors

        public GUCTimer()
        {
        }

        public GUCTimer(long interval, Action callback)
        {
            SetInterval(interval);
            SetCallback(callback);
        }

        public GUCTimer(long interval)
        {
            SetInterval(interval);
        }

        public GUCTimer(Action callback)
        {
            SetCallback(callback);
        }

        #endregion

        #region Start & Stop

        public void Start()
        {
            if (!started)
            {
                if (this.callback == null)
                    throw new NullReferenceException("Callback is null!");

                started = true;
                timerList.Add(this, ref this.id);
                SetNextCallTime(DateTime.UtcNow.Ticks);
            }
        }

        public void Stop(bool fire = false)
        {
            if (started)
            {
                started = false;
                timerList.Remove(ref this.id);
                if (fire)
                {
                    callback();
                }
            }
        }

        #endregion
    }
}
