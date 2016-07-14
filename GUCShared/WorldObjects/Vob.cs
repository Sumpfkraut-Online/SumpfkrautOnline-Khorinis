using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;
using GUC.Models;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class Vob : BaseVob
    {
        public override VobTypes VobType { get { return VobTypes.Vob; } }

        #region ScriptObject

        public partial interface IScriptVob : IScriptBaseVob
        {
        }

        public new IScriptVob ScriptObject
        {
            get { return (IScriptVob)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public VobInstance Instance
        {
            get { return (VobInstance)base.Instance; }
            set { base.Instance = value; }
        }

        public Model Model { get { return Instance.Model; } }
        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }

        #endregion

        #region Commandable

        /// <summary>
        /// The client which is used for positioning and pathfinding f.e. for NPCs or dropped items.
        /// </summary>
        public GameClient Commander { get; internal set; }

        #endregion

        #region Spawn & Despawn

        partial void pSpawn();
        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            base.Spawn(world, position, direction);
            pSpawn();
        }

        partial void pDespawn();
        public override void Despawn()
        {
            base.Despawn();
            pDespawn();
        }

        #endregion
    }
}
