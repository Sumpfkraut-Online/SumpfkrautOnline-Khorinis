using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations
{

    public class BeingAttackedObservation : BaseAIObservation
    {

        protected AITarget attackers;
        public AITarget Attackers { get { return attackers; } }


        public BeingAttackedObservation(AITarget attackers)
        {
            this.attackers = attackers;
        }

    }

}
