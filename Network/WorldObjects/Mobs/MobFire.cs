using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class MobFire : MobInter
    {
        public override VobTypes VobType { get { return VobTypes.MobFire; } }

        #region ScriptObject

        public partial interface IScriptMobFire : IScriptMobInter
        {
        }

        new public IScriptMobFire ScriptObject { get { return (IScriptMobFire)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobFireInstance Instance { get { return (MobFireInstance)base.Instance; } }

        public string FireVobTree { get { return Instance.FireVobTree; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public MobFire(IScriptMobFire scriptObject, MobFireInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public MobFire(IScriptMobFire scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
