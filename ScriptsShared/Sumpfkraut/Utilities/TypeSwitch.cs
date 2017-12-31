using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Utilities
{

    public class TypeSwitch
    {

        protected Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();

        public TypeSwitch Case<T>(Action<T> action)
        {
            matches.Add(typeof(T), (x) => action((T)x));
            return this;
        }

        public void Switch(object x)
        {
            matches[x.GetType()](x);
        }

        //protected List<Type> cases = new List<Type>();
        //protected List<Action<object>> actions = new List<Action<object>>();
        //protected Action defaultAction;


        //public TypeSwitch Case<T>(Action<T> action)
        //{
        //    cases.Add(typeof(T));
        //    actions.Add((x) => action((T)x));
        //    return this;
        //}

        //public TypeSwitch Default(Action action)
        //{
        //    defaultAction = action;
        //    return this;
        //}

        //public TypeSwitch Switch(object x)
        //{
        //    var index = cases.IndexOf(x.GetType());
        //    if (index > -1) { actions[index](x); }
        //    else if (!(defaultAction is null)) { defaultAction(); }
        //    return this;
        //}

    }

}
