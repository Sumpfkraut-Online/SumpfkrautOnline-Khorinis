using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Utilities;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Arena
{
    static partial class HordeMode
    {
        static HordeMode()
        {
            NPCInst.sOnHit += NPCInst_sOnHit;
            NPCInst.sOnNPCInstMove += NPCInst_sOnNPCInstMove;
        }

        static void NPCInst_sOnNPCInstMove(NPCInst npc, Vec3f oldPos, Angles oldAng, NPCMovement oldMovement)
        {
            if (npc.IsDead || !npc.IsPlayer || npc.IsUnconscious)
                return;
            
            ArenaClient client = (ArenaClient)npc.Client;
            if (client.HordeClass == null)
                return;

            if (ActiveHordeStand != null)
                return;
            
            foreach (var pair in ActiveStands)
            {
                if (npc.GetPosition().GetDistance(pair.Item1.Position) < pair.Item1.Range)
                {
                    StartStand(pair.Item1);
                    break;
                }
            }
        }

        static void NPCInst_sOnHit(NPCInst attacker, NPCInst target, int damage)
        {
            if (attacker.IsPlayer)
            {
                if (target.IsPlayer)
                    return;

                ArenaClient player = (ArenaClient)attacker.Client;
                if (player.HordeClass == null) return;

                player.HordeScore += damage / 10.0f;
                if (target.HP <= 0)
                {
                    player.HordeKills++;
                    player.HordeScore += 5;
                }
            }
            else if (target.IsPlayer)
            {
                ArenaClient player = (ArenaClient)target.Client;
                if (player.HordeClass == null) return;

                if (target.HP <= 1)
                {
                    player.HordeDeaths++;
                    if (Players.TrueForAll(p => p.Character.HP <= 1))
                        EndHorde(false);
                }
            }
        }

        static GUCTimer gameTimer = new GUCTimer();

        static WorldInst activeWorld;
        static List<VobInst> spawnBarriers = new List<VobInst>(10);

        static List<HordeStand, VobInst[]> ActiveStands = new List<HordeStand, VobInst[]>(3);
        static HordeStand ActiveHordeStand;

        static List<ArenaClient> players = new List<ArenaClient>(10);
        public static List<ArenaClient> Players { get { return players; } }

        static int currentIndex = 0;
        public static void StartHorde()
        {
            currentIndex++;
            if (currentIndex >= HordeDef.GetAll().Count())
                currentIndex = 0;

            StartHorde(HordeDef.GetAll().ElementAt(currentIndex));
        }

        public static bool StartHorde(string name)
        {
            HordeDef def = HordeDef.GetDef(name);
            if (def == null) return false;

            StartHorde(def);

            return true;
        }

        public static void StartHorde(HordeDef def)
        {
            if (def == null) return;

            Log.Logger.Log("horde init");

            ArenaClient.ForEach(c =>
            {
                var client = (ArenaClient)c;
                client.HordeScore = 0;
                client.HordeDeaths = 0;
                client.HordeKills = 0;
                client.HordeClass = null;
            });
            players.Clear();

            if (activeWorld != null)
                activeWorld.BaseWorld.ForEachVob(v => v.Despawn());

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.HordeStart);
            stream.Write(def.Name);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));

            activeDef = def;
            activeWorld = WorldInst.List.Find(w => w.Path == def.WorldPath);

            spawnBarriers.Clear();
            foreach (var bar in activeDef.SpawnBarriers)
            {
                if (!bar.AddAfterEvent)
                    spawnBarriers.Add(CreateBarrier(bar));
            }

            ActiveStands.Clear();
            foreach (var stand in activeDef.Stands)
            {
                VobInst[] barriers = new VobInst[stand.Barriers.Length];
                for (int i = 0; i < barriers.Length; i++)
                {
                    var bar = stand.Barriers[i];
                    if (!bar.AddAfterEvent)
                        barriers[i] = CreateBarrier(bar);
                }
                ActiveStands.Add(stand, barriers);
            }

            foreach (var hi in activeDef.Items)
            {
                ItemInst item = new ItemInst(ItemDef.Get(hi.ItemDef));
                item.Spawn(activeWorld, hi.Position, hi.Angles);
            }

            standEnemies.Clear();
            standAgent = null;

            gameTimer.SetInterval(30 * TimeSpan.TicksPerSecond);
            gameTimer.SetCallback(Start);
            gameTimer.Stop();

            SetPhase(HordePhase.WarmUp);
        }

        static void StartStand(HordeStand stand)
        {
            Log.Logger.Log("Start stand");
            ActiveHordeStand = stand;
            standEnemies.Clear();

            var pers = new Sumpfkraut.AI.SimpleAI.AIPersonalities.SimpleAIPersonality(2000, 1);
            standAgent = new Sumpfkraut.AI.SimpleAI.AIAgent(new List<VobInst>(), pers);
            Sumpfkraut.AI.SimpleAI.AIManager.aiManagers[0].SubscribeAIAgent(standAgent);
            pers.Init(null, null);
            pers.GoTo(standAgent, ActiveHordeStand.Position);

            gameTimer.SetInterval(stand.Duration * TimeSpan.TicksPerSecond);
            gameTimer.SetCallback(EndStand);
            gameTimer.Start();

            FillUpStandEnemies();

            SetPhase(HordePhase.Stand);
        }

        static void EndStand()
        {
            ActiveHordeStand = null;
            SetPhase(HordePhase.Fight);
            Log.Logger.Log("end stand");
        }

        static List<NPCInst> standEnemies = new List<NPCInst>(20);
        static Sumpfkraut.AI.SimpleAI.AIAgent standAgent;

        static void FillUpStandEnemies()
        {
            if (ActiveHordeStand == null)
                return;

            int maxCount = (int)Math.Ceiling(ActiveHordeStand.MaxEnemies * players.Count);
            for (int i = standEnemies.Count; i < maxCount; i++)
            {
                float prob = Randomizer.GetFloat();
                foreach (var e in ActiveHordeStand.Enemies)
                    if (prob <= e.CountScale)
                    {
                        var npc = SpawnEnemy(e.Enemy, Randomizer.Get(ActiveHordeStand.EnemySpawns));
                        npc.OnDeath += OnStandEnemyDeath;
                        standAgent.aiClients.Add(npc);
                        standEnemies.Add(npc);
                        break;
                    }
            }
        }

        static void OnStandEnemyDeath(NPCInst npc)
        {
            standEnemies.Remove(npc);
            FillUpStandEnemies();
        }

        static VobInst CreateBarrier(HordeBarrier bar)
        {
            VobInst vob = new VobInst(VobDef.Get(bar.Definition));
            vob.Spawn(activeWorld, bar.Position, bar.Angles);
            return vob;
        }

        static void Start()
        {
            Log.Logger.Log("horde start");
            foreach (var bar in spawnBarriers)
                bar.Despawn();
            spawnBarriers.Clear();

            foreach (var bar in activeDef.SpawnBarriers)
                if (bar.AddAfterEvent)
                    CreateBarrier(bar);

            var aiManager = Sumpfkraut.AI.SimpleAI.AIManager.aiManagers[0];
            foreach (var group in activeDef.Enemies)
            {
                List<VobInst> vobs = new List<VobInst>();
                foreach (var pair in group.npcs)
                {
                    int maxCount = (int)Math.Ceiling(pair.CountScale * players.Count);
                    vobs.Capacity += maxCount;
                    for (int i = 0; i < maxCount; i++)
                    {
                        vobs.Add(SpawnEnemy(pair.Enemy, group.Position, group.Range));
                    }
                }

                var pers = new Sumpfkraut.AI.SimpleAI.AIPersonalities.SimpleAIPersonality(800, 1);
                aiManager.SubscribeAIAgent(new Sumpfkraut.AI.SimpleAI.AIAgent(vobs, pers));
                pers.Init(null, null);
            }

            SetPhase(HordePhase.Fight);
            gameTimer.Stop();
        }

        static void EndHorde(bool victory)
        {
            if (victory)
            {
                SetPhase(HordePhase.Victory);
                players.ForEach(p => p.Character.LiftUnconsciousness());
            }
            else
            {
                SetPhase(HordePhase.Lost);
            }

            gameTimer.SetCallback(StartHorde);
            gameTimer.SetInterval(60 * TimeSpan.TicksPerSecond);
            gameTimer.Start();
        }

        static void SpawnGroup()
        {
            /*var pers = new Sumpfkraut.AI.SimpleAI.AIPersonalities.SimpleAIPersonality(attackRadius, 1);
            pers.Init(null, null);
            pers.GoTo(npc, targetPos);
            var agent = new Sumpfkraut.AI.SimpleAI.AIAgent(vobs, pers);
            Sumpfkraut.AI.SimpleAI.AIManager.aiManagers[0].SubscribeAIAgent(agent);*/
        }

        static NPCInst SpawnEnemy(HordeEnemy enemy, Vec3f spawnPoint, float spawnRange = 100)
        {
            NPCInst npc = new NPCInst(NPCDef.Get(enemy.NPCDef));

            if (enemy.WeaponDef != null)
            {
                ItemInst item = new ItemInst(ItemDef.Get(enemy.WeaponDef));
                npc.Inventory.AddItem(item);
                npc.EffectHandler.TryEquipItem(item);
            }

            if (enemy.ArmorDef != null)
            {
                ItemInst item = new ItemInst(ItemDef.Get(enemy.ArmorDef));
                npc.Inventory.AddItem(item);
                npc.EffectHandler.TryEquipItem(item);
            }

            npc.SetHealth(enemy.Health, enemy.Health);

            Vec3f spawnPos = Randomizer.GetVec3fRad(spawnPoint, spawnRange);
            Angles spawnAng = Randomizer.GetYaw();

            npc.BaseInst.SetNeedsClientGuide(true);
            npc.Spawn(activeWorld, spawnPos, spawnAng);
            return npc;
        }

        static void SetPhase(HordePhase phase)
        {
            if (Phase == phase) return;

            Phase = phase;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.HordePhase);
            stream.Write((byte)Phase);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));

            OnPhaseChange?.Invoke(phase);
        }

        static void SpawnPlayer(ArenaClient client)
        {
            var charInfo = client.CharInfo;
            NPCInst npc = new NPCInst(NPCDef.Get(charInfo.BodyMesh == HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"))
            {
                UseCustoms = true,
                CustomBodyTex = charInfo.BodyTex,
                CustomHeadMesh = charInfo.HeadMesh,
                CustomHeadTex = charInfo.HeadTex,
                CustomVoice = charInfo.Voice,
                CustomFatness = charInfo.Fatness,
                CustomScale = new Vec3f(charInfo.BodyWidth, 1.0f, charInfo.BodyWidth),
                CustomName = charInfo.Name
            };

            foreach (string e in client.HordeClass.Equipment)
            {
                ItemInst item = new ItemInst(ItemDef.Get(e));
                npc.Inventory.AddItem(item);
                npc.EffectHandler.TryEquipItem(item);
            }

            if (client.HordeClass.NeedsBolts)
            {

            }

            npc.Inventory.AddItem(new ItemInst(ItemDef.Get("hptrank")));

            Vec3f spawnPos = Randomizer.GetVec3fRad(activeDef.SpawnPos, activeDef.SpawnRange);
            Angles spawnAng = Randomizer.GetYaw();
            npc.Spawn(activeWorld, spawnPos, spawnAng);
            client.SetControl(npc);
        }

        public static void WriteGameInfo(PacketWriter stream)
        {
            stream.Write(activeDef.Name);
            stream.Write((byte)Phase);
        }

        public static void JoinClass(PacketReader stream, ArenaClient client)
        {
            int index = stream.ReadByte();

            HordeClassDef classDef = activeDef.Classes.ElementAtOrDefault(index);
            if (classDef == null)
                return;

            if (activeDef == null)
                StartHorde();

            if (Phase != HordePhase.WarmUp)
                return;

            client.HordeClass = classDef;
            players.Add(client);
            SpawnPlayer(client);

            if (players.Count == 1)
                gameTimer.Start();
        }

        public static void LeaveHorde(ArenaClient client)
        {
            client.HordeClass = null;
            if (players.Remove(client) && players.Count == 0)
            {
                StartHorde(); // restart / next
            }
        }

        public static void JoinSpectate(ArenaClient client)
        {
            PosAng specPA = activeDef.SpecPA;
            foreach (var player in players)
            {
                if (player.Character != null && player.Character.IsSpawned)
                {
                    specPA = new PosAng(player.Character.GetPosition(), player.Character.GetAngles());
                    specPA.Position.Y += 100;
                    break;
                }
            }
            client.SetToSpectator(activeWorld, specPA.Position, specPA.Angles);
        }
    }
}
