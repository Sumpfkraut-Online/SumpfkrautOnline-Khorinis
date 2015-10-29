using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Server.WorldObjects;
using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
//using GUC.Server.Scripting.Objects.World.WeatherType;

namespace GUC.Server.Scripts.Sumpfkraut.WeatherSystem
{

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
        protected WeatherState currWeatherState; // the current weather-state
        protected bool currWSChanged; // true, when current weather state was changed until the current tick

        protected int maxQueueLength; // maximum length of queue which will be filled with calculated states
        public int GetMaxQueueLength() { return this.maxQueueLength; }
        public void SetMaxQueueLength (int mql) { this.maxQueueLength = mql; }

        // maximum timespan for weather-states... old weather-entries are uneffected (clear to reset them)
        protected TimeSpan maxWSTime;
        public TimeSpan GetMaxWSTimeh() { return this.maxWSTime; }
        public void SetMaxWSTime(TimeSpan maxWSTime) { this.maxWSTime = maxWSTime; }

        // minimum timespan for weather-states... old weather-entries are uneffected (clear to reset them)
        protected TimeSpan minWSTime;
        public TimeSpan GetMinWSTimeh() { return this.minWSTime; }
        public void SetMinWSTime(TimeSpan minWSTime) { this.minWSTime = minWSTime; }

        // possibility of precipitation, including rain and snow (0 - 100)
        protected int precFactor;
        public int GetPrecFactor () { return this.precFactor; }
        public void SetPrecFactor (int precFactor)
        {
            if ((precFactor > 0) && (precFactor <= 100))
            {
                this.precFactor = precFactor;
            }
            else
            {
                MakeLogWarning("Prevented illegal change of precFactor to " + precFactor + "!");
            }
        }

        // possibility of snow when precipitation was chosen (0 - 100)
        protected int snowFactor;
        public int GetSnowFactor() { return this.snowFactor; }
        public void SetSnowFactor(int snowFactor)
        {
            if ((snowFactor > 0) && (snowFactor <= 100))
            {
                this.snowFactor = snowFactor;
            }
            else
            {
                MakeLogWarning("Prevented illegal change of snowFactor to " + snowFactor + "!");
            }
        }

        protected IGTime lastIGTime; // internal variable to track the last requested ig-time

        protected TimeSpan defaultTimeout; // the default timeout / sleeptime of the thread
        // custom timeout changable during runtime
        protected TimeSpan customTimeout;
        public TimeSpan GetTimeout () { return this.customTimeout; }
        public void SetTimeout(TimeSpan timeout)
        {
            this.customTimeout = timeout;
            this.timeout = timeout;
        }

        protected Random random;

        protected Object lock_WSQueue;



        public Weather ()
            : this("Weather (default)")
        { }

        public Weather (String _objName)
            : this(true, _objName)
        { }

        public Weather (bool startOnCreate, String _objName)
            : base(false)
        {
            this.printStateControls = true;

            this._objName = _objName;
            this.weatherStateQueue = new List<WeatherState> { };
            this.currWSChanged = false;
            this.maxQueueLength = 10;

            this.maxWSTime = new TimeSpan(0, 0, 0, 20);
            this.minWSTime = new TimeSpan(0, 0, 0, 10);
            //this.maxWSTime = new TimeSpan(0, 2, 0, 0); // 2 hours
            //this.minWSTime = new TimeSpan(0, 0, 30, 0); // 30 minutes
            this.precFactor = 90; // 90 % chance of weather with precipitation
            this.snowFactor = 33; // 33 % change of snow when there is precipitation

            this.lastIGTime = new IGTime();
            this.lastIGTime = World.NewWorld.GetIGTime();

            this.random = new Random();
            this.lock_WSQueue = new Object();

            this.defaultTimeout = new TimeSpan(0, 0, 3);
            //this.defaultTimeout = new TimeSpan(0, 2, 0); // default timeout / threadsleep is 2 minutes
            this.customTimeout = defaultTimeout;
            this.timeout = this.customTimeout;

            if (startOnCreate)
            {
                this.Start();
            }
        }



        public static bool IsValidWeatherState(WeatherState ws)
        {
            return IsValidWeatherState(ws, true);
        }

        public static bool IsValidWeatherState(WeatherState ws, bool timeCheck)
        {
            return IsValidWeatherState(ws, timeCheck, false);
        }

        public static bool IsValidWeatherState(WeatherState ws, bool timeCheck,
            bool isCompleteFuture)
        {
            return IsValidWeatherState(ws, timeCheck, isCompleteFuture, DateTime.Now);
        }

        // check if the WheatherState makes sense to the system
        public static bool IsValidWeatherState(WeatherState ws, bool timeCheck,
            bool isCompleteFuture, DateTime checkDateTime)
        {
            if ((ws.weatherType != WeatherType.undefined) &&
                (ws.weatherType != WeatherType.rain) &&
                (ws.weatherType != WeatherType.snow))
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



        public void FillUpQueue()
        {
            lock (lock_WSQueue)
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
                        (int)this.minWSTime.TotalSeconds, (int)this.maxWSTime.TotalSeconds) + 1);
                    newWS.endTime = newWS.startTime + tempTS;

                    // determine weatherType
                    tempInt = random.Next(101);
                    if (tempInt >= this.precFactor)
                    {
                        // no precipitation == default weather
                        newWS.weatherType = WeatherType.undefined;
                    }
                    else
                    {
                        tempInt = this.random.Next(101);
                        if (tempInt >= this.snowFactor)
                        {
                            newWS.weatherType = WeatherType.rain;
                        }
                        else
                        {
                            newWS.weatherType = WeatherType.snow;
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
        public void IndexRangeOfWSOverlap(WeatherState ws, out int[] overlapRange, out int relPos)
        {
            lock (lock_WSQueue)
            {
                List<WeatherState> wsQueue = this.weatherStateQueue;
                overlapRange = new int[] { -999, -999 };
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

        public void InsertWeatherState(WeatherState ws)
        {
            lock (lock_WSQueue)
            {
                List<WeatherState> wsQueue = this.weatherStateQueue;

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
                    MakeLogWarning("Prevented insert of already expired WeatherState into "
                        + "the queue (relPos == -1)!");
                    return;
                }
                else if (relPos == -999)
                {
                    MakeLogWarning("Prevented insert of invalid WeatherState into " 
                        + "the queue (relPos == -999)!");
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

        public void CleanupQueue()
        {
            lock (lock_WSQueue)
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

        public void UpdateCurrWeatherState(bool setIGWeather, bool forceIGWeather = false)
        {
            lock (this.lock_WSQueue)
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
                    IGTime igTimeNow = World.NewWorld.GetIGTime();
                    this.lastIGTime = igTimeNow;
                    //Print(">>> " + igTimeNow.day + " " + igTimeNow.hour + " " + igTimeNow.minute);

                    if (this.currWeatherState == null)
                    {
                        World.NewWorld.ChangeWeather(WeatherType.undefined,
                            igTimeNow, new IGTime(23, 59));
                        MakeLog("Updated ingame-weather to the default.");
                    }
                    else
                    {
                        World.NewWorld.ChangeWeather(this.currWeatherState.weatherType,
                            igTimeNow, new IGTime(23, 59));
                        MakeLog("Updated ingame-weather to " + this.currWeatherState.weatherType +
                            ". Description: " + this.currWeatherState.description);
                    }

                    this.currWSChanged = false;
                }
            }
        }

        // outsourced subroutine of InsertWeatherState where is actual queue-manipulation is performed
        protected void OverrideQueue(ref WeatherState ws, ref int[] overlap, ref WSSplitDecision[] indexOverride)
        {
            lock (lock_WSQueue)
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



        public override void Run()
        {
            DateTime dtNow = DateTime.Now;
            IGTime currIGTime = World.NewWorld.GetIGTime();
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
        }

    }
}
