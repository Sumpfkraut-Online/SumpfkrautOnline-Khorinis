using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Scripting.Listener;

namespace GMP_Server.Scripting
{
    class Timer
    {
        public long startTime;
        public long time;

        public ITimerListener listener;
    }
}
