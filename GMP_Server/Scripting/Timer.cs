using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting
{
    public class Timer
    {
        internal static List<Timer> TimerList = new List<Timer>();
        internal static void Update(long now)
        {
            Timer timer;
            for (int i = TimerList.Count - 1; i >= 0; i--)
            {
                timer = TimerList[i];
                if (timer.CallTime <= now)
                {
                    timer.Callback(timer.Argument);
                    timer.Stop();
                }
            }
        }

        public long CallTime;

        public Action<object> Callback;
        public object Argument;

        bool started;
        public bool Started { get { return started; } }

        public Timer(Action<object> callback, object argument)
        {
            started = false;
            CallTime = 0;
            Callback = callback;
            Argument = argument;
        }

        public void Start()
        {
            if (!started)
            {
                started = true;
                TimerList.Add(this);
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
    }
}
