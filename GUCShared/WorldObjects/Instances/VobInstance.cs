using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Models;

namespace GUC.WorldObjects.Instances
{
    public partial class VobInstance : BaseVobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Vob; } }

        #region ScriptObject

        public partial interface IScriptVobInstance : IScriptBaseVobInstance
        {
        }

        public new IScriptVobInstance ScriptObject
        {
            get { return (IScriptVobInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties
        
        public Model Model;
        
        /// <summary>
        /// Gothic-collision against dynamic Vobs.
        /// </summary>
        public bool CDDyn = true;
        
        /// <summary>
        /// Gothic-collision against static Vobs.
        /// </summary>
        public bool CDStatic = true;

        #endregion

        #region Create

        public override void Create()
        {
            if (this.Model == null)
                throw new NullReferenceException("Model is null!");

            base.Create();
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write((ushort)this.Model.ID);
            stream.Write(this.CDDyn);
            stream.Write(this.CDStatic);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            int modelID = stream.ReadUShort();
            if (!Model.TryGet(modelID, out this.Model))
            {
                throw new Exception("Model not found! " + modelID);
            }
            this.CDDyn = stream.ReadBit();
            this.CDStatic = stream.ReadBit();
        }

        #endregion
    }
}
