using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.Models
{
    public partial class Model : GameObject
    {
        #region ScriptObject

        public interface IScriptModel : IScriptGameObject
        {
        }

        new public IScriptModel ScriptObject
        {
            get { return (IScriptModel)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Collection
        
        static StaticCollection<Model> idColl = new StaticCollection<Model>();
        static DynamicCollection<Model> models = new DynamicCollection<Model>();
        static DynamicCollection<Model> dynModels = new DynamicCollection<Model>();

        #region Create & Delete

        partial void pCreate();
        public void Create()
        {
            if (this.isCreated)
                throw new ArgumentException("Model is already in the collection!");

            idColl.Add(this);

            models.Add(this, ref this.collID);

            if (!this.IsStatic)
            {
                dynModels.Add(this, ref this.dynID);
            }

            pCreate();

            this.isCreated = true;
        }

        partial void pDelete();
        public void Delete()
        {
            if (!this.isCreated)
                throw new ArgumentException("Model is not in the collection!");

            pDelete();

            idColl.Remove(this);
            models.Remove(ref this.collID);

            if (!this.IsStatic)
            {
                dynModels.Remove(ref this.dynID);
            }

            this.isCreated = false;
        }

        #endregion

        #region Access

        public static bool Contains(int id)
        {
            return idColl.ContainsID(id);
        }

        public static bool TryGet(int id, out Model model)
        {
            return idColl.TryGet(id, out model);
        }

        public static void ForEach(Action<Model> action)
        {
            models.ForEach(action);
        }

        public static void ForEach(Predicate<Model> predicate)
        {
            models.ForEach(predicate);
        }

        public static int GetCount()
        {
            return models.Count;
        }

        public static void ForEachDynamic(Action<Model> action)
        {
            dynModels.ForEach(action);
        }

        public static void ForEachDynamic(Predicate<Model> predicate)
        {
            dynModels.ForEach(predicate);
        }

        public static int GetCountDynamics()
        {
            return dynModels.Count;
        }

        #endregion

        #endregion

        #region Properties

        string visual = "";
        public string Visual
        {
            get { return this.visual; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Visual is null!");

                this.visual = value.ToUpper();
            }
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Visual = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(this.Visual);
        }

        #endregion

        #region Animations

        #endregion
    }
}
