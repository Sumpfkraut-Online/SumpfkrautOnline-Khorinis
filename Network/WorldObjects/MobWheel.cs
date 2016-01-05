using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class MobWheel : MobInter
    {
        public static readonly Collections.VobDictionary MobWheels = Vob.AllVobs.GetDict(Enumeration.VobTypes.MobWheel);

        new public MobWheelInstance Instance { get; protected set; }

        internal MobWheel()
        {
        }
    }
}
