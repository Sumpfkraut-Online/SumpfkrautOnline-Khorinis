using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects.Instances;
using Gothic.zClasses;

namespace GUC.Client.WorldObjects
{
    public class MobLadder : MobInter
    {
        new public MobLadderInstance Instance { get; protected set; }
        new public oCMobLadder gVob { get; protected set; }

        public MobLadder(uint id, ushort instanceid) : this(id, instanceid, null)
        {
        }

        public MobLadder(uint id, ushort instanceid, oCMobLadder mob) : base(id, instanceid, mob)
        {
        }

        protected override void CreateVob()
        {
            gVob = oCMobLadder.Create(Program.Process);
        }
    }
}
