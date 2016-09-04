using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.Models;

namespace GUC.Animations
{
    /// <summary>
    /// An Animation of an Overlay contains frame and layer information for the animation system.
    /// </summary>
    public class Animation
    {
        #region ScriptObject

        public interface IScriptAnimation : GameObject.IScriptGameObject
        {
        }

        public IScriptAnimation ScriptObject;

        #endregion

        #region Properties

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
                if (this.IsCreated)
                    throw new NotSupportedException("Can't change value when the Animation is already created!");
                if (value < 0 || value > byte.MaxValue)
                    throw new ArgumentOutOfRangeException("Layer id needs to be in range of [0..255]! Is " + value);

                this.layer = value;
            }
        }

        float startFrame = 0;
        /// <summary>
        /// From which frame the gothic animation should start.
        /// </summary>
        public float StartFrame
        {
            get { return this.startFrame; }
            set
            {
                if (this.IsCreated)
                    throw new NotSupportedException("Can't change value when the Animation is already created!");
                if (value < 0)
                    throw new ArgumentOutOfRangeException("StartFrame needs to be greater than or zero! Is " + value);

                this.startFrame = value;
            }
        }

        float endFrame = 0;
        /// <summary>
        /// At what frame the gothic animation should end. EndFrame = 0 plays the whole animation
        /// </summary>
        public float EndFrame
        {
            get { return this.endFrame; }
            set
            {
                if (this.IsCreated)
                    throw new NotSupportedException("Can't change value when the Animation is already created!");
                if (value < 0)
                    throw new ArgumentOutOfRangeException("EndFrame needs to be greater than or zero! Is " + value);

                this.endFrame = value;
            }
        }

        float fps = 25;
        /// <summary>
        /// The the frame speed.
        /// </summary>
        public float FPS
        {
            get { return this.fps; }
            set
            {
                if (this.IsCreated)
                    throw new NotSupportedException("Can't change value when the Animation is already created!");
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("FPS needs to be greater than zero! Is " + value);

                this.fps = value;
            }
        }

        /// <summary>
        /// The number of frames of this Animation.
        /// </summary>
        public float GetFrameNum() { return this.endFrame - this.startFrame; }

        /// <summary> Calculates the duration of this animation in ticks (1/10000ms). </summary>
        public long GetDuration()
        {
            return GetDuration(this.fps);
        }

        /// <summary> Calculates the duration of this animation in ticks (1/10000ms). </summary>
        public long GetDuration(float fps)
        {
            return (long)((this.endFrame - this.startFrame) / fps * TimeSpan.TicksPerSecond);
        }

        Overlay overlay;
        /// <summary>
        /// The Overlay of this Animation.
        /// </summary>
        public Overlay Overlay { get { return this.overlay; } }

        AniJob aniJob;
        /// <summary>
        /// The associated AniJob of this Animation.
        /// </summary>
        public AniJob AniJob { get { return this.aniJob; } }

        /// <summary>
        /// Is true if this Animation is added to an AniJob.
        /// </summary>
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
            stream.Write((byte)Layer);
            stream.Write(startFrame);
            stream.Write(endFrame);
            stream.Write(fps);
        }

        void ReadProperties(PacketReader stream)
        {
            this.Layer = stream.ReadByte();
            this.startFrame = stream.ReadFloat();
            this.endFrame = stream.ReadFloat();
            this.fps = stream.ReadFloat();
        }

        /// <summary>
        /// Writes all base & script properties into the stream.
        /// </summary>
        public void WriteStream(PacketWriter stream)
        {
            if (stream == null)
                throw new ArgumentNullException("Stream is null!");

            this.WriteProperties(stream);
            this.ScriptObject.OnWriteProperties(stream);
        }

        /// <summary>
        /// Reads all base & script properties into the object.
        /// </summary>
        public void ReadStream(PacketReader stream)
        {
            if (stream == null)
                throw new ArgumentNullException("Stream is null!");

            this.ReadProperties(stream);
            this.ScriptObject.OnReadProperties(stream);
        }

        #endregion
    }
}
