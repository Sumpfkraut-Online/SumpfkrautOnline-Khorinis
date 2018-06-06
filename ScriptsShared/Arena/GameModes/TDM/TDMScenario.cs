using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    class TDMScenario : GameScenario
    { 
        public override GameMode GetMode()
        {
            return TDMMode.Instance;
        }

        public class TeamDef
        {
            public string Name;
            public ColorRGBA Color;
            public PosAng[] SpawnPoints;
            public NPCClass[] Classes;
        }

        public TeamDef[] Teams;

        public static void Init()
        {
            #region TDM_BURG

            scenarios.Add(new TDMScenario()
            {
                Name = "tdm_burg",
                WorldPath = "G1-OLDCAMP.ZEN",
                FightDuration = 10 * TimeSpan.TicksPerMinute,
                MaxDepth = -400,
                SpecPoint = new PosAng(-2442.949f, 676.9498f, 412.3001f, -0.2303832f, -1.818634f, 0f),
                Teams = new TeamDef[]
                {
                    // ALTES LAGER
                    new TeamDef()
                    {
                        Name = "Team Gomez",
                        Color = ColorRGBA.Red,
                        Classes = new NPCClass[]
                        {
                            new NPCClass()
                            {
                                Name = "Gardist",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"2HST1"},
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("2hschwert"),
                                    new NPCClass.InvItem("itar_garde"),
                                    new NPCClass.InvItem("light_xbow"),
                                    new NPCClass.InvItem("itrw_bolt", 20),
                                },
                            },
                            new NPCClass()
                            {
                                Name = "Schatten",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"1HST1"},
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("1hschwert"),
                                    new NPCClass.InvItem("itar_schatten"),
                                    new NPCClass.InvItem("itrw_longbow"),
                                    new NPCClass.InvItem("itrw_arrow", 50),
                                }
                            },
                        },
                        SpawnPoints = new PosAng[]
                        {
                            new PosAng(-4017.539f, -289.0983f, -2818.167f, 0f, 0.02443461f, 0f),
                            new PosAng(-3741.954f, -282.7443f, -2828.874f, 0f, 0.09773839f, 0f),
                            new PosAng(-3501.845f, -281.3712f, -2854.833f, 0f, 0.2164207f, 0f),
                            new PosAng(-4204.289f, -302.1991f, -2865.264f, 0f, -0.1675519f, 0f),
                            new PosAng(-4414.396f, -308.5389f, -2741.288f, 0f, -0.4572764f, 0f),
                            new PosAng(-4682.539f, -294.4325f, -2370.788f, 0f, -0.677188f, 0f),
                        }
                    },

                    // NEUES LAGER
                    new TeamDef()
                    {
                        Name = "Tetriandoch",
                        Color = ColorRGBA.Blue,
                        Classes = new NPCClass[]
                        {
                            new NPCClass()
                            {
                                Name = "Söldner",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"2HST1"},
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("2haxt"),
                                    new NPCClass.InvItem("itar_söldner"),
                                    new NPCClass.InvItem("light_xbow"),
                                    new NPCClass.InvItem("itrw_bolt", 20),
                                },
                            },
                            new NPCClass()
                            {
                                Name = "Bandit",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"1HST1"},
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("1haxt"),
                                    new NPCClass.InvItem("itar_bandit"),
                                    new NPCClass.InvItem("itrw_longbow"),
                                    new NPCClass.InvItem("itrw_arrow", 50),
                                }
                            },
                        },
                        SpawnPoints = new PosAng[]
                        {
                            new PosAng(3326.211f, 248.1775f, 1184.516f, 0f, 2.523746f,  0f),
                            new PosAng(2996.751f, 248.1097f, 1348.412f, 0f, 2.9147f,    0f),
                            new PosAng(3485.681f, 368.1476f,  963.361f, 0f, 1.912882f,  0f),
                            new PosAng(2278.622f, 248.1326f, 1518.721f, 0f, -2.911208f, 0f),
                            new PosAng(2036.856f, 248.1774f, 1372.729f, 0f, -2.565633f, 0f),
                            new PosAng(1895.824f, 248.1548f, 864.6687f, 0f, -1.731365f, 0f),
                        }
                    }
                }
            });

            #endregion
            
            #region TDM_MINE

            scenarios.Add(new TDMScenario()
            {
                Name = "tdm_mine",
                WorldPath = "G1-OLDMINE.ZEN",
                FightDuration = 10 * TimeSpan.TicksPerMinute,
                MaxHeight = -6800,
                MaxWorldDistance = 8635,
                SpecPoint = new PosAng(1478.838f, -9251.064f, -6242.642f, 0.1151917f, -1.291543f, 0f),
                WorldTimeScale = 0,
                WorldBarrier = 0,
                WorldWeather = 0,
                Teams = new TeamDef[]
                {
                    // ALTES LAGER
                    new TeamDef()
                    {
                        Name = "Buddleraufsicht",
                        Color = ColorRGBA.Red,
                        Classes = new NPCClass[]
                        {
                            new NPCClass()
                            {
                                Name = "Gardist",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"1HST1"},
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("grobes_schwert"),
                                    new NPCClass.InvItem("itar_garde_l"),
                                },
                            },
                            new NPCClass()
                            {
                                Name = "Templer",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"2HST1"},
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("leichter_zweihaender"),
                                    new NPCClass.InvItem("itar_templer"),
                                }
                            },
                        },
                        SpawnPoints = new PosAng[]
                        {
                            new PosAng(1647.881f, -7898.187f, -2613.269f, 0f, 0f, 0f),
                            new PosAng(1842.845f, -7897.371f, -2325.282f, 0f, 1.37532f, 0f),
                            new PosAng(3441.214f, -7475.371f, -2124.406f, 0f, 1.43466f, 0f),
                            new PosAng(2990.72f, -7441.649f, -2360.442f, 0f, 0.8063417f, 0f),
                            new PosAng(2485.762f, -7425.826f, -2425.143f, 0f, -0.2303839f, 0f),
                            new PosAng(1916.954f, -7273.945f, -2269.227f, 0f, -1.528909f, 0f),
                        }
                    },

                    // MINECRAWLER
                    new TeamDef()
                    {
                        Name = "Schachtcrew",
                        Color = ColorRGBA.Orange,
                        Classes = new NPCClass[]
                        {
                            new NPCClass()
                            {
                                Name = "Klauen-Minecrawler",
                                Definition = "minecrawler",
                                Protection = 35,
                                Damage = 50,
                                HP = 100,
                            },
                            new NPCClass()
                            {
                                Name = "Panzer-Minecrawler",
                                Definition = "minecrawler_warrior",
                                Protection = 40,
                                Damage = 45,
                                HP = 100,
                            },
                        },
                        SpawnPoints = new PosAng[]
                        {
                            new PosAng(4240.823f, -8092.504f, 5476.157f, -0.1134214f, -1.923348f, 0f),
                            new PosAng(4326.87f, -8130.521f, 4848.703f, -0.1358788f, -1.483522f, 0),
                            new PosAng(-527.3607f, -8292.744f, 5760.481f, 0.002349615f, -2.275903f, 0),
                            new PosAng(-271.3119f, -8320.184f, 5945.153f, 0.03162932f, -2.600533f, 0),
                            new PosAng(5399.01f, -8575.909f, -5303.596f, -0.0418613f, 1.413725f, 0),
                            new PosAng(5369.831f, -8570.879f, -5632.852f, -0.0684464f, 1.253157f, 0),
                        }
                    }
                }
            });

            #endregion

            #region TDM_PASS

            scenarios.Add(new TDMScenario()
            {
                Name = "tdm_pass",
                WorldPath = "G2-PASS.ZEN",
                FightDuration = 10 * TimeSpan.TicksPerMinute,
                MaxHeight = 950,
                MaxWorldDistance = 10650,
                SpecPoint = new PosAng(3725.298f, -735.0935f, 2743.517f, -0.06981301f, 0.9250249f, 0f),
                WorldTime = new WorldTime(0, 20),
                WorldTimeScale = 0,
                WorldBarrier = 0,
                WorldWeather = 1,
                Teams = new TeamDef[]
                {
                    // PALADINE
                    new TeamDef()
                    {
                        Name = "Paladintrupp'",
                        Color = ColorRGBA.Red,
                        Classes = new NPCClass[]
                        {
                            new NPCClass()
                            {
                                Name = "Ritter",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"2HST2" , "XbowT1" },
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("2hschwert"),
                                    new NPCClass.InvItem("itar_ritter"),
                                    new NPCClass.InvItem("light_xbow"),
                                    new NPCClass.InvItem("itrw_bolt", 20),
                                },
                            },
                            new NPCClass()
                            {
                                Name = "Milizsoldat",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"1HST2", "XbowT2" },
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("1hschwert"),
                                    new NPCClass.InvItem("itar_miliz_s"),
                                    new NPCClass.InvItem("heavy_xbow"),
                                    new NPCClass.InvItem("itrw_bolt", 50),
                                }
                            },
                        },
                        SpawnPoints = new PosAng[]
                        {
                            new PosAng(3069.88f, 148.2805f, 9595.466f, 0f, 2.01411f, 0f),
                            new PosAng(3278.937f, 196.152f, 9662.452f, 0f, 2.031563f, 0f),
                            new PosAng(2932.983f, 10.72608f, 9838.522f, 0f, 2.432988f, 0f),
                            new PosAng(2686.674f, -7.369984f, 9910.67f, 0f, 2.44346f, 0f),
                            new PosAng(2493.167f, 168.4053f, 10023.55f, 0f, 2.455878f, 0f),
                        }
                    },

                    // NEUES LAGER
                    new TeamDef()
                    {
                        Name = "Grünfellwacht",
                        Color = ColorRGBA.Blue,
                        Classes = new NPCClass[]
                        {
                            new NPCClass()
                            {
                                Name = "Ork-Elite",
                                Definition = "orc_elite",
                                Protection = 40,
                                Damage = 0,
                                HP = 100,
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("orc_sword"),
                                    new NPCClass.InvItem("light_xbow"),
                                    new NPCClass.InvItem("itrw_bolt", 20),
                                },
                            },
                            new NPCClass()
                            {
                                Name = "Ork-Krieger",
                                Definition = "orc_warrior",
                                Protection = 35,
                                Damage = 0,
                                HP = 100,
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("krush_pach"),
                                    new NPCClass.InvItem("heavy_xbow"),
                                    new NPCClass.InvItem("itrw_bolt", 50),
                                }
                            },
                        },
                        SpawnPoints = new PosAng[]
                        {
                            new PosAng(-1181.993f, -437.5759f, -2589.357f, 0f, 0.5689777f, 0f),
                            new PosAng(-1033.761f, -438.4826f, -2530.26f, 0f, 0.4817112f, 0f),
                            new PosAng(-766.6343f, -425.4779f, -2489.346f, 0f, 0.6946411f, 0f),
                            new PosAng(-628.7674f, -355.7959f, -2276.756f, 0f, 0.959931f, 0f),
                            new PosAng(-558.9549f, -192.2555f, -2474.483f, 0f, 0.9305235f, 0f),
                        }
                    }
                }
            });

            #endregion

            #region TDM_TEMPLE

            scenarios.Add(new TDMScenario()
            {
                Name = "tdm_tempel",
                WorldPath = "ADDON-TEMPLE.ZEN",
                FightDuration = 10 * TimeSpan.TicksPerMinute,
                MaxDepth = -2100,
                MaxHeight = 400,
                MaxWorldDistance = 7400,
                SpecPoint = new PosAng(2012.767f, -195.6564f, 4388.974f, -0.153589f, 2.816961f, 0f),
                WorldTime = new WorldTime(0, 12),
                WorldTimeScale = 0,
                WorldBarrier = 0,
                Teams = new TeamDef[]
                {
                    // BANDITEN
                    new TeamDef()
                    {
                        Name = "Ravens Späher",
                        Color = ColorRGBA.White,
                        Classes = new NPCClass[]
                        {
                            new NPCClass()
                            {
                                Name = "Grober Bandit",
                                Definition = null,
                                Protection = 5,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"2HST1" },
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("grober_2h"),
                                    new NPCClass.InvItem("itar_bandit"),
                                    new NPCClass.InvItem("light_xbow"),
                                    new NPCClass.InvItem("itrw_bolt", 20),
                                },
                            },
                            new NPCClass()
                            {
                                Name = "Leichter Bandit",
                                Definition = null,
                                Protection = 0,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"1HST1" },
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("grobes_schwert"),
                                    new NPCClass.InvItem("itar_bandit_m"),
                                    new NPCClass.InvItem("itrw_longbow"),
                                    new NPCClass.InvItem("itrw_arrow", 50),
                                }
                            },
                        },
                        SpawnPoints = new PosAng[]
                        {
                            new PosAng(2036.342f, -332.4806f, 6549.103f, 0f, 2.813471f, 0f),
                            new PosAng(1911.383f, -309.5511f, 6635.414f, 0f, 2.771583f, 0f),
                            new PosAng(1714.081f, -261.7822f, 6643.623f, 0f, 2.960078f, 0f),
                            new PosAng(1898.435f, -343.3698f, 6514.929f, 0f, 2.712242f, 0f),
                            new PosAng(2125.315f, -352.5587f, 6391.019f, 0f, 2.286381f, 0f),
                        }
                    },

                    // NEUES LAGER
                    new TeamDef()
                    {
                        Name = "Grabwächter",
                        Color = ColorRGBA.Blue,
                        Classes = new NPCClass[]
                        {
                            new NPCClass()
                            {
                                Name = "Schattenlord",
                                Definition = "skeleton_lord",
                                Protection = 40,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"2HST1" },
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("2hschwert"),
                                    new NPCClass.InvItem("itar_pal_skel"),
                                    new NPCClass.InvItem("light_xbow"),
                                    new NPCClass.InvItem("itrw_bolt", 20),
                                },
                            },
                            new NPCClass()
                            {
                                Name = "Skelettkrieger",
                                Definition = null,
                                Protection = 35,
                                Damage = 0,
                                HP = 100,
                                Overlays = new string[] {"1HST1" },
                                ItemDefs = new NPCClass.InvItem[]
                                {
                                    new NPCClass.InvItem("grobes_schwert"),
                                    new NPCClass.InvItem("itrw_longbow"),
                                    new NPCClass.InvItem("itrw_arrow", 50),
                                }
                            },
                        },
                        SpawnPoints = new PosAng[]
                        {
                            new PosAng(-1181.993f, -437.5759f, -2589.357f, 0f, 0.5689777f, 0f),
                            new PosAng(-1033.761f, -438.4826f, -2530.26f, 0f, 0.4817112f, 0f),
                            new PosAng(-766.6343f, -425.4779f, -2489.346f, 0f, 0.6946411f, 0f),
                            new PosAng(-628.7674f, -355.7959f, -2276.756f, 0f, 0.959931f, 0f),
                            new PosAng(-558.9549f, -192.2555f, -2474.483f, 0f, 0.9305235f, 0f),
                        }
                    }
                }
            });

            #endregion

        }
    }
}
