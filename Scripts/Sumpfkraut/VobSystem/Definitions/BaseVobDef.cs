using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef : ScriptObject, BaseVobInstance.IScriptBaseVobInstance
    {
        #region Properties

        protected BaseVobInstance baseDef;

        public int ID { get { return baseDef.ID; } }
        public VobTypes VobType { get { return baseDef.VobType; } }
        public bool IsStatic { get { return baseDef.IsStatic; } }

        #endregion

        protected void SetBaseDef(BaseVobInstance def)
        {
            if (this.baseDef != null)
                throw new Exception("Can't change BaseDef!");

            if (def == null)
                throw new ArgumentNullException("BaseDef is null!");

            this.baseDef = def;
        }

        protected void ReadDef(BaseVobInstance def, PacketReader stream)
        {
            if (stream == null)
                throw new ArgumentNullException("Stream is null!");

            SetBaseDef(def);
            def.ReadStream(stream); // calls OnReadProperties too!
        }

        public void Create()
        {
            InstanceCollection.Add(this.baseDef);
        }

        public void Delete()
        {
            InstanceCollection.Remove(this.baseDef);
        }

        public virtual void OnWriteProperties(PacketWriter stream) { }
        public virtual void OnReadProperties(PacketReader stream) { }
    }
}
