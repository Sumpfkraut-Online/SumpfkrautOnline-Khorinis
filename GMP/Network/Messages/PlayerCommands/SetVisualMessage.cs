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
                NPCProto npcP = (NPCProto)vob;
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

                npcP.BodyMesh = BodyMesh;
                npcP.BodyTex = bodyTex;
                npcP.SkinColor = skinColor;
                npcP.HeadMesh = HeadMesh;
                npcP.HeadTex = headTex;
                npcP.TeethTex = teethTex;
                npcP.setWeaponMode(1);

                if (vob.Address == 0)
                    return;

                if (!visualSame)
                {
                    int oldAdress = npcP.Address;
                    npcP.Despawn();
                    npcP.Spawn(npcP.Map, npcP.Position, npcP.Direction);
                    if (Player.Hero == npcP)
                    {
                        new oCNpc(Process.ThisProcess(), npcP.Address).SetAsPlayer();

                        if (oldAdress != 0)
                        {
                            new oCNpc(Process.ThisProcess(), oldAdress).Disable();
                        }
                    }
                    npcP.Enable(npcP.Position);

                    

                    return;
                }

                



                Process process = Process.ThisProcess();
                zCVob zVob = new zCVob(Process.ThisProcess(), vob.Address);

                oCNpc npc = new oCNpc(process, vob.Address);

                npc.SetAdditionalVisuals(BodyMesh, bodyTex, skinColor, HeadMesh, headTex, teethTex, -1);
            }
        }
    }
}
