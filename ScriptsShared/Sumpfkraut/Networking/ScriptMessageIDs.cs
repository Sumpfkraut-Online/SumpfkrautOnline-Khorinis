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
    public enum RequestMessageIDs : byte
    {
        JumpFwd,
        JumpRun,
        JumpUp,

        ClimbLow,
        ClimbMid,
        ClimbHigh,

        MaxGuidedMessages, // everything above can be used by guided vobs & the hero, everything below is hero exclusive

        DrawFists,
        DrawWeapon,

        AttackForward,
        AttackLeft,
        AttackRight,
        AttackRun,

        Parry,
        Dodge,

        Aim,
        Unaim,
        Shoot,

        TakeItem,
        DropItem,
        EquipItem,
        UnequipItem,
        UseItem,

        OfferItem,
        RemoveItem,
        ConfirmOffer,
        DeclineOffer,
        RequestTrade,

        MaxNPCRequests,
    }

    // messages concerning the events or states of a vob in the world, sent by the server
    public enum ScriptVobMessageIDs : byte
    {
        HitMessage,
        ParryMessage,
        Climb,
        Uncon,
    }
}
