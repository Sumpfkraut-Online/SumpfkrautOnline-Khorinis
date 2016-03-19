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
    public partial class BaseVobInst : ScriptObject, BaseVob.IScriptBaseVob
    {
        #region Properties

        // GUC - Base - Object
        BaseVob baseInst;
        public BaseVob BaseInst { get { return baseInst; } }

        // Definition 
        BaseVobDef definition;
        public BaseVobDef Definition { get { return definition; } }

        public int ID { get { return baseInst.ID; } }
        public VobTypes VobType { get { return baseInst.VobType; } }
        public bool IsStatic { get { return baseInst.IsStatic; } }

        #endregion

        protected BaseVobInst(BaseVob baseInst, PacketReader stream) : this(baseInst)
        {
            baseInst.ReadStream(stream);
        }

        protected BaseVobInst(BaseVob baseInst)
        {
            if (baseInst == null)
                throw new ArgumentNullException("BaseInst is null!");

            this.baseInst = baseInst;
            this.baseInst.ScriptObject = this;
        }

        public void Spawn(WorldInst world)
        {
            this.baseInst.Spawn(world.BaseWorld);
        }

        public void Delete()
        {
            baseInst.Despawn();
        }

        public virtual void OnReadProperties(PacketReader stream)
        {
            this.definition = (BaseVobDef)baseInst.Instance.ScriptObject;
        }
        public virtual void OnWriteProperties(PacketWriter stream) { }
    }
}
