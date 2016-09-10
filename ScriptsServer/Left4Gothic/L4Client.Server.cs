using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;


namespace GUC.Scripts.Left4Gothic
{
    partial class L4Client
    {
        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessageIDs id = (ScriptMessageIDs)stream.ReadByte();
            switch (id)
            {
                case ScriptMessageIDs.CharCreation:
                    ReadCharCreation(stream);
                    break;
            }
        }

        void ReadCharCreation(PacketReader stream)
        {
            CharacterInfo info = new CharacterInfo();
            info.Read(stream);
            
            NPCInst npc = new NPCInst(NPCDef.Get(info.BodyMesh == Sumpfkraut.Visuals.HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"));
            npc.UseCustoms = true;
            npc.CustomBodyTex = info.BodyTex;
            npc.CustomHeadMesh = info.HeadMesh;
            npc.CustomHeadTex = info.HeadTex;
            npc.CustomVoice = info.Voice;
            npc.CustomFatness = info.Fatness;
            npc.CustomScale = new Types.Vec3f(info.BodyWidth, 1.0f, info.BodyWidth);
            npc.CustomName = info.Name;
            
            this.SetControl(npc);
            npc.Spawn(Sumpfkraut.WorldSystem.WorldInst.Current);
            
        }
    }
}
