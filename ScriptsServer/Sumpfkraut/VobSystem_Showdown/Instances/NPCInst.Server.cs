using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Enumeration;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        public NPCInst(NPCDef def) : base(def, new WorldObjects.NPC())
        {
        }

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
    }
}
