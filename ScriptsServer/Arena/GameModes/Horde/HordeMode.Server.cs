using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    partial class HordeMode
    {
        class StandInst
        {
            public int Index;
            public HordeScenario.Stand Stand;
            public List<VobInst> Barriers;
            public NPCInst Boss;
            public AIAgent Agent;
        }

        List<StandInst> Stands = new List<StandInst>();
        StandInst ActiveStand;

        List<VobInst> spawnBarriers = new List<VobInst>(10);

        static bool BossProtection(NPCInst attacker, NPCInst target)
        {
            return false;
        }

        protected override void Start(GameScenario scenario)
        {
            base.Start(scenario);

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
                    inst.Boss = SpawnEnemy(stand.Boss, stand.Position);
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

        protected override void End()
        {
            spawnBarriers.Clear();
            Stands.Clear();

            base.End();
        }

        VobInst CreateBarrier(HordeScenario.Barrier bar)
        {
            VobInst vob = new VobInst(VobDef.Get(bar.Definition));
            vob.Spawn(World, bar.Position, bar.Angles);
            return vob;
        }



        int standEnemyCount = 0;
        void StartStand(StandInst inst)
        {
            Log.Logger.Log("Start stand");
            ActiveStand = inst;
            standEnemyCount = 0;

            inst.Agent = CreateAgent(2 * inst.Stand.Range);
            if (inst.Boss != null)
            {
                inst.Agent.aiClients.Add(inst.Boss);
                inst.Boss.AllowHitTarget.Remove(BossProtection);
                inst.Boss.OnDeath += boss => EndStand();
            }
            else
            {
                phaseTimer.SetInterval(inst.Stand.Duration * TimeSpan.TicksPerSecond);
                phaseTimer.SetCallback(EndStand);
                phaseTimer.Start();
            }

            FillUpStandEnemies();

            SetPhase(GamePhase.Fight + 1 + inst.Index);
        }

        void EndStand()
        {
            Stands.Remove(ActiveStand);
            foreach (var v in ActiveStand.Barriers)
                v.Despawn();

            foreach (var bar in ActiveStand.Stand.Barriers)
                if (bar.AddAfterEvent)
                    CreateBarrier(bar);

            ActiveStand = null;
            if (Stands.Count == 0)
            {
                End();
            }
            else
            {
                SetPhase(GamePhase.Fight);
            }
            Log.Logger.Log("end stand");
        }

        void FillUpStandEnemies()
        {
            if (ActiveStand == null)
                return;

            var def = ActiveStand.Stand;

            int maxCount = (int)Math.Ceiling(def.MaxEnemies * players.Count);
            for (int i = standEnemyCount; i < maxCount; i++)
            {
                float prob = Randomizer.GetFloat();
                foreach (var e in def.Enemies)
                    if (prob <= e.CountScale)
                    {
                        var npc = SpawnEnemy(e.Enemy, Randomizer.Get(def.EnemySpawns));
                        npc.OnDeath += OnStandEnemyDeath;
                        ActiveStand.Agent.aiClients.Add(npc);
                        ((SimpleAIPersonality)ActiveStand.Agent.AIPersonality).GoTo(npc, ActiveStand.Stand.Position);
                        standEnemyCount++;
                        break;
                    }
            }
        }

        void OnStandEnemyDeath(NPCInst npc)
        {
            standEnemyCount--;
            FillUpStandEnemies();
        }

        protected override void Fight()
        {
            foreach (var bar in spawnBarriers)
                bar.Despawn();
            spawnBarriers.Clear();

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
                        agent.aiClients.Add(SpawnEnemy(pair.Enemy, group.Position, group.Range));
                    }
                }
            }

            base.Fight();
        }

        protected override void FadeOut()
        {
            if (players.TrueForAll(p => !p.IsCharacter || p.Character.HP <= 1))
            {
                //SetPhase(HordePhase.Lost);
                players.ForEach(p => p.Character.SetHealth(0));
            }
            else
            {
                //SetPhase(HordePhase.Victory);
                players.ForEach(p => p.Character.LiftUnconsciousness());
            }

            base.FadeOut();
        }

        public override void SelectClass(ArenaClient client, int index)
        {
            if (!Scenario.PlayerClasses.TryGet(index, out NPCClass pc))
                return;

            client.GMClass = pc;
            client.GMTeamID = 0;

            if (client.Character == null || Phase == GamePhase.WarmUp)
                SpawnCharacter(client, Scenario.SpawnPos, Scenario.SpawnRange);
        }
    }
}
