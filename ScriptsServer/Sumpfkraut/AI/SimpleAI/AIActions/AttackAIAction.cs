using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions
{

    public class AttackAIAction : BaseAIAction
    {

        new public static readonly string _staticName = "AttackAIAction (s)";



        public AttackAIAction (AITarget aiTarget)
            : base(aiTarget)
        {
            SetObjName("AttackAIAction");
            actionType = Enumeration.AiActionType.AttackAIAction;
        }

    }

}
