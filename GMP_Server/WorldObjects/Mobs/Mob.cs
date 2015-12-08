using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects.Mobs
{
    public class Mob : Vob
    {
        new public MobInstance Instance { get; protected set; }
        public string FocusName { get { return Instance.FocusName; } }

        public Mob(MobInstance instance, object scriptObject) 
            : base(instance, scriptObject)
        {
        }
    }
}
