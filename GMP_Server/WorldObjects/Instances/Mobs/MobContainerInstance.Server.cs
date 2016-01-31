using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Instances
{
    public partial class MobContainerInstance : MobLockableInstance
    {
        public MobContainerInstance(string instanceName, IScriptMobContainerInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobContainerInstance(ushort ID, string instanceName, IScriptMobContainerInstance scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }
    }
}
