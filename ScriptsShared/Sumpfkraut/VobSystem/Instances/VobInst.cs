using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst : BaseVobInst, Vob.IScriptVob
    {
        #region Properties

        public new Vob BaseInst { get { return (Vob)base.BaseInst; } }

        public new VobDef Definition { get { return (VobDef)base.Definition; } }

        public ModelDef Model { get { return this.Definition.Model; } }

        #endregion

        public VobInst() : this(new Vob())
        {
        }

        protected VobInst(Vob baseInst) : base(baseInst)
        {
        }
    }
}
