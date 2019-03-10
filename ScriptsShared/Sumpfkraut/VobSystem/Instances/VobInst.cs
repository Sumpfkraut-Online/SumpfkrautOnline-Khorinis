using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst : BaseVobInst, Vob.IScriptVob
    {
        #region Constructors

        partial void pConstruct();
        public VobInst()
        {
            pConstruct();
        }

        protected override BaseVob CreateVob()
        {
            return new Vob(new ModelInst(this), this);
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return new VobInstEffectHandler(null, null, this);
        }

        #endregion

        #region Properties

        public override VobType VobType { get { return VobType.Vob; } }

        new public VobInstEffectHandler EffectHandler { get { return (VobInstEffectHandler)base.EffectHandler; } }

        public new Vob BaseInst { get { return (Vob)base.BaseInst; } }

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
