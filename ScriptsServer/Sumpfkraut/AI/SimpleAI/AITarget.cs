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



        public List<WorldObjects.BaseVob> vobTargets;



        public AITarget (WorldObjects.BaseVob vobTarget)
        {
            this.vobTargets = new List<WorldObjects.BaseVob>() { vobTarget };
        }

        public AITarget (List<WorldObjects.BaseVob> vobTargets)
        {
            this.vobTargets = vobTargets;
        }

    }

}
