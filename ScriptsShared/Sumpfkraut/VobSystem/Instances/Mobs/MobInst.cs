using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions.Mobs;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.Networking;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs
{
    public partial class MobInst : NamedVobInst, WorldObjects.GUCMobInst.IScriptMob
    {

        #region Properties

        public override VobType VobType { get { return VobType.Mob; } }

        new public WorldObjects.GUCMobInst BaseInst { get { return (WorldObjects.GUCMobInst)base.BaseInst; } }

        new public MobDef Definition { get { return (MobDef)base.Definition; } set { base.Definition = value; } }

        #endregion

        #region Constructors

        protected override WorldObjects.GUCBaseVobInst CreateVob()
        {
            return new WorldObjects.GUCMobInst(new ModelInst(this), this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when NPC starts interacting with the mob.
        /// </summary>
        public virtual void StartUsing(NPCInst npcInst)
        {
            // implemented in inheriting class
            Log.Logger.Log("Start using mob for this mob not implemented.");
        }

        /// <summary>
        /// Called when NPC stops interacting with the mob.
        /// </summary>
        public virtual void StopUsing(NPCInst npcInst)
        {
            // implemented in inheriting class
            Log.Logger.Log("Stop using mob for this mob not implemented.");
        }

        /// <summary>
        /// Check whether the npc has the requirements to use this vob. (Client: Otherwise display feedback).
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
