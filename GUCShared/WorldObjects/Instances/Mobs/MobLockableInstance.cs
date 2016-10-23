using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Instances.Mobs
{
    public abstract partial class MobLockableInstance : MobInterInstance
    {
        #region ScriptObject

        public partial interface IScriptMobLockableInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobLockableInstance ScriptObject { get { return (IScriptMobLockableInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public MobLockableInstance(IScriptMobLockableInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        public string OnTryOpenClientFunc = "";

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.OnTryOpenClientFunc = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(OnTryOpenClientFunc);
        }

        #endregion
    }
}
