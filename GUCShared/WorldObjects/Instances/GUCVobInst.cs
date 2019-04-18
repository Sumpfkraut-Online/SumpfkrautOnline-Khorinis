using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.WorldObjects.Definitions;
using GUC.Models;
using GUC.WorldObjects.VobGuiding;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class GUCVobInst : GuidedVob
    {
        #region ScriptObject

        public partial interface IScriptVob : IScriptBaseVob
        {
            void Throw(Vec3f velocity);
        }
        
        public new IScriptVob ScriptObject { get { return (IScriptVob)base.ScriptObject; } }

        #endregion

        #region Constructors

        public GUCVobInst(GUCModelInst.IScriptModelInst scriptModel, IScriptVob scriptObject) : base(scriptObject)
        {
            this.model = new GUCModelInst(this, scriptModel);
        }

        #endregion

        #region Properties
        
        /// <summary> The VobType of this Vob. </summary>
        public override GUCVobTypes VobType { get { return GUCVobTypes.Vob; } }

        public override Type DefinitionType { get { return typeof(GUCVobDef); } }
        /// <summary> The Definition of this object. </summary>
        new public GUCVobDef Definition
        {
            get { return (GUCVobDef)base.Definition; }
            set { SetDefinition(value); }
        }

        /// <summary> The ModelInstance of this vob's instance. </summary>
        public GUCModelDef ModelInstance { get { return Definition.ModelInstance; } }

        GUCModelInst model;
        /// <summary> The Model of this vob. </summary>
        public GUCModelInst Model { get { return this.model; } }
        
        /// <summary> The dynamic collision detection setting of this vob's instance. </summary>
        public bool CDDyn { get { return Definition.CDDyn; } }
        /// <summary> The static collision detection setting of this vob's instance. </summary>
        public bool CDStatic { get { return Definition.CDStatic; } }

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
