using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities
{

    public class SimpleAIPersonality : BaseAIPersonality
    {

        new public static readonly string _staticName = "SimpleAIPersonality (static)";



        protected int awarenessRadius;
        public int AwarenessRadius { get { return this.awarenessRadius; } }
        public void SetAwarenessRadius (int value)
        {
            this.awarenessRadius = value;
        }



        public SimpleAIPersonality ()
        {
            SetObjName("SimpleAIPersonality (default)");
        }



        public override void Init (AIMemory aiMemory, BaseAIRoutine aiRoutine)
        {
            this.aiMemory = aiMemory ?? new AIMemory();
            this.aiRoutine = aiRoutine ?? new SimpleAIRoutine();
        }

        public override void ProcessActions (AIAgent aiAgent)
        {
            List<BaseAIAction> aiActions = aiMemory.GetAIActions();

            // TO DO
        }

        public override void ProcessObservations (AIAgent aiAgent)
        {
            List<BaseAIObservation> aiObservations = aiMemory.GetAIObservations();

            // TO DO
        }

    }

}
