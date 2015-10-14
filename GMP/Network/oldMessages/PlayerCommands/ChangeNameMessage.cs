using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.PlayerCommands
{
    class ChangeNameMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            String name = "";
            int plID = 0;
            stream.Read(out plID);
            stream.Read(out name);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPC))
                throw new Exception("Vob is not an NPC!");
            ((NPC)vob).Name = name;

            if (vob.Address == 0)
                return;
            Process process = Process.ThisProcess();
            oCNpc pl = new oCNpc(process, vob.Address);
            if (((NPC)vob).hideName)
                pl.Name.Set("");
            else
                pl.Name.Set(name);
        }
    }
}
