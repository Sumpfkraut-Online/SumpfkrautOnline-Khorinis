using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC
{
    public static class Alg
    {
        public static float Clamp(float min, float value, float max)
        {
            if (value <= min)
                return min;
            if (value >= max)
                return max;
            return value;
        }

        public static int Clamp(int min, int value, int max)
        {
            if (value <= min)
                return min;
            if (value >= max)
                return max;
            return value;
        }
    }
}
