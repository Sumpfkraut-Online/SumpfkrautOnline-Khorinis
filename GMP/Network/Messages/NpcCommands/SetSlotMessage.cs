using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.Types;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.NpcCommands
{
    class SetSlotMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0;
            int slot = 0, itemID = 0;
            stream.Read(out playerID);
            stream.Read(out slot);
            stream.Read(out itemID);

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[playerID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPCProto!");

            Vob itemVob = null;
            Item item = null;

            if (itemID != 0)
                sWorld.VobDict.TryGetValue(itemID, out itemVob);
            if(itemVob != null && itemVob is Item)
                item = (Item)itemVob;

            //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Set Slot Item 3: " + slot + "; NewItem: " + item + ", " + itemVob + ", " + itemID + ", " + playerID, 0, "NPCProto.Client.cs", 0);
            ((NPCProto)vob).setSlotItem(slot, item);
        }
    }
}
