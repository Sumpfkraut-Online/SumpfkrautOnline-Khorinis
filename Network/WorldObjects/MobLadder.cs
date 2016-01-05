using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class MobLadder : MobInter
    {
        public static readonly Collections.VobDictionary MobLadders = Vob.AllVobs.GetDict(Enumeration.VobTypes.MobLadder);

        new public MobLadderInstance Instance { get; protected set; }

        internal MobLadder()
        {
        }
    }
}
