using GUC.Scripts.Sumpfkraut.Utilities.Functions.Enumeration;
using GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions
{

    public class FunctionManager : ExtendedObject
    {

        protected object _runLock;
        protected object _bufferLock;

        protected bool isRunning = false;
        public bool IsRunning { get { return isRunning; } }

        protected Dictionary<TimedFunction, int> storage;
        protected List<IManagerInteraction> storageBuffer;
        protected SortedDictionary<DateTime, List<TimedFunction>> schedule;



        public FunctionManager ()
        {
            _runLock = new object();
            _bufferLock = new object();
            storage = new Dictionary<TimedFunction, int>();
            storageBuffer = new List<IManagerInteraction>();
            schedule = new SortedDictionary<DateTime, List<TimedFunction>>();
        }



        public void Add (TimedFunction f, int amount, bool allowDuplicate)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_Add(f, amount, allowDuplicate));
            }
        }

        public void Add (TimedFunction[] f, int amount, bool allowDuplicate)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_AddRange(f, amount, allowDuplicate));
            }
        }

        public void Clear ()
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_Clear());
            }
        }

        public void Remove (TimedFunction f, bool removeAll, int amount)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_Remove(f, removeAll, amount));
            }
        }

        public void Remove (TimedFunction[] f, bool removeAll, int amount)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_RemoveRange(f, removeAll, amount));
            }
        }

        public void Replace (TimedFunction oldTF, TimedFunction newTF, bool replaceAll, int amount)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_Replace(oldTF, newTF, replaceAll, amount));
            }
        }

        public void Replace (TimedFunction[] oldTF, TimedFunction[] newTF, bool replaceAll, int amount)
        {
            lock (_bufferLock)
            {
                storageBuffer.Add(new MI_ReplaceRange(oldTF, newTF, replaceAll, amount));
            }
        }



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

        protected void Buffer_Clear ()
        {
            storage.Clear();
            schedule.Clear();
        }

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

        protected void Buffer_AddRange (MI_AddRange action)
        {
            for (int i = 0; i < action.TF.Length; i++)
            {
                Buffer_Add(new MI_Add(action.TF[i], action.Amount, action.AllowDuplicate));
            }
        }

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

        protected void Buffer_RemoveRange (MI_RemoveRange action)
        {
            for (int i = 0; i < action.TF.Length; i++)
            {
                Buffer_Remove(new MI_Remove(action.TF[i], action.RemoveAll, action.Amount));
            }
        }

        protected void Buffer_RemoveExceptTimeRange (MI_RemoveExceptTimeRange action)
        {
            throw new NotImplementedException();    
        }

        protected void Buffer_RemoveInTimeRange (MI_RemoveInTimeRange action)
        {
            throw new NotImplementedException();
        }

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

        protected void Buffer_ReplaceRange (MI_ReplaceRange action)
        {
            int maxLength = action.OldTF.Length <= action.NewTF.Length ? action.OldTF.Length : action.NewTF.Length;

            for (int i = 0; i < maxLength; i++)
            {
                Buffer_Replace(new MI_Replace(action.OldTF[i], action.NewTF[i], action.ReplaceAll, action.Amount));
            }
        }



        // only use in internally in Run-method
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

        // only use in internally in Run-method
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

        // only use in internally in Run-method
        protected bool RemoveFromSchedule (DateTime time, TimedFunction tf)
        {
            List<TimedFunction> timedFunctions;
            if (schedule.TryGetValue(time, out timedFunctions))
            {
                // possibly remove the schedule entry if it would'nt contain any TimedFunctions anyway
                if (timedFunctions.Count < 2) { schedule.Remove(time); Print("GOTCHA"); }
                else { timedFunctions.Remove(tf); }
                return true;
            }

            return false;
        }



        // only use in internally in Run-method
        protected int InvokeFunction (TimedFunction tf)
        {
            int callAmount;
            if (storage.TryGetValue(tf, out callAmount))
            {
                return tf.InvokeFunction(callAmount);
            }
            else { return 0; }
        }



        public void Start ()
        {
            lock (_runLock) { isRunning = true; }
        }

        public void Stop ()
        {
            lock (_runLock) { isRunning = false; }
        }

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
