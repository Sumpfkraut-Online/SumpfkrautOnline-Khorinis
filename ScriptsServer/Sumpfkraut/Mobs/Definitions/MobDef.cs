using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Mobs;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Network;
using GUC.WorldObjects.Instances.Mobs;
using GUC.WorldObjects.Instances;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.Mobs.Definitions
{
    public class MobDef : VobDef, MobInstance.IScriptMobInstance
    {
        // Zugriff auf BasisKlasse (MobInstance = MobDef)
        new public MobInstance BaseDef { get { return (MobInstance)base.BaseDef; } }

        public string FocusName { get { return this.BaseDef.FocusName; } set { this.BaseDef.FocusName = value; } }

        public MobDef()
        {
            
        }

        public MobDef(string codename, Visuals.ModelDef model, string focusName)
            : base(codename)
        {
            Model = model;
            FocusName = focusName;
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return base.CreateHandler();
        }

        protected override BaseVobInstance CreateVobInstance()
        {
            return new MobInstance(this);
        }

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
