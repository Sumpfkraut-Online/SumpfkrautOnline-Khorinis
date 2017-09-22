using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC
{
    public static class Cast
    {
        /// <summary>
        /// Casts the object to T or returns false if the object is default(T) or not of type T.
        /// </summary>
        public static bool Get<T>(object input, out T output)
        {
            output = (T)input;
            return true;
        }

        /// <summary>
        /// Casts the object to T or returns false if the object is default(T) or not of type T.
        /// </summary>
        public static bool Try<T>(object input, out T output)
        {
            if (input is T)
            {
                output = (T)input;
                return true;
            }
            output = default(T);
            return false;
        }
    }
}
