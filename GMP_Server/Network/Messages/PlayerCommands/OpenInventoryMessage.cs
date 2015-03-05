using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects;

namespace GUC.Server.Network.Messages.PlayerCommands
{
    class OpenInventoryMessage : IMessage
    {
        public static void Write(Player player, bool open)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.PlayerOpenInventoryMessage);
            stream.Write(player.ID);
            stream.Write(open);

            using (RakNetGUID guid = player.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int playerID = 0;
            bool open = false;
            stream.Read(out playerID);
            stream.Read(out open);

            if (playerID == 0 || !sWorld.VobDict.ContainsKey(playerID))
                throw new Exception("Vob not found! ID:" + playerID);
            Vob vob = sWorld.VobDict[playerID];
            if (!(vob is Player))
                throw new Exception("Vob is not a Player!");

            if(open)
                Scripting.Objects.Character.Player.isOnOpenInventory((Scripting.Objects.Character.Player)vob.ScriptingVob);
            else
                Scripting.Objects.Character.Player.isOnCloseInventory((Scripting.Objects.Character.Player)vob.ScriptingVob);
        }
    }
}
