using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Models;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class ProjectileInstance : BaseVobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Projectile; } }
        
        #region ScriptObject

        public partial interface IScriptProjectileInstance : IScriptBaseVobInstance
        {
        }

        public new IScriptProjectileInstance ScriptObject { get { return (IScriptProjectileInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public ProjectileInstance(IScriptProjectileInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        public ModelInstance Model;

        float velocity = 0.01f;
        public float Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }

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
            stream.Write(this.velocity);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            int modelID = stream.ReadUShort();
            if (!ModelInstance.TryGet(modelID, out this.Model))
            {
                throw new Exception("Model not found! " + modelID);
            }
            this.velocity = stream.ReadFloat();
        }

        #endregion
    }
}
