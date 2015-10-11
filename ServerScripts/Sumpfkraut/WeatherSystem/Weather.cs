using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Server.WorldObjects;
using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;

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

        private List<WeatherState> weatherStateQueue; // queue of (future) weather-states
        private WeatherState currWeatherState; // the current weather-state
        private bool currWSChanged; // true, when current weather state was changed and not yet activated
        private bool currWSExpired; // true, when current weather state is expired and not yet substituted
        private int maxQueueLength; // maximum length of queue which will be filled with calculated states
        private TimeSpan maxWSTime; // maximum timespan for weather-states
        private TimeSpan minWSTime; // minimum timespan for weather-states
        private int precFactor; // possibility of precipitation, including rain and snow (0 - 100)
        private int snowFactor; // possibility of snow when precipitation was chosen (0 - 100)

        private IGTime lastIGTime; // internal variable to track the last requested ig-time

        private Thread thread; // the thread on which object operates
        private TimeSpan defaultTimeout; // the default timeout / sleeptime of the thread

        private Random random;

        private Object wsQueueLock;



        public Weather ()
            : this(true)
        { }

        public Weather (bool startOnCreate)
            : base(false)
        {
            this._objName = "weather (default)";
            this.printStateControls = true;

            this.weatherStateQueue = new List<WeatherState> { };
            this.currWSChanged = false;
            this.currWSExpired = false;
            this.maxQueueLength = 10;

            this.maxWSTime = new TimeSpan(0, 2, 0, 0); // 2 hours
            this.minWSTime = new TimeSpan(0, 0, 30, 0); // 30 minutes
            this.precFactor = 33; // 33 % chance of weather with precipitation
            this.snowFactor = 0; // no snow on default

            this.lastIGTime = new IGTime();
            World.getTime(out this.lastIGTime.day, out this.lastIGTime.hour, out this.lastIGTime.minute);

            this.random = new Random();
            this.wsQueueLock = new Object();

            this.defaultTimeout = new TimeSpan(0, 2, 0); // default timeout / threadsleep is 2 minutes
            this.timeout = this.defaultTimeout;

            if (startOnCreate)
            {
                this.Start();
            }
        }



        public static bool IsValidWeatherState (WeatherState ws)
        {
            return IsValidWeatherState(ws, true);
        }

        public static bool IsValidWeatherState (WeatherState ws, bool timeCheck)
        {
            return IsValidWeatherState(ws, timeCheck, false);
        }

        public static bool IsValidWeatherState (WeatherState ws, bool timeCheck, 
            bool isCompleteFuture)
        {
            return IsValidWeatherState(ws, timeCheck, isCompleteFuture, DateTime.Now);
        }

        // check if the WheatherState makes sense to the system
        public static bool IsValidWeatherState (WeatherState ws, bool timeCheck, 
            bool isCompleteFuture, DateTime checkDateTime)
        {
            int wtInt = (int) ws.weatherType;
            if ((wtInt < 0) || (wtInt > 2))
            {
                return false;
            }
            if (ws.startTime > ws.endTime)
            {
                return false;
            }
            if (timeCheck)
            {
                if (ws.endTime < checkDateTime)
                {
                    return false;
                }
                if (isCompleteFuture)
                {
                    if (ws.startTime < checkDateTime)
                    {
                        return false;
                    }
                }
                
            }
            return true;
        }
        


        // detect the index-range of ws with already registered WheaterState-objects
        // (ignores maxQueueLength)
        // overlapRange: the index-range which overlapts with the ws-timeinterval 
        //               (-1 means overlap with this.currentWeatherState;
        //                ignore overlapRange if relPos is not 0)
        // relPos: relative position of the overlap (-1: before, 0: within, 1: after registered states,
        //         -999: undefined)
        public void IndexRangeOfWSOverlap (WeatherState ws, out int[] overlapRange, out int relPos)
        {
            List<WeatherState> wsQueue = this.weatherStateQueue;
            overlapRange = new int[]{-999, -999};
            bool setRangeStart = false;
            bool setRangeEnd = false;
            relPos = -999;

            if (ws.endTime < this.currWeatherState.startTime)
            {
                // completely before registered weather-states
                relPos = -1;
                return;
            }
            else if (ws.startTime > wsQueue[wsQueue.Count - 1].endTime)
            {
                // completely after registered weather-states
                relPos = 1;
                return;
            }

            // first check on current state as it might avoid further searching
            if ((ws.startTime <= this.currWeatherState.endTime) && 
                (ws.endTime >= this.currWeatherState.startTime))
            {
                // ws definitely overlaps the current weather-state at least as starting-point
                overlapRange[0] = -1;
                setRangeStart = true;

                if (ws.endTime <= this.currWeatherState.endTime)
                {
                    // ws ends in the current weather-state
                    overlapRange[1] = -1;
                    setRangeEnd = true;
                }
            }

            // if necessary, search in the queue
            if ((!setRangeStart) && (!setRangeEnd))
            {
                int i = 0;
                while (i < wsQueue.Count)
                {
                    if (!setRangeStart)
                    {
                        if ((ws.startTime <= wsQueue[i].endTime) && (ws.endTime >= wsQueue[i].startTime))
                        {
                            overlapRange[0] = i;
                            setRangeStart = true;
                        }
                    }

                    if (!setRangeEnd)
                    {
                        if (setRangeStart && (ws.endTime <= wsQueue[i].endTime))
                        {
                            // exit on finding the overlap-end
                            overlapRange[1] = i;
                            setRangeEnd = true;
                            break;
                        }
                    }
                
                    i++;
                }
                if (!setRangeEnd)
                {
                    // if it's still not set, the las index must close the overlapRange
                    overlapRange[1] = wsQueue.Count - 1;
                }
            }
            
        }
        
        public void InsertWeatherState (WeatherState ws)
        {
            lock (wsQueueLock)
            {
                List<WeatherState> wsQueue= this.weatherStateQueue;

                if (!IsValidWeatherState(ws))
                {
                    MakeLogWarning("Invalid WeatherState detected in method InsertWeatherState!");
                    return;
                }

                int[] overlap;
                int relPos;
                IndexRangeOfWSOverlap(ws, out overlap, out relPos);

                // -- simple processing if possible --

                if (relPos == -1)
                {
                    MakeLogWarning("Prevented insert of already expired WeatherState into the queue!");
                    return;
                }
                else if (relPos == -999)
                {
                    MakeLogWarning("Prevented insert of invalid WeatherState into the queue!");
                    return;
                }
                else if (relPos == 1)
                {
                    // simply append the WeatherState when it doesn't conflict with registered states
                    // and is still relevant (in the future)
                    wsQueue.Add(ws);
                    return;
                }

                // -- more complex processing if there is no way around it --

                //if ((ws.startTime <= ))
                //{

                //}

                //if ((ws.startTime <= wsQueue[overlap[0]].startTime) && 
                //    (ws.endTime >= wsQueue[overlap[0]].endTime))
                //{
                //    // complete override of the starting
                //}
                
                // TO DO
            }
        }
              
        // deletes all expired weatherState-entries in the queue, but leaves
        // all others in place, including the potential current weatherState
        // !! it does not pick the current state like JumpToNextState !!
        public void CleanupQueue ()
        {
            lock (wsQueueLock)
            {
                DateTime dtNow = DateTime.Now;
                int nextStateIndex = -1;

                // find the weatherState which would be the next chronologically
                if ((this.weatherStateQueue != null) && (this.weatherStateQueue.Count > 0))
                {
                    for (int i = 0; i < this.weatherStateQueue.Count; i++)
                    {
                        // iterate until a future weatherState is met and use the previous
                        // state-entry as current one
                        if (this.weatherStateQueue[i].startTime <= dtNow)
                        {
                            nextStateIndex = i - 1;
                        }

                        if (this.weatherStateQueue[i].startTime > dtNow)
                        {
                            break;
                        }
                    }
                }

                // remove all WeatherStates which would be too old
                // or which are overriden by subsequent states
                if (nextStateIndex > -1)
                {
                    this.weatherStateQueue.RemoveRange(0, 1 + nextStateIndex);
                }
            }
        }

        // clean up the weatherState-queue, removing expired entries including the
        // potential current state + picks and the latter and activates it
        public void JumpToNextState ()
        {
            JumpToNextState(true);
        }

        public void JumpToNextState (bool activate)
        {
            lock (wsQueueLock)
            {
                DateTime dtNow = DateTime.Now;
                int jumpToIndex = -1;

                // find the weatherState which would be the next chronologically
                if ((this.weatherStateQueue != null) && (this.weatherStateQueue.Count > 0))
                {
                    for (int i = 0; i < this.weatherStateQueue.Count; i++)
                    {
                        // iterate until a future weatherState is met and use the previous
                        // state-entry as current one
                        if (this.weatherStateQueue[i].startTime <= dtNow)
                        {
                            jumpToIndex = i;
                        }

                        if (this.weatherStateQueue[i].startTime > dtNow)
                        {
                            break;
                        }
                    }
                }

                // remove all WeatherStates which would be too old
                // or which are overriden by subsequent states
                if (jumpToIndex > -1)
                {
                    if (this.weatherStateQueue[jumpToIndex].endTime > dtNow)
                    {
                        // only apply this weatherstate when it is still relevant
                        this.currWeatherState = this.weatherStateQueue[jumpToIndex];
                        this.currWSChanged = true;
                        Print("Changed current weatherState to " + this.currWeatherState.weatherType
                            + this.currWeatherState.startTime + " " + this.currWeatherState.endTime);
                    }
                    // delete expired weatherStates from queue, including the current one,
                    // as it is saved elsewhere in this.currweatherState
                    this.weatherStateQueue.RemoveRange(0, jumpToIndex);
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
            while (this.maxQueueLength > this.weatherStateQueue.Count)
            {
                newWS = new WeatherState();

                // determine startTime
                if (this.weatherStateQueue.Count > 0)
                {
                    if (this.weatherStateQueue[this.weatherStateQueue.Count - 1].endTime < dtNow)
                    {
                        newWS.startTime = dtNow;
                    }
                    else
                    {
                        newWS.startTime = 
                            this.weatherStateQueue[this.weatherStateQueue.Count - 1].endTime;
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
                lock (wsQueueLock)
                {
                    MakeLog("Adding new weatherState: " + newWS.weatherType
                        + newWS.startTime + " " + newWS.endTime);
                    this.weatherStateQueue.Add(newWS);
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
                // let the current weatherState expire without a new one available from queue
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
