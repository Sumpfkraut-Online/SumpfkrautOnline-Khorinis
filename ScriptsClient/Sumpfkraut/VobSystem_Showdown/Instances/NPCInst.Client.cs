using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Scripts.Sumpfkraut.Visuals;

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

        partial void pOnCmdApplyOverlay(ScriptOverlay overlay)
        {
            this.ApplyOverlay(overlay);
        }

        partial void pOnCmdRemoveOverlay(ScriptOverlay overlay)
        {
            this.RemoveOverlay(overlay);
        }


        partial void pOnCmdStartAni(ScriptAniJob job)
        {
            this.StartAnimation(job);
        }

        partial void pOnCmdStopAni(bool fadeOut)
        {
            this.StopAnimation(fadeOut);
        }
    }
}
