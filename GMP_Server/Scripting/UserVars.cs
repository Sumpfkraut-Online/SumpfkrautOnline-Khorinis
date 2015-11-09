using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting
{
    public class UserVars<T> : Dictionary<string, T>
    {
        public new T this[string s]
        {
            get
            {
                T result = default(T);
                this.TryGetValue(s, out result);
                return result;
            }
            set
            {
                T result;

                if (this.TryGetValue(s, out result))
                {
                    result = value;
                }
                else
                {
                    this.Add(s, value);
                }
            }
        }
    }
}
