using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    class MobSwitch : MobInter
    {
        new public static readonly Collections.VobDictionary Vobs = Network.Server.sVobs.GetDict(MobSwitchInstance.sVobType);

        new public MobSwitchInstance Instance { get; protected set; }

        public MobSwitch(MobSwitchInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
