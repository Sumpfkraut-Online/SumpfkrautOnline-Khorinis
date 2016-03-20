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

        /// <summary>
        /// The base animation when no overlays are applied.
        /// </summary>
        public Animation BaseAni;

        /// <summary>
        /// List of overlay animations.
        /// </summary>
        public List<Animation> OverlayAnis;

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

        #region Access

        public Animation GetAni(List<int> overlays = null)
        {
            if (this.OverlayAnis != null && overlays != null && overlays.Count > 0)
            {
                for (int i = overlays.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < this.OverlayAnis.Count; j++)
                    {
                        if (this.OverlayAnis[j].Overlay == overlays[i])
                            return this.OverlayAnis[j];
                    }
                }
            }
            return this.BaseAni;
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
            this.Name = stream.ReadString();

            this.BaseAni = ScriptManager.Interface.CreateAnimation(stream);
            if (this.BaseAni == null)
                throw new NullReferenceException("BaseInfo is null!");

            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                Animation ani = ScriptManager.Interface.CreateAnimation(stream);
                if (ani == null)
                    throw new NullReferenceException("Animation is null!");
            }
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write(this.Name);

            this.BaseAni.WriteStream(stream);

            if (this.OverlayAnis == null)
            {
                stream.Write((byte)0);
            }
            else
            {
                stream.Write((byte)this.OverlayAnis.Count);
                for (int i = 0; i < this.OverlayAnis.Count; i++)
                {
                    this.OverlayAnis[i].WriteStream(stream);
                }
            }

        }

        #endregion
    }
}
