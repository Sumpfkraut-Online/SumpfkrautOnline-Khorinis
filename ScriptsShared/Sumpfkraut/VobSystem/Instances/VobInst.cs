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

        public new WorldObjects.Vob BaseInst { get { return (WorldObjects.Vob)base.BaseInst; } }

        #endregion
        
        public VobInst(PacketReader stream) : this(new Vob(), stream)
        {
        }

        protected VobInst(Vob baseInst, PacketReader stream) : base(baseInst, stream)
        {
        }

        protected VobInst(Vob baseInst) : base(baseInst)
        {
        }
    }
}
