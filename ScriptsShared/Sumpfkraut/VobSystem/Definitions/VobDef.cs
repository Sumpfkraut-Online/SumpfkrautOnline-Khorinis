using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class VobDef : BaseVobDef, VobInstance.IScriptVobInstance
    {
        new public static readonly String _staticName = "VobDef (static)";

        #region Constructors

        partial void pConstruct();
        public VobDef()
        {
            pConstruct();
        }

        protected override BaseVobInstance CreateVobInstance()
        {
            return new VobInstance(this);
        }

        #endregion

        #region Properties

        new public VobInstance BaseDef { get { return (VobInstance)base.BaseDef; } }
        
        public ModelDef Model
        {
            get { return (ModelDef)this.BaseDef.ModelInstance.ScriptObject; }
            set { this.BaseDef.ModelInstance = value.BaseDef; }
        }

        public bool CDDyn { get { return BaseDef.CDDyn; } set { BaseDef.CDDyn = value; } }
        public bool CDStatic { get { return BaseDef.CDStatic; } set { BaseDef.CDStatic = value; } }

        #endregion

    }
}
