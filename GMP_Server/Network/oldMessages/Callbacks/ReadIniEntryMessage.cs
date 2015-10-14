using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Server.Network.Messages.Callbacks
{
    class ReadIniEntryMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, Client client)
        {
            int callBackID = 0, playerID = 0;
            String section, entry, value;
            stream.Read(out callBackID);
            stream.Read(out playerID);
            stream.Read(out section);
            stream.Read(out entry);
            stream.Read(out value);

            if (!sWorld.VobDict.ContainsKey(playerID) )
                throw new Exception("Player was not in the List!");

            Player proto = (Player)sWorld.VobDict[playerID];
            

            Scripting.Objects.Character.Player.isOnReadInit(callBackID, (Scripting.Objects.Character.Player)proto.ScriptingNPC, section, entry, value);

        }
    }
}
