using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.Models;

namespace GUC.Animations
{
    public partial class AniJob : GameObject
    {
        #region ScriptObject

        public interface IScriptAniJob : IScriptGameObject
        {
            void SetDefaultAni(Animation ani);

            void AddOverlayAni(Animation ani, Overlay overlay);
            void RemoveOverlayAni(Animation ani);
        }

        new public IScriptAniJob ScriptObject
        {
            get { return (IScriptAniJob)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        string name = "";
        /// <summary>
        /// The Gothic name of the animation. (case insensitive)
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { if (value == null) this.name = ""; else this.name = value.ToUpper(); }
        }

        Animation defaultAni;
        /// <summary>
        /// The base animation when no overlays are applied.
        /// </summary>
        public Animation DefaultAni { get { return this.defaultAni; } }

        Model model;
        /// <summary>
        /// The associated model of this animation.
        /// </summary>
        public Model Model { get { return this.model; } }

        /// <summary>
        /// Is true when this is added to a model.
        /// </summary>
        public bool IsCreated { get { return this.isCreated; } }

        internal void SetModel(Model model)
        {
            this.model = model;
            this.isCreated = model != null;
        }

        #endregion

        partial void pSetDefaultAni(Animation ani);
        public void SetDefaultAni(Animation ani)
        {
            if (ani == null)
                throw new ArgumentNullException("Animation is null!");

            if (ani.AniJob != null)
                throw new ArgumentException("Animation is already added to an AniJob!");

            if (this.defaultAni != null)
                this.defaultAni.SetAniJob(null, null);

            this.defaultAni = ani;
            ani.SetAniJob(this, null);
            pSetDefaultAni(ani);
        }

        #region Overlays

        List<Animation> overlays;

        #region Access

        public bool ContainsOverlayAni(Overlay overlay)
        {
            if (this.overlays != null)
                for (int i = 0; i < this.overlays.Count; i++)
                    if (this.overlays[i].Overlay == overlay)
                        return true;
            return false;
        }

        public bool TryGetOverlayAni(Overlay overlay, out Animation ani)
        {
            if (this.overlays != null)
                for (int i = 0; i < this.overlays.Count; i++)
                    if (this.overlays[i].Overlay == overlay)
                    {
                        ani = this.overlays[i];
                        return true;
                    }
            ani = null;
            return false;
        }

        public void ForEachOverlayAni(Action<Animation> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            if (this.overlays != null)
                for (int i = 0; i < this.overlays.Count; i++)
                    action(this.overlays[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"> Return FALSE to break the loop. </param>
        public void ForEachOverlayAni(Predicate<Animation> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate is null!");

            if (this.overlays != null)
                for (int i = 0; i < this.overlays.Count; i++)
                    if (!predicate(this.overlays[i]))
                        break;
        }

        public int GetOverlayAniCount() { return this.overlays != null ? this.overlays.Count : 0; }
        #endregion

        #region Add & Remove

        partial void pAddOverlayAni(Animation ani);
        public void AddOverlayAni(Animation ani, Overlay overlay)
        {
            if (ani == null)
                throw new ArgumentNullException("Animation is null!");
            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (ani.AniJob != null)
                throw new ArgumentException("Animation is already added to an AniJob!");

            if (overlay.Model != this.model)
                throw new ArgumentException("Overlay is not for the same model!");

            if (overlays == null)
                overlays = new List<Animation>(1);

            overlays.Add(ani);

            ani.SetAniJob(this, overlay);
            pAddOverlayAni(ani);
        }

        partial void pRemoveOverlayAni(Animation ani);
        public void RemoveOverlayAni(Animation ani)
        {
            if (ani == null)
                throw new ArgumentNullException("Animation is null!");

            if (ani.AniJob != this)
                throw new ArgumentException("Animation is not from this AniJob!");

            if (ani.Overlay == null)
                throw new ArgumentException("Animation is BaseAni!");

            pRemoveOverlayAni(ani);

            overlays.Remove(ani);
            ani.SetAniJob(null, null);
        }

        #endregion

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
            this.Name = stream.ReadString();

            // baseAni
            var ani = ScriptManager.Interface.CreateAnimation();
            this.ScriptObject.SetDefaultAni(ani);
            ani.ReadStream(stream);

            // overlayAnis
            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                int overlayID = stream.ReadByte();

                Overlay ov;
                if (this.model.TryGetOverlay(overlayID, out ov))
                {
                    ani = ScriptManager.Interface.CreateAnimation();
                    this.ScriptObject.AddOverlayAni(ani, ov);
                    ani.ReadStream(stream);
                }
                else
                {
                    throw new Exception("Unknown Overlay ID!");
                }
            }
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write(this.Name);

            this.DefaultAni.WriteStream(stream);

            if (this.overlays == null)
            {
                stream.Write((byte)0);
            }
            else
            {
                stream.Write((byte)this.overlays.Count);
                for (int i = 0; i < this.overlays.Count; i++)
                {
                    stream.Write((byte)this.overlays[i].Overlay.ID);
                    this.overlays[i].WriteStream(stream);
                }
            }
        }

        #endregion
    }
}
