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

    public struct WeatherState
    {
        public World.WeatherType weatherType;
        public DateTime startTime;
        public DateTime endTime;
    }

    class Weather : Runnable
    {

        private List<WeatherState> weatherStateQuery; // query of (future) weather-states
        private WeatherState currWeatherState; // the current weather-state
        private bool currWSChanged; // true, when current weather state was changed and not yet activated
        private bool currWSExpired; // true, when current weather state is expired and not yet substituted
        private int maxQueryLength; // maximum length of query which will be filled with calculated states
        private TimeSpan maxWSTime; // maximum timespan for weather-states
        private TimeSpan minWSTime; // minimum timespan for weather-states
        private int precFactor; // possibility of precipitation, including rain and snow (0 - 100)
        private int snowFactor; // possibility of snow when precipitation was chosen (0 - 100)

        private IGTime lastIGTime; // internal variable to track the last requested ig-time

        private Thread thread; // the thread on which object operates
        private TimeSpan defaultTimeout; // the default timeout / sleeptime of the thread

        private Random random;

        private Object wsQueryLock;



        public Weather ()
            : this(true)
        { }

        public Weather (bool startOnCreate)
            : base(false)
        {
            this.objName = "weather (default)";
            this.printStateControls = true;

            this.weatherStateQuery = new List<WeatherState> { };
            this.currWSChanged = false;
            this.currWSExpired = false;
            this.maxQueryLength = 10;

            this.maxWSTime = new TimeSpan(0, 2, 0, 0); // 2 hours
            this.minWSTime = new TimeSpan(0, 0, 30, 0); // 30 minutes
            this.precFactor = 33; // 33 % chance of weather with precipitation
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

        // deletes all expired weatherState-entries in the query, but leaves
        // all others in place, including the potential current weatherState
        // !! it does not pick the current state like JumpToNextState !!
        public void CleanupQuery ()
        {
            lock (wsQueryLock)
            {
                DateTime dtNow = DateTime.Now;
                int nextStateIndex = -1;

                // find the weatherState which would be the next chronologically
                if ((this.weatherStateQuery != null) && (this.weatherStateQuery.Count > 0))
                {
                    for (int i = 0; i < this.weatherStateQuery.Count; i++)
                    {
                        // iterate until a future weatherState is met and use the previous
                        // state-entry as current one
                        if (this.weatherStateQuery[i].startTime <= dtNow)
                        {
                            nextStateIndex = i - 1;
                        }

                        if (this.weatherStateQuery[i].startTime > dtNow)
                        {
                            break;
                        }
                    }
                }

                // remove all WeatherStates which would be too old
                // or which are overriden by subsequent states
                if (nextStateIndex > -1)
                {
                    this.weatherStateQuery.RemoveRange(0, 1 + nextStateIndex);
                }
            }
        }

        // clean up the weatherState-query, removing expired entries including the
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

                // find the weatherState which would be the next chronologically
                if ((this.weatherStateQuery != null) && (this.weatherStateQuery.Count > 0))
                {
                    for (int i = 0; i < this.weatherStateQuery.Count; i++)
                    {
                        // iterate until a future weatherState is met and use the previous
                        // state-entry as current one
                        if (this.weatherStateQuery[i].startTime <= dtNow)
                        {
                            jumpToIndex = i;
                        }

                        if (this.weatherStateQuery[i].startTime > dtNow)
                        {
                            break;
                        }
                    }
                }

                // remove all WeatherStates which would be too old
                // or which are overriden by subsequent states
                if (jumpToIndex > -1)
                {
                    if (this.weatherStateQuery[jumpToIndex].endTime > dtNow)
                    {
                        // only apply this weatherstate when it is still relevant
                        this.currWeatherState = this.weatherStateQuery[jumpToIndex];
                        this.currWSChanged = true;
                        Print("Changed current weatherState to " + this.currWeatherState.weatherType
                            + this.currWeatherState.startTime + " " + this.currWeatherState.endTime);
                    }
                    // delete expired weatherStates from query, including the current one,
                    // as it is saved elsewhere in this.currweatherState
                    this.weatherStateQuery.RemoveRange(0, jumpToIndex);
                }

                if (activate && this.currWSChanged)
                {
                    MakeLog("Activating weatherState: " + this.currWeatherState.weatherType
                            + this.currWeatherState.startTime + " " + this.currWeatherState.endTime);
                    World.setRainTime(this.currWeatherState.weatherType, 0, 0, 23, 59);
                }
            }
         }

        public override void Run ()
        {
            DateTime dtNow = DateTime.Now;
            IGTime currIGTime = new IGTime();
            World.getTime(out currIGTime.day, out currIGTime.hour, out currIGTime.minute);

            // -- plan the future weather -- 

            WeatherState newWS;
            TimeSpan tempTS;
            int tempInt;
            while (this.maxQueryLength > this.weatherStateQuery.Count)
            {
                newWS = new WeatherState();

                // determine startTime
                if (this.weatherStateQuery.Count > 0)
                {
                    if (this.weatherStateQuery[this.weatherStateQuery.Count - 1].endTime < dtNow)
                    {
                        newWS.startTime = dtNow;
                    }
                    else
                    {
                        newWS.startTime = 
                            this.weatherStateQuery[this.weatherStateQuery.Count - 1].endTime;
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

                // determine weatherType
                tempInt = random.Next(101);
                if (tempInt >= this.precFactor)
                {
                    // no precipitation == default weather
                    newWS.weatherType = World.WeatherType.Undefined;
                }
                else
                {
                    tempInt = this.random.Next(101);
                    if (tempInt >= this.snowFactor)
                    {
                        newWS.weatherType = World.WeatherType.Rain;
                    }
                    else
                    {
                        newWS.weatherType = World.WeatherType.Snow;
                    }
                        
                }

                // go go little weatherState!
                lock (wsQueryLock)
                {
                    MakeLog("Adding new weatherState: " + newWS.weatherType
                        + newWS.startTime + " " + newWS.endTime);
                    this.weatherStateQuery.Add(newWS);
                }
            }

            // -- manage the current weather -- 

            JumpToNextState(false);

            if (this.currWSChanged)
            {
                // replace the expired or default current weather
                this.currWSExpired = false;
                World.setRainTime(this.currWeatherState.weatherType, 0, 0, 23, 59);
            }
            else if (this.currWeatherState.endTime <= dtNow)
            {
                // let the current weatherState expire without a new one available from query
                this.currWSExpired = true;
                World.setRainTime(World.WeatherType.Undefined, 0, 0, 23, 59);
            }
            else if (this.lastIGTime.hour > currIGTime.hour)
            {
                // with a new day, try to resume the current weatherState
                // (maybe add day-comparison in else if-condition later, if necessary and feasable)
                if (!this.currWSExpired)
                {
                    MakeLog("Activating weatherState: " + this.currWeatherState.weatherType
                        + this.currWeatherState.startTime + " " + this.currWeatherState.endTime);
                    World.setRainTime(this.currWeatherState.weatherType, 0, 0, 23, 59);
                }
            }
        }

    }
}
