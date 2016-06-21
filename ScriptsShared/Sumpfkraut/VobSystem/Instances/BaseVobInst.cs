using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Enumeration;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public abstract partial class BaseVobInst : ScriptObject, BaseVob.IScriptBaseVob
    {
        #region Properties

        // GUC - Base - Object
        BaseVob baseInst;
        public BaseVob BaseInst { get { return baseInst; } }

        // Definition 
        public BaseVobDef Definition
        {
            get { return (BaseVobDef)baseInst.Instance.ScriptObject; }
            set { baseInst.Instance = value.BaseDef; }
        }

        public int ID { get { return baseInst.ID; } }
        public VobTypes VobType { get { return baseInst.VobType; } }
        public bool IsStatic { get { return baseInst.IsStatic; } }
        public bool IsSpawned { get { return baseInst.IsSpawned; } }

        public WorldInst World { get { return (WorldInst)this.baseInst.World.ScriptObject; } }

        public Vec3f GetPosition() { return this.baseInst.GetPosition(); }
        public Vec3f GetDirection() { return this.baseInst.GetDirection(); }

        public void SetPosition(Vec3f position) { this.baseInst.SetPosition(position); }
        public void SetDirection(Vec3f direction) { this.baseInst.SetDirection(direction); }

        #endregion

        public virtual void OnPosChanged()
        {
        }

        protected BaseVobInst(BaseVob baseInst)
        {
            if (baseInst == null)
                throw new ArgumentNullException("BaseInst is null!");

            this.baseInst = baseInst;
            this.baseInst.ScriptObject = this;
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
            this.baseInst.Spawn(world.BaseWorld, pos, dir);
        }

        public virtual void Despawn()
        {
            baseInst.Despawn();
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
