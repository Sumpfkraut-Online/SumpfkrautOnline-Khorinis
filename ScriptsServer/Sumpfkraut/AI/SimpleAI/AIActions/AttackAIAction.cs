using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions
{

    public class AttackAIAction : BaseAIAction
    {

        public AttackAIAction (AITarget aiTarget)
            : base(aiTarget)
        {
            SetObjName("AttackAIAction");
            actionType = Enumeration.AiActionType.AttackAIAction;
        }

    }

}
