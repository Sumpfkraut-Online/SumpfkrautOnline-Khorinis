using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class MobDoor : MobLockable
    {
        public static readonly Collections.VobDictionary MobDoors = Vob.AllVobs.GetDict(Enumeration.VobTypes.MobDoor);

        new public MobDoorInstance Instance { get; protected set; }

        internal MobDoor()
        {
        }
    }
}
