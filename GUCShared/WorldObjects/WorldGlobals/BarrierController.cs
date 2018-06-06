using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class BarrierController : SkyController
    {
        #region ScriptObject

        public partial interface IScriptBarrierController : IScriptSkyController
        {
        }

        /// <summary> The ScriptObject of this GameObject. </summary>
        new public IScriptBarrierController ScriptObject { get { return (IScriptBarrierController)base.ScriptObject; } }

        #endregion

        #region Constructors

        internal BarrierController(World world, IScriptBarrierController scriptObject) : base(world, scriptObject)
        {
        }

        #endregion

        #region SetNextWeight

        partial void pSetNextWeight();
        public override void SetNextWeight(long ticks, float weight)
        {
            base.SetNextWeight(ticks, weight);
            pSetNextWeight();
        }

        #endregion

        #region UpdateWeight
        
        partial void pUpdateWeight();
        internal override void UpdateWeight()
        {
            base.UpdateWeight();
            pUpdateWeight();
        }

        #endregion
    }
}
