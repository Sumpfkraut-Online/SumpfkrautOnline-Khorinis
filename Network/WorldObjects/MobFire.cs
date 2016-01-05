using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class MobFire : MobInter
    {
        public static readonly Collections.VobDictionary MobFires = Vob.AllVobs.GetDict(Enumeration.VobTypes.MobFire);

        new public MobFireInstance Instance { get; protected set; }
        public string FireVobTree { get { return Instance.FireVobTree; } }

        internal MobFire()
        {
        }
    }
}
