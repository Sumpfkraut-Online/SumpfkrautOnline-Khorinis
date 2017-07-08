using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions
{

    public struct ScheduleProtocol
    {

        public DateTime InvocationTime;
        public TimedFunction TF;
        public int CallAmount;



        public ScheduleProtocol (DateTime invocationTime, TimedFunction tf, int callAmount)
        {
            InvocationTime = invocationTime;
            TF = tf;
            CallAmount = callAmount;
        }

    }

}
