using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class MobSwitch : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobSwitch; } }

        #region ScriptObject

        public partial interface IScriptMobSwitch : IScriptMobInter
        {
        }

        new public IScriptMobSwitch ScriptObject { get { return (IScriptMobSwitch)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobSwitchInstance Instance { get { return (MobSwitchInstance)base.Instance; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobSwitch(IScriptMobSwitch scriptObject, MobSwitchInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobSwitch(IScriptMobSwitch scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
