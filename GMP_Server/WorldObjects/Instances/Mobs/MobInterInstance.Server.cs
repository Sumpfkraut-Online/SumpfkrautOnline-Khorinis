using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobInterInstance : MobInstance
    {
        public MobInterInstance(string instanceName, IScriptMobInterInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobInterInstance(ushort ID, string instanceName, IScriptMobInterInstance scriptObject) 
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }
    }
}
