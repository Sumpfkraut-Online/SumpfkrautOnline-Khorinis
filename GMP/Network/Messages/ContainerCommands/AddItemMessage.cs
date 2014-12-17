using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Mobs;

namespace GUC.Network.Messages.ContainerCommands
{
    class AddItemMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobID = 0;
            Item item = null;
            stream.Read(out vobID);
            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("VobID not valid!");
            Vob vob = sWorld.VobDict[vobID];
            if (!(vob is IContainer))
                throw new Exception("Vob has to be a container!");


            if (vob is MobContainer)
            {
                int itemID = 0;
                stream.Read(out itemID);
                item = (Item)sWorld.VobDict[itemID];

            }
            else
            {
                //item = new Item();
                //item.Read(stream);
                //sWorld.addVob(item);
                int itemID = 0;
                stream.Read(out itemID);
                item = (Item)sWorld.VobDict[itemID];
            }

            

            

            
            


            IContainer container = (IContainer)vob;
            container.addItem(item);
        }
    }
}
