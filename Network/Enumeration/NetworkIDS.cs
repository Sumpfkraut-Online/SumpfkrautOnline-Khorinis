using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum NetworkID : byte
    {
        ConnectionMessage,
        DisconnectMessage,

        CreateItemInstanceMessage,
        CreateSpellMessage,

        PlayerFreezeMessage,
        AddItemMessage,
        StartDialogAnimMessage,
        AnimationMessage,
        NPCChangeAttributeMessage,
        NPCChangeSkillMessage,
        NPCSpawnMessage,
        GuiMessage,
        OnDamageMessage,
        DropUnconsciousMessage,
        ReviveMessage,
        SetVisualMessage,
        DropItemMessage,
        TakeItemMessage,
        CamToVobMessage,
        ChangeNameMessage,
        CreateVobMessage,
        SpawnVobMessage,
        SetVobPositionMessage,
        SetVobDirectionMessage,

        SetVobPosDirMessage,
        SetVobListPosDirMessage,

        PlayerUpdateMessage,
        AnimationUpdateMessage,
        NPCUpdateMessage,

        MobInterMessage,
        ItemChangeAmount,
        ItemRemovedByUsing,
        ContainerItemChangedMessage,
        ItemChangeContainer,
        ClearInventory,
        TimerMessage,
        RainMessage,
        
        CallbackNPCCanSee,
        ReadIniEntryMessage,
        ReadMd5Message,

        DoDieMessage,
        ExitGameMessage,
        EquipItemMessage,

        ChangeWorldMessage,
        NPCSetInvisibleMessage,
        NPCSetInvisibleName,
        PlayVideo,
        NPCControllerMessage,
        ScaleMessage,
        NPCFatnessMessage,
        
        PlayerKeyMessage,

        NPCProtoSetWeaponMode,
        NPCEnableMessage,

        UseItemMessage,
        CastSpell,
        PlayEffectMessage,

        SpellInvestMessage,

        SetSlotMessage,
        CamToPlayerFront,

        InterfaceOptionsMessage

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
