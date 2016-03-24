using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances
{
    /**
     *   Class which handles spell creation.
     */
    class SpellInst : VobInst
    {

        new public static readonly String _staticName = "SpellInst (static)";



        public SpellInst ()
        {
            SetObjName("SpellInst (default)");
        }

    }
}
