using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public struct Change
    {

        private Enumeration.ChangeType changeType;
        public Enumeration.ChangeType ChangeType { get { return this.changeType; } }

        private object[] parameters;
        public object[] Parameters { get { return this.parameters; } }

        private Type[] parameterTypes;
        public Type[] ParameterTypes { get { return this.parameterTypes; } }



        public Change (Enumeration.ChangeType changeType, object[] parameters, Type[] parameterTypes = null)
        {
            this.changeType = changeType;
            this.parameters = parameters;
            if ((parameterTypes != null) && (parameters.Length == parameterTypes.Length))
            {
                this.parameterTypes = parameterTypes;
            }
            else
            {
                this.parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    this.parameterTypes[i] = parameters.GetType();
                }
            }
        }

    }

}
