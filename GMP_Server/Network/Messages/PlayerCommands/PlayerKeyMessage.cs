using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Server.Network.Messages.PlayerCommands
{
    class PlayerKeyMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int plID = 0;
            stream.Read(out plID);

            byte keyCounts = 0;
            stream.Read(out keyCounts);
            Dictionary<byte, byte> keys = new Dictionary<byte, byte>();
            for (int i = 0; i < keyCounts; i++)
            {
                byte key = 0;
                byte value = 0;
                stream.Read(out key);
                stream.Read(out value);

                keys.Add(key, value);
            }

            if (plID == 0 || !sWorld.VobDict.ContainsKey(plID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[plID];
            if (!(vob is Player))
                throw new Exception("Vob is not a Player!");

            Player proto = (Player)vob;

            Scripting.Objects.Character.Player.sOnPlayerKey((Scripting.Objects.Character.Player)proto.ScriptingNPC, keys);



        }
    }
}
