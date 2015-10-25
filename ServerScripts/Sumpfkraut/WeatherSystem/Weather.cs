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

    //public struct WeatherState
    //{
    //    public World.WeatherType weatherType;
    //    public DateTime startTime;
    //    public DateTime endTime;
    //}

    enum WSSplitDecision
    {
        undefined,
        completeOverride,
        keepStart,
        keepMid,
        keepEnd,
    }

    class Weather : Runnable
    {

        public List<WeatherState> weatherStateQueue; // queue of (future) weather-states
        private WeatherState currWeatherState; // the current weather-state
        private bool currWSChanged; // true, when current weather state was changed until the current tick
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
            this.objName = "weather (default)";
            this.printStateControls = true;

            this.weatherStateQueue = new List<WeatherState> { };
            this.currWSChanged = false;
            this.maxQueueLength = 10;

            this.maxWSTime = new TimeSpan(0, 0, 0, 5);
            this.minWSTime = new TimeSpan(0, 0, 0, 3);
            //this.maxWSTime = new TimeSpan(0, 2, 0, 0); // 2 hours
            //this.minWSTime = new TimeSpan(0, 0, 30, 0); // 30 minutes
            this.precFactor = 33; // 33 % chance of weather with precipitation
            this.snowFactor = 0; // no snow on default

            this.lastIGTime = new IGTime();
            World.getTime(out this.lastIGTime.day, out this.lastIGTime.hour, out this.lastIGTime.minute);

            this.random = new Random();
            this.wsQueueLock = new Object();

            this.defaultTimeout = new TimeSpan(0, 0, 5);
            //this.defaultTimeout = new TimeSpan(0, 2, 0); // default timeout / threadsleep is 2 minutes
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
            if ((ws.weatherType != World.WeatherType.Undefined) && 
                (ws.weatherType != World.WeatherType.Rain) &&
                (ws.weatherType != World.WeatherType.Snow))
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



        public void FillUpQueue ()
        {
            lock (wsQueueLock)
            {
                DateTime dtNow = DateTime.Now;
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
                    MakeLog("Adding new weatherState: " + newWS.weatherType
                        + newWS.startTime + " " + newWS.endTime);
                    this.weatherStateQueue.Add(newWS);
                }
            }
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
            lock (wsQueueLock)
            {
                List<WeatherState> wsQueue = this.weatherStateQueue;
                overlapRange = new int[]{-999, -999};
                bool setRangeStart = false;
                bool setRangeEnd = false;
                relPos = -999;

                if (wsQueue.Count < 1)
                {
                    // nothing in queue mean nothing the WeatherState can conflict with
                    relPos = 1;
                    return;
                }
                else if (ws.endTime < wsQueue[0].startTime)
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

                // if still necessary, then search in the queue
                relPos = 0;
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

                    if (setRangeStart && (ws.endTime <= wsQueue[i].endTime))
                    {
                        // exit on finding the overlap-end
                        overlapRange[1] = i;
                        setRangeEnd = true;
                        break;
                    }
                
                    i++;
                }
                if (!setRangeEnd)
                {
                    // if it's still not set, the last index must close the overlapRange
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
                    MakeLogWarning("Prevented insert of already expired WeatherState into the queue (relPos == -1)!");
                    return;
                }
                else if (relPos == -999)
                {
                    MakeLogWarning("Prevented insert of invalid WeatherState into the queue (relPos == -999)!");
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

                WSSplitDecision[] indexOverride = new WSSplitDecision[1 + overlap[1] - overlap[0]];

                int i = 0;
                while (i < indexOverride.Length)
                {
                    if (ws.startTime <= wsQueue[overlap[0] + i].startTime)
                    {
                        if (ws.endTime >= wsQueue[overlap[0] + i].endTime)
                        {
                            indexOverride[i] = WSSplitDecision.completeOverride;
                        }
                        else
                        {
                            indexOverride[i] = WSSplitDecision.keepEnd;
                        }
                    }
                    else
                    {
                        if (ws.endTime >= wsQueue[overlap[0] + i].endTime)
                        {
                            indexOverride[i] = WSSplitDecision.keepStart;
                        }
                        else
                        {
                            indexOverride[i] = WSSplitDecision.keepMid;
                        }
                    }
                    i++;
                }

                OverrideQueue(ref ws, ref overlap, ref indexOverride);
            }
        }

        public void CleanupQueue ()
        {
            lock (wsQueueLock)
            {
                List<WeatherState> wsQueue = this.weatherStateQueue;

                DateTime dtNow = DateTime.Now;
                int jumpToIndex = -1;

                // find the weatherState which would be the next chronologically
                if ((wsQueue != null) && (wsQueue.Count > 0))
                {
                    for (int i = 0; i < wsQueue.Count; i++)
                    {
                        // iterate until a future weatherState is met and use the previous
                        // state-entry as current one
                        if (wsQueue[i].startTime <= dtNow)
                        {
                            jumpToIndex = i;
                        }

                        if (wsQueue[i].startTime > dtNow)
                        {
                            break;
                        }
                    }
                }

                // finally remove all WeatherStates which are be too old
                // or which are overriden by subsequent states
                // the next relevant entry will be index 0 from there on
                if (jumpToIndex > -1)
                {
                    wsQueue.RemoveRange(0, jumpToIndex);
                }
            }
         }

        public void UpdateCurrWeatherState (bool setIGWeather, bool forceIGWeather=false)
        {
            lock (this.wsQueueLock)
            {
                DateTime dtNow = DateTime.Now;
                List<WeatherState> wsQueue = this.weatherStateQueue;

                CleanupQueue();

                if (wsQueue.Count > 0)
                {
                    if ((this.currWeatherState == null) || 
                    (this.currWeatherState.endTime < dtNow))
                    {
                        this.currWeatherState = wsQueue[0];
                        this.currWSChanged = true;
                    }
                }
                else
                {
                    if (this.currWeatherState == null)
                    { }
                    else if (this.currWeatherState.endTime < dtNow)
                    {
                        this.currWeatherState = null;
                        this.currWSChanged = true;
                    }
                }

                if ((forceIGWeather) || (setIGWeather && this.currWSChanged))
                {
                    if (this.currWeatherState == null)
                    {
                        World.setRainTime(World.WeatherType.Undefined, 0, 0, 23, 59);
                        MakeLog("Updated ingame-weather to the default.");
                    }
                    else
                    {
                        World.setRainTime(this.currWeatherState.weatherType, 0, 0, 23, 59);
                        MakeLog("Updated ingame-weather to " + this.currWeatherState.weatherType +
                            ". Description: " + this.currWeatherState.description);
                    }
                }
            }
        }

         // outsourced subroutine of InsertWeatherState where is actual queue-manipulation is performed
        private void OverrideQueue (ref WeatherState ws, ref int[] overlap, ref WSSplitDecision[] indexOverride)
        {
            lock (wsQueueLock)
            {
                List<WeatherState> wsQueue = this.weatherStateQueue;

                // correct indices after splitting and removing entries during override
                int corrIndex = overlap[0];
                // index where the new WeatherState will be inserted at the end
                int insertIndex = overlap[0];
                WeatherState tempWS;
                int i = 0;
                while (i < indexOverride.Length)
                {
                    switch (indexOverride[i])
                    {
                        case WSSplitDecision.completeOverride:
                            wsQueue.RemoveAt(corrIndex);
                            //corrIndex -= 1;
                            break;
                        case WSSplitDecision.keepStart:
                            wsQueue[corrIndex].endTime = ws.startTime;
                            // right after the preserved entry-rest
                            insertIndex = corrIndex;
                            break;
                        case WSSplitDecision.keepMid:
                            tempWS = wsQueue[corrIndex];
                            wsQueue.RemoveAt(corrIndex);
                            wsQueue.Insert(corrIndex, new WeatherState(tempWS.weatherType, 
                                tempWS.startTime, ws.startTime));
                            corrIndex += 1;
                            wsQueue.Insert(corrIndex, new WeatherState(tempWS.weatherType, 
                                ws.endTime, tempWS.endTime));
                            // right after the preserved start-part of the splitted entry
                            insertIndex = corrIndex - 1;
                            break;
                        case WSSplitDecision.keepEnd:
                            wsQueue[corrIndex].startTime = ws.endTime;
                            // right before the preserved entry-rest
                            insertIndex = corrIndex - 1;
                            break;
                    }
                    corrIndex++;
                    i++;
                }

                wsQueue.Insert(insertIndex, ws);
            }
        }



        public override void Run ()
        {
            DateTime dtNow = DateTime.Now;
            IGTime currIGTime = new IGTime();
            World.getTime(out currIGTime.day, out currIGTime.hour, out currIGTime.minute);
            Print(this.weatherStateQueue.Count);
            FillUpQueue();

            if (this.lastIGTime.hour > currIGTime.hour)
            {
                // forcefully (re-)set the ingame-weather when the current weather
                // might be expired due to daybreak
                UpdateCurrWeatherState(true, true);
            }
            else
            {
                // no need to enforce weather when there is absolute need to
                UpdateCurrWeatherState(true, false);
            }
            Print(this.weatherStateQueue.Count);
        }

    }
}
