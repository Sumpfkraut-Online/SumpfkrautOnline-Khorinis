using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class MobLockableInstance : MobInterInstance
    {
        public partial interface IScriptMobLockableInstance : IScriptMobInterInstance
        {
        }

        #region Properties

        public string OnTryOpenClientFunc = "";

        #endregion

        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.OnTryOpenClientFunc = stream.ReadString();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(OnTryOpenClientFunc);
        }
    }
}
