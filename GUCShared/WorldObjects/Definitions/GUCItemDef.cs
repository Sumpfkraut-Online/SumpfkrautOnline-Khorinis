using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class GUCItemDef : GUCVobDef
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.Item; } }

        #region ScriptObject

        public partial interface IScriptItemInstance : IScriptVobInstance
        {
        }

        public new IScriptItemInstance ScriptObject { get { return (IScriptItemInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public GUCItemDef(IScriptItemInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Read & Write

        /*protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
        }*/

        #endregion
    }
}