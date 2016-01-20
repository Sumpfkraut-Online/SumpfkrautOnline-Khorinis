using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    class MobWheel : MobInter
    {
        new public static readonly Collections.VobDictionary Vobs = Network.Server.Vobs.GetDict(MobWheelInstance.sVobType);

        new public MobWheelInstance Instance { get; protected set; }

        public MobWheel(MobWheelInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
