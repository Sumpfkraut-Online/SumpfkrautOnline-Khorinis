using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobInterInstance : MobInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobInter; } }

        #region ScriptObject

        public partial interface IScriptMobInterInstance : IScriptMobInstance
        {
        }

        public new IScriptMobInterInstance ScriptObject
        {
            get { return (IScriptMobInterInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        public string OnTriggerClientFunc = "";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public MobInterInstance(IScriptMobInterInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobInterInstance(IScriptMobInterInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.OnTriggerClientFunc = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(OnTriggerClientFunc);
        }

        #endregion
    }
}
