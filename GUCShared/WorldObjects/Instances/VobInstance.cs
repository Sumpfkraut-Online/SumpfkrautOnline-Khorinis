using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class VobInstance : BaseVobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Vob; } }

        #region ScriptObject

        public partial interface IScriptVobInstance : IScriptBaseVobInstance
        {
        }
        
        /// <summary> The ScriptObject of this object. </summary>
        public new IScriptVobInstance ScriptObject { get { return (IScriptVobInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public VobInstance(IScriptVobInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        ModelInstance modelInstance;
        public ModelInstance ModelInstance
        {
            get { return this.modelInstance; }
            set
            {
                CanChangeNow();
                this.modelInstance = value;
            }
        }

        bool cddyn = true;
        /// <summary> Gothic-Collision-Detection against dynamic Gothic-Vobs. </summary>
        public bool CDDyn
        {
            get { return this.cddyn; }
            set
            {
                CanChangeNow();
                this.cddyn = value;
            }
        }
        
        bool cdstatic = true;
        /// <summary> Gothic-Collision-Detection against static Gothic-Vobs. </summary>
        public bool CDStatic
        {
            get { return this.cdstatic; }
            set
            {
                CanChangeNow();
                this.cdstatic = value;
            }
        }

        #endregion

        #region Create

        public override void Create()
        {
            if (this.ModelInstance == null)
                throw new NullReferenceException("ModelInstance is null!");

            base.Create();
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write((ushort)this.modelInstance.ID);
            stream.Write(this.cddyn);
            stream.Write(this.cdstatic);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            int modelID = stream.ReadUShort();
            if (!ModelInstance.TryGet(modelID, out this.modelInstance))
            {
                throw new Exception("Model not found! " + modelID);
            }
            this.cddyn = stream.ReadBit();
            this.cdstatic = stream.ReadBit();
        }

        #endregion
    }
}
