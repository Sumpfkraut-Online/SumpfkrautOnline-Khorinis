using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Network;

namespace GUC.WorldObjects
{
    public abstract partial class WorldObject : GameObject
    {
        #region ScriptObject

        public partial interface IScriptWorldObject : IScriptGameObject
        {
        }

        public new IScriptWorldObject ScriptObject
        {
            get { return (IScriptWorldObject)base.ScriptObject; }
        }

        #endregion

        #region Properties

        private BaseInstance instance;
        public BaseInstance Instance
        {
            get { return instance; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new WorldObject with the given ID or [-1] a free ID.
        /// </summary>
        protected WorldObject(IScriptGameObject scriptObject, BaseInstance instance, int id = -1) : base(scriptObject, id)
        {
            if (instance == null)
                throw new ArgumentNullException("Instance is null!");

            //if (!InstanceCollection.Contains(instance))
            // throw new Exception

            this.instance = instance;
        }

        /// <summary>
        /// Creates a new WorldObject by reading a networking stream.
        /// </summary>
        protected WorldObject(IScriptGameObject scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write((ushort)this.Instance.ID); // MAX_ID
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
            int instanceID = stream.ReadUShort(); // MAX_ID
            //this.Instance = InstanceCollection.Get(instanceID);
        }

        #endregion
    }
}
