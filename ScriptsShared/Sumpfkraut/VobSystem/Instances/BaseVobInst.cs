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

        #endregion

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
        
        public virtual void Spawn(WorldInst world)
        {
            this.baseInst.Spawn(world.BaseWorld);
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
    }
}
