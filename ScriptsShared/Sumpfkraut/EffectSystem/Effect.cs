using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.Utilities;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    /// <summary>
    /// Effects, as central building blocks of the effect system, serve as amanaged container
    /// objects for Changes. Also global effects are collected here statically.
    /// </summary>
    public partial class Effect : ExtendedObject
    {

        /// <summary>
        /// Lock used for synchronized access to global Effects.
        /// </summary>
        protected static object globalLock;

        /// <summary>
        /// Globally / Statically reusable Effects, accessed by unique ids.
        /// </summary>
        protected static Dictionary<string, Effect> globalEffects = new Dictionary<string, Effect>();



        /// <summary>
        /// Lock to synchronize access to Change objects managed by this Effect instance.
        /// </summary>
        protected object changeLock;

        /// <summary>
        /// EffectHandler-instance used to link the Effect with an influenced arbitrary instance
        /// which will be affected by this Effects Change objects.
        /// </summary>
        protected EffectHandlers.BaseEffectHandler effectHandler;
        /// <summary>
        /// Get EffectHandler-instance used to link the Effect with an influenced arbitrary instance
        /// which will be affected by this Effects Change objects.
        /// </summary>
        public EffectHandlers.BaseEffectHandler GetEffectHandler () { return effectHandler; }
        /// <summary>
        /// Set EffectHandler-instance used to link the Effect with an influenced arbitrary instance
        /// which will be affected by this Effects Change objects.
        /// </summary>
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

        /// <summary>
        /// Change objects which determine the actual nature of having this
        /// Effect be applied to an arbitrary instance through an EffectHandler.
        /// </summary>
        protected List<Change> changes;
        /// <summary>
        /// Get Change objects which determine the actual nature of having this
        /// Effect be applied to an arbitrary instance through an EffectHandler.
        /// </summary>
        public List<Change> GetChanges () { lock (changeLock) { return changes; } }

        // dates when changes were subscribed
        protected List<DateTime> changeSubDates;
        public List<DateTime> GetChangeSubDates (){ lock (changeLock) { return changeSubDates; } }

        protected Dictionary<Enumeration.ChangeDestination, List<Change>> changeDestinationToChanges;

        /// <summary>
        /// Standard name to use then no EffectName was provided.
        /// </summary>
        protected static string defaultEffectName = "";
        /// <summary>
        /// Standard name to use then no EffectName was provided.
        /// </summary>
        public static string DefaultEffectName { get { return defaultEffectName; } }

        /// <summary>
        /// Globally/Statically unique id of the Effect object.
        /// </summary>
        protected string globalID;
        /// <summary>
        /// Get globally/statically unique id of the Effect object.
        /// </summary>
        public string GetGlobalID () { return globalID; }
        /// <summary>
        /// Set globally/statically unique id of the Effect object.
        /// </summary>
        public void SetGlobalID (string globalID)
        {
            // remove former global effect
            RemoveGlobalEffect(this.globalID);
            AddGlobalEffect(globalID, this, true);
        }

        /// <summary>
        /// Effects can be part of other Effects which are referred to as parents.
        /// </summary>
        protected List<Effect> parents;
        /// <summary>
        /// Effects can be part of other Effects which are referred to as parents.
        /// </summary>
        public List<Effect> GetParents () { return parents; }
        /// <summary>
        /// Makes this Effect child of another.
        /// </summary>
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
        /// <summary>
        /// Check if this Effect is child of another.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool ContainsParent (Effect parent)
        {
            return parents.Contains(parent);
        }

        /// <summary>
        /// Effects can be composed of other Effects, namely children.
        /// </summary>
        protected List<Effect> children;
        /// <summary>
        /// Effects can be composed of other Effects, namely children.
        /// </summary>
        public List<Effect> GetChildren () { return children; }
        /// <summary>
        /// Makes this Effect parent of another Effect-object.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public bool AddChild (Effect child)
        {
            if (children.Contains(child)) { return true; }
            children.Add(child);

            // if not already done induce AddParent on child as well
            var childParents = child.GetParents();
            if (!childParents.Contains(this)) { child.AddParent(this); }
            return true;
        }
        /// <summary>
        /// Checks if this Effect ist parent of another Effect-object.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public bool ContainsChild (Effect child)
        {
            return children.Contains(child);
        }

        /// <summary>
        /// Humanly readable name of the rather abstract Effect object.
        /// </summary>
        protected string effectName;
        /// <summary>
        /// Get the humanly readable name of the rather abstract Effect object.
        /// </summary>
        public string GetEffectName () { return effectName; }
        /// <summary>
        /// Set the humanly readable name of the rather abstract Effect object.
        /// </summary>
        public void SetEffectName (string effectName) { this.effectName = effectName; }



        public Effect (List<Change> changes = null)
        {
            changeLock = new object();
            this.changes = changes ?? new List<Change>();
            this.effectName = defaultEffectName;
            this.changeDestinationToChanges = new Dictionary<Enumeration.ChangeDestination, List<Change>>();
            this.globalID = null;
            this.parents = new List<Effect>();
            this.children = new List<Effect>();
        }



        /// <summary>
        /// Add a global, unique Effect which can be reused.
        /// </summary>
        /// <param name="globalID"></param>
        /// <param name="effect"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if there already is a global Effect with that uid.
        /// </summary>
        /// <param name="globalID"></param>
        /// <returns></returns>
        public static bool GlobalEffectExists (string globalID)
        {
            lock (globalLock)
            {
                return globalEffects.ContainsKey(globalID);
            }
        }

        /// <summary>
        /// Check if there is globa effect that resembles the provided one.
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static bool GlobalEffectExists (Effect effect)
        {
            lock (globalLock)
            {
                return globalEffects.ContainsValue(effect);
            }
        }

        /// <summary>
        /// Remove an  effect from the global effects.
        /// </summary>
        /// <param name="globalID"></param>
        /// <returns></returns>
        public static bool RemoveGlobalEffect (string globalID)
        {
            lock (globalLock)
            {
                return globalEffects.Remove(globalID);
            }
        }

        /// <summary>
        /// Remove 1 or all siblings of the provided effect from the global effects.
        /// Return the number of successfully removed entries.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="all"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Search for a possibly existing global Effect.
        /// </summary>
        /// <param name="globalID"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static bool TryGetGlobalEffect (string globalID, out Effect effect)
        {
            return globalEffects.TryGetValue(globalID, out effect);
        }



        /// <summary>
        /// Add multiple Changes to this Effect and inform a possible linked EffectHandler.
        /// </summary>
        /// <param name="cl"></param>
        public void AddChanges (List<Change> cl)
        {
            lock (changeLock)
            {
                // add all Changes
                changes.AddRange(cl);

                // add dates of subscription of each Change on this Effect
                var changeSubDates = new List<DateTime>(cl.Count);
                var changeBaseSubDate = DateTime.Now;
                ListUtil.Populate(changeSubDates, changeBaseSubDate);
                for (int i = 0; i < changeSubDates.Count; i++)
                {
                    changeSubDates[i] = new DateTime(changeBaseSubDate.Ticks + i);
                }
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

        /// <summary>
        /// Add a Change to this Effect and inform a possible linked EffectHandler.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove all Changes vom this Effect and inform a possible linked EffectHandler.
        /// This can be used to reset the Changes, rearrange them, et cetera.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Remove a Change from this Effect and inform a possible linked EffectHandler.
        /// </summary>
        /// <param name="changeType"></param>
        /// <returns></returns>
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



        /// <summary>
        /// Destroy all upward references to let the garbage collection take care of the rest.
        /// </summary>
        public void Dispose ()
        {
            if (effectHandler == null) { return; }
            effectHandler.RemoveEffect(this);
            effectHandler = null;
        }

    }

}
