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

namespace GUC.Server.Scripts.TFFA
{
    public static class TFFAGame
    {
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
            RemoveFromTeam(client);
            Teams[team].Add(client);
            client.Team = team;
        }

        public static void RemoveFromTeam(TFFAClient client)
        {
            Teams[client.Team].Remove(client);
        }

        public static void SelectClass(TFFAClient client, PlayerClass c)
        {
            client.Class = c;
        }

        public static void UpdateStats()
        {
            // TEAM MENU STATS
            PacketWriter stream = GameClient.GetMenuMsgStream();
            stream.Write((byte)MenuMsgID.OpenTeamMenu);
            stream.Write((byte)GetCount(Team.Spec));
            stream.Write((byte)GetCount(Team.AL));
            stream.Write((byte)GetCount(Team.NL));
            for (int i = 0; i < teamMenuClients.Count; i++)
                teamMenuClients[i].BaseClient.SendMenuMsg(stream);

            // CLASS MENU STATS

            stream = GameClient.GetMenuMsgStream();
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
                    classMenuClients[i].BaseClient.SendMenuMsg(stream);

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
                    classMenuClients[i].BaseClient.SendMenuMsg(stream);

        }

        static GUCTimer statUpdateTimer;
        static TFFAGame()
        {
            statUpdateTimer = new GUCTimer(5000000, UpdateStats);
            statUpdateTimer.Start();

            respawnTimer = new GUCTimer(10 * TimeSpan.TicksPerSecond, RespawnPlayers);
            respawnTimer.Start();             
        }

        public static List<TFFAClient> teamMenuClients = new List<TFFAClient>();
        public static List<TFFAClient> classMenuClients = new List<TFFAClient>();
        public static List<TFFAClient> scoreboardClients = new List<TFFAClient>();

        #region Respawn

        static GUCTimer respawnTimer;

        static Random rand = new Random();
        static void RespawnPlayers()
        {
            var list = Teams[Team.AL];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Class == PlayerClass.None)
                    continue;

                var character = list[i].Character;
                if (character == null || character.BaseInst.HP <= 0)
                {
                    NPCDef def = BaseVobDef.Get<NPCDef>("player");
                    NPCInst npc = new NPCInst(def);

                    if (list[i].Class == PlayerClass.Heavy)
                    {
                        var item = new ItemInst(ItemDef.Get<ItemDef>("2hschwert"));
                        npc.AddItem(item);
                        npc.EquipItem(1, item);
                        npc.DrawnWeapon = item;

                        item = new ItemInst(ItemDef.Get<ItemDef>("itar_Garde"));
                        npc.AddItem(item);
                        npc.EquipItem(0, item);

                        ScriptOverlay ov;
                        if (def.Model.TryGetOverlay("2HST2", out ov))
                        {
                            npc.ApplyOverlay(ov);
                        }
                    }
                    else
                    {
                        var item = new ItemInst(ItemDef.Get<ItemDef>("1hschwert"));
                        npc.AddItem(item);
                        npc.EquipItem(1, item);
                        npc.DrawnWeapon = item;

                        item = new ItemInst(ItemDef.Get<ItemDef>("itar_schatten"));
                        npc.AddItem(item);
                        npc.EquipItem(0, item);
                    }

                    var spawnPoint = ALSpawns[rand.Next(0, 6)];
                    npc.Spawn(WorldInst.Current, spawnPoint.Item1, spawnPoint.Item2);
                    list[i].SetControl(npc);
                }
            }

            list = Teams[Team.NL];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Class == PlayerClass.None)
                    continue;

                var character = list[i].Character;
                if (character == null || character.BaseInst.HP <= 0)
                {
                    NPCDef def = BaseVobDef.Get<NPCDef>("player");
                    NPCInst npc = new NPCInst(def);
                    
                    if (list[i].Class == PlayerClass.Heavy)
                    {
                        var item = new ItemInst(ItemDef.Get<ItemDef>("2haxt"));
                        npc.AddItem(item);
                        npc.EquipItem(1, item);
                        npc.DrawnWeapon = item;

                        item = new ItemInst(ItemDef.Get<ItemDef>("itar_söldner"));
                        npc.AddItem(item);
                        npc.EquipItem(0, item);

                        ScriptOverlay ov;
                        if (def.Model.TryGetOverlay("2HST1", out ov))
                        {
                            npc.ApplyOverlay(ov);
                        }
                    }
                    else
                    {
                        var item = new ItemInst(ItemDef.Get<ItemDef>("1haxt"));
                        npc.AddItem(item);
                        npc.EquipItem(1, item);
                        npc.DrawnWeapon = item;

                        item = new ItemInst(ItemDef.Get<ItemDef>("itar_bandit"));
                        npc.AddItem(item);
                        npc.EquipItem(0, item);
                    }

                    var spawnPoint = NLSpawns[rand.Next(0, 6)];
                    npc.Spawn(WorldInst.Current, spawnPoint.Item1, spawnPoint.Item2);
                    list[i].SetControl(npc);
                }
            }
        }

        #endregion
    }
}
