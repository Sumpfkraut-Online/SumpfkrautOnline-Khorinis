using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting
{
    public class Timer
    {
        protected long timeSpan = 0;

        protected bool isStarted = false;
        protected long time = 0;

        public event Events.TimerEvent OnTick;


        public Timer(long timespan)
        {
            this.timeSpan = timespan;
        }

        public long TimeSpan { get { return timeSpan; } set { timeSpan = value; } }

        public virtual void Start()
        {
            if (isStarted)
                return;
            this.time = DateTime.Now.Ticks;
            isStarted = true;

            Program.ScriptManager.TimerList.Add(this);
        }

        public virtual void End()
        {
            if (!isStarted)
                return;

            Program.ScriptManager.TimerList.Remove(this);
            isStarted = false;
        }

        internal void iUpdate(long now)
        {
            update(now);
        }

        protected virtual void update(long now)
        {
            if ( time + timeSpan< now)
            {
                if (OnTick != null)
                    OnTick();
                this.time = now;
            }
        }



    }
}
