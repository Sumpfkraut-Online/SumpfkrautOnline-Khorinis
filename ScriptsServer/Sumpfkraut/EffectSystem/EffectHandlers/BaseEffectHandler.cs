using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
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
        public static Dictionary<ChangeType, List<ChangeDestination>> changeTypeToDestinations =
            new Dictionary<ChangeType, List<ChangeDestination>>() { };

        public delegate void CalculateTotalChange (BaseEffectHandler effectHandler);
        protected static Dictionary<ChangeDestination, CalculateTotalChange> destToCalcTotal =
            new Dictionary<ChangeDestination, CalculateTotalChange>() { };

        public delegate void ApplyTotalChange (BaseEffectHandler effectHandler);
        protected static Dictionary<ChangeDestination, ApplyTotalChange> destToApplyTotal =
            new Dictionary<ChangeDestination, ApplyTotalChange>() { };



        protected object linkedObject;
        public T GetLinkedObject<T> () { return (T) linkedObject; }

        protected Type linkedObjectType;
        public Type LinkedObjectType { get { return linkedObjectType; } }
        public void SetLinkedObjectType (Type linkedObjectType) { this.linkedObjectType = linkedObjectType; }

        protected List<Effect> effects;
        protected object effectLock;

        public Dictionary<ChangeDestination, TotalChange> destToTotalChange;
        public Dictionary<ChangeDestination, TotalChange> DestToTotalChange { get { return destToTotalChange; } }

        public Dictionary<ChangeDestination, List<Effect>> destToEffects;
        public Dictionary<ChangeDestination, List<Effect>> DestToEffects{ get { return destToEffects; } }



        static BaseEffectHandler ()
        {
            // register all necessary destinations by providing their type
            // (only register those which are not already registered beforehand by a parent class' static constructor)
            RegisterDestination(typeof(Destinations.Dest_Effect_Name), true);
        }
        
        // base constructor that must be called for clean initialization
        public BaseEffectHandler (string objName, List<Effect> effects, object linkedObject, Type linkedObjectType = null)
        {
            if (objName == null) { SetObjName("EffectHandler (default)"); }
            else { SetObjName(objName); }

            this.linkedObject = linkedObject;
            this.linkedObjectType = linkedObjectType ?? linkedObject.GetType();

            this.effects = effects ?? new List<Effect>();
            this.destToTotalChange = new Dictionary<ChangeDestination, TotalChange>();
            this.destToEffects = new Dictionary<ChangeDestination, List<Effect>>();

            // initial sorting
        }



        protected static void RegisterDestinations (List<Type> destTypes, bool printNotice)
        {
            for (int d = 0; d < destTypes.Count; d++)
            {
                RegisterDestination(destTypes[d], printNotice);
            }
        }

        protected static void RegisterDestination (Type destType, bool printNotice)
        {
            ChangeType[] supportedCT;
            ChangeDestination cd;
            List<ChangeDestination> destinations;
            CalculateTotalChange calcTotalChange;
            ApplyTotalChange applyTotalChange;

            try
            {
                supportedCT = (ChangeType[]) destType.GetField("supportedChangeTypes").GetValue(null);
                cd = (ChangeDestination) destType.GetField("changeDestination").GetValue(null);

                calcTotalChange = (CalculateTotalChange) Delegate.CreateDelegate(typeof(CalculateTotalChange), 
                    destType.GetMethod("CalculateTotalChange"));

                applyTotalChange = (ApplyTotalChange) Delegate.CreateDelegate(typeof(ApplyTotalChange), 
                    destType.GetMethod("ApplyTotalChange"));

                destToCalcTotal.Add(cd, calcTotalChange);
                destToApplyTotal.Add(cd, applyTotalChange);

                for (int i = 0; i < supportedCT.Length; i++)
                {
                    if ((changeTypeToDestinations.TryGetValue(supportedCT[i], out destinations))
                            && (!destinations.Contains(cd)))
                    {
                        destinations.Add(ChangeDestination.Effect_Name);
                    }
                    else
                    {
                        changeTypeToDestinations.Add(supportedCT[i], new List<ChangeDestination>() { cd });
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogErrorStatic(typeof(BaseEffectHandler), "Failed to register destination: " + ex);
            }
        }



        public void AddEffects (List<Effect> effects, bool allowDuplicate)
        {
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                for (int e = 0; e < effects.Count; e++)
                {
                    AddEffect(effects[e], allowDuplicate, false);
                }
                
                if (TryGetDestinations(effects, out destinations))
                {
                    RecalculateTotals(destinations);
                    ReapplyTotals(destinations);
                }
            }
        }
        
        // adds effect to the internal management and recalculate the TotalChanges if recalculateTotals is true
        // setting recalculateTotals to false can be used to postpone the costly recalculation until all changes are added
        public int AddEffect (Effect effect, bool allowDuplicate, bool recalcAndApplyTotals = true)
        {
            int index = -1;
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                if ((!allowDuplicate) && (effects.Contains(effect))) { return -1; }
                
                effects.Add(effect);
                index = effects.Count;
                AddToTotalChanges(effect.Changes);

                if (!recalcAndApplyTotals) { return index; }

                if (!TryGetDestinations(effect, out destinations))
                {
                    MakeLogWarning(string.Format("Couldn't find ChangeDestinations for Effect: {1}", effect));
                    return index;
                }

                RecalculateTotals(destinations);
                ReapplyTotals(destinations);
            }

            return index;
        }

        public void RemoveEffects (List<Effect> effects)
        {
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                for (int e = 0; e < effects.Count; e++)
                {
                    RemoveEffect(effects[e]);
                }
                
                if (TryGetDestinations(effects, out destinations))
                {
                    RecalculateTotals(destinations);
                    ReapplyTotals(destinations);
                }
            }
        }

        public int RemoveEffect (string effectName, bool recalcAndApplyTotals = true)
        {
            int index = -1;
            Effect effect = null;
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i].EffectName == effectName)
                    {
                        index = i;
                        effect = effects[index];
                        break;
                    }
                }

                // if effect wasn't there in the first place --> do nothing
                if (index < 0) { return index; }

                if (!recalcAndApplyTotals)
                {
                    RemoveFromTotalChanges(effects[index].Changes);
                    effects[index].Dispose();
                    effects.RemoveAt(index);
                    return index;
                }

                if (!TryGetDestinations(effect, out destinations))
                {
                    MakeLogWarning(string.Format("Couldn't find ChangeDestinations for Effect: {1}", effect));
                    return index;
                }

                RecalculateTotals(destinations);
                ReapplyTotals(destinations);
                RemoveFromTotalChanges(effects[index].Changes);
                effects[index].Dispose();
                effects.RemoveAt(index);
            }
            return index;
        }

        public int RemoveEffect (Effect effect, bool recalcAndApplyTotals = true)
        {
            int index = -1;
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                index = effects.IndexOf(effect);

                if (index < 0) { return index; }

                if (!recalcAndApplyTotals)
                {
                    RemoveFromTotalChanges(effects[index].Changes);
                    effects[index].Dispose();
                    effects.RemoveAt(index);
                }

                if (!TryGetDestinations(effect, out destinations))
                {
                    MakeLogWarning(string.Format("Couldn't find ChangeDestinations for Effect: {1}", effect));
                    return index;
                }

                RecalculateTotals(destinations);
                ReapplyTotals(destinations);
                RemoveFromTotalChanges(effects[index].Changes);
                effects[index].Dispose();
                effects.RemoveAt(index);
            }
            return index;
        }



        //protected void ApplyEffects (List<Effect> effects)
        //{
        //    for (int i = 0; i < effects.Count; i++)
        //    {
        //        if (effects[i].EffectHandler == this)
        //        {
        //            ApplyEffect(effects[i]);
        //        }
        //    }
        //}

        //virtual protected void ApplyEffect (Effect effect, bool reverse = false)
        //{
        //    throw new NotImplementedException();
        //}

        //protected void ReverseEffects (List<Effect> effects)
        //{
        //    for (int i = 0; i < effects.Count; i++)
        //    {
        //        if (effects[i].EffectHandler == this)
        //        {
        //            ReverseEffect(effects[i]);
        //        }
        //    }
        //}

        //virtual protected void ReverseEffect (Effect effect)
        //{
        //    ApplyEffect(effect, true);
        //}



        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void AddToTotalChanges (List<BaseChange> changes)
        {
            lock (effectLock)
            {
                for (int c = 0; c < changes.Count; c++)
                {
                    AddToTotalChanges(changes[c]);
                }
            }
        }
            
        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void AddToTotalChanges (BaseChange change)
        {
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                if (!changeTypeToDestinations.TryGetValue(change.ChangeType, out destinations)) { return; }

                for (int d = 0; d < destinations.Count; d++)
                {
                    try
                    {
                        destToTotalChange[destinations[d]].AddChange(change);
                    }
                    catch (Exception ex) { Print(ex); }
                }
            }
        }

        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void RemoveFromTotalChanges (List<BaseChange> changes)
        {
            lock (effectLock)
            {
                for (int c = 0; c < changes.Count; c++)
                {
                    RemoveFromTotalChanges(changes[c]);
                }
            }
        }

        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void RemoveFromTotalChanges (BaseChange change)
        {
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                if (!changeTypeToDestinations.TryGetValue(change.ChangeType, out destinations)) { return; }

                for (int d = 0; d < destinations.Count; d++)
                {
                    try
                    {
                        destToTotalChange[destinations[d]].RemoveChange(change);
                    }
                    catch (Exception ex) { Print(ex); }
                }
            }
        }



        public bool TryGetDestinations (List<Effect> effects, out List<Enumeration.ChangeDestination> destinations)
        {
            destinations = null;
            List<ChangeDestination> allDestinations = new List<Enumeration.ChangeDestination>();
            List<ChangeDestination> tempDestinations;

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

        public bool TryGetDestinations (Effect effect, out List<ChangeDestination> destinations)
        {
            return TryGetDestinations(effect.Changes, out destinations);
        }

        public bool TryGetDestinations (List<BaseChange> changes, out List<ChangeDestination> destinations)
        {
            destinations = null;
            List<ChangeDestination> allDestinations = new List<ChangeDestination>();
            List<ChangeDestination> tempDestinations;

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

        public bool TryGetDestinations (BaseChange change, out List<ChangeDestination> destinations)
        {
            return changeTypeToDestinations.TryGetValue(change.ChangeType, out destinations);
        }



        public void RecalculateTotals (List<ChangeDestination> destinations)
        {
            lock (effectLock)
            {
                for (int d = 0; d < destinations.Count; d++)
                {
                    RecalculateTotal(destinations[d]);
                }
            }
        }

        public void RecalculateTotal (ChangeDestination destination)
        {
            CalculateTotalChange calcTotal;
            lock (effectLock)
            {
                if (destToCalcTotal.TryGetValue(destination, out calcTotal))
                {
                    calcTotal(this);
                }
            }
        }

        public void ReapplyTotals (List<ChangeDestination> destinations)
        {
            lock (effectLock)
            {
                for (int d = 0; d < destinations.Count; d++)
                {
                    ReapplyTotal(destinations[d]);
                }
            }
        }

        public void ReapplyTotal (ChangeDestination destination)
        {
            ApplyTotalChange applyTotal;
            lock (effectLock)
            {
                if (destToApplyTotal.TryGetValue(destination, out applyTotal))
                {
                    applyTotal(this);
                }
            }
        }

    }

}
