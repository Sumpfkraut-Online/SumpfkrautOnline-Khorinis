using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst : BaseVobInst, Vob.IScriptVob
    {
        #region Properties

        public new Vob BaseInst { get { return (Vob)base.BaseInst; } }

        #endregion
        
        public VobInst() : this(new Vob())
        {
        }

        protected VobInst(Vob baseInst) : base(baseInst)
        {
        }
    }
}
