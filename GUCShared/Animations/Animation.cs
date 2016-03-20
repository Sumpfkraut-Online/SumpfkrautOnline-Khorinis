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

        int overlay = 0;
        /// <summary>
        /// The overlay number of this animation. (byte)
        /// </summary>
        public int Overlay
        {
            get { return this.overlay; }
            set
            {
                if (overlay < 0 || overlay > byte.MaxValue)
                    throw new ArgumentOutOfRangeException("Overlay is out of range! 0.." + byte.MaxValue);
                this.overlay = value;
            }
        }

        int startPercent = 0;
        /// <summary>
        /// From which percentage the gothic animation should start. 255 = 100% (byte)
        /// </summary>
        public int StartPercent
        {
            get { return this.startPercent; }
            set
            {
                if (startPercent < 0 || startPercent > byte.MaxValue)
                    throw new ArgumentOutOfRangeException("StartPercent is out of range! 0.." + byte.MaxValue);

                this.startPercent = value;
            }
        }

        #endregion

        #region Read & Write

        void WriteProperties(PacketWriter stream)
        {
            stream.Write(duration);
            stream.Write((byte)overlay);
            stream.Write((byte)startPercent);
        }

        void ReadProperties(PacketReader stream)
        {
            this.duration = stream.ReadInt();
            this.overlay = stream.ReadByte();
            this.startPercent = stream.ReadByte();
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
