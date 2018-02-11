using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities
{

    public abstract class BaseAIPersonality : ExtendedObject
    {

        protected List<BaseAIPersonality> superPersonalities;
        public List<BaseAIPersonality> SuperPersonalities { get { return superPersonalities; } }

        protected AIMemory aiMemory;
        public AIMemory AIMemory { get { return aiMemory; } }

        protected BaseAIRoutine aiRoutine;
        public BaseAIRoutine AIRoutine { get { return aiRoutine; } }

        protected DateTime lastTick;
        public DateTime LastTick { get { return lastTick; } }


        protected BaseAIPersonality ()
        { }



        public void Init (AIMemory aiMemory, BaseAIRoutine aiRoutine, 
            List<BaseAIPersonality> superPersonalities)
        {
            this.superPersonalities = superPersonalities ?? new List<BaseAIPersonality>();
            this.aiMemory = aiMemory ?? new AIMemory();
            this.aiRoutine = aiRoutine ?? new SimpleAIRoutine();
            this.lastTick = DateTime.Now;
        }

        abstract public void MakeActiveObservation (AIAgent aiAgent);
        abstract public void ProcessActions (AIAgent aiAgent);
        abstract public void ProcessObservations (AIAgent aiAgent);
        
    }

}
