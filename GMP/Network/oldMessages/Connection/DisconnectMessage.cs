using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Network.Messages.Connection
{
    class DisconnectMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID = 0;
            stream.Read(out plID);

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is Player))
                throw new Exception("Vob is not an Player!");

            if (Player.Hero == vob)
                return;

            vob.Despawn();
            sWorld.removeVob(vob);
        }
    }
}
