using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.GameObjects
{
    /// <summary>
    /// Provides an ID and a ScriptObject.
    /// </summary>
    public abstract partial class IDObject : GameObject
    {
        #region Constructors

        public IDObject(IScriptGameObject scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        protected virtual void CanChangeNow()
        {
            if (this.isCreated)
                throw new NotSupportedException("Can't change value when the object has been created/spawned!");
        }

        /// <summary>
        /// The upper (excluded) limit for IDObject-IDs (ushort.MaxValue+1).
        /// </summary>
        public const int MAX_ID = 65536;

        int id = -1;
        /// <summary> 
        /// The object's ID. Will be set automatically when the object has its default value on creation/spawn. 
        /// The ID is unique among objects of one kind and used for networking. (Default: -1) 
        /// </summary>
        public int ID
        {
            get { return id; }
            set
            {
                CanChangeNow();
                this.id = value;
            }
        }

        bool isStatic = false;
        /// <summary> Static objects will not be communicated via the GUC-Basis. (Default: False) </summary>
        public virtual bool IsStatic
        {
            get { return this.isStatic; }
            set
            {
                CanChangeNow();
                this.isStatic = value;
            }
        }

        #endregion

        #region Collection

        protected bool isCreated = false;
        internal int collID = -1;
        internal int dynID = -1;

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write((ushort)id);
        }
        
        protected override void ReadProperties(PacketReader stream)
        {
            this.id = stream.ReadUShort();
        }
        
        #endregion
    }
}
