using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Utilities.Threading
{
    public abstract class AbstractRunnable : ExtendedObject
    {

        new public static readonly string _staticName = "AbstractRunnable (s)"; 

        public bool printStateControls;

        protected Thread thread; // the thread, on which the object operates
        protected EventWaitHandle resetEvent;
        public TimeSpan timeout;
        public bool runOnce;
        public AutoResetEvent waitHandle;


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
            this.printStateControls = false;
            this.timeout = timeout;
            if ((this.timeout == TimeSpan.MinValue) || (this.timeout < TimeSpan.Zero))
            {
                this.timeout = TimeSpan.Zero;
            }
            this.runOnce = runOnce;
            this.resetEvent = new ManualResetEvent(true);
            this.thread = new Thread(this._Run);

            if (startOnCreate)
            {
                Start();
            }
        }



        public virtual void Abort ()
        {
            try
            {
                if (printStateControls)
                {
                    Print("Aborting thread...");
                }
                thread.Abort();
            }
            catch (Exception ex)
            {
                MakeLogError("Failed to abort thread: " + ex);
            }
        }

        public virtual void Reset ()
        {
            try
            {
                Suspend();
                Resume();
            }
            catch (Exception ex)
            {
                MakeLogError("Failed to reset thread: " + ex);
            }
        }
        
        public virtual void Resume ()
        {
            try
            {
                if (printStateControls)
                {
                    Print("Resuming thread...");
                }
                resetEvent.Set();
            }
            catch (Exception ex)
            {
                MakeLogError("Failed to resume thread: " + ex);
            }
        }

        public virtual void Start ()
        {
            try
            {
                if (printStateControls)
                {
                    Print("Starting thread...");
                }
                if (thread.ThreadState == ThreadState.Unstarted)
                {
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                MakeLogError("Failed to start thread: " + ex);
            }
        }

        public virtual void Suspend ()
        {
            try
            {
                if (printStateControls)
                {
                    Print("Suspending thread...");
                }
                //this.resetEvent.WaitOne(Timeout.Infinite);
                resetEvent.Reset();
            }
            catch (Exception ex)
            {
                MakeLogError("Failed to suspend thread: " + ex);
            }
        }

        protected virtual void _Run ()
        {
            Init();

            while (true)
            {
                resetEvent.WaitOne(Timeout.Infinite);

                Run();

                if (runOnce)
                {
                    resetEvent.Reset();
                }
                Thread.Sleep(timeout);

                //waitHandle.WaitOne();
                //this.Run();
                //Thread.Sleep(this.timeout);
            }
        }



        // override this method
        public virtual void Init ()
        { }
        
        // override this method
        public virtual void Run ()
        { }

    }
}
