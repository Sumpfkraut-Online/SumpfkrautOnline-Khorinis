using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.WorldObjects.Instances;
using GUC.Models;
using GUC.WorldObjects.VobGuiding;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class Vob : GuidedVob
    {
        #region ScriptObject

        public partial interface IScriptVob : IScriptBaseVob
        {
            void Throw(Vec3f velocity);
        }
        
        public new IScriptVob ScriptObject { get { return (IScriptVob)base.ScriptObject; } }

        #endregion

        #region Constructors

        public Vob(Model.IScriptModel scriptModel, IScriptVob scriptObject) : base(scriptObject)
        {
            this.model = new Model(this, scriptModel);
        }

        #endregion

        #region Properties

        /// <summary> The VobType of this Vob. </summary>
        public override VobTypes VobType { get { return VobTypes.Vob; } }

        public override Type InstanceType { get { return typeof(VobInstance); } }
        /// <summary> The Instance of this object. </summary>
        new public VobInstance Instance
        {
            get { return (VobInstance)base.Instance; }
            set { SetInstance(value); }
        }

        /// <summary> The ModelInstance of this vob's instance. </summary>
        public ModelInstance ModelInstance { get { return Instance.ModelInstance; } }

        Model model;
        /// <summary> The Model of this vob. </summary>
        public Model Model { get { return this.model; } }
        
        /// <summary> The dynamic collision detection setting of this vob's instance. </summary>
        public bool CDDyn { get { return Instance.CDDyn; } }
        /// <summary> The static collision detection setting of this vob's instance. </summary>
        public bool CDStatic { get { return Instance.CDStatic; } }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            this.model.WriteStream(stream);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
            this.model.ReadStream(stream);
        }

        #endregion

        partial void pThrow(Vec3f velocity);
        public virtual void Throw(Vec3f velocity)
        {
            pThrow(velocity);
        }
    }
}
