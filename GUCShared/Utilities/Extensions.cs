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

    public static class ArrayExtensions
    {
        public static bool TryGet<T>(this T[] array, int index, out T result)
        {
            if (index < array.Length && index >= 0)
            {
                result = array[index];
                return true;
            }
            result = default(T);
            return false;
        }

        public static T TryGet<T>(this T[] array, int index)
        {
            return (index < array.Length && index >= 0) ? array[index] : default(T);
        }
    }
}
