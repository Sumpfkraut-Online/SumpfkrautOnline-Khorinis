using GUC.Scripts.Sumpfkraut.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI
{

    public class AIManager : AbstractRunnable
    {

        new public static readonly string _staticName = "AIManager (static)";

        protected List<AIAgent> aiAgents = new List<AIAgent>();



        public override void Run ()
        {

            for (int i = 0; i < aiAgents.Count; i++)
            {
                aiAgents[i].Act();
            }

        }

    }

}
