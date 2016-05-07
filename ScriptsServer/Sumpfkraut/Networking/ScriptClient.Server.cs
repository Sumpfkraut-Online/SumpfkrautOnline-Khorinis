using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Log;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Enumeration;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        public void OnReadMenuMsg(PacketReader stream)
        {
            Logger.Log("Login!");

            NPCDef def = BaseVobDef.Get<NPCDef>("player");
            NPCInst npc = new NPCInst(def);

            var item = new ItemInst(ItemDef.Get<ItemDef>("zweihander"));
            npc.AddItem(item);
            npc.EquipItem(1, item);

            item = new ItemInst(ItemDef.Get<ItemDef>("itar_Garde"));
            npc.AddItem(item);
            npc.EquipItem(0, item);

            /*ScriptOverlay ov;
            if (!npc.Definition.Model.TryGetOverlay(0, out ov))
            {
                throw new Exception("Wo ist nur das Overlay hin?");
            }
            npc.ApplyOverlay(ov);*/

            SetControl(npc);
            npc.Spawn(WorldInst.Current);
        }

        public void OnReadIngameMsg(PacketReader stream)
        {
        }
    }
}
