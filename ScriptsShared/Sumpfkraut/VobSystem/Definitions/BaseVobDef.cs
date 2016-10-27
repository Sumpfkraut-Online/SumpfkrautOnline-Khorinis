using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef : ScriptObject, BaseVobInstance.IScriptBaseVobInstance
    {
        #region Constructors

        protected abstract BaseVobInstance CreateVobInstance();
        partial void pConstruct();
        public BaseVobDef()
        {
            this.baseDef = CreateVobInstance();
            if (baseDef == null)
                throw new ArgumentNullException("BaseDef is null!");
            pConstruct();
        }

        #endregion

        #region Properties

        BaseVobInstance baseDef;
        public BaseVobInstance BaseDef { get { return this.baseDef; } }

        public int ID { get { return BaseDef.ID; } }
        public VobTypes VobType { get { return BaseDef.VobType; } }
        public bool IsStatic { get { return BaseDef.IsStatic; } }
        public bool IsCreated { get { return baseDef.IsCreated; } }

        #endregion

        partial void pCreate();
        public void Create()
        {
            this.BaseDef.Create();
            pCreate();
        }

        partial void pDelete();
        public void Delete()
        {
            this.baseDef.Delete();
            pDelete();
        }

        public virtual void OnWriteProperties(PacketWriter stream) { }
        public virtual void OnReadProperties(PacketReader stream) { }

        public static bool TryGetDef<T>(int id, out T def) where T : BaseVobDef
        {
            BaseVobInstance instance;
            if (BaseVobInstance.TryGet(id, out instance) && instance.ScriptObject is T)
            {
                def = (T)instance.ScriptObject;
                return true;
            }
            def = default(T);
            return false;
        }
    }
}
