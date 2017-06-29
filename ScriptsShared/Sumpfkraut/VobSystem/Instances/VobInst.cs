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
            SetObjName("VobInst");
            effectHandler = effectHandler ?? new EffectSystem.EffectHandlers.VobInstEffectHandler(null, this);
            pConstruct();
        }

        protected override BaseVob CreateVob()
        {
            return new Vob(new ModelInst(this), this);
        }

        #endregion

        #region Properties

        new public static readonly string _staticName = "VobInst (s)";



        new protected VobInstEffectHandler effectHandler;
        new public VobInstEffectHandler GetEffectHandler () { return effectHandler; }

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
