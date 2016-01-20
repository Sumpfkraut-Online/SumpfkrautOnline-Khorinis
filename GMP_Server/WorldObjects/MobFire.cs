using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    public class MobFire : MobInter
    {
        new public static readonly Collections.VobDictionary Vobs = Network.Server.Vobs.GetDict(MobFireInstance.sVobType);

        new public MobFireInstance Instance { get; protected set; }
        public string FireVobTree { get { return Instance.FireVobTree; } }

        public MobFire(MobFireInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
