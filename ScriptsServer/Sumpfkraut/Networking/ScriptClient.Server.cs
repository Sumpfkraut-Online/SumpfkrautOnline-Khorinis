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
        public static int GetCount()
        {
            return GameClient.GetCount();
        }

        public virtual void OnConnection()
        {
        }

        public virtual void OnDisconnection()
        {
        }

        public virtual void OnReadMenuMsg(PacketReader stream)
        {
            Logger.Log("Login!");

            NPCDef def = BaseVobDef.Get<NPCDef>("player");
            NPCInst npc = new NPCInst(def);

            var item = new ItemInst(ItemDef.Get<ItemDef>("zweihander"));
            npc.AddItem(item);
            npc.EquipItem(item);

            item = new ItemInst(ItemDef.Get<ItemDef>("itar_Garde"));
            npc.AddItem(item);
            npc.EquipItem(item);

            /*ScriptOverlay ov;
            if (!npc.Definition.Model.TryGetOverlay(0, out ov))
            {
                throw new Exception("Wo ist nur das Overlay hin?");
            }
            npc.ApplyOverlay(ov);*/

            SetControl(npc);
            npc.Spawn(WorldInst.Current);
        }

        public virtual void OnReadIngameMsg(PacketReader stream)
        {
        }
    }
}
