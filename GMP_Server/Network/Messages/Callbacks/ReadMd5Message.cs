using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Server.Network.Messages.Callbacks
{
    class ReadMd5Message : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int callBackID = 0, playerID = 0;
            String md5File, value;
            stream.Read(out callBackID);
            stream.Read(out playerID);
            stream.Read(out md5File);
            stream.Read(out value);

            if (!sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Player was not in the List!");

            Player proto = (Player)sWorld.VobDict[playerID];


            Scripting.Objects.Character.Player.isOnCheckMd5(callBackID, (Scripting.Objects.Character.Player)proto.ScriptingNPC, md5File, value);

        }
    }
}
