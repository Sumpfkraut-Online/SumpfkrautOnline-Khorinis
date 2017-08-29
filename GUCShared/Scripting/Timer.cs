using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;
using GUC.GameObjects.Collections;

namespace GUC.Scripting
{
    public abstract class GUCAbstractTimer
    {
        #region Static Loop
        // Sorted timers (Latest ... Next)
        static List<GUCAbstractTimer> activeTimers = new List<GUCAbstractTimer>(100);
        internal static void Update(long now)
        {
            int index;
            while ((index = activeTimers.Count-1) >= 0)
            {
                var current = activeTimers[index];
                if (current.NextCallTime > now)
                    break;

                activeTimers.RemoveAt(index);
                current.Fire();
                if (current.Started)
                    current.SetNextCallTime(now);
                
            }
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
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Callback is null!");
                this.callback = value;
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

                this.SetNextCallTime(GameTime.Ticks);
                this.started = true;
            }
        }

        public void Stop(bool fire = false)
        {
            if (started)
            {
                activeTimers.Remove(this);
                started = false;

                if (fire)
                    this.Fire();
            }
        }

        public void Restart(bool fire = false)
        {
            Stop(fire); Start();
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
            if (this.started)
            {
                this.interval = interval;
                activeTimers.Remove(this);
                SetNextCallTime(this.startTime);
            }
        }

        void SetNextCallTime(long now)
        {
            this.startTime = now;
            this.nextCallTime = now + this.interval;
            for (int i = 0; i < activeTimers.Count; i++)
                if (activeTimers[i].nextCallTime <= this.nextCallTime)
                    activeTimers.Insert(i, this);
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
            ((Action<T1,T2>)this.Callback).Invoke(arg1, arg2);
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

        public GUCTimer(Action<T1,T2> callback, T1 argument1, T2 argument2)
        {
            SetCallback(callback, argument1, argument2);
        }

        #endregion
    }
}
