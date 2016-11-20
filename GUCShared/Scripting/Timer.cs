using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using GUC.GameObjects.Collections;

namespace GUC.Scripting
{
    public class GUCTimer
    {
        #region Static Loop

        static GUCTimer[] activeTimers = new GUCTimer[100];
        static int lastActiveTimer = -1; // index of the last add active timer

        internal static void Update(long now)
        {
            int i = 0;
            while (i <= lastActiveTimer)
            {
                GUCTimer current = activeTimers[i];
                if (current.nextCallTime <= now)
                {
                    current.callback();
                    if (current.started)
                    {
                        current.SetNextCallTime(now);
                    }
                    else
                    {
                        continue; // timer has stopped and was replaced by the lastActiveTimer
                    }
                }
                i++;
            }
        }

        #endregion

        #region Start & Stop

        public void Start()
        {
            if (!started)
            {
                if (this.callback == null)
                    throw new NullReferenceException("Callback is null!");

                this.started = true;
                this.index = ++lastActiveTimer;

                if (index >= activeTimers.Length)
                {
                    GUCTimer[] newArr = new GUCTimer[2 * activeTimers.Length];
                    Array.Copy(activeTimers, newArr, activeTimers.Length);
                    activeTimers = newArr;
                }

                activeTimers[this.index] = this;
                this.SetNextCallTime(GameTime.Ticks);
            }
        }

        public void Stop(bool fire = false)
        {
            if (started)
            {
                started = false;

                activeTimers[this.index] = this.index != lastActiveTimer ? activeTimers[lastActiveTimer] : null;
                lastActiveTimer--;
                
                if (fire)
                {
                    callback();
                }
            }
        }

        public void Restart(bool fire = false)
        {
            if (started)
            {
                if (fire) callback();
                SetNextCallTime(GameTime.Ticks);
            }
            else
            {
                this.Start();
            }
        }

        #endregion

        #region Properties
        
        int index;

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

        public long GetRemainingTicks() { return nextCallTime - GameTime.Ticks; }

        long startTime = 0;
        /// <summary>
        /// DateTime.UtcNow.Ticks from the timer's last call.
        /// </summary>
        public long StartTime { get { return this.startTime; } }

        public long GetElapsedTicks() { return GameTime.Ticks - startTime; }

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
            startTime = now;
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
    }
}
