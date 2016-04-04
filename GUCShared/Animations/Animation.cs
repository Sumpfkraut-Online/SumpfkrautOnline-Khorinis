using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.Models;

namespace GUC.Animations
{
    public class Animation
    {
        #region ScriptObject

        public interface IScriptAnimation : GameObject.IScriptGameObject
        {
        }

        public IScriptAnimation ScriptObject;

        #endregion

        #region Properties

        int duration = 0;
        /// <summary>
        /// Duration of the animation in ms. (int)
        /// </summary>
        public int Duration
        {
            get { return this.duration; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Duration can't be < 0!");
                this.duration = value;
            }
        }

        int startFrame = 0;
        /// <summary>
        /// From which frame the gothic animation should start. (ushort)
        /// </summary>
        public int StartFrame
        {
            get { return this.startFrame; }
            set
            {
                if (startFrame < 0 || startFrame > ushort.MaxValue)
                    throw new ArgumentOutOfRangeException("StartFrame is out of range! 0.." + ushort.MaxValue);

                this.startFrame = value;
            }
        }

        Overlay overlay;
        /// <summary>
        /// The overlay of this animation.
        /// </summary>
        public Overlay Overlay { get { return this.overlay; } }

        AniJob aniJob;
        /// <summary>
        /// The associated AniJob of this animation.
        /// </summary>
        public AniJob AniJob { get { return this.aniJob; } }

        public bool IsCreated { get { return this.aniJob != null; } }

        internal void SetAniJob(AniJob job, Overlay overlay)
        {
            this.aniJob = job;
            this.overlay = overlay;
        }

        #endregion

        #region Read & Write

        void WriteProperties(PacketWriter stream)
        {
            stream.Write(duration);
            stream.Write((ushort)startFrame);
        }

        void ReadProperties(PacketReader stream)
        {
            this.duration = stream.ReadInt();
            this.startFrame = stream.ReadUShort();
        }

        /// <summary>
        /// Writes all base & script properties into the stream.
        /// </summary>
        public void WriteStream(PacketWriter stream)
        {
            if (stream == null)
                throw new ArgumentNullException("Stream is null!");

            this.WriteProperties(stream);
            if (this.ScriptObject != null)
            {
                this.ScriptObject.OnWriteProperties(stream);
            }
        }

        /// <summary>
        /// Reads all base & script properties into the object.
        /// </summary>
        public void ReadStream(PacketReader stream)
        {
            if (stream == null)
                throw new ArgumentNullException("Stream is null!");

            this.ReadProperties(stream);
            if (this.ScriptObject != null)
            {
                this.ScriptObject.OnReadProperties(stream);
            }
        }

        #endregion
    }
}
