using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Mobs;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
namespace GUC.Scripts.Sumpfkraut.Mobs.Definitions
{
    public class MobDef : VobDef
    {
        public MobDef(string codename, Visuals.ModelDef model)
            : base(codename)
        {
            Model = model;
        }

    }
}
