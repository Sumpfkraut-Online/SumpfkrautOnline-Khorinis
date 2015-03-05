using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobDoor : MobLockable
    {
        public MobDoor()
            : base()
        {
            this.VobType = Enumeration.VobTypes.MobDoor;
        }
    }
}
