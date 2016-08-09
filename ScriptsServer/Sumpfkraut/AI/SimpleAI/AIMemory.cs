using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI
{

    public class AIMemory : ExtendedObject
    {

        new public static readonly string _staticName = "AIMemory (static)";



        protected object aiActionLock;
        protected object aiObservationLock;

        protected List<BaseAIAction> aiActions;
        protected List<BaseAIObservation> aiObservations;



        public AIMemory ()
        {
            SetObjName("AIMemory (default)");
            this.aiActionLock = new object();
            this.aiObservationLock = new object();
            this.aiActions = new List<BaseAIAction>();
            this.aiObservations = new List<BaseAIObservation>();
        }



        public void AddAIAction (BaseAIAction aiAction)
        {
            lock (aiActionLock)
            {
                aiActions.Add(aiAction);
            }
        }

        public void AddAIActions (IEnumerable<BaseAIAction> newActions)
        {
            lock (aiActionLock)
            {
                aiActions.AddRange(newActions);
            }
        }

        public void AddAIObservation (BaseAIObservation aiObservation)
        {
            lock (aiObservationLock)
            {
                aiObservations.Add(aiObservation);
            }
        }

        public void AddAIObservations (IEnumerable<BaseAIObservation> newObservations)
        {
            lock (aiObservationLock)
            {
                aiObservations.AddRange(newObservations);
            }
        }

        public List<BaseAIAction> GetAIActions ()
        {
            lock (aiActionLock)
            {
                return aiActions;
            }
        }

        public List<BaseAIObservation> GetAIObservations ()
        {
            lock (aiObservationLock)
            {
                return aiObservations;
            }
        }

        public void RemoveAIAction (BaseAIAction aiAction)
        {
            lock (aiActionLock)
            {
                aiActions.Remove(aiAction);
            }
        }

        public void RemoveAIObservation (BaseAIObservation aiObservation)
        {
            lock (aiObservationLock)
            {
                aiObservations.Remove(aiObservation);
            }
        }

        public void SetAIActions (List<BaseAIAction> aiActions)
        {
            lock (aiActionLock)
            {
                this.aiActions = aiActions ?? new List<BaseAIAction>();
            }
        }

        public void SetAIObservations (List<BaseAIObservation> aiObservations)
        {
            lock (aiObservationLock)
            {
                this.aiObservations = aiObservations ?? new List<BaseAIObservation>();
            }
        }

    }

}
