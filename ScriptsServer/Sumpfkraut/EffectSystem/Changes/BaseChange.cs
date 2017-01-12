using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public abstract class BaseChange : ExtendedObject
    {

        new public static readonly string _staticName = "BaseChange (static)";

        public static readonly ChangeType[] supportedChangeTypes = new ChangeType[] { };
        public static readonly Type[] parameterTypes = new Type[] { };

        // will be filled up automatically by EffectHandlers due to information from initialized Destinations
        public static List<ChangeDestination> influencedDestinations = new List<ChangeDestination>();



        // effect to which this change belongs
        protected Effect effect;
        public Effect Effect { get { return effect; } }
        public void SetEffect (Effect effect) { this.effect = effect; }

        protected ChangeType changeType;
        public ChangeType ChangeType { get { return this.changeType; } }

        protected object[] parameters;
        public object[] Parameters { get { return this.parameters; } }
        public void SetParameters(object[] parameters)
        {
            if (!(parameters.Length <= parameterTypes.Length))
            {
                MakeLogWarning("Not enough items in array paramterTypes to describe the types of given paramters.");
                return;
            }

            for (int p = 0; p < parameterTypes.Length; p++)
            {
                if (parameters[p].GetType() != parameterTypes[p])
                {
                    MakeLogWarning(String.Format("Parameter {0} of type {1} does not match "
                        + "the required type of {2} in SetParameters.", parameters[p], 
                        parameters[p].GetType(), parameterTypes[p]));
                    return;
                }
            }

            this.parameters = parameters;
        }



        protected BaseChange (Effect effect, ChangeType changeType, object[] parameters)
        {
            this.effect = effect;
            this.changeType = changeType;
            this.parameters = parameters;
        }



        public static BaseChange Create (Effect effect, ChangeType changeType, object[] parameters)
        {
            return null;
        }

        public static bool CreateCheckBasics (Effect effect, ChangeType changeType, 
            object[] parameters, Type[] types)
        {
            if (effect == null) { return false; }
            if (changeType == ChangeType.Undefined) { return false; }
            if (parameters == null) { return false; }
            if (parameters.Length < types.Length) { return false; }
            for (int t = 0; t < types.Length; t++)
            {
                if (parameters[t].GetType() != types[t]) { return false; }
            }
            return true;
        }




        public abstract void CalculateTotalChange (BaseEffectHandler effectHandler);

        public override string ToString ()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} [effect: {1}, changeType: {2}, parameters: [", GetObjName(), effect, changeType);
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.AppendFormat("{0}: ({1}) {2}, ", i, ">REPLACE ME<", parameters[i]);
            }
            sb.Append("]]");
            return sb.ToString();
        }

    }

}
