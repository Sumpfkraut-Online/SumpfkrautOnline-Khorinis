using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Utilities.Threading
{
    public class Runnable : AbstractRunnable
    {

        public delegate void InitEventHandler (Runnable sender);
        public InitEventHandler OnInit;

        public delegate void RunEventHandler (Runnable sender);
        public RunEventHandler OnRun;

        //new public AutoResetEvent waitHandle;



        public Runnable (bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(startOnCreate, timeout, runOnce)
        {
        }



        public override void Init ()
        {
            if (OnInit != null)
            {
                OnInit.Invoke(this);
            }
        }

        public override void Run ()
        {
            if (OnRun != null)
            { 
                OnRun.Invoke(this);
            }
        }

    }
}
