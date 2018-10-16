using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects.Instances.Mobs;
using GUC.WorldObjects.Instances;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class MobDef : NamedVobDef, MobInstance.IScriptMobInstance
    {
        #region Properties 
        // Zugriff auf BasisKlasse (MobInstance = MobDef)
        new public MobInstance BaseDef { get { return (MobInstance)base.BaseDef; } }

        new public MobDefEffectHandler EffectHandler { get { return (MobDefEffectHandler)base.EffectHandler; } }
        public string FocusName { get { return this.BaseDef.FocusName; } set { this.BaseDef.FocusName = value; } }

        #endregion

        #region Constructors

        public MobDef()
        {
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return new MobDefEffectHandler(null, null, this);
        }

        protected override BaseVobInstance CreateVobInstance()
        {
            return new MobInstance(this);
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
