using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.Scripting
{
    public abstract class GUCAbstractTimer
    {
        #region Static Loop
        // Sorted timers (0 [Last] ... Count-1 [Next])
        static List<GUCAbstractTimer> activeTimers = new List<GUCAbstractTimer>(100);
        static GUCAbstractTimer currentTimer;
        internal static void Update(long now)
        {
            int index;
            while ((index = activeTimers.Count - 1) >= 0)
            {
                GUCAbstractTimer timer = activeTimers[index];
                if (timer.NextCallTime > now)
                    break;

                activeTimers.RemoveAt(index);

                currentTimer = timer; // in case of Fire() changing the interval so SetNextCallTime is not called twice
                timer.Fire();

                if (timer.Started) // still running?
                    timer.SetNextCallTime(now);
            }
            currentTimer = null;
        }

        // search by cutting the list repeatedly in half
        static int FindIndex(long time)
        {
            int upper = activeTimers.Count;
            int lower = -1;

            int index = upper / 2;
            while (true)
            {
                if (index == upper)
                    break;
                if (index == lower)
                {
                    index++;
                    break;
                }

                long otherTime = activeTimers[index].NextCallTime;
                if (otherTime > time)
                {
                    lower = index;
                    // index += (upper - index) / 2;
                    index += (upper - index + 1) / 2; // round up
                }
                else if (otherTime < time)
                {
                    upper = index;
                    index -= (index - lower + 1) / 2; // round up
                }
                else break;
            }
            return index;
        }

        static void AddTimer(GUCAbstractTimer timer)
        {
            activeTimers.Insert(FindIndex(timer.NextCallTime), timer);
        }

        static bool RemoveTimer(GUCAbstractTimer timer)
        {                
            //return activeTimers.Remove(timer);
            int index = FindIndex(timer.NextCallTime);
            int i = index;

            if (index >= activeTimers.Count)
                return false;

            while (activeTimers[i] != timer)
            {
                i++;
                if (i >= activeTimers.Count || activeTimers[i].NextCallTime != timer.NextCallTime)
                {
                    i = index;
                    do
                    {
                        i--;
                        if (i < 0 || activeTimers[i].NextCallTime != timer.NextCallTime)
                            return false; // has not been in the list

                    } while (activeTimers[i] != timer);
                    break;
                }
            }

            activeTimers.RemoveAt(index);            
            return true;
        }

        #endregion

        protected abstract void Fire();

        #region Properties

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

        object callback;
        protected object Callback
        {
            get { return this.callback; }
            set { this.callback = value ?? throw new ArgumentNullException("Callback is null!"); }
        }

        #endregion

        #region Start & Stop

        public void Start()
        {
            if (!started)
            {
                if (this.callback == null)
                    throw new NullReferenceException("Callback is null!");

                // don't insert timer twice during timed Fire event
                if (currentTimer != this)
                    this.SetNextCallTime(GameTime.Ticks);

                this.started = true;
            }
        }

        public void Stop(bool fire = false)
        {
            if (started)
            {
                started = false;

                // because it's already taken out before the timed Fire event
                if (currentTimer != this)
                    RemoveTimer(this);

                if (fire)
                    this.Fire();
            }
        }

        public void Restart(bool fire = false)
        {
            Stop(fire);
            Start();
        }

        #endregion

        #region Set Methods

        public void SetInterval(long interval)
        {
            if (interval <= 0)
                throw new ArgumentOutOfRangeException("Interval is <= 0!");

            if (this.interval == interval)
                return;

            // update
            this.interval = interval;
            if (this.started && currentTimer != this) // don't double the calculation
            {
                RemoveTimer(this);
                SetNextCallTime(this.startTime);
            }
        }

        void SetNextCallTime(long now)
        {
            this.startTime = now;
            this.nextCallTime = now + this.interval;
            AddTimer(this);
        }

        #endregion
    }

    public class GUCTimer : GUCAbstractTimer
    {
        protected override void Fire()
        {
            ((Action)this.Callback).Invoke();
        }

        public void SetCallback(Action callback)
        {
            this.Callback = callback;
        }

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

    public class GUCTimer<T> : GUCAbstractTimer
    {
        T arg;

        protected override void Fire()
        {
            ((Action<T>)this.Callback).Invoke(arg);
        }

        public void SetCallback(Action<T> callback, T argument)
        {
            this.Callback = callback;
            this.arg = argument;
        }

        #region Constructors

        public GUCTimer()
        {
        }

        public GUCTimer(long interval, Action<T> callback, T argument)
        {
            SetInterval(interval);
            SetCallback(callback, argument);
        }

        public GUCTimer(long interval)
        {
            SetInterval(interval);
        }

        public GUCTimer(Action<T> callback, T argument)
        {
            SetCallback(callback, argument);
        }

        #endregion
    }

    public class GUCTimer<T1, T2> : GUCAbstractTimer
    {
        T1 arg1;
        T2 arg2;

        protected override void Fire()
        {
            ((Action<T1, T2>)this.Callback).Invoke(arg1, arg2);
        }

        public void SetCallback(Action<T1, T2> callback, T1 argument1, T2 argument2)
        {
            this.Callback = callback;
            this.arg1 = argument1;
            this.arg2 = argument2;
        }

        #region Constructors

        public GUCTimer()
        {
        }

        public GUCTimer(long interval, Action<T1, T2> callback, T1 argument1, T2 argument2)
        {
            SetInterval(interval);
            SetCallback(callback, argument1, argument2);
        }

        public GUCTimer(long interval)
        {
            SetInterval(interval);
        }

        public GUCTimer(Action<T1, T2> callback, T1 argument1, T2 argument2)
        {
            SetCallback(callback, argument1, argument2);
        }

        #endregion
    }
}
