using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting
{
    public class UserVars
    {
        object[] arr;

        public UserVars(int num)
        {
            arr = new object[num];
        }

        public object Get(int i)
        {
            return arr[i];
        }

        public T Get<T>(int i)
        {
            return (T)arr[i];
        }

        public void Set(int i, object var)
        {
            arr[i] = var;
        }

        public void Set<T>(int i, T var)
        {
            arr[i] = (object)var;
        }
    }
}
