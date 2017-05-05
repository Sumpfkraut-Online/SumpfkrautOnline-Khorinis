using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations
{

    public class EnemyAIObservation : BaseAIObservation
    {

        new public static readonly string _staticName = "EnemyAIOberservation (s)";



        public EnemyAIObservation (AITarget aiTarget)
            : base(aiTarget)
        {
            SetObjName("EnemyAIObservation");
        }

    }

}
