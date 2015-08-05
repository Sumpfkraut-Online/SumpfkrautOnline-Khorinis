using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public struct NPCAttributes
    {
        public const int Health = 0;
        public const int Health_Max = 1;
        public const int Mana = 2;
        public const int Mana_Max = 3;
        public const int Strength = 4;
        public const int Dexterity = 5;
        public const int Capacity = 6;
        public const int MAX_ATTRIBUTES = 7;
    }

    public struct NPCTalents
    {
        public const int Breathing = 0;
        public const int MAX_TALENTS = 1;
    }

    public enum NPCWeaponState : byte
    {
        None,
        Fists,
        Melee,
        Ranged,
        Magic
    }
}
