using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;

namespace GUC.Network.Messages.ContainerCommands
{
    class ItemChangedMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte type = 0;
            int itemID = 0, playerID =0, amount = 0;
            stream.Read(out type);
            stream.Read(out playerID);
            stream.Read(out itemID);
            stream.Read(out amount);

            ContainerItemChanged cic = (ContainerItemChanged)type;
            if (cic == ContainerItemChanged.itemInsertedOld)
            {
                
            }
        }
    }
}
