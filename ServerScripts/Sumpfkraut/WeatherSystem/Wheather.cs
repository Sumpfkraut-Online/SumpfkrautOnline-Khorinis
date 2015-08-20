using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
//using GUC.Server.Scripting.Objects.World.WeatherType;

namespace GUC.Server.Scripts.Sumpfkraut.WeatherSystem
{

    public struct IGTime 
    {
        public int day;
        public int hour;
        public int minute;
    }

    public struct WheatherState
    {
        public World.WeatherType wheatherType;
        public DateTime startTime;
        public DateTime endTime;
    }

    class Wheather : Runnable
    {

        private List<WheatherState> wheaterStateQuery; // query of (future) wheather-states
        private WheatherState currWheatherState; // the current wheather-state
        private bool currWSChanged; // true, when current wheather state was changed and not yet activated
        private bool currWSExpired; // true, when current wheather state is expired and not yet substituted
        private int maxQueryLength; // maximum length of query which will be filled with calculated states
        private TimeSpan maxWSTime; // maximum timespan for wheather-states
        private TimeSpan minWSTime; // minimum timespan for wheather-states
        private int precFactor; // possibility of precipitation, including rain and snow (0 - 100)
        private int snowFactor; // possibility of snow when precipitation was chosen (0 - 100)

        private IGTime lastIGTime; // internal variable to track the last requested ig-time

        private Thread thread; // the thread on which object operates
        private TimeSpan defaultTimeout; // the default timeout / sleeptime of the thread

        private Random random;

        private Object wsQueryLock;



        public Wheather ()
            : this(true)
        { }

        public Wheather (bool startOnCreate)
            : base(false)
        {
            this.objName = "Wheather (default)";
            this.printStateControls = true;

            this.wheaterStateQuery = new List<WheatherState> { };
            this.currWSChanged = false;
            this.currWSExpired = false;
            this.maxQueryLength = 10;

            this.maxWSTime = new TimeSpan(0, 2, 0, 0); // 2 hours
            this.minWSTime = new TimeSpan(0, 0, 30, 0); // 30 minutes
            this.precFactor = 33; // 33 % chance of wheather with precipitation
            this.snowFactor = 0; // no snow on default

            this.lastIGTime = new IGTime();
            World.getTime(out this.lastIGTime.day, out this.lastIGTime.hour, out this.lastIGTime.minute);

            this.random = new Random();
            this.wsQueryLock = new Object();

            this.defaultTimeout = new TimeSpan(0, 2, 0); // default timeout / threadsleep is 2 minutes
            this.timeout = this.defaultTimeout;

            if (startOnCreate)
            {
                this.Start();
            }
        }

        // deletes all expired WheatherState-entries in the query, but leaves
        // all others in place, including the potential current WheatherState
        // !! it does not pick the current state like JumpToNextState !!
        public void CleanupQuery ()
        {
            lock (wsQueryLock)
            {
                DateTime dtNow = DateTime.Now;
                int nextStateIndex = -1;

                // find the WheatherState which would be the next chronologically
                if ((this.wheaterStateQuery != null) && (this.wheaterStateQuery.Count > 0))
                {
                    for (int i = 0; i < this.wheaterStateQuery.Count; i++)
                    {
                        // iterate until a future WheatherState is met and use the previous
                        // state-entry as current one
                        if (this.wheaterStateQuery[i].startTime <= dtNow)
                        {
                            nextStateIndex = i - 1;
                        }

                        if (this.wheaterStateQuery[i].startTime > dtNow)
                        {
                            break;
                        }
                    }
                }

                // remove all WeatherStates which would be too old
                // or which are overriden by subsequent states
                if (nextStateIndex > -1)
                {
                    this.wheaterStateQuery.RemoveRange(0, 1 + nextStateIndex);
                }
            }
        }

        // clean up the WheatherState-query, removing expired entries including the
        // potential current state + picks and the latter and activates it
        public void JumpToNextState ()
        {
            JumpToNextState(true);
        }

        public void JumpToNextState (bool activate)
        {
            lock (wsQueryLock)
            {
                DateTime dtNow = DateTime.Now;
                int jumpToIndex = -1;

                // find the WheatherState which would be the next chronologically
                if ((this.wheaterStateQuery != null) && (this.wheaterStateQuery.Count > 0))
                {
                    for (int i = 0; i < this.wheaterStateQuery.Count; i++)
                    {
                        // iterate until a future WheatherState is met and use the previous
                        // state-entry as current one
                        if (this.wheaterStateQuery[i].startTime <= dtNow)
                        {
                            jumpToIndex = i;
                        }

                        if (this.wheaterStateQuery[i].startTime > dtNow)
                        {
                            break;
                        }
                    }
                }

                // remove all WeatherStates which would be too old
                // or which are overriden by subsequent states
                if (jumpToIndex > -1)
                {
                    if (this.wheaterStateQuery[jumpToIndex].endTime > dtNow)
                    {
                        // only apply this Wheatherstate when it is still relevant
                        this.currWheatherState = this.wheaterStateQuery[jumpToIndex];
                        this.currWSChanged = true;
                        Print("Changed current WheatherState to " + this.currWheatherState.wheatherType
                            + this.currWheatherState.startTime + " " + this.currWheatherState.endTime);
                    }
                    // delete expired WheatherStates from query, including the current one,
                    // as it is saved elsewhere in this.currWheatherState
                    this.wheaterStateQuery.RemoveRange(0, jumpToIndex);
                }

                if (activate && this.currWSChanged)
                {
                    MakeLog("Activating WheatherState: " + this.currWheatherState.wheatherType
                            + this.currWheatherState.startTime + " " + this.currWheatherState.endTime);
                    World.setRainTime(this.currWheatherState.wheatherType, 0, 0, 23, 59);
                }
            }
         }

        public override void Run ()
        {
            DateTime dtNow = DateTime.Now;
            IGTime currIGTime = new IGTime();
            World.getTime(out currIGTime.day, out currIGTime.hour, out currIGTime.minute);

            // -- plan the future wheather -- 

            WheatherState newWS;
            TimeSpan tempTS;
            int tempInt;
            while (this.maxQueryLength > this.wheaterStateQuery.Count)
            {
                newWS = new WheatherState();

                // determine startTime
                if (this.wheaterStateQuery.Count > 0)
                {
                    if (this.wheaterStateQuery[this.wheaterStateQuery.Count - 1].endTime < dtNow)
                    {
                        newWS.startTime = dtNow;
                    }
                    else
                    {
                        newWS.startTime = 
                            this.wheaterStateQuery[this.wheaterStateQuery.Count - 1].endTime;
                    }
                }
                else
                {
                    newWS.startTime = dtNow;
                }

                // determine endTime
                tempTS = new TimeSpan(0, 0, this.random.Next(
                    (int) this.minWSTime.TotalSeconds, (int) this.maxWSTime.TotalSeconds) + 1);
                newWS.endTime = newWS.startTime + tempTS;

                // determine WheatherType
                tempInt = random.Next(101);
                if (tempInt >= this.precFactor)
                {
                    // no precipitation == default wheather
                    newWS.wheatherType = World.WeatherType.Undefined;
                }
                else
                {
                    tempInt = this.random.Next(101);
                    if (tempInt >= this.snowFactor)
                    {
                        newWS.wheatherType = World.WeatherType.Rain;
                    }
                    else
                    {
                        newWS.wheatherType = World.WeatherType.Snow;
                    }
                        
                }

                // go go little WheatherState!
                lock (wsQueryLock)
                {
                    MakeLog("Adding new WheatherState: " + newWS.wheatherType
                        + newWS.startTime + " " + newWS.endTime);
                    this.wheaterStateQuery.Add(newWS);
                }
            }

            // -- manage the current wheather -- 

            JumpToNextState(false);

            if (this.currWSChanged)
            {
                // replace the expired or default current wheather
                this.currWSExpired = false;
                World.setRainTime(this.currWheatherState.wheatherType, 0, 0, 23, 59);
            }
            else if (this.currWheatherState.endTime <= dtNow)
            {
                // let the current WheatherState expire without a new one available from query
                this.currWSExpired = true;
                World.setRainTime(World.WeatherType.Undefined, 0, 0, 23, 59);
            }
            else if (this.lastIGTime.hour > currIGTime.hour)
            {
                // with a new day, try to resume the current WheatherState
                // (maybe add day-comparison in else if-condition later, if necessary and feasable)
                if (!this.currWSExpired)
                {
                    MakeLog("Activating WheatherState: " + this.currWheatherState.wheatherType
                        + this.currWheatherState.startTime + " " + this.currWheatherState.endTime);
                    World.setRainTime(this.currWheatherState.wheatherType, 0, 0, 23, 59);
                }
            }
        }

    }
}
