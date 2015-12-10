using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    public class MobDoor : MobLockable
    {
        new public static readonly Collections.VobDictionary Vobs = Network.Server.sVobs.GetDict(MobDoorInstance.sVobType);

        new public MobDoorInstance Instance { get; protected set; }

        public MobDoor(MobDoorInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
