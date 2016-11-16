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
        protected static bool isInitialized = false;

        

        protected List<Effect> effects;
        protected Dictionary<string, List<Change>> eventNameToChange;
        protected object effectLock;



        // base constructor that must be called for clean initialization
        public BaseEffectHandler (string objName, List<Effect> effects)
        {
            if (objName == null) { SetObjName("EffectHandler (default)"); }
            else { SetObjName(objName); }

            if (!isInitialized)
            {
                this.Init();
                isInitialized = true;
            }

            eventNameToChange = new Dictionary<string, List<Change>>();
            this.effects = effects ?? new List<Effect>();
        }



        public int AddEffect (Effect effect)
        {
            int index = -1;
            lock (effectLock)
            {
                effects.Add(effect);
                index = effects.Count;
            }
            return index;
        }

        public int RemoveEffect (string effectName)
        {
            int index = -1;
            Effect effect = null;
            lock (effectLock)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    List<Change> changes = effects[i].Changes;
                    for (int c = 0; c < changes.Count; c++)
                    {
                        if (changes[c].ChangeType == )
                    }
                }
                if (effects != null) { index = RemoveEffect(effect); }
            }
            return index;
        }

        public int RemoveEffect (Effect effect)
        {
            int index = -1;
            lock (effectLock)
            {
                index = effects.IndexOf(effect);
                if (index > -1) { effects.RemoveAt(index); }
            }
            return index;
        }



        // register all listeners to their respective events in VobInst- or World-classes
        // ( must be defined in every child class! )
        virtual public void Init ()
        {
            throw new NotImplementedException();
        }

    }

}
