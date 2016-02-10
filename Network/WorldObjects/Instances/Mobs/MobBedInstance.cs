using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobBedInstance : MobInterInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobBed; } }

        #region ScriptObject

        public partial interface IScriptMobBedInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobBedInstance ScriptObject
        {
            get { return (IScriptMobBedInstance)base.ScriptObject; }
        }

        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Instance with the given ID or [-1] a free ID.
        /// </summary>
        public MobBedInstance(IScriptMobBedInstance scriptObject, int id = -1) : base(scriptObject, id)
        {
        }

        /// <summary>
        /// Creates a new Instance by reading a networking stream.
        /// </summary>
        public MobBedInstance(IScriptMobBedInstance scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}