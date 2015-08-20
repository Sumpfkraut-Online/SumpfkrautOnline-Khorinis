using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Utilities.Threading
{
    class TestRunnable : Runnable
    {

        public TestRunnable (bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(startOnCreate, timeout, runOnce)
        { }

        public override void Run ()
        {
            Console.WriteLine("TestRunnable: " + DateTime.Now + " " + this.runOnce);
        }

    }
}
