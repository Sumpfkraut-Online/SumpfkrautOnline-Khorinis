using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects
{
    public abstract partial class GameObject
    {
        public const int MAX_ID = 65536; // ushort.MaxValue + 1 => If you change this, change the ushort cast in WriteStream & ReadStream too!

        #region ScriptObject

        public partial interface IScriptGameObject
        {
            void OnWriteProperties(PacketWriter stream);
            void OnReadProperties(PacketReader stream);
        }

        public IScriptGameObject ScriptObject { get; private set; }

        #endregion

        #region Properties

        internal int id;
        public int ID { get { return id; } }

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

            this.ScriptObject = scriptObject;
        }

        #endregion

        #region Read & Write

        protected virtual void WriteProperties(PacketWriter stream)
        {
            stream.Write((ushort)this.ID); // MAX_ID!
        }

        public void WriteStream(PacketWriter stream)
        {
            this.WriteProperties(stream);
            this.ScriptObject.OnWriteProperties(stream);
        }

        protected virtual void ReadProperties(PacketReader stream)
        {
            this.id = stream.ReadUShort(); // MAX_ID!
        }

        public void ReadStream(PacketReader stream)
        {
            this.ReadProperties(stream);
            this.ScriptObject.OnReadProperties(stream);
        }

        #endregion
    }
}
