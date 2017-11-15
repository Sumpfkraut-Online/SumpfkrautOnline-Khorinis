using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects.Instances;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class ProjDef : BaseVobDef, ProjectileInstance.IScriptProjectileInstance
    {
        #region Constructors

        partial void pConstruct();
        public ProjDef()
        {
            pConstruct();
        }

        protected override BaseVobInstance CreateVobInstance()
        {
            return new ProjectileInstance(this);
        }

        protected override BaseEffectHandler CreateHandler()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Properties

        new public ProjectileInstance BaseDef { get { return (ProjectileInstance)base.BaseDef; } }

        #endregion

    }
}
