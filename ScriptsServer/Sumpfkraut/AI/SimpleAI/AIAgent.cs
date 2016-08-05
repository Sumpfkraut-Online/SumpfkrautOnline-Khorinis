using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI
{

    public class AIAgent : ExtendedObject
    {

        new public static readonly string _staticName = "AIAgent (static)";



        protected object attributeLock;

        protected List<object> aiObservations;
        protected List<AIActions.BaseAIAction> aiActions;

        protected int awarenessRadius;
        public int AwarenessRadius { get { return this.awarenessRadius; } }
        public void SetAwarenessRadius (int value)
        {
            this.awarenessRadius = value;
        }



        public AIAgent ()
        {
            SetObjName("AIAgent (default)");
            this.attributeLock = new object();
            this.aiActions = new List<AIActions.BaseAIAction>();
            this.awarenessRadius = 50;
        }



        public void ProcessObservations ()
        {
            lock (attributeLock)
            {
                // TODO
            }
        }

        public void ProcessActions ()
        {
            lock (attributeLock)
            {
                // TODO
            }
        }
        
        // general entry-point to act according to all circumstances when called
        public void Act ()
        {
            lock (attributeLock)
            {
                ProcessObservations();
                ProcessActions();
            }
        }

    }

}
