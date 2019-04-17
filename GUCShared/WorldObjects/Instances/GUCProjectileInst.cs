using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.WorldObjects.Instances;
using GUC.Models;
using GUC.Types;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class GUCProjectileInst : GUCBaseVobInst
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.Projectile; } }

        #region ScriptObject

        public partial interface IScriptProjectile : IScriptBaseVob
        {
        }
        
        public new IScriptProjectile ScriptObject { get { return (IScriptProjectile)base.ScriptObject; } }

        #endregion

        #region Constructors

        public GUCProjectileInst(IScriptProjectile scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        public override Type InstanceType { get { return typeof(GUCProjectileDef); } }
        new public GUCProjectileDef Instance
        {
            get { return (GUCProjectileDef)base.Instance; }
            set { SetInstance(value); }
        }
        
        float velocity;
        public float Velocity
        {
            get { return this.velocity; }
            set
            {
                if (this.IsSpawned)
                    throw new NotSupportedException();
                this.velocity = value;
            }
        }

        ModelInstance model;
        public ModelInstance Model
        {
            get { return this.model; }
            set
            {
                if (this.IsSpawned)
                    throw new NotSupportedException();
                this.model = value;
            }
        }

        Vec3f destination;
        public Vec3f Destination
        {
            get { return this.destination; }
            set
            {
                if (this.IsSpawned)
                    throw new NotSupportedException();
                this.destination = value;
            }
        }

        /// <summary> Projectiles are always dynamic! Will throw a NotSupportedException when set. </summary>
        public override bool IsStatic
        {
            get { return false; }
            set { throw new NotSupportedException("Projectiles are dynamic!"); }
        }

        #endregion

        #region Spawn
        
        Vec3f lastPos;
        public Vec3f LastPosition { get { return lastPos; } }

        long startTime;

        Vec3f startPos;
        public Vec3f StartPosition { get { return this.startPos; } }

        partial void pSpawn();
        public override void Spawn(World world, Vec3f position, Angles angles)
        {
            base.Spawn(world, position, angles);
            startTime = GameTime.Ticks;
            startPos = position;
            lastPos = position;

            pSpawn();
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
            return startPos + (destination - startPos).Normalise() * Velocity * flyTime;
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(velocity);
            stream.Write((ushort)model.ID);
            stream.WriteCompressedPosition(this.destination);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.velocity = stream.ReadFloat();
            int modelID = stream.ReadUShort();
            if (!ModelInstance.TryGet(modelID, out model))
                throw new Exception("Model not found! " + modelID);
            this.destination = stream.ReadCompressedPosition();
        }
    }
}
