using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Enumeration;

namespace GUC.Network.Messages.VobCommands
{
    class CreateVobMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobType = 0;

            stream.Read(out vobType);

            Vob v = Vob.createVob((VobTypes)vobType);
            if (v == null)
                throw new Exception("Vobtype was not known in Vob.createVob: "+vobType);
            v.Read(stream);

            sWorld.addVob(v);
        }
    }
}
