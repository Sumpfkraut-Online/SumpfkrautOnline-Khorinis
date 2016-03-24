using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Enumeration;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        public NPCInst (NPCDef vobDef) : base(vobDef, new WorldObjects.NPC())
        { }

        partial void pOnCmdMove (NPCStates state)
        {
            SetState(state);
        }

        partial void pOnCmdJump ()
        {
            Jump();
        }
    }
}
