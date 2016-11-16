using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public class Effect : ExtendedObject
    {

        protected EffectHandlers.BaseEffectHandler effectHandler;

        protected List<Change> changes;
        public List<Change> Changes { get { return changes; } }

        protected static string defaultEffectName = "";
        protected string effectName;
        public string EffectName { get { return this.effectName; } }

        protected object changeLock;



        public Effect (EffectHandlers.BaseEffectHandler effectHandler, List<Change> changes = null)
        {
            SetObjName("Effect (default)");
            changeLock = new object();
            this.effectHandler = effectHandler;
            this.changes = changes ?? new List<Change>();
            this.effectName = defaultEffectName;
            ApplyEffectSpecifics(false);
        }



        protected void ApplyEffectSpecifics (bool reverse)
        {
            for (int i = 0; i < changes.Count; i++)
            {
                ApplyEffectSpecifics(changes[i], reverse);
            }
        }

        protected void ApplyEffectSpecifics (Change change, bool reverse)
        {
            switch (change.ChangeType)
            {
                case Enumeration.ChangeType.Effect_Name_Set:
                    object[] parameters = change.Parameters;
                    if (reverse) { this.effectName = defaultEffectName; }
                    else
                    {
                        if (parameters.Length > 0) { effectName = change.Parameters[0].ToString(); }
                    }
                    break;

                default:
                    break;
            }
        }



        public int AddChange (Change change)
        {
            int index = -1;
            lock (changeLock)
            {
                changes.Add(change);
                index = changes.Count;
                ApplyEffectSpecifics(change, false);
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
                    if (changes[i].ChangeType == changeType)
                    {
                        ApplyEffectSpecifics(changes[i], true);
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
            // inform the EffectHandler that there is an effect to be reversed
        }

    }

}
