using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects;
using GUC.Network;
using System.Collections;

namespace GUC.Server.Scripting.Objects.Character
{
    public class NPCProto : Vob, IEnumerable
    {
        #region constants
        public const String BODYMESH_MALE = "hum_body_Naked0";
        public const String BODYMESH_FEMALE = "Hum_Body_Babe0";

        public const int BODYTEX_MALE_PALE = 0;
        public const int BODYTEX_MALE_NORMAL = 1;
        public const int BODYTEX_MALE_LATINO = 2;
        public const int BODYTEX_MALE_BLACK = 3;
        public const int BODYTEX_MALE_PLAYER = 8;
        public const int BODYTEX_MALE_TAETOWIERT = 10;
        

        public const int BODYTEX_FEMALE_PALE = 4;
        public const int BODYTEX_FEMALE_NORMAL = 5;
        public const int BODYTEX_FEMALE_LATINO = 6;
        public const int BODYTEX_FEMALE_BLACK = 7;
        public const int BODYTEX_FEMALE_FELLKRAGEN = 11;
        public const int BODYTEX_FEMALE_SCHWARZKLEIN = 12;


        #region Gesichter
        // ------- Gesichter für Männer ------
        public const int Face_N_Gomez = 0;
        public const int Face_N_Scar = 1;
        public const int Face_N_Raven = 2;
        public const int Face_N_Bullit = 3;	//zu lieb!
        public const int Face_B_Thorus = 4;
        public const int Face_N_Corristo = 5;
        public const int Face_N_Milten = 6;
        public const int Face_N_Bloodwyn = 7;	//zu lieb!
        public const int Face_L_Scatty = 8;
        public const int Face_N_YBerion = 9;
        public const int Face_N_CoolPock = 10;
        public const int Face_B_CorAngar = 11;
        public const int Face_B_Saturas = 12;
        public const int Face_N_Xardas = 13;
        public const int Face_N_Lares = 14;
        public const int Face_L_Ratford = 15;
        public const int Face_N_Drax = 16;	//Buster
        public const int Face_B_Gorn = 17;
        public const int Face_N_Player = 18;
        public const int Face_P_Lester = 19;
        public const int Face_N_Lee = 20;
        public const int Face_N_Torlof = 21;
        public const int Face_N_Mud = 22;
        public const int Face_N_Ricelord = 23;
        public const int Face_N_Horatio = 24;
        public const int Face_N_Richter = 25;
        public const int Face_N_Cipher_neu = 26;
        public const int Face_N_Homer = 27;	//Headmesh thief
        public const int Face_B_Cavalorn = 28;
        public const int Face_L_Ian = 29;
        public const int Face_L_Diego = 30;
        public const int Face_N_MadPsi = 31;
        public const int Face_N_Bartholo = 32;
        public const int Face_N_Snaf = 33;
        public const int Face_N_Mordrag = 34;
        public const int Face_N_Lefty = 35;
        public const int Face_N_Wolf = 36;
        public const int Face_N_Fingers = 37;
        public const int Face_N_Whistler = 38;
        public const int Face_P_Gilbert = 39;
        public const int Face_L_Jackal = 40;

        //Pale
        public const int Face_P_ToughBald = 41;
        public const int Face_P_Tough_Drago = 42;
        public const int Face_P_Tough_Torrez = 43;
        public const int Face_P_Tough_Rodriguez = 44;
        public const int Face_P_ToughBald_Nek = 45;
        public const int Face_P_NormalBald = 46;
        public const int Face_P_Normal01 = 47;
        public const int Face_P_Normal02 = 48;
        public const int Face_P_Normal_Fletcher = 49;
        public const int Face_P_Normal03 = 50;
        public const int Face_P_NormalBart01 = 51;
        public const int Face_P_NormalBart_Cronos = 52;
        public const int Face_P_NormalBart_Nefarius = 53;
        public const int Face_P_NormalBart_Riordian = 54;
        public const int Face_P_OldMan_Gravo = 55;
        public const int Face_P_Weak_Cutter = 56;
        public const int Face_P_Weak_Ulf_Wohlers = 57;

        //Normal
        public const int Face_N_Important_Arto = 58;
        public const int Face_N_ImportantGrey = 59;
        public const int Face_N_ImportantOld = 60;
        public const int Face_N_Tough_Lee_ähnlich = 61;
        public const int Face_N_Tough_Skip = 62;
        public const int Face_N_ToughBart01 = 63;
        public const int Face_N_Tough_Okyl = 64;
        public const int Face_N_Normal01 = 65;
        public const int Face_N_Normal_Cord = 66;
        public const int Face_N_Normal_Olli_Kahn = 67;
        public const int Face_N_Normal02 = 68;
        public const int Face_N_Normal_Spassvogel = 69;
        public const int Face_N_Normal03 = 70;
        public const int Face_N_Normal04 = 71;
        public const int Face_N_Normal05 = 72;
        public const int Face_N_Normal_Stone = 73;
        public const int Face_N_Normal06 = 74;
        public const int Face_N_Normal_Erpresser = 75;
        public const int Face_N_Normal07 = 76;
        public const int Face_N_Normal_Blade = 77;
        public const int Face_N_Normal08 = 78;
        public const int Face_N_Normal14 = 79;
        public const int Face_N_Normal_Sly = 80;
        public const int Face_N_Normal16 = 81;
        public const int Face_N_Normal17 = 82;
        public const int Face_N_Normal18 = 83;
        public const int Face_N_Normal19 = 84;
        public const int Face_N_Normal20 = 85;
        public const int Face_N_NormalBart01 = 86;
        public const int Face_N_NormalBart02 = 87;
        public const int Face_N_NormalBart03 = 88;
        public const int Face_N_NormalBart04 = 89;
        public const int Face_N_NormalBart05 = 90;
        public const int Face_N_NormalBart06 = 91;
        public const int Face_N_NormalBart_Senyan = 92;
        public const int Face_N_NormalBart08 = 93;
        public const int Face_N_NormalBart09 = 94;
        public const int Face_N_NormalBart10 = 95;
        public const int Face_N_NormalBart11 = 96;
        public const int Face_N_NormalBart12 = 97;
        public const int Face_N_NormalBart_Dexter = 98;
        public const int Face_N_NormalBart_Graham = 99;
        public const int Face_N_NormalBart_Dusty = 100;
        public const int Face_N_NormalBart16 = 101;
        public const int Face_N_NormalBart17 = 102;
        public const int Face_N_NormalBart_Huno = 103;
        public const int Face_N_NormalBart_Grim = 104;
        public const int Face_N_NormalBart20 = 105;
        public const int Face_N_NormalBart21 = 106;
        public const int Face_N_NormalBart22 = 107;
        public const int Face_N_OldBald_Jeremiah = 108;
        public const int Face_N_Weak_Ulbert = 109;
        public const int Face_N_Weak_BaalNetbek = 110;
        public const int Face_N_Weak_Herek = 111;
        public const int Face_N_Weak04 = 112;
        public const int Face_N_Weak05 = 113;
        public const int Face_N_Weak_Orry = 114;
        public const int Face_N_Weak_Asghan = 115;
        public const int Face_N_Weak_Markus_Kark = 116;
        public const int Face_N_Weak_Cipher_alt = 117;
        public const int Face_N_NormalBart_Swiney = 118;
        public const int Face_N_Weak12 = 119;

        //Latinos
        public const int Face_L_ToughBald01 = 120;
        public const int Face_L_Tough01 = 121;
        public const int Face_L_Tough02 = 122;
        public const int Face_L_Tough_Santino = 123;
        public const int Face_L_ToughBart_Quentin = 124;
        public const int Face_L_Normal_GorNaBar = 125;
        public const int Face_L_NormalBart01 = 126;
        public const int Face_L_NormalBart02 = 127;
        public const int Face_L_NormalBart_Rufus = 128;

        //Black
        public const int Face_B_ToughBald = 129;
        public const int Face_B_Tough_Pacho = 130;
        public const int Face_B_Tough_Silas = 131;
        public const int Face_B_Normal01 = 132;
        public const int Face_B_Normal_Kirgo = 133;
        public const int Face_B_Normal_Sharky = 134;
        public const int Face_B_Normal_Orik = 135;
        public const int Face_B_Normal_Kharim = 136;

        // ------ Gesichter für Frauen ------

        public const int FaceBabe_N_BlackHair = 137;
        public const int FaceBabe_N_Blondie = 138;
        public const int FaceBabe_N_BlondTattoo = 139;
        public const int FaceBabe_N_PinkHair = 140;
        public const int FaceBabe_L_Charlotte = 141;
        public const int FaceBabe_B_RedLocks = 142;
        public const int FaceBabe_N_HairAndCloth = 143;
        //
        public const int FaceBabe_N_WhiteCloth = 144;
        public const int FaceBabe_N_GreyCloth = 145;
        public const int FaceBabe_N_Brown = 146;
        public const int FaceBabe_N_VlkBlonde = 147;
        public const int FaceBabe_N_BauBlonde = 148;
        public const int FaceBabe_N_YoungBlonde = 149;
        public const int FaceBabe_N_OldBlonde = 150;
        public const int FaceBabe_P_MidBlonde = 151;
        public const int FaceBabe_N_MidBauBlonde = 152;
        public const int FaceBabe_N_OldBrown = 153;
        public const int FaceBabe_N_Lilo = 154;
        public const int FaceBabe_N_Hure = 155;
        public const int FaceBabe_N_Anne = 156;
        public const int FaceBabe_B_RedLocks2 = 157;
        public const int FaceBabe_L_Charlotte2 = 158;


        //-----------------ADD ON---------------------------------
        public const int Face_N_Fortuno = 159;

        //Piraten
        public const int Face_P_Greg = 160;
        public const int Face_N_Pirat01 = 161;
        public const int Face_N_ZombieMud = 162;
        #endregion

        #endregion


        internal NPCProto(GUC.WorldObjects.Character.NPCProto proto)
            : base(proto)
        {
            
        }

        internal GUC.WorldObjects.Character.NPCProto proto { get { return (GUC.WorldObjects.Character.NPCProto)vob; } }


        public static NPCProto[] getAll()
        {
            NPCProto[] protoList = new NPCProto[sWorld.NpcProtoList.Count()];

            for (int i = 0; i < sWorld.NpcProtoList.Count(); i++ )
            {
                protoList[i] = sWorld.NpcProtoList[i].ScriptingNPC;
            }

            return protoList;
        }

        public static IEnumerator ToEnumerable()
        {
            foreach (GUC.WorldObjects.Character.NPCProto item in sWorld.NpcProtoList)
            {
                yield return (NPCProto)item.ScriptingNPC;
            }
        }

        #region Fields

        public String Name { get { return proto.Name; } set { setName(value); } }

        public Vec3f Scale { get { return proto.Scale; } set { setScale(value); } }

        public ColorRGBA Color { get; set; }

        public int Strength {
            get { return proto.Attributes[(byte)NPCAttribute.ATR_STRENGTH]; }
            set { setAttribute(NPCAttribute.ATR_STRENGTH, value); }
        }

        public int Dexterity
        {
            get { return proto.Attributes[(byte)NPCAttribute.ATR_DEXTERITY]; }
            set { setAttribute(NPCAttribute.ATR_DEXTERITY, value); }
        }

        public int HP
        {
            get { return proto.Attributes[(byte)NPCAttribute.ATR_HITPOINTS]; }
            set { setAttribute(NPCAttribute.ATR_HITPOINTS, value); }
        }

        public int HPMax
        {
            get { return proto.Attributes[(byte)NPCAttribute.ATR_HITPOINTS_MAX]; }
            set { setAttribute(NPCAttribute.ATR_HITPOINTS_MAX, value); }
        }

        public int MP
        {
            get { return proto.Attributes[(byte)NPCAttribute.ATR_MANA]; }
            set { setAttribute(NPCAttribute.ATR_MANA, value); }
        }

        public int MPMax
        {
            get { return proto.Attributes[(byte)NPCAttribute.ATR_MANA_MAX]; }
            set { setAttribute(NPCAttribute.ATR_MANA_MAX, value); }
        }

        

        public Item EquippedArmor { 
            get {
                if (proto.Armor == null)
                    return null;
                return proto.Armor.ScriptingProto;
            } }

        public Item EquippedWeapon
        {
            get
            {
                if (proto.Weapon == null)
                    return null;
                return proto.Weapon.ScriptingProto;
            }
        }

        public Item EquippedRangeWeapon
        {
            get
            {
                if (proto.RangeWeapon == null)
                    return null;
                return proto.RangeWeapon.ScriptingProto;
            }
        }

        

        public int WeaponMode { get { return proto.WeaponMode; } set { setWeaponMode(value); } }

        #region Protection
        public int ProtectionFire
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_FIRE); }
            set { setProtection(DamageTypeIndex.DAM_INDEX_FIRE, value); }
        }

        public int ProtectionEdge
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_EDGE); }
            set { setProtection(DamageTypeIndex.DAM_INDEX_EDGE, value); }
        }

        public int ProtectionBarrier
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_BARRIER); }
            set { setProtection(DamageTypeIndex.DAM_INDEX_BARRIER, value); }
        }

        public int ProtectionBlunt
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_BLUNT); }
            set { setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, value); }
        }

        public int ProtectionFall
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_FALL); }
            set { setProtection(DamageTypeIndex.DAM_INDEX_FALL, value); }
        }

        public int ProtectionFly
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_FLY); }
            set { setProtection(DamageTypeIndex.DAM_INDEX_FLY, value); }
        }

        public int ProtectionMagic
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_MAGIC); }
            set { setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, value); }
        }

        public int ProtectionPoint
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_POINT); }
            set { setProtection(DamageTypeIndex.DAM_INDEX_POINT, value); }
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Returns an Integer with the callback ID!
        /// </summary>
        /// <returns></returns>
        public int CanSee(Vob vob)
        {
            Player plToCheck = null;
            if (this is NPC)
            {
                if (((WorldObjects.Character.NPC)proto).NpcController == null)
                    return -1;
            }
            else
            {
                plToCheck = (Player)this;
            }

            int callBackID = sWorld.getNewCallBackID();
            
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.CallbackNPCCanSee);
            stream.Write(callBackID);
            stream.Write(proto.ID);
            stream.Write(vob.ID);
            using (RakNet.RakNetGUID guid = new RakNetGUID(plToCheck.proto.Guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            return callBackID;
        }


        public Item getSlotItem(int index)
        {
            if (index >= 9)
                throw new ArgumentException("Index can't be greater than 8!");
            if (proto.Slots[index] == null)
                return null;
            return proto.Slots[index].ScriptingProto;
        }

        public void setWeaponMode(int weaponMode)
        {
            proto.WeaponMode = weaponMode;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCProtoSetWeaponMode);
            stream.Write(vob.ID);
            stream.Write(proto.WeaponMode);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setSlotItem(int slot, Item item)
        {
            if (item != null)
                proto.Slots[slot] = item.ProtoItem;
            else
                proto.Slots[slot] = null;
            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetSlotMessage);
            stream.Write(vob.ID);
            stream.Write(slot);
            if (item == null)
                stream.Write(0);
            else
                stream.Write(item.ID);

            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public int getProtection(DamageTypes index)
        {
            if (EquippedArmor != null)
            {
                return EquippedArmor.getProtection(index.getDamageTypeIndex());
            }
            else
            {
                return proto.protection[(int)index.getDamageTypeIndex()];
            }
        }

        public int getProtection(DamageTypeIndex index)
        {
            if (EquippedArmor != null)
            {
                return EquippedArmor.getProtection(index) + proto.protection[(int)index];
            }
            else
            {
                return proto.protection[(int)index];
            }
        }

        public void setProtection(DamageTypeIndex index, int value)
        {
            proto.protection[(int)index] = value;
        }

        public int getTotalDamage()
        {
            if (EquippedWeapon != null)
                return EquippedWeapon.TotalDamage;
            else if (EquippedRangeWeapon != null)
                return EquippedRangeWeapon.TotalDamage;
            else
                return proto.totalDamage;
        }

        public DamageTypes getDamageType()
        {
            if (EquippedWeapon != null)
                return EquippedWeapon.DamageType;
            else if (EquippedRangeWeapon != null)
                return EquippedRangeWeapon.DamageType;
            else
                return proto.damageType;
        }

        public void setTotalDamage(int damage)
        {
            proto.totalDamage = damage;
        }

        public void setDamageType(DamageTypes type)
        {
            proto.damageType = type;
        }

        public void setDamage(DamageTypeIndex dti, int value)
        {
            proto.damages[(int)dti] = value;
        }

        public int getDamage(DamageTypeIndex dti)
        {
            if (EquippedWeapon != null)
                return EquippedWeapon.getDamage(dti);
            else if (EquippedRangeWeapon != null)
                return EquippedRangeWeapon.getDamage(dti);
            else
                return proto.damages[(int)dti];
        }


        public virtual void setScale(Vec3f scale)
        {
            proto.Scale = scale;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ScaleMessage);
            stream.Write(vob.ID);
            stream.Write(proto.Scale);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public virtual void setFatness(float Fatness)
        {
            proto.Fatness = Fatness;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCFatnessMessage);
            stream.Write(vob.ID);
            stream.Write(proto.Fatness);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setName(String name)
        {
            if (name == null)
                throw new ArgumentNullException("Name can't be null!");

            proto.Name = name;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChangeNameMessage);
            stream.Write(proto.ID);
            stream.Write(name);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void dropUnconscious(float time)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            if (HP <= 0)
                return;
            HP = 1;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.DropUnconsciousMessage);
            stream.Write(this.ID);
            stream.Write(time);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void revive()
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            HP = HPMax;
            MP = MPMax;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ReviveMessage);
            stream.Write(this.proto.ID);
            
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public int getAttribute(NPCAttribute attrib)
        {
            if (attrib >= NPCAttribute.ATR_MAX)
                throw new ArgumentException("attribute is not valid!");
            return proto.Attributes[(byte)attrib];
        }

        public int getHitChances(NPCTalent talents)
        {
            if (talents != NPCTalent.H1 && talents != NPCTalent.H2 && talents != NPCTalent.Bow && talents != NPCTalent.CrossBow)
                throw new ArgumentException("Talents have to be fighting skills Like H1, H2, Bow or CrossBow!");

            return proto.Hitchances[(byte)talents];
        }

        public int getTalentValues(NPCTalent talent)
        {
            if (talent == NPCTalent.Unknown || talent == NPCTalent.MaxTalents)
                throw new ArgumentException("Invalid Talent: " + talent);

            return proto.TalentValues[(byte)talent];
        }

        public int getTalentSkills(NPCTalent talent)
        {
            if (talent == NPCTalent.Unknown || talent == NPCTalent.MaxTalents)
                throw new ArgumentException("Invalid Talent: " + talent);

            return proto.TalentSkills[(byte)talent];

        }

        public void setInvisible(bool invisible)
        {
            proto.IsInvisible = invisible;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCSetInvisibleMessage);
            stream.Write(proto.ID);
            stream.Write(invisible);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setInvisible(Player player, bool invisible)
        {
            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCSetInvisibleMessage);
            stream.Write(proto.ID);
            stream.Write(invisible);
            using(RakNetGUID guid = player.proto.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void hideName(bool invisible)
        {
            proto.hideName = invisible;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCSetInvisibleName);
            stream.Write(proto.ID);
            stream.Write(invisible);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void hideNameFrom(Player player, bool invisible)
        {
            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCSetInvisibleName);
            stream.Write(proto.ID);
            stream.Write(invisible);
            using (RakNetGUID guid = player.proto.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void setAttribute(NPCAttribute attrib, int value)
        {
            if (attrib >= NPCAttribute.ATR_MAX)
                throw new ArgumentException("attribute is not valid!");
            if (value < 0)
                value = 0;
            if (attrib == NPCAttribute.ATR_HITPOINTS && value > HPMax)
                value = HPMax;
            else if (attrib == NPCAttribute.ATR_MANA && value > MPMax)
                value = MPMax;

            if (proto.Attributes[(byte)attrib] == value)
                return;
            int oV = proto.Attributes[(byte)attrib];

            proto.Attributes[(byte)attrib] = value;

            NPCProto.isOnAttributeChanged(this, attrib, oV, value);


            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCChangeAttributeMessage);
            stream.Write(proto.ID);
            stream.Write((byte)attrib);
            stream.Write(value);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setHitchances(NPCTalent talents, int value)
        {
            if (talents != NPCTalent.H1 && talents != NPCTalent.H2 && talents != NPCTalent.Bow && talents != NPCTalent.CrossBow)
                throw new ArgumentException("Talents have to be fighting skills Like H1, H2, Bow or CrossBow!");
            if (proto.Hitchances[(byte)talents] == value)
                return;

            int oV = proto.Hitchances[(byte)talents];
            proto.Hitchances[(byte)talents] = value;

            NPCProto.isOnHitchancesChanged(this, talents, oV, value);

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCChangeSkillMessage);
            stream.Write(proto.ID);
            stream.Write((byte)ChangeSkillType.Hitchances);
            stream.Write((byte)talents);
            stream.Write(value);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setTalentValues(NPCTalent talent, int value)
        {
            if (talent == NPCTalent.Unknown || talent == NPCTalent.MaxTalents)
                throw new ArgumentException("Invalid Talent: "+talent);

            if (proto.TalentValues[(byte)talent] == value)
                return;
            int oV = proto.TalentValues[(byte)talent];
            proto.TalentValues[(byte)talent] = value;

            NPCProto.isOnTalentValueChanged(this, talent, oV, value);

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCChangeSkillMessage);
            stream.Write(proto.ID);
            stream.Write((byte)ChangeSkillType.Value);
            stream.Write((byte)talent);
            stream.Write(value);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setTalentSkills(NPCTalent talent, int value)
        {
            if (talent == NPCTalent.Unknown || talent == NPCTalent.MaxTalents)
                throw new ArgumentException("Invalid Talent: " + talent);

            if (proto.TalentSkills[(byte)talent] == value)
                return;
            int oV = proto.TalentSkills[(byte)talent];
            proto.TalentSkills[(byte)talent] = value;

            NPCProto.isOnTalentSkillChanged(this, talent, oV, value);

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCChangeSkillMessage);
            stream.Write(proto.ID);
            stream.Write((byte)ChangeSkillType.Skill);
            stream.Write((byte)talent);
            stream.Write(value);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public Item[] getItemList()
        {
            Item[] itemList = new Item[this.proto.ItemList.Count];
            int i = 0;
            foreach(WorldObjects.Item item in this.proto.ItemList){
                itemList[i] = item.ScriptingProto;
                i += 1;
            }

            return itemList;
        }

        public IEnumerator GetEnumerator()
        {
            foreach (GUC.WorldObjects.Item item in proto.ItemList)
            {
                yield return item.ScriptingProto;
            }
        }

        public int ItemCount { get { return proto.ItemList.Count; } }

        public Item this[int i]
        {
            get { return proto.ItemList[i].ScriptingProto; }
        }



        public Item addItem(String instance, int amount)
        {
            if (instance == null)
                throw new ArgumentNullException("Instance-String can not be null!");
            if (ItemInstance.getItemInstance(instance) == null)
                throw new ArgumentException("Instance: "+instance+" can not be found!");
            return addItem(ItemInstance.getItemInstance(instance), amount);
        }

        public Item addItem(ItemInstance instance, int amount)
        {
            if (instance == null)
                throw new ArgumentNullException("Instance can't be null!");
            if (amount <= 0)
                throw new ArgumentException("amount can't be 0 or lower!");

            if (instance.itemInstances.Flags.HasFlag(Flags.ITEM_MULTI))
            {
                Item oldItem = null;
                foreach(WorldObjects.Item i in proto.ItemList){
                    if( i.ItemInstance == instance.itemInstances){
                        oldItem = i.ScriptingProto;
                        break;
                    }
                }

                if (oldItem != null)
                {
                    oldItem.Amount = oldItem.Amount + amount;
                    return oldItem;
                }
            }

            Item itm = new Item(instance, amount);
            //sWorld.addVob(itm);
            proto.addItem(itm.ProtoItem);

            if (!created)
                return itm;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AddItemMessage);
            stream.Write(proto.ID);
            stream.Write(itm.ID);
            //itm.Write(stream);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


            return itm;
        }

        public void hit(NPCProto victim)
        {
            hit(victim, DamageTypes.DAM_BLUNT, 0, this.proto.Weapon.ScriptingProto, null, null, null, 0.0f);
        }

        public void hit(NPCProto victim, DamageTypes damageMode)
        {
            hit(victim, damageMode, 0, this.proto.Weapon.ScriptingProto, null, null, null, 0.0f);
        }

        public void hit(NPCProto victim, DamageTypes damageMode, int weaponMode)
        {
            hit(victim, damageMode, weaponMode, this.proto.Weapon.ScriptingProto, null, null, null, 0.0f);
        }

        public void hit(NPCProto victim, DamageTypes damageMode, int weaponMode, Vec3f hitLoc)
        {
            hit(victim, damageMode, weaponMode, this.proto.Weapon.ScriptingProto, null, hitLoc, null, 0.0f);
        }

        public void hit(NPCProto victim, DamageTypes damageMode, int weaponMode, Item weapon, Vec3f hitLoc)
        {
            hit(victim, damageMode, weaponMode, weapon, null, hitLoc, null, 0.0f);
        }

        public void hit(NPCProto victim, DamageTypes damageMode, int weaponMode, Item weapon, Spell spell, Vec3f hitLoc)
        {
            hit(victim, damageMode, weaponMode, weapon, spell, hitLoc, null, 0.0f);
        }

        public void hit(NPCProto victim, DamageTypes damageMode, int weaponMode, Item weapon, Spell spell, Vec3f hitLoc, Vec3f flyDir, float fallDownDistanceY)
        {
            WorldObjects.Spell objSpell = (spell == null) ? null : spell.spell;
            WorldObjects.Item objItem = (weapon == null) ? null : weapon.ProtoItem;
            GUC.Server.Network.Messages.PlayerCommands.OnDamageMessage.Write(victim.proto, damageMode, hitLoc, flyDir, this.proto, weaponMode, objSpell, objItem, fallDownDistanceY, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS);

            NPCProto.isOnDamage(victim, (DamageTypes)damageMode, hitLoc, flyDir, this, weaponMode, spell, weapon, fallDownDistanceY);

        }

        /// <summary>
        /// The npc/player drops the item into the world.
        /// </summary>
        /// <param name="item">the item needs to be in the inventory of the player</param>
        public void dropItem(Item item)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            if (item.ProtoItem.Container != this.proto)
                throw new Exception("Item does not belong to NPC!");
            proto.DropItem(item.ProtoItem);

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.DropItemMessage);
            stream.Write(proto.ID);
            stream.Write(item.ProtoItem.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        /// <summary>
        /// The npc/player takes an item lying in the world.
        /// </summary>
        /// <param name="item">The item needs to be in the world.</param>
        public void takeItem(Item item)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            if (item.ProtoItem.Container == null || !(item.ProtoItem.Container is WorldObjects.World))
                throw new ArgumentException("Item has to be an Item in the World!");
            if (item.ProtoItem.Container != GUC.WorldObjects.sWorld.getWorld(this.proto.Map))
                throw new ArgumentException("Item is not in the same World!");

            proto.TakeItem(item.ProtoItem);

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.TakeItemMessage);
            stream.Write(proto.ID);
            stream.Write(item.ProtoItem.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        /// <summary>
        /// This function can only be created with npcs which are not created!
        /// For created npcs and players use
        /// <code> setVisual(String visual, String bodyMesh, int bodyTex, int skinColor, String headMesh, int headTex, int TeethTex)</code>
        /// </summary>
        /// <param name="visual"></param>
        public override void setVisual(String visual)
        {
            if (!created)
                vob.Visual = visual;
            else
                throw new NotImplementedException("You can't use setVisual with NPCs, use SetVisual(String visual, ....) instead!");
        }


        public String BodyMesh { get { return proto.BodyMesh; } }
        public String HeadMesh { get { return proto.HeadMesh; } }

        public int SkinColor { get { return proto.SkinColor; } }
        public int BodyTex { get { return proto.BodyTex; } }
        public int HeadTex { get { return proto.HeadTex; } }
        public int TeethTex { get { return proto.TeethTex; } }

        public bool isDead { get { return proto.IsDead; } }
        public bool isUnconscious { get { return proto.IsUnconcious; } }
        public bool isSwimming { get { return proto.IsSwimming; } }

        public virtual void setVisual(String visual, String bodyMesh, int bodyTex, int skinColor, String headMesh, int headTex, int TeethTex)
        {
            vob.Visual = visual;
            proto.BodyMesh = bodyMesh;
            proto.BodyTex = bodyTex;
            proto.SkinColor = skinColor;
            proto.HeadMesh = headMesh;
            proto.HeadTex = headTex;
            proto.TeethTex = TeethTex;
            
            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVisualMessage);
            stream.Write(vob.ID);
            stream.Write(visual);
            stream.Write(bodyMesh);
            stream.Write(bodyTex);
            stream.Write(skinColor);
            stream.Write(headMesh);
            stream.Write(headTex);
            stream.Write(TeethTex);
            
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void clearInventory()
        {
            foreach (WorldObjects.Item item in proto.ItemList.ToArray())
            {
                sWorld.removeVob(item);
            }

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ClearInventory);
            stream.Write(this.ID);

            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void Equip(Item item)
        {
            if (item == null)
                throw new Exception("Item was null!");
            if (item.ProtoItem.Container != this.proto)
                throw new Exception("Item must be in the inventory of the player!");

            if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF) ||
                item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF) ||
                item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR))
            {
                if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF) && this.EquippedWeapon != null)
                {
                    UnEquip(this.EquippedWeapon);
                }
                else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF) && this.EquippedRangeWeapon != null)
                {
                    UnEquip(this.EquippedRangeWeapon);
                }
                else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR) && this.EquippedArmor != null)
                {
                    UnEquip(this.EquippedArmor);
                }
            }

            if(!proto.EquippedList.Contains(item.ProtoItem))
                proto.EquippedList.Add(item.ProtoItem);

            if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF))
                proto.Weapon = item.ProtoItem;
            else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF))
                proto.RangeWeapon = item.ProtoItem;
            else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR))
                proto.Armor = item.ProtoItem;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.EquipItemMessage);
            stream.Write(proto.ID);
            stream.Write(item.ID);
            stream.Write(true);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void UnEquip(Item item)
        {
            if (item == null)
                throw new Exception("Item was null!");
            if (item.ProtoItem.Container != this.proto)
                throw new Exception("Item must be in the inventory of the player!");

            if (proto.EquippedList.Contains(item.ProtoItem))
                proto.EquippedList.Remove(item.ProtoItem);
            if (this.EquippedArmor == item)
                this.proto.Armor = null;
            if (this.EquippedWeapon == item)
                this.proto.Weapon = null;
            if (this.EquippedRangeWeapon == item)
                this.proto.RangeWeapon = null;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.EquipItemMessage);
            stream.Write(proto.ID);
            stream.Write(item.ID);
            stream.Write(false);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        public void clearAnimation()
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write("");
            stream.Write((byte)5);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void playAnimation( String anim )
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write(anim);
            stream.Write((byte)1);
            if (this is Player && !vob.IsSpawned)
            {
                using (RakNetGUID guid = ((WorldObjects.Character.Player)this.proto).GUID)
                {
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
                }
            }
            else
            {
                this.proto.SendToAreaPlayersAndPlayer(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED);
            }
            
            //Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void stopAnimation(String anim)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write(anim);
            stream.Write((byte)0);
            if (this is Player && !vob.IsSpawned)
            {
                using (RakNetGUID guid = ((WorldObjects.Character.Player)this.proto).GUID)
                {
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
                }
            }
            else
            {
                this.proto.SendToAreaPlayersAndPlayer(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED);
            }
            //Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void ApplyOverlay(String anim)
        {
            if (!this.proto.Overlays.Contains(anim))
            {
                this.proto.Overlays.Add(anim);
            }

            if (!created)
                return;
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write(anim);
            stream.Write((byte)2);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void RemoveOverlay(String anim)
        {
            if (this.proto.Overlays.Contains(anim))
            {
                this.proto.Overlays.Remove(anim);
            }

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write(anim);
            stream.Write((byte)3);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void ClearOverlays()
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");

            this.proto.Overlays.Clear();

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write("");
            stream.Write((byte)4);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        public void startDialogAnimation()
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.StartDialogAnimMessage);
            stream.Write(proto.ID);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
        #endregion



        #region Events
        #region Callbacks

        #region OnCanSee
        public event GUC.Server.Scripting.Events.NPCCanSeeEventHandler OnCanSee;
        internal void iOnCanSee(int callbackID, NPCProto proto, Vob vob, bool canSee)
        {
            if (OnCanSee != null)
                OnCanSee(callbackID, proto, vob, canSee);
        }

        public static event GUC.Server.Scripting.Events.NPCCanSeeEventHandler sOnCanSeeCallback;
        internal static void iOnCanSeeCallback(int callbackID, NPCProto proto, Vob vob, bool canSee)
        {
            proto.iOnCanSee(callbackID, proto, vob, canSee);
            if (sOnCanSeeCallback != null)
                sOnCanSeeCallback(callbackID, proto, vob, canSee);
        }

        #endregion

        #endregion

        #region OnAnimation
        public event GUC.Server.Scripting.Events.NPCAnimationUpdate OnAnimation;
        internal void iOnAnimation(NPCProto proto, short animID, short oldAnimID)
        {
            if (OnAnimation != null)
                OnAnimation(proto, animID, oldAnimID);
        }

        public static event GUC.Server.Scripting.Events.NPCAnimationUpdate sOnAnimation;
        internal static void isOnAnimation(NPCProto proto, short animID, short oldAnimID)
        {
            proto.iOnAnimation(proto, animID, oldAnimID);
            if (sOnAnimation != null)
                sOnAnimation(proto, animID, oldAnimID);
        }


        #endregion


        #region OnEquip
        public event GUC.Server.Scripting.Events.NPCEquipEventHandler OnEquip;
        internal void iOnEquip(NPCProto proto, Item item)
        {
            if (OnEquip != null)
                OnEquip(proto, item);
        }

        public static event GUC.Server.Scripting.Events.NPCEquipEventHandler sOnEquip;
        internal static void isOnEquip(NPCProto proto, Item item)
        {
            proto.iOnEquip(proto, item);
            item.ItemInstance.iOnEquip(proto, item);
            if (sOnEquip != null)
                sOnEquip(proto, item);
        }

        public event GUC.Server.Scripting.Events.NPCEquipEventHandler OnUnEquip;
        internal void iOnUnEquip(NPCProto proto, Item item)
        {
            if (OnUnEquip != null)
                OnUnEquip(proto, item);
        }

        public static event GUC.Server.Scripting.Events.NPCEquipEventHandler sOnUnEquip;
        internal static void isOnUnEquip(NPCProto proto, Item item)
        {
            proto.iOnUnEquip(proto, item);
            item.ItemInstance.iOnUnEquip(proto, item);
            if (sOnUnEquip != null)
                sOnUnEquip(proto, item);
        }

        #endregion

        #region OnUseItem
        public event GUC.Server.Scripting.Events.UseItemEventHandler OnUseItem;
        internal void iOnUseItem(NPCProto proto, Item item, short state, short targetState)
        {
            if (OnUseItem != null)
                OnUseItem(proto, item, state, targetState);
        }

        public static event GUC.Server.Scripting.Events.UseItemEventHandler sOnUseItem;
        internal static void isOnUseItem(NPCProto proto, Item item, short state, short targetState)
        {
            proto.iOnUseItem(proto, item, state, targetState);
            item.ItemInstance.iOnUse(proto, item, state, targetState);
            if (sOnUseItem != null)
                sOnUseItem(proto, item, state, targetState);
        }

        #endregion

        #region Spells
        public event GUC.Server.Scripting.Events.CastSpell OnCastSpell;
        internal void iOnCastSpell(NPCProto caster, Spell spell, Vob target)
        {
            if (OnCastSpell != null)
                OnCastSpell(caster, spell, target);
        }

        public static event GUC.Server.Scripting.Events.CastSpell sOnCastSpell;
        internal static void isOnCastSpell(NPCProto caster, Spell spell, Vob target)
        {
            caster.iOnCastSpell(caster, spell, target);
            spell.iOnCastSpell(caster, spell, target);
            if (sOnCastSpell != null)
                sOnCastSpell(caster, spell, target);
        }

        #endregion

        #region Attributes
        public event GUC.Server.Scripting.Events.AttributeChangedEventHandler OnAttributeChanged;
        internal void iOnAttributeChanged(NPCProto proto, NPCAttribute attrib, int oldValue, int newValue)
        {
            if (OnAttributeChanged != null)
                OnAttributeChanged(proto, attrib, oldValue, newValue);
        }

        public static event GUC.Server.Scripting.Events.AttributeChangedEventHandler sOnAttributeChanged;
        internal static void isOnAttributeChanged(NPCProto proto, NPCAttribute attrib, int oldValue, int newValue)
        {
            proto.iOnAttributeChanged(proto, attrib, oldValue, newValue);

            if (sOnAttributeChanged != null)
                sOnAttributeChanged(proto, attrib, oldValue, newValue);
        }

        #endregion

        #region Talent_Values
        public event GUC.Server.Scripting.Events.TalentChangedEventHandler OnTalentValueChanged;
        internal void iOnTalentValueChanged(NPCProto proto, NPCTalent talent, int oldValue, int newValue)
        {
            if (OnTalentValueChanged != null)
                OnTalentValueChanged(proto, talent, oldValue, newValue);
        }

        public static event GUC.Server.Scripting.Events.TalentChangedEventHandler sOnTalentValueChanged;
        internal static void isOnTalentValueChanged(NPCProto proto, NPCTalent talent, int oldValue, int newValue)
        {
            proto.iOnTalentValueChanged(proto, talent, oldValue, newValue);

            if (sOnTalentValueChanged != null)
                sOnTalentValueChanged(proto, talent, oldValue, newValue);
        }

        #endregion

        #region Talent_Skills
        public event GUC.Server.Scripting.Events.TalentChangedEventHandler OnTalentSkillChanged;
        internal void iOnTalentSkillChanged(NPCProto proto, NPCTalent talent, int oldValue, int newValue)
        {
            if (OnTalentSkillChanged != null)
                OnTalentSkillChanged(proto, talent, oldValue, newValue);
        }

        public static event GUC.Server.Scripting.Events.TalentChangedEventHandler sOnTalentSkillChanged;
        internal static void isOnTalentSkillChanged(NPCProto proto, NPCTalent talent, int oldValue, int newValue)
        {
            proto.iOnTalentSkillChanged(proto, talent, oldValue, newValue);

            if (sOnTalentSkillChanged != null)
                sOnTalentSkillChanged(proto, talent, oldValue, newValue);
        }

        #endregion

        #region Hitchances
        public event GUC.Server.Scripting.Events.TalentChangedEventHandler OnHitchancesChanged;
        internal void iOnHitchancesChanged(NPCProto proto, NPCTalent talent, int oldValue, int newValue)
        {
            if (OnHitchancesChanged != null)
                OnHitchancesChanged(proto, talent, oldValue, newValue);
        }

        public static event GUC.Server.Scripting.Events.TalentChangedEventHandler sOnHitchancesChanged;
        internal static void isOnHitchancesChanged(NPCProto proto, NPCTalent talent, int oldValue, int newValue)
        {
            proto.iOnHitchancesChanged(proto, talent, oldValue, newValue);

            if (sOnHitchancesChanged != null)
                sOnHitchancesChanged(proto, talent, oldValue, newValue);
        }

        #endregion


        #region Damage
        public event GUC.Server.Scripting.Events.PlayerDamageEventHandler OnDamage;
        internal void iOnDamage(NPCProto victim, DamageTypes damageMode, Vec3f hitLoc, Vec3f flyDir, Vob attacker, int weaponMode, Spell spellID, Item weapon, float fallDownDistanceY)
        {
            if (OnDamage != null)
                OnDamage(victim, damageMode, hitLoc, flyDir, attacker, weaponMode, spellID, weapon, fallDownDistanceY);
        }

        public static event Events.PlayerDamageEventHandler sOnDamage;
        internal static void isOnDamage(NPCProto victim, DamageTypes damageMode, Vec3f hitLoc, Vec3f flyDir, Vob attacker, int weaponMode, Spell spellID, Item weapon, float fallDownDistanceY)
        {
            victim.iOnDamage(victim, damageMode, hitLoc, flyDir, attacker, weaponMode, spellID, weapon, fallDownDistanceY);
            if (sOnDamage != null)
                sOnDamage(victim, damageMode, hitLoc, flyDir, attacker, weaponMode, spellID, weapon, fallDownDistanceY);
        }


        #endregion

        #region TakeItem
        public event GUC.Server.Scripting.Events.PlayerItemEventHandler OnTakeItem;
        internal void iOnTakeItem(NPCProto npc, Item item, int amount)
        {
            if (OnTakeItem != null)
                OnTakeItem(npc, item, amount);
        }

        public static event GUC.Server.Scripting.Events.PlayerItemEventHandler sOnTakeItem;
        internal static void OnItemTakes(NPCProto npc, Item item, int amount)
        {
            npc.iOnTakeItem(npc, item, amount);
            if (sOnTakeItem != null)
                sOnTakeItem(npc, item, amount);
        }
        #endregion

        #region DropItem
        public event GUC.Server.Scripting.Events.PlayerItemEventHandler OnDropItem;
        internal void iOnDropItem(NPCProto npc, Item item, int amount)
        {
            if (OnDropItem != null)
                OnDropItem(npc, item, amount);
        }

        public static event GUC.Server.Scripting.Events.PlayerItemEventHandler sOnDropItem;
        internal static void isOnDropItem(NPCProto npc, Item item, int amount)
        {
            npc.iOnDropItem(npc, item, amount);
            if (sOnDropItem != null)
                sOnDropItem(npc, item, amount);
        }
        #endregion




        

        #endregion


    }
}
