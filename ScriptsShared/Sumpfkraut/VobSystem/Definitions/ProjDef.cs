using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class ProjDef : BaseVobDef, GUCProjectileDef.IScriptProjectileInstance
    {
        #region Constructors

        partial void pConstruct();
        public ProjDef()
        {
            pConstruct();
        }

        protected override GUCBaseVobDef CreateVobInstance()
        {
            return new GUCProjectileDef(this);
        }

        protected override BaseEffectHandler CreateHandler()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Properties

        public override VobType VobType { get { return VobType.Projectile; } }

        new public GUCProjectileDef BaseDef { get { return (GUCProjectileDef)base.BaseDef; } }

        #endregion

    }
}
