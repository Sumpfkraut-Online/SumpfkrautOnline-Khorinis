using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;

namespace GUC.Network
{
    /// <summary>
    /// Types of network messages(data) which can be exchanged between clients and server.
    /// </summary>
    internal enum ServerMessages : byte
    {
        RakNet_ConnectionRequestAccepted = DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED,
        RakNet_ConnectionAttemptFailed = DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED,
        RakNet_AlreadyConnected = DefaultMessageIDTypes.ID_ALREADY_CONNECTED,
        RakNet_NoFreeIncomingConnections = DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS,
        RakNet_DisconnectionNotification = DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION,
        RakNet_ConnectionLost = DefaultMessageIDTypes.ID_CONNECTION_LOST,
        RakNet_ConnectionBanned = DefaultMessageIDTypes.ID_CONNECTION_BANNED,
        RakNet_InvalidPassword = DefaultMessageIDTypes.ID_INVALID_PASSWORD,
        RakNet_IncompatibleProtocolVersion = DefaultMessageIDTypes.ID_INCOMPATIBLE_PROTOCOL_VERSION,
        RakNet_UserPackets = DefaultMessageIDTypes.ID_USER_PACKET_ENUM,

        // general
        DynamicsMessage, // sends all dynamic vob & model instances to the client
        
        // instances
        VobInstanceCreateMessage,
        VobInstanceDeleteMessage,

        // models
        ModelInstanceCreateMessage,
        ModelInstanceDeleteMessage,
        
        // spectator
        SpectatorMessage, // lets the client know that he's a spectator

        // player control
        PlayerControlMessage, // let the client know that he's controlling a npc

        // load world
        LoadWorldMessage, // for changing the world

        // Messages for Scripts
        ScriptMessage,
        ScriptVobMessage,
        
        // world & spawns
        WorldCellMessage, // when moving in the world, sends a list of vobs to despawn and a list of vobs to spawn
        WorldJoinMessage, // when joining the world, sens a list of vobs to spawn
        WorldLeaveMessage, // tells the client to go to the main menu, despawn all vobs

        // world clock
        WorldTimeMessage, // to set the world's time
        WorldTimeStartMessage, // to start the world's time
        WorldTimeStopMessage, // to stop the world's time

        // weather controller
        WorldWeatherMessage, // to set the world's weather
        WorldWeatherTypeMessage, // rain & snow

        // barrier controller
        WorldBarrierMessage,

        // vob messages
        VobSpawnMessage, // for spawning a vob in the world
        VobDespawnMessage, // for despawning a vob in the world
        VobPosDirMessage, // updating position and direction of a vob

        // models
        ModelAniStartMessage,
        ModelAniStartFPSMessage,
        ModelAniStopMessage,
        ModelAniFadeMessage,
        ModelOverlayAddMessage,
        ModelOverlayRemoveMessage,

        // npcs 
        NPCPosDirMessage, // updating position and direction of a npc

        NPCHealthMessage,
        NPCFightModeSetMessage,
        NPCFightModeUnsetMessage,

        // equipment
        NPCEquipAddMessage,
        NPCEquipSwitchMessage,
        NPCEquipRemoveMessage,

        // player messages
        PlayerNPCEquipAddMessage, 
        PlayerInvAddItemMessage, // add item to player inventory
        PlayerInvRemoveItemMessage, // remove item from player inventory
        PlayerItemAmountMessage,

        // GuidedVobs
        GuideAddMessage,
        GuideRemoveMessage,
        GuideAddCmdMessage,
        GuideSetCmdMessage,
        GuideRemoveCmdMessage,
    }
}
