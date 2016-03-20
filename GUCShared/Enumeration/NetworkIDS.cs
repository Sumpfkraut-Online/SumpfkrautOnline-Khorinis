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

        // models
        ModelCreateMessage,
        ModelDeleteMessage,
        
        PlayerControlMessage, // npc control
        LoadWorldMessage, // for changing the world

        // Messages for Scripts
        MenuMessage,
        IngameMessage,

        // world & spawns
        WorldCellMessage, // for changing cells
        WorldSpawnMessage, // for spawning a vob in the world
        WorldDespawnMessage, // for despawning a vob in the world

        //vobs
        VobPosDirMessage, // updating position and direction of a vob

        // npcs 
        NPCStateMessage,
        NPCEquipMessage,
        NPCUnequipMessage,
        NPCApplyOverlayMessage,
        NPCRemoveOverlayMessage,

        //inventory
        InventoryAddMessage, // add item to player inventory
        InventoryRemoveMessage, // remove item from player inventory
        InventoryEquipMessage,
        InventoryUnequipMessage,




        PlayerPickUpItemMessage,
        PlayerAttributeMSMessage,
        PlayerAttributeMessage,

        //world
        WorldTimeMessage,
        WorldWeatherMessage,
        
        //mobs
        MobUseMessage,
        MobUnUseMessage,

        //npcs
        NPCAniStartMessage,
        NPCAniStopMessage,
        NPCJumpMessage,



        NPCDrawItemMessage,
        NPCDrawFistsMessage,
        
        NPCHealthMessage,
        NPCStatBarMessage,

        //instances
        ItemInstanceMessage,
        NPCInstanceMessage,

        //inventory
        InventoryAmountMessage, //remove
        InventoryDropItemMessage,
        InventoryUseItemMessage,

        //controller stuff
        ControlAddVobMessage,
        ControlRemoveVobMessage,
        ControlCmdMessage
    }
}
