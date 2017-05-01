using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities
{

    public class ArrayUtil : ExtendedObject
    {

        public static readonly string _staticName = "ArrayUtil (s)";



        protected ArrayUtil () { }



        public static T[] Populate<T> (T[] arr, T value, bool createNew = false)
        {
            T[] a;
            if (createNew) { a = new T[arr.Length]; }
            else { a = arr; }
            for (int i = 0; i < a.Length;i++)
            {
                a[i] = value;
            }
            return a;
        }

    }

}
