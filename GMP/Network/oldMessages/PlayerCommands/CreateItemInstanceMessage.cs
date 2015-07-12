using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;

namespace GUC.Network.Messages.PlayerCommands
{
    class CreateItemInstanceMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            ItemInstance ii = new ItemInstance();
            ii.Read(stream);
            ItemInstance.addItemInstance(ii);
        }
    }
}
