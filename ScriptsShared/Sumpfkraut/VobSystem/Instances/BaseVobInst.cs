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

        new public static readonly String _staticName = "BaseVobInst (static)";

        // GUC - Base - Object
        BaseVob baseInst;
        public BaseVob BaseInst { get { return baseInst; } }

        // definition 
        BaseVobDef vobDef;
        public BaseVobDef VobDef { get { return vobDef; } }

        public int Id { get { return baseInst.ID; } }
        public VobTypes VobType { get { return baseInst.VobType; } }
        public bool IsStatic { get { return baseInst.IsStatic; } }

        #endregion

        protected BaseVobInst (BaseVob baseInst, PacketReader stream) : this(baseInst)
        {
            baseInst.ReadStream(stream);
        }

        protected BaseVobInst (BaseVob baseInst)
        {
            if (baseInst == null)
            {
                throw new ArgumentNullException("Invalid null-value provided as baseInst in constructor!");
            }

            this.baseInst = baseInst;
            this.baseInst.ScriptObject = this;
        }

        public void Spawn (WorldInst world)
        {
            baseInst.Spawn(world.BaseWorld);
        }

        public void Delete ()
        {
            baseInst.Despawn();
        }

        public virtual void OnReadProperties(PacketReader stream)
        {
            this.vobDef = (BaseVobDef) baseInst.Instance.ScriptObject;
        }

        public virtual void OnWriteProperties(PacketWriter stream)
        { }
    }
}
