using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Instances
{
    public partial class MobFireInstance : MobInterInstance
    {
        public MobFireInstance(string instanceName, IScriptMobFireInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobFireInstance(ushort ID, string instanceName, IScriptMobFireInstance scriptObject) 
            : base(ID, instanceName, scriptObject)
        {
        }
    }
}
