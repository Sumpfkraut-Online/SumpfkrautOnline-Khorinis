using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects
{
    public abstract partial class WorldObject
    {
        public const int MAX_ID = 65536; // ushort.MaxValue + 1 => If you change this, change the ushort cast in WriteStream & ReadStream too!

        #region ScriptObject

        public partial interface IScriptWorldObject
        {
            void OnWriteProperties(PacketWriter stream);
            void OnReadProperties(PacketReader stream);
        }

        public IScriptWorldObject ScriptObject { get; private set; }

        #endregion      

        #region Constructors

        /// <summary>
        /// Creates a new WorldObject with the given ID or searches a new one when needed.
        /// </summary>
        protected WorldObject(IScriptWorldObject scriptObject, int id = -1) : this(scriptObject)
        {
            this.id = id;
        }

        /// <summary>
        /// Creates a new WorldObject by reading a networking stream.
        /// </summary>
        protected WorldObject(IScriptWorldObject scriptObject, PacketReader stream) : this(scriptObject)
        {
            this.ReadStream(stream);
        }

        private WorldObject(IScriptWorldObject scriptObject)
        {
            if (scriptObject == null)
                throw new ArgumentNullException("WorldObject: ScriptObject is null!");

            this.ScriptObject = scriptObject;
        }

        #endregion

        #region Properties

        internal int id;
        public int ID { get { return id; } }

        #endregion

        #region Read & Write

        protected abstract void WriteProperties(PacketWriter stream);
        public void WriteStream(PacketWriter stream)
        {
            stream.Write((ushort)this.ID); // If you change the ushort cast, change MAX_ID too!
            this.WriteProperties(stream);
            this.ScriptObject.OnWriteProperties(stream);
        }

        protected abstract void ReadProperties(PacketReader stream);
        public void ReadStream(PacketReader stream)
        {
            this.id = stream.ReadUShort(); // If you change the ushort cast, change MAX_ID too!
            this.ReadProperties(stream);
            this.ScriptObject.OnReadProperties(stream);
        }

        #endregion
    }
}
