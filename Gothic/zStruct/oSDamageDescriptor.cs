using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;

namespace Gothic.zStruct
{
    public class oSDamageDescriptor: Gothic.zClasses.zClass
    {
        public oSDamageDescriptor()
        { }
        public oSDamageDescriptor(Process process, int address)
            : base(process, address)
        {

        }

        #region OffsetList
        public enum Offsets
        {
            fieldsValid = 0,
            vobAttacker = 4,
            npcAttacker = 8,
            vobHit = 12,
            fXHit = 16,
            itemWeapon = 20,
            spellID = 24,
            spellcat = 28,
            Spelllevel = 32,
            ModeDamage = 36,
            ModeWeapon = 40,
            Damage = 44,
            DamageTotal = 76,
            DamageMultiplier = 80,
            LocationHit = 84,
            DirectionFly = 96,
            Duration = 108,
            TimeInterval = 112,
            DamagePerInterval = 116,
            DamageDontKill = 120,
            bOnce = 124,
            bFinished = 128,
            bDead = 132,
            bUnconscious = 136,
            bReserved = 140,
            Azimuth = 144,
            Elevation = 148,
            TimeCurrent = 152,
            DamageReal = 156,
            DamageEffective = 160
        }

        public enum DamageTypes
        {
            DAM_INVALID = 0,
            DAM_BARRIER = 1,
            DAM_BLUNT = 2,
            DAM_EDGE = 4,
            DAM_FIRE = 8,
            DAM_FLY = 16,
            DAM_MAGIC = 32,
            DAM_POINT = 64,
            DAM_FALL = 128
        }
        #endregion

        #region Fields
        public oCNpc AttackerNPC
        {
            get { return new oCNpc(Process, Process.ReadInt(Address+(int)Offsets.npcAttacker)); }
            set { Process.Write(value.Address, Address + (int)Offsets.npcAttacker); }
        }

        public zCVob AttackerVob
        {
            get { return new zCVob(Process, Process.ReadInt(Address+(int)Offsets.vobAttacker)); }
            set { Process.Write(value.Address, Address + (int)Offsets.vobAttacker); }
        }

        public oCItem Weapon
        {
            get { return new oCItem(Process, Process.ReadInt(Address + (int)Offsets.itemWeapon)); }
            set { Process.Write(value.Address, Address + (int)Offsets.itemWeapon); }
        }

        public int DamageTotal
        {
            get { return Process.ReadInt(Address + (int)Offsets.DamageTotal); }
        }

        public int SpellID
        {
            get { return Process.ReadInt(Address + (int)Offsets.spellID); }
        }

        public int ModeDamage
        {
            get { return Process.ReadInt(Address + (int)Offsets.ModeDamage); }
            set { Process.Write(value, Address + (int)Offsets.ModeDamage); }
        }

        public int ModeWeapon
        {
            get { return Process.ReadInt(Address + (int)Offsets.ModeWeapon); }
            set { Process.Write(value, Address + (int)Offsets.ModeWeapon); }
        }


        public zVec3 LocationHit
        {
            get { return new zVec3(Process, Address + (int)Offsets.LocationHit); }
        }

        public zVec3 DirectionFly
        {
            get { return new zVec3(Process, Address + (int)Offsets.DirectionFly); }
        }

        #endregion
    }
}
