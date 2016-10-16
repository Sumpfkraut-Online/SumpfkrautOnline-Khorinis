using GUC.Scripts.Sumpfkraut.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI
{

    public class AIManager : AbstractRunnable
    {

        new public static readonly string _staticName = "AIManager (static)";

        public static List<AIManager> aiManagers = new List<AIManager>();
        public static List<AIManager> aiManagers_SingleThreaded = new List<AIManager>();

        // total number of all AIAgents of all susbsribed AIManagers
        protected static int totalAIAgents = 0;
        public static int TotalAIAgents { get { return totalAIAgents; } }
        // lock for changing the total number of AIAgents of all AIManagers
        protected static object totalAIAgentsLock = new object();

        protected static void AddTotalAIAgents (int value)
        {
            lock (totalAIAgentsLock)
            {
                totalAIAgents += value;
            }
        }

        // total number of all AIAgents of all susbsribed, singlethreaded AIManagers
        protected static int totalAIAgentsSinglethreaded = 0;
        public static int TotalAIAgentsSinglethreaded { get { return totalAIAgentsSinglethreaded; } }

        protected static void AddTotalAIAgentsSinglethreaded (int value)
        {
            lock (totalAIAgentsLock)
            {
                totalAIAgentsSinglethreaded += value;
            }
        }


        // total number of all (in this ai-cycle) simulated AIAgents of all susbsribed susbsribed AIManagers
        protected static int totalSimulated = 0;
        public static int TotalSimulated { get { return totalSimulated; } }
        // lock for changing the total number of (in this ai-cycle) simulated AIAgents of all susbsribed AIManagers
        protected static object totalSimulatedLock = new object();

        protected static void AddTotalSimulated (int value)
        {
            lock (totalSimulatedLock) { totalSimulated += value; }
        }

        protected static int simulatedAgentDiscrepancy = 0;
        public static int SimulatedAgentDiscrepancy { get { return simulatedAgentDiscrepancy; } }

        protected static int simulationTimeDiscrepancy = 0;
        public static int SimulationTimeDiscrepancy { get { return simulationTimeDiscrepancy; } }

        protected static readonly TimeSpan totalSimulationThreshold = new TimeSpan (0, 0, 10);

        protected static TimeSpan aiCicleTime = new TimeSpan(0, 0, 1);
        public static TimeSpan AICycleTime { get { return aiCicleTime; } }
        public static void SetAICycleTime (TimeSpan value)
        {
            if (value <= TimeSpan.Zero)
            {
                MakeLogWarningStatic(typeof(AIManager), 
                    "Cannot change aiCicleTime to <= TimeSpan.Zero! Using old value...");
            }
            if (value > new TimeSpan(0, 0, 10))
            {
                MakeLogWarningStatic(typeof(AIManager), String.Format(
                    "Long aim for aiCicleTime of {0} detected, exceeding threshold of {1}.",
                    value, totalSimulationThreshold));
            }
            aiCicleTime = value;
        }



        protected bool isSubscribed;

        protected bool isActive;
        public bool IsActive { get { return isActive; } }

        protected bool useSingleThread = false;
        public void SetUseSingleThread (bool useSingleThread, bool restart = false)
        {
            if (useSingleThread)
            {
                Suspend();
                if ((isSubscribed) && (!aiManagers_SingleThreaded.Contains(this)))
                {
                    aiManagers_SingleThreaded.Add(this);
                    
                }
                if (!this.useSingleThread){ AddTotalAIAgentsSinglethreaded(aiAgents.Count); }
            }
            else
            {
                aiManagers_SingleThreaded.Remove(this);
                if (this.useSingleThread){ AddTotalAIAgentsSinglethreaded(-aiAgents.Count); }
                if (restart) { Start(); }
            }

            this.useSingleThread = useSingleThread;
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
                AddTotalAIAgents(this.aiAgents.Count);
                if (useSingleThread)
                {
                    aiManagers_SingleThreaded.Add(this);
                    AddTotalAIAgentsSinglethreaded(this.aiAgents.Count);
                }
                isSubscribed = true;
            }
        }

        public void UnsubscribeAIManager ()
        {
            aiManagers.Remove(this);
            AddTotalAIAgents(-this.aiAgents.Count);
            if (useSingleThread)
            {
                aiManagers_SingleThreaded.Remove(this);
                AddTotalAIAgentsSinglethreaded(-this.aiAgents.Count);
            }
            isSubscribed = false;
        }

        public void SubscribeAIAgent (AIAgent aiAgent)
        {
            if (!aiAgents.Contains(aiAgent))
            {
                aiAgents.Add(aiAgent);
                if (isSubscribed)
                {
                    AddTotalAIAgents(1);
                    if (useSingleThread) { AddTotalAIAgentsSinglethreaded(1); }
                }
            }
        }

        public void UnsubscribeAIAgent (AIAgent aiAgent)
        {
            // removing it this way may throw an exception when iteration in Run-method 
            // reaches this aiAgent
            aiAgents.Remove(aiAgent);
            if (isSubscribed)
            {
                AddTotalAIAgents(-1);
                if (useSingleThread) { AddTotalAIAgentsSinglethreaded(-1); }
            }
        }



        // entry point to run all singlethreaded AIManagers on one thread
        public static void RunAllSinglethreaded ()
        {
            try
            {
                // agents to simulate per servertick (of theoretical length) to fullfill own rate of ai-cicle
                // (note: maybe find a shorter, more elegant version to an equation with multiple type conversions)
                // (sum_agents [] / time_aiCycle [ms]) * (updateRate [ticks] * tps [ticks / ms])
                //  =       simulationRate             *                  time
                int agentsToSimulate = (int) Math.Ceiling(
                    (((double) TotalAIAgentsSinglethreaded) / AICycleTime.TotalMilliseconds)
                    * ((double) Program.UpdateRate / (double) TimeSpan.TicksPerMillisecond)
                    );
                int simNext = agentsToSimulate;

                for (int i = 0; i < aiManagers_SingleThreaded.Count; i++)
                {
                    if (aiManagers_SingleThreaded[i].IsActive)
                    {
                        simNext -= aiManagers_SingleThreaded[i].RunSinglethreaded(simNext);
                        if (simNext <= 0)
                        {
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogErrorStatic(typeof(AIManager), "Exception thrown in method RunAllSinglethreaded:"
                    + ex);
            }
        }

        // standard run method if an AIManagers runs on it's own thread
        public override void Run ()
        {
            int simulatedAgents = 0;
            int agentsToSimulate = 0 + (int)( ((TotalAIAgents - TotalSimulated) / aiManagers.Count) );

            lock (runLock)
            {
                try
                {
                    for (int i = 0; i < aiAgents.Count; i++)
                    {
                        aiAgents[i].Act();
                        simulatedAgents++;
                    }
                }
                catch (Exception ex)
                {
                    MakeLogError(ex);
                }
            }
        }


        // run method for singlethreaded AIManager-instances
        public int RunSinglethreaded (int maxAgents)
        {
            int tempSimulated = 0;
            int loopLimit = maxAgents;
            if (aiAgents.Count < maxAgents) { loopLimit = aiAgents.Count; }

            try
            {
                lock (runLock)
                {
                    for (int i = 0; i < loopLimit; i++)
                    {
                        aiAgents[i].Act();
                        totalSimulated++;
                        tempSimulated++;
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogError(ex);
            }

            return tempSimulated;
        }

        //// Run-method for singlethreaded AIManager-instances
        //public void RunSinglethreaded ()
        //{
        //    int controlAfterAgents = 10;
        //    int tempSimulated = 0;
        //    Stopwatch controlSW = new Stopwatch();
        //    Stopwatch agentSW = new Stopwatch();

        //    lock (runLock)
        //    {
        //        // take the global mean to calculate the milliseconds per simulated agent
        //        // and substract a ceiled value (to be safe) to cope with time discrepancies
        //        int msPerAgent = (((int) AICycleTime.TotalMilliseconds) / TotalAIAgentsSinglethreaded)
        //            -  ((int) Math.Ceiling((float) simulationTimeDiscrepancy / TotalAIAgentsSinglethreaded));
        //        int actualSleepTime = msPerAgent;

        //        try
        //        {
        //            controlSW.Start();
        //            for (int i = 0; i < aiAgents.Count; i++)
        //            {
        //                agentSW.Restart();

        //                aiAgents[i].Act();
        //                totalSimulated++;
        //                tempSimulated++;

        //                // update discrepancy and sleeptime after specified amount of simulated AIAgents
        //                if ((i % controlAfterAgents) == 0)
        //                {
        //                    controlSW.Stop();
        //                    simulatedAgentDiscrepancy += (int) Math.Ceiling(
        //                        ((double) controlSW.ElapsedMilliseconds) / ((double) tempSimulated))
        //                        - msPerAgent;
        //                    // take the global mean to calculate the milliseconds per simulated agent
        //                    // and substract a ceiled value (to be safe) to cope with time discrepancies
        //                    msPerAgent = (((int) AICycleTime.TotalMilliseconds) / TotalAIAgentsSinglethreaded)
        //                        -  ((int) Math.Ceiling((float) simulationTimeDiscrepancy / TotalAIAgentsSinglethreaded));
        //                    tempSimulated = 0;
        //                    controlSW.Restart();
        //                }

        //                agentSW.Stop(); 

        //                actualSleepTime = msPerAgent - ((int) agentSW.ElapsedMilliseconds);
        //                if (actualSleepTime > 0) { System.Threading.Thread.Sleep(msPerAgent); }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MakeLogError(ex);
        //        }
        //    }
        //}

    }

}
