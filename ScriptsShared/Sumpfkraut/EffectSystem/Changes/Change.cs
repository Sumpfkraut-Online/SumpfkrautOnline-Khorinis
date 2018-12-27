using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    /// <summary>
    /// As most basic unit of the effect system it stores information
    /// about the type of change and possible additional parameters to be
    /// used properly when influencing its host instance. Each Change belongs
    /// to exactly one Effect which in turn in used by an EffectHandler to
    /// apply the respective changes to the host instance.
    /// </summary>
    public class Change : ExtendedObject
    {

        /// <summary>
        /// Effect to which this change belongs.
        /// </summary>
        protected Effect effect;
        /// <summary>
        /// Get Effect to which this change belongs.
        /// </summary>
        public Effect GetEffect () { return effect; }
        /// <summary>
        ///  Set Effect to which this change belongs.
        /// </summary>
        public void SetEffect (Effect value) { effect = value; }

        protected ChangeInitInfo changeInitInfo;
        public ChangeInitInfo GetChangeInitInfo () { return changeInitInfo; }
        public ChangeType GetChangeType () { return changeInitInfo.ChangeType; }
        public List<Type> GetParameterTypes () { return changeInitInfo.ParameterTypes; }

        /// <summary>
        /// Mutable parameters further specifying how the Change operates.
        /// </summary>
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
            this.changeInitInfo = changeInitInfo;
            this.parameters = new List<object>(changeInitInfo.ParameterTypes.Count);
        }

        /// <summary>
        /// Preferred way to instantiate new Changes.
        /// </summary>
        /// <param name="changeType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Preferred way to instantiate new Changes.
        /// </summary>
        /// <param name="changeInitInfo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
