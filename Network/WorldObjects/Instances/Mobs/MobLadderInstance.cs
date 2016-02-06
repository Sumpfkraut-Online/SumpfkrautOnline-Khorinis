using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobLadderInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobLadder; } }

        #region ScriptObject

        public partial interface IScriptMobLadderInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobLadderInstance ScriptObject
        {
            get { return (IScriptMobLadderInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public MobLadderInstance(IScriptMobLadderInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobLadderInstance(IScriptMobLadderInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
