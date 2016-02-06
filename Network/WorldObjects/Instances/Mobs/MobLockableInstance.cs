using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class MobLockableInstance : MobInterInstance
    {
        #region ScriptObject

        public partial interface IScriptMobLockableInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobLockableInstance ScriptObject
        {
            get { return (IScriptMobLockableInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        public string OnTryOpenClientFunc = "";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        protected MobLockableInstance(IScriptMobLockableInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobLockableInstance(IScriptMobLockableInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

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
