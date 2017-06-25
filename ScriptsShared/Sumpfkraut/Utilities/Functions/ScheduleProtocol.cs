using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions
{

    public struct ScheduleProtocol
    {

        public TimedFunction TF;
        public int CallAmount;



        public ScheduleProtocol (TimedFunction tf, int callAmount)
        {
            TF = tf;
            CallAmount = callAmount;
        }

    }

}
