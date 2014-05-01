using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum NetworkIDS : byte
    {
        ConnectionMessage,
        DisconnectMessage,

        CreateItemInstanceMessage,
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
        NPCEnableMessage
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

    }

    public enum ContainerItemChanged : byte
    {
        itemRemoved,
        itemInsertedNew,
        itemInsertedOld
    }
}
