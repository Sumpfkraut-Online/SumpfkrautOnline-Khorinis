using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Models;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Definitions
{
    public partial class GUCProjectileDef : GUCBaseVobDef
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.Projectile; } }
        
        #region ScriptObject

        public partial interface IScriptProjectileInstance : IScriptBaseVobInstance
        {
        }

        public new IScriptProjectileInstance ScriptObject { get { return (IScriptProjectileInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public GUCProjectileDef(IScriptProjectileInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        // visual fx
        public object VisualFX;

        #endregion

        #region Create

        public override void Create()
        {
            base.Create();
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
        }

        #endregion
    }
}
