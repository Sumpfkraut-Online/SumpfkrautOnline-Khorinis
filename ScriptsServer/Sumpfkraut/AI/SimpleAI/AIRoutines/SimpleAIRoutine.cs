using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines
{

    public class SimpleAIRoutine : BaseAIRoutine
    {

        new public static readonly string _staticName = "SimpleAIRoutine (s)";



        public SimpleAIRoutine ()
        {
            SetObjName("SimpleAIRoutine");
        }



        public override void FollowRoutine (AIAgent aiAgent)
        {
            throw new NotImplementedException();
        }

    }

}
