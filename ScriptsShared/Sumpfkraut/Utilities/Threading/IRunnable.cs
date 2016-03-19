using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Utilities.Threading
{
    interface IRunnable
    {

        void Abort ();
        void Resume ();
        void Start ();
        void Suspend ();
        //void _Run ();
        void Run ();

    }
}
