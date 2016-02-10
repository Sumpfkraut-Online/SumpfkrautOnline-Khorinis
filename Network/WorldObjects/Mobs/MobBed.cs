using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobBed : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobBed; } }

        #region ScriptObject

        public partial interface IScriptMobBed : IScriptMobInter
        {
        }

        new public IScriptMobBed ScriptObject { get { return (IScriptMobBed)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobBedInstance Instance { get { return (MobBedInstance)base.Instance; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobBed(IScriptMobBed scriptObject, MobBedInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobBed(IScriptMobBed scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
