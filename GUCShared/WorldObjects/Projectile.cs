using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.WorldObjects.Instances;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class Projectile : BaseVob
    {
        public override VobTypes VobType { get { return VobTypes.Projectile; } }

        #region ScriptObject

        public partial interface IScriptProjectile : IScriptBaseVob
        {
        }
        
        public new IScriptProjectile ScriptObject { get { return (IScriptProjectile)base.ScriptObject; } }

        #endregion

        #region Constructors

        public Projectile(IScriptProjectile scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(ProjectileInstance); } }
        new public ProjectileInstance Instance
        {
            get { return (ProjectileInstance)base.Instance; }
            set { SetInstance(value); }
        }

        public ModelInstance Model { get { return this.Instance.Model; } }
        public float Velocity { get { return this.Instance.Velocity; } }

        /// <summary> Projectiles are always dynamic! Will throw a NotSupportedException when set. </summary>
        public override bool IsStatic
        {
            get { return false; }
            set { throw new NotSupportedException("Projectiles are dynamic!"); }
        }

        #endregion

        #region Spawn
        
        Vec3f lastPos;

        long startTime;
        Vec3f startPos;
        public Vec3f GetStartPos() { return new Vec3f(this.startPos); }
        Vec3f startDir;
        public Vec3f GetStartDir() { return new Vec3f(this.startDir); }

        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            base.Spawn(world, position, direction);
            startTime = GameTime.Ticks;
            startPos = position;
            startDir = direction;
            
            lastPos = position;
        }

        #endregion

        #region OnTick

        partial void pOnTick(long now);
        internal override void OnTick(long now)
        {
            pOnTick(now);
        }

        #endregion

        Vec3f GetTimedPosition(long flyTime)
        {
            return startPos + (startDir * Velocity) * flyTime;
        }
    }
}
