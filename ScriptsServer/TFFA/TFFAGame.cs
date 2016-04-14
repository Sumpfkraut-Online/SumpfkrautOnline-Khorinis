using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.TFFA;
using GUC.Network;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Enumeration;

namespace GUC.Server.Scripts.TFFA
{
    public static class TFFAGame
    {
        const long FightTime = 1 * TimeSpan.TicksPerMinute;
        const int KillsToWin = 30;

        static List<Tuple<Vec3f, Vec3f>> NLSpawns = new List<Tuple<Vec3f, Vec3f>>()
        {
            new Tuple<Vec3f, Vec3f>(new Vec3f(3728, 645, 282), new Vec3f(-0.245f, 0, -0.95f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(4084, 645, 197), new Vec3f(-0.945f, 0, -0.324f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(3987, 645, -174), new Vec3f(-1.0f, 0, 0.14f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(3635, 645, -1442), new Vec3f(-0.923f, 0, 0.384f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(3643, 645, -2446), new Vec3f(-0.437f, 0, 0.899f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(2453, 645, -2247), new Vec3f(0.255f, 0, 0.966f)),
        };

        static List<Tuple<Vec3f, Vec3f>> ALSpawns = new List<Tuple<Vec3f, Vec3f>>()
        {
            new Tuple<Vec3f, Vec3f>(new Vec3f(-3643, -268, -2608), new Vec3f(-0.297f, 0, 0.955f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-4090, -292, -2746), new Vec3f(0.267f, 0, 0.963f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-4983, -315, -3184), new Vec3f(0.646f, 0, 0.763f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-4603, -292, -2325), new Vec3f(0.462f, 0, 0.887f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-4732, -315, -3286), new Vec3f(-0.134f, 0, 0.991f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-3609, -232, -2306), new Vec3f(-0.996f, 0, -0.082f)),
        };

        static Dictionary<Team, List<TFFAClient>> Teams = new Dictionary<Team, List<TFFAClient>>()
        {
            { Team.Spec, new List<TFFAClient>() },
            { Team.AL, new List<TFFAClient>() },
            { Team.NL, new List<TFFAClient>() }
        };

        public static int GetCount(Team team)
        {
            return Teams[team].Count;
        }

        public static void AddToTeam(TFFAClient client, Team team)
        {
            if (client.Team != team && client.Character != null)
            {
                client.Character.SetHealth(0);
                if (status == TFFAPhase.Warmup)
                    SpawnNewPlayer(client);
            }
            RemoveFromTeam(client);
            Teams[team].Add(client);
            client.Team = team;

            if (team == Team.Spec)
                client.BaseClient.SetToSpectate(WorldInst.Current.BaseWorld, new Vec3f(0, 300, 0), new Vec3f());
        }

        public static void RemoveFromTeam(TFFAClient client)
        {
            Teams[client.Team].Remove(client);
        }

        public static void SelectClass(TFFAClient client, PlayerClass c)
        {
            if (client.Class != c && client.Character != null)
            {
                client.Character.SetHealth(0);
                if (status == TFFAPhase.Warmup)
                    SpawnNewPlayer(client);
            }
            client.Class = c;
        }

        public static void UpdateStats()
        {
            // TEAM MENU STATS
            if (teamMenuClients.Count > 0)
            {
                var stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.OpenTeamMenu);
                stream.Write((byte)GetCount(Team.Spec));
                stream.Write((byte)GetCount(Team.AL));
                stream.Write((byte)GetCount(Team.NL));
                for (int i = 0; i < teamMenuClients.Count; i++)
                    teamMenuClients[i].BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.UNRELIABLE);
            }

            // CLASS MENU STATS
            if (classMenuClients.Count > 0)
            {
                var stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.OpenClassMenu);
                int lights = 0;
                int heavies = 0;
                var list = Teams[Team.AL];
                for (int i = 0; i < list.Count; i++)
                    if (list[i].Class == PlayerClass.Light) lights++;
                    else if (list[i].Class == PlayerClass.Heavy) heavies++;
                stream.Write((byte)lights);
                stream.Write((byte)heavies);
                for (int i = 0; i < classMenuClients.Count; i++)
                    if (classMenuClients[i].Team == Team.AL)
                        classMenuClients[i].BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.UNRELIABLE);

                stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.OpenClassMenu);
                lights = 0;
                heavies = 0;
                list = Teams[Team.NL];
                for (int i = 0; i < list.Count; i++)
                    if (list[i].Class == PlayerClass.Light) lights++;
                    else if (list[i].Class == PlayerClass.Heavy) heavies++;
                stream.Write((byte)lights);
                stream.Write((byte)heavies);
                for (int i = 0; i < classMenuClients.Count; i++)
                    if (classMenuClients[i].Team == Team.NL)
                        classMenuClients[i].BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.UNRELIABLE);
            }

            if (scoreboardClients.Count > 0)
            {
                var stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.OpenScoreboard);
                stream.Write((int)(gameTimer.GetRemainingTicks() / TimeSpan.TicksPerSecond));

                var list = Teams[Team.AL];
                stream.Write((byte)list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    stream.Write(list[i].Name);
                    stream.Write((byte)list[i].Kills);
                    stream.Write((byte)list[i].Deaths);
                    stream.Write((ushort)list[i].Damage);
                }

                list = Teams[Team.NL];
                stream.Write((byte)list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    stream.Write(list[i].Name);
                    stream.Write((byte)list[i].Kills);
                    stream.Write((byte)list[i].Deaths);
                    stream.Write((ushort)list[i].Damage);
                }


                for (int i = 0; i < scoreboardClients.Count; i++)
                    scoreboardClients[i].BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.UNRELIABLE);
            }
        }

        static GUCTimer gameTimer;
        static GUCTimer statUpdateTimer;
        static TFFAGame()
        {
            NPCInst.sOnHit += OnHit;

            statUpdateTimer = new GUCTimer(10000000, UpdateStats);
            statUpdateTimer.Start();

            respawnTimer = new GUCTimer(10 * TimeSpan.TicksPerSecond, RespawnPlayers);
            respawnTimer.Start();


            gameTimer = new GUCTimer();
            PhaseFight();
        }

        static int ALKills = 0;
        static int NLKills = 0;

        static void AddKill(TFFAClient client)
        {
            client.Kills++;
            if (client.Team == Team.AL)
            {
                ALKills++;
            }
            else if (client.Team == Team.NL)
            {
                NLKills++;
            }


            if (ALKills >= KillsToWin || NLKills >= KillsToWin)
                PhaseEnd();
        }

        static void OnHit(NPCInst attacker, NPCInst target, int damage)
        {
            if (attacker.BaseInst.Client == null || target.BaseInst.Client == null)
                return;

            ((TFFAClient)attacker.BaseInst.Client.ScriptObject).Damage += damage;
            if (target.BaseInst.HP <= 0)
            {
                //Log.Logger.Log()
                ((TFFAClient)target.BaseInst.Client.ScriptObject).Deaths++;
                AddKill((TFFAClient)attacker.BaseInst.Client.ScriptObject);
            }
        }

        static TFFAPhase status;
        public static TFFAPhase Status { get { return status; } }

        static void PhaseWait()
        {
            Log.Logger.Log("Wait Phase");
            status = TFFAPhase.Wait;
            TFFAClient.ForEach(client =>
            {
                client.Deaths = 0;
                client.Kills = 0;
                client.Damage = 0;
                ALKills = 0;
                NLKills = 0;
            });
            RemoveAllPlayers();

            gameTimer.SetInterval(20 * TimeSpan.TicksPerSecond);
            gameTimer.SetCallback(PhaseWarmup);
            gameTimer.Restart();
        }

        static void RemoveAllPlayers()
        {
            WorldInst.Current.BaseWorld.ForEachVob(v =>
            {
                if (v is GUC.WorldObjects.NPC && !((GUC.WorldObjects.NPC)v).IsPlayer)
                    v.Despawn();
            });
        }

        static void PhaseWarmup()
        {
            Log.Logger.Log("Warmup Phase");
            status = TFFAPhase.Warmup;
            TFFAClient.ForEach(client =>
            {
                SpawnNewPlayer(client);
                client.Deaths = 0;
                client.Kills = 0;
                client.Damage = 0;
                ALKills = 0;
                NLKills = 0;
            });
            RemoveAllPlayers();

            gameTimer.SetInterval(10 * TimeSpan.TicksPerSecond);
            gameTimer.SetCallback(PhaseFight);
            gameTimer.Restart();
        }

        static void PhaseFight()
        {
            Log.Logger.Log("Fight Phase");
            status = TFFAPhase.Fight;
            TFFAClient.ForEach(client =>
            {
                SpawnNewPlayer(client);
                client.Deaths = 0;
                client.Kills = 0;
                client.Damage = 0;
                ALKills = 0;
                NLKills = 0;
            });
            RemoveAllPlayers();

            gameTimer.SetInterval(FightTime);
            gameTimer.SetCallback(PhaseEnd);
            gameTimer.Restart();
        }

        static void PhaseEnd()
        {
            Log.Logger.Log("End Phase");
            status = TFFAPhase.End;

            if (ALKills > NLKills)
            {
                Teams[Team.NL].ForEach(client => client.Character.SetHealth(0));
                var stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.WinMsg);
                stream.Write((byte)Team.AL);
                TFFAClient.ForEach(client => client.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE));
                Log.Logger.Log("TEAM GOMEZ HAT GEWONNEN");
            }
            else if (NLKills > ALKills)
            {
                Teams[Team.AL].ForEach(client => client.Character.SetHealth(0));
                var stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.WinMsg);
                stream.Write((byte)Team.NL);
                TFFAClient.ForEach(client => client.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE));
                Log.Logger.Log("TETRIANDOCH HAT GEWONNEN");
            }

            gameTimer.SetInterval(10 * TimeSpan.TicksPerSecond);
            gameTimer.SetCallback(PhaseWait);
            gameTimer.Restart();
        }

        public static List<TFFAClient> teamMenuClients = new List<TFFAClient>();
        public static List<TFFAClient> classMenuClients = new List<TFFAClient>();
        public static List<TFFAClient> scoreboardClients = new List<TFFAClient>();

        #region Respawn

        static GUCTimer respawnTimer;

        static Random rand = new Random();
        static void RespawnPlayers()
        {
            if (status != TFFAPhase.Fight)
                return;

            TFFAClient.ForEach(client =>
            {
                if (client.Character == null || client.Character.BaseInst.HP <= 0)
                    SpawnNewPlayer(client);
            });
        }


        static int BodyTex = 0;
        static int HeadMesh = 0;
        static void SpawnNewPlayer(TFFAClient client)
        {
            if (client.Team == Team.Spec || client.Class == PlayerClass.None)
                return;

            if (client.Character != null && client.Character.BaseInst.HP > 0)
                client.Character.SetHealth(0);

            var def = BaseVobDef.Get<NPCDef>("player");
            NPCInst npc = new NPCInst(def);

            npc.CustomBodyTex = (HumBodyTexs)BodyTex++;
            if (BodyTex == 4) BodyTex = 8;
            if (BodyTex == 11) BodyTex = 0;

            npc.CustomHeadMesh = (HumHeadMeshs)HeadMesh++;
            if (HeadMesh == 6) HeadMesh = 0;

            if (npc.CustomBodyTex == HumBodyTexs.M_Black)
                npc.CustomHeadTex = (HumHeadTexs)rand.Next(129, 137);
            else if (npc.CustomBodyTex == HumBodyTexs.M_Latino)
                npc.CustomHeadTex = (HumHeadTexs)rand.Next(120, 129);
            else if (npc.CustomBodyTex == HumBodyTexs.M_Pale)
                npc.CustomHeadTex = (HumHeadTexs)rand.Next(41, 58);
            else
                npc.CustomHeadTex = (HumHeadTexs)rand.Next(58, 120);

            npc.CustomVoice = (HumVoices)rand.Next(1, 15);
            npc.Fatness = rand.Next(-100, 250) / 100.0f;

            npc.ModelScale = new Vec3f(rand.Next(95, 105) / 100.0f, rand.Next(95, 105) / 100.0f, rand.Next(95, 105) / 100.0f);

            npc.CustomName = client.Name;

            npc.UseCustoms = true;

            ItemInst weapon;
            ItemInst armor;
            ScriptOverlay overlay;
            Tuple<Vec3f, Vec3f> spawnPoint;

            if (client.Team == Team.AL)
            {
                if (client.Class == PlayerClass.Heavy)
                {
                    weapon = new ItemInst(ItemDef.Get<ItemDef>("2hschwert"));
                    armor = new ItemInst(ItemDef.Get<ItemDef>("itar_Garde"));
                    def.Model.TryGetOverlay("2HST2", out overlay);
                }
                else
                {
                    weapon = new ItemInst(ItemDef.Get<ItemDef>("1hschwert"));
                    armor = new ItemInst(ItemDef.Get<ItemDef>("itar_schatten"));
                    def.Model.TryGetOverlay("1HST2", out overlay);
                }
                spawnPoint = ALSpawns[rand.Next(0, 6)];
            }
            else
            {
                if (client.Class == PlayerClass.Heavy)
                {
                    weapon = new ItemInst(ItemDef.Get<ItemDef>("2haxt"));
                    armor = new ItemInst(ItemDef.Get<ItemDef>("itar_söldner"));
                    def.Model.TryGetOverlay("2HST1", out overlay);
                }
                else
                {
                    weapon = new ItemInst(ItemDef.Get<ItemDef>("1haxt"));
                    armor = new ItemInst(ItemDef.Get<ItemDef>("itar_bandit"));
                    def.Model.TryGetOverlay("1HST1", out overlay);
                }
                spawnPoint = NLSpawns[rand.Next(0, 6)];
            }

            npc.AddItem(weapon);
            npc.EquipItem(1, weapon); // 1 = DrawnWeapon

            npc.AddItem(armor);
            npc.EquipItem(0, armor);

            if (overlay != null)
            {
                npc.ApplyOverlay(overlay);
            }


            npc.Spawn(WorldInst.Current, spawnPoint.Item1, spawnPoint.Item2);
            client.SetControl(npc);
        }

        #endregion
    }
}
