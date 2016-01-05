using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class Mob : Vob
    {
        public static readonly Collections.VobDictionary Mobs = Vob.AllVobs.GetDict(Enumeration.VobTypes.Mob);

        new public MobInstance Instance { get; protected set; }
        public string FocusName { get { return Instance.FocusName; } }

        internal Mob()
        {
        }
    }
}
