using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class VobInst : BaseVobInst, WorldObjects.Vob.IScriptVob
    {
        #region Properties

        protected new WorldObjects.Vob baseInst { get { return (WorldObjects.Vob)base.baseInst; } }

        #endregion
    }
}
