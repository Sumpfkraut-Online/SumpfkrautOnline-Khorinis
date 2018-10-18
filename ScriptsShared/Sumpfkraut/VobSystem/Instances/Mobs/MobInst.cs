using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions.Mobs;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs
{
    public partial class MobInst : NamedVobInst, WorldObjects.Mob.IScriptMob
    {
        #region Constructors

        public MobInst()
        {

        }

        protected override WorldObjects.BaseVob CreateVob()
        {
            return new WorldObjects.Mob(new ModelInst(this), this);
        }

        #endregion

        #region Properties

        public override VobType VobType { get { return VobType.Mob; } }

        public new WorldObjects.Mob BaseInst { get { return (WorldObjects.Mob)base.BaseInst; } }
        
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
