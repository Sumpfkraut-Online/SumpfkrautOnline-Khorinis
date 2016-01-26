using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Instances
{
    public partial class MobSwitchInstance : MobInterInstance
    {
        public MobSwitchInstance(string instanceName, IScriptMobSwitchInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobSwitchInstance(ushort ID, string instanceName, IScriptMobSwitchInstance scriptObject)
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }
    }
}
