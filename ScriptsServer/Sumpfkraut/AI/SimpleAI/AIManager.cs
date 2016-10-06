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

        // total number of all AIAgents of all susbsribed AIManagers
        protected static long totalAIAgents = 0;
        public static long TotalAIAgents { get { return totalAIAgents; } }
        // lock for changing the total number of AIAgents of all AIManagers
        protected static object totalAIAgentsLock = new object();

        protected static void AddTotalAgents (long value)
        {
            lock (totalAIAgentsLock) { totalAIAgents += value; }
        }

        // total number of all (in this ai-cycle) simulated AIAgents of all susbsribed susbsribed AIManagers
        protected static long totalSimulated = 0;
        public static long TotalSimulated { get { return totalSimulated; } }
        // lock for changing the total number of (in this ai-cycle) simulated AIAgents of all susbsribed AIManagers
        protected static object totalSimulatedLock = new object();

        protected static void AddTotalSimulated (long value)
        {
            lock (totalSimulatedLock) { totalSimulated += value; }
        }



        protected bool isSubscribed;

        protected bool isActive;
        public bool IsActive { get { return isActive; } }

        protected bool useSingleThread = false;
        public void SetUseSingleThread (bool useSingleThread, bool restart = false)
        {
            this.useSingleThread = useSingleThread;
            if (useSingleThread)
            {
                Suspend();
                if ((isSubscribed) && (!aiManagers_SingleThreaded.Contains(this)))
                {
                    aiManagers_SingleThreaded.Add(this);
                }
            }
            else
            {
                aiManagers_SingleThreaded.Remove(this);
                if (restart) { Start(); }
            }
        }

        protected List<AIAgent> aiAgents;

        // index-ranges of this.aiAgents which have already been simulated in the current ai-cycle
        protected List<Types.Vec2i> simulatedRanges;

        protected object runLock;



        public AIManager (bool useSingleThread)
            : this(useSingleThread, false, TimeSpan.Zero)
        { }

        public AIManager (bool useSingleThread, bool startOnCreate, TimeSpan timeout)
            : base(startOnCreate, timeout, false)
        {
            SetObjName("AIManager (default)");
            runLock = new object();
            aiAgents = new List<AIAgent>();
            isSubscribed = false;

            SubscribeAIManager();
            SetUseSingleThread(useSingleThread);
        }



        public override void Abort ()
        {
            isActive = false;
            if (!useSingleThread) { base.Abort(); }
        }

        public override void Resume ()
        {
            isActive = true;
            if (!useSingleThread) { base.Resume(); }
        }

        public override void Start ()
        {
            isActive = true;
            if (!useSingleThread) { base.Start(); }
        }

        public override void Suspend ()
        {
            isActive = false;
            if (!useSingleThread) { base.Suspend(); }
        }



        public void SubscribeAIManager ()
        {
            if (!aiManagers.Contains(this))
            {
                aiManagers.Add(this);
                if (useSingleThread) { aiManagers_SingleThreaded.Add(this); }
                isSubscribed = true;
                AddTotalAgents(this.aiAgents.Count);
            }
        }

        public void UnsubscribeAIManager ()
        {
            aiManagers.Remove(this);
            aiManagers_SingleThreaded.Remove(this);
            isSubscribed = false;
            AddTotalAgents(-this.aiAgents.Count);
        }

        public void SubscribeAIAgent (AIAgent aiAgent)
        {
            if (!aiAgents.Contains(aiAgent))
            {
                aiAgents.Add(aiAgent);
                if (isSubscribed) { AddTotalAgents(1L); }
            }
        }

        public void UnsubscribeAIAgent (AIAgent aiAgent)
        {
            // removing it this way may throw an exception when iteration in Run-method 
            // reaches this aiAgent
            aiAgents.Remove(aiAgent);
            if (isSubscribed) { AddTotalAgents(-1L); }
        }



        public static void RunAllSingleThreaded ()
        {
            for (int i = 0; i < aiManagers_SingleThreaded.Count; i++)
            {
                if (aiManagers_SingleThreaded[i].IsActive)
                {
                    aiManagers_SingleThreaded[i].Run();
                }
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
