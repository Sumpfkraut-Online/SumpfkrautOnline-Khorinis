using GUC.Scripts.Sumpfkraut.Utilities.Functions.ManagerInteraction;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities.Functions
{

    public class FunctionManager : ExtendedObject
    {

        new public static readonly string _staticName = "FunctionManager (s)";



        protected object runLock;
        protected object bufferLock;

        protected bool isRunning = false;
        public bool IsRunning { get { return isRunning; } }

        protected List<TimedFunction> timedFunctions;
        protected List<IManagerInteraction> buffer;



        public FunctionManager ()
        {
            SetObjName("FunctionManager");
            runLock = new object();
            bufferLock = new object();
            timedFunctions = new List<TimedFunction>();
        }



        public void AddAction (Action a)
        {
            lock (bufferLock)
            {
                
            }
        }

        protected void ProcessBuffer ()
        {
            lock (runLock)
            {
                lock (bufferLock)
                {
                    foreach (var item in buffer)
                    {
                        Type type = item.GetType();
                        if      (type == typeof(MI_Clear)) { Buffer_Clear(); }
                        else if (type == typeof(MI_Add)) { Buffer_Add((MI_Add)item);  }
                        else if (type == typeof(MI_AddRange)) { Buffer_AddRange((MI_AddRange)item); }
                        else if (type == typeof(MI_Remove)) { Buffer_Remove((MI_Remove)item); }
                        else if (type == typeof(MI_RemoveRange)) { Buffer_RemoveRange((MI_RemoveRange)item); }
                        else if (type == typeof(MI_RemoveExceptTimeRange))
                            { Buffer_RemoveExceptTimeRange((MI_RemoveExceptTimeRange)item); }
                        else if (type == typeof(MI_RemoveInTimeRange))
                            { Buffer_RemoveInTimeRange((MI_RemoveInTimeRange)item); }
                        else if (type == typeof(MI_Replace)) { Buffer_Replace((MI_Replace)item); }
                        else if (type == typeof(MI_ReplaceRangel)) { Buffer_ReplaceRange((MI_ReplaceRangel)item); }
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
            timedFunctions.Add(ia.TF);
        }

        protected void Buffer_AddRange (MI_AddRange ia)
        {
            timedFunctions.AddRange(ia.TF);
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

        protected void Buffer_ReplaceRange (MI_ReplaceRangel ia)
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



        public void Start ()
        {
            lock (runLock)
            {
                isRunning = true;
                Run();
            }
        }

        public void Resume ()
        {
            lock (runLock) { if (!isRunning) { Start(); } }
        }

        public void Stop ()
        {
            lock (runLock) { isRunning = false; }
        }

        public void Run ()
        {
            while (isRunning)
            {
                lock (runLock)
                {
                    
                }
            }
        }

    }

}
