using GUC.Scripts.Sumpfkraut.Utilities.Functions.Enumeration;
using GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions
{

    /// <summary>
    /// Class which manages the listing and on-point application of TimedFunctions
    /// which are able to preserve state.
    /// Use it for function calls which are repeateded in intervals or at certain times.
    /// Very useful for lingering ingame status effects likme poisoning.
    /// </summary>
    public class FunctionManager : ExtendedObject
    {

        /// <summary>
        /// Lock which is used when TimedFunctions are processed.
        /// </summary>
        protected object _runLock;
        /// <summary>
        /// Lock which is used when the storageBuffer is accessed.
        /// </summary>
        protected object _bufferLock;

        protected bool isRunning = false;
        /// <summary>
        /// Whether the FunctionManager is active or not at the moment.
        /// </summary>
        public bool IsRunning { get { return isRunning; } }

        /// <summary>
        /// Internal sotrage of all subscribed TimedFunctions with their respective amounts.
        /// </summary>
        protected Dictionary<TimedFunction, int> storage;
        /// <summary>
        /// Buffer used to seperate public access from internal sotrage processes.
        /// </summary>
        protected List<IManagerInteraction> storageBuffer;
        /// <summary>
        /// Ever up-to-date schedule which determines which TimedFunctions are to be
        /// invoked at which times. TimedFunctions are only listed for one point in time
        /// and possible subsequent invocations are deduced one after another.
        /// </summary>
        protected SortedDictionary<DateTime, List<TimedFunction>> schedule;



        public FunctionManager ()
        {
            _runLock = new object();
            _bufferLock = new object();
            storage = new Dictionary<TimedFunction, int>();
            storageBuffer = new List<IManagerInteraction>();
            schedule = new SortedDictionary<DateTime, List<TimedFunction>>();
        }



        /// <summary>
        /// Add a TimedFunction to the query amount-times, possibly ignoring
        /// that the same TimedFunction is already listed.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="amount"></param>
        /// <param name="allowDuplicate"></param>
        public void Add (TimedFunction f, int amount, bool allowDuplicate)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_Add(f, amount, allowDuplicate));
            }
        }

        /// <summary>
        /// Add multiple TimedFunctions to the query amount-times each, possibly ignoring
        /// that the same TimedFunctions are already listed.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="amount"></param>
        /// <param name="allowDuplicate"></param>
        public void Add (TimedFunction[] f, int amount, bool allowDuplicate)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_AddRange(f, amount, allowDuplicate));
            }
        }

        /// <summary>
        /// Removes all TimedFunctions from this FunctionManager.
        /// </summary>
        public void Clear ()
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_Clear());
            }
        }

        /// <summary>
        /// Remove a specified TimedFunction from storage and schedule. Either all
        /// occurrences can be deleted or a certain amount.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="removeAll"></param>
        /// <param name="amount"></param>
        public void Remove (TimedFunction f, bool removeAll, int amount)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_Remove(f, removeAll, amount));
            }
        }

        /// <summary>
        /// Remove specified TimedFunctions from storage and schedule. Either all
        /// occurrences can be deleted or a certain amount each.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="removeAll"></param>
        /// <param name="amount"></param>
        public void Remove (TimedFunction[] f, bool removeAll, int amount)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_RemoveRange(f, removeAll, amount));
            }
        }

        /// <summary>
        /// Replace a specified TimedFunction with another. Either all occurences are converted
        /// or a given amount.
        /// </summary>
        /// <param name="oldTF"></param>
        /// <param name="newTF"></param>
        /// <param name="replaceAll"></param>
        /// <param name="amount"></param>
        public void Replace (TimedFunction oldTF, TimedFunction newTF, bool replaceAll, int amount)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_Replace(oldTF, newTF, replaceAll, amount));
            }
        }

        /// <summary>
        /// Replace specified TimedFunctions with their coutnerparts. Either all occurences are converted
        /// or a given amount.
        /// </summary>
        /// <param name="oldTF"></param>
        /// <param name="newTF"></param>
        /// <param name="replaceAll"></param>
        /// <param name="amount"></param>
        public void Replace (TimedFunction[] oldTF, TimedFunction[] newTF, bool replaceAll, int amount)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_ReplaceRange(oldTF, newTF, replaceAll, amount));
            }
        }



        /// <summary>
        /// Applies all buffered storage access actions (ManagerInteractions).
        /// </summary>
        public void IntegrateBuffer ()
        {
            lock (_runLock)
            {
                lock (_bufferLock)
                {
                    foreach (var item in storageBuffer)
                    {
                        Type type = item.GetType();
                        if      (type == typeof(MI_Clear)) { Buffer_Clear(); }
                        else if (type == typeof(MI_Add)) { Buffer_Add((MI_Add)item); }
                        else if (type == typeof(MI_AddRange)) { Buffer_AddRange((MI_AddRange)item); }
                        else if (type == typeof(MI_Remove)) { Buffer_Remove((MI_Remove)item); }
                        else if (type == typeof(MI_RemoveRange)) { Buffer_RemoveRange((MI_RemoveRange)item); }
                        //else if (type == typeof(MI_RemoveExceptTimeRange))
                        //    { Buffer_RemoveExceptTimeRange((MI_RemoveExceptTimeRange)item); }
                        //else if (type == typeof(MI_RemoveInTimeRange))
                        //    { Buffer_RemoveInTimeRange((MI_RemoveInTimeRange)item); }
                        else if (type == typeof(MI_Replace)) { Buffer_Replace((MI_Replace)item); }
                        else if (type == typeof(MI_ReplaceRange)) { Buffer_ReplaceRange((MI_ReplaceRange)item); }
                    }
                    storageBuffer.Clear();
                }
            }
        }

        /// <summary>
        /// Used to apply buffered clear action (MI_Clear).
        /// </summary>
        protected void Buffer_Clear ()
        {
            storage.Clear();
            schedule.Clear();
        }

        /// <summary>
        /// Used to apply buffered add action.
        /// </summary>
        /// <param name="action"></param>
        protected void Buffer_Add (MI_Add action)
        {
            if (storage.ContainsKey(action.TF))
            {
                storage[action.TF] = storage[action.TF] + action.Amount;
            }
            else
            {
                DateTime nextTime;
                var nextType = action.TF.GetNextInvocationType();
                switch (nextType)
                {
                    case InvocationType.Interval:
                        // only add to storage and schedule if not already expired
                        // iterate to next interval to use the time of the previous one
                        if (action.TF.TryIterateNextInvocation(DateTime.Now) && action.TF.TryGetNextInvocationTime(out nextTime))
                        {
                            storage.Add(action.TF, action.Amount);
                            AddToSchedule(nextTime, action.TF);
                        }
                        break;
                    case InvocationType.SpecifiedTime:
                        if (action.TF.TryGetNextInvocationTime(out nextTime))
                        {
                            storage.Add(action.TF, action.Amount);
                            AddToSchedule(nextTime, action.TF);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Used to apply buffered add range action.
        /// </summary>
        /// <param name="action"></param>
        protected void Buffer_AddRange (MI_AddRange action)
        {
            for (int i = 0; i < action.TF.Length; i++)
            {
                Buffer_Add(new MI_Add(action.TF[i], action.Amount, action.AllowDuplicate));
            }
        }

        /// <summary>
        /// Used to apply buffered remove action.
        /// </summary>
        /// <param name="action"></param>
        protected void Buffer_Remove (MI_Remove action)
        {
            if (action.RemoveAll)
            {
                storage.Remove(action.TF);
            }
            else
            {
                if (storage.ContainsKey(action.TF))
                {
                    storage[action.TF] = storage[action.TF] - action.Amount;
                    if (storage[action.TF] <= 0)
                    {
                        storage.Remove(action.TF);
                        RemoveFromSchedule(action.TF);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Used to apply buffered remove range action.
        /// </summary>
        /// <param name="action"></param>
        protected void Buffer_RemoveRange (MI_RemoveRange action)
        {
            for (int i = 0; i < action.TF.Length; i++)
            {
                Buffer_Remove(new MI_Remove(action.TF[i], action.RemoveAll, action.Amount));
            }
        }

        ///// <summary>
        ///// Used to apply buffered remove action while preserving a specific time range.
        ///// NOT IMPLEMENTED YET.
        ///// </summary>
        ///// <param name="action"></param>
        //protected void Buffer_RemoveExceptTimeRange (MI_RemoveExceptTimeRange action)
        //{
        //    throw new NotImplementedException();    
        //}

        ///// <summary>
        ///// Used to apply buffered remove action while only deleting in a specific time range.
        ///// NOT IMPLEMENTED YET.
        ///// </summary>
        ///// <param name="action"></param>
        //protected void Buffer_RemoveInTimeRange (MI_RemoveInTimeRange action)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Used to apply buffered replace action.
        /// </summary>
        /// <param name="action"></param>
        protected void Buffer_Replace (MI_Replace action)
        {
            int amount = 1;
            if (action.ReplaceAll)
            {
                if (storage.TryGetValue(action.OldTF, out amount))
                {
                    Buffer_Remove(new MI_Remove(action.OldTF, true, -1));
                }
                amount = action.HasAmount ? action.Amount : amount;
                Buffer_Add(new MI_Add(action.NewTF, amount, true));
            }
            else
            {
                amount = action.HasAmount ? action.Amount : 1;
                Buffer_Remove(new MI_Remove(action.OldTF, false, amount));
                Buffer_Add(new MI_Add(action.NewTF, amount, true));
            }
        }

        /// <summary>
        /// Used to apply buffered replace action.
        /// </summary>
        /// <param name="action"></param>
        protected void Buffer_ReplaceRange (MI_ReplaceRange action)
        {
            int maxLength = action.OldTF.Length <= action.NewTF.Length ? action.OldTF.Length : action.NewTF.Length;

            for (int i = 0; i < maxLength; i++)
            {
                Buffer_Replace(new MI_Replace(action.OldTF[i], action.NewTF[i], action.ReplaceAll, action.Amount));
            }
        }



        /// <summary>
        /// Adds a TimedFunction to the time schedule if not already present.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="tf"></param>
        protected void AddToSchedule (DateTime time, TimedFunction tf)
        {
            if (schedule.ContainsKey(time))
            {
                // only add to schedule if not already there
                // (would result in duplicate invocations at the same time otherwise!)
                var timedFunctions = schedule[time];
                if (!timedFunctions.Contains(tf)) { timedFunctions.Add(tf); }
            }
            else
            {
                schedule.Add(time, new List<TimedFunction>() { tf });
            }
        }

        /// <summary>
        /// Removes a TimedFunction from the timed schedule.
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        protected bool RemoveFromSchedule (TimedFunction tf)
        {
            // get copy of all schedule times (keys)
            var times = schedule.Keys.ToArray();
            // iterate and search for the right one
            foreach (var t in times)
            {
                if (RemoveFromSchedule(t, tf)) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Removes a possible TimedFunction-entry in the schedule at the given time.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="tf"></param>
        /// <returns></returns>
        protected bool RemoveFromSchedule (DateTime time, TimedFunction tf)
        {
            List<TimedFunction> timedFunctions;
            if (schedule.TryGetValue(time, out timedFunctions))
            {
                if (timedFunctions.Contains(tf))
                {
                    // possibly remove the schedule entry if it would'nt contain any TimedFunctions anyway
                    if (timedFunctions.Count < 2) { schedule.Remove(time); }
                    else { timedFunctions.Remove(tf); }
                    return true;
                }
            }

            return false;
        }



        /// <summary>
        /// Invoke a TimedFunction and pass it the calculated new state parameters.
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        protected int InvokeFunction (TimedFunction tf)
        {
            int callAmount;
            if (storage.TryGetValue(tf, out callAmount))
            {
                return tf.InvokeFunction(callAmount);
            }
            else { return 0; }
        }



        /// <summary>
        /// Starts the FunctionManager.
        /// It only functions when Run-method is called continuously on a Thread.
        /// </summary>
        public void Start ()
        {
            lock (_runLock) { isRunning = true; }
        }

        /// <summary>
        /// Marks the FunctionManager to be stopped.
        /// Prevents invocation of scheduled TimedFunctions and automated internal management
        /// routines in the Run-method.
        /// </summary>
        public void Stop ()
        {
            lock (_runLock) { isRunning = false; }
        }

        /// <summary>
        /// Central routine which dives automated internal management and invocation of scheduled
        /// TimedFunctions.
        /// Must be called externally on a single thread for the Function Manager to work properly.
        /// </summary>
        public void Run ()
        {
            if (isRunning)
            {
                lock (_runLock)
                {
                    lock (_bufferLock) { IntegrateBuffer(); }

                    KeyValuePair<DateTime, List<TimedFunction>> first;
                    var now = DateTime.Now;

                    // no schedule means no further processing is necessary
                    if (schedule.Count < 1) { return; }
                    // grab the first and thus next list of protocols in the chronologically series
                    // (need it before the loop)
                    first = schedule.First();

                    var lastTime = DateTime.MinValue;
                    // as long as there are schedule entries which are due in respect to their DateTime
                    while ((schedule.Count > 0) && (first.Key <= now))
                    {
                        lastTime = first.Key;
                        // create a copy of the first point in time of the schedule
                        var timedFunctions = first.Value.ToArray();
                        // remove first schedule entry now that we have it's data
                        schedule.Remove(lastTime);

                        DateTime nextTime;
                        foreach (var tf in timedFunctions)
                        {
                            // call the function before any more delay occurs
                            InvokeFunction(tf);

                            if (tf.TryIterateNextInvocation(now) && tf.TryGetNextInvocationTime(out nextTime))
                            {
                                AddToSchedule(nextTime, tf);
                                Print("NEW: " + tf.GetLastInvocationType() + " --- " + schedule.Count 
                                    + " --- " + storage.Count);
                            }
                            else
                            {
                                storage.Remove(tf);
                                Print("TERMINATED: " + tf.GetLastInvocationType() + " --- " + schedule.Count 
                                    + " --- " + storage.Count);
                            }
                        }

                        // get the new first entry of the schedule after removal and possible new addition
                        first = schedule.Count > 0 ? schedule.First() : first;
                    }
                }
            }
        }

    }

}
