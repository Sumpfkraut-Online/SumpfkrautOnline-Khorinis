using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;
using Gothic.zStruct;

namespace Gothic.zClasses
{
    public class oCNpc : zCVob
    {
        
        #region OffsetLists
        public enum Offsets : uint
        {
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
            falldowndamage= 0x910,
            flags = 0x01B4,
            fatness = 0x07BC,
            model_scale = 0x07B0,
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
        public enum FuncOffsets : uint
        {
            OpenInventory = 0x00762250,
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
            CloseDeadNpc = 0x00762B40,
            OpenSteal = 0x00762430,
            CloseSteal = 0x00762950,
            CanSee = 0x00741C10,
            SetHead = 0x007380F0,
            InitModel = 0x00738480,
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
            SetInteractItem = 0x0074ACC0
        }

        public enum HookSize : uint
        {
            OpenInventory = 6,
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
            SetInteractItem = 6
        }
        #endregion

        #region Standard
        public oCNpc(Process process, int address) : base (process, address)
        {
            
        }

        public oCNpc()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion

        #region statics
        public static oCNpc Player(Process process)
        {
            return new oCNpc(process, process.ReadInt(0xAB2684));
        }
        
        public static oCNpc StealNPC(Process process)
        {
            return new oCNpc(process, process.ReadInt(0x00AB27D4));
        }

        public static oCStealContainer StealContainer(Process process)
        {
            return new oCStealContainer(process, process.ReadInt(0x00AB27DC));
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

        public int Flags
        {
            get { return Process.ReadInt(Address + (int)Offsets.flags); }
            set { Process.Write(value, Address + (int)Offsets.flags); }
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
        public ushort BodyTex
        {
            //get { return Process.ReadUShort(Address + (int)Offsets.BitFieldNPC + 2); }
            //set { Process.Write(value, Address + (int)Offsets.BitFieldNPC + 2); }
            get
            {
                int oCNpc_bitfield0_body_TexVarNr = ((1 << 16) - 1) << 14;
                return (ushort)((BitfieldNPC0 & oCNpc_bitfield0_body_TexVarNr)>>14);
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
            get { return Process.ReadUShort(Address + (int)Offsets.BitFieldNPC + 4 + 2);  }
            set { Process.Write(value, Address + (int)Offsets.BitFieldNPC + 4 + 2);  }
        }

        public int Instance
        {
            get { return Process.ReadInt(Address + (int)Offsets.instance); }
            set { Process.Write(value, Address + (int)Offsets.instance); }
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

        public int HP
        {
            get { return Process.ReadInt(Address + (int)Offsets.hp_current); }
            set { Process.Write(value, Address + (int)Offsets.hp_current); }
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

        public int HPMax
        {
            get { return Process.ReadInt(Address + (int)Offsets.hp_max); }
            set { Process.Write(value, Address + (int)Offsets.hp_max); }
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

        public oCAniCtrl_Human AniCtrl
        {
            get { return new oCAniCtrl_Human(Process, Process.ReadInt(Address + (int)Offsets.AniCtrl)); }
            set { Process.Write(value.Address, Address + (int)Offsets.AniCtrl); }
        }

        public oCAiHuman HumanAI
        {
            get { return new oCAiHuman(Process, Process.ReadInt(Address + (int)Offsets.HumanAI)); }
            set { Process.Write(value.Address, Address + (int)Offsets.HumanAI); }
        }

        public zCVob FocusVob
        {
            get { return new zCVob(Process, Process.ReadInt(Address + (int)Offsets.FocusVob)); }
            set {
                Process.Write(value.Address, Address + (int)Offsets.FocusVob);
            }
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
            get{   return new zCArray<zString>(Process, Address + (int)Offsets.activeOverlays); }
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

        public zString Name
        {
            get{return new zString(Process, Address + (int)Offsets.name);}
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


        public zVec3 GetPosition()
        {
            Matrix4 traf = this.TrafoObjToWorld;
            zVec3 pos = zVec3.Create(Process);
            pos.X = Process.ReadFloat(traf.Address + 3 * 4);
            pos.Y = Process.ReadFloat(traf.Address + 7 * 4);
            pos.Z = Process.ReadFloat(traf.Address + 11 * 4);
            return pos;
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
                for (int i = 0; i < rVal.Length; i++ )
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
            Process.CDECLCALL<NullReturnCall>((uint)FuncOffsets.SetNpcAIDisabled, new CallValue[] { new IntArg(x) });
        }
        #endregion

        #region methods
        public void Turn(zVec3 pos)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Turn, new CallValue[] { pos });
        }

        public void SetModelScale(zVec3 val)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetModelScale, new CallValue[] { val });
        }
        public void SetFatness(float val)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetFatness, new CallValue[] { new FloatArg(val) });
        }

        public void OnDamage(oSDamageDescriptor oDD)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OnDamage_DD, new CallValue[] { oDD });
        }

        /// <summary>
        /// Hat bisher immer ein Bug gebracht... Eventuell weil es in einem anderen Thread aufgerufen wurde, bzw eine Ai-Funktion ist die sich nach dem ausführen selbst aus der Ai-Queue nimmt?
        /// </summary>
        /// <param name="msg"></param>
        public void EV_OutputSVM_Overlay(oCMsgConversation msg)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.EV_OutputSVM_Overlay, new CallValue[] { msg });
        }

        public int AssessTalk_S(oCNpc npc)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.AssessTalk_S, new CallValue[] { npc });
        }

        public int AssessPlayer_S(oCNpc npc)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.AssessPlayer_S, new CallValue[] { npc });
        }

        public int CanBeTalkedTo()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.CanBeTalkedTo, new CallValue[] {  });
        }

        public int CanSense(zCVob npc)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.CanSense, new CallValue[] { npc });
        }

        public int CanSee(zCVob vob, int arg)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.CanSee, new CallValue[] { vob, new IntArg(arg) }).Address;
        }

        public void CreatePassivePerception(int arg, zCVob vob, zCVob vob2)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.CreatePassivePerception, new CallValue[] { new IntArg(arg), vob, vob2 });
        }

        public void OnMessage(int eventMessage, zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OnMessage, new CallValue[] { (IntArg)eventMessage, vob });
        }

        public void PerceptionCheck()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.PerceptionCheck, new CallValue[] { });
        }

        public void SetHead()
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.SetHead, new CallValue[] { });
        }

        public void InitModel()
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.InitModel, new CallValue[] { });
        }

        public void CloseSteal()
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.CloseSteal, new CallValue[] { });
        }

        public void OpenSteal()
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.OpenSteal, new CallValue[] { });
        }

        public void CloseDeadNpc()
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.CloseDeadNpc, new CallValue[] { });
        }

        public int GetActiveSpellNr()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetActiveSpellNr, new CallValue[] { }).Address;
        }
        public void Equip(oCItem slot)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Equip, new CallValue[] { slot });
        }
       
        public void RemoveFromSlot(zString slot, int vob, int i)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RemoveFromSlot, new CallValue[] { slot, new IntArg(vob), new IntArg(i) });
        }
        
        public void PutInSlot(zString slot, zCVob vob, int i)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.PutInSlot, new CallValue[] { slot, vob, new IntArg(i) });
        }

        public void ResetPos(zVec3 pos)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ResetPos, new CallValue[] { pos });
        }

        public void StartDialogAni()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartDialogAni, new CallValue[] { });
        }
        
        public void SetAsPlayer()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetAsPlayer, new CallValue[] { });
        }
        //32 percs?
        public void DisablePerception(int perc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DisablePerception, new CallValue[] { new IntArg(perc) });
        }

        public int GetPermAttitude(oCNpc npc)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetPermAttitude, new CallValue[] { npc }).Address;
        }

        public int IsAIState(int state)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsAIState, new CallValue[] { (IntArg)state });
        }


        public void ClearPerception()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ClearPerception, new CallValue[] { });
        }

        public void ClearPerceptionLists()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ClearPerceptionLists, new CallValue[] { });
        }

        public int IsGoblin()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsGoblin, new CallValue[] {  }).Address;
        }

        public int IsHuman()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsHuman, new CallValue[] { }).Address;
        }

        public int IsOrc()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsOrc, new CallValue[] { }).Address;
        }

        public int IsMonster()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsMonster, new CallValue[] { }).Address;
        }

        public int IsSkeleton()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsSkeleton, new CallValue[] { }).Address;
        }

        public oCMobInter GetInteractMob()
        {
            return Process.THISCALL<oCMobInter>((uint)Address, (uint)FuncOffsets.GetInteractMob, new CallValue[] { });
        }
        /// <summary>
        /// GetModel liefert das Model des NPCs zurück
        /// </summary>
        /// <returns></returns>
        public zCModel GetModel()
        {
            return Process.THISCALL<zCModel>((uint)Address, (uint)FuncOffsets.GetModel, new CallValue[] { });
        }

        public oCAniCtrl_Human GetAnictrl()
        {
            return Process.THISCALL<oCAniCtrl_Human>((uint)Address, (uint)FuncOffsets.GetAnictrl, new CallValue[] { });
        }

        public oCMag_Book GetSpellBook()
        {
            return Process.THISCALL<oCMag_Book>((uint)Address, (uint)FuncOffsets.GetSpellBook, new CallValue[] { });
        }

        /// <summary>
        /// Spawnt den NPC
        /// </summary>
        /// <param name="vec"></param>
        public void Enable(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Enable, new CallValue[] { vec });
        }

        public void ApplyOverlay(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ApplyOverlay, new CallValue[] { str });
        }

        public void ApplyTimedOverlayMds(zString str, float val)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ApplyTimedOverlayMds, new CallValue[] { str, new FloatArg(val) });
        }

        public void RemoveOverlay(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RemoveOverlay, new CallValue[] { str });
        }

        public void OpenInventory(int inv)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OpenInventory, new CallValue[] { new IntArg(inv) });
        }

        public void OpenDeadNPC()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OpenDeadNPC, new CallValue[] { });
        }

        public oCItem PutInInv(oCItem code)
        {
            
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.PutInInv_Item, new CallValue[] { code });
        }

        public oCItem PutInInv(zString code, int count)
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.PutInInv_Str, new CallValue[] { code, new IntArg(count) });
        }

        public oCItem PutInInv(int item, int count)
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.PutInInv_Int, new CallValue[] { new IntArg(item), new IntArg(count) });
        }

        public oCItem IsInInv(int item, int count)
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.IsInInv, new CallValue[] { new IntArg(item), new IntArg(count) });
        }

        public oCItem IsInInv(zString str, int count)
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.IsInInv_Str, new CallValue[] { str, new IntArg(count) });
        }

        public oCItem GetSlotItem(zString code)
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.GetSlotItem, new CallValue[] { code });
        }

        public oCItem RemoveFromInv(int item, int count)
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.RemoveFromInv_Int, new CallValue[] { new IntArg(item), new IntArg(count) });
        }

        public oCItem RemoveFromInv(oCItem item, int count)
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.RemoveFromInv_Item, new CallValue[] { item, new IntArg(count) });
        }

        public void SetTalentValue(int x, int y)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetTalentValue, new CallValue[] { new IntArg(x), new IntArg(y) });
        }

        public void SetTalentSkill(int x, int y)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetTalentSkill, new CallValue[] { new IntArg(x), new IntArg(y) });
        }

        public int GetTalentValue(int x)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetTalentValue, new CallValue[] { new IntArg(x) }).Address;
        }

        public int GetTalentSkill(int x)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetTalentSkill, new CallValue[] { new IntArg(x) }).Address;
        }

        public int GetBodyState()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetBodyState, new CallValue[] { }).Address;
        }

        public void SetBodyState(int x)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetBodyState, new CallValue[] { new IntArg(x) });
        }

        public void DoShootArrow( int arrowid )
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DoShootArrow, new CallValue[] { new IntArg(arrowid) });
        }

        public void DoSpellBook()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DoSpellBook, new CallValue[] {});
        }

        public void CompleteHeal()
        {
            IntArg x = 123;
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.CompleteHeal, new CallValue[] { });
        }

        public int GetOverlay(zString str)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetOverlay, new CallValue[] { str }).Address;
        }

        public oCItem GetEquippedArmor()
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.GetEquippedArmor, new CallValue[] { });
        }

        public oCItem GetEquippedMeleeWeapon()
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.GetEquippedMeleeWeapon, new CallValue[] { });
        }

        public oCItem GetEquippedRangedWeapon()
        {
            return Process.THISCALL<oCItem>((uint)Address, (uint)FuncOffsets.GetEquippedRangedWeapon, new CallValue[] { });
        }

        public void EquipArmor(oCItem item)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.EquipArmor, new CallValue[] { item });
        }

        public void EquipWeapon(oCItem item)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.EquipWeapon, new CallValue[] { item });
        }
        public void EquipFarWeapon(oCItem item)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.EquipFarWeapon, new CallValue[] { item });
        }

        public void UnequipItem(oCItem item)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.UnequipItem, new CallValue[] { item });
        }

        public void DropUnconscious(int arg, oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DropUnconscious, new CallValue[] { new IntArg(arg), npc });
        }

        public void CheckUnconscious()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.CheckUnconscious, new CallValue[] { });
        }

        public void SetWeaponMode(int str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetWeaponMode, new CallValue[] { new IntArg(str) });
        }

        public void SetWeaponMode2(int str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetWeaponMode2_Int, new CallValue[] { new IntArg(str)});
        }

        public void SetWeaponMode2(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetWeaponMode2_Str, new CallValue[] { str });
        }

        public void DoDropVob(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DoDropVob, new CallValue[] { vob });
        }

        public void DoTakeVob(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DoTakeVob, new CallValue[] { vob });
        }
        #endregion



        public static void Freeze(Process process, bool r)
        {
            if(r)
                process.Write(1, 0x00AB2664);
            else
                process.Write(0, 0x00AB2664);
        }
    }
}
