using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions
{

    public class TimedFunction
    {

        // used to lock changes on the TimedAction-object
        protected object _lock;

        // Action to be performed with any number of parameters provided 
        // (convertion of objects to their respective types necessary for
        // effective use => avoid when possible and / or use surrounding
        // function scope to define parameters which then can be used by the
        // Action as inner function with access to that scope
        // ... which prevents garbage collection / closing of the surrounding scope)
        protected Func<object[], object[]> func;
        public Func<object[], object[]> GetFunc () { lock (_lock) { return func; } }
        public void SetFunc (Func<object[], object[]> value) { lock (_lock) { func = value; } }

        protected bool hasSpecifiedTimes;
        public bool HasSpecificTimes { get { return hasSpecifiedTimes; } }
        protected DateTime[] specifiedTimes;
        public DateTime[] GetSpecifiedTimes () { lock (_lock) { return specifiedTimes; } }

        public bool hasIntervals;
        public bool HasIntervals { get { return hasIntervals; } }
        protected TimeSpan[] intervals;
        public TimeSpan[] GetIntervals () { lock (_lock) { return intervals; } }

        public bool hasStart;
        public bool HasStart { get { return hasStart; } }
        protected DateTime start;
        public DateTime GetStart () { lock (_lock) { return start; } }

        public bool hasEnd;
        public bool HasEnd { get { return hasEnd; } }
        protected DateTime end;
        public DateTime GetEnd () { lock (_lock) { return end; } }



        // run the action as soon as possible a single time
        public TimedFunction ()
            : this(null, null, DateTime.Now, DateTime.Now)
        { }

        // run at specified times until they all passed away
        public TimedFunction (DateTime[] specifiedTimes)
            : this(specifiedTimes, null, DateTime.MinValue, DateTime.MaxValue)
        { }

        // run at specified times in a certain time range
        public TimedFunction (DateTime[] specifiedTimes, Tuple<DateTime, DateTime> startEnd)
            : this(specifiedTimes, null, startEnd.Item1, startEnd.Item2)
        { }

        // run endlessly at given intervals
        public TimedFunction (TimeSpan[] intervals)
            : this(null, intervals, DateTime.MinValue, DateTime.MaxValue)
        { }

        // run at given intervals in a certain time range
        public TimedFunction (TimeSpan[] intervals,  Tuple<DateTime, DateTime> startEnd)
            : this(null, intervals, startEnd.Item1, startEnd.Item2)
        { }

        // general constructor
        public TimedFunction (DateTime[] specifiedTimes, TimeSpan[] intervals, 
            DateTime start, DateTime end)
        {
            if (specifiedTimes != null)
            {
                hasSpecifiedTimes = true;
                this.specifiedTimes = specifiedTimes;
            }
            if (intervals != null)
            {
                hasIntervals = true;
                this.intervals = intervals;
            }
            this.start = start;
            this.end = end;
            _lock = new object();
        }

    }

}
