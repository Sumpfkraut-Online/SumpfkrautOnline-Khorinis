using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobWheelInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobWheel; } }

        #region ScriptObject

        public partial interface IScriptMobWheelInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobWheelInstance ScriptObject
        {
            get { return (IScriptMobWheelInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public MobWheelInstance(IScriptMobWheelInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobWheelInstance(IScriptMobWheelInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
