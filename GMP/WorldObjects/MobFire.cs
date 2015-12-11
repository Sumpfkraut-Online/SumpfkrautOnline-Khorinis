using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    public class MobFire : MobInter
    {
        new public MobFireInstance Instance { get; protected set; }
        public string FireVobTree { get { return Instance.FireVobTree; } }
        new public oCMobFire gVob { get; protected set; }

        public MobFire(uint id, ushort instanceid) : this(id, instanceid, null)
        {
        }

        public MobFire(uint id, ushort instanceid, oCMobFire mob) : base(id, instanceid, mob)
        {
        }

        protected override void CreateVob()
        {
            gVob = oCMobFire.Create(Program.Process);
        }

        protected override void SetProperties()
        {
            base.SetProperties();

            // set fire tree
        }
    }
}
