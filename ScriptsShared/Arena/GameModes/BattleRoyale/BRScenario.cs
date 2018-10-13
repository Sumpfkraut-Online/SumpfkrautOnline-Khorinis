using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;

namespace GUC.Scripts.Arena.GameModes.BattleRoyale
{
    partial class BRScenario : GameScenario
    {
        public const float VobSpotItemProb = 0.05f;
        public const float VobSpotNPCProb = 0.1f;

        public override GameMode GetMode() { return BRMode.Instance; }

        #region Starter Class
        public static NPCClass StartClass = new NPCClass()
        {
            Name = "BRStarter",
            Definition = null,
            Damage = 0,
            Protection = 0,
            HP = 0,
            Overlays = null,
            ItemDefs = new NPCClass.InvItem[]
            {
                new NPCClass.InvItem("ItMw_1h_Bau_Mace"),
            },
        };
        #endregion

        #region NPCs

        public static BRRandomizer<NPCClass> NPCs = new BRBucketCollector<NPCClass>()
        {
            {
                new NPCClass()
                {
                    Name = "Lurker",
                    Definition = "Lurker",
                    Protection = 30,
                    Damage = 40,
                    HP = 100,
                },
                0.1f
            },
            {
                new NPCClass()
                {
                    Name = "Bloodfly",
                    Definition = "Bloodfly",
                    Protection = 20,
                    Damage = 20,
                    HP = 40,
                },
                0.2f
            },
            {
                new NPCClass()
                {
                    Name = "Dragonsnapper",
                    Definition = "Dragonsnapper",
                    Protection = 50,
                    Damage = 60,
                    HP = 500,
                },
                0.01f
            },
            {
                new NPCClass()
                {
                    Name = "Rat",
                    Definition = "Rat",
                    Protection = 20,
                    Damage = 20,
                    HP = 60,
                },
                0.15f
            },
            {
                new NPCClass()
                {
                    Name = "Scavenger",
                    Definition = "Scavenger",
                    Protection = 20,
                    Damage = 30,
                    HP = 60,
                },
                0.6f
            },
        };

        #endregion

        #region Items

        #region helper classes

        class BRItemCollector : BRBucketCollector<ItemBucket>
        {
            public void Add(string def, double prob, int amount = 1)
            {
                Add(new ItemBucket()
                {
                    Definition = def,
                    Amount = amount,
                }, prob);
            }
        }

        public struct ItemBucket
        {
            public string Definition;
            public int Amount;
        }

        #endregion

        public static BRRandomizer<ItemBucket> Items = new BRItemCollector()
        {
            { "", 0.1 }, // nothing

            { "itrw_arrow", 0.3, 10 }, // pfeile
            { "itrw_bolt", 0.3, 10 }, // bolzen

            // WAFFEN
            { "ItMw_1h_Bau_Mace", 0.5 }, // schwerer ast
            { "grobes_schwert", 0.4 },
            { "1haxt", 0.4 },
            { "1hschwert", 0.3 },
            { "krush_pach", 0.2 },
            { "leichter_zweihaender", 0.2 },
            { "grober_2h", 0.1 },
            { "orc_sword", 0.05 },
            { "2hschwert", 0.01 }, // zweihänder
            { "2haxt", 0.01 }, // 2h axt

            // RANGED
            { "itrw_shortbow", 0.2 },
            { "light_xbow", 0.2 },
            { "itrw_longbow", 0.1 },
            { "heavy_xbow", 0.1 },

            // RÜSTUNG
            { "ITAR_prisoner", 0.3 },
            { "ITAR_bandit", 0.2 },
            { "ITAR_bandit_m", 0.1 },
            { "ITAR_schatten", 0.1 },
            { "ITAR_garde_l", 0.05 },
            { "ITAR_garde", 0.02 },
            { "ITAR_söldner", 0.01 },
            { "ITAR_miliz_s", 0.01 },
            { "ITAR_templer", 0.005 },
            { "ITAR_ritter", 0.001 },
            { "ITAR_pal_skel", 0.0001 },

            // ITEMS
            { "hptrank", 0.1 },
        };

        #endregion

        public PosAng[] Spawnpoints;
        public float BarrierStartScale;
        public float BarrierEndScale;

        public static void Init()
        {
            #region Minental

            scenarios.Add(new BRScenario()
            {
                Name = "br_minental",
                WorldPath = "BR_MINENTAL.ZEN",
                WarmUpDuration = 60 * TimeSpan.TicksPerSecond,
                FightDuration = 15 * TimeSpan.TicksPerMinute,
                SpecPoint = new PosAng(-2442.949f, 676.9498f, 412.3001f, -0.2303832f, -1.818634f, 0f),
                Spawnpoints = new PosAng[]
                {
                    new PosAng(5587, 5461, 36548, 0, 2.89f, 0), // erzaustausch, G1 start
                },
                BarrierStartScale = 1,
                BarrierEndScale = 0.2f,
            });

            #endregion
        }
    }
}
