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

        public new VobInstance BaseDef { get { return (VobInstance)base.BaseDef; } }

        public string Visual { get { return BaseDef.Visual; } set { BaseDef.Visual = value; } }
        public bool CDDyn { get { return BaseDef.CDDyn; } set { BaseDef.CDDyn = value; } }
        public bool CDStatic { get { return BaseDef.CDStatic; } set { BaseDef.CDStatic = value; } }

        #endregion

        public VobDef(PacketReader stream) : this(new VobInstance(), stream)
        {
        }

        protected VobDef(VobInstance baseDef, PacketReader stream) : base(baseDef, stream)
        {
        }
    }
}
