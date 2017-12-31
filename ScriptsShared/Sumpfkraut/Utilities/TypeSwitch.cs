using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Utilities
{

    public class TypeSwitch
    {

        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();

        public TypeSwitch Case<T>(Action<T> action)
        {
            matches.Add(typeof(T), (x) => action((T)x));
            return this;
        }

        public void Switch(object x)
        {
            matches[x.GetType()](x);
        }

    }

}
