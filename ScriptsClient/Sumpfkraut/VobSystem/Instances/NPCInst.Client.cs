using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;


namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        partial void pOnCmdMove(NPCStates state)
        {
            this.SetState(state);
        }

        partial void pOnCmdJump()
        {
            this.Jump();
        }
    }
}
