using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class MobContainer : MobLockable
    {
        public static readonly Collections.VobDictionary MobContainers = Vob.AllVobs.GetDict(Enumeration.VobTypes.MobContainer);

        new public MobContainerInstance Instance { get; protected set; }

        internal MobContainer()
        {
        }
    }
}
