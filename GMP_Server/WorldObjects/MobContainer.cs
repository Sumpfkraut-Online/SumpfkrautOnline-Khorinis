using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    public class MobContainer : MobLockable
    {
        new public static readonly Collections.VobDictionary Vobs = Network.Server.sVobs.GetDict(MobContainerInstance.sVobType);

        new public MobContainerInstance Instance { get; protected set; }

        public MobContainer(MobContainerInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
