using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    public class Mob : Vob
    {
        new public oCMob gVob { get; protected set; }
        new public MobInstance Instance { get; protected set; }
        public string FocusName { get { return Instance.FocusName; } }

        public Mob(uint id, ushort instanceid)
         : this(id, instanceid, null)
        {
        }

        public Mob(uint id, ushort instanceid, oCMob mob) 
            : base(id, instanceid, mob)
        {
        }

        protected override void CreateVob()
        {
            gVob = oCMob.Create(Program.Process);
        }
    }
}
