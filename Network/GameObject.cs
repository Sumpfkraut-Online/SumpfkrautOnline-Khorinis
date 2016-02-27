using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC
{
    /// <summary>
    /// The lowermost object. Provides an ID and a ScriptObject.
    /// </summary>
    public abstract partial class GameObject
    {
        #region ScriptObject

        /// <summary>
        /// The underlying ScriptObject interface for all GameObjects.
        /// </summary>
        public partial interface IScriptGameObject
        {
            /// <summary>
            /// Can be used to write additional script properties when "WriteStream" is called.
            /// </summary>
            void OnWriteProperties(PacketWriter stream);

            /// <summary>
            /// Can be used to read the additional script properties written when "ReadStream" is called.
            /// </summary>
            void OnReadProperties(PacketReader stream);
        }

        /// <summary>
        /// The ScriptObject of this GameObject.
        /// </summary>
        public IScriptGameObject ScriptObject = null;

        #endregion

        #region Properties

        public virtual int ID
        {
            get { return id; }
            set { this.id = value; }
        }
        int id = -1;

        #endregion

        #region Read & Write

        protected abstract void WriteProperties(PacketWriter stream);

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

        protected abstract void ReadProperties(PacketReader stream);

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
