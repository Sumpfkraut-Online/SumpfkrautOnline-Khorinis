using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Network;
using GUC.WorldObjects;

namespace GUC.Network.Messages.VobCommands
{
    class SetVobDirectionMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobID = 0;
            Vec3f dir;

            stream.Read(out vobID);
            stream.Read(out dir);

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            vob.setDirection(dir);
        }
    }
}
