using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    class MobSwitch : MobInter
    {
        new public MobSwitchInstance Instance { get; protected set; }
        new public oCMobSwitch gVob { get; protected set; }

        public MobSwitch(uint id, ushort instanceid) : this(id, instanceid, null)
        {
        }

        public MobSwitch(uint id, ushort instanceid, oCMobSwitch mob) : base(id, instanceid, mob)
        {
        }

        protected override void CreateVob()
        {
            gVob = oCMobSwitch.Create(Program.Process);
        }
    }
}
