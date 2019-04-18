using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects;
using GUC.WorldObjects.Collections;
using GUC.GameObjects.Collections;
using GUC.Types;

namespace GUC.WorldObjects.Definitions
{
    /// <summary>
    /// A VobInstance is used to define a Vob's default settings. 
    /// </summary>
    public abstract partial class GUCBaseVobDef : IDObject, VobTypeObject
    {
        public abstract GUCVobTypes VobType { get; }

        #region ScriptObject

        public partial interface IScriptBaseVobInstance : IScriptGameObject
        {
            void Create();
            void Delete();
            byte GetVobType();
        }
        
        /// <summary> The ScriptObject of this object. </summary>
        public new IScriptBaseVobInstance ScriptObject { get { return (IScriptBaseVobInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public GUCBaseVobDef(IScriptBaseVobInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        /// <summary> Checks whether this Instance is added to the static Instance collection. </summary>
        public bool IsCreated { get { return this.isCreated; } }

        #endregion

        #region Static Collection

        internal int collTypeID = -1;
        internal int dynTypeID = -1;

        static StaticCollection<GUCBaseVobDef> idColl = new StaticCollection<GUCBaseVobDef>();

        static VobTypeCollection<GUCBaseVobDef> definitions = new VobTypeCollection<GUCBaseVobDef>();
        static VobTypeCollection<GUCBaseVobDef> dynDefinitions = new VobTypeCollection<GUCBaseVobDef>();

        #region Create & Delete

        partial void pAfterCreate();
        /// <summary> Adds this Instance to the static Instance collection. </summary>
        public virtual void Create()
        {
            if (this.isCreated)
                throw new ArgumentException("Instance is already in the collection!");
            
            idColl.Add(this);
            
            definitions.Add(this, ref this.collID, ref this.collTypeID);

            if (!this.IsStatic)
            {
                dynDefinitions.Add(this, ref this.dynID, ref this.dynTypeID);
            }

            this.isCreated = true;

            pAfterCreate();
        }

        partial void pBeforeDelete();
        /// <summary> Removes this Instance from the static Instance collection. </summary>
        public virtual void Delete()
        {
            if (!this.isCreated)
                throw new ArgumentException("Instance is not in the collection!");
            
            pBeforeDelete();

            this.isCreated = false;

            idColl.Remove(this);
            definitions.Remove(this, ref this.collID, ref this.collTypeID);

            if (!this.IsStatic)
            {
                dynDefinitions.Remove(this, ref this.dynID, ref this.dynTypeID);
            }
        }

        #endregion

        #region Access

        /// <summary> Gets any Instance with the given ID or null from the static Instance collection. </summary>
        public static bool TryGet(int id, out GUCBaseVobDef definition)
        {
            return idColl.TryGet(id, out definition);
        }

        /// <summary> Gets an Instance of a specific type with the given ID or null from the static Instance collection. </summary>
        public static bool TryGet<T>(int id, out T instance) where T : GUCBaseVobDef
        {
            return idColl.TryGet(id, out instance);
        }

        /// <summary> Loops through all Instances in the static Instance collection. </summary>
        public static void ForEach(Action<GUCBaseVobDef> action)
        {
            definitions.ForEach(action);
        }

        /// <summary> Gets the count of all Instances in the static Instance collection. </summary>
        public static int GetCount()
        {
            return definitions.GetCount();
        }

        /// <summary> Loops through all Instances of the given type in the static Instance collection. </summary>
        public static void ForEachOfType(GUCVobTypes type, Action<GUCBaseVobDef> action)
        {
            definitions.ForEachOfType(type, action);
        }

        /// <summary> Gets the count of all Instances of the given type in the static Instance collection. </summary>
        public static int GetCountOfType(GUCVobTypes type)
        {
            return definitions.GetCountOfType(type);
        }

        public static void ForEachDynamic(Action<GUCBaseVobDef> action)
        {
            dynDefinitions.ForEach(action);
        }

        public static void ForEachDynamicOfType(GUCVobTypes type, Action<GUCBaseVobDef> action)
        {
            dynDefinitions.ForEachOfType(type, action);
        }

        public static int GetCountDynamics()
        {
            return dynDefinitions.GetCount();
        }

        public static int GetCountDynamicsOfType(GUCVobTypes type)
        {
            return dynDefinitions.GetCountOfType(type);
        }

        #endregion

        #endregion
    }
}
