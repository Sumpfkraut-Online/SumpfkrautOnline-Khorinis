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
        const long ScoreboardUpdateTime = 1 * TimeSpan.TicksPerSecond;

        const long RespawnTime = 10 * TimeSpan.TicksPerSecond;

        const long FightTime = 6 * TimeSpan.TicksPerMinute;
        const long WaitTime = 15 * TimeSpan.TicksPerSecond;
        const long EndTime = 10 * TimeSpan.TicksPerSecond;
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
            new Tuple<Vec3f, Vec3f>(new Vec3f(-5352.223f , 251.3666f , -1174.69f), new Vec3f(0.3176593f , 0 , 0.9482043f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-5614.832f , 251.3338f , -835.5253f), new Vec3f(0.927043f , 0 , -0.3749532f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-5567.365f , 251.0296f , -251.2744f), new Vec3f(0.9848722f , 0 , 0.1732796f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-5424.19f , 249.4978f , 185.3704f), new Vec3f(0.333454f , 0 , -0.9427655f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-5217.961f , 251.48f , 722.0439f), new Vec3f(0.5184531f , 0 , -0.8551042f)),
            new Tuple<Vec3f, Vec3f>(new Vec3f(-4914.649f , 250.9502f , -119.0678f), new Vec3f(-0.9691378f , 0 , 0.2465149f)),
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

        public static void AddToTeam(TFFAClient client, Team team, bool force = false)
        {
            if (!force && client.Team == team)
                return;

            Kill(client, false);

            client.Class = PlayerClass.None;

            RemoveFromTeam(client);
            Teams[team].Add(client);
            client.Team = team;

            if (team == Team.Spec)
                client.SetToSpectator(WorldInst.Current, new Vec3f(-3175.43f, 472.1683f, 1359.367f), new Vec3f(0.5502188f, -0.03141155f, -0.8344474f));

            client.SendTeamChanged();
        }

        public static void RemoveFromTeam(TFFAClient client)
        {
            Teams[client.Team].Remove(client);
        }

        public static void SelectClass(TFFAClient client, PlayerClass c)
        {
            if (client.Class != c)
            {
                Kill(client, false);

                client.Class = c;
                if (status == TFFAPhase.Waiting && client.Team != Team.Spec)
                    SpawnNewPlayer(client);

                client.SendClassChanged();
            }
        }

        public static void UpdateStats(TFFAClient single = null)
        {
            if (scoreboardClients.Count > 0)
            {
                var stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.OpenScoreboard);

                stream.Write((byte)ALKills);
                stream.Write((byte)NLKills);

                List<TFFAClient> team1 = Teams[Team.AL];
                List<TFFAClient> team2 = Teams[Team.NL];

                stream.Write((byte)(team1.Count + team2.Count));
                for (int i = 0; i < team1.Count; i++)
                {
                    var c = team1[i];
                    stream.Write((byte)c.ID);
                    stream.Write((byte)c.Kills);
                    stream.Write((byte)c.Deaths);
                    stream.Write((ushort)c.Damage);
                    stream.Write((ushort)c.BaseClient.GetLastPing());
                }

                for (int i = 0; i < team2.Count; i++)
                {
                    var c = team2[i];
                    stream.Write((byte)c.ID);
                    stream.Write((byte)c.Kills);
                    stream.Write((byte)c.Deaths);
                    stream.Write((ushort)c.Damage);
                    stream.Write((ushort)c.BaseClient.GetLastPing());
                }

                if (single == null || statUpdateTimer.GetRemainingTicks() < ScoreboardUpdateTime / 4) // 25% time left, just send it to everyone
                {
                    for (int i = 0; i < scoreboardClients.Count; i++)
                        scoreboardClients[i].BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.UNRELIABLE);
                }
                else
                {
                    single.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.UNRELIABLE);
                }
            }
        }

        static GUCTimer gameTimer;
        static GUCTimer statUpdateTimer;
        static TFFAGame()
        {
            NPCInst.sOnHit += OnHit;

            statUpdateTimer = new GUCTimer(ScoreboardUpdateTime, () => UpdateStats());
            statUpdateTimer.Start();

            respawnTimer = new GUCTimer(RespawnTime, RespawnPlayers);
            respawnTimer.Start();


            gameTimer = new GUCTimer();
            PhaseFight();
        }

        public static int ALKills = 0;
        public static int NLKills = 0;

        public static void Kill(TFFAClient client, bool changePoints)
        {
            if (client.Character == null || client.Character.BaseInst.IsDead)
                return;

            client.Character.SetHealth(0);

            if (status == TFFAPhase.Fight)
            {
                if (changePoints)
                {
                    client.Deaths++;
                    if (client.Team == Team.AL)
                        NLKills++;
                    else if (client.Team == Team.NL)
                        ALKills++;
                }
                
                if (ALKills >= KillsToWin || NLKills >= KillsToWin)
                    PhaseEnd();
            }
        }

        static void OnHit(NPCInst attacker, NPCInst target, int damage)
        {
            if (attacker.BaseInst.Client == null || target.BaseInst.Client == null)
                return;

            TFFAClient att = ((TFFAClient)attacker.BaseInst.Client.ScriptObject);
            TFFAClient tar = ((TFFAClient)target.BaseInst.Client.ScriptObject);

            int realDamage = att.Team == tar.Team ? (int)(damage * 0.5f) : damage;
            int newHP = target.BaseInst.HP - realDamage;

            if (att.Team != tar.Team)
                att.Damage += damage;

            if (newHP <= 0)
            {
                if (att.Team != tar.Team)
                {
                    att.Kills++;
                }
                else
                {
                    att.Kills--;
                }
                Kill(tar, true);
            }
            else
            {
                target.SetHealth(newHP);
            }
        }

        static TFFAPhase status;
        public static TFFAPhase Status { get { return status; } }

        public static uint GetPhaseSecsLeft()
        {
            return (uint)(gameTimer.GetRemainingTicks() / TimeSpan.TicksPerSecond);
        }

        static PacketWriter GetPhaseMsg()
        {
            var stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.PhaseMsg);
            stream.Write((byte)status);
            stream.Write((uint)(gameTimer.Interval / TimeSpan.TicksPerSecond));
            return stream;
        }

        public static void PhaseWait()
        {
            Log.Logger.Log("Wait Phase");
            status = TFFAPhase.Waiting;
            gameTimer.SetInterval(WaitTime);
            gameTimer.SetCallback(PhaseFight);

            TFFAClient.ForEach(client =>
            {
                client.BaseClient.SendMenuMsg(GetPhaseMsg(), PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
                AddToTeam(client, Team.Spec);
                client.Deaths = 0;
                client.Kills = 0;
                client.Damage = 0;
            });
            RemoveAllVobs();
            ALKills = 0;
            NLKills = 0;

            gameTimer.Restart();
        }

        static void RemoveAllVobs()
        {
            WorldInst.Current.BaseWorld.ForEachVob(v => v.Despawn());
        }

        public static void PhaseFight()
        {
            Log.Logger.Log("Fight Phase");

            status = TFFAPhase.Fight;
            gameTimer.SetInterval(FightTime);
            gameTimer.SetCallback(PhaseEnd);

            TFFAClient.ForEach(client =>
            {
                client.BaseClient.SendMenuMsg(GetPhaseMsg(), PktPriority.LOW_PRIORITY, PktReliability.RELIABLE);
                SpawnNewPlayer(client);
                client.Deaths = 0;
                client.Kills = 0;
                client.Damage = 0;
            });
            RemoveAllVobs();
            ALKills = 0;
            NLKills = 0;

            gameTimer.Restart();
        }

        public static void PhaseEnd()
        {
            Log.Logger.Log("End Phase");
            status = TFFAPhase.End;
            gameTimer.SetInterval(EndTime);
            gameTimer.SetCallback(PhaseWait);

            if (ALKills > NLKills)
            {
                Teams[Team.NL].ForEach(client => { if (client.Character != null) client.Character.SetHealth(0); });
                var stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.WinMsg);
                stream.Write((byte)Team.AL);
                stream.Write((uint)(gameTimer.Interval / TimeSpan.TicksPerSecond));
                TFFAClient.ForEach(client => client.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE));
                Log.Logger.Log("TEAM GOMEZ HAT GEWONNEN");
            }
            else if (NLKills > ALKills)
            {
                Teams[Team.AL].ForEach(client => { if (client.Character != null) client.Character.SetHealth(0); });
                var stream = GameClient.GetMenuMsgStream();
                stream.Write((byte)MenuMsgID.WinMsg);
                stream.Write((byte)Team.NL);
                stream.Write((uint)(gameTimer.Interval / TimeSpan.TicksPerSecond));
                TFFAClient.ForEach(client => client.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE));
                Log.Logger.Log("TETRIANDOCH HAT GEWONNEN");
            }
            else
            {
                var stream = GetPhaseMsg();
                TFFAClient.ForEach(client => client.BaseClient.SendMenuMsg(stream, PktPriority.LOW_PRIORITY, PktReliability.RELIABLE));
            }

            gameTimer.Restart();
        }

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
            
            npc.UseCustoms = true;

            ItemInst weapon;
            ItemInst rangedWep;
            ItemInst ammo;
            ItemInst armor;
            ScriptOverlay overlay;
            Tuple<Vec3f, Vec3f> spawnPoint;

            if (client.Team == Team.AL)
            {
                if (client.Class == PlayerClass.Heavy)
                {
                    weapon = new ItemInst(ItemDef.Get<ItemDef>("2hschwert"));
                    armor = new ItemInst(ItemDef.Get<ItemDef>("itar_Garde"));
                    rangedWep = new ItemInst(ItemDef.Get<ItemDef>("itrw_crossbow"));
                    ammo = new ItemInst(ItemDef.Get<ItemDef>("itrw_bolt"));
                    def.Model.TryGetOverlay("2HST2", out overlay);
                }
                else
                {
                    weapon = new ItemInst(ItemDef.Get<ItemDef>("1hschwert"));
                    armor = new ItemInst(ItemDef.Get<ItemDef>("itar_schatten"));
                    rangedWep = new ItemInst(ItemDef.Get<ItemDef>("itrw_crossbow"));
                    ammo = new ItemInst(ItemDef.Get<ItemDef>("itrw_bolt"));
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
                    rangedWep = new ItemInst(ItemDef.Get<ItemDef>("itrw_longbow"));
                    ammo = new ItemInst(ItemDef.Get<ItemDef>("itrw_arrow"));
                    def.Model.TryGetOverlay("2HST1", out overlay);
                }
                else
                {
                    weapon = new ItemInst(ItemDef.Get<ItemDef>("1haxt"));
                    armor = new ItemInst(ItemDef.Get<ItemDef>("itar_bandit"));
                    rangedWep = new ItemInst(ItemDef.Get<ItemDef>("itrw_longbow"));
                    ammo = new ItemInst(ItemDef.Get<ItemDef>("itrw_arrow"));
                    def.Model.TryGetOverlay("1HST1", out overlay);
                }
                spawnPoint = NLSpawns[rand.Next(0, 6)];
            }

            npc.AddItem(weapon);
            npc.EquipItem(weapon); // 1 = DrawnWeapon

            npc.AddItem(rangedWep);
            npc.EquipItem(rangedWep);

            ammo.BaseInst.SetAmount(5);
            npc.AddItem(ammo);
            npc.EquipItem(ammo);

            npc.AddItem(armor);
            npc.EquipItem(armor);

            if (overlay != null)
            {
                npc.ApplyOverlay(overlay);
            }

            npc.Spawn(WorldInst.Current, spawnPoint.Item1, spawnPoint.Item2);
            client.SetControl(npc);
            client.SendNPCChanged();
        }

        #endregion
    }
}
