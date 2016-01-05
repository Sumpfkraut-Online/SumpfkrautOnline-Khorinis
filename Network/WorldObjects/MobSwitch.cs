using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class MobSwitch : MobInter
    {
        public static readonly Collections.VobDictionary MobSwitchs = Vob.AllVobs.GetDict(Enumeration.VobTypes.MobSwitch);

        new public MobSwitchInstance Instance { get; protected set; }

        internal MobSwitch()
        {
        }
    }
}
