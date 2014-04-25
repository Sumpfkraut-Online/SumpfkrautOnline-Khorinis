using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;

namespace GUC.Network.Messages.PlayerCommands
{
    class AnimationMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            String animation = "";
            byte start = 0;
            int plID = 0;
            stream.Read(out plID);
            stream.Read(out animation);
            stream.Read(out start);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not a NPC!");

            NPCProto npcProto = (NPCProto)vob;

            if (start == 1 && !npcProto.AnimationList.Contains(animation))
                npcProto.AnimationList.Add(animation);
            else if (start == 0 && npcProto.AnimationList.Contains(animation))
                npcProto.AnimationList.Remove(animation);
            else if (start == 2)
                npcProto.Overlays.Add(animation);
            else if (start == 3)
                npcProto.Overlays.Remove(animation);
            else if (start == 4)
                npcProto.Overlays.Clear();
            else if (start == 5 && vob.Address == 0)
                npcProto.AnimationList.Clear();


            if (vob.Address == 0)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, vob.Address);
            
            
            zString str = zString.Create(process, animation);
            if(start == 1)
                npc.GetModel().StartAnimation(str);
            else if (start == 0)
                npc.GetModel().StopAnimation(str);
            else if (start == 2)
                npc.ApplyOverlay(str);
            else if (start == 3)
                npc.RemoveOverlay(str);
            else if (start == 4)
            {
                zCArray<zString> overlays = npc.ActiveOverlays;
                int size = overlays.Size;
                for (int i = size - 1; i >= 0; i--)
                {
                    npc.RemoveOverlay(overlays.get(i));
                }
            }
            else if (start == 5)
            {
                foreach (String iAnim in npcProto.AnimationList)
                {
                    zString iStr = zString.Create(process, iAnim);
                    npc.GetModel().StopAnimation(iStr);
                    iStr.Dispose();
                }
                npcProto.AnimationList.Clear();
            }
            str.Dispose();


        }
    }
}
