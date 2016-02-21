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

        public BaseVob baseInst { get; private set; }

        public BaseVobDef Def { get; private set; }

        public int ID { get { return baseInst.ID; } }
        public VobTypes VobType { get { return baseInst.VobType; } }
        public bool IsStatic { get { return baseInst.IsStatic; } }

        #endregion

        protected BaseVobInst(BaseVobDef def)
        {
            if (def == null)
                throw new ArgumentNullException("VobDef is null!");

            this.Def = def;
        }

        protected void SetBaseInst(BaseVob inst)
        {
            if (this.baseInst != null)
                throw new Exception("Can't change BaseInst!");

            if (inst == null)
                throw new ArgumentNullException("BaseInst is null!");

            this.baseInst = inst;
        }

        protected void ReadDef(BaseVob inst, PacketReader stream)
        {
            if (stream == null)
                throw new ArgumentNullException("Stream is null!");

            SetBaseInst(inst);
            inst.ReadStream(stream); // calls OnReadProperties too!
        }

        public void Spawn(WorldInst world)
        {
            world.SpawnVob(this);
        }

        public void Delete()
        {
            baseInst.Despawn();
        }
        public virtual void OnReadProperties(PacketReader stream) { }
        public virtual void OnWriteProperties(PacketWriter stream) { }
    }
}
