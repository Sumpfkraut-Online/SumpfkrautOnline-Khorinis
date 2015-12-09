using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum NPCState : byte
    {
        //States:

        Stand,

        MoveForward,
        MoveBackward,

        Animation,



        //Target states:

        MoveLeft,
        MoveRight,

        AttackForward,
        AttackLeft,
        AttackRight,
        AttackRun,
        Parry,
        DodgeBack
    }
}
