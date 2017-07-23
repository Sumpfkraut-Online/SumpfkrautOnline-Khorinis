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

        protected List<VobInst> aiClients;
        public List<VobInst> AIClients { get { return aiClients; } }


        protected BaseAIPersonality aiPersonality;
        public BaseAIPersonality AIPersonality { get { return aiPersonality; } }



        public AIAgent (List<VobInst> aiClients, BaseAIPersonality aiPersonality = null)
        {
            SetObjName("AIAgent");
            this.attributeLock = new object();
            this.aiClients = aiClients ?? new List<VobInst>();
            if (aiPersonality == null)
            {
                this.aiPersonality = new SimpleAIPersonality(0f, 1f);
                this.aiPersonality.Init(new AIMemory(), new SimpleAIRoutine());
            }
            else
            {
                this.aiPersonality = aiPersonality;
            }
        }



        public bool HasAIClient (VobInst aiClient)
        {
            return aiClients.Contains(aiClient);
        }

        public bool HasAIClient (WorldObjects.Vob baseVob)
        {
            bool hasAIClient = false;
            for (int i = 0; i < aiClients.Count; i++)
            {
                if (aiClients[i].BaseInst == baseVob)
                {
                    hasAIClient = true;
                    break;
                }
            }
            return hasAIClient;
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
