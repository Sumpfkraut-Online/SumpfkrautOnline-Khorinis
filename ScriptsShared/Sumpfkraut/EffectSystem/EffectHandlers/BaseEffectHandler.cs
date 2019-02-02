using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.Destinations;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{

    /// <summary>
    /// Basic EffectHandler which manages Changes made to a host object. Specialized EffectHandler
    /// allow fine-grained control on how the host is accessed.
    /// </summary>
    public partial class BaseEffectHandler : ExtendedObject
    {

        /// <summary>
        /// Map ChangeType to influenced ChangeDestinations.
        /// </summary>
        protected static Dictionary<ChangeType, List<ChangeDestination>> changeTypeToDestinations =
            new Dictionary<ChangeType, List<ChangeDestination>>() { };
        public static Dictionary<ChangeType, List<ChangeDestination>> GetChangeTypeToDestinations ()
        {
            return changeTypeToDestinations;
        }

        /// <summary>
        /// A function used to calculate the TotalChange, thereby transforming its state.
        /// </summary>
        /// <param name="effectHandler"></param>
        /// <param name="totalChange"></param>
        public delegate void CalculateTotalChange (BaseEffectHandler effectHandler, TotalChange totalChange);
        /// <summary>
        /// Maps the ChangeDestination to functions which are used to calculate TotalChanges.
        /// </summary>
        protected static Dictionary<ChangeDestination, CalculateTotalChange> destToCalcTotal =
            new Dictionary<ChangeDestination, CalculateTotalChange>() { };
        /// <summary>
        /// Get Dictionary that maps the ChangeDestination to functions 
        /// which are used to calculate TotalChanges.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<ChangeDestination, CalculateTotalChange> GetDestToCalcTotal ()
        {
            return destToCalcTotal;
        }

        /// <summary>
        /// Function used to apply a TotalChange to the host-object.
        /// </summary>
        /// <param name="effectHandler"></param>
        /// <param name="totalChange"></param>
        public delegate void ApplyTotalChange (BaseEffectHandler effectHandler, TotalChange totalChange);
        protected static Dictionary<ChangeDestination, ApplyTotalChange> destToApplyTotal =
            new Dictionary<ChangeDestination, ApplyTotalChange>() { };
        public static Dictionary<ChangeDestination, ApplyTotalChange> GetDestToApplyTotal ()
        {
            return destToApplyTotal;
        }


        /// <summary>
        /// Lock being used whenever access to the effect management is made.
        /// </summary>
        protected object effectLock;

        /// <summary>
        /// Object that manipulated by this EffectHandler.
        /// </summary>
        protected object host;
        /// <summary>
        /// Object that manipulated by this EffectHandler.
        /// </summary>
        public object Host { get { return host; } }
        /// <summary>
        /// Get object that manipulated by this EffectHandler and cast to correct type.
        /// </summary>
        public T GetHost<T> () { return (T) host; }

        /// <summary>
        /// Returns the classe or interface of the host object, intended by the creator.
        /// </summary>
        protected Type hostType;
        /// <summary>
        /// Returns the classe or interface of the host object.
        /// Due to the creator being able to specify a certain type,
        /// the HostType might transmit more intention than the actual object type.
        /// </summary>
        public Type HostType { get { return hostType; } }

        protected List<Effect> effects;
        /// <summary>
        /// Get all effects registered at this EffectHandler.
        /// </summary>
        /// <returns></returns>
        public List<Effect> GetEffects ()
        {
            lock (effectLock)
            {
                return effects;
            }
        }
        /// <summary>
        /// Tries to retrieve an Effect at the given index-position,
        /// and returns true if successful, false otherwise.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Dictonary used to easily find the subscription date of an Effect.
        /// </summary>
        protected Dictionary<Effect, DateTime> effectToSubDate;
        /// <summary>
        /// Try to find out if and when an Effect was registered on this EffectHandler.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public bool TryGetEffectSubDate (Effect effect, out DateTime sd)
        {
            lock (effectLock)
            {
                return effectToSubDate.TryGetValue(effect, out sd);
            }
        }

        /// <summary>
        /// Dictionary that allows to easily find the TotalChange for its respective ChangeDestination.
        /// </summary>
        protected Dictionary<ChangeDestination, TotalChange> destToTotalChange;
        /// <summary>
        /// Get Dictionary that allows to easily find the TotalChange for its respective ChangeDestination.
        /// </summary>
        /// <returns></returns>
        public Dictionary<ChangeDestination, TotalChange> GetDestToTotalChange () { return destToTotalChange; }
        public bool TryGetTotalChange (ChangeDestination cd, out TotalChange tc)
        {
            lock (effectLock)
            {
                return destToTotalChange.TryGetValue(cd, out tc);
            }
        }
        /// <summary>
        /// Try to retrieve the Change-object which represents the integral of all Changes
        /// related to a ChangeDestination (is part of TotalChange).
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="totalledChange"></param>
        /// <returns></returns>
        public bool TryGetTotal (ChangeDestination cd, out Change totalledChange)
        {
            totalledChange = null;
            lock (effectLock)
            {
                TotalChange tc;
                if (!TryGetTotalChange(cd, out tc)) { return false; }
                totalledChange = tc.GetTotal();
                return totalledChange == null;
            }
        }

        /// <summary>
        /// Dictionary to easily access all Effects that are related to 
        /// the provided ChangeDestination.
        /// </summary>
        protected Dictionary<ChangeDestination, List<Effect>> destToEffects;
        /// <summary>
        /// Get Dictionary to easily access all Effects that are related to 
        /// the provided ChangeDestination.
        /// </summary>
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
            // (only register those which are not already registered beforehand 
            // by a parent class' static constructor)
            PrintStatic(typeof(BaseEffectHandler), 
                "Start subscribing ChangeDestinations and EventHandler...");
            //RegisterDestination(ChangeDestination.Effect_Child);
            RegisterDestination(ChangeDestination.Effect_GlobalID);
            RegisterDestination(ChangeDestination.Effect_Name);
            RegisterDestination(ChangeDestination.Effect_Parent);
            PrintStatic(typeof(BaseEffectHandler), 
                "Finished subscribing ChangeDestinations and EventHandler...");
        }
        
        /// <summary>
        /// Mandatory base constructor crucial for a celan initialization of
        /// all childclass-objects.
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="effects"></param>
        /// <param name="host"></param>
        /// <param name="hostType"></param>
        public BaseEffectHandler (string objName, List<Effect> effects, object host, 
            Type hostType = null)
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



        /// <summary>
        /// Register necessary function for ToalChange calculation and application
        /// </summary>
        /// <param name="cd">ChangeDestination to register to</param>
        /// <returns></returns>
        protected static bool RegisterDestination (ChangeDestination cd)
        {
            DestInitInfo info; 
            List<ChangeDestination> destinations;
            if (!BaseDestInit.TryGetDestInitInfo(cd, out info))
            {
                MakeLogWarningStatic(typeof(BaseEffectHandler), 
                    "Could not register ChangeDestination "
                    + cd + " because there are not entries for it.");
                return false;
            }

            try
            {
                destToCalcTotal.Add(cd, info.CalculateTotalChange);
                destToApplyTotal.Add(cd, info.ApplyTotalChange);

                for (int i = 0; i < info.SupportedChangeTypes.Count; i++)
                {
                    if ((changeTypeToDestinations.TryGetValue(
                            info.SupportedChangeTypes[i], out destinations))
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



        /// <summary>
        /// Returns the permanent Effect at index-position 0.
        /// </summary>
        /// <returns></returns>
        public Effect GetPermanentEffect ()
        {
            lock (effectLock) { return effects[0]; }
        }

        /// <summary>
        /// Reaaply the permanent Effect at index-position 0.
        /// </summary>
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

        /// <summary>
        /// Adds effects to the internal management and recalculate the TotalChanges 
        /// if recalculateTotals is true.
        /// Setting recalculateTotals to false can be used to postpone the 
        /// costly recalculation until all changes are added.
        /// <param name="effects"></param>
        /// <param name="recalcAndApplyTotals"></param>
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

        /// <summary>
        /// Adds an effect to the internal management and recalculate the TotalChanges 
        /// if recalculateTotals is true.
        /// Setting recalculateTotals to false can be used to postpone the 
        /// costly recalculation until all changes are added.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="recalcAndApplyTotals"></param>
        /// <returns></returns>
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
                    MakeLogWarning(string.Format(
                        "Couldn't find ChangeDestinations for Effect: {1}", 
                        effect));
                    return index;
                }

                RecalculateTotals(destinations);
                ReapplyTotals(destinations);
            }

            return index;
        }

        /// <summary>
        /// Remove multiple Effects from the EffectHandler in one go.
        /// This will recalculate all touched TotalChanges and reapply
        /// their influence on the host object.
        /// </summary>
        /// <param name="effects"></param>
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

        /// <summary>
        /// Remove an Effect from this EffectHandler, identifying it by its name.
        /// The previous index position will be returned or -1 if it wasn't found.
        /// As per default, the TotalChange-objects will be recalculated and reapplied.
        /// </summary>
        /// <param name="effectName"></param>
        /// <param name="recalcAndApplyTotals"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove an Effect from this EffectHandler.
        /// The previous index position will be returned or -1 if it wasn't found.
        /// As per default, the TotalChange-objects will be recalculated and reapplied.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="recalcAndApplyTotals"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove an Effect at the provided index position from this EffectHandler.
        /// The previous index position will be returned or -1 if it wasn't found.
        /// As per default, the TotalChange-objects will be recalculated and reapplied.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="recalcAndApplyTotals"></param>
        /// <returns></returns>
        protected int RemoveEffect (int index, bool recalcAndApplyTotals = true)
        {
            Effect effect;
            List<ChangeDestination> destinations;
            lock (effectLock)
            {
                // if effect wasn't there in the first place or permanent index 
                // is targeted --> do nothing
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
        public void AddToTotalChanges (List<Change> changes, DateTime effectSubDate, 
            List<DateTime> changeSubDates)
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
                if (!changeTypeToDestinations.TryGetValue(change.GetChangeType(), 
                    out destinations))
                {
                    return;
                }

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
                            MakeLogError(
                                "Requested non-registered ChangeDestination in AddToTotalChanges: " 
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



        /// <summary>
        /// Get all ChangeDestinations which are used by Changes of the provided Effects.
        /// </summary>
        /// <param name="effects"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all ChangeDestinations which are used by Changes of the provided Effect.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public bool TryGetDestinations (Effect effect, out List<ChangeDestination> destinations)
        {
            return TryGetDestinations(effect.GetChanges(), out destinations);
        }

        /// <summary>
        /// Get all ChangeDestinations which are used by the provided Changes.
        /// </summary>
        /// <param name="changes"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all ChangeDestinations which are used by the provided Change.
        /// </summary>
        /// <param name="change"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public bool TryGetDestinations (Change change, out List<ChangeDestination> destinations)
        {
            return changeTypeToDestinations.TryGetValue(change.GetChangeType(), out destinations);
        }



        /// <summary>
        /// Reevaluate all Changes and calculate TotalChanges from them.
        /// Invoking the method without destinations-parameter causes all Changes
        /// to be reevaluated.
        /// </summary>
        /// <param name="destinations"></param>
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

        /// <summary>
        /// Reevaluate all Change for a given ChangeDestination and calculate the 
        /// TotalChange from them.
        /// </summary>
        /// <param name="destination"></param>
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

        /// <summary>
        /// Reapplies all TotalChanges of the given ChangeDestinations to their respective
        /// host objects. If invoked without the parameter destination, all TotalChanges
        /// will reapplied.
        /// </summary>
        /// <param name="destinations"></param>
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

        /// <summary>
        /// Reapplies all TotalChanges of the given ChangeDestination to their respective
        /// host objects.
        /// </summary>
        /// <param name="destination"></param>
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
