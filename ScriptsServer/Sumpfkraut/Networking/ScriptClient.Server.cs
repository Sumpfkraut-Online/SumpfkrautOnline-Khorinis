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

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        public void OnReadMenuMsg(PacketReader stream)
        {
            Logger.Log("Login!");

            NPCDef def = BaseVobDef.Get<NPCDef>("player");
            NPCInst npc = new NPCInst(def);

            var item = new ItemInst(ItemDef.Get<ItemDef>("apple"));
            item.BaseInst.SetAmount(42);
            npc.BaseInst.Inventory.Add(item.BaseInst);
            npc.BaseInst.Inventory.Add((new ItemInst(ItemDef.Get<ItemDef>("apple"))).BaseInst);
            npc.BaseInst.Inventory.Add((new ItemInst(ItemDef.Get<ItemDef>("apple"))).BaseInst);

            ScriptOverlay ov;
            if (!npc.Definition.Model.TryGetOverlay(0, out ov))
            {
                throw new Exception("Wo ist nur das Overlay hin?");
            }
            npc.ApplyOverlay(ov);

            SetControl(npc);
            npc.Spawn(WorldInst.NewWorld);
        }

        public void OnReadIngameMsg(PacketReader stream)
        {
            /*NPCInst newNPC = null;
            this.baseClient.Character.World.ForEachVob(v =>
            {
                if (v.ID > baseClient.Character.ID && newNPC == null && v is WorldObjects.NPC && v != baseClient.Character)
                {
                    newNPC = (NPCInst)v.ScriptObject;
                }
            });

            if (newNPC == null)
            {
                this.baseClient.Character.World.ForEachVob(v =>
                {
                    if (v.ID < baseClient.Character.ID && newNPC == null && v is WorldObjects.NPC && v != baseClient.Character)
                    {
                        newNPC = (NPCInst)v.ScriptObject;
                    }
                });
            }
            SetControl(newNPC);*/

            ItemInst apple = new ItemInst(ItemDef.Get<ItemDef>("apple"));
            baseClient.Character.Inventory.Add(apple.BaseInst);
        }

        public void SetControl(NPCInst npc)
        {
            BaseClient.SetControl(npc.BaseInst);
        }
    }
}
