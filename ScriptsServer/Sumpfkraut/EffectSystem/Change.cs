using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public class Change : ExtendedObject
    {

        new public static readonly string _staticName = "Change (static)";



        // effect to which this change belongs
        private Effect effect;
        public Effect Effect { get { return effect; } }
        public void SetEffect (Effect effect) { this.effect = effect; }

        private Enumeration.ChangeType changeType;
        public Enumeration.ChangeType ChangeType { get { return this.changeType; } }

        private object[] parameters;
        public object[] Parameters { get { return this.parameters; } }
        public void SetParametersComplete (object[] parameters, Type[] parameterTypes)
        {
            if (!(parameters.Length <= parameterTypes.Length))
            {
                MakeLogWarning("Not enough items in array paramterTypes to describe the types of given paramters.");
            }

            this.parameters = parameters;
            this.parameterTypes = parameterTypes;
        }

        private Type[] parameterTypes;
        public Type[] ParameterTypes { get { return this.parameterTypes; } }



        public Change (Effect effect, Enumeration.ChangeType changeType, object[] parameters, Type[] parameterTypes)
        {
            SetObjName("Change (default)");
            this.effect = effect;
            this.changeType = changeType;

            if (!(parameters.Length <= parameterTypes.Length))
            {
                MakeLogWarning("Not enough items in array paramterTypes to describe the types of given paramters.");
            }
            this.parameters = parameters;
            this.parameterTypes = parameterTypes;
        }



        public override string ToString ()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Change [effect: {0}, changeType: {1}, parameters: [", effect, changeType);
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.AppendFormat("{0}: ({1}) {2}, ", i, parameterTypes[i], parameters[i]);
            }
            sb.Append("]]");
            return sb.ToString();
        }

        public static bool operator ==(Change c1, Change c2) 
        {
            if ((c1.changeType == c2.changeType) 
                && (c1.parameters.Length == c2.parameters.Length))
            {
                for (int i = 0; i < c1.parameters.Length; i++)
                {
                    if (!c2.parameters.Contains(c1.parameters[i])) { return false; }
                }
                return true;
            }
            else { return false; }
        }

        public static bool operator !=(Change c1, Change c2)
        {
            return !(c1 == c2);
        }

    }

}
