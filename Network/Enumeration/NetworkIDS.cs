using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    /** 
    * Types of network messages (data) which can be exchanged between clients and server.
    */
    internal enum NetworkIDs : byte
    {
        // general
        ConnectionMessage,
        
        // instances
        InstanceCreateMessage,
        InstanceDeleteMessage,

        // npc control
        PlayerControlMessage,

        // Messages for Scripts
        MenuMessage,
        IngameMessage,

        // world & spawns
        WorldCellMessage,
        WorldSpawnMessage,
        WorldDespawnMessage,

        //vobs
        VobPosDirMessage,





        PlayerPickUpItemMessage,
        PlayerAttributeMSMessage,
        PlayerAttributeMessage,

        //world
        WorldVobSpawnMessage,
        WorldVobDeleteMessage,
        WorldNPCSpawnMessage,
        WorldItemSpawnMessage,
        WorldTimeMessage,
        WorldWeatherMessage,
        
        //mobs
        MobUseMessage,
        MobUnUseMessage,

        //npcs
        NPCAniStartMessage,
        NPCAniStopMessage,
        NPCEquipMessage,
        NPCUnequipMessage,
        NPCJumpMessage,

        NPCStateMessage,
        NPCTargetStateMessage,
        NPCDrawItemMessage,
        NPCDrawFistsMessage,
        
        NPCHealthMessage,
        NPCStatBarMessage,

        //instances
        ItemInstanceMessage,
        NPCInstanceMessage,

        //inventory
        InventoryAddMessage, //add iteminstance to player inventory
        InventoryAmountMessage, //remove
        InventoryDropItemMessage,
        InventoryUseItemMessage,

        //controller stuff
        ControlAddVobMessage,
        ControlRemoveVobMessage,
        ControlCmdMessage
    }
}
