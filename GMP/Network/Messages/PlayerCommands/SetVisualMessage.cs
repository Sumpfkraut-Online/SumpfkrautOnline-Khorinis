using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using GUC.WorldObjects.Character;

namespace GUC.Network.Messages.PlayerCommands
{
    class SetVisualMessage : IMessage
    {

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            String visual = "", BodyMesh = "", HeadMesh = "";
            int plID = 0, bodyTex = 0, skinColor = 0, headTex = 0, teethTex = 0;



            stream.Read(out plID);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];

            if (vob is NPCProto)
            {
                stream.Read(out visual);
                stream.Read(out BodyMesh);
                stream.Read(out bodyTex);
                stream.Read(out skinColor);
                stream.Read(out HeadMesh);
                stream.Read(out headTex);
                stream.Read(out teethTex);



                if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                    throw new Exception("Vob not found!");


                bool visualSame = false;
                if (vob.Visual.ToUpper().Trim() == visual.ToUpper().Trim())
                    visualSame = true;
                vob.Visual = visual;

                if (vob.Address == 0)
                    return;
                Process process = Process.ThisProcess();
                zCVob zVob = new zCVob(Process.ThisProcess(), vob.Address);

                oCNpc npc = new oCNpc(process, vob.Address);//oCObjectFactory.GetFactory(Process.ThisProcess()).CreateNPC(zCParser.getParser(process).GetIndex(zString.Create(process, "OTHERS_NPC")));
                if (!visualSame)
                    npc.Disable();


                if (!visualSame)
                {
                    zString visualStr = zString.Create(Process.ThisProcess(), visual);
                    npc.SetVisual(visualStr);
                    visualStr.Dispose();
                }

                npc.SetAdditionalVisuals(BodyMesh, bodyTex, skinColor, HeadMesh, headTex, teethTex, -1);
                npc.SetWeaponMode(1);
                npc.SetWeaponMode2(1);
                //npc.Guild = 59;
                //npc.SetTrueGuild(59);

                if (!visualSame)
                {
                    //npc.HumanAI.refCtr -= 1;
                    //npc.AniCtrl.refCtr -= 1;
                    //if(npc.HumanAI.refCtr == 0)
                    //    npc.HumanAI.Destroy();
                    npc.HumanAI = new oCAiHuman(process, 0);
                    npc.AniCtrl = new oCAniCtrl_Human(process, 0);

                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Orc: " + npc.IsOrc(), 0, "Program.cs", 0);

                    zVec3 pos = npc.GetPosition();
                    npc.Enable(pos);
                    pos.Dispose();
                }
            }
        }
    }
}
