using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinApiNew.Utilities
{
    static class ExceptionHelper
    {
        public static void ArgumentNull(object input, string name = "")
        {
            if (input == null)
                throw new ArgumentNullException(string.Format("Argument {0} is null!", name));
        }

        public static void AddressZero(int address)
        {
            if (address <= 0)
                throw new Exception(string.Format("Address was smaller or equal to zero! ({0})", address.ToString("X4")));
        }

        public static void ArrayCount(Array input, int count)
        {
            if (input.Length < count)
                throw new IndexOutOfRangeException(string.Format("Array length is smaller than count! ({0} < {1})", input.Length, count));
        }

        public static void Length(int input, int minimum)
        {
            if (input < minimum)
                throw new Exception(string.Format("Length is too short! ({0} < {1})", input, minimum));
        }

        public static void EmptyArray(Array array)
        {
            if (array == null || array.Length == 0)
                throw new Exception("Array is empty!");
        }

        public static void SEQZero(int arg, string name = "")
        {
            if (arg <= 0)
                throw new ArgumentOutOfRangeException(string.Format("Argument {0} is smaller or equal to zero! ({1})", name, arg));
        }
    }
}
