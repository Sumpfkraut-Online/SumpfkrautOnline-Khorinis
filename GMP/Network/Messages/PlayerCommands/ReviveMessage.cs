using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using Gothic.zTypes;
using GUC.WorldObjects;

namespace GUC.Network.Messages.PlayerCommands
{
    class ReviveMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID = 0;
            stream.Read(out plID);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPC!");

            if (vob.Address == 0)
                return;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, vob.Address);

            zVec3 pos = npc.GetPosition();
            npc.ResetPos(pos);
            pos.Dispose();
        }
    }
}
