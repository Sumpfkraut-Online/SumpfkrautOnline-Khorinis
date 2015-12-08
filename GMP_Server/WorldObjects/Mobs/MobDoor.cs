using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobDoor : MobLockable
    {
        new public MobDoorInstance Instance { get; protected set; }

        public MobDoor(MobDoorInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
