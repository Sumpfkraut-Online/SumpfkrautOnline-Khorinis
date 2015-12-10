using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    public class Mob : Vob
    {
        new public static readonly Collections.VobDictionary Vobs = Network.Server.sVobs.GetDict(MobInstance.sVobType);

        new public MobInstance Instance { get; protected set; }
        public string FocusName { get { return Instance.FocusName; } }

        public Mob(MobInstance instance, object scriptObject) 
            : base(instance, scriptObject)
        {
        }
    }
}
