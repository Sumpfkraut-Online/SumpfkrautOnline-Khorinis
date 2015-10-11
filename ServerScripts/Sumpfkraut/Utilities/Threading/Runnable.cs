using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Server.Scripts.Sumpfkraut.Utilities.Threading
{
    public abstract class Runnable : ScriptObject, IRunnable
    {

        protected bool printStateControls;

        protected Thread thread; // the thread, on which the object operates
        protected EventWaitHandle resetEvent;
        public TimeSpan timeout;
        public bool runOnce;


        public Runnable () 
            : this(true)
        { }

        public Runnable (bool startOnCreate)
            : this (startOnCreate, new TimeSpan(0, 0, 0))
        { }

        public Runnable (bool startOnCreate, TimeSpan timeout)
            : this(startOnCreate, timeout, false)
        { }

        public Runnable (bool startOnCreate, TimeSpan timeout, bool runOnce)
        {
            this._objName = "Runnable (default)";
            this.printStateControls = false;
            this.timeout = timeout;
            this.runOnce = runOnce;
            this.resetEvent = new ManualResetEvent(true);
            this.thread = new Thread(this._Run);
            this.Start();
        }



        public virtual void Abort ()
        {
            if (this.printStateControls)
            {
                Print("Aborting thread...");
            }
            this.thread.Abort();
        }
        
        public virtual void Resume ()
        {
            if (this.printStateControls)
            {
                Print("Resuming thread...");
            }
            this.resetEvent.Set();
        }

        public virtual void Start ()
        {
            if (this.printStateControls)
            {
                Print("Starting thread...");
            }
            if (this.thread.ThreadState == ThreadState.Unstarted)
            {
                this.thread.Start();
            }
        }

        public virtual void Suspend ()
        {
            if (this.printStateControls)
            {
                Print("Suspending thread...");
            }
            //this.resetEvent.WaitOne(Timeout.Infinite);
            this.resetEvent.Reset();
        }



        protected virtual void _Run ()
        {
            while (true)
            {
                this.resetEvent.WaitOne(Timeout.Infinite);
                this.Run();
                Thread.Sleep(this.timeout);
                if (this.runOnce)
                {
                    this.resetEvent.Reset();
                }
            }
        }

        // override this one
        public virtual void Run ()
        {
            Console.WriteLine("Runnable: " + DateTime.Now + "  " + this.timeout);
        }

    }
}
