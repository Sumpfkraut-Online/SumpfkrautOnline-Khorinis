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
        
        SpectatorMessage,
        SpecPosMessage,
        PlayerControlMessage, // npc control
        LoadWorldMessage, // for changing the world

        // Messages for Scripts
        ScriptMessage,
        ScriptVobMessage,

        // world & spawns
        WorldCellMessage, // for changing cells
        WorldSpawnMessage, // for spawning a vob in the world
        WorldDespawnMessage, // for despawning a vob in the world
        WorldTimeMessage, // to set the world's time
        WorldTimeStartMessage, // to start/stop the world's time
        WorldWeatherMessage, // to set the world's weather
        WorldWeatherTypeMessage, // rain & snow

        //vobs
        VobPosDirMessage, // updating position and direction of a vob

        // npcs 
        NPCStateMessage,
        NPCEquipMessage,
        NPCUnequipMessage,
        NPCApplyOverlayMessage,
        NPCRemoveOverlayMessage,
        NPCAniStartMessage,
        NPCAniStopMessage,
        NPCHealthMessage,

        //inventory
        InventoryAddMessage, // add item to player inventory
        InventoryRemoveMessage, // remove item from player inventory
        InventoryEquipMessage,
        InventoryUnequipMessage,








        PlayerPickUpItemMessage,
        PlayerAttributeMSMessage,
        PlayerAttributeMessage,
                
        //mobs
        MobUseMessage,
        MobUnUseMessage,

        //npcs
        NPCJumpMessage,



        NPCDrawItemMessage,
        NPCDrawFistsMessage,
        
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
