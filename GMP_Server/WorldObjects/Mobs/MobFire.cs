using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobFire : MobInter
    {
        new public MobFireInstance Instance { get; protected set; }
        public string FireVobTree { get { return Instance.FireVobTree; } }

        public MobFire(MobFireInstance instance, object scriptObject) : base(instance, scriptObject)
        {
        }
    }
}
