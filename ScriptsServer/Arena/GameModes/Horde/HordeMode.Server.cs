using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities;
using GUC.Scripting;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    partial class HordeMode
    {
        AIManager aiMan = new AIManager(true);

        #region Stands

        class StandInst
        {
            public int Index;
            public HordeScenario.Stand Stand;
            public List<VobInst> Barriers;
            public NPCInst Boss;
            public AIAgent Agent;
        }

        IEnumerable<ArenaClient> StandingPlayers { get { return players.Where(p => p.IsCharacter && !p.Character.IsDead && !p.Character.IsUnconscious); } }

        List<StandInst> Stands = new List<StandInst>();
        StandInst ActiveStand;

        List<NPCInst> AmbientNPCs = new List<NPCInst>();

        GUCTimer standTimer = new GUCTimer();
        GUCTimer standSpawnTimer = new GUCTimer();
        void CheckStandDistance()
        {
            if (ActiveStand != null)
                return;

            foreach (var player in players)
            {
                if (player.GMTeamID < TeamIdent.GMPlayer || !player.IsCharacter || player.Character.HP <= 1)
                    continue;

                if (player.Character.GetPosition().GetDistance(Stands[0].Stand.Position) < Stands[0].Stand.Range)
                {
                    StartStand(Stands[0]);
                    return;
                }
            }
        }

        void StartStand(StandInst inst)
        {
            Log.Logger.Log("Start stand");
            ActiveStand = inst;

            inst.Agent = CreateAgent(2 * inst.Stand.Range);
            if (inst.Boss != null)
            {
                inst.Agent.Add(inst.Boss);
                inst.Boss.AllowHitTarget.Remove(BossProtection);
                inst.Boss.OnDeath += boss => EndStand();
            }

            standSpawnTimer.SetInterval(inst.Stand.EnemySpawnInterval);
            standSpawnTimer.SetCallback(StandSpawn);
            standSpawnTimer.Start();
            StandSpawn();

            SetPhase(GamePhase.Fight + 1 + inst.Index);
            standTimer.SetInterval(inst.Stand.Duration);
            standTimer.SetCallback(EndStand);
        }

        HashSet<NPCInst> AliveStandEnemies = new HashSet<NPCInst>();
        int standSpawnIndex = 0;
        void StandSpawn()
        {
            if (ActiveStand == null || Phase < GamePhase.Fight)
                return;

            var def = ActiveStand.Stand;
            var agent = ActiveStand.Agent;

            int max = (int)Math.Ceiling(def.EnemyCountPerGroup * players.Count);
            for (int g = 0; g < def.EnemyGroupsPerSpawn; g++)
            {
                if (standSpawnIndex >= def.EnemySpawns.Length)
                    standSpawnIndex = 0;

                var spawnPos = def.EnemySpawns[standSpawnIndex++];
                for (int i = 0; i < max; i++)
                {
                    float prob = Randomizer.GetFloat();
                    var e = def.Enemies.Last(n => prob <= n.CountScale);

                    var npc = SpawnNPC(e.Enemy, spawnPos, 10);
                    agent.Add(npc);
                    if (ActiveStand.Stand.KillSpawnsOnEnd)
                    {
                        AliveStandEnemies.Add(npc);
                        npc.OnDeath += OnStandSpawnDeath;
                    }

                    ((SimpleAIPersonality)agent.AIPersonality).Attack(npc, Randomizer.Get(players.Where(p => p.IsCharacter && p.Character.HP > 1)).Character);
                }
            }
        }

        void OnStandSpawnDeath(NPCInst npc)
        {
            AliveStandEnemies.Remove(npc);
        }

        void EndStand()
        {
            if (ActiveStand == null)
                return;

            standSpawnTimer.Stop();
            Stands.Remove(ActiveStand);
            foreach (var v in ActiveStand.Barriers)
                v.Despawn();

            foreach (var bar in ActiveStand.Stand.Barriers)
                if (bar.AddAfterEvent)
                    CreateBarrier(bar);

            if (ActiveStand.Stand.KillSpawnsOnEnd)
            {
                foreach(var npc in AliveStandEnemies)
                {
                    npc.OnDeath -= OnStandSpawnDeath;
                    npc.SetHealth(0);
                }
                AliveStandEnemies.Clear();
            }

            ActiveStand = null;
            if (Stands.Count == 0)
            {
                HordeFadeOut(true);
            }
            else
            {
                standTimer.SetInterval(TimeSpan.TicksPerSecond);
                standTimer.SetCallback(CheckStandDistance);
                SetPhase(GamePhase.Fight);
            }
            Log.Logger.Log("end stand");
        }

        static bool BossProtection(NPCInst attacker, NPCInst target)
        {
            return false;
        }

        #endregion

        #region Barriers

        List<VobInst> spawnBarriers = new List<VobInst>(10);

        VobInst CreateBarrier(HordeScenario.Barrier bar)
        {
            VobInst vob = new VobInst(VobDef.Get(bar.Definition));
            vob.Spawn(World, bar.Position, bar.Angles);
            return vob;
        }

        #endregion

        #region Phases (Start, Fight, FadeOut, End)

        protected override void Start(GameScenario scenario)
        {
            base.Start(scenario);

            if (Scenario.SpawnBarriers != null)
                foreach (var bar in Scenario.SpawnBarriers)
                {
                    if (!bar.AddAfterEvent)
                        spawnBarriers.Add(CreateBarrier(bar));
                }

            foreach (var hi in Scenario.Items)
            {
                ItemInst item = new ItemInst(ItemDef.Get(hi.ItemDef));
                item.Spawn(World, hi.Position, hi.Angles);
            }

            for (int i = 0; i < Scenario.Stands.Length; i++)
            {
                var stand = Scenario.Stands[i];
                StandInst inst = new StandInst()
                {
                    Index = i,
                    Stand = stand,
                };

                if (stand.Boss != null)
                {
                    inst.Boss = SpawnNPC(stand.Boss, stand.Position);
                    inst.Boss.AllowHitTarget.Add(BossProtection);
                }

                inst.Barriers = new List<VobInst>(stand.Barriers.Length);
                foreach (var bar in stand.Barriers)
                {
                    if (!bar.AddAfterEvent)
                        inst.Barriers.Add(CreateBarrier(bar));
                }

                Stands.Add(inst);
            }
        }

        protected override void Fight()
        {
            foreach (var bar in spawnBarriers)
                bar.Despawn();
            spawnBarriers.Clear();

            if (Scenario.SpawnBarriers != null)
                foreach (var bar in Scenario.SpawnBarriers)
                    if (bar.AddAfterEvent)
                        CreateBarrier(bar);

            foreach (var group in Scenario.Enemies)
            {
                var agent = CreateAgent();
                foreach (var pair in group.npcs)
                {
                    int maxCount = (int)Math.Ceiling(pair.CountScale * players.Count);
                    for (int i = 0; i < maxCount; i++)
                    {
                        agent.Add(SpawnNPC(pair.Enemy, group.Position, group.Range));
                    }
                }
            }

            foreach (var group in Scenario.AmbientNPCs)
            {
                foreach (var pair in group.npcs)
                {
                    for (int i = 0; i < pair.CountScale; i++)
                    {
                        var npc = SpawnNPC(pair.Enemy, group.Position, new Angles(0, group.Yaw, 0), group.Range, 0, false);
                        ItemInst weapon = npc.GetEquipmentBySlot(NPCSlots.TwoHanded);
                        if (weapon == null) weapon = npc.GetEquipmentBySlot(NPCSlots.OneHanded1);
                        if (weapon != null) npc.DoDrawWeapon(weapon);
                        AmbientNPCs.Add(npc);
                    }
                }
            }

            standTimer.SetInterval(TimeSpan.TicksPerSecond);
            standTimer.SetCallback(CheckStandDistance);
            standTimer.Start();

            base.Fight();

            NPCInst.sOnHit += OnHit;
        }

        protected override void FadeOut()
        {
            HordeFadeOut(Stands.Count == 0 && players.TrueForAll(p => !p.IsCharacter || (!p.Character.IsUnconscious && !p.Character.IsDead)));
        }

        void HordeFadeOut(bool playersWon)
        {
            if (Phase == GamePhase.FadeOut)
                return;

            standSpawnTimer.Stop();
            standTimer.Stop();
            if (playersWon)
            {
                var stream = ArenaClient.GetStream(ScriptMessages.HordeWin);
                players.ForEach(p =>
                {
                    p.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
                    if (p.IsCharacter)
                        p.Character.LiftUnconsciousness();
                });

                var agent = CreateAgent();
                foreach (var npc in AmbientNPCs)
                {
                    npc.BaseInst.SetNeedsClientGuide(true);
                    agent.Add(npc);
                }
            }
            else
            {
                var stream = ArenaClient.GetStream(ScriptMessages.HordeLoss);
                players.ForEach(p =>
                {
                    p.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
                    if (p.IsCharacter)
                        p.Character.SetHealth(0);
                });

                ClearAgents();
            }
            base.FadeOut();
        }

        protected override void End()
        {
            spawnBarriers.Clear();
            Stands.Clear();
            standSpawnTimer.Stop();
            standTimer.Stop();
            NPCInst.sOnHit -= OnHit;
            HordeBoard.Instance.RemoveAll();
            AmbientNPCs.Clear();


            base.End();
        }

        #endregion

        void OnHit(NPCInst attacker, NPCInst target, int damage)
        {
            if (!IsActive || ActiveMode.Phase < GamePhase.Fight)
                return;

            if (attacker.IsPlayer)
            {
                if (target.IsPlayer)
                    return;

                ArenaClient player = (ArenaClient)attacker.Client;
                player.GMScore += damage / 10.0f;
                if (target.HP <= 0)
                {
                    player.GMKills++;
                    player.GMScore += 5;
                }
            }
            else if (target.IsPlayer)
            {
                ArenaClient player = (ArenaClient)target.Client;
                if (target.HP <= 1)
                {
                    player.GMDeaths++;
                    if (players.TrueForAll(p => !p.IsCharacter || p.Character.IsDead || p.Character.IsUnconscious))
                        HordeFadeOut(false);
                }
            }
        }

        public override void SelectClass(ArenaClient client, int index)
        {
            if (!Scenario.PlayerClasses.TryGet(index, out NPCClass pc))
                return;

            client.GMClass = pc;
            client.SetTeamID(TeamIdent.GMPlayer);
            
            if (Phase == GamePhase.WarmUp || Phase == GamePhase.None)
            {
                InitialSpawnClient(client, true);
            }
            else if (Phase != GamePhase.FadeOut)
            {
                RespawnClient(client);
            }
        }

        public override void OnSuicide(ArenaClient client)
        {
            if (Phase < GamePhase.Fight)
                return;

            client.GMDeaths++;
            if (players.TrueForAll(p => !p.IsCharacter || p.Character.IsDead || p.Character.IsUnconscious))
                HordeFadeOut(false);
        }

        public void RespawnClient(ArenaClient client)
        {
            if (Phase < GamePhase.Fight)
                return;

            Vec3f nextStand = Stands[0].Stand.Position;
            // find player closest to next stand
            IEnumerable<Vec3f> positions = StandingPlayers.Select(p => p.Character.GetPosition());
            if (!Vec3f.FindClosest(Stands[0].Stand.Position, positions, out Vec3f best))
                return;

            // two closest respawns to player
            IEnumerable<Vec3f> x = Scenario.Respawns.OrderBy(p => p.GetDistance(best)).Take(2);

            // closest of the two respawns to next stand
            if (!Vec3f.FindClosest(best, x, out Vec3f result))
                return;

            var npc = SpawnCharacter(client, World, result, 100);
            npc.DropUnconscious();
        }

        bool OnAllowHit(NPCInst attacker, NPCInst target)
        {
            return attacker.TeamID != target.TeamID;
        }

        protected override NPCInst SpawnCharacter(ArenaClient client, WorldInst world, PosAng spawnPoint)
        {
            NPCInst pc = base.SpawnCharacter(client, world, spawnPoint);
            pc.AllowHitTarget.Add(OnAllowHit);
            pc.DropUnconsciousOnDeath = true;
            pc.UnconsciousDuration = -1;
            return pc;
        }

        protected override NPCInst SpawnNPC(NPCClass classDef, Vec3f pos, Angles ang, float posOffset = 100, int teamID = 1, bool giveAI = true)
        {
            NPCInst npc =  base.SpawnNPC(classDef, pos, ang, posOffset, teamID, giveAI);
            npc.AllowHitTarget.Add(OnAllowHit);
            return npc;
        }
    }
}
