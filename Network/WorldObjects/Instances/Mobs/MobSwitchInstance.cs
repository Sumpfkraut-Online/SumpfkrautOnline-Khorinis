using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobSwitchInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobSwitch; } }

        #region ScriptObject

        public partial interface IScriptMobSwitchInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobSwitchInstance ScriptObject
        {
            get { return (IScriptMobSwitchInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public MobSwitchInstance(IScriptMobSwitchInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobSwitchInstance(IScriptMobSwitchInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
