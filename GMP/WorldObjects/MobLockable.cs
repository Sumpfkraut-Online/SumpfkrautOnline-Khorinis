using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    public abstract class MobLockable : MobInter
    {
        new public oCMobLockable gVob { get; protected set; }
        new public MobLockableInstance Instance { get; protected set; }
        public string OnTryOpenClientFunc { get { return Instance.OnTryOpenClientFunc; } }

        public MobLockable(uint id, ushort instanceid) : base(id, instanceid, null)
        {
        }

        public MobLockable(uint id, ushort instanceid, oCMobLockable mob) : base(id, instanceid, mob)
        {
        }
    }
}
