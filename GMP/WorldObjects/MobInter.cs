using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    public class MobInter : Mob
    {
        new public oCMobInter gVob { get; protected set; }
        new public MobInterInstance Instance { get; protected set; }
        public string OnTriggerClientFunc { get { return Instance.OnTriggerClientFunc; } }

        public MobInter(uint id, ushort instanceid) : this(id, instanceid, null)
        {

        }

        public MobInter(uint id, ushort instanceid, oCMobInter mobInter) : base(id, instanceid, mobInter)
        {
        }

        protected override void CreateVob()
        {
            gVob = oCMobInter.Create(Program.Process);
        }
    }
}
