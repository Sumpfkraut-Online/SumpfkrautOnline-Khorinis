using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;

namespace GUC.Network.Messages.PlayerCommands
{
    class CreateSpellMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            Spell ii = new Spell();
            ii.Read(stream);
            Spell.addItemInstance(ii);
        }
    }
}
