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

        new public static readonly string _staticName = "BaseAIPersonality (static) ";



        protected AIMemory aiMemory;
        public AIMemory AIMemory { get { return aiMemory; } }

        protected BaseAIRoutine aiRoutine;
        public BaseAIRoutine AIRoutine { get { return aiRoutine; } }

        protected DateTime lastTick;
        public DateTime LastTick { get { return lastTick; } }


        public BaseAIPersonality ()
        {
            SetObjName("BaseAIPersonality (default)");
        }



        abstract public void Init (AIMemory aiMemory, BaseAIRoutine aiRoutine);
        abstract public void MakeActiveObservation (AIAgent aiAgent);
        abstract public void ProcessActions (AIAgent aiAgent);
        abstract public void ProcessObservations (AIAgent aiAgent);
        
    }

}
