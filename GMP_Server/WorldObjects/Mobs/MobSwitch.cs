using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects.Mobs
{
    class MobSwitch : MobInter
    {
        new public MobSwitchInstance Instance { get; protected set; }

        public MobSwitch(MobSwitchInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
