using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Types;
using WinApi;
using Gothic.zClasses;

namespace GUC.Network.Messages.NpcCommands
{
    class NPCEnableMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID = 0;
            Vec3f position = new Vec3f();
            bool enabled = false;

            stream.Read(out plID);
            stream.Read(out position);
            stream.Read(out enabled);


            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPC!");

            //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Enable: "+enabled, 0, "Program.cs", 0);

            if(enabled)
                ((NPCProto)vob).Enable(position);
            else
                ((NPCProto)vob).Disable();
        }
    }
}
