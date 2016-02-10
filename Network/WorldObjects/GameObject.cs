using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects
{
    /// <summary>
    /// The lowermost object. Provides an ID and a ScriptObject.
    /// </summary>
    public abstract partial class GameObject
    {
        /// <summary>
        /// The upper (excluded) limit for GameObject-IDs (ushort.MaxValue + 1).
        /// </summary>
        public const int MAX_ID = 65536; // ushort.MaxValue + 1 => If you change this, change the ushort cast in WriteStream & ReadStream too!

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
        public IScriptGameObject ScriptObject { get { return this.scriptObject; } }
        IScriptGameObject scriptObject;

        #endregion

        #region Properties

        /// <summary>
        /// The ID of this GameObject. Range is ushort (0...65535).
        /// </summary>
        public int ID { get { return id; } }
        internal int id;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ServerObject with the given ID or [-1] a free ID.
        /// </summary>
        protected GameObject(IScriptGameObject scriptObject, int id = -1) : this(scriptObject)
        {
            this.id = id;
        }

        /// <summary>
        /// Creates a new ServerObject by reading a networking stream.
        /// </summary>
        protected GameObject(IScriptGameObject scriptObject, PacketReader stream) : this(scriptObject)
        {
            this.ReadStream(stream);
        }

        private GameObject(IScriptGameObject scriptObject)
        {
            if (scriptObject == null)
                throw new ArgumentNullException("WorldObject: ScriptObject is null!");

            this.scriptObject = scriptObject;
        }

        #endregion

        #region Read & Write

        protected virtual void WriteProperties(PacketWriter stream)
        {
            stream.Write((ushort)this.ID); // MAX_ID!
        }

        /// <summary>
        /// Writes all base & script properties into the stream.
        /// </summary>
        public void WriteStream(PacketWriter stream)
        {
            this.WriteProperties(stream);
            this.ScriptObject.OnWriteProperties(stream);
        }

        protected virtual void ReadProperties(PacketReader stream)
        {
            this.id = stream.ReadUShort(); // MAX_ID!
        }

        /// <summary>
        /// Reads all base & script properties into the object.
        /// </summary>
        public void ReadStream(PacketReader stream)
        {
            this.ReadProperties(stream);
            this.ScriptObject.OnReadProperties(stream);
        }

        #endregion
    }
}
