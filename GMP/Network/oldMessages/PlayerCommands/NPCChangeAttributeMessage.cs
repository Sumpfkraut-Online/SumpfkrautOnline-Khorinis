using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;

namespace GUC.Network.Messages.PlayerCommands
{
    class NPCChangeAttributeMessage : IMessage
    {

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID, value;
            byte attribType;

            stream.Read(out plID);
            stream.Read(out attribType);
            stream.Read(out value);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPC))
                throw new Exception("Vob is not an NPC!");

            NPC proto = (NPC)vob;
            proto.Attributes[attribType] = value;

            if (vob.Address == 0)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, vob.Address);
            npc.setAttributes(attribType, value);
        }
    }
}
