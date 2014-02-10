using System;
using System.Collections.Generic;
using System.Text;
using RakNet;

namespace Network
{
    public enum NetWorkIDS : byte
    {
        Status,
        ConnectionRequest,
        PlayerListRequest,
        NewPlayerMessage,
        DisconnectedMessage,
        PlayerStatusMessage,
        PlayerStatusMessage2,
        AnimationMessage,
        AllPlayerSynchMessage,
        WeaponModeMessage,
        ItemSynchro_DoDrop,
        MobSynch,
        DownloadModulesMessage,
        ChatMessage,
        LevelChangeMessage,
        AnimationOverlayMessage,
        VisualSynchro_SetAsPlayer,
        InventorySynch,
        MagSetupSynch,
        TimeMessage,
        StaticNPCMessage,
        NPCControllerMessage,
        SpawnNPCMessage,
        SpawnNPCMessageRemoveFromWorld,
        PassivePerceptionMessage,
        AttackSynchMessage,
        FriendMessage,
        AssessPlayerMessage,
        SoundSynch,
        AssessTalkMessage,
        CommandoMessage,
        LevelDataMessage,
        ItemStealMessage,
        StartLevelChangeMessage,
        PlayerKeyMessage,
        TextBoxSendMessage,
        MobInteractDiffMessage,
        UseItemMessage
    }

    [Flags]
    public enum PSM_SentTypes : int
    {
        None = 0,
        EquipmentSent_1 = 1,
        EquipmentSent_2 = 2,
        EquipmentSent_3 = 4,
        Slot_1 = 8,
        Slot_2 = 16,
        Slot_3 = 32,
        Slot_4 = 64,
        Slot_5 = 128,
        Slot_6 = 256,
        Slot_7 = 512,
        Slot_8 = 1024,
        Slot_9 = 2048,
        appearanceSent = 4096,
        Magic = 8192,
        Scale_FatNess = 16384,
        MobInteract = 32768
        //MagicSent = 8,
        //appearanceSent = 16,
        //overlaySent = 32
    }
    [Flags]
    public enum CommandoFlags : byte
    {
        sentToAll= 1,
        sentToPlayer = 2
    }

    [Flags]
    public enum CommandoArgumentsFlags : byte
    {
        None = 0,
        Arg1 = 1,
        Arg2 = 2,
        Arg3 = 4
    }

    public enum AllPlayerSynchMessageTypes : byte
    {
        HP,
        HP_Max,
        MP,
        MP_Max,
        Str,
        Dex,

        last
    }

    public enum CommandoType : byte
    {
        SetHP = 0,
        SetMaxHP = 1,
        SetMana = 2,
        SetMaxMana = 3,
        SetTalents = 4,
        GiveItems = 5,
        SetInventory = 6,
        RemoveNPC = 7,
        SetPosition = 8,
        SetDirection = 9,
        PlayAnimation = 10,
        FreezePlayer = 11,
        SetStrength = 12,
        SetDexterity = 13,
        Revive = 14,
        ChangeWorld = 15,
        SetAngle = 16,
        EquipArmor = 17,
        EquipWeapon = 18,
        EquipRangeWeapon = 19,
        Equip = 20,
        StopAnimation = 21,

        ViewSetPosition = 22,
        ViewHide = 23,
        ViewShow = 24,
        TextureTex = 25,
        TextureSize = 26,
        TextureCreate = 27,
        ViewDestroy = 28,
        TextCreate = 29,
        TextSet = 30,
        TextSetColor = 31,
        TextSetFont = 32,
        EnablePlayerKey = 33,

        TextBoxCreate = 34,
        TextBoxSet = 35,
        TextBoxColorSet = 36,
        TextBoxFontSet = 37,
        TextBoxSendKey = 38,
        TextBoxResetKey = 39,
        TextBoxStartKey = 40,
        TextBoxCallSend = 41,
        TextBoxStartWirting = 42,
        EnableChatBox = 43,
        SetPlayerAdditionalVisual = 44,
        SetPlayerFatness = 45,
        SetVoice = 46,
        OutputSVM = 47,
        SetInstance = 48,
        
        MessagesBoxCreate = 49,
        MessagesBoxSetFont = 50,
        MessagesBoxSetLine = 51,
        SetPlayerScale = 52
    }

    [Flags]
    public enum AutoPlayerMessageTypes : byte
    {
        None = 0,
        AnimationMessage = 1,
        XYZ = 2,
        XYZ2 = 4,
        XYZ3 = 8,
        XYZ4 = 16,
        XYZ5 = 32,
        XYZ6 = 64,
        XYZ7 = 128
    }


}
