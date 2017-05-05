using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{

    public partial class NamedVobInst : VobInst, Vob.IScriptVob
    {

        new public static readonly string _staticName = "NamedVobInst (s)";



        #region Constructors

        partial void pConstruct();
        public NamedVobInst()
        {
            SetObjName("NamedVobInst");
            effectHandler = effectHandler ?? new EffectSystem.EffectHandlers.VobEffectHandler(null, this);
            pConstruct();
        }

        protected override BaseVob CreateVob()
        {
            return new Vob(new ModelInst(this), this);
        }

        #endregion

        #region Properties

        new public NamedVobDef Definition { get { return (NamedVobDef)base.Definition; } set { base.Definition = value; } }

        #endregion

    }

}
