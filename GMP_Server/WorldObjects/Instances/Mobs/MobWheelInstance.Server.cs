using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Instances
{
    public partial class MobWheelInstance : MobInterInstance
    {
        public MobWheelInstance(string instanceName, IScriptMobWheelInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobWheelInstance(ushort ID, string instanceName, IScriptMobWheelInstance scriptObject)
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }
    }
}
