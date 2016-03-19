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

        /// <summary>
        /// The upper (excluded) limit for GameObject-IDs (ushort.MaxValue+1).
        /// </summary>
        public const int MAX_ID = 65536;

        public virtual int ID
        {
            get { return id; }
            set
            {
                if (this.isCreated)
                    throw new Exception("The ID can't be changed while the object is created/spawned.");

                this.id = value;
            }
        }
        int id = -1;

        /// <summary>
        /// Static objects will not be communicated via the GUC-Base.
        /// </summary>
        public virtual bool IsStatic
        {
            get { return this.isStatic; }
            set
            {
                if (this.isCreated)
                    throw new Exception("The IsStatic-property can't be changed while the object is created/spawned.");
                
                this.isStatic = value;
            }
        }
        bool isStatic = false;

        #endregion

        #region Collection

        protected bool isCreated = false;
        internal int collID = -1;
        internal int dynID = -1;

        #endregion

        #region Read & Write

        protected virtual void WriteProperties(PacketWriter stream)
        {
            stream.Write((ushort)id);
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

        protected virtual void ReadProperties(PacketReader stream)
        {
            this.id = stream.ReadUShort();
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
