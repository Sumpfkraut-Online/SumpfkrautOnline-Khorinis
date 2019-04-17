using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions.Mobs
{
    public partial class MobDef : NamedVobDef, WorldObjects.Instances.GUCMobDef.IScriptMobInstance
    {
        #region Properties 

        public override VobType VobType { get { return VobType.Mob; } }

        // Zugriff auf BasisKlasse (MobInstance = MobDef)
        new public WorldObjects.Instances.GUCMobDef BaseDef { get { return (WorldObjects.Instances.GUCMobDef)base.BaseDef; } }
        
        public string FocusName { get { return this.BaseDef.FocusName; } set { this.BaseDef.FocusName = value; } }

        #endregion

        #region Constructors

        public MobDef()
        {
        }
        
        protected override WorldObjects.Instances.GUCBaseVobDef CreateVobInstance()
        {
            return new WorldObjects.Instances.GUCMobDef(this);
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
