using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;

namespace GUC.Network.Messages.NpcCommands
{
    class ClearInventory : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0;
            stream.Read(out playerID);

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[playerID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPCProto!");

            NPCProto npc = (NPCProto)vob;

            if (npc.Address != 0)
            {
                foreach (Item item in npc.ItemList)
                {
                    Process process = Process.ThisProcess();
                    oCItem gI = new oCItem(process, item.Address);


                    new oCNpc(process, npc.Address).RemoveFromInv(gI, gI.Amount);
                    

                    gI.Amount = 0;
                }
            }

            foreach (Item item in npc.ItemList)
            {
                sWorld.removeVob(item);
            }
        }
    }
}
