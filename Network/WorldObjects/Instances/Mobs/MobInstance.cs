using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobInstance : VobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Mob; } }

        #region ScriptObject

        public partial interface IScriptMobInstance : IScriptVobInstance
        {
        }

        public new IScriptMobInstance ScriptObject
        {
            get { return (IScriptMobInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        public string FocusName = "";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public MobInstance(IScriptMobInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobInstance(IScriptMobInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FocusName = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FocusName);
        }

        #endregion
    }
}
