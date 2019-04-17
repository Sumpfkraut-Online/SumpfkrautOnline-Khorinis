using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst : BaseVobInst, GUCVobInst.IScriptVob
    {
        #region Constructors

        partial void pConstruct();
        public VobInst()
        {
            pConstruct();
        }

        protected override GUCBaseVobInst CreateVob()
        {
            return new GUCVobInst(new ModelInst(this), this);
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return new VobInstEffectHandler(null, null, this);
        }

        #endregion

        #region Properties

        public override VobType VobType { get { return VobType.Vob; } }

        new public VobInstEffectHandler EffectHandler { get { return (VobInstEffectHandler)base.EffectHandler; } }

        public new GUCVobInst BaseInst { get { return (GUCVobInst)base.BaseInst; } }

        public ModelInst ModelInst { get { return (ModelInst)this.BaseInst.Model.ScriptObject; } }

        new public VobDef Definition { get { return (VobDef)base.Definition; } set { base.Definition = value; } }
        public ModelDef ModelDef { get { return this.Definition.Model; } }

        #endregion

        public void Throw(Vec3f velocity)
        {
            this.BaseInst.Throw(velocity);
        }
    }
}
