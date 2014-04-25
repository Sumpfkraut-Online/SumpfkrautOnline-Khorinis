using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;
using GUC.Hooks;

namespace GUC.Network.Messages.PlayerCommands
{
    class DropItemMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID = 0, itemID = 0;
            stream.Read(out plID);
            stream.Read(out itemID);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPC!");

            if (itemID == 0 || !sWorld.VobDict.ContainsKey(itemID))
                throw new Exception("Item not found!");
            Vob item = sWorld.VobDict[itemID];
            if (!(item is Item))
                throw new Exception("Vob is not an Item!");

            NPCProto proto = (NPCProto)vob;
            proto.DropItem((Item)item);



            if (vob.Address == 0 || item.Address == 0)
                return;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, vob.Address);
            oCItem it = new oCItem(process, item.Address);

            hNpc.dontSend = true;
            npc.DoDropVob(it);
        }
    }
}
