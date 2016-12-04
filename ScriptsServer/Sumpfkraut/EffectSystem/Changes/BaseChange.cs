using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
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

        public static readonly Type[] parameterTypes = new Type[] { };



        // effect to which this change belongs
        private Effect effect;
        public Effect Effect { get { return effect; } }
        public void SetEffect (Effect effect) { this.effect = effect; }

        private Enumeration.ChangeType changeType;
        public Enumeration.ChangeType ChangeType { get { return this.changeType; } }

        private object[] parameters;
        public object[] Parameters { get { return this.parameters; } }



        public BaseChange (Effect effect, Enumeration.ChangeType changeType, object[] parameters)
        {
            //SetObjName("BaseChange (default)");
            this.effect = effect;
            this.changeType = changeType;
            this.parameters = parameters;
        }



        public static BaseChange Create (Effect effect, Enumeration.ChangeType changeType, object[] parameters)
        {
            return null;
        }

        public static bool CreateCheckBasics (Effect effect, Enumeration.ChangeType changeType, 
            object[] parameters, Type[] types)
        {
            if (effect == null) { return false; }
            if (changeType == Enumeration.ChangeType.Undefined) { return false; }
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

        //public static bool operator ==(BaseChange c1, BaseChange c2) 
        //{
        //    if ((c1.changeType == c2.changeType) 
        //        && (c1.parameters.Length == c2.parameters.Length))
        //    {
        //        for (int i = 0; i < c1.parameters.Length; i++)
        //        {
        //            if (!c2.parameters.Contains(c1.parameters[i])) { return false; }
        //        }
        //        return true;
        //    }
        //    else { return false; }
        //}

        //public static bool operator !=(BaseChange c1, BaseChange c2)
        //{
        //    return !(c1 == c2);
        //}

    }

}
