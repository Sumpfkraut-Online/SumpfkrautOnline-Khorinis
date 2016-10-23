using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class ItemInstance : VobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Item; } }

        #region ScriptObject

        public partial interface IScriptItemInstance : IScriptVobInstance
        {
        }

        public new IScriptItemInstance ScriptObject { get { return (IScriptItemInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public ItemInstance(IScriptItemInstance scriptObject) : base(scriptObject)
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