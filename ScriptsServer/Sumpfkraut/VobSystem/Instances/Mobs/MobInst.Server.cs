using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions.Mobs;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs
{
    partial class MobInst
    {
        public MobInst()
        {

        }

        public MobInst(MobDef def) : this()
        {
            this.Definition = def;
        }

        /// <summary>
        /// Calls a callback on client when starting animation ended
        /// </summary>
        public void StartAnimation(NPCInst npcInst, ScriptAniJob scriptAniJob)
        {
            npcInst.ModelInst.StartAniJob(scriptAniJob
               /*() =>
               {
                   Log.Logger.Log("Send start animation end message");
                   var strm = npcInst.BaseInst.GetScriptVobStream();
                   strm.Write((byte)ScriptVobMessageIDs.StartUsingMob);
                   npcInst.BaseInst.SendScriptVobStream(strm);
               }*/
           );
        }

        /// <summary>
        /// Calls a callback on client when stop using animation ended
        /// </summary>
        public void StopAnimation(NPCInst npcInst, ScriptAniJob scriptAniJob)
        {
            npcInst.ModelInst.StartAniJob(scriptAniJob,
               () =>
               {
                   Log.Logger.Log("Send stop animation end message");
                   var strm = npcInst.BaseInst.GetScriptVobStream();
                   strm.Write((byte)ScriptVobMessageIDs.StopUsingMob);
                   npcInst.BaseInst.SendScriptVobStream(strm);
               }
           );
        }
    }
}
