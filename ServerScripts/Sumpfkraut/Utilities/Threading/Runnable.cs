using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Utilities.Threading
{
    class Runnable : AbstractRunnable
    {

        public delegate void InitEventHandler ();
        public InitEventHandler OnInit;

        public delegate void RunEventHandler ();
        public RunEventHandler OnRun;



        public Runnable (bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(startOnCreate, timeout, runOnce)
        { }



        public override void Init ()
        {
            if (OnInit != null)
            {
                OnInit.Invoke();
            }
        }

        public override void Run ()
        {
            if (OnRun != null)
            {
                OnRun.Invoke();
            }
        }

    }
}
