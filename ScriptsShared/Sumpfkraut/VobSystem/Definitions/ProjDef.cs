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

        public ModelDef Model
        {
            get { return (ModelDef)this.BaseDef.Model.ScriptObject; }
            set { this.BaseDef.Model = value.BaseDef; }
        }

        public float Velocity
        {
            get { return this.BaseDef.Velocity; }
            set { this.BaseDef.Velocity = value; }
        }

        #endregion

    }
}
