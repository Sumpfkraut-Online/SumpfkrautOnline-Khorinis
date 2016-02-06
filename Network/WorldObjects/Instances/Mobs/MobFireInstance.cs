using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobFireInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobFire; } }

        #region ScriptObject

        public partial interface IScriptMobFireInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobFireInstance ScriptObject
        {
            get { return (IScriptMobFireInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        public string FireVobTree = "";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public MobFireInstance(IScriptMobFireInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobFireInstance(IScriptMobFireInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FireVobTree = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FireVobTree);
        }

        #endregion
    }
}
