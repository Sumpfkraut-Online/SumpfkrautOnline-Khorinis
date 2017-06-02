using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities.Actions
{

    public class ActionManager : ExtendedObject
    {

        new public static readonly string _staticName = "ActionManager (s)";



        protected object runLock;
        protected object bufferLock;

        protected bool isRunning = false;
        public bool IsRunning { get { return isRunning; } }

        protected List<Action> actions;
        protected List<Action> actionBuffer;
        //protected List<List<Action>>



        public ActionManager ()
        {
            SetObjName("ActionManager");
            runLock = new object();
            bufferLock = new object();
            actions = new List<Action>();
        }



        public void AddAction (Action a)
        {
            lock (bufferLock)
            {
                actionBuffer.Add(a);
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
