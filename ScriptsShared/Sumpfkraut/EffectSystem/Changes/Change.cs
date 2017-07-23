using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class Change : ExtendedObject
    {

        // effect to which this change belongs
        protected Effect effect;
        public Effect GetEffect () { return effect; }
        public void SetEffect (Effect value) { effect = value; }

        protected ChangeInitInfo changeInitInfo;
        public ChangeInitInfo GetChangeInitInfo () { return changeInitInfo; }
        public ChangeType GetChangeType () { return changeInitInfo.ChangeType; }
        public List<Type> GetParameterTypes () { return changeInitInfo.ParameterTypes; }

        protected List<object> parameters;
        public List<object> GetParameters () { return parameters; }
        public bool SetParameters(List<object> parameters)
        {
            List<Type> pTypes = GetParameterTypes();
            //if (pTypes.Count < parameters.Count)
            //{
            //    MakeLogWarning("Not enough items in array paramterTypes to describe the types of all given parameters."
            //        + " Need " + pTypes.Count + " items instead of " + parameters.Count);
            //    return false;
            //}

            for (int p = 0; p < pTypes.Count; p++)
            {
                if (parameters[p].GetType() != pTypes[p])
                {
                    MakeLogWarning(string.Format("Parameter {0} of type {1} does not match "
                        + "the required type of {2} in SetParameters.", parameters[p], 
                        parameters[p].GetType(), pTypes[p]));
                    return false;
                }
            }

            // remove unused, provided parameters
            parameters.RemoveRange(pTypes.Count - 1, parameters.Count - pTypes.Count);
            this.parameters = parameters;
            return true;
        }



        protected Change (ChangeInitInfo changeInitInfo)
        {
            SetObjName("Change");
            this.changeInitInfo = changeInitInfo;
            this.parameters = new List<object>(changeInitInfo.ParameterTypes.Count);
        }

        public static Change Create (ChangeType changeType, List<object> parameters)
        {
            ChangeInitInfo changeInitInfo;
            if (!BaseChangeInit.TryGetChangeInitInfo(changeType, out changeInitInfo))
            {
                MakeLogErrorStatic(typeof(Change), "Aborting Create because there exist no ChangeInitInfo "
                    + "for unsupported changeType " + changeType + "!");
                return null;
            }
            return Create(changeInitInfo, parameters);
        }

        public static Change Create (ChangeInitInfo changeInitInfo, List<object> parameters)
        {
            if (changeInitInfo == null)
            {
                MakeLogWarningStatic(typeof(Change), 
                    "Aborting Create because insufficient changeInitInfo was provided!");
                return null;
            }
            Change change = new Change(changeInitInfo);
            if (!change.SetParameters(parameters))
            {
                MakeLogWarningStatic(typeof(Change), 
                    "Aborting Create because insufficient parameters were provided!");
                return null;
            }
            return change;
        }



        public override string ToString ()
        {
            List<Type> parameterTypes = GetParameterTypes();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} [effect: {1}, changeType: {2}, parameters: [", GetObjName(), effect, 
                changeInitInfo.ChangeType);
            for (int i = 0; i < parameters.Count; i++)
            {
                sb.AppendFormat("{0}: ({1}) {2}, ", i, parameterTypes[i], parameters[i]);
            }
            sb.Append("]]");
            return sb.ToString();
        }

    }

}
