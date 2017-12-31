using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations
{

    public class EnemyAIObservation : BaseAIObservation
    {

        protected AITarget enemies;
        public AITarget Enemies { get { return enemies; } }



        public EnemyAIObservation (AITarget enemies)
        {
            this.enemies = enemies;
        }

    }

}
