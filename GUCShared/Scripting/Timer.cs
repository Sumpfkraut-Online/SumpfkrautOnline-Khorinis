using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.Scripting
{
    public abstract class AbstractTimer
    {
        static List<AbstractTimer> TimerList = new List<AbstractTimer>();

        internal static void Update()
        {
            long now = DateTime.UtcNow.Ticks;

            AbstractTimer timer;
            for (int i = TimerList.Count - 1; i >= 0; i--)
            {
                timer = TimerList[i];
                if (timer.nextCallTime <= now)
                {
                    timer.Fire(now);
                }
            }
        }

        bool started;
        long nextCallTime;
        long interval;

        /// <summary>
        /// The interval in ticks in which the callback should be called. Setting this while the timer is running restarts it with the new value.
        /// </summary>
        public long Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                SetNextCallTime(DateTime.UtcNow.Ticks);
            }
        }

        /// <summary>
        /// DateTime.UtcNow.Ticks + Interval
        /// </summary>
        public long NextCallTime { get { return nextCallTime; } }

        /// <summary>
        /// Returns whether the timer is running.
        /// </summary>
        public bool Started { get { return started; } }

        public AbstractTimer(long interval)
        {
            this.started = false;
            this.interval = interval;
        }

        public void Start()
        {
            if (!started)
            {
                started = true;
                TimerList.Add(this);
                SetNextCallTime(DateTime.UtcNow.Ticks);
            }
        }

        public void Stop()
        {
            if (started)
            {
                started = false;
                TimerList.Remove(this);
            }
        }

        protected abstract void Fire(long now);

        protected void SetNextCallTime(long now)
        {
            nextCallTime = now + interval;
        }
    }

    public class GUCTimer : AbstractTimer
    {
        Action callback;

        public GUCTimer(long interval, Action callback) : base(interval)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("Timer callback is null!");
            }

            this.callback = callback;
        }

        protected override void Fire(long now)
        {
            callback();
            SetNextCallTime(now);
        }
    }

    public class GUCTimer<T> : AbstractTimer
    {
        Action<T> callback;
        T arg1;

        public GUCTimer(long interval, Action<T> callback, T arg1) : base(interval)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("Timer callback is null!");
            }

            this.callback = callback;
            this.arg1 = arg1;
        }

        protected override void Fire(long now)
        {
            callback(arg1);
            SetNextCallTime(now);
        }
    }

    public class GUCTimer<T1,T2> : AbstractTimer
    {
        Action<T1,T2> callback;
        T1 arg1;
        T2 arg2;

        public GUCTimer(long interval, Action<T1,T2> callback, T1 arg1, T2 arg2)
            : base(interval)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("Timer callback is null!");
            }

            this.callback = callback;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        protected override void Fire(long now)
        {
            callback(arg1, arg2);
            SetNextCallTime(now);
        }
    }

    public class GUCTimer<T1, T2, T3> : AbstractTimer
    {
        Action<T1, T2, T3> callback;
        T1 arg1;
        T2 arg2;
        T3 arg3;

        public GUCTimer(long interval, Action<T1, T2, T3> callback, T1 arg1, T2 arg2, T3 arg3)
            : base(interval)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("Timer callback is null!");
            }

            this.callback = callback;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }

        protected override void Fire(long now)
        {
            callback(arg1, arg2, arg3);
            SetNextCallTime(now);
        }
    }
}
