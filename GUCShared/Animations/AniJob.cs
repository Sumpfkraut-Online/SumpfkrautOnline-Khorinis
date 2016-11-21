using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.Models;
using GUC.GameObjects;

namespace GUC.Animations
{
    /// <summary>
    /// An Animation-Job is a collection of Animations of different Overlays for the same Gothic-Animation-String.
    /// </summary>
    public partial class AniJob : IDObject
    {
        #region ScriptObject

        public interface IScriptAniJob : IScriptGameObject
        {
            void SetDefaultAni(Animation ani);

            void AddOverlayAni(Animation ani, Overlay overlay);
            void RemoveOverlayAni(Animation ani);
        }

        new public IScriptAniJob ScriptObject { get { return (IScriptAniJob)base.ScriptObject; } }

        #endregion

        #region Constructors

        public AniJob(IScriptAniJob scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        void CheckModelCreated()
        {
            if (this.ModelInstance != null && this.ModelInstance.IsCreated)
                throw new NotSupportedException("Can't change value when the AniJob's ModelInstance is already created!");
        }

        string name = "";
        /// <summary>
        /// The Gothic name of the animation. (case insensitive)
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                CheckModelCreated();

                if (value == null)
                    this.name = "";
                else
                    this.name = value.ToUpper();
            }
        }

        Animation defaultAni;
        /// <summary>
        /// The base animation for when no overlays are applied.
        /// </summary>
        public Animation DefaultAni { get { return this.defaultAni; } }

        ModelInstance modelInstance;
        /// <summary>
        /// The associated model of this animation.
        /// </summary>
        public ModelInstance ModelInstance { get { return this.modelInstance; } }

        /// <summary>
        /// Is true when this is added to a model.
        /// </summary>
        public bool IsCreated { get { return this.isCreated; } }

        internal void SetModel(ModelInstance modelInstance)
        {
            this.modelInstance = modelInstance;
            this.isCreated = modelInstance != null;
        }

        AniJob nextAni;
        /// <summary>
        /// The AniJob which will be played immediately after this AniJob's animation ends.
        /// Declared OnStop-Actions in the AnimatedVob's play start will be postponed to the last played Animation.
        /// </summary>
        public AniJob NextAni
        {
            get { return this.nextAni; }
            set
            {
                CheckModelCreated();

                if (value != null)
                {
                    if (value.layer != this.layer)
                        throw new ArgumentException("NextAni has to have the same layer! " + value.name + " " + this.name);
                }

                this.nextAni = value;
            }
        }

        int layer = 1;
        /// <summary>
        /// Layer number, sync with Gothic's animations pls. [0..255]
        /// Default is 1.
        /// </summary>
        public int Layer
        {
            get { return this.layer; }
            set
            {
                CheckModelCreated();
                if (value < 0 || value > byte.MaxValue)
                    throw new ArgumentOutOfRangeException("Layer id needs to be in range of [0..255]! Is " + value);
                if (this.nextAni != null && this.nextAni.layer != value)
                    throw new ArgumentException("NextAni has to have the same layer! " + this.nextAni.name + " " + this.name);

                this.layer = value;
            }
        }

        #endregion

        #region Validate Animation

        void ValidateAnimation(Animation ani)
        {
            if (ani == null)
                throw new ArgumentNullException("Animation is null!");

            if (ani.AniJob != null)
                throw new ArgumentException("Animation is already added to an AniJob!");

            if (ani.EndFrame < ani.StartFrame)
                throw new ArgumentException(string.Format("Animation's end frame is smaller than its start frame! StartFrame: {0} EndFrame: {1}", ani.StartFrame, ani.EndFrame));
        }

        #endregion

        #region Default Animation

        partial void pSetDefaultAni(Animation ani);
        /// <summary>
        /// Sets the default animation which is played when no overlays are applied.
        /// </summary>
        public void SetDefaultAni(Animation ani)
        {
            CheckModelCreated();

            ValidateAnimation(ani);

            if (this.defaultAni != null)
                this.defaultAni.SetAniJob(null, null);

            this.defaultAni = ani;
            ani.SetAniJob(this, null);

            pSetDefaultAni(ani);
        }

        #endregion

        #region Overlay-Animations

        List<Animation> overlays;

        #region Access

        /// <summary>
        /// Checks if this AniJob contains an Animation for the specified Overlay.
        /// </summary>
        public bool ContainsOverlayAni(Overlay overlay)
        {
            if (this.overlays != null)
                for (int i = 0; i < this.overlays.Count; i++)
                    if (this.overlays[i].Overlay == overlay)
                        return true;
            return false;
        }

        /// <summary>
        /// Gets the Animation of the specified Overlay.
        /// </summary>
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

        /// <summary>
        /// Loops through all Overlay-Animations of this AniJob (default ani excluded!).
        /// </summary>
        public void ForEachOverlayAni(Action<Animation> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            if (this.overlays != null)
                for (int i = 0; i < this.overlays.Count; i++)
                    action(this.overlays[i]);
        }

        /// <summary>
        /// Loops through all Overlay-Animations of this AniJob (default ani excluded!).
        /// Let the predicate return FALSE to BREAK the loop.
        /// </summary>
        public void ForEachOverlayAniPredicate(Predicate<Animation> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate is null!");

            if (this.overlays != null)
                for (int i = 0; i < this.overlays.Count; i++)
                    if (!predicate(this.overlays[i]))
                        return;
        }

        /// <summary>
        /// Gets the number of Overlay-Animations of this AniJob.
        /// </summary>
        public int OverlayAniCount { get { return this.overlays != null ? this.overlays.Count : 0; } }

        #endregion

        #region Add & Remove

        partial void pAddOverlayAni(Animation ani);
        /// <summary>
        /// Adds an Animation for the specified Overlay to this AniJob.
        /// </summary>
        public void AddOverlayAni(Animation ani, Overlay overlay)
        {
            CheckModelCreated();

            ValidateAnimation(ani);

            if (overlay == null)
                throw new ArgumentNullException("Overlay is null!");

            if (overlay.Model != this.modelInstance)
                throw new ArgumentException("Overlay is not for the same model!");

            if (overlays != null)
            {
                if (overlays.Count >= byte.MaxValue)
                    throw new ArgumentException("Overlay maximum reached! " + byte.MaxValue);
            }
            else
            {
                overlays = new List<Animation>(1);
            }

            overlays.Add(ani);
            ani.SetAniJob(this, overlay);

            pAddOverlayAni(ani);
        }

        partial void pRemoveOverlayAni(Animation ani);
        /// <summary>
        /// Removes the Overlay-Animation from this AniJob.
        /// </summary>
        public void RemoveOverlayAni(Animation ani)
        {
            CheckModelCreated();

            if (ani == null)
                throw new ArgumentNullException("Animation is null!");

            if (ani.AniJob != this)
                throw new ArgumentException("Animation is not from this AniJob!");

            if (ani.Overlay == null)
                throw new ArgumentException("Animation is no Overlay-Animation!");

            overlays.Remove(ani);
            ani.SetAniJob(null, null);

            pRemoveOverlayAni(ani);
        }

        #endregion

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
            this.Name = stream.ReadString();
            this.Layer = stream.ReadByte();
            if (stream.ReadBit())
            {
                // baseAni
                AniJob next;
                this.modelInstance.TryGetAniJob(stream.ReadUShort(), out next);
                this.nextAni = next;
            }

            if (stream.ReadBit())
            {
                // baseAni
                var ani = ScriptManager.Interface.CreateAnimation();
                this.ScriptObject.SetDefaultAni(ani);
                ani.ReadStream(stream);
            }

            if (stream.ReadBit())
            {
                // overlayAnis
                int count = stream.ReadByte();
                for (int i = 0; i < count; i++)
                {
                    int overlayID = stream.ReadByte();

                    Overlay ov;
                    if (this.modelInstance.TryGetOverlay(overlayID, out ov))
                    {
                        var ani = ScriptManager.Interface.CreateAnimation();
                        this.ScriptObject.AddOverlayAni(ani, ov);
                        ani.ReadStream(stream);
                    }
                    else
                    {
                        throw new Exception("Unknown Overlay ID!");
                    }
                }
            }
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write(this.name);
            stream.Write((byte)this.layer);
            if (this.nextAni != null)
            {
                stream.Write(true);
                stream.Write((ushort)this.nextAni.ID);
            }
            else
            {
                stream.Write(false);
            }

            if (this.defaultAni == null)
            {
                stream.Write(false);
            }
            else
            {
                stream.Write(true);
                this.defaultAni.WriteStream(stream);
            }

            if (this.overlays == null)
            {
                stream.Write(false);
            }
            else
            {
                stream.Write(true);
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
