using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects;

using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public abstract partial class BaseVobInst : GUC.Utilities.ExtendedObject, BaseVob.IScriptBaseVob
    {
        #region Properties

        // GUC - Base - Object
        BaseVob baseInst;
        public BaseVob BaseInst { get { return this.baseInst; } }

        // Definition 
        public BaseVobDef Definition
        {
            get { return (BaseVobDef)BaseInst.Instance.ScriptObject; }
            set { BaseInst.Instance = value.BaseDef; }
        }

        public int ID { get { return BaseInst.ID; } }
        public VobTypes VobType { get { return BaseInst.VobType; } }
        public bool IsStatic { get { return BaseInst.IsStatic; } }
        public bool IsSpawned { get { return BaseInst.IsSpawned; } }

        public WorldInst World { get { return (WorldInst)this.BaseInst.World.ScriptObject; } }

        public Vec3f GetPosition() { return this.BaseInst.GetPosition(); }
        public Vec3f GetDirection() { return this.BaseInst.GetDirection(); }

        public void SetPosition(Vec3f position) { this.BaseInst.SetPosition(position); }
        public void SetDirection(Vec3f direction) { this.BaseInst.SetDirection(direction); }

        #endregion

        protected abstract BaseVob CreateVob();
        public BaseVobInst()
        {
            this.baseInst = CreateVob();
            if (this.baseInst == null)
                throw new Exception("BaseInst is null!");
        }

        public void Spawn(World world)
        {
            this.Spawn((WorldInst)world.ScriptObject);
        }

        public void Spawn(WorldInst world)
        {
            this.Spawn(world, this.GetPosition(), this.GetDirection());
        }

        public virtual void Spawn(WorldInst world, Vec3f pos, Vec3f dir)
        {
            this.BaseInst.Spawn(world.BaseWorld, pos, dir);
        }

        public virtual void Despawn()
        {
            BaseInst.Despawn();
        }

        public virtual void OnReadProperties(PacketReader stream)
        {
        }
        public virtual void OnWriteProperties(PacketWriter stream)
        {
        }

        public virtual void OnReadScriptVobMsg(PacketReader stream)
        {
        }
    }
}
