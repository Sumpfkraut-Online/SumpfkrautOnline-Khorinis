using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects.Mobs;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class MobInst : NamedVobInst, Mob.IScriptMob
    {
        #region Constructors

        public MobInst()
        {
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return new MobInstEffectHandler(null, null, this);
        }

        protected override BaseVob CreateVob()
        {
            return new Mob(new ModelInst(this), this);
        }

        #endregion

        #region Properties

        public new Mob BaseInst { get { return (Mob)base.BaseInst; } }

        new public MobInstEffectHandler EffectHandler { get { return (MobInstEffectHandler)base.EffectHandler; } }
        new public MobDef Definition { get { return (MobDef)base.Definition; } set { base.Definition = value; } }

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
