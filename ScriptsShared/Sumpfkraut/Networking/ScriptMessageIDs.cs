using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    // general purpose message for client & server
    public enum ScriptMessageIDs : byte
    {

    }

    // commands the client can send
    public enum ScriptCommandMessageIDs : byte
    {
        JumpFwd,
        JumpUp,
        Climb,

        MaxGuidedMessages, // everything above can be used by guided vobs & the hero, everything below is hero exclusive

        AttackForward,
        AttackLeft,
        AttackRight,

        Parry,
        Dodge,

        TakeItem,
        DropItem,
        EquipItem,
        UnequipItem,
        UseItem,
    }

    // messages concerning the events or states of a vob in the world, sent by the server
    public enum ScriptVobMessageIDs : byte
    {
        HitMessage,
        ParryMessage
    }
}
