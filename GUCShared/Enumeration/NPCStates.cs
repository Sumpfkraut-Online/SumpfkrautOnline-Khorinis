using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum MoveState : byte
    {
        Stand,

        Forward,
        Backward,
        Left,
        Right,

        Falling
    }

    public enum EnvironmentState : byte
    {
        None,
        InWater,
        Wading,

        Swimming,
        Diving,
        InAir,
    }
}
