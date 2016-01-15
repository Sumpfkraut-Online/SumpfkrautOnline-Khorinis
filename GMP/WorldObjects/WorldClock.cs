using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Types;

namespace GUC.Client.WorldObjects
{

    class WorldClock
    {

        protected Thread thread;
        protected EventWaitHandle resetEvent;
        public TimeSpan timeout;
        protected AutoResetEvent waitHandle = new AutoResetEvent(false);

        public WorldClock (TimeSpan timeout, IgTime igTime)
        {
            this.timeout = timeout;
            this.resetEvent = new ManualResetEvent(true);
            this.thread = new Thread(_Run);
        }

        public void _Run ()
        {
            while (true)
            {
                this.resetEvent.WaitOne(Timeout.Infinite);
                this.Run();
                Thread.Sleep(this.timeout);
            }
        }

        public void Run ()
        {

        }

    }

}
