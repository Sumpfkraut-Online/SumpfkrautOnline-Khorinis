using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects;
using GUC.WorldObjects.Instances;

using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public abstract partial class BaseVobInst : GUC.Utilities.ExtendedObject, GUCBaseVobInst.IScriptBaseVob
    {
        #region Properties

        public byte GetVobType() { return (byte)this.VobType; } // for base vob interface
        public abstract VobType VobType { get; }

        // Effect Handler
        BaseEffectHandler effectHandler;
        public BaseEffectHandler EffectHandler
        {
            get
            {
                if (this.effectHandler == null)
                    this.effectHandler = CreateHandler();

                return effectHandler;
            }
        }
        protected abstract BaseEffectHandler CreateHandler();

        // GUC - Base - Object
        GUCBaseVobInst baseInst;
        public GUCBaseVobInst BaseInst { get { return this.baseInst; } }
        protected abstract GUCBaseVobInst CreateVob();

        // Definition 
        public BaseVobDef Definition
        {
            get { return (BaseVobDef)BaseInst.Definition?.ScriptObject; }
            set { BaseInst.Definition = value?.BaseDef; }
        }

        public int ID { get { return BaseInst.ID; } }
        public bool IsStatic { get { return BaseInst.IsStatic; } }
        public bool IsSpawned { get { return BaseInst.IsSpawned; } }

        public WorldInst World { get { return (WorldInst)this.BaseInst.World.ScriptObject; } }

        public Vec3f GetPosition() { return this.BaseInst.Position; }
        public Angles GetAngles() { return this.BaseInst.Angles; }

        public void SetPosition(Vec3f position) { this.BaseInst.SetPosition(position); }
        public void SetAngles(Angles angles) { this.BaseInst.SetAngles(angles); }

        #endregion

        #region Constructors

        partial void pConstruct();
        public BaseVobInst()
        {
            this.baseInst = CreateVob();
            if (this.baseInst == null)
                throw new NullReferenceException("BaseInst is null!");

            pConstruct();
        }

        #endregion

        #region Spawn & Despawn

        public void Spawn(World world)
        {
            this.Spawn((WorldInst)world.ScriptObject);
        }

        public void Spawn(WorldInst world)
        {
            this.Spawn(world, this.GetPosition(), this.GetAngles());
        }

        public virtual void Spawn(WorldInst world, Vec3f pos, Angles ang)
        {
            this.BaseInst.Spawn(world.BaseWorld, pos, ang);
        }

        public delegate void DespawnHandler(BaseVobInst vob, WorldInst oldWorld);
        public event DespawnHandler OnDespawn;
        public virtual void Despawn()
        {
            if (!this.IsSpawned)
                return;

            WorldInst oldWorld = this.World;
            BaseInst.Despawn();
            OnDespawn?.Invoke(this, oldWorld);
        }


        #endregion

        public virtual void OnReadProperties(PacketReader stream)
        {
        }
        public virtual void OnWriteProperties(PacketWriter stream)
        {
        }

        public virtual void OnReadScriptVobMsg(PacketReader stream)
        {
        }

        public float GetDistance(BaseVobInst other)
        {
            return this.GetPosition().GetDistance(other.GetPosition());
        }
    }
}
