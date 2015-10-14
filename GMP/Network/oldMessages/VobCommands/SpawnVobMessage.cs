using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Types;

namespace GUC.Network.Messages.VobCommands
{
    class SpawnVobMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobID = 0;
            bool spawn = true;
            String map = "";
            Vec3f pos, dir;

            stream.Read(out vobID);
            stream.Read(out spawn);

            stream.Read(out map);
            stream.Read(out pos);
            stream.Read(out dir);

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];
            sWorld.getWorld(map).addVob(vob);
            vob.Spawn(map ,pos, dir);
            
        }
    }
}
