using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    public class MobDoor : MobLockable
    {
        new public MobDoorInstance Instance { get; protected set; }
        new public oCMobDoor gVob { get; protected set; }

        public MobDoor(uint id, ushort instanceid) : this(id, instanceid, null)
        {
        }

        public MobDoor(uint id, ushort instanceid, oCMobDoor mob) : base(id, instanceid, mob)
        {
        }

        protected override void CreateVob()
        {
            gVob = oCMobDoor.Create(Program.Process);
        }
    }
}
