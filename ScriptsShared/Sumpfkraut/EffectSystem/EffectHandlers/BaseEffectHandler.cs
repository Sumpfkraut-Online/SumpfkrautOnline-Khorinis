using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.Destinations;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    public partial class BaseEffectHandler : ExtendedObject
    {

        // map ChangeType to influenced ChangeDestinations
        protected static Dictionary<ChangeType, List<ChangeDestination>> changeTypeToDestinations =
            new Dictionary<ChangeType, List<ChangeDestination>>() { };
        public static Dictionary<ChangeType, List<ChangeDestination>> GetChangeTypeToDestinations ()
        {
            return changeTypeToDestinations;
        }

        public delegate void CalculateTotalChange (BaseEffectHandler effectHandler, TotalChange totalChange);
        protected static Dictionary<ChangeDestination, CalculateTotalChange> destToCalcTotal =
            new Dictionary<ChangeDestination, CalculateTotalChange>() { };
        public static Dictionary<ChangeDestination, CalculateTotalChange> GetDestToCalcTotal ()
        {
            return destToCalcTotal;
        }

        public delegate void ApplyTotalChange (BaseEffectHandler effectHandler, TotalChange totalChange);
        protected static Dictionary<ChangeDestination, ApplyTotalChange> destToApplyTotal =
            new Dictionary<ChangeDestination, ApplyTotalChange>() { };
        public static Dictionary<ChangeDestination, ApplyTotalChange> GetDestToApplyTotal ()
        {
            return destToApplyTotal;
        }



        protected object effectLock;

        // target object of effects
        protected object host;
        public object Host { get { return host; } }
        public T GetHost<T> () { return (T) host; }

        protected Type hostType;
        public Type HostType { get { return hostType; } }
        //public void SetHostType (Type value) { hostType = value; }

        protected List<Effect> effects;
        public List<Effect> GetEffects ()
        {
            lock (effectLock)
            {
                return effects;
            }
        }
        public bool TryGetEffectAtIndex (int index, out Effect effect)
        {
            effect = null;
            lock (effectLock)
            {
                if (index > (effects.Count - 1)) { return false; }
                effect = effects[index];
                return true;
            }
        }

        protected Dictionary<Effect, DateTime> effectToSubDate;
        public bool TryGetEffectSubDate (Effect effect, out DateTime sd)
        {
            lock (effectLock)
            {
                return effectToSubDate.TryGetValue(effect, out sd);
            }
        }

        protected Dictionary<ChangeDestination, TotalChange> destToTotalChange;
        public Dictionary<ChangeDestination, TotalChange> GetDestToTotalChange () { return destToTotalChange; }
        public bool TryGetTotalChange (ChangeDestination cd, out TotalChange tc)
        {
            lock (effectLock)
            {
                return destToTotalChange.TryGetValue(cd, out tc);
            }
        }
        public bool TryGetTotal (ChangeDestination cd, out Change fc)
        {
            fc = null;
            lock (effectLock)
            {
                TotalChange tc;
                if (!TryGetTotalChange(cd, out tc)) { return false; }
                fc = tc.GetTotal();
                if (fc == null) { return false; }
                return true;
            }
        }

        protected Dictionary<ChangeDestination, List<Effect>> destToEffects;
        public Dictionary<ChangeDestination, List<Effect>> GetDestToEffects () { return destToEffects; }
        public bool TryGetEffects (ChangeDestination changeDestination, out List<Effect> effects)
        {
            lock (effectLock)
            {
                return destToEffects.TryGetValue(changeDestination, out effects);
            }
        }



        static BaseEffectHandler ()
        {
            // register all necessary destinations by providing their type
            // (only register those which are not already registered beforehand by a parent class' static constructor)
            PrintStatic(typeof(BaseEffectHandler), "Start subscribing ChangeDestinations and EventHandler...");
            //RegisterDestination(ChangeDestination.Effect_Child);
            RegisterDestination(ChangeDestination.Effect_GlobalID);
            RegisterDestination(ChangeDestination.Effect_Name);
            RegisterDestination(ChangeDestination.Effect_Parent);
            PrintStatic(typeof(BaseEffectHandler), "Finished subscribing ChangeDestinations and EventHandler...");
        }
        
        // base constructor that must be called for clean initialization
        public BaseEffectHandler (string objName, List<Effect> effects, object host, Type hostType = null)
        {
            SetObjName(objName);

            this.host = host;
            if ((hostType != null) || (host != null))
            {
                this.hostType = hostType ?? host.GetType();
            }

            this.effects = effects ?? new List<Effect>();
            this.effectToSubDate = new Dictionary<Effect, DateTime>();
            this.destToTotalChange = new Dictionary<ChangeDestination, TotalChange>();
            this.destToEffects = new Dictionary<ChangeDestination, List<Effect>>();
        }



        // register necessary function for ToalChange calculation and application
        // cd: ChangeDestination to register to
        protected static bool RegisterDestination (ChangeDestination cd)
        {
            DestInitInfo info; 
            List<ChangeDestination> destinations;
            if (!BaseDestInit.TryGetDestInitInfo(cd, out info))
            {
                MakeLogErrorStatic(typeof(BaseEffectHandler), "Could not register ChangeDestination "
                    + cd + " because there are not entries for it.");
                return false;
            }

            try
            {
                destToCalcTotal.Add(cd, info.CalculateTotalChange);
                destToApplyTotal.Add(cd, info.ApplyTotalChange);

                for (int i = 0; i < info.SupportedChangeTypes.Count; i++)
                {
                    if ((changeTypeToDestinations.TryGetValue(info.SupportedChangeTypes[i], out destinations))
                            && (!destinations.Contains(cd)))
                    {
                        destinations.Add(ChangeDestination.Effect_Name);
                    }
                    else
                    {
                        changeTypeToDestinations.Add(info.SupportedChangeTypes[i], 
                            new List<ChangeDestination>() { cd });
                    }
                }
            }
            catch (Exception ex)
            {
                MakeLogErrorStatic(typeof(BaseEffectHandler), "Failed to register ChangeDestination " 
                    + cd + " : " + ex);

                // clear already reigstered values after unfinished registration
                // TO DO

                return false;
            }

            return true;
        }



        public Effect GetPermanentEffect ()
        {
            lock (effectLock) { return effects[0]; }
        }

        public void ReapplyPermanentEffect ()
        {
            lock (effectLock)
            {
                List<ChangeDestination> destinations;
                if (TryGetDestinations(effects[0], out destinations))
                {
                    RecalculateTotals(destinations);
                    ReapplyTotals(destinations);
                }
            }
        }

        public void AddEffects (List<Effect> effects, bool recalcAndApplyTotals = true)
        {
            List<ChangeDestination> destinations;
            lock (effectLock)
            {
                for (int e = 0; e < effects.Count; e++)
                {
                    AddEffect(effects[e], false);
                }
                
                if (recalcAndApplyTotals && TryGetDestinations(effects, out destinations))
                {
                    RecalculateTotals(destinations);
                    ReapplyTotals(destinations);
                }
            }
        }
        
        // adds effect to the internal management and recalculate the TotalChanges if recalculateTotals is true
        // setting recalculateTotals to false can be used to postpone the costly recalculation until all changes are added
        public int AddEffect (Effect effect, bool recalcAndApplyTotals = true)
        {
            int index = -1;
            List<ChangeDestination> destinations;
            lock (effectLock)
            {
                // will be added as new Effect at the end
                if (effects.Contains(effect)) { return -1; }
                effects.Add(effect);
                index = effects.Count;

                // set subscription date for the new Effect
                var subDate = DateTime.Now;
                effectToSubDate.Add(effect, subDate);
                
                AddToTotalChanges(effect.GetChanges(), subDate, effect.GetChangeSubDates());

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
                // first remove effects without recalculating and reapplying the TotalChanges
                for (int e = 0; e < effects.Count; e++)
                {
                    RemoveEffect(effects[e], false);
                }
                
                // recalc and reapply TotalChanges in one swoop                
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
            lock (effectLock)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    if (effects[i].GetEffectName() == effectName)
                    {
                        index = i;
                        break;
                    }
                }

                RemoveEffect(index, recalcAndApplyTotals);
            }

            return index;
        }

        public int RemoveEffect (Effect effect, bool recalcAndApplyTotals = true)
        {
            int index = -1;
            lock (effectLock)
            {
                index = effects.IndexOf(effect);
                RemoveEffect(index, recalcAndApplyTotals);
                //// only remove if it's not the permanent Effect... You can't touch this :)
                //if (index > 0) { RemoveEffect(index, recalcAndApplyTotals); }
            }
            return index;
        }

        protected int RemoveEffect (int index, bool recalcAndApplyTotals = true)
        {
            Effect effect;
            List<ChangeDestination> destinations;
            lock (effectLock)
            {
                // if effect wasn't there in the first place or permanent index is targeted --> do nothing
                if (index < 1) { return -1; }

                effect = effects[index];

                if (!recalcAndApplyTotals)
                {
                    // if no recalculation and reapplicaiton of influenced TotalChanges wanted
                    // => simply remove and dispose
                    RemoveFromTotalChanges(effects[index].GetChanges());
                    effects[index].Dispose();
                    effects.RemoveAt(index);
                    effectToSubDate.Remove(effect);
                    return index;
                }

                if (!TryGetDestinations(effect, out destinations))
                {
                    MakeLogWarning(string.Format("Couldn't find ChangeDestinations for Effect: {1}", effect));
                    return index;
                }

                // remove and dispose to let garbage collect + recalculate and reapply TotalChanges
                RemoveFromTotalChanges(effects[index].GetChanges());
                effect.Dispose();
                effects.RemoveAt(index);
                RecalculateTotals(destinations);
                ReapplyTotals(destinations);
            }
            return index;
        }



        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void AddToTotalChanges (List<Change> changes, DateTime effectSubDate, List<DateTime> changeSubDates)
        {
            lock (effectLock)
            {
                for (int c = 0; c < changes.Count; c++)
                {
                    AddToTotalChanges(changes[c], effectSubDate, changeSubDates[c]);
                }
            }
        }
            
        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void AddToTotalChanges (Change change, DateTime effectSubDate, DateTime changeSubDate)
        {
            List<ChangeDestination> destinations;
            TotalChange tc;

            lock (effectLock)
            {
                if (!changeTypeToDestinations.TryGetValue(change.GetChangeType(), out destinations)) { return; }

                for (int d = 0; d < destinations.Count; d++)
                {
                    if (destToTotalChange.TryGetValue(destinations[d], out tc))
                    {
                        // if a TotalChange already exists, simply add the Change to it
                        tc.AddChange(change, effectSubDate, changeSubDate);
                    }
                    else
                    {
                        // if not present, create the TotalChange and then add the Change to it
                        CalculateTotalChange calcTotal;
                        ApplyTotalChange applyTotal;
                        if (destToCalcTotal.TryGetValue(destinations[d], out calcTotal)
                            && destToApplyTotal.TryGetValue(destinations[d], out applyTotal))
                        {
                            tc = new TotalChange();
                            tc.SetCalcFunction(calcTotal);
                            tc.SetApplyFunction(applyTotal);
                            tc.AddChange(change, effectSubDate, changeSubDate);
                        }
                        else
                        {
                            MakeLogError("Requested non-registered ChangeDestination in AddToTotalChanges: " 
                                + destinations[d]);
                        }    
                    }
                }
            }
        }

        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void RemoveFromTotalChanges (List<Change> changes)
        {
            lock (effectLock)
            {
                foreach (var c in changes) { RemoveFromTotalChanges(c); }
            }
        }

        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void RemoveFromTotalChanges (Change change)
        {
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                if (!changeTypeToDestinations.TryGetValue(change.GetChangeType(), out destinations)) { return; }

                foreach (var d in destinations)
                {
                    try { destToTotalChange[d].RemoveChange(change); }
                    catch (Exception ex) { MakeLogError(ex); }
                }
            }
        }



        public bool TryGetDestinations (List<Effect> effects, out List<ChangeDestination> destinations)
        {
            destinations = null;
            List<ChangeDestination> allDestinations = new List<ChangeDestination>();
            List<ChangeDestination> tempDestinations;

            for (int e = 0; e < effects.Count; e++)
            {
                if (TryGetDestinations(effects[e].GetChanges(), out tempDestinations))
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
            return TryGetDestinations(effect.GetChanges(), out destinations);
        }

        public bool TryGetDestinations (List<Change> changes, out List<ChangeDestination> destinations)
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

        public bool TryGetDestinations (Change change, out List<ChangeDestination> destinations)
        {
            return changeTypeToDestinations.TryGetValue(change.GetChangeType(), out destinations);
        }



        public void RecalculateTotals (List<ChangeDestination> destinations = null)
        {
            lock (effectLock)
            {
                destinations = destinations ?? destToCalcTotal.Keys.ToList();
                for (int d = 0; d < destinations.Count; d++)
                {
                    RecalculateTotal(destinations[d]);
                }
            }
        }

        public void RecalculateTotal (ChangeDestination destination)
        {
            TotalChange tc;
            lock (effectLock)
            {
                if (destToTotalChange.TryGetValue(destination, out tc))
                {
                    tc.GetCalcFunction()(this, tc);
                }
            }
        }

        public void ReapplyTotals (List<ChangeDestination> destinations = null)
        {
            lock (effectLock)
            {
                destinations = destinations ?? destToApplyTotal.Keys.ToList();
                for (int d = 0; d < destinations.Count; d++)
                {
                    ReapplyTotal(destinations[d]);
                }
            }
        }

        public void ReapplyTotal (ChangeDestination destination)
        {
            TotalChange tc;
            lock (effectLock)
            {
                if (destToTotalChange.TryGetValue(destination, out tc))
                {
                    tc.GetApplyFunction()(this, tc);
                }
            }
        }

    }

}
