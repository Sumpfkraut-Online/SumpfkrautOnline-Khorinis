using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.Types;
using GUC.WorldObjects;

namespace GUC.Network.Messages.NpcCommands
{
    class NPCProtoSetWeaponMode : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0;
            int weaponMode = 0;
            stream.Read(out playerID);
            stream.Read(out weaponMode);

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[playerID];
            if (!(vob is NPCProto))
                throw new Exception("Vob is not an NPCProto!");

            ((NPCProto)vob).WeaponMode = weaponMode;

            if (vob.Address != 0)
            {
                ((NPCProto)vob).setWeaponMode(weaponMode);
            }
        }
    }
}
