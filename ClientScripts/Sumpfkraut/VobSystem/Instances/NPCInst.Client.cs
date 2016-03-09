using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;


namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        partial void pOnCmdMove(NPCStates state, BaseVobInst target = null)
        {
            this.SetState(state, target);
        }

        partial void pOnCmdJump()
        {
            this.Jump();
        }
    }
}
