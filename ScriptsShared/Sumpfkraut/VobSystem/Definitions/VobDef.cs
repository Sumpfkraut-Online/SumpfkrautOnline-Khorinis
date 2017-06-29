using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class VobDef : BaseVobDef, VobInstance.IScriptVobInstance
    {

        #region Constructors

        partial void pConstruct();
        public VobDef()
        {
            SetObjName("VobDef");
            effectHandler = effectHandler ?? new EffectSystem.EffectHandlers.VobDefEffectHandler(null, this);
            pConstruct();
        }

        protected override BaseVobInstance CreateVobInstance()
        {
            return new VobInstance(this);
        }

        #endregion

        #region Properties

        new public static readonly string _staticName = "VobDef (s)";



        new protected VobDefEffectHandler effectHandler;
        new public VobDefEffectHandler GetEffectHandler () { return effectHandler; }

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
