using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum NPCState : byte
    {
        Stand,

        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,

        AttackForward,
        AttackLeft,
        AttackRight,
        AttackRun,
        Parry,
        DodgeBack,
    }

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

    internal struct NPCAppearanceFlags
    {
        public const short Name = 1;
        public const short Visual = 2;
        public const short BodyMesh = 4;
        public const short BodyTex = 8;
        public const short HeadMesh = 16;
        public const short HeadTex = 32;
        public const short BodyHeight = 64;
        public const short BodyWidth = 128;
        public const short Fatness = 256;
        public const short Voice = 512;
        public const short HumanHead = 1024;
    }
}
