using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinApi.NEW
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

        public static void ArrayCount(Array input, uint count)
        {
            if (input.Length < count)
                throw new Exception(string.Format("Array length is smaller than count! ({0} < {1})", input.Length, count));
        }
    }
}
