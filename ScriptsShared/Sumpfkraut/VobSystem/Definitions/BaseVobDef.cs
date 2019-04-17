using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects;
using GUC.WorldObjects.Definitions;
using GUC.Network;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef : ExtendedObject, GUCBaseVobDef.IScriptBaseVobInstance
    {
        #region Constructors
        
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

        // Definition
        GUCBaseVobDef baseDef;
        public GUCBaseVobDef BaseDef { get { return this.baseDef; } }
        protected abstract GUCBaseVobDef CreateVobInstance();

        public int ID { get { return BaseDef.ID; } }
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
            GUCBaseVobDef instance;
            if (GUCBaseVobDef.TryGet(id, out instance) && instance.ScriptObject is T)
            {
                def = (T)instance.ScriptObject;
                return true;
            }
            def = default(T);
            return false;
        }

        public static void ForEachOfType(GUCVobTypes type, Action<BaseVobDef> action)
        {
            GUCBaseVobDef.ForEachOfType(type, v => action((BaseVobDef)v.ScriptObject));
        }
    }
}
