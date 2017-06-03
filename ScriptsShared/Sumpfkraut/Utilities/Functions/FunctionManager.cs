using GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction;
using GUC.Utilities;
using System;
using System.Collections.Generic;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions
{

    public class FunctionManager : ExtendedObject
    {

        new public static readonly string _staticName = "FunctionManager (s)";



        protected object _runLock;
        protected object _bufferLock;

        protected bool isRunning = false;
        public bool IsRunning { get { return isRunning; } }

        // find better infrastructure than a single list (which becomes cpu-costly when growing bigger
            // and when often changed internally) .... !!!!
        protected List<TimedFunction> timedFunctions;

        protected List<IManagerInteraction> buffer;



        public FunctionManager ()
        {
            SetObjName("FunctionManager");
            _runLock = new object();
            _bufferLock = new object();
            timedFunctions = new List<TimedFunction>();
        }



        public void Add (TimedFunction f, bool allowDuplicate)
        {
            lock (_bufferLock)
            {
                buffer.Add(new MI_Add(f, allowDuplicate));
            }
        }

        public void Add (TimedFunction[] f, bool allowDuplicate)
        {
            lock (_bufferLock)
            {
                buffer.Add(new MI_AddRange(f, allowDuplicate));
            }
        }

        public void Clear ()
        {
            lock (_bufferLock)
            {
                buffer.Add(new MI_Clear());
            }
        }

        public void Remove (TimedFunction f, bool removeAll)
        {
            lock (_bufferLock)
            {
                buffer.Add(new MI_Remove(f, removeAll));
            }
        }

        public void Remove (TimedFunction[] f, bool removeAll)
        {
            lock (_bufferLock)
            {
                buffer.Add(new MI_RemoveRange(f, removeAll));
            }
        }

        public void Replace (TimedFunction oldTF, TimedFunction newTF, bool replaceAll)
        {
            lock (_bufferLock)
            {
                buffer.Add(new MI_Replace(oldTF, newTF, replaceAll));
            }
        }

        public void Replace (TimedFunction[] oldTF, TimedFunction[] newTF, bool replaceAll)
        {
            lock (_bufferLock)
            {
                buffer.Add(new MI_ReplaceRange(oldTF, newTF, replaceAll));
            }
        }



        public void IntegrateBuffer ()
        {
            lock (_runLock)
            {
                lock (_bufferLock)
                {
                    foreach (var item in buffer)
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
                    buffer.Clear();
                }
            }
        }

        protected void Buffer_Clear ()
        {
            timedFunctions.Clear();
        }

        protected void Buffer_Add (MI_Add ia)
        {
            if (!(ia.AllowDuplicate && timedFunctions.Contains(ia.TF))) { timedFunctions.Add(ia.TF); }
        }

        protected void Buffer_AddRange (MI_AddRange ia)
        {
            for (int i = 0; i < ia.TF.Length; i++)
            {
                if (!(ia.AllowDuplicate && timedFunctions.Contains(ia.TF[i]))) { timedFunctions.Add(ia.TF[i]); }
            }
        }

        protected void Buffer_Remove (MI_Remove ia)
        {
            if (ia.RemoveAll)
            {
                timedFunctions.RemoveAll((TimedFunction tf) => { return tf == ia.TF; });
            }
            else
            {
                timedFunctions.Remove(ia.TF);
            }
        }

        protected void Buffer_RemoveRange (MI_RemoveRange ia)
        {
            for (int i = 0; i < ia.TF.Length; i++)
            {
                if (ia.RemoveAll)
                {
                    timedFunctions.RemoveAll((TimedFunction tf) => { return tf == ia.TF[i]; });
                }
                else
                {
                    timedFunctions.Remove(ia.TF[i]);
                }
            }
        }

        protected void Buffer_RemoveExceptTimeRange (MI_RemoveExceptTimeRange ia)
        {
            throw new NotImplementedException();    
        }

        protected void Buffer_RemoveInTimeRange (MI_RemoveInTimeRange ia)
        {
            throw new NotImplementedException();
        }

        protected void Buffer_Replace (MI_Replace ia)
        {
            int index;
            if (ia.ReplaceAll)
            {
                do
                {
                    index = timedFunctions.IndexOf(ia.OldTF);
                    if (index > -1) { timedFunctions[index] = ia.NewTF; }
                }
                while (index > -1);
            }
            else
            {
                index = timedFunctions.IndexOf(ia.OldTF);
                if (index > -1) { timedFunctions[index] = ia.NewTF; }
            } 
        }

        protected void Buffer_ReplaceRange (MI_ReplaceRange ia)
        {
            int index;
            int maxLength = ia.OldTF.Length;
            if (ia.OldTF.Length > ia.NewTF.Length) { maxLength = ia.NewTF.Length; }

            for (int o = 0; o < maxLength; o++)
            {
                if (ia.ReplaceAll)
                {
                    do
                    {
                        index = timedFunctions.IndexOf(ia.OldTF[o]);
                        if (index > -1) { timedFunctions[index] = ia.NewTF[o]; }
                    }
                    while (index > -1);
                }
                else
                {
                    index = timedFunctions.IndexOf(ia.OldTF[o]);
                    if (index > -1) { timedFunctions[index] = ia.NewTF[o]; }
                }
            }
        }



        public List<TimedFunction> FindDueFunctions ()
        {
            return FindDueFunctions(DateTime.Now);
        }

        public List<TimedFunction> FindDueFunctions (DateTime now)
        {
            var due = new List<TimedFunction>();

            lock (_runLock)
            {
                foreach (var item in timedFunctions)
                {
                    if (IsDue(item, now)) { due.Add(item); }
                }
            }

            return due;
        }

        public bool IsDue (TimedFunction tf)
        {
            return IsDue(tf, DateTime.Now);
        }

        public bool IsDue (TimedFunction tf, DateTime now)
        {
            if (tf.HasMaxInvocations && (tf.GetInvocations() < tf.GetMaxInvocations())) { return false; }
            if (tf.HasStartEnd)
            {
                if (tf.GetStart() < now) { return false; }
                if (tf.GetEnd() > now) { return false; }
            }

            if (tf.HasSpecifiedTimes)
            {
                foreach (var t in tf.GetSpecifiedTimes()) { if (t >= now) { return true; } }
            }
            if (tf.HasIntervals)
            {
                var lastIntervalIndex = tf.GetLastIntervalIndex();
                var lastIntervalTime = tf.GetLastIntervalTime();
                if ((lastIntervalTime + tf.GetIntervals()[lastIntervalIndex]) <= now) { return true; }
            }

            return false;
        }

        public void InvokeTimedFunctions (List<TimedFunction> tf)
        {
            lock (_runLock)
            {
                foreach (var item in tf)
                {
                    InvokeTimedFunction(item);
                }
            }
        }

        protected void InvokeTimedFunction (TimedFunction tf)
        {
            try
            {
                tf.SetParameters( tf.GetFunc()(tf.GetParameters()) );
                tf.IterateInvocations();
            }
            catch (Exception ex)
            {
                MakeLogError(ex);
            }
        }

        public int RemoveExpiredFunctions ()
        {
            var count = 0;
            Predicate<TimedFunction> predicate = (TimedFunction tf) => { return IsExpired(tf); };

            lock (_runLock) { count = timedFunctions.RemoveAll(predicate); }

            return count;
        }

        public bool IsExpired (TimedFunction tf)
        {
            return IsExpired(tf, DateTime.Now);
        }

        public bool IsExpired (TimedFunction tf, DateTime now)
        {
            bool isOutdated = true;

            if (tf.HasStartEnd && (tf.GetEnd() > now)) { isOutdated = false; }
            else if (tf.HasSpecifiedTimes)
            {
                var specifiedTimes = tf.GetSpecifiedTimes();
                for (int i = 0; i < specifiedTimes.Length; i++)
                {
                    if (specifiedTimes[i] > now) { isOutdated = false; break; }
                }
            }
            else if (tf.HasMaxInvocations && (tf.GetInvocations() < tf.GetMaxInvocations())) { isOutdated = false; }

            return isOutdated;
        }



        public void Start ()
        {
            lock (_runLock)
            {
                isRunning = true;
                Run();
            }
        }

        public void Resume ()
        {
            lock (_runLock) { if (!isRunning) { Start(); } }
        }

        public void Stop ()
        {
            lock (_runLock) { isRunning = false; }
        }

        public void Run ()
        {
            while (isRunning)
            {
                lock (_runLock)
                {
                    lock (_bufferLock) { IntegrateBuffer(); }
                    var due = FindDueFunctions();
                    InvokeTimedFunctions(due);
                    RemoveExpiredFunctions();
                }
            }
        }

    }

}
