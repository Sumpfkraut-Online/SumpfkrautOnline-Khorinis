using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class MobInter : Mob
    {
        public override VobTypes VobType { get { return VobTypes.MobInter; } }

        #region ScriptObject

        public partial interface IScriptMobInter : IScriptMob
        {
        }

        new public IScriptMobInter ScriptObject { get { return (IScriptMobInter)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobInterInstance Instance { get { return (MobInterInstance)base.Instance; } }

        public string OnTriggerClientFunc { get { return Instance.OnTriggerClientFunc; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobInter(IScriptMobInter scriptObject, MobInterInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobInter(IScriptMobInter scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
