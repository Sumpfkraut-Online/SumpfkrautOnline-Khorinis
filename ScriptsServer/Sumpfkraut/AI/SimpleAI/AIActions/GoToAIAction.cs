using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions
{
    public class GoToAIAction : BaseAIAction
    {

        new public static readonly string _staticName = "AttackAIAction (s)";



        public GoToAIAction (AITarget aiTarget)
            : base(aiTarget)
        {
            SetObjName("GoToAIAction");
            actionType = Enumeration.AiActionType.GoToAIAction;
        }

    }
}
