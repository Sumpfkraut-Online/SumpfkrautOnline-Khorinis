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
        protected List<BaseAIAction> aiActions;
        protected Dictionary<Type, int> indexByActionType;

        protected object aiObservationLock;
        protected List<BaseAIObservation> aiObservations;
        protected Dictionary<Type, int> indexByObservationType;



        public AIMemory ()
        {
            SetObjName("AIMemory (default)");
            this.aiActionLock = new object();
            this.aiActions = new List<BaseAIAction>();
            this.indexByActionType = new Dictionary<Type, int>();

            this.aiObservationLock = new object();
            this.aiObservations = new List<BaseAIObservation>();
            this.indexByObservationType = new Dictionary<Type, int>();
        }



        public void AddAIAction (BaseAIAction aiAction)
        {
            lock (aiActionLock)
            {
                int index;
                if (indexByActionType.TryGetValue(aiAction.GetType(), out index))
                {
                    aiActions.Insert(index, aiAction);
                }
                else { aiActions.Add(aiAction); }
            }
        }

        public void AddAIActions (List<BaseAIAction> newActions)
        {
            for (int i = 0; i < newActions.Count; i++)
            {
                AddAIAction(newActions[i]);
            }
        }

        public void AddAIObservation (BaseAIObservation aiObservation)
        {
            int index;
            if (indexByObservationType.TryGetValue(aiObservation.GetType(), out index))
            {
                aiObservations.Insert(index, aiObservation);
            }
            else { aiObservations.Add(aiObservation); }
        }

        public void AddAIObservations (List<BaseAIObservation> newObservations)
        {
            for (int i = 0; i < newObservations.Count; i++)
            {
                AddAIObservation(newObservations[i]);
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
