using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions.Mobs
{
    public  partial class MobInterDef : MobDef, WorldObjects.Instances.MobInterInstance.IScriptMobInterInstance
    {
        #region Properties 

        public override VobType VobType { get { return VobType.MobInter; } }

        // Zugriff auf BasisKlasse (MobInstance = MobDef)
        new public WorldObjects.Instances.MobInterInstance BaseDef { get { return (WorldObjects.Instances.MobInterInstance)base.BaseDef; } }

        #endregion

        #region Constructors

        public MobInterDef()
        {
        }

        protected override WorldObjects.Instances.BaseVobInstance CreateVobInstance()
        {
            return new WorldObjects.Instances.MobInterInstance(this);
        }

        #endregion

        #region Read & Write

        public override void OnReadProperties(PacketReader stream)
        {
            base.OnReadProperties(stream);
        }

        public override void OnWriteProperties(PacketWriter stream)
        {
            base.OnWriteProperties(stream);
        }

        #endregion
    }
}
