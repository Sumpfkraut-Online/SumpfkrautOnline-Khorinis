using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class MobInter : Mob
    {
        public static readonly Collections.VobDictionary MobInters = Vob.AllVobs.GetDict(Enumeration.VobTypes.MobInter);

        new public MobInterInstance Instance { get; protected set; }
        public string OnTriggerClientFunc { get { return Instance.OnTriggerClientFunc; } }

        internal MobInter()
        {

        }
    }
}
