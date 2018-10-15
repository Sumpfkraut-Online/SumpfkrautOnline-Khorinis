using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects;
using GUC.WorldObjects.Mobs;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.Mobs.Instances
{
    public class MobInst : VobInst, Mob.IScriptMob
    {
        // MobDef und MobInst sind die ScriptObjekte für MobInstance und Mob, in der Reihenfolge
        // d.h.sie müssen von MobInstance.ScriptMobInstance und Mob.ScriptMob ableiten

        public MobInst()
        {
            Log.Logger.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> CREATED MOB INST");
        }

        public new void Spawn(WorldSystem.WorldInst world)
        {
            base.Spawn(world);
            Log.Logger.Log("########################################################################################################################### Creating chair");
            new Mob( this.BaseInst.Model.ScriptObject, this);
        }

        public override void Despawn()
        {
            base.Despawn();
        }

        public override void OnReadProperties(PacketReader stream)
        {
            base.OnReadProperties(stream);
            string hello = stream.ReadString();
            Log.Logger.Log("########################################################################################################################### Readingprobs " + hello);
        }

        public override void OnWriteProperties(PacketWriter stream)
        {
            base.OnWriteProperties(stream);
            Log.Logger.Log("########################################################################################################################### Writing probs");
        }
    }
}
