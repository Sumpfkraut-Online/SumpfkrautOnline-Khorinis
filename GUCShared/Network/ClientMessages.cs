using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;

namespace GUC.Network
{
    internal enum ClientMessages
    {
        RakNet_NewIncomingConnection = DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION,
        RakNet_DisconnectionNotification = DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION,
        RakNet_ConnectionLost = DefaultMessageIDTypes.ID_CONNECTION_LOST,
        RakNet_UserPackets = DefaultMessageIDTypes.ID_USER_PACKET_ENUM,

        // general
        ConnectionMessage,

        WorldLoadedMessage,

        ScriptMessage, // generic script message (menus etc)
        ScriptCommandMessage, // command sent from any npc
        ScriptCommandHeroMessage, // command sent from the hero

        GuidedVobMessage,
        GuidedNPCMessage,

        SpecatorPosMessage,
    }
}
