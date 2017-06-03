using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions
{

    public class TimedFunction : ExtendedObject
    {

        new public static readonly string _staticName = "TimedFunction (s)";



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

        // state of parameters used by the surrounded function as well as passed to it if invoked again
        // (provide starter set of parameters if necessary)
        protected object[] parameters;
        public object[] GetParameters () { lock (_lock) { return parameters; } }
        public void SetParameters (object[] value) { lock (_lock) { parameters = value; } }

        protected long numberOfInvokes;
        public long GetNumberOfInvokes () { lock (_lock) { return numberOfInvokes; } }
        public void SetNumberOfInvokes (long value) { lock (_lock) { numberOfInvokes = value; } }
        public void IterateNumberOfInvokes () { lock (_lock) { numberOfInvokes++; } }

        protected bool hasMaxInvokes;
        public bool HasMaxInvokes { get { return hasMaxInvokes; } }
        protected long maxInvokes;
        public long GetMaxInvokes () { return maxInvokes; }

        protected bool hasSpecifiedTimes;
        public bool HasSpecificTimes { get { return hasSpecifiedTimes; } }
        protected DateTime[] specifiedTimes;
        public DateTime[] GetSpecifiedTimes () { return specifiedTimes; }

        protected bool hasIntervals;
        public bool HasIntervals { get { return hasIntervals; } }
        protected TimeSpan[] intervals;
        public TimeSpan[] GetIntervals () { return intervals; }

        protected bool hasStartEnd;
        public bool HasStartEnd { get { return hasStartEnd; } }
        protected Tuple<DateTime, DateTime> startEnd;
        public Tuple<DateTime, DateTime> GetStartEnd () { return startEnd; }
        public DateTime GetStart () { return startEnd.Item1; }
        public DateTime GetEnd () { return startEnd.Item2; }



        // run the action as soon as possible a single time
        public TimedFunction ()
            : this(null, null, null)
        { }

        // run at specified times until they all passed away
        public TimedFunction (DateTime[] specifiedTimes)
            : this(specifiedTimes, null, null)
        { }

        // run at specified times in a certain time range
        public TimedFunction (DateTime[] specifiedTimes, Tuple<DateTime, DateTime> startEnd)
            : this(specifiedTimes, null, startEnd)
        { }

        // run endlessly at given intervals
        public TimedFunction (TimeSpan[] intervals)
            : this(null, intervals, null)
        { }

        // run at given intervals in a certain time range
        public TimedFunction (TimeSpan[] intervals,  Tuple<DateTime, DateTime> startEnd)
            : this(null, intervals, startEnd)
        { }

        // general constructor
        public TimedFunction (DateTime[] specifiedTimes, TimeSpan[] intervals, 
            Tuple<DateTime, DateTime> startEnd)
        {
            SetObjName("TimedFunction");
            _lock = new object();
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
            if (startEnd != null)
            {
                hasStartEnd = true;
                this.startEnd = startEnd;
            }
        }

    }

}
