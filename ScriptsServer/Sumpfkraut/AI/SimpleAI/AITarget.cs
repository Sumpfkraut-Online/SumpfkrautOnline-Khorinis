using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI
{

    public class AITarget : ExtendedObject
    {

        public List<VobInst> VobTargets;



        public AITarget (VobInst vobTarget)
        {
            this.VobTargets = new List<VobInst>() { vobTarget };
        }

        public AITarget (List<VobInst> vobTargets)
        {
            this.VobTargets = vobTargets;
        }

    }

}
