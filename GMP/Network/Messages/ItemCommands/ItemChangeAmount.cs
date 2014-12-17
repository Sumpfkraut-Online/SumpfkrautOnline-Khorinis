using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;
using GUC.WorldObjects.Character;
using GUC.WorldObjects.Mobs;

namespace GUC.Network.Messages.ItemCommands
{
    class ItemChangeAmount : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int itemID = 0, amount = 0;
            stream.Read(out itemID);
            stream.Read(out amount);

            if (itemID == 0 || !sWorld.VobDict.ContainsKey(itemID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[itemID];
            if (!(vob is Item))
                throw new Exception("Vob is not an Item!");

            Item item = (Item)vob;
            item.Amount = amount;


            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Item-Change-Amount message!", 0, "Program.cs", 0);
            
            if (item.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCItem gI = new oCItem(process, item.Address);
                

                if (item.Amount <= 0)
                {
                    if (item.Container is NPCProto)
                        new oCNpc(process, ((NPCProto)item.Container).Address).RemoveFromInv(gI, gI.Amount);
                    else if (item.Container is MobContainer)
                        new oCMobContainer(process, ((MobContainer)item.Container).Address).Remove(gI, gI.Amount);
                    else if (item.Container is World)
                        oCGame.Game(process).World.RemoveVob(gI);
                }

                gI.Amount = item.Amount;
            }

            if (item.Amount <= 0)
                sWorld.removeVob(item);
            
        }
    }
}
