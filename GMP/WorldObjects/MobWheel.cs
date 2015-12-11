using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    class MobWheel : MobInter
    {
        new public MobWheelInstance Instance { get; protected set; }
        new public oCMobWheel gVob { get; protected set; }

        public MobWheel(uint id, ushort instanceid) : this(id, instanceid, null)
        {
        }

        public MobWheel(uint id, ushort instanceid, oCMobWheel mob) : base(id, instanceid, mob)
        {
        }

        protected override void CreateVob()
        {
            gVob = oCMobWheel.Create(Program.Process);
        }
    }
}
