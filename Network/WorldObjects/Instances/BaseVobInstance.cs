using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseVobInstance : GameObject, VobTypeObject
    {
        public abstract VobTypes VobType { get; }

        #region ScriptObject

        public partial interface IScriptBaseVobInstance : IScriptGameObject
        {
        }

        public new IScriptBaseVobInstance ScriptObject
        {
            get { return (IScriptBaseVobInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties
        
        public bool IsCreated { get { return this.isCreated; } }

        #endregion

        #region Collection

        internal int collTypeID = -1;
        internal int dynTypeID = -1;

        static StaticCollection<BaseVobInstance> idColl = new StaticCollection<BaseVobInstance>();

        static VobTypeCollection<BaseVobInstance> instances = new VobTypeCollection<BaseVobInstance>();
        static VobTypeCollection<BaseVobInstance> dynInstances = new VobTypeCollection<BaseVobInstance>();

        #region Create & Delete

        partial void pCreate();
        public void Create()
        {
            if (this.isCreated)
                throw new ArgumentException("Instance is already in the collection!");
            
            idColl.Add(this);
            
            instances.Add(this, ref this.collID, ref this.collTypeID);

            if (!this.IsStatic)
            {
                dynInstances.Add(this, ref this.dynID, ref this.dynTypeID);
            }

            pCreate();

            this.isCreated = true;
        }

        partial void pDelete();
        public void Delete()
        {
            if (!this.isCreated)
                throw new ArgumentException("Instance is not in the collection!");

            pDelete();

            idColl.Remove(this);
            instances.Remove(this, ref this.collID, ref this.collTypeID);

            if (!this.IsStatic)
            {
                dynInstances.Remove(this, ref this.dynID, ref this.dynTypeID);
            }
            
            this.isCreated = false;
        }

        #endregion

        #region Access

        public static bool TryGet(int id, out BaseVobInstance instance)
        {
            return idColl.TryGet(id, out instance);
        }

        public static bool TryGet<T>(int id, out T instance) where T : BaseVobInstance
        {
            return idColl.TryGet(id, out instance);
        }

        public static void ForEach(Action<BaseVobInstance> action)
        {
            instances.ForEach(action);
        }

        public static int GetCount()
        {
            return instances.GetCount();
        }

        public static void ForEachOfType(VobTypes type, Action<BaseVobInstance> action)
        {
            instances.ForEachOfType(type, action);
        }

        public static int GetCountOfType(VobTypes type)
        {
            return instances.GetCountOfType(type);
        }

        public static void ForEachDynamic(Action<BaseVobInstance> action)
        {
            dynInstances.ForEach(action);
        }

        public static void ForEachDynamicOfType(VobTypes type, Action<BaseVobInstance> action)
        {
            dynInstances.ForEachOfType(type, action);
        }

        public static int GetCountDynamics()
        {
            return dynInstances.GetCount();
        }

        public static int GetCountDynamicsOfType(VobTypes type)
        {
            return dynInstances.GetCountOfType(type);
        }

        #endregion

        #endregion
    }
}
