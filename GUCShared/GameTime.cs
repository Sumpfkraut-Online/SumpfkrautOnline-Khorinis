using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC
{
    public static class GameTime
    {
        static long ticks = DateTime.UtcNow.Ticks;
        public static long Ticks { get { return ticks; } }

        internal static void Update()
        {
            ticks = DateTime.UtcNow.Ticks;
        }
    }
}
