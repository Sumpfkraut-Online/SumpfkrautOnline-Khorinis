using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities;
using GUC.Utilities;

namespace GUC.Scripts.Arena.GameModes
{
    partial class GameMode
    {
        // Lists
        protected List<ArenaClient> players = new List<ArenaClient>(10);
        public readonly ReadOnlyList<ArenaClient> Players;

        public GameMode()
        {
            this.Players = new ReadOnlyList<ArenaClient>(players);
        }
        

        public static int NextScenarioIndex = 0;
        public static void StartNextScenario()
        {
            if (IsActive)
                ActiveMode.FadeOut();
            else
                InitScenario(GameScenario.Get(NextScenarioIndex));
        }

        public static void InitScenario(GameScenario scenario)
        {
            if (scenario == null)
                return;

            if (++NextScenarioIndex >= GameScenario.Count)
                NextScenarioIndex = 0;

            Log.Logger.Log("Init game scenario " + scenario.Name);
            ActiveMode = scenario.GetMode();

            var world = new WorldInst(null)
            {
                Path = scenario.WorldPath
            };
            world.Create();

            if (scenario.WorldTimeScale > 0)
            {
                world.Clock.SetTime(scenario.WorldTime, scenario.WorldTimeScale);
            }
            else
            {
                world.Clock.SetTime(scenario.WorldTime, 1);
                world.Clock.Stop();
            }

            if (scenario.WorldBarrier >= 0)
            {
                world.Barrier.StopTimer();
                if (scenario.WorldBarrier > 0)
                    world.Barrier.SetNextWeight(0, scenario.WorldBarrier);
            }

            if (scenario.WorldWeather >= 0)
            {
                world.Weather.StopRainTimer();
                if (scenario.WorldWeather > 0)
                    world.Weather.SetNextWeight(0, scenario.WorldWeather);
            }

            ActiveMode.World = world;

            ActiveMode.Start(scenario);
        }

        public WorldInst World { get; private set; }

        protected GUCTimer phaseTimer = new GUCTimer();
        public uint PhaseRemainingMsec { get { return (uint)(phaseTimer.GetRemainingTicks() / TimeSpan.TicksPerMillisecond); } }

        public virtual void JoinAsSpectator(ArenaClient client)
        {
            client.KillCharacter();
            if (!players.Contains(client))
                players.Add(client);

            client.SetTeamID(TeamIdent.GMSpectator);
            client.SetToSpectator(World, Scenario.SpecPoint.Position, Scenario.SpecPoint.Angles);
        }

        /// <summary> Does not change the TeamID! </summary>
        public virtual bool Leave(ArenaClient client)
        {
            if (!client.GMJoined)
                return false;

            client.KillCharacter();
            return players.Remove(client);
        }

        public static void WriteGameInfo(PacketWriter stream)
        {
            if (!stream.Write(IsActive))
                return;

            stream.Write(ActiveMode.Scenario.Name);
            stream.Write((byte)ActiveMode.Phase);
            stream.Write(ActiveMode.PhaseRemainingMsec);
        }

        protected virtual void Start(GameScenario scenario)
        {
            this.Scenario = scenario;

            var stream = ArenaClient.GetStream(ScriptMessages.ModeStart);
            stream.Write(scenario.Name);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
            Phase = GamePhase.None;
        }

        protected virtual void Fight()
        {
            SetPhase(GamePhase.Fight);

            phaseTimer.SetInterval(Scenario.FightDuration);
            phaseTimer.SetCallback(FadeOut);
            phaseTimer.Start();
        }

        protected virtual void FadeOut()
        {
            SetPhase(GamePhase.FadeOut);

            phaseTimer.SetInterval(FadeOutDuration);
            phaseTimer.SetCallback(End);
            phaseTimer.Start();
        }

        protected virtual void End()
        {
            ArenaClient.ForEach(c =>
            {
                c.GMClass = null;
                c.GMKills = 0;
                c.GMScore = 0;
                c.GMDeaths = 0;
            });

            this.World.Delete();
            this.World = null;

            InitScenario(GameScenario.Get(NextScenarioIndex));

            foreach (var player in players)
            {
                JoinAsSpectator(player);
            }

            players.Clear();
        }

        protected void SetPhase(GamePhase phase)
        {
            this.Phase = phase;

            var stream = ArenaClient.GetStream(ScriptMessages.ModePhase);
            stream.Write((byte)Phase);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
        }

        protected void SpawnCharacter(ArenaClient client, Vec3f position, float range)
        {
            SpawnCharacter(client, new PosAng(Randomizer.GetVec3fRad(position, range), Randomizer.GetYaw()));
        }

        protected virtual void SpawnCharacter(ArenaClient client, PosAng spawnPoint)
        {
            if (client == null || !client.GMJoined || client.GMClass == null)
                return;

            client.KillCharacter();

            NPCClass classDef = client.GMClass;

            NPCInst npc;
            if (classDef.Definition == null)
            {
                var charInfo = client.CharInfo;
                npc = new NPCInst(NPCDef.Get(charInfo.BodyMesh == HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"))
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
            }
            else
            {
                npc = new NPCInst(NPCDef.Get(classDef.Definition));
            }

            if (classDef.ItemDefs != null)
                foreach (var invItem in classDef.ItemDefs)
                {
                    var item = new ItemInst(ItemDef.Get(invItem.DefName));
                    item.SetAmount(invItem.Amount);
                    npc.Inventory.AddItem(item);
                    npc.EffectHandler.TryEquipItem(item);
                }

            if (classDef.Overlays != null)
                foreach (var overlay in classDef.Overlays)
                {
                    if (npc.ModelDef.TryGetOverlay(overlay, out ScriptOverlay ov))
                        npc.ModelInst.ApplyOverlay(ov);
                }

            npc.TeamID = (int)client.GMTeamID;

            npc.SetHealth(100, 100);
            npc.Spawn(World, spawnPoint.Position, spawnPoint.Angles);
            client.SetControl(npc);

            if (Phase == GamePhase.None && players.Count(p => p.IsCharacter) == 1)
            {
                SetPhase(GamePhase.WarmUp);
                phaseTimer.SetInterval(Scenario.WarmUpDuration);
                phaseTimer.SetCallback(Fight);
                phaseTimer.Start();
            }
        }

        public virtual void SelectClass(ArenaClient client, int index)
        {
        }
        public virtual void OnSuicide(ArenaClient client)
        {
        }

        protected NPCInst SpawnEnemy(NPCClass enemy, Vec3f spawnPoint, float spawnRange = 100, int teamID = 1)
        {
            NPCInst npc = new NPCInst(NPCDef.Get(enemy.Definition));

            if (enemy.ItemDefs != null)
                foreach (var invItem in enemy.ItemDefs)
                {
                    var item = new ItemInst(ItemDef.Get(invItem.DefName));
                    item.SetAmount(invItem.Amount);
                    npc.Inventory.AddItem(item);
                    npc.EffectHandler.TryEquipItem(item);
                }

            if (enemy.Overlays != null)
                foreach (var overlay in enemy.Overlays)
                {
                    if (npc.ModelDef.TryGetOverlay(overlay, out ScriptOverlay ov))
                        npc.ModelInst.ApplyOverlay(ov);
                }

            npc.SetHealth(enemy.HP, enemy.HP);

            Vec3f spawnPos = Randomizer.GetVec3fRad(spawnPoint, spawnRange);
            Angles spawnAng = Randomizer.GetYaw();

            npc.TeamID = teamID;
            npc.BaseInst.SetNeedsClientGuide(true);
            npc.Spawn(World, spawnPos, spawnAng);
            return npc;
        }

        protected static AIAgent CreateAgent(float aggressionRad = 800)
        {
            var pers = new SimpleAIPersonality(aggressionRad, 1);
            var agent = new AIAgent(new List<VobInst>(), pers);
            AIManager.aiManagers[0].SubscribeAIAgent(agent);
            pers.Init(null, null);
            return agent;
        }
    }
}
