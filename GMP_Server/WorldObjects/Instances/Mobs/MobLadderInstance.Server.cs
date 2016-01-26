using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Instances
{
    public partial class MobLadderInstance : MobInterInstance
    {

        public MobLadderInstance(string instanceName, IScriptMobLadderInstance scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobLadderInstance(ushort ID, string instanceName, IScriptMobLadderInstance scriptObject) 
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }
    }
}
