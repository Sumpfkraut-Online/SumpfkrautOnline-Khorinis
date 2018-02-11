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

        protected object attributeLock;

        protected List<VobInst> aiHosts;
        public List<VobInst> AIHosts { get { return aiHosts; } }


        protected BaseAIPersonality aiPersonality;
        public BaseAIPersonality AIPersonality { get { return aiPersonality; } }



        public AIAgent (List<VobInst> aiClients, BaseAIPersonality aiPersonality = null)
        {
            this.attributeLock = new object();
            this.aiHosts = aiClients ?? new List<VobInst>();
            if (aiPersonality == null)
            {
                this.aiPersonality = new SimpleAIPersonality(0f);
                this.aiPersonality.Init(new AIMemory(), new SimpleAIRoutine());
            }
            else
            {
                this.aiPersonality = aiPersonality;
            }
        }



        public bool HasAIHost (VobInst aiHost)
        {
            return aiHosts.Contains(aiHost);
        }

        public bool HasAIHost(WorldObjects.Vob baseVob)
        {
            bool hasAIHost = false;
            for (int i = 0; i < aiHosts.Count; i++)
            {
                if (aiHosts[i].BaseInst == baseVob)
                {
                    hasAIHost = true;
                    break;
                }
            }
            return hasAIHost;
        }



        public void MakeActiveObservation ()
        {
            lock (attributeLock)
            {
                aiPersonality.MakeActiveObservation(this);
            }
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
                MakeActiveObservation();
                ProcessObservations();
                ProcessActions();
            }
        }

    }

}
