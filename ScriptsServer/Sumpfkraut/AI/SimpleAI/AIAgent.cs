using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI
{

    public class AIAgent : ExtendedObject
    {

        new public static readonly string _staticName = "AIAgent (static)";



        protected object attributeLock;

        protected List<VobInst> aiClients;
        public List<VobInst> AIClients { get { return aiClients; } }


        protected BaseAIPersonality aiPersonality;
        public BaseAIPersonality AIPersonality { get { return aiPersonality; } }



        public AIAgent (List<VobInst> aiClients, BaseAIPersonality aiPersonality = null)
        {
            SetObjName("AIAgent (default)");
            this.attributeLock = new object();
            this.aiClients = aiClients ?? new List<VobInst>();
            this.aiPersonality = aiPersonality ?? new SimpleAIPersonality(0f);
        }



        public void ProcessActions ()
        {
            lock (attributeLock)
            {
                aiPersonality.ProcessActions(this);
            }
        }

        public void ProcessObservations ()
        {
            lock (attributeLock)
            {
                aiPersonality.ProcessObservations(this);
            }
        }
        
        // general entry-point to act according to all circumstances when called
        public void Act ()
        {
            lock (attributeLock)
            {
                ProcessObservations();
                ProcessActions();
            }
        }

    }

}
