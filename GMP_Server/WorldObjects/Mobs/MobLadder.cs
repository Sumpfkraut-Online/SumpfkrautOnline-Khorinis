using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobLadder : MobInter
    {
        new public MobLadderInstance Instance { get; protected set; }

        public MobLadder(MobLadderInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
