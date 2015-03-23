using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    /** 
    * Types of network messages (data) which can be exchanged between clients and server.
    */
    public enum NetworkID : byte
    {
        ConnectionMessage, /**< Network messages concerning player connection state. */
        DisconnectMessage, /**< Network messages concerning player disconnects. */

        CreateItemInstanceMessage, /**< Network messages concerning item instantiation. */
        CreateSpellMessage, /**< Network messages concerning spell instantiation. */

        PlayerFreezeMessage, /**< Network messages concerning players being frozen (immobile). */
        AddItemMessage, /**< Network messages concerning adding items to npc inventories. */
        StartDialogAnimMessage, /**< Network messages concerning animation on dialogue start. */
        AnimationMessage, /**< Network messages concerning animations (start, stop, change). */
        NPCChangeAttributeMessage, /**< Network messages concerning attribute changes of npcs. */
        NPCChangeSkillMessage, /**< Network messages concerning skill changes of npcs. */
        NPCSpawnMessage, /**< Network messages concerning spawning npcs. */
        GuiMessage, /**< Network messages concerning GUI events. */
        OnDamageMessage, /**< Network messages concerning damage events. */
        DropUnconsciousMessage, /**< Network messages concerning the unconscious state of npcs. */
        ReviveMessage, /**< Network messages concerning npcs revival. */
        SetVisualMessage, /**< Network messages concerning changes of npc visuals. */
		SetVobChangeMessage,
        DropItemMessage, /**< Network messages concerning events which are triggered when items are dropped into the world. */
        TakeItemMessage, /**< Network messages concerning events which are triggered when world-items are taken. */
        CamToVobMessage, /**< Network messages when a camera is set/moved to a vob. */
        ChangeNameMessage, /**< Network messages concerning name changes of npcs. */
        CreateVobMessage, /**< Network messages concerning vob creation (not the final spawning process). */
        SpawnVobMessage, /**< Network messages concerning spawning vobs. */
		DespawnVobMessage,
        SetVobPositionMessage, /**< Network messages concerning vob positioning (cartesian coordinates). */
        SetVobDirectionMessage, /**< Network messages concerning vob orientation/rotation. */

        SetVobPosDirMessage, /**< Network messages concerning forced vob positioning and rotation (in world). */
        SetVobListPosDirMessage, /**< ??? Additional message for organizational purposes ???  */

        PlayerUpdateMessage, /**< ??? Doesn't seem to have a purpose due to lack of references in whole G:UC solution ??? */
        AnimationUpdateMessage, /**< Network messages concerning npc animations. */
        NPCUpdateMessage, /**< Network messages concerning npc-status-updates. */

        MobInterMessage, /**< Network messages concerning mob interaction (e.g. using an avil). */
        ItemChangeAmount, /**< Network messages concerning changing amounts of item stacks. */
        ItemRemovedByUsing, /**< Network messages concerning item removable following item use events. */
        ContainerItemChangedMessage, /**< Network messages concerning container exchange (chests, inventory, etc.). */
        ItemChangeContainer, /**< Network messages when item instances change containers. */
        ClearInventory, /**< Network messages when clearing the whole inventory. */
        TimerMessage, /**< Network messages concerning the ingame time. */
        RainMessage, /**< Network messages concerning timed rain weather events. */

        CallbackNPCCanSee, /**< Network messages concerning visibility by npcs. */
        ReadIniEntryMessage, /**< Network messages concerning ???. */
        ReadMd5Message, /**< Network messages concerning MD5 data encryption. */

        DoDieMessage, /**< Network messages concerning object/npc/player death. */
        ExitGameMessage, /**< Network messages from server to client to force game exit. */
        EquipItemMessage, /**< Network messages concerning item equipping. */

        ChangeWorldMessage, /**< Network messages concerning switching the world (go to another world-instance) */
        NPCSetInvisibleMessage, /**< Network messages concerning visibility of npcs/players for others. */
        NPCSetInvisibleName, /**< Network messages concerning the visibility of names above npcs in focus. */
        PlayVideo, /**< Network messages to enforce start of a video. */
        NPCControllerMessage, /**< Network messages to give player control over npcs (???). */
        ScaleMessage, /**< Network messages concerning npc scale (x, y, z). */
        NPCFatnessMessage, /**< Network messages concerning npc fatness. */

        PlayerKeyMessage, /**< Network messages concerning keys triggered/pressed by players. */

        NPCProtoSetWeaponMode, /**< Network messages when setting the weapon mode. */
        NPCEnableMessage, /**< Network messages concerning control of npcs given to players (???). */

        UseItemMessage, /**< Network messages concerning item use. */
        CastSpell, /**< Network messages concerning cast spells. */
        PlayEffectMessage,

        SpellInvestMessage, /**< Network messages concerning investments of an npcs to cast a spell (e.g. mana use). */

        SetSlotMessage, /**< Network messages when putting items into inventory slots of npcs. */
        CamToPlayerFront, /**< Network messages concerning camera movement to the fron of the player (use in dialogues?). */

        InterfaceOptionsMessage, /**< Network messages concerning Gothic 2 standart menus navigated by players. */

        PlayerOpenInventoryMessage,

        ChatMessage,
        TradeMessage

    }

    public enum TradeStatus : byte
    {
        Request,
        Start,
        OfferItem,
        TakeBackItem
    }

    public enum ChatTextType : byte
    {
        Say,
        Shout,
        Whisper,
        Ambient,
        OOC,
        GlobalOOC,
        Global
    }

    public enum VobChangeID : byte
    {
        CDDyn,
        CDStatic,

        TriggerTarget,
        FocusName,
        UseWithItem,

        IsLocked,
        KeyInstance,
        PickLockStr,

    }

    public enum ChangeSkillType : byte
    {
        Hitchances,
        Skill,
        Value
    }

    public enum GuiMessageType : byte
    {
        Show,
        Hide,
        SetPosition,
        Destroy,

        CreateTexture,
        CreateText,
        CreateTextBox,
        CreateTextArea,
        CreateMessageBox,
        CreateCursor,
        CreateButton,

        /*List-Items:*/
        CreateList,
        CreateListText,
        CreateListButton,
        CreateListTextBox,

        CreateText3D,
        CreateTextPlayer,


        Text3DPosition,
        Text3DAddRow,
        Text3DClear,

        SetTexture,
        SetSize,

        SetText,
        SetTextColor,
        SetTextFont,

        TextBoxStartWriting,
        TextBoxStopWriting,
        TextBoxCallSend,
        TextBoxSetStartWritingKey,
        TextBoxSetSendKey,
        TextBoxSetResetKey,

        MessageBoxAddLine,

        ButtonPressed,
        GuiEvent,
    }

    public enum ContainerItemChanged : byte
    {
        itemRemoved,
        itemInsertedNew,
        itemInsertedOld
    }
}
