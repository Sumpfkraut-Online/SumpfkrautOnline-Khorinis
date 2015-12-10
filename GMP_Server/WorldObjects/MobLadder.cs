using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Instances;

namespace GUC.Server.WorldObjects
{
    public class MobLadder : MobInter
    {
        new public static readonly Collections.VobDictionary Vobs = Network.Server.sVobs.GetDict(MobLadderInstance.sVobType);

        new public MobLadderInstance Instance { get; protected set; }

        public MobLadder(MobLadderInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
