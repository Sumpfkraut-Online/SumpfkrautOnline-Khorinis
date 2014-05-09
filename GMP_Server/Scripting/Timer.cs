using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting
{
    /// <summary>
    /// Implements a Timer-class for use in server-side Scripts.
    /// It can be used to create your own timer too.
    /// </summary>
    public class Timer
    {
        protected long timeSpan = 0;

        protected bool isStarted = false;
        protected long time = 0;

        public event Events.TimerEvent OnTick;

        /// <summary>
        /// Constructor of Timer-Class.
        /// Do not forget to use the Start method to activate the timer.
        /// </summary>
        /// <param name="timespan">Timespan, how long the timer waits until it calls the OnTick event. The timespan uses ticks (10000 ticks are 1 millisecond)</param>
        public Timer(long timespan)
        {
            this.timeSpan = timespan;
        }

        /// <summary>
        /// Get or Set the used TimeSpan for this timer.
        /// The timespan uses ticks ( 10000 ticks are 1 ms )
        /// </summary>
        public long TimeSpan { get { return timeSpan; } set { timeSpan = value; } }

        /// <summary>
        /// Start the timer.
        /// 
        /// You can overwrite this method but you should call the base with base.Start() to add it to the update queue.
        /// </summary>
        public virtual void Start()
        {
            if (isStarted)
                return;
            this.time = DateTime.Now.Ticks;
            isStarted = true;

            Program.ScriptManager.TimerList.Add(this);
        }

        /// <summary>
        /// Stops the timer
        /// 
        /// You can overwrite this method but you should call the base with base.End() to remove it from the update queue.
        /// </summary>
        public virtual void End()
        {
            if (!isStarted)
                return;

            Program.ScriptManager.TimerList.Remove(this);
            isStarted = false;
        }

        /// <summary>
        /// internal function, will be called by ScriptManager.
        /// </summary>
        /// <param name="now"></param>
        internal void iUpdate(long now)
        {
            update(now);
        }

        /// <summary>
        /// Update-Function
        /// </summary>
        /// <param name="now">DateTime.Now.Ticks, the generation of the timestamp seems to be slow, so we gernerate it before</param>
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
