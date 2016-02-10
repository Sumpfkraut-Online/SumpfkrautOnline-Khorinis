using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances.Mobs;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.WorldObjects.Mobs
{
    public partial class Mob : Vob
    {
        public override VobTypes VobType { get { return VobTypes.Mob; } }

        #region ScriptObject

        public partial interface IScriptMob : IScriptVob
        {
        }

        new public IScriptMob ScriptObject { get { return (IScriptMob)base.ScriptObject; } }

        #endregion

        #region Properties

        new public MobInstance Instance { get { return (MobInstance)base.Instance; } }
        
        public string FocusName { get { return Instance.FocusName; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public Mob(IScriptMob scriptObject, MobInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public Mob(IScriptMob scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        #endregion
    }
}
