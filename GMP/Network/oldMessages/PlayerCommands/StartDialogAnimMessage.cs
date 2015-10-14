using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using Gothic.zTypes;

namespace GUC.Network.Messages.PlayerCommands
{
    class StartDialogAnimMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID = 0;
            stream.Read(out plID);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPC))
                throw new Exception("Vob is not an NPC!");

            if (vob.Address == 0)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, vob.Address);
            //npc.StartDialogAni();
            zString str = zString.Create(process, "T_DIALOGGESTURE_09");
            npc.GetModel().StartAnimation(str);
            str.Dispose();

            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "npc StartDialog: "+vob.Address, 0, "Client.cs", 0);
        }
    }
}
