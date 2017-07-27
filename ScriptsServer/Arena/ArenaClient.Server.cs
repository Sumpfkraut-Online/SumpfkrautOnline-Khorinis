using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        partial void pOnConnect()
        {
            this.SetToSpectator(WorldInst.Current, new Vec3f(), new Vec3f());
        }

        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessages id = (ScriptMessages)stream.ReadByte();
            switch (id)
            {
                case ScriptMessages.CharCreation:
                    ReadCharCreation(stream);
                    break;
            }
        }

        void ReadCharCreation(PacketReader stream)
        {
            CharCreationInfo info = new CharCreationInfo();
            info.Read(stream);

            NPCInst npc = new NPCInst(NPCDef.Get(info.BodyMesh == Sumpfkraut.Visuals.HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"));
            npc.UseCustoms = true;
            npc.CustomBodyTex = info.BodyTex;
            npc.CustomHeadMesh = info.HeadMesh;
            npc.CustomHeadTex = info.HeadTex;
            npc.CustomVoice = info.Voice;
            npc.CustomFatness = info.Fatness;
            npc.CustomScale = new Vec3f(info.BodyWidth, 1.0f, info.BodyWidth);
            npc.CustomName = info.Name;

            var item = new ItemInst(ItemDef.Get("ItMw_1h_Bau_Mace"));
            npc.Inventory.AddItem(item);
            npc.EquipItem(item);

            item = new ItemInst(ItemDef.Get("ITAR_Prisoner"));
            npc.Inventory.AddItem(item);
            npc.EquipItem(item);

            //Sumpfkraut.Visuals.ScriptOverlay ov;
            //if (npc.ModelDef.TryGetOverlay("1HST1", out ov))
            //    npc.ModelInst.ApplyOverlay(ov);

            npc.Spawn(WorldInst.Current);
            this.SetControl(npc);
        }
    }
}
