using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ProjInst : BaseVobInst, Projectile.IScriptProjectile
    {
        #region Properties

        new public Projectile BaseInst { get { return (Projectile)base.BaseInst; } }
        new public ProjDef Definition { get { return (ProjDef)base.Definition; } set { base.Definition = value; } }

        public ModelDef Model { get { return this.Definition.Model; } }
        public float Velocity { get { return this.Definition.Velocity; } }

        #endregion

        #region Constructors

        partial void pConstruct();
        public ProjInst()
        {
            SetObjName("ProjInst");
            pConstruct();
        }

        protected override BaseVob CreateVob()
        {
            return new Projectile(this);
        }

        #endregion

        #region Spawn / Despawn

        partial void pSpawn(WorldInst world, Vec3f pos, Vec3f dir);
        public override void Spawn(WorldInst world, Vec3f pos, Vec3f dir)
        {
            base.Spawn(world, pos, dir);
            pSpawn(world, pos, dir);
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
