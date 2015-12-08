using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects.Mobs
{
    class MobWheel : MobInter
    {
        new public MobWheelInstance Instance { get; protected set; }

        public MobWheel(MobWheelInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
