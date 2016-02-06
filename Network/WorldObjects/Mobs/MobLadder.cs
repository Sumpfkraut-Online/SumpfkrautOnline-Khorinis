using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class MobLadder : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobLadder; } }

        #region ScriptObject

        public partial interface IScriptMobLadder : IScriptMobInter
        {
        }

        new public IScriptMobLadder ScriptObject { get { return (IScriptMobLadder)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobLadderInstance Instance { get { return (MobLadderInstance)base.Instance; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobLadder(IScriptMobLadder scriptObject, MobLadderInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobLadder(IScriptMobLadder scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
