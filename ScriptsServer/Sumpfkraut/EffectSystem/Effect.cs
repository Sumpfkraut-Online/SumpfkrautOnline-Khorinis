using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public class Effect : ExtendedObject
    {

        new public static readonly string _staticName = "Effect (static)";



        protected EffectHandlers.BaseEffectHandler effectHandler;
        public EffectHandlers.BaseEffectHandler EffectHandler { get { return effectHandler; } }

        protected List<Change> changes;
        public List<Change> Changes { get { return changes; } }

        protected Dictionary<Enumeration.ChangeDestination, List<Change>> changeDestinationToChanges;

        protected static string defaultEffectName = "";
        public static string DefaultEffectName { get { return defaultEffectName; } }

        protected object changeLock;

        protected string effectName;
        public string EffectName { get { return this.effectName; } }
        public void SetEffectName (string effectName) { this.effectName = effectName; }



        public Effect (EffectHandlers.BaseEffectHandler effectHandler, List<Change> changes = null)
        {
            SetObjName("Effect (default)");
            changeLock = new object();
            this.effectHandler = effectHandler;
            this.changes = changes ?? new List<Change>();
            this.effectName = defaultEffectName;
            this.changeDestinationToChanges = new Dictionary<Enumeration.ChangeDestination, List<Change>>();
        }



        public int AddChange (Change change)
        {
            int index = -1;
            lock (changeLock)
            {
                changes.Add(change);
                index = changes.Count;
                effectHandler.AddToTotalChanges(change);
            }
            return index;
        }

        public int RemoveChange (Enumeration.ChangeType changeType)
        {
            int index = 0;
            lock (changeLock)
            {
                for (int i = 0; i < changes.Count; i++)
                {
                    if (changes[i].GetChangeType() == changeType)
                    {
                        effectHandler.RemoveFromTotalChanges(changes[i]);
                        index = i;
                        changes.RemoveAt(i);
                        break;
                    }
                }
            }
            return index;
        }

        //public int RemoveChange (Enumeration.ChangeType changeType, int amount = -1)
        //{
        //    int totallyRemoved = 0;
        //    lock (changeLock)
        //    {
        //        totallyRemoved = changes.RemoveAll(change => change.ChangeType == changeType);
        //    }
        //    return totallyRemoved;
        //}



        public void Dispose ()
        {
            // destroy all upward references to let the garbage collection take care of the rest
            effectHandler = null;
        }

    }

}
