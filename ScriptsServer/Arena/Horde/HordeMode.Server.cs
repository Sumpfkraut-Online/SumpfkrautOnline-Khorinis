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
using GUC.Scripts.Sumpfkraut.AI.SimpleAI;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities;

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

            if (ActiveStandInst != null)
                return;

            foreach (var s in ActiveStands)
            {
                if (npc.GetPosition().GetDistance(s.Stand.Position) < s.Stand.Range)
                {
                    StartStand(s);
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

        class StandInst
        {
            public int Index;
            public HordeStand Stand;
            public List<VobInst> Barriers;
            public NPCInst Boss;
            public AIAgent Agent;
        }

        static List<StandInst> ActiveStands = new List<StandInst>();
        static StandInst ActiveStandInst;

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

        static bool BossProtection(NPCInst attacker, NPCInst target)
        {
            return false;
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
            players.ForEach(c => c.Spectate());
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
            for (int i = 0; i < activeDef.Stands.Length; i++)
            {
                var stand = activeDef.Stands[i];
                StandInst inst = new StandInst()
                {
                    Index = i,
                    Stand = stand,
                };

                if (stand.Boss != null)
                {
                    inst.Boss = SpawnEnemy(stand.Boss, stand.Position);
                    inst.Boss.CanGetHit += BossProtection;
                }

                inst.Barriers = new List<VobInst>(stand.Barriers.Length);
                foreach (var bar in stand.Barriers)
                {
                    if (!bar.AddAfterEvent)
                        inst.Barriers.Add(CreateBarrier(bar));
                }

                ActiveStands.Add(inst);
            }

            foreach (var hi in activeDef.Items)
            {
                ItemInst item = new ItemInst(ItemDef.Get(hi.ItemDef));
                item.Spawn(activeWorld, hi.Position, hi.Angles);
            }

            standEnemyCount = 0;
            ActiveStandInst = null;

            gameTimer.SetInterval(30 * TimeSpan.TicksPerSecond);
            gameTimer.SetCallback(Start);
            gameTimer.Stop();

            SetPhase(HordePhase.WarmUp);
        }

        static void StartStand(StandInst inst)
        {
            Log.Logger.Log("Start stand");
            ActiveStandInst = inst;
            standEnemyCount = 0;

            inst.Agent = CreateAgent(2 * inst.Stand.Range);
            if (inst.Boss != null)
            {
                inst.Agent.aiClients.Add(inst.Boss);
                inst.Boss.CanGetHit -= BossProtection;
                inst.Boss.OnDeath += boss => EndStand();
                
            }
            else
            {
                gameTimer.SetInterval(inst.Stand.Duration * TimeSpan.TicksPerSecond);
                gameTimer.SetCallback(EndStand);
                gameTimer.Start();
            }
            FillUpStandEnemies();

            SetPhase(HordePhase.Stand);
        }

        static void EndStand()
        {
            var inst = ActiveStandInst;
            ActiveStands.Remove(inst);
            foreach (var v in inst.Barriers)
                v.Despawn();

            foreach (var bar in inst.Stand.Barriers)
                if (bar.AddAfterEvent)
                    CreateBarrier(bar);

            ActiveStandInst = null;
            if (ActiveStands.Count == 0)
            {
                EndHorde(true);
            }
            else
            {
                SetPhase(HordePhase.Fight);
            }
            Log.Logger.Log("end stand");
        }

        static int standEnemyCount = 0;

        static void FillUpStandEnemies()
        {
            if (ActiveStandInst == null)
                return;

            var def = ActiveStandInst.Stand;

            int maxCount = (int)Math.Ceiling(def.MaxEnemies * players.Count);
            for (int i = standEnemyCount; i < maxCount; i++)
            {
                float prob = Randomizer.GetFloat();
                foreach (var e in def.Enemies)
                    if (prob <= e.CountScale)
                    {
                        var npc = SpawnEnemy(e.Enemy, Randomizer.Get(def.EnemySpawns));
                        npc.OnDeath += OnStandEnemyDeath;
                        ActiveStandInst.Agent.aiClients.Add(npc);
                        ((SimpleAIPersonality)ActiveStandInst.Agent.AIPersonality).GoTo(npc, ActiveStandInst.Stand.Position);
                        standEnemyCount++;
                        break;
                    }
            }
        }

        static void OnStandEnemyDeath(NPCInst npc)
        {
            standEnemyCount--;
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

            foreach (var group in activeDef.Enemies)
            {
                var agent = CreateAgent();
                foreach (var pair in group.npcs)
                {
                    int maxCount = (int)Math.Ceiling(pair.CountScale * players.Count);
                    for (int i = 0; i < maxCount; i++)
                    {
                        agent.aiClients.Add(SpawnEnemy(pair.Enemy, group.Position, group.Range));
                    }
                }
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
                players.ForEach(p => p.Character.SetHealth(0));
            }

            gameTimer.SetCallback(StartHorde);
            gameTimer.SetInterval(30 * TimeSpan.TicksPerSecond);
            gameTimer.Restart();
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

            //npc.TeamID = 1;
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
            if (Phase == HordePhase.Stand)
                stream.Write((byte)ActiveStandInst.Index);

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
            //npc.TeamID = 0;
            npc.Spawn(activeWorld, spawnPos, spawnAng);
            client.SetControl(npc);
        }

        public static void WriteGameInfo(PacketWriter stream)
        {
            stream.Write(activeDef.Name);
            stream.Write((byte)Phase);
            if (Phase == HordePhase.Stand)
                stream.Write((byte)ActiveStandInst.Index);
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

        static AIAgent CreateAgent(float aggressionRad = 800)
        {
            var pers = new SimpleAIPersonality(aggressionRad, 1);
            var agent = new AIAgent(new List<VobInst>(), pers);
            AIManager.aiManagers[0].SubscribeAIAgent(agent);
            pers.Init(null, null);
            return agent;
        }
    }
}
