using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances
{
    /**
     *   Class which handles mob creation.
     */
    class MobInst
    {
        
        private WorldInst inWorld;
        public WorldInst getInWorld () { return inWorld; }
        public void setInWorld (WorldInst inWorld) { this.inWorld = inWorld; }

    }
}
