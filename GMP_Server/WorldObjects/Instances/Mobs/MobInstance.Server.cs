using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobInstance : VobInstance
    {
        public MobInstance(string instanceName, IScriptMobInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobInstance(ushort ID, string instanceName, IScriptMobInstance scriptObject) 
            : base (ID, instanceName, scriptObject)
        {
        }
    }
}
