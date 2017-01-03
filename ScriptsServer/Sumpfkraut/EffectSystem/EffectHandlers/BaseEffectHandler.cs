using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public abstract class BaseEffectHandler : ExtendedObject
    {

        new public static readonly string _staticName = "EffectHandler (static)";

        // map ChangeType to influenced ChangeDestinations
        public static Dictionary<Enumeration.ChangeType, List<Enumeration.ChangeDestination>> changeTypeToDestinations =
            new Dictionary<Enumeration.ChangeType, List<Enumeration.ChangeDestination>>() { };

        public delegate void CalculateTotal (BaseEffectHandler effectHandler);
        public static Dictionary<Enumeration.ChangeDestination, CalculateTotal> destinationToTotalDelegate =
            new Dictionary<Enumeration.ChangeDestination, CalculateTotal>() { };



        protected object linkedObject;
        public T GetLinkedObject<T> () { return (T) linkedObject; }

        protected Type linkedObjectType;
        public Type LinkedObjectType { get { return linkedObjectType; } }
        public void SetLinkedObjectType (Type linkedObjectType) { this.linkedObjectType = linkedObjectType; }

        protected List<Effect> effects;
        protected Dictionary<string, List<BaseChange>> eventNameToChange;
        protected object effectLock;

        protected Dictionary<Enumeration.ChangeDestination, TotalChange> destinationToTotal;
        public Dictionary<Enumeration.ChangeDestination, TotalChange> DestinationToTotal { get { return destinationToTotal; } }

        protected Dictionary<Enumeration.ChangeDestination, List<Effect>> destinationToEffects;
        public Dictionary<Enumeration.ChangeDestination, List<Effect>> DestinationToEffects{ get { return destinationToEffects; } }



        static BaseEffectHandler ()
        {
            //// map changeTypes common to all EffectHandlers to their respective desitnation(s)
            //changeTypeToDestination.Add(Enumeration.ChangeType.Effect_Name_Set,
            //    new List<Enumeration.ChangeDestination>() { Enumeration.ChangeDestination.Effect_Name });

            //// map methods to calculate TotalChanges common to all EffectHandlers to their respective ChangeDestination
            //destinationToTotalDelegate.Add(Enumeration.ChangeDestination.Effect_Name)

            // no events to subscribe to
        }
        
        // base constructor that must be called for clean initialization
        public BaseEffectHandler (string objName, List<Effect> effects, object linkedObject, Type linkedObjectType = null)
        {
            if (objName == null) { SetObjName("EffectHandler (default)"); }
            else { SetObjName(objName); }

            this.linkedObject = linkedObject;
            this.linkedObjectType = linkedObjectType ?? linkedObject.GetType();

            eventNameToChange = new Dictionary<string, List<BaseChange>>();
            this.effects = effects ?? new List<Effect>();
            this.destinationToTotal = new Dictionary<Enumeration.ChangeDestination, TotalChange>();
            this.destinationToEffects = new Dictionary<Enumeration.ChangeDestination, List<Effect>>();

            // initial sorting

        }



        public void AddEffects (Effect[] effects)
        {
            for (int e = 0; e < effects.Length; e++)
            {
                AddEffect(effects[e], false);
            }
            lock (effectLock)
            {
                // to do
                
            }
        }
        
        // adds effect to the internal management and recalculate the TotalChanges if recalculateTotals is true
        // setting recalculateTotals to false can be used to postpone the costly recalculation until all changes are added
        public int AddEffect (Effect effect, bool recalculateTotals = true)
        {
            int index = -1;
            CalculateTotal calcTotal;
            List<BaseChange> changes;
            List<Enumeration.ChangeDestination> destinations;

            lock (effectLock)
            {
                effects.Add(effect);
                index = effects.Count;
                changes = effect.Changes;

                if (!recalculateTotals) { return index; }

                // calculate new TotalChanges of all affected ChangeDestinations
                for (int c = 0; c < changes.Count; c++)
                {
                    if (!changeTypeToDestinations.TryGetValue(changes[c].ChangeType, out destinations))
                    {
                        MakeLogWarning(string.Format("Couldn't find ChangeDestination for ChangeType "
                            + "{0} while adding Effect: {1}", changes[c].ChangeType, effect));
                        break;
                    }

                    for (int d = 0; d < destinations.Count; d++)
                    {
                        if (!destinationToTotalDelegate.TryGetValue(destinations[d], out calcTotal))
                        {
                            MakeLogWarning(string.Format("Couldn't find CalculateTotal-delegate for ChangeDestination "
                            + "{0} while adding Effect: {1}", destinations[d], effect));
                            break;
                        }
                        calcTotal(this);
                    }
                }

                // apply new TotalChanges
            }

            return index;
        }

        public int RemoveEffect (string effectName)
        {
            int index = -1;
            lock (effectLock)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i].EffectName == effectName)
                    {
                        index = i;
                        ReverseEffect(effects[index]);
                        effects[index].Dispose();
                        effects.RemoveAt(index);
                    }
                }
            }
            return index;
        }

        public int RemoveEffect (Effect effect)
        {
            int index = -1;
            lock (effectLock)
            {
                index = effects.IndexOf(effect);
                if (index > -1)
                {
                    ReverseEffect(effects[index]);
                    effects[index].Dispose();
                    effects.RemoveAt(index);
                }
            }
            return index;
        }

        protected void ApplyEffects (Effect[] effects)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].EffectHandler == this)
                {
                    ApplyEffect(effects[i]);
                }
            }
        }

        virtual protected void ApplyEffect (Effect effect, bool reverse = false)
        {
            throw new NotImplementedException();
        }

        protected void ReverseEffects (Effect[] effects)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].EffectHandler == this)
                {
                    ReverseEffect(effects[i]);
                }
            }
        }

        virtual protected void ReverseEffect (Effect effect)
        {
            ApplyEffect(effect, true);
        }



        public void AddToTotalChanges (BaseChange change)
        {
            List<Enumeration.ChangeDestination> destinations;
            if (!changeTypeToDestinations.TryGetValue(change.ChangeType, out destinations)) { return; }

            for (int d = 0; d < destinations.Count; d++)
            {
                try
                {
                    destinationToTotal[destinations[d]].AddChange(change);
                }
                catch (Exception ex) { Print(ex); }
            }
        }

        //public void AddToTotalChange (BaseChange change, Enumeration.ChangeDestination destination)
        //{
        //    TotalChange totalChange;
        //    if (!destinationToTotal.TryGetValue(destination, out totalChange)) { return; }

        //    totalChange.AddChange(change);
        //}

        public void RemoveFromTotalChanges (BaseChange change)
        {
            List<Enumeration.ChangeDestination> destinations;
            if (!changeTypeToDestinations.TryGetValue(change.ChangeType, out destinations)) { return; }

            for (int d = 0; d < destinations.Count; d++)
            {
                try
                {
                    destinationToTotal[destinations[d]].RemoveChange(change);
                }
                catch (Exception ex) { Print(ex); }
            }
        }



        public bool TryGetDestinations (List<Effect> effects, out List<Enumeration.ChangeDestination> destinations)
        {
            destinations = null;
            List<Enumeration.ChangeDestination> allDestinations = new List<Enumeration.ChangeDestination>();
            List<Enumeration.ChangeDestination> tempDestinations;

            for (int e = 0; e < effects.Count; e++)
            {
                if (TryGetDestinations(effects[e].Changes, out tempDestinations))
                {
                    allDestinations.Union(tempDestinations);
                }
            }

            if (allDestinations.Count < 1) { return false; }
            destinations = allDestinations;
            return true;
        }

        public bool TryGetDestinations (Effect effect, out List<Enumeration.ChangeDestination> destinations)
        {
            return TryGetDestinations(effect.Changes, out destinations);
        }

        public bool TryGetDestinations (List<BaseChange> changes, out List<Enumeration.ChangeDestination> destinations)
        {
            destinations = null;
            List<Enumeration.ChangeDestination> allDestinations = new List<Enumeration.ChangeDestination>();
            List<Enumeration.ChangeDestination> tempDestinations;

            for (int c = 0; c < changes.Count; c++)
            {
                if (TryGetDestinations(changes[c], out tempDestinations))
                {
                    allDestinations.Union(tempDestinations);
                    tempDestinations = null;
                }
            }

            if (allDestinations.Count < 1) { return false; }
            destinations = allDestinations;
            return true;
        }

        public bool TryGetDestinations (BaseChange change, out List<Enumeration.ChangeDestination> destinations)
        {
            return changeTypeToDestinations.TryGetValue(change.ChangeType, out destinations);
        }

    }

}
