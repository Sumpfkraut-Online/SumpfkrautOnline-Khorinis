using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects.Collections;
using GUC.Scripting;
using GUC.Animations;

namespace GUC.Models
{
    public partial class Model : GameObject
    {
        #region ScriptObject

        public interface IScriptModel : IScriptGameObject
        {
            void Create();
            void Delete();

            void AddOverlay(Overlay overlay);
            void RemoveOverlay(Overlay overlay);

            void AddAniJob(AniJob aniJob);
            void RemoveAniJob(AniJob aniJob);
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

        public bool IsCreated { get { return this.isCreated; } }

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
        /// <summary>
        /// The Gothic visual of this Model. (case insensitive)
        /// </summary>
        public string Visual
        {
            get { return this.visual; }
            set { if (value == null) this.visual = ""; else this.visual = value.ToUpper(); }
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Visual = stream.ReadString();

            // overlays
            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                var ov = ScriptManager.Interface.CreateOverlay();
                ov.ReadStream(stream);
                this.ScriptObject.AddOverlay(ov);
            }

            // anijobs
            count = stream.ReadUShort();
            for (int i = 0; i < count; i++)
            {
                var job = ScriptManager.Interface.CreateAniJob();
                job.SetModel(this); // meh
                job.ReadStream(stream);
                job.SetModel(null);
                this.ScriptObject.AddAniJob(job);
            }
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(this.Visual);

            // overlays
            stream.Write((byte)this.overlays.Count);
            dynOvs.ForEach(ov => ov.WriteStream(stream));

            // anijobs
            stream.Write((ushort)this.dynJobs.Count);
            dynJobs.ForEach(job => job.WriteStream(stream));
        }

        #endregion

        #region Animations

        /// <summary>
        /// The upper excluded limit for animations of a model. (ushort.MaxValue + 1)
        /// </summary>
        public const int MaxAnimations = 65536;

        StaticCollection<AniJob> aniIDs = new StaticCollection<AniJob>();
        DynamicCollection<AniJob> aniJobs = new DynamicCollection<AniJob>();
        DynamicCollection<AniJob> dynJobs = new DynamicCollection<AniJob>();

        #region Add & Remove

        partial void pAddAniJob(AniJob job);
        public void AddAniJob(AniJob job)
        {
            if (job == null)
                throw new ArgumentNullException("AniJob is null!");

            if (job.IsCreated)
                throw new ArgumentException("AniJob is already added to another Model!");

            if (job.DefaultAni == null)
                throw new ArgumentException("BaseInfo of AniJob is null!");

            aniIDs.Add(job);
            aniJobs.Add(job, ref job.collID);
            dynJobs.Add(job, ref job.dynID);

            pAddAniJob(job);

            job.SetModel(this);
        }

        partial void pRemoveAniJob(AniJob job);
        public void RemoveAniJob(AniJob job)
        {
            if (job == null)
                throw new ArgumentNullException("AniJob is null!");

            if (job.Model != this)
                throw new Exception("AniJob is not from this Model!");

            aniIDs.Remove(job);
            aniJobs.Remove(ref job.collID);
            dynJobs.Remove(ref job.dynID);

            pRemoveAniJob(job);

            job.SetModel(null);
        }

        #endregion

        #region Access

        public bool ContainsAni(int id)
        {
            if (id < 0 || id >= MaxAnimations)
            {
                throw new ArgumentOutOfRangeException("ID is out of range! 0.." + MaxAnimations);
            }
            return aniIDs.ContainsID(id);
        }

        public bool TryGetAni(int id, out AniJob job)
        {
            if (id < 0 || id >= MaxAnimations)
            {
                throw new ArgumentOutOfRangeException("ID is out of range! 0.." + MaxAnimations);
            }
            return aniIDs.TryGet(id, out job);
        }



        public void ForEachAni(Action<AniJob> action)
        {
            aniJobs.ForEach(action);
        }

        public void ForEachAni(Predicate<AniJob> predicate)
        {
            aniJobs.ForEach(predicate);
        }

        public int GetAniCount() { return aniJobs.Count; }



        public void ForEachDynamicAni(Action<AniJob> action)
        {
            dynJobs.ForEach(action);
        }

        public void ForEachDynamicAni(Predicate<AniJob> predicate)
        {
            dynJobs.ForEach(predicate);
        }

        public int GetDynamicAniCount() { return dynJobs.Count; }

        #endregion

        #endregion

        #region Overlays

        /// <summary>
        /// The upper excluded limit for overlays of a model. (byte.MaxValue + 1)
        /// </summary>
        public const int MaxOverlays = 256;

        StaticCollection<Overlay> ovIDs = new StaticCollection<Overlay>(MaxOverlays);
        DynamicCollection<Overlay> overlays = new DynamicCollection<Overlay>(MaxOverlays);
        DynamicCollection<Overlay> dynOvs = new DynamicCollection<Overlay>(MaxOverlays);

        #region Add & Remove

        partial void pAddOverlay(Overlay overlay);
        public void AddOverlay(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlay.IsCreated)
                throw new ArgumentException("Overlay is already added to another Model!");

            ovIDs.Add(overlay);
            overlays.Add(overlay, ref overlay.collID);
            dynOvs.Add(overlay, ref overlay.dynID);

            pAddOverlay(overlay);

            overlay.SetModel(this);
        }

        partial void pRemoveOverlay(Overlay overlay);
        public void RemoveOverlay(Overlay overlay)
        {
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlay.Model != this)
                throw new Exception("Overlay is not from this Model!");

            ovIDs.Remove(overlay);
            overlays.Remove(ref overlay.collID);
            dynOvs.Remove(ref overlay.dynID);

            pRemoveOverlay(overlay);

            overlay.SetModel(null);
        }

        #endregion

        #region Access

        public bool ContainsOverlay(int id)
        {
            if (id < 0 || id >= MaxOverlays)
            {
                throw new ArgumentOutOfRangeException("ID is out of range! 0.." + MaxOverlays);
            }
            return aniIDs.ContainsID(id);
        }

        public bool TryGetOverlay(int id, out Overlay overlay)
        {
            if (id < 0 || id >= MaxOverlays)
            {
                throw new ArgumentOutOfRangeException("ID is out of range! 0.." + MaxOverlays);
            }
            return ovIDs.TryGet(id, out overlay);
        }



        public void ForEachOverlay(Action<Overlay> action)
        {
            overlays.ForEach(action);
        }

        public void ForEachOverlay(Predicate<Overlay> predicate)
        {
            overlays.ForEach(predicate);
        }

        public int GetOverlayCount() { return overlays.Count; }



        public void ForEachDynamicOverlay(Action<Overlay> action)
        {
            dynOvs.ForEach(action);
        }

        public void ForEachDynamicOverlay(Predicate<Overlay> predicate)
        {
            dynOvs.ForEach(predicate);
        }

        public int GetDynamicOverlayCount() { return dynOvs.Count; }

        #endregion

        #endregion
    }
}
