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

        new public static readonly string _staticName = "AITarget (static)";



        public List<VobInst> vobTargets;



        public AITarget (VobInst vobTarget)
        {
            this.vobTargets = new List<VobInst>() { vobTarget };
        }

        public AITarget (List<VobInst> vobTargets)
        {
            this.vobTargets = vobTargets;
        }

    }

}
