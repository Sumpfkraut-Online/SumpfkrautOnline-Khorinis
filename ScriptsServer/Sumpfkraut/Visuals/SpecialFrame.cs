using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public enum SpecialFrame
    {
        // Draw Animation
        Draw = 0,

        // Attack Animation, Hit must be <= Combo!
        Combo = 1,
        Hit = 2,
    }
}
