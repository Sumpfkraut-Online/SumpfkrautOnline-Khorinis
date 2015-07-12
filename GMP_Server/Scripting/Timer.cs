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
        /// <summary>
        /// Get or Set the used TimeSpan for this timer.
        /// The timespan uses ticks ( 10000 ticks are 1 ms )
        /// </summary>
        public long TimeSpan { get; set; }

        public bool IsStarted { get; protected set; }

        protected long m_Time = 0;

        public delegate void TimerEvent();
        public event TimerEvent OnTick;

        /// <summary>
        /// Constructor of Timer-Class.
        /// Do not forget to use the Start method to activate the timer.
        /// </summary>
        /// <param name="timespan">Timespan, how long the timer waits until it calls the OnTick event. The timespan uses ticks (10000 ticks are 1 millisecond)</param>
        public Timer(long timespan)
        {
            this.TimeSpan = timespan;
        }

        /// <summary>
        /// Start the timer.
        /// 
        /// You can overwrite this method but you should call the base with base.Start() to add it to the update queue.
        /// </summary>
        public virtual void Start()
        {
            if (IsStarted)
                return;
            this.m_Time = DateTime.Now.Ticks;
            IsStarted = true;

            Program.ScriptManager.m_TimerList.Add(this);
        }

        /// <summary>
        /// Stops the timer
        /// 
        /// You can overwrite this method but you should call the base with base.End() to remove it from the update queue.
        /// </summary>
        public virtual void End()
        {
            if (!IsStarted)
                return;

            Program.ScriptManager.m_TimerList.Remove(this);
            IsStarted = false;
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
            if ( m_Time + TimeSpan< now)
            {
                if (OnTick != null)
                    OnTick();
                this.m_Time = now;
            }
        }



    }
}
