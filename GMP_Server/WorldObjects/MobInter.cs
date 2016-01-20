using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    public class MobInter : Mob
    {
        new public static readonly Collections.VobDictionary Vobs = Network.Server.Vobs.GetDict(MobInterInstance.sVobType);

        new public MobInterInstance Instance { get; protected set; }
        public string OnTriggerFunc { get { return Instance.OnTriggerFunc; } }
        public string OnTriggerClientFunc { get { return Instance.OnTriggerClientFunc; } }
        public bool GetIsMulti { get { return Instance.IsMulti; } }

        public MobInter(MobInterInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
