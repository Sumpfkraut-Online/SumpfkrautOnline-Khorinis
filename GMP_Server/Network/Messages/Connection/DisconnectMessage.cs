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
        public void Write(BitStream stream, Server server, Player pl)
        {
            stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.DisconnectMessage);
            stream.Write(pl.ID);

            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            Scripting.Objects.Character.Player.isOnPlayerDisconnect((GUC.Server.Scripting.Objects.Character.Player)pl.ScriptingNPC);
        }
    }
}
