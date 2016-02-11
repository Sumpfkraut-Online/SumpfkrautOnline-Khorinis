using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class NPCDef
    {
        public NPCDef(PacketReader stream)
        {
            this.ReadDef(new NPCInstance(this), stream);
        }
    }
}
