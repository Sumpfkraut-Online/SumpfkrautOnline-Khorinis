using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Scripts.Sumpfkraut.Utilities.Threading
{
    public abstract class AbstractRunnable : ScriptObject, IRunnable
    {

        new public static readonly string _staticName = "AbstractRunnable (static)"; 

        public bool printStateControls;

        protected Thread thread; // the thread, on which the object operates
        protected EventWaitHandle resetEvent;
        public TimeSpan timeout;
        public bool runOnce;
        protected AutoResetEvent waitHandle = new AutoResetEvent(false);


        public AbstractRunnable () 
            : this(true)
        { }

        public AbstractRunnable (bool startOnCreate)
            : this (startOnCreate, new TimeSpan(0, 0, 0))
        { }

        public AbstractRunnable (bool startOnCreate, TimeSpan timeout)
            : this(startOnCreate, timeout, false)
        { }

        public AbstractRunnable (bool startOnCreate, TimeSpan timeout, bool runOnce)
        {
            //SetObjName("AbstractRunnable (default)");
            this.printStateControls = false;
            this.timeout = timeout;
            if ((this.timeout == TimeSpan.MinValue) || (this.timeout < TimeSpan.Zero))
            {
                this.timeout = TimeSpan.Zero;
            }
            this.runOnce = runOnce;
            this.resetEvent = new ManualResetEvent(true);
            this.thread = new Thread(this._Run);
            if (startOnCreate) { this.Start(); }
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

        protected virtual void _Run ()
        {
            Init();

            while (true)
            {
                this.resetEvent.WaitOne(Timeout.Infinite);

                this.Run();

                if (this.runOnce)
                {
                    this.resetEvent.Reset();
                }
                Thread.Sleep(this.timeout);

                //waitHandle.WaitOne();
                //this.Run();
                //Thread.Sleep(this.timeout);
            }
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



        // override this method
        public virtual void Init ()
        { }
        
        // override this method
        public virtual void Run ()
        { }

    }
}
