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
