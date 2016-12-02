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
        protected Dictionary<string, List<Change>> eventNameToChange;
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

            eventNameToChange = new Dictionary<string, List<Change>>();
            this.effects = effects ?? new List<Effect>();
            this.destinationToTotal = new Dictionary<Enumeration.ChangeDestination, TotalChange>();
            this.destinationToEffects = new Dictionary<Enumeration.ChangeDestination, List<Effect>>();

            // initial sorting

        }



        public int AddEffect (Effect effect)
        {
            int index = -1;
            CalculateTotal calcTotal;
            List<Change> changes;
            List<Enumeration.ChangeDestination> destinations;

            lock (effectLock)
            {
                effects.Add(effect);
                index = effects.Count;
                changes = effect.Changes;

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
                    effects.RemoveAt(index);
                    effects[index].Dispose();
                }
            }
            return index;
        }

        protected void ApplyEffect (Effect effect, bool reverse = false)
        {
            

            //// handle changes relevant for the effect itself before the rest
            //// (define the rest in child classes)
            //List<Change> changes = effect.Changes;
            //for (int c = 0; c < changes.Count; c++)
            //{
            //    switch (changes[c].ChangeType)
            //    {
            //        case Enumeration.ChangeType.Effect_Name_Set:
            //            object[] parameters = changes[c].Parameters;
            //            if (reverse) { effect.SetEffectName(Effect.DefaultEffectName); }
            //            else
            //            {
            //                if (parameters.Length > 0) { effect.SetEffectName(changes[c].Parameters[0].ToString()); }
            //            }
            //            break;

            //        default:
            //            break;
            //    }
            //}

            ApplyEffectInner(effect, reverse);
        }

        virtual protected void ApplyEffectInner (Effect effect, bool reverse = false)
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



        public void AddToTotalChange (Change change)
        {
            //Enumeration.ChangeDestination destination =
        }

        public void AddToTotalChange (Change change, Enumeration.ChangeDestination destination)
        {
            
        }

    }

}
