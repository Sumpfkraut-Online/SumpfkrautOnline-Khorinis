using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.Utilities;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public partial class Effect : ExtendedObject
    {

        new public static readonly string _staticName = "Effect (static)";
        protected static object globalLock;

        protected static Dictionary<string, Effect> globalEffects = new Dictionary<string, Effect>();



        protected object changeLock;

        protected EffectHandlers.BaseEffectHandler effectHandler;
        public EffectHandlers.BaseEffectHandler GetEffectHandler () { return effectHandler; }
        public void SetEffectHandler (EffectHandlers.BaseEffectHandler value)
        {
            lock (changeLock)
            {
                if (effectHandler == value) { return; }
                if (effectHandler != null)
                {
                    effectHandler.RemoveEffect(this);
                }
                effectHandler = value;
                value.AddEffect(this);
            }
        }

        protected List<Change> changes;
        public List<Change> GetChanges () { lock (changeLock) { return changes; } }

        // dates when changes were subscribed
        protected List<DateTime> changeSubDates;
        public List<DateTime> GetChangeSubDates (){ lock (changeLock) { return changeSubDates; } }

        protected Dictionary<Enumeration.ChangeDestination, List<Change>> changeDestinationToChanges;

        protected static string defaultEffectName = "";
        public static string DefaultEffectName { get { return defaultEffectName; } }

        protected string globalID;
        public string GetGlobalID () { return globalID; }
        public void SetGlobalID (string globalID)
        {
            // remove former global effect
            RemoveGlobalEffect(this.globalID);
            AddGlobalEffect(globalID, this, true);
        }

        protected List<Effect> parents;
        public List<Effect> GetParents () { return parents; }
        public bool AddParent (Effect parent)
        {
            if (parents.Contains(parent)) { return true; }
            parents.Add(parent);

            // add the parent's Changes
            AddChanges(parent.GetChanges());

            // if not already done induce AddChild on parent es well
            var parentChildren = parent.GetChildren();
            if (!parentChildren.Contains(this)) { parent.AddChild(this); }
            return true;
        }
        public bool ContainsParent (Effect parent)
        {
            return parents.Contains(parent);
        }

        protected List<Effect> children;
        public List<Effect> GetChildren () { return children; }
        public bool AddChild (Effect child)
        {
            if (children.Contains(child)) { return true; }
            children.Add(child);

            // if not already done induce AddParent on child as well
            var childParents = child.GetParents();
            if (!childParents.Contains(this)) { child.AddParent(this); }
            return true;
        }
        public bool ContainsChild (Effect child)
        {
            return children.Contains(child);
        }

        protected string effectName;
        public string GetEffectName () { return effectName; }
        public void SetEffectName (string effectName) { this.effectName = effectName; }



        public Effect (List<Change> changes = null)
        {
            SetObjName("Effect (default)");
            changeLock = new object();
            this.changes = changes ?? new List<Change>();
            this.effectName = defaultEffectName;
            this.changeDestinationToChanges = new Dictionary<Enumeration.ChangeDestination, List<Change>>();
            this.globalID = null;
            this.parents = new List<Effect>();
            this.children = new List<Effect>();
        }



        public static bool AddGlobalEffect (string globalID, Effect effect, bool replace = true)
        {
            lock (globalLock)
            {
                if (globalEffects.ContainsKey(globalID))
                {
                    if (!replace) { return false; }
                    globalEffects[globalID] = effect;
                    return true;
                }
                globalEffects.Add(globalID, effect);
            }
            return true;
        }

        public static bool GlobalEffectExists (string globalID)
        {
            lock (globalLock)
            {
                return globalEffects.ContainsKey(globalID);
            }
        }

        public static bool GlobalEffectExists (Effect effect)
        {
            lock (globalLock)
            {
                return globalEffects.ContainsValue(effect);
            }
        }

        public static bool RemoveGlobalEffect (string globalID)
        {
            lock (globalLock)
            {
                return globalEffects.Remove(globalID);
            }
        }

        // remove 1 or all siblings of the provided effect from the global effects
        // and return the number of successfully removed entries
        public static int RemoveGlobalEffect (Effect effect, bool all = true)
        {
            var remKeys = new List<string>();
            int failedRemovals = 0;
            lock (globalLock)
            {
                foreach (var keyVal in globalEffects)
                {
                    if ((keyVal.Value == effect))
                    {
                        remKeys.Add(keyVal.Key);
                        if (!all) { break; }
                    }
                }

                int i;
                for (i = 0; i < remKeys.Count; i++)
                {
                    if (!RemoveGlobalEffect(remKeys[i])) { failedRemovals++; }
                }
            }
            return remKeys.Count - failedRemovals;
        }

        public static bool TryGetGlobalEffect (string globalID, out Effect effect)
        {
            return globalEffects.TryGetValue(globalID, out effect);
        }



        public void AddChanges (List<Change> cl)
        {
            lock (changeLock)
            {
                // add all Changes
                changes.AddRange(cl);

                // add dates of subscription of each Change on this Effect
                var changeSubDates = new List<DateTime>(cl.Count);
                ListUtil.Populate(changeSubDates, DateTime.Now);
                this.changeSubDates.AddRange(changeSubDates);

                // use date when the Effect itself was added to it's EffectHandler
                // to inform the TotalChange(s)
                DateTime effectSubDate;
                if (effectHandler != null)
                {
                    if (effectHandler.TryGetEffectSubDate(this, out effectSubDate))
                    {
                        effectHandler.AddToTotalChanges(cl, effectSubDate, changeSubDates);
                    }
                    else
                    {
                        MakeLogError("Effect was not properly subscribed in EffectHandler!");
                    }
                }
            }
        }

        public int AddChange (Change c)
        {
            int index = -1;
            lock (changeLock)
            {
                var changeSubDate = DateTime.Now;
                changes.Add(c);
                index = changes.Count;
                DateTime effectSubDate;
                if (effectHandler != null)
                {
                    if (effectHandler.TryGetEffectSubDate(this, out effectSubDate))
                    {
                        effectHandler.AddToTotalChanges(c, effectSubDate, changeSubDate);
                    }
                    else
                    {
                        MakeLogError("Effect was not properly subscribed in EffectHandler!");
                    }
                }
            }
            return index;
        }

        // remove all Changes vom this Effect
        // (can be used to reset the Changes, rearrange them, etc.)
        public int ClearChanges ()
        {
            int amount;

            lock (changeLock)
            {
                amount = changes.Count;
                if (effectHandler != null) { effectHandler.RemoveFromTotalChanges(changes); }
                changes.Clear();
                changeSubDates.Clear();
            }

            return amount;
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
                        if (effectHandler != null) { effectHandler.RemoveFromTotalChanges(changes[i]); }
                        index = i;
                        changes.RemoveAt(i);
                        changeSubDates.RemoveAt(i);
                        break;
                    }
                }
            }
            return index;
        }



        // destroy all upward references to let the garbage collection take care of the rest
        public void Dispose ()
        {
            if (effectHandler == null) { return; }
            effectHandler.RemoveEffect(this);
            effectHandler = null;
        }

    }

}
