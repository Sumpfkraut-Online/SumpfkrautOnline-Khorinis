using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Utilities;
using GUC.Types;

namespace GUC.Scripts.Arena
{
    class HordeItem
    {
        public string ItemDef;
        public Vec3f Position;
        public Angles Angles;

        public HordeItem(string itemDef, float x, float y, float z) : this(itemDef, new Vec3f(x, y, z), default(Angles))
        {
        }

        public HordeItem(string itemDef, Vec3f position, Angles angles = default(Angles))
        {
            this.ItemDef = itemDef;
            this.Position = position;
            this.Angles = angles;
        }
    }

    class HordeEnemy
    {
        public string NPCDef;
        public string WeaponDef;
        public string ArmorDef;
        public int Protection;
        public int Damage;
        public int Health;

        public HordeEnemy(string npcDef, string wep = null, string armor = null, int prot = 0, int dam = 0, int hp = 100)
        {
            this.NPCDef = npcDef;
            this.WeaponDef = wep;
            this.ArmorDef = armor;
            this.Protection = prot;
            this.Damage = dam;
            this.Health = hp;
        }
    }

    struct HordePair
    {
        public HordeEnemy Enemy;
        public float CountScale;

        public HordePair(HordeEnemy enemy, float countScale)
        {
            this.Enemy = enemy;
            this.CountScale = countScale;
        }
    }

    class HordeGroup
    {
        public HordePair[] npcs;
        public Vec3f Position;
        public float Range;

        public HordeGroup(Vec3f position, float range, HordePair[] enemies)
        {
            this.Position = position;
            this.Range = range;
            this.npcs = enemies;
        }

        public HordeGroup(float x, float y, float z, float range, params HordePair[] enemies) : this(new Vec3f(x, y, z), range, enemies)
        {
        }
    }

    class HordeBarrier
    {
        public Vec3f Position;
        public Angles Angles;
        public string Definition;
        public bool AddAfterEvent;

        public HordeBarrier(string def, Vec3f pos, Angles ang, bool add = false)
        {
            this.Position = pos;
            this.Angles = ang;
            this.Definition = def;
            this.AddAfterEvent = add;
        }
    }

    class HordeStand
    {
        public HordeBarrier[] Barriers;

        public Vec3f Position;
        public float Range;

        public int Duration; // sec

        public float MaxEnemies;
        public HordePair[] Enemies; // enemy + probability
        public Vec3f[] EnemySpawns;
    }

    struct HordeZone
    {
        public Vec3f Position;
        public float Range;

        public HordeZone(Vec3f pos, float range)
        {
            this.Position = pos;
            this.Range = range;
        }

        public HordeZone(float x, float y, float z, float range) : this(new Vec3f(x,y,z), range)
        {
        }
    }
}
