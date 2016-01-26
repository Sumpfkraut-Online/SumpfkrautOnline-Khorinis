using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Instances
{
    public partial class MobDoorInstance : MobLockableInstance
    {
        public MobDoorInstance(string instanceName, IScriptMobDoorInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobDoorInstance(ushort ID, string instanceName, IScriptMobDoorInstance scriptObject)
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }
    }
}
