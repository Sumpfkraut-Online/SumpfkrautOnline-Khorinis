using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ProjInst : BaseVobInst, Projectile.IScriptProjectile
    {
        #region Properties

        public new Projectile BaseInst { get { return (Projectile)base.BaseInst; } }
        public new ProjDef Definition { get { return (ProjDef)base.Definition; } }

        public ModelDef Model { get { return this.Definition.Model; } }
        public float Velocity { get { return this.Definition.Velocity; } }

        #endregion

        #region Constructors

        partial void pConstruct();
        
        public ProjInst() : base(new Projectile())
        {
            pConstruct();
        }

        #endregion
    }
}
