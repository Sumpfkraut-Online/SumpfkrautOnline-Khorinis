using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.Utilities.Threading
{
    class TestRunnable : Runnable
    {

        int day = 0;
        int hour = 0;
        int minute = 0;

        public TestRunnable (bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(startOnCreate, timeout, runOnce)
        { }

        public override void Run ()
        {
            Console.WriteLine("TestRunnable: " + DateTime.Now + " " + this.runOnce);
            Console.WriteLine("TestRunnable: " + day + " " + hour + " " + minute);
            WorldObjects.World.NewWorld.ChangeTime(day, hour, minute);
            hour++;
        }

    }
}
