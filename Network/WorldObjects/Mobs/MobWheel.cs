using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobWheel : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobWheel; } }

        #region ScriptObject

        public partial interface IScriptMobWheel : IScriptMobInter
        {
        }

        new public IScriptMobWheel ScriptObject { get { return (IScriptMobWheel)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobWheelInstance Instance { get { return (MobWheelInstance)base.Instance; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobWheel(IScriptMobWheel scriptObject, MobWheelInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobWheel(IScriptMobWheel scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
