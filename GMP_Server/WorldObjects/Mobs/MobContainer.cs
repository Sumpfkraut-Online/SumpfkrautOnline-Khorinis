using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobContainer : MobLockable
    {
        new public MobContainerInstance Instance { get; protected set; }

        public MobContainer(MobContainerInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
