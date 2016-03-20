using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class ItemInstance : VobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Item; } }

        #region ScriptObject

        public partial interface IScriptItemInstance : IScriptVobInstance
        {
        }

        public new IScriptItemInstance ScriptObject
        {
            get { return (IScriptItemInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
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