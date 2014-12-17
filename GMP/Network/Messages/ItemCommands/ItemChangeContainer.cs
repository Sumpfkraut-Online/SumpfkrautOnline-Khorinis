using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;
using GUC.WorldObjects.Mobs;
using GUC.WorldObjects.Character;

namespace GUC.Network.Messages.ItemCommands
{
    class ItemChangeContainer : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int itemID, containerID;
            stream.Read(out itemID);
            stream.Read(out containerID);

            if (!sWorld.VobDict.ContainsKey(itemID))
                throw new Exception("ItemID was not found!: "+itemID);
            if (!sWorld.VobDict.ContainsKey(containerID))
                throw new Exception("ContainerID was not found!: " + itemID);

            Vob itemVob = sWorld.VobDict[itemID];
            Vob containerVob = sWorld.VobDict[containerID];

            if (!(itemVob is Item))
                throw new Exception("ItemVob is not an Item! "+itemVob);
            if (!(containerVob is IContainer))
                throw new Exception("Container is not an IContainer! " + containerVob);

            Item item = (Item)itemVob;
            IContainer container = (IContainer)containerVob;

            if(item.Container is MobContainer ){
                MobContainer itC = (MobContainer)item.Container;
                if (itC.Address != 0)
                {
                    new oCMobContainer(Process.ThisProcess(), itC.Address).Remove(new oCItem(Process.ThisProcess(), item.Address));
                }
            }
            else if (item.Container is NPCProto)
            {
                NPCProto itC = (NPCProto)item.Container;
                if (itC.Address != 0)
                {
                    new oCNpc(Process.ThisProcess(), itC.Address).RemoveFromInv(new oCItem(Process.ThisProcess(), item.Address), item.Amount);
                }
            }

            container.addItem(item);

            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Item-Change-Container message!", 0, "Program.cs", 0);
        }
    }
}
