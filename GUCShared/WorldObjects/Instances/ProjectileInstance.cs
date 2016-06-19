using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Models;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class ProjectileInstance : BaseVobInstance
    {
        public override VobTypes VobType { get { return VobTypes.Projectile; } }
        
        #region ScriptObject

        public partial interface IScriptProjectileInstance : IScriptBaseVobInstance
        {
        }

        public new IScriptProjectileInstance ScriptObject
        {
            get { return (IScriptProjectileInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        public Model Model;

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
            if (!Model.TryGet(modelID, out this.Model))
            {
                throw new Exception("Model not found! " + modelID);
            }
            this.velocity = stream.ReadFloat();
        }

        #endregion
    }
}
