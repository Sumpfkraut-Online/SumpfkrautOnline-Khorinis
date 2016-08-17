using GUC.Scripts.Sumpfkraut.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI
{

    public class AIManager : AbstractRunnable
    {

        new public static readonly string _staticName = "AIManager (static)";

        public static List<AIManager> aiManagers = new List<AIManager>();
        public static List<AIManager> aiManagers_SingleThreaded = new List<AIManager>();

        protected bool useSingleThread = false;
        public void SetUseSingleThread (bool useSingleThread, bool restart = false)
        {
            this.useSingleThread = useSingleThread;
            if (useSingleThread)
            {
                Suspend();
                if (!aiManagers_SingleThreaded.Contains(this)) { aiManagers_SingleThreaded.Add(this); }
            }
            else
            {
                aiManagers_SingleThreaded.Remove(this);
                if (restart) { Start(); }
            }
        }

        protected List<AIAgent> aiAgents = new List<AIAgent>();

        protected object runLock;



        public AIManager (bool useSingleThread)
            : this(useSingleThread, false, TimeSpan.Zero)
        { }

        public AIManager (bool useSingleThread, bool startOnCreate, TimeSpan timeout)
            : base(startOnCreate, timeout, false)
        {
            SetObjName("AIManager (default)");
            runLock = new object();

            SubscribeAIManager();
            SetUseSingleThread(useSingleThread);
        }



        public void SubscribeAIManager ()
        {
            if (!aiManagers.Contains(this)) { aiManagers.Add(this); }
        }

        public void UnsubscribeAIManager ()
        {
            aiManagers.Remove(this);
            aiManagers_SingleThreaded.Remove(this);
        }

        public void SubscribeAIAgent (AIAgent aiAgent)
        {
            if (aiAgents.Contains(aiAgent))
            {
                aiAgents.Add(aiAgent);
            }
        }

        public void UnsubscribeAIAgent (AIAgent aiAgent)
        {
            aiAgents.Remove(aiAgent);
            // removing it this way may throw an exception when iteration in Run-method 
            // reaches this aiAgent
        }



        public static void RunAllSingleThreaded ()
        {
            for (int i = 0; i < aiManagers.Count; i++)
            {
                aiManagers[i].Run();
            }
        }

        public override void Run ()
        {
            try
            {
                lock (runLock)
                {
                    for (int i = 0; i < aiAgents.Count; i++)
                    {
                        aiAgents[i].Act();
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogError(ex);
            }
        }

    }

}
