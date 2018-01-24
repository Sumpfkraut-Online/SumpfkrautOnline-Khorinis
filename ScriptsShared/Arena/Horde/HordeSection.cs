using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Utilities;
using GUC.Types;

namespace GUC.Scripts.Arena
{
    class HordeBarrier
    {
        public Vec3f Position;
        public Angles Angles;
        public string Definition;
    }

    class HordeEnemy
    {
        public string NPCDef;
        public float CountScale;
        public string WeaponDef;
        public string ArmorDef;
        public int Protection;
        public int Damage;
        public int Health;

        public HordeEnemy(string npcDef, float countScale, string wep = null, string armor = null, int prot = 0, int dam = 0, int hp = 100)
        {
            this.NPCDef = npcDef;
            this.CountScale = countScale;
            this.WeaponDef = wep;
            this.ArmorDef = armor;
            this.Protection = prot;
            this.Damage = dam;
            this.Health = hp;
        }
    }

    class HordeGroup
    {
        public List<HordeEnemy> npcs;
        public Vec3f Position;
        public float Range;
    }

    class HordeSection
    {
        public List<HordeBarrier> barriers;
        public List<HordeBarrier> bridges;
        public List<HordeGroup> groups;

        public HordeSection Next;
        public Vec3f SpawnPos;
        public float SpawnRange;

        public int SecsTillNext = 30;

        public Vec3f SpecPos;
        public Angles SpecAng;

        public string FinishedMessage;
    }
}
