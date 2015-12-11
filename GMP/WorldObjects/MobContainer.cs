using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    public class MobContainer : MobLockable
    {
        new public MobContainerInstance Instance { get; protected set; }
        new public oCMobContainer gVob { get; protected set; }

        public MobContainer(uint id, ushort instanceid) : this(id, instanceid, null)
        {
        }

        public MobContainer(uint id, ushort instanceid, oCMobContainer mob) : base(id, instanceid, mob)
        {
        }

        protected override void CreateVob()
        {
            gVob = oCMobContainer.Create(Program.Process);
        }
    }
}
