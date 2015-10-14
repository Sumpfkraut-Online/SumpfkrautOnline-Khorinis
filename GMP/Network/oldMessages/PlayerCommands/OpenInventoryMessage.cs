using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.PlayerCommands
{
    class OpenInventoryMessage : IMessage
    {

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int playerID = 0;
            bool open = false;

            stream.Read(out playerID);
            stream.Read(out open);

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[playerID];
            if (!(vob is Player))
                throw new Exception("Vob is not a Player!");

            Player player = (Player)vob;

            if (player.Address != 0)
            {
                if (open)
                    new oCNpc(Process.ThisProcess(), player.Address).OpenInventory(0);
                else
                    new oCNpc(Process.ThisProcess(), player.Address).CloseInventory();
            }
        }
    }
}
