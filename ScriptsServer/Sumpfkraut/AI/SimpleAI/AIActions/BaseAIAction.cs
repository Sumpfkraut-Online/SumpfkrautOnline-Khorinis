using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions
{
    public abstract class BaseAIAction : ExtendedObject
    {

        new public static readonly string _staticName = "BaseAIAction (static)";



        protected object attributeLock;

        //protected Enumeration.AIActionType actionType;
        //public Enumeration.AIActionType ActionType { get { return actionType; } }
        //public void SetActionType (Enumeration.AIActionType value)
        //{
        //    lock (attributeLock)
        //    {
        //        this.actionType = value;
        //    }
        //}

        protected AITarget aiTarget;
        public AITarget AITarget { get { return aiTarget; } }
        public void SetAITarget (AITarget value)
        {
            lock (attributeLock)
            {
                this.aiTarget = value;
            }
        }



        public BaseAIAction (AITarget aiTarget)
        {
            SetObjName("BaseAIAction (default)");
            this.attributeLock = new object();
            //this.actionType = actionType;
            this.aiTarget = aiTarget;
        }

    }
}
