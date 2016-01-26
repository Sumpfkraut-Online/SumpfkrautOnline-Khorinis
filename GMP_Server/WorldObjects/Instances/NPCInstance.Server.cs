using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class NPCInstance : VobInstance
    {
        public NPCInstance(string instanceName, IScriptNPCInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public NPCInstance(ushort ID, string instanceName, IScriptNPCInstance scriptObject)
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }
    }
}