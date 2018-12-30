using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract partial class BaseVobDef : ExtendedObject, BaseVobInstance.IScriptBaseVobInstance
    {
        #region Constructors
        
        public BaseVobDef ()
        {
            baseDef = CreateVobInstance();
            if (baseDef == null)
            {
                throw new ArgumentNullException("BaseDef is null!");
            }
        }

        #endregion

        #region Properties

        // for base vob interface
        public byte GetVobType () { return (byte)VobType; } 
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
        BaseVobInstance baseDef;
        public BaseVobInstance BaseDef { get { return baseDef; } }
        protected abstract BaseVobInstance CreateVobInstance();

        public int ID { get { return BaseDef.ID; } }
        public bool IsStatic { get { return BaseDef.IsStatic; } }
        public bool IsCreated { get { return baseDef.IsCreated; } }

        #endregion



        public static bool TryGetDef<T> (int id, out T def)
            where T : BaseVobDef
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

        public static void ForEachOfType (GUCVobTypes type, Action<BaseVobDef> action)
        {
            BaseVobInstance.ForEachOfType(type, v => action((BaseVobDef)v.ScriptObject));
        }

        public void Create()
        {
            BaseDef.Create();
        }

        public void Delete()
        {
            baseDef.Delete();
        }

        public virtual void OnWriteProperties(PacketWriter stream) { }

        public virtual void OnReadProperties(PacketReader stream) { }

    }
}
