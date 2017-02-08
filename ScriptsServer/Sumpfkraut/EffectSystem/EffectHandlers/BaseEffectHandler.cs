using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.Destinations;
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
        public static Dictionary<ChangeDestination, CalculateTotalChange> GetDestToCalcTotal ()
        {
            return destToCalcTotal;
        }

        public delegate void ApplyTotalChange (BaseEffectHandler effectHandler);
        protected static Dictionary<ChangeDestination, ApplyTotalChange> destToApplyTotal =
            new Dictionary<ChangeDestination, ApplyTotalChange>() { };
        public static Dictionary<ChangeDestination, ApplyTotalChange> GetDestToApplyTotal ()
        {
            return destToApplyTotal;
        }



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
            RegisterDestination(ChangeDestination.Effect_Name, DestInit_Effect.representative);
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
        }



        //protected static bool RegisterDestination (DestinationInfo info)
        //{
        //    List<ChangeDestination> destinations;
        //    try
        //    {
        //        destToCalcTotal.Add(info.changeDestination, info.calculateTotalChange);
        //        destToApplyTotal.Add(info.changeDestination, info.applyTotalChange);

        //        for (int i = 0; i < info.supportedChangeTypes.Count; i++)
        //        {
        //            if ((changeTypeToDestinations.TryGetValue(info.supportedChangeTypes[i], out destinations))
        //                    && (!destinations.Contains(info.changeDestination)))
        //            {
        //                destinations.Add(ChangeDestination.Effect_Name);
        //            }
        //            else
        //            {
        //                changeTypeToDestinations.Add(info.supportedChangeTypes[i], 
        //                    new List<ChangeDestination>() { info.changeDestination });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MakeLogErrorStatic(typeof(BaseEffectHandler), "Failed to register ChangeDestination " 
        //            + info.changeDestination + " : " + ex);

        //        // clear already reigstered values after unfinished registration
        //        // TO DO

        //        return false;
        //    }

        //    return true;
        //}

        // register necessary function for ToalChange calculation and application
        // cd: ChangeDestination to register to
        // destInit: representative object of the class which should contain the desired TotalChange-functions
        //           (only used to trigger static initialization of the class if still necessary)
        protected static bool RegisterDestination (ChangeDestination cd, BaseDestInit destInitRepresentative)
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

        //protected static void RegisterDestination (Type destType, 
        //    string calcTotalName, string applyTotalName,
        //    bool printNotice)
        //{
        //    ChangeType[] supportedCT;
        //    ChangeDestination cd;
        //    List<ChangeDestination> destinations;
        //    CalculateTotalChange calcTotalChange;
        //    ApplyTotalChange applyTotalChange;

        //    try
        //    {
        //        supportedCT = (ChangeType[]) destType.GetField("supportedChangeTypes").GetValue(null);
        //        cd = (ChangeDestination) destType.GetField("changeDestination").GetValue(null);

        //        calcTotalChange = (CalculateTotalChange) Delegate.CreateDelegate(typeof(CalculateTotalChange), 
        //            destType.GetMethod(calcTotalName));

        //        applyTotalChange = (ApplyTotalChange) Delegate.CreateDelegate(typeof(ApplyTotalChange), 
        //            destType.GetMethod(applyTotalName));

        //        destToCalcTotal.Add(cd, calcTotalChange);
        //        destToApplyTotal.Add(cd, applyTotalChange);

        //        for (int i = 0; i < supportedCT.Length; i++)
        //        {
        //            if ((changeTypeToDestinations.TryGetValue(supportedCT[i], out destinations))
        //                    && (!destinations.Contains(cd)))
        //            {
        //                destinations.Add(ChangeDestination.Effect_Name);
        //            }
        //            else
        //            {
        //                changeTypeToDestinations.Add(supportedCT[i], new List<ChangeDestination>() { cd });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MakeLogErrorStatic(typeof(BaseEffectHandler), "Failed to register destination: " + ex);
        //        //MakeLogErrorStatic(typeof(BaseEffectHandler), "Terminating server start...");
        //    }
        //}



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
        public void AddToTotalChanges (List<Change> changes)
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
        public void AddToTotalChanges (Change change)
        {
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                if (!changeTypeToDestinations.TryGetValue(change.GetChangeType(), out destinations)) { return; }

                for (int d = 0; d < destinations.Count; d++)
                {
                    try
                    {
                        destToTotalChange[destinations[d]].AddChange(change);
                    }
                    catch (Exception ex) { MakeLogError(ex); }
                }
            }
        }

        // perhaps make this protected and adding a slower, less direct method for Effects to use ???
        public void RemoveFromTotalChanges (List<Change> changes)
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
        public void RemoveFromTotalChanges (Change change)
        {
            List<ChangeDestination> destinations;

            lock (effectLock)
            {
                if (!changeTypeToDestinations.TryGetValue(change.GetChangeType(), out destinations)) { return; }

                for (int d = 0; d < destinations.Count; d++)
                {
                    try
                    {
                        destToTotalChange[destinations[d]].RemoveChange(change);
                    }
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
