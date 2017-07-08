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
        protected SortedDictionary<DateTime, List<ScheduleProtocol>> schedule;



        public FunctionManager ()
        {
            SetObjName("FunctionManager");
            _runLock = new object();
            _bufferLock = new object();
            storage = new Dictionary<TimedFunction, int>();
            storageBuffer = new List<IManagerInteraction>();
            schedule = new SortedDictionary<DateTime, List<ScheduleProtocol>>();
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
                        else if (type == typeof(MI_Add)) { Buffer_Add((MI_Add)item);  }
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
        }

        protected void Buffer_Add (MI_Add action)
        {
            if (storage.ContainsKey(action.TF)) { storage[action.TF] = storage[action.TF] + action.Amount; }
            else                                { storage.Add(action.TF, action.Amount); }
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
                        Buffer_Remove(new MI_Remove(action.TF, action.RemoveAll, action.Amount));
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



        protected bool TryCreateNextProtocol (DateTime referenceTime, ScheduleProtocol old, out ScheduleProtocol next)
        {
            DateTime nextTime;
            next = new ScheduleProtocol();
            var preserveExpired = old.TF.GetPreserveDueInvocations();
            var callAmount = 0;

            // detect max invocations
            if (old.TF.HasMaxInvocations && (old.TF.GetInvocations() >= old.TF.GetMaxInvocations())) { return false; }

            // determine possible next specified time
            old.TF.TryGetNextSpecifiedTime(out nextTime);

            // detect interval
            TimeSpan lastInterval;
            if (old.TF.TryGetLastInterval(out lastInterval))
            {
                
            }

            next = new ScheduleProtocol(old.TF, callAmount);
            return true;
        }

        protected int InvokeProtocol (ScheduleProtocol protocol)
        {
            int invokes = 0;

            try
            {
                var tf = protocol.TF;
                for (int i = 0; i < protocol.CallAmount; i++)
                {
                    tf.SetParameters( tf.GetFunc()(tf.GetParameters()) );
                    tf.IterateInvocations(1);
                    invokes++;
                }
            }
            catch (Exception ex)
            {
                MakeLogError(ex);
            }

            return invokes;
        }

        public bool IsExpired (TimedFunction tf)
        {
            return IsExpired(tf, DateTime.Now);
        }

        public bool IsExpired (TimedFunction tf, DateTime now)
        {
            bool isExpired = true;

            if (tf.HasStartEnd && (tf.GetEnd() > now)) { isExpired = false; }
            else if (tf.HasSpecifiedTimes)
            {
                var specifiedTimes = tf.GetSpecifiedTimes();
                for (int i = 0; i < specifiedTimes.Length; i++)
                {
                    if (specifiedTimes[i] > now) { isExpired = false; break; }
                }
            }
            else if (tf.HasMaxInvocations && (tf.GetInvocations() < tf.GetMaxInvocations())) { isExpired = false; }

            return isExpired;
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

                    KeyValuePair<DateTime, List<ScheduleProtocol>> first;
                    ScheduleProtocol newProtocol;

                    var now = DateTime.Now;
                    if ((schedule.Count < 1) && (schedule.First().Key > now)) { return; }

                    do
                    {
                        // grab the first and thus next list of protocols in the chronologically series
                        first = schedule.First();
                        // remove first schedule entry now that we have it's data
                        schedule.Remove(first.Key);
                        // create a copy of the first point in time of the schedule
                        var protocols = first.Value.ToArray();

                        foreach (var oldProtocol in protocols)
                        {
                            InvokeProtocol(oldProtocol);
                            // get possible protocol that should follow the old one
                            // and add it to the schedule
                            if (TryCreateNextProtocol(now, oldProtocol, out newProtocol))
                            {
                                // add possible new protocol to the schedule
                                Buffer_Add(new MI_Add(newProtocol.TF, newProtocol.CallAmount, true));
                            }
                        }
                    }
                    while ((schedule.Count > 0) && (first.Key <= now));
                }
            }
        }

    }

}
