using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.WorldObjects.Character;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages.Connection
{
    class DisconnectMessage
    {
        public static void Write(NPC player, bool connectionLost)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.DisconnectMessage);
            stream.Write(player.ID);

            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (connectionLost)
            {
                Scripting.Objects.Character.Player.isOnConnectionLost((Scripting.Objects.Character.Player)player.ScriptingNPC);
            }
            else
            {
                Scripting.Objects.Character.Player.isOnPlayerDisconnect((GUC.Server.Scripting.Objects.Character.Player)player.ScriptingNPC);
            }
        }
    }
}
