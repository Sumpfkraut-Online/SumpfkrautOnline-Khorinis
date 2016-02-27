using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Log;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        public bool OnValidation()
        {
            // Check MAC-, Drive-Hashs and IP for bans
            return BaseClient.MacHash != null && BaseClient.DriveHash != null && BaseClient.SystemAddress != null;
        }

        public void OnReadMenuMsg(PacketReader stream)
        {
            Logger.Log("Login!");

            NPCDef def = BaseVobDef.Get<NPCDef>("player");
            NPCInst npc = new NPCInst(def);

            SetControl(npc);
            npc.Spawn(WorldInst.NewWorld);
        }

        public void OnReadIngameMsg(PacketReader stream)
        {
        }

        public void SetControl(NPCInst npc)
        {
            BaseClient.SetControl(npc.BaseInst);
        }
    }
}
