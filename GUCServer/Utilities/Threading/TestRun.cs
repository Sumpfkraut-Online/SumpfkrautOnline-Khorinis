using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Utilities.Threading
{
    public class TestRun : Runnable
    {

        new public readonly String _staticName = "TestRun (s)";



        public TestRun (bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(false, timeout, runOnce)
        {
            SetObjName("TestRun");
            printStateControls = true;

            if (startOnCreate)
            {
                Start();
            }
        }

        public override void Run ()
        {
            base.Run();

            Print(DateTime.Now);
        }

    }
}
