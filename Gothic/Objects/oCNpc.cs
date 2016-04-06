using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;
using Gothic.Objects.Meshes;

namespace Gothic.Objects
{
    public class oCNpc : zCVob
    {
        new public abstract class VarOffsets : zCVob.VarOffsets
        {
            public const int guild = 0x0230,
            hp_current = 0x1B8,
            hp_max = 0x1BC,
            mp_current = 0x1C0,
            mp_max = 0x1C4,
            strength = 0x1C8,
            dexterity = 0x1CC,

            hitchances = 472,
            hc_empty = 472,
            hc_h1 = 476,
            hc_h2 = 480,
            hc_bow = 484,
            hc_crossbow = 488,

            enemy = 0x498,
            talents_array = 0x0568,
            name = 0x0124,
            Inventory = 0x0668,
            FocusVob = 0x09AC,
            DiveTime = 0x07D0,
            DiveCTR = 0x07D4,
            PercList = 0x07F4,
            percActive = 0x08FC,
            Senses = 0x0280,
            Senses_Range = 0x0284,
            AniCtrl = 0x0980,
            HumanAI = 0x097C,
            NpcStates = 0x0588,
            ActiveSpells = 0x0918,
            MagBook = 0x0914,
            activeOverlays = 0x0928,

            traderContainer = 0x0734,
            traderNPC = 0x0738,

            instance = 0x0770,
            HeadVisualString = 0x079C,
            BodyVisualString = 0x0788,
            BitFieldNPC = 0x075C,
            MDS_Name = 0x0774,
            voice_index = 0x074C,
            voice = 0x0254,
            falldowndamage = 0x910,
            flags = 0x01B4,
            fatness = 0x07BC,
            model_scale = 0x07B0,
            weaponMode = 0x250,

            InteractItem = 0x968,
            InteractItemState = 0x96C,
            InteractItemTargetState = 0x0970,

            VoiceHandleList = 0x0740;
        }

        public enum NPC_Talents
        {
            Unknown,
            H1,
            H2,
            Bow,
            CrossBow,
            Picklock,
            Pickpocket,//g1 nicht verwenden?
            Mage,
            Sneak,
            Regenerate,
            Firemaster,
            acrobat,
            pickpocketG2,
            Smith,
            Runes,
            Alchemy,
            TakeAnimalTrophy,
            Foreignlanguage
        }

        [Flags]
        public enum NPC_Flags
        {
            None = 0,
            Friend = 1,
            Immortal = 2,
            Ghost = 3
        }

        new public abstract class FuncAddresses : zCVob.FuncAddresses
        {
            new public const int SetVisual = 0x0072E3F0;
            public const int Disable = 0x00745A20,
            OpenInventory = 0x00762250,
            CloseInventory = 0x00762410,
            PutInInv_Str = 0x00749570,
            PutInInv_Int = 0x007494B5,
            PutInInv_Item = 0x00749350,
            RemoveFromInv_Int = 0x007495F0,
            RemoveFromInv_Item = 0x007495A0,
            IsInInv = 0x007491E0,
            IsInInv_Str = 0x00749200,
            Enable = 0x00745D40,
            SetTalentSkill = 0x00730F60,
            GetTalentSkill = 0x007317F0,
            SetTalentValue = 0x00730BE0,
            GetTalentValue = 0x00730DC0,
            DoShootArrow = 0x007446B0,
            CompleteHeal = 0x00736720,
            GetModel = 0x00738720,
            OpenDeadNPC = 0x00762970,
            SetBodyState = 0x0075E920,
            GetBodyState = 0x0075EAE0,
            GetEquippedArmor = 0x00737B50,
            GetEquippedMeleeWeapon = 0x00737930,
            GetEquippedRangedWeapon = 0x00737A40,
            GetSlotItem = 0x00731F90,
            EquipArmor = 0x0073A490,
            EquipWeapon = 0x0073A030,
            EquipFarWeapon = 0x0073A310,
            UnequipItem = 0x007326C0,
            CheckUnconscious = 0x00736230,
            DropUnconscious = 0x00735EB0,
            GetWeaponMode = 0x00738C40,
            SetWeaponMode = 0x00739940,
            SetWeaponMode2_Str = 0x00738C60,
            SetWeaponMode2_Int = 0x00738E80,
            DoDropVob = 0x00744DD0,
            DoTakeVob = 0x007449C0,
            DoSpellBook = 0x0073E980,
            EV_CastSpell = 0x0067FB20,
            ApplyOverlay = 0x0072D2C0,
            ApplyTimedOverlayMds = 0x00756890,
            RemoveOverlay = 0x0072D5C0,
            SetAsPlayer = 0x007426A0,
            ClearPerception = 0x0075E200,
            SetNpcAIDisabled = 0x0075F310,
            ClearPerceptionLists = 0x0075D640,
            DisablePerception = 0x0075E360,
            GetAnictrl = 0x00734B40,
            GetPermAttitude = 0x0072FB30,
            IsHuman = 0x00742640,
            IsGoblin = 0x00742650,
            IsMonster = 0x00742600,
            IsOrc = 0x00742670,
            IsSkeleton = 0x00742680,
            ProcessNPC = 0x0073E480,
            StartDialogAni = 0x00757DE0,
            GetInteractMob = 0x0074ACA0,
            ResetPos = 0x006824D0,
            PutInSlot = 0x00749CB0,
            RemoveFromSlot = 0x0074A270,
            Equip = 0x00739C90,
            GetSpellBook = 0x0073EA00,
            GetActiveSpellNr = 0x0073CF60,
            GetActiveSpellLevel = 0x0073CFE0,
            CloseDeadNpc = 0x00762B40,
            OpenSteal = 0x00762430,
            CloseSteal = 0x00762950,
            CanSee = 0x00741C10,
            SetHead = 0x007380F0,
            InitModel = 0x00738480,
            InitHumanAI = 0x0072F5B0,
            GetOverlay = 0x00730007,
            CreatePassivePerception = 0x0075B270,
            OnDamage_DD = 0x006660E0,
            CallScript = 0x007561D0,
            AssessPlayer_S = 0x0075A740,
            IsAIState = 0x0073F000,
            PerceptionCheck = 0x0075DD30,
            CanSense = 0x00740740,
            EV_OutputSVM_Overlay = 0x00756A60,
            EV_Output = 0x007576F0,
            EV_OutputSVM = 0x007571F0,
            OnMessage = 0x0074B020,
            SetFatness = 0x0072D8A0,
            SetModelScale = 0x0072D7B0,
            AssessTalk_S = 0x0075C890,
            CanBeTalkedTo = 0x006BD060,
            Turn = 0x00683000,
            UseItem = 0x0073BC10,
            EV_UseItem = 0x00755620,
            EV_UseItemToState = 0x007558F0,
            SetInteractItem = 0x0074ACC0,
            SetTrueGuild = 0x00730780,
            GetTrueGuild = 0x00730770,
            SetAdditionalVisuals = 0x00738350,
            IsUnconscious = 0x00736750,
            IsDead = 0x00736740,
            EV_AttackFinish = 0x00751AF0,
            DoDie = 0x00736760,
            GetFullBodyState = 0x0075EAF0,
            IsBodyStateInterruptable = 0x0075EFA0,
            SetToFistMode = 0x0073A940,

            ReadySpell = 0x006802E0,
            UnreadySpell = 0x00680480,
            EV_RemoveWeapon = 0x0074DB20,
            Shrink = 0x0072CA20,
            UnShrink = 0x0072CBA0,
            CloseSpellBook = 0x0073E9E0,
            AvoidShrink = 0x0072D250,

            EV_Strafe = 0x683DE0;
        }

        /*public enum HookSize : uint
        {
            OpenInventory = 6,
            CloseInventory = 9,
            PutInInv_Str = 5,
            PutInInv_Int = 7,
            PutInInv_Item = 6,
            RemoveFromInv_Int = 5,
            RemoveFromInv_Item = 6,
            IsInInv = 6,
            IsInInv_Str = 6,
            Enable = 7,
            SetTalentSkill = 6,
            GetTalentSkill = 6,
            SetTalentValue = 6,
            GetTalentValue = 6,
            DoShootArrow = 6,
            CompleteHeal = 6,
            GetModel = 5,
            OpenDeadNPC = 6,
            SetBodyState = 6,
            GetBodyState = 6,
            GetEquippedArmor = 0xA,
            GetEquippedMeleeWeapon = 9,
            GetEquippedRangedWeapon = 9,
            GetSlotItem = 5,
            Equip = 5,
            EquipArmor = 7,
            EquipWeapon = 5,
            EquipFarWeapon = 5,
            UnequipItem = 6,
            CheckUnconscious = 7,
            DropUnconscious = 7,
            GetWeaponMode = 6,
            SetWeaponMode = 5,
            SetWeaponMode2_Str = 7,
            SetWeaponMode2_Int = 6,
            DoDropVob = 6,
            DoTakeVob = 6,
            EV_CastSpell = 7,
            ApplyOverlay = 6,
            ApplyTimedOverlayMds = 7,
            RemoveOverlay = 6,
            SetAsPlayer = 6,
            ProcessNPC = 6,
            PutInSlot = 9,
            CreatePassivePerception = 7,
            OnDamage_DD = 7,
            CallScript = 5,
            AssessPlayer_S = 6,
            PerceptionCheck = 6,
            EV_OutputSVM_Overlay = 7,
            EV_Output = 7,
            EV_OutputSVM = 7,
            AssessTalk_S = 6,
            CanBeTalkedTo = 6,
            UseItem = 7,
            EV_UseItem = 6,
            EV_UseItemToState = 6,
            SetInteractItem = 6,
            IsUnconscious = 8,
            EV_AttackFinish = 7,
            DoDie = 7,

            EV_Strafe = 7
        }*/

        public oCNpc(int address) : base(address)
        {

        }

        public oCNpc()
        {

        }

        new public static oCNpc Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x0075FA00); //_CreateInstance()
            Process.THISCALL<NullReturnCall>(address, 0x0072D950); //Konstruktor...
            return new oCNpc(address);
        }

        public const int player = 0xAB2684;
        public static oCNpc GetPlayer()
        {
            return new oCNpc(Process.ReadInt(player));
        }

        public override void SetVisual(zCVisual visual)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetVisual, visual);
        }

        public void SetAdditionalVisuals(zString bodyMesh, int bodyTex, int skinColor, zString headMesh, int headTex, int TeethTex, int Armor)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetAdditionalVisuals, bodyMesh, new IntArg(bodyTex), new IntArg(skinColor), headMesh, new IntArg(headTex), new IntArg(TeethTex), new IntArg(Armor));
        }

        public void SetAdditionalVisuals(String bodyMesh, int bodyTex, int skinColor, String headMesh, int headTex, int TeethTex, int Armor)
        {
            using (zString zBodyMesh = zString.Create(bodyMesh))
            using (zString zHeadMesh = zString.Create(headMesh))
                Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetAdditionalVisuals, zBodyMesh, new IntArg(bodyTex), new IntArg(skinColor), zHeadMesh, new IntArg(headTex), new IntArg(TeethTex), new IntArg(Armor));
        }

        public int HP
        {
            get { return Process.ReadInt(Address + VarOffsets.hp_current); }
            set { Process.Write(value, Address + VarOffsets.hp_current); }
        }

        public int HPMax
        {
            get { return Process.ReadInt(Address + VarOffsets.hp_max); }
            set { Process.Write(value, Address + VarOffsets.hp_max); }
        }

        public int Instance
        {
            get { return Process.ReadInt(Address + VarOffsets.instance); }
            set { Process.Write(value, Address + VarOffsets.instance); }
        }

        public zString Name
        {
            get { return new zString(Address + VarOffsets.name); }
        }


        public void SetModelScale(zVec3 val)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetModelScale, val);
        }
        public void SetFatness(float val)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetFatness, new FloatArg(val));
        }

        public void SetToFistMode()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetToFistMode);
        }

        public void InitHumanAI()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.InitHumanAI);
        }

        public void Enable(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Enable, vec);
        }

        public void Enable(float x, float y, float z)
        {
            using (zVec3 vec = zVec3.Create(x, y, z))
                Enable(vec);
        }

        public void SetAsPlayer()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetAsPlayer);
        }

        public void Disable()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Disable);
        }

        public oCAIHuman HumanAI
        {
            get { return new oCAIHuman(Process.ReadInt(Address + VarOffsets.HumanAI)); }
            set { Process.Write(value.Address, Address + VarOffsets.HumanAI); }
        }

        public zCVob FocusVob
        {
            get { return new zCVob(Process.ReadInt(Address + VarOffsets.FocusVob)); }
            set { Process.Write(value.Address, Address + VarOffsets.FocusVob); }
        }

        public oCNpc GetFocusNpc()
        {
            return Process.THISCALL<oCNpc>(Address, 0x732BF0);
        }

        public oCAniCtrl_Human AniCtrl
        {
            get { return new oCAniCtrl_Human(Process.ReadInt(Address + VarOffsets.AniCtrl)); }
            set { Process.Write(value.Address, Address + VarOffsets.AniCtrl); }
        }

        public zCModel GetModel()
        {
            return Process.THISCALL<zCModel>(Address, FuncAddresses.GetModel);
        }

        public int GetBodyState()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetBodyState).Value;
        }

        public void SetBodyState(int x)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetBodyState, new IntArg(x));
        }

        public void SetEnemy(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x734BC0, npc);
        }

        public void ApplyOverlay(string str)
        {
            using (zString z = zString.Create(str))
                ApplyOverlay(z);
        }

        public void ApplyOverlay(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ApplyOverlay, str);
        }

        public void ApplyTimedOverlayMds(zString str, float val)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ApplyTimedOverlayMds, str, new FloatArg(val));
        }

        public void RemoveOverlay(string str)
        {
            using (zString z = zString.Create(str))
                RemoveOverlay(z);
        }

        public void RemoveOverlay(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RemoveOverlay, str);
        }


        public void EquipArmor(oCItem item)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.EquipArmor, item);
        }

        public void EquipWeapon(oCItem item)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.EquipWeapon, item);
        }
        public void EquipFarWeapon(oCItem item)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.EquipFarWeapon, item);
        }

        public void UnequipItem(oCItem item)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.UnequipItem, item);
        }
        
        public void SetToFightMode(oCItem item, int mode)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x73A740, item, (IntArg)mode);
        }

        public void SetWeaponMode(int str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetWeaponMode, new CallValue[] { new IntArg(str) });
        }

        public void SetWeaponMode2(int str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetWeaponMode2_Int, new CallValue[] { new IntArg(str) });
        }

        public void SetWeaponMode2(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetWeaponMode2_Str, new CallValue[] { str });
        }

        public int GetWeaponMode()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetWeaponMode).Value;
        }

        /*
        public static oCNpc StealNPC(Process process)
        {
            return new oCNpc(process, process.ReadInt(0x00AB27D4));
        }

        public static oCStealContainer StealContainer(Process process)
        {
            return new oCStealContainer(process, process.ReadInt(0x00AB27DC));
        }

        public static zString getSlotString(Process process, int id)
        {
            if (id >= 9)
                throw new ArgumentException("ID can't be greater than 9!");

            if (id == 0)
                return SLOT_RIGHTHAND(process);
            if (id == 1)
                return SLOT_LEFTHAND(process);
            if (id == 2)
                return SLOT_SWORD(process);
            if (id == 3)
                return SLOT_LONGSWORD(process);
            if (id == 4)
                return SLOT_BOW(process);
            if (id == 5)
                return SLOT_CROSSBOW(process);
            if (id == 6)
                return SLOT_TORSO(process);
            if (id == 7)
                return SLOT_HELMET(process);
            if (id == 8)
                return SLOT_SHIELD(process);

            return null;
        }

        public static zString SLOT_RIGHTHAND(Process process)
        {
            return new zString(process, 0x00AB2148);
        }

        public static zString SLOT_LEFTHAND(Process process)
        {
            return new zString(process, 0x00AB2424);
        }

        public static zString SLOT_CROSSBOW(Process process)
        {
            return new zString(process, 0x00AB2114);
        }

        public static zString SLOT_TORSO(Process process)
        {
            return new zString(process, 0x00AB1FA0);
        }

        public static zString SLOT_LONGSWORD(Process process)
        {
            return new zString(process, 0x00AB1F60);
        }

        public static zString SLOT_BOW(Process process)
        {
            return new zString(process, 0x00AB1F48);
        }

        public static zString SLOT_SHIELD(Process process)
        {
            return new zString(process, 0x00AB1E0C);
        }

        public static zString SLOT_SWORD(Process process)
        {
            return new zString(process, 0x00AB1EF0);
        }

        public static zString SLOT_HELMET(Process process)
        {
            return new zString(process, 0x00AB1F04);
        }

        #endregion

        #region Fields

        public zCArray<IntArg> listOfVoiceHandles
        {
            get { return new zCArray<IntArg>(Process, this.Address + (int)Offsets.VoiceHandleList); }
        }


        public int InteractItemState
        {
            get { return Process.ReadInt(Address + (int)Offsets.InteractItemState); }
            set { Process.Write(value, Address + (int)Offsets.InteractItemState); }
        }

        public int InteractItemTargetState
        {
            get { return Process.ReadInt(Address + (int)Offsets.InteractItemTargetState); }
            set { Process.Write(value, Address + (int)Offsets.InteractItemTargetState); }
        }

        public oCItem InteractItem
        {
            get { return new oCItem(Process, Process.ReadInt(Address + (int)Offsets.InteractItem)); }
        }

        public void SetInteractItem(oCItem item)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetInteractItem, new CallValue[] { item });
        }

        public int Flags
        {
            get { return Process.ReadInt(Address + (int)Offsets.flags); }
            set { Process.Write(value, Address + (int)Offsets.flags); }
        }

        public int Guild
        {
            get { return Process.ReadInt(Address + (int)Offsets.guild); }
            set { Process.Write(value, Address + (int)Offsets.guild); }
        }

        public int WeaponMode
        {
            get { return Process.ReadInt(Address + (int)Offsets.weaponMode); }
            set { Process.Write(value, Address + (int)Offsets.weaponMode); }
        }

        public int FallDownDamage
        {
            get { return Process.ReadInt(Address + (int)Offsets.falldowndamage); }
            set { Process.Write(value, Address + (int)Offsets.falldowndamage); }
        }

        /// <summary>
        /// Wirkt ohne weiteres nicht. Also eher zum Abfragen, eventuell ReadOnly setzen
        /// </summary>
        public float Fatness
        {
            get { return Process.ReadFloat(Address + (int)Offsets.fatness); }
            set { Process.Write(value, Address + (int)Offsets.fatness); }
        }

        public zVec3 Scale
        {
            get { return new zVec3(Process, Address + (int)Offsets.model_scale); }
        }

        public int Voice
        {
            get { return Process.ReadInt(Address + (int)Offsets.voice); }
            set { Process.Write(value, Address + (int)Offsets.voice); }
        }

        public int VoiceIndex
        {
            get { return Process.ReadInt(Address + (int)Offsets.voice_index); }
            set { Process.Write(value, Address + (int)Offsets.voice_index); }
        }

        public int BitfieldNPC0
        {
            get { return Process.ReadInt(Address + (int)Offsets.BitFieldNPC); }
            set { Process.Write(value, Address + (int)Offsets.BitFieldNPC); }
        }

        public int BitfieldNPC1
        {
            get { return Process.ReadInt(Address + (int)Offsets.BitFieldNPC + 4); }
            set { Process.Write(value, Address + (int)Offsets.BitFieldNPC + 4); }
        }

        public int BitfieldNPC2
        {
            get { return Process.ReadInt(Address + (int)Offsets.BitFieldNPC + 8); }
            set { Process.Write(value, Address + (int)Offsets.BitFieldNPC + 8); }
        }

        public int BitfieldNPC3
        {
            get { return Process.ReadInt(Address + (int)Offsets.BitFieldNPC + 12); }
            set { Process.Write(value, Address + (int)Offsets.BitFieldNPC + 12); }
        }

        public int BitfieldNPC4
        {
            get { return Process.ReadInt(Address + (int)Offsets.BitFieldNPC + 16); }
            set { Process.Write(value, Address + (int)Offsets.BitFieldNPC + 16); }
        }

        public int BodyState
        {
            get
            {
                int v = ((1 << 19) - 1);
                return (ushort)((BitfieldNPC4 & v));
            }
            set
            {
                int v = ((1 << 19) - 1);
                BitfieldNPC4 &= ~v;
                BitfieldNPC4 |= (int)value;
            }
        }

        public ushort BodyTex
        {
            //get { return Process.ReadUShort(Address + (int)Offsets.BitFieldNPC + 2); }
            //set { Process.Write(value, Address + (int)Offsets.BitFieldNPC + 2); }
            get
            {
                int oCNpc_bitfield0_body_TexVarNr = ((1 << 16) - 1) << 14;
                return (ushort)((BitfieldNPC0 & oCNpc_bitfield0_body_TexVarNr) >> 14);
            }
            set
            {
                int oCNpc_bitfield0_body_TexVarNr = ((1 << 16) - 1) << 14;
                BitfieldNPC0 &= ~oCNpc_bitfield0_body_TexVarNr;
                BitfieldNPC0 |= (int)value << 14;
            }
        }


        public ushort HeadTex
        {
            get { return Process.ReadUShort(Address + (int)Offsets.BitFieldNPC + 4 + 2); }
            set { Process.Write(value, Address + (int)Offsets.BitFieldNPC + 4 + 2); }
        }


        public int Hitchance_h1
        {
            get { return Process.ReadInt(Address + (int)Offsets.hc_h1); }
            set { Process.Write(value, Address + (int)Offsets.hc_h1); }
        }

        public int Hitchance_h2
        {
            get { return Process.ReadInt(Address + (int)Offsets.hc_h2); }
            set { Process.Write(value, Address + (int)Offsets.hc_h2); }
        }

        public int Hitchance_Bow
        {
            get { return Process.ReadInt(Address + (int)Offsets.hc_bow); }
            set { Process.Write(value, Address + (int)Offsets.hc_bow); }
        }

        public int Hitchance_Crossbow
        {
            get { return Process.ReadInt(Address + (int)Offsets.hc_crossbow); }
            set { Process.Write(value, Address + (int)Offsets.hc_crossbow); }
        }



        public int Senses
        {
            get { return Process.ReadInt(Address + (int)Offsets.Senses); }
            set { Process.Write(value, Address + (int)Offsets.Senses); }
        }

        public int Senses_Range
        {
            get { return Process.ReadInt(Address + (int)Offsets.Senses_Range); }
            set { Process.Write(value, Address + (int)Offsets.Senses_Range); }
        }



        public int MP
        {
            get { return Process.ReadInt(Address + (int)Offsets.mp_current); }
            set { Process.Write(value, Address + (int)Offsets.mp_current); }
        }

        public int MPMax
        {
            get { return Process.ReadInt(Address + (int)Offsets.mp_max); }
            set { Process.Write(value, Address + (int)Offsets.mp_max); }
        }

        public int Strength
        {
            get { return Process.ReadInt(Address + (int)Offsets.strength); }
            set { Process.Write(value, Address + (int)Offsets.strength); }
        }

        public int Dexterity
        {
            get { return Process.ReadInt(Address + (int)Offsets.dexterity); }
            set { Process.Write(value, Address + (int)Offsets.dexterity); }
        }

        public float DiveTime
        {
            get { return Process.ReadFloat(Address + (int)Offsets.DiveTime); }
            set { Process.Write(value, Address + (int)Offsets.DiveTime); }
        }
        public float DiveCTR
        {
            get { return Process.ReadFloat(Address + (int)Offsets.DiveCTR); }
            set { Process.Write(value, Address + (int)Offsets.DiveCTR); }
        }

        public oCItemContainer TraderContainer
        {
            get { return new oCItemContainer(Process, Process.ReadInt(Address + (int)Offsets.traderContainer)); }
        }

        public oCNpc TraderNPC
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.traderNPC)); }
        }

        public oCNpc Enemy
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.enemy)); }
            set { Process.Write(value.Address, Address + (int)Offsets.enemy); }
        }

        public oCNpc_States NpcStates
        {
            get { return new oCNpc_States(Process, Address + (int)Offsets.NpcStates); }
        }

        public zCArray<oCNpcTalent> Talents
        {
            get
            {
                return new zCArray<oCNpcTalent>(Process, Address + (int)Offsets.talents_array);
            }
        }

        public zCArray<zString> ActiveOverlays
        {
            get { return new zCArray<zString>(Process, Address + (int)Offsets.activeOverlays); }
        }

        public zCArray<oCNpc> ActiveSpells
        {
            get { return new zCArray<oCNpc>(Process, Address + (int)Offsets.ActiveSpells); }
        }

        public oCMag_Book MagBook
        {
            get { return new oCMag_Book(Process, Process.ReadInt(Address + (int)Offsets.MagBook)); }
            set { Process.Write(value.Address, Address + (int)Offsets.MagBook); }
        }



        public zString HeadVisualString
        {
            get { return new zString(Process, Address + (int)Offsets.HeadVisualString); }
            set { HeadVisualString.Set(value); }
        }

        public zString BodyVisualString
        {
            get { return new zString(Process, Address + (int)Offsets.BodyVisualString); }
            set { BodyVisualString.Set(value); }
        }

        public zString MDSName
        {
            get { return new zString(Process, Address + (int)Offsets.MDS_Name); }
            set { MDSName.Set(value); }
        }

        public oCNpcInventory Inventory
        {
            get { return new oCNpcInventory(Process, Address + (int)Offsets.Inventory); }
        }


        #endregion

        #region ownMethods
        public void ClearOverlays()
        {
            int overlayCount = this.ActiveOverlays.Size;
            String[] overlayList = new String[overlayCount];
            for (int i = 0; i < overlayCount; i++)
            {
                overlayList[i] = this.ActiveOverlays.get(i, 20).Value.ToUpper().Trim();
            }

            for (int i = 0; i < overlayCount; i++)
            {
                using (zString str = zString.Create(Process, overlayList[i]))
                {
                    this.RemoveOverlay(str);
                }
            }

        }

        public void setAttributes(byte type, int value)
        {
            Process.Write(value, Address + (int)Offsets.hp_current + type * 4);
        }

        public void SetHitChances(int talent, int percent)
        {
            if (talent == (int)NPC_Talents.H1)
                Hitchance_h1 = percent;
            else if (talent == (int)NPC_Talents.H2)
                Hitchance_h2 = percent;
            else if (talent == (int)NPC_Talents.Bow)
                Hitchance_Bow = percent;
            else if (talent == (int)NPC_Talents.CrossBow)
                Hitchance_Crossbow = percent;
        }

        public int GetHitChances(int talent)
        {
            if (talent == (int)NPC_Talents.H1)
                return Hitchance_h1;
            else if (talent == (int)NPC_Talents.H2)
                return Hitchance_h2;
            else if (talent == (int)NPC_Talents.Bow)
                return Hitchance_Bow;
            else if (talent == (int)NPC_Talents.CrossBow)
                return Hitchance_Crossbow;
            return 0;
        }
        public void SetFightSkill(int talent, int percent)
        {
            SetHitChances(talent, percent);
            if (percent >= 0)
                SetTalentSkill(talent, 0);
            if (percent >= 30)
                SetTalentSkill(talent, 1);
            if (percent >= 60)
                SetTalentSkill(talent, 2);
        }




        public void SetLookAt(zVec3 lookAt)
        {
            zVec3 pos = GetPosition();
            float[] dir = new float[] { lookAt.X - pos.X, lookAt.Y - pos.Y, lookAt.Z - pos.Z };
            pos.Dispose();


            float revScalar = (float)Math.Sqrt(dir[0] * dir[0] + dir[1] * dir[1] + dir[2] * dir[2]);
            dir[0] /= revScalar; dir[1] /= revScalar; dir[2] /= revScalar;



            float[] upVector = new float[] { TrafoObjToWorld.get(1), TrafoObjToWorld.get(5), TrafoObjToWorld.get(9) };
            float[] rightVector = new float[3];
            rightVector[0] = dir[1] * upVector[2] - dir[2] * upVector[1];
            rightVector[1] = dir[2] * upVector[0] - dir[0] * upVector[2];
            rightVector[2] = dir[0] * upVector[1] - dir[1] * upVector[0];

            TrafoObjToWorld.set(0, -1 * rightVector[0]);
            TrafoObjToWorld.set(4, -1 * rightVector[1]);
            TrafoObjToWorld.set(8, -1 * rightVector[2]);

            TrafoObjToWorld.set(2, dir[0]); TrafoObjToWorld.set(6, dir[1]); TrafoObjToWorld.set(10, dir[2]);
        }

        public void SetPosition(zVec3 position)
        {
            TrafoObjToWorld.set(3, position.X);
            TrafoObjToWorld.set(7, position.Y);
            TrafoObjToWorld.set(11, position.Z);
        }

        public void SetPosition(float x, float y, float z)
        {
            TrafoObjToWorld.set(3, x);
            TrafoObjToWorld.set(7, y);
            TrafoObjToWorld.set(11, z);
        }


        public void SetName(zString str)
        {
            Name.Clear();
            Name.Insert(0, str);
        }

        public int[] PercList
        {
            get
            {
                int[] rVal = new int[66];
                for (int i = 0; i < rVal.Length; i++)
                {
                    rVal[i] = Process.ReadInt(Address + (int)Offsets.PercList + i * 4);
                }
                return rVal;
            }
            set
            {
                if (value.Length != 66)
                    return;
                for (int i = 0; i < value.Length; i++)
                {
                    Process.Write(value[i], Address + (int)Offsets.PercList + i * 4);
                }
            }
        }

        public static void SetNpcAIDisabled(Process Process, int x)
        {
            Process.CDECLCALL<NullReturnCall>(FuncAddresses.SetNpcAIDisabled, new CallValue[] { new IntArg(x) });
        }
        #endregion

        #region methods


        public void Shrink()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Shrink);
        }

        public void UnShrink()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.UnShrink);
        }

        public void AvoidShrink(int val)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.AvoidShrink, new CallValue[] { new IntArg(val) });
        }

        public void CloseSpellBook(bool x)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CloseSpellBook, new CallValue[] { new BoolArg(x) });
        }



        public int ReadySpell(int val, int val2)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.ReadySpell, new CallValue[] { new IntArg(val), new IntArg(val2) });
        }

        public int UnreadySpell()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.UnreadySpell);
        }

        public void SetTrueGuild(int val)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetTrueGuild, new CallValue[] { new IntArg(val) });
        }

        public void Turn(zVec3 pos)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Turn, new CallValue[] { pos });
        }


        public void OnDamage(oSDamageDescriptor oDD)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.OnDamage_DD, new CallValue[] { oDD });
        }

        /// <summary>
        /// Hat bisher immer ein Bug gebracht... Eventuell weil es in einem anderen Thread aufgerufen wurde, bzw eine Ai-Funktion ist die sich nach dem ausführen selbst aus der Ai-Queue nimmt?
        /// </summary>
        /// <param name="msg"></param>
        public void EV_OutputSVM_Overlay(oCMsgConversation msg)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.EV_OutputSVM_Overlay, new CallValue[] { msg });
        }

        public int AssessTalk_S(oCNpc npc)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.AssessTalk_S, new CallValue[] { npc });
        }

        public int AssessPlayer_S(oCNpc npc)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.AssessPlayer_S, new CallValue[] { npc });
        }

        public int CanBeTalkedTo()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.CanBeTalkedTo);
        }

        public int GetTrueGuild()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetTrueGuild);
        }

        public int IsBodyStateInterruptable()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsBodyStateInterruptable);
        }

        public int CanSense(zCVob npc)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.CanSense, new CallValue[] { npc });
        }

        public int IsUnconscious()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsUnconscious);
        }

        public int IsDead()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsDead);
        }

        public int GetFullBodyState()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetFullBodyState);
        }

        public int CanSee(zCVob vob, int arg)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.CanSee, new CallValue[] { vob, new IntArg(arg) }).Address;
        }

        public int EV_RemoveWeapon(oCMsgWeapon msg)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.EV_RemoveWeapon, new CallValue[] { msg }).Address;
        }

        public void CreatePassivePerception(int arg, zCVob vob, zCVob vob2)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CreatePassivePerception, new CallValue[] { new IntArg(arg), vob, vob2 });
        }

        public void OnMessage(int eventMessage, zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.OnMessage, new CallValue[] { (IntArg)eventMessage, vob });
        }

        public void PerceptionCheck()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.PerceptionCheck);
        }

        public void SetHead()
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.SetHead);
        }

        public void InitModel()
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.InitModel);
        }


        public void CloseSteal()
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.CloseSteal);
        }

        public void OpenSteal()
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.OpenSteal);
        }

        public void CloseDeadNpc()
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.CloseDeadNpc);
        }

        public int GetActiveSpellNr()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetActiveSpellNr).Address;
        }

        public int GetActiveSpellLevel()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetActiveSpellLevel).Address;
        }

        public void Equip(oCItem slot)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Equip, new CallValue[] { slot });
        }

        public void EquipItem(oCItem item)
        {
            Process.THISCALL<NullReturnCall>(Address, (uint)0x7323C0, new CallValue[] { item });
        }

        public void RemoveFromSlot(zString slot, int vob, int i)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RemoveFromSlot, new CallValue[] { slot, new IntArg(vob), new IntArg(i) });
        }

        public int GetInvSlot(zString slot)
        {
            return Process.THISCALL<IntArg>(Address, (uint)0x749AE0, new CallValue[] { slot });
        }

        public void PutInSlot(zString slot, zCVob vob, int i)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.PutInSlot, new CallValue[] { slot, vob, new IntArg(i) });
        }

        public void PutInSlot(int slot, zCVob vob, int i)
        {
            Process.THISCALL<NullReturnCall>(Address, (uint)0x749D80, new CallValue[] { new IntArg(slot), vob, new IntArg(i) });
        }

        public void ResetPos(zVec3 pos)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ResetPos, new CallValue[] { pos });
        }

        public void StartDialogAni()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StartDialogAni);
        }


        //32 percs?
        public void DisablePerception(int perc)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DisablePerception, new CallValue[] { new IntArg(perc) });
        }

        public int GetPermAttitude(oCNpc npc)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetPermAttitude, new CallValue[] { npc }).Address;
        }

        public int IsAIState(int state)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsAIState, new CallValue[] { (IntArg)state });
        }


        public void ClearPerception()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ClearPerception);
        }

        public void ClearPerceptionLists()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ClearPerceptionLists);
        }

        public int IsGoblin()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsGoblin).Address;
        }

        public int IsHuman()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsHuman).Address;
        }

        public int IsOrc()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsOrc).Address;
        }

        public int IsMonster()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsMonster).Address;
        }

        public int IsSkeleton()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsSkeleton).Address;
        }

        public oCMobInter GetInteractMob()
        {
            return Process.THISCALL<oCMobInter>(Address, FuncAddresses.GetInteractMob);
        }

        public oCAniCtrl_Human GetAnictrl()
        {
            return Process.THISCALL<oCAniCtrl_Human>(Address, FuncAddresses.GetAnictrl);
        }

        public oCMag_Book GetSpellBook()
        {
            return Process.THISCALL<oCMag_Book>(Address, FuncAddresses.GetSpellBook);
        }

        public void OpenInventory(int inv)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.OpenInventory, new CallValue[] { new IntArg(inv) });
        }

        public void CloseInventory()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CloseInventory);
        }

        public void OpenDeadNPC()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.OpenDeadNPC);
        }


        public oCItem PutInInv(oCItem code)
        {

            return Process.THISCALL<oCItem>(Address, FuncAddresses.PutInInv_Item, new CallValue[] { code });
        }

        public oCItem PutInInv(zString code, int count)
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.PutInInv_Str, new CallValue[] { code, new IntArg(count) });
        }

        public oCItem PutInInv(int item, int count)
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.PutInInv_Int, new CallValue[] { new IntArg(item), new IntArg(count) });
        }

        public oCItem IsInInv(int item, int count)
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.IsInInv, new CallValue[] { new IntArg(item), new IntArg(count) });
        }

        public oCItem IsInInv(zString str, int count)
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.IsInInv_Str, new CallValue[] { str, new IntArg(count) });
        }

        public oCItem GetSlotItem(zString code)
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.GetSlotItem, new CallValue[] { code });
        }

        public oCItem RemoveFromInv(int item, int count)
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.RemoveFromInv_Int, new CallValue[] { new IntArg(item), new IntArg(count) });
        }

        public oCItem RemoveFromInv(oCItem item, int count)
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.RemoveFromInv_Item, new CallValue[] { item, new IntArg(count) });
        }

        public void SetTalentValue(int x, int y)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetTalentValue, new CallValue[] { new IntArg(x), new IntArg(y) });
        }

        public void SetTalentSkill(int x, int y)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetTalentSkill, new CallValue[] { new IntArg(x), new IntArg(y) });
        }

        public int GetTalentValue(int x)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetTalentValue, new CallValue[] { new IntArg(x) }).Address;
        }

        public int GetTalentSkill(int x)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetTalentSkill, new CallValue[] { new IntArg(x) }).Address;
        }

        public void DoShootArrow(int arrowid)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DoShootArrow, new CallValue[] { new IntArg(arrowid) });
        }

        public void DoSpellBook()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DoSpellBook);
        }

        public void CompleteHeal()
        {
            IntArg x = 123;
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CompleteHeal);
        }

        public int GetOverlay(zString str)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetOverlay, new CallValue[] { str }).Address;
        }

        public oCItem GetEquippedArmor()
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.GetEquippedArmor);
        }

        public oCItem GetEquippedMeleeWeapon()
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.GetEquippedMeleeWeapon);
        }

        public oCItem GetEquippedRangedWeapon()
        {
            return Process.THISCALL<oCItem>(Address, FuncAddresses.GetEquippedRangedWeapon);
        }


        public void DropUnconscious(float arg, oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DropUnconscious, new CallValue[] { new FloatArg(arg), npc });
        }

        public void CheckUnconscious()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CheckUnconscious);
        }


        public void DoDropVob(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DoDropVob, new CallValue[] { vob });
        }

        public void DoTakeVob(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DoTakeVob, new CallValue[] { vob });
        }

        public void SetLeftHand(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, (uint)0x73B0C0, new CallValue[] { vob });
        }
        #endregion



        public static void Freeze(Process process, bool r)
        {
            if (r)
                process.Write(1, 0x00AB2664);
            else
                process.Write(0, 0x00AB2664);
        }


        public int GetNextWeaponmode(int arg1, int arg2, int arg3)
        {
            return Process.THISCALL<IntArg>(Address, (uint)0x739A30, new CallValue[] { (IntArg)arg1, (IntArg)arg2, (IntArg)arg3 });
        }

        public bool IsInFightRange(zCVob vob, float range)
        {
            IntPtr ptr = Process.Alloc(4);
            Process.Write(range, ptr.ToInt32());
            int result = Process.THISCALL<IntArg>(Address, (uint)0x67CB60, new CallValue[] { vob, (IntArg)ptr.ToInt32() });
            Process.Free(ptr, 4);
            return result > 0;
        }

        public bool IsInFightFocus(zCVob vob)
        {
            return Process.THISCALL<IntArg>(Address, (uint)0x735290, new CallValue[] { vob }) > 0;
        }

        public bool IsSameHeight(zCVob vob)
        {
            return Process.THISCALL<IntArg>(Address, (uint)0x737BE0, new CallValue[] { vob }) > 0;
        }

        public void DoStrafe(bool right)
        {
            oCMsgMovement msg = oCMsgMovement.Create(Process, oCMsgMovement.SubTypes.Strafe, new zCVob());
            msg.Animation = right ? AniCtrl._t_strafer : AniCtrl._t_strafel;
            GetEM(0).OnMessage(msg, this);
        }

        public int StartFaceAni(string ani, float arg1, float arg2)
        {
            int result;
            using (zString z = zString.Create(Process, ani))
            {
                result = StartFaceAni(z, arg1, arg2);
            }
            return result;
        }

        public int StartFaceAni(zString ani, float arg1, float arg2)
        {
            return Process.THISCALL<IntArg>(Address, (uint)0x738860, new CallValue[] { ani, (FloatArg)arg1, (FloatArg)arg2 });
        }

        public int StopFaceAni(string ani)
        {
            int result;
            using (zString z = zString.Create(Process, ani))
            {
                result = StopFaceAni(z);
            }
            return result;
        }

        public int StopFaceAni(zString ani)
        {
            return Process.THISCALL<IntArg>(Address, (uint)0x738B50, new CallValue[] { ani });
        }*/
    }
}
