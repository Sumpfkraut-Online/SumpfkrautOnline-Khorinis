using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC
{
    public static class GameTime
    {
        public static long Ticks { get { return DateTime.UtcNow.Ticks; } }
    }
}
