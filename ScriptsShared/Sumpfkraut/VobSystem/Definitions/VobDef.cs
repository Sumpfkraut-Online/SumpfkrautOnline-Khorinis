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
        #region properties

        new public static readonly String _staticName = "VobDef (static)";

        public new VobInstance BaseDef { get { return (VobInstance) base.BaseDef; } }

        private ModelDef modelDef;
        public ModelDef ModelDef
        {
            get  {return modelDef; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Invalid null-value detected while assigning modelDef!");
                }
                modelDef = value;
                BaseDef.Model = modelDef.BaseDef;
            }
        }

        public bool CDDyn
        {
            get { return BaseDef.CDDyn; }
            set { BaseDef.CDDyn = value; }
        }

        public bool CDStatic
        {
            get { return BaseDef.CDStatic; }
            set { BaseDef.CDStatic = value; }
        }

        #endregion

        public VobDef (PacketReader stream) : this(new VobInstance(), stream)
        { }

        protected VobDef (VobInstance baseDef, PacketReader stream) : base(baseDef, stream)
        { }
    }
}
