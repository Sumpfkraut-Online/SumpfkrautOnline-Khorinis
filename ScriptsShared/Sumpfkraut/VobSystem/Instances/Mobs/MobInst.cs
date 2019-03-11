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

        #region Methods
        /// <summary>
        /// Called when NPC starts interacting with the mob.
        /// </summary>
        public virtual void StartUsing(NPCInst npcInst)
        {
            // implemented in inheriting class
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when NPC stops interacting with the mob.
        /// </summary>
        public virtual void StopUsing(NPCInst npcInst)
        {
            // implemented in inheriting class
            throw new NotImplementedException();
        }
        /// <summary>
        /// Check whether the npc has the requirements to use this vob. Display Feedback if not.
        /// </summary>
        public virtual bool HasRequirements(NPCInst npcInst)
        {
            return true;
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
