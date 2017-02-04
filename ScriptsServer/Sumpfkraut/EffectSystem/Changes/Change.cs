using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class Change : ExtendedObject
    {

        // effect to which this change belongs
        protected Effect effect;
        public Effect Effect { get { return effect; } }
        public void SetEffect (Effect value) { effect = value; }

        protected int changeInitIndex;
        protected BaseChangeInit changeInit;
        public BaseChangeInit ChangeInit;

        protected ChangeType changeType;
        public ChangeType ChangeType { get { return changeType; } }

        public List<Type> GetParameterTypes () { return changeInit.GetParameterTypeLists()[changeInitIndex]; }

        protected List<object> parameters;
        public List<object> Parameters { get { return parameters; } }
        public void SetParameters(List<object> parameters)
        {
            List<Type> pTypes = GetParameterTypes();
            if (!(parameters.Count < pTypes.Count))
            {
                MakeLogWarning("Not enough items in array paramterTypes to describe the types of given paramters.");
                return;
            }

            for (int p = 0; p < pTypes.Count; p++)
            {
                if (parameters[p].GetType() != pTypes[p])
                {
                    MakeLogWarning(String.Format("Parameter {0} of type {1} does not match "
                        + "the required type of {2} in SetParameters.", parameters[p], 
                        parameters[p].GetType(), pTypes[p]));
                    return;
                }
            }

            this.parameters = parameters;
        }



        protected Change (Effect effect, ChangeType changeType, List<object> parameters)
        {
            this.effect = effect;
            this.changeType = changeType;
            this.parameters = parameters;
        }



        public static Change Create (Effect effect, ChangeType changeType, object[] parameters)
        {
            return null;
        }

        public static bool CheckCreateBasics (Effect effect, ChangeType changeType, 
            List<object> parameters, List<Type> types)
        {
            if (effect == null) { return false; }
            if (changeType == ChangeType.Undefined) { return false; }
            if (parameters == null) { return false; }
            if (parameters.Count < types.Count) { return false; }
            for (int t = 0; t < types.Count; t++)
            {
                if (parameters[t].GetType() != types[t]) { return false; }
            }
            return true;
        }



        public override string ToString ()
        {
            List<Type> parameterTypes = GetParameterTypes();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} [effect: {1}, changeType: {2}, parameters: [", GetObjName(), effect, changeType);
            for (int i = 0; i < parameters.Count; i++)
            {
                sb.AppendFormat("{0}: ({1}) {2}, ", i, parameterTypes[i], parameters[i]);
            }
            sb.Append("]]");
            return sb.ToString();
        }

    }

}
