using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class VobDef : BaseVobDef, VobInstance.IScriptVobInstance
    {
        new public static readonly String _staticName = "VobDef (static)";

        #region Properties

        protected new VobInstance baseDef { get { return (VobInstance)base.baseDef; } }

        public string Visual { get { return baseDef.Visual; } set { baseDef.Visual = value; } }
        public bool CDDyn { get { return baseDef.CDDyn; } set { baseDef.CDDyn = value; } }
        public bool CDStatic { get { return baseDef.CDStatic; } set { baseDef.CDStatic = value; } }

        #endregion

    }
}
