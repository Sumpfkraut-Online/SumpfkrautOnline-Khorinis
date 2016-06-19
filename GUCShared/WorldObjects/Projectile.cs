using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
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

        public new IScriptProjectile ScriptObject
        {
            get { return (IScriptProjectile)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public ProjectileInstance Instance
        {
            get { return (ProjectileInstance)base.Instance; }
            set { base.Instance = value; }
        }

        public Model Model { get { return this.Instance.Model; } }
        public float Velocity { get { return this.Instance.Velocity; } }

        public override bool IsStatic
        {
            get { return false; }
            set { throw new Exception("Projectiles are dynamic!"); }
        }

        #endregion

        #region Spawn
        
        Vec3f lastPos;

        long startTime;
        Vec3f startPos;
        Vec3f startDir;

        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            base.Spawn(world, position, direction);
            startTime = GameTime.Ticks;
            startPos = position;
            startDir = direction;
            
            lastPos = position;
        }

        #endregion

        partial void pOnTick(long now);
        internal override void OnTick(long now)
        {
            pOnTick(now);
        }
        
        Vec3f GetTimedPosition(long flyTime)
        {
            return startPos + (startDir * Velocity) * flyTime;
        }
    }
}
