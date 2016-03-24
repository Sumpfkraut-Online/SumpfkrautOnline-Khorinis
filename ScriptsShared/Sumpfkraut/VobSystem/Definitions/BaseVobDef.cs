using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef : ScriptObject, BaseVobInstance.IScriptBaseVobInstance
    {
        #region Properties

        new public static readonly String _staticName = "BaseVobDef (static)";

        private BaseVobInstance baseDef;
        public BaseVobInstance BaseDef { get { return baseDef; } }

        public int Id { get { return baseDef.ID; } }
        public bool IsCreated { get { return baseDef.IsCreated; } }
        public bool IsStatic { get { return baseDef.IsStatic; } }
        public VobTypes VobType { get { return baseDef.VobType; } }

        #endregion

        protected BaseVobDef (BaseVobInstance baseDef, PacketReader stream) : this(baseDef)
        {
            this.baseDef.ReadStream(stream); // calls OnReadProperties too!
        }

        private BaseVobDef (BaseVobInstance baseDef)
        {
            if (baseDef == null)
            {
                throw new Exception(this.getObjName() 
                    + ": Invalid null-value provided for baseDef in constrcutor!");
            }

            this.baseDef = baseDef;
            this.baseDef.ScriptObject = this;
        }

        partial void pCreate();
        public void Create ()
        {
            baseDef.Create();
            pCreate();
        }

        partial void pDelete();
        public void Delete ()
        {
            baseDef.Delete();
            pDelete();
        }

        public virtual void OnWriteProperties(PacketWriter stream) { }
        public virtual void OnReadProperties(PacketReader stream) { }
    }
}
