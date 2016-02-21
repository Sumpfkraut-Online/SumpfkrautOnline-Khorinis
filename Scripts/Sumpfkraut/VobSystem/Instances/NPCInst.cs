using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst : VobInst, WorldObjects.NPC.IScriptNPC
    {

        #region Properties

        protected new WorldObjects.NPC baseInst { get { return (WorldObjects.NPC)base.baseInst; } }

        #endregion
    }
}
