using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class MobLockableInstance : MobInterInstance
    {
        public MobLockableInstance(string instanceName, IScriptMobLockableInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobLockableInstance(ushort ID, string instanceName, IScriptMobLockableInstance scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }
    }
}
