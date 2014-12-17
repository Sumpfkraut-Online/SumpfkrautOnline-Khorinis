using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Types;
using GUC.Network;

namespace GUC.Network.Messages.VobCommands
{
    class SetVobPosDirMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobID = 0;
            Vec3f pos, dir;

            stream.Read(out vobID);
            stream.Read(out pos);
            stream.Read(out dir);

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            
            vob.setDirection(dir);
            vob.setPosition(pos);
        }
    }
}
