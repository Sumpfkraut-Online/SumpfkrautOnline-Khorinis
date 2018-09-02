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
        public ReadOnlyList<ArenaClient> Players { get { return players; } }        

        public static int NextScenarioIndex = 0;
        public static void StartNextScenario()
        {
            if (IsActive)
                ActiveMode.FadeOut();
            else
                InitScenario(GameScenario.Get(NextScenarioIndex));
        }

        public static GameMode InitScenario(GameScenario scenario)
        {
            if (scenario == null)
                return null;

            Log.Logger.Log("Init game scenario " + scenario.Name);

            if (++NextScenarioIndex >= GameScenario.Count)
                NextScenarioIndex = 0;

            var mode = scenario.GetMode();
            ActiveMode = mode;

            var world = new WorldInst(null)
            {
                Path = scenario.WorldPath
            };
            world.Create();

            if (scenario.WorldTimeScale > 0)
            {
                world.Clock.SetTime(scenario.WorldTime, scenario.WorldTimeScale);
                world.Clock.Start();
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

            mode.World = world;
            mode.Start(scenario);

            return mode;
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
            client.GMClass = null;
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

            phaseTimer.Stop();
        }

        protected virtual void Fight()
        {
            SetPhase(GamePhase.Fight);

            phaseTimer.SetInterval(Scenario.FightDuration);
            phaseTimer.SetCallback(FadeOut);
            phaseTimer.Restart();
        }

        protected virtual void FadeOut()
        {
            SetPhase(GamePhase.FadeOut);

            phaseTimer.SetInterval(FadeOutDuration);
            phaseTimer.SetCallback(End);
            phaseTimer.Restart();
        }
        
        protected virtual void End()
        {
            Log.Logger.Log("End");

            phaseTimer.Stop();

            // Reset game mode stats of players
            ArenaClient.ForEach(c =>
            {
                c.GMClass = null;
                c.GMKills = 0;
                c.GMScore = 0;
                c.GMDeaths = 0;
            });

            var oldWorld = this.World;
            var oldPlayers = new List<ArenaClient>(players);
            this.players.Clear();

            // initialize next scenario, creates a new world
            var newMode = InitScenario(GameScenario.Get(NextScenarioIndex));

            // move players to next scenario
            oldPlayers.ForEach(p => newMode.JoinAsSpectator(p));

            ClearAgents();
            // delete old world
            oldWorld.Delete();
        }

        protected void SetPhase(GamePhase phase)
        {
            this.Phase = phase;
            Log.Logger.Log("Set Phase " + phase);

            // send phase update to clients
            var stream = ArenaClient.GetStream(ScriptMessages.ModePhase);
            stream.Write((byte)Phase);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
        }

        protected NPCInst CreateNPC(NPCClass def, int teamID, CharCreationInfo cInfo = null)
        {
            if (def == null)
                return null;

            NPCInst npc;
            if (def.Definition == null)
            {
                if (cInfo == null)
                    cInfo = new CharCreationInfo(); // default one, should not happen anyway

                npc = new NPCInst(NPCDef.Get(cInfo.BodyMesh == HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"))
                {
                    UseCustoms = true,
                    CustomBodyTex = cInfo.BodyTex,
                    CustomHeadMesh = cInfo.HeadMesh,
                    CustomHeadTex = cInfo.HeadTex,
                    CustomVoice = cInfo.Voice,
                    CustomFatness = cInfo.Fatness,
                    CustomScale = new Vec3f(cInfo.BodyWidth, 1.0f, cInfo.BodyWidth),
                    CustomName = cInfo.Name
                };
            }
            else
            {
                npc = new NPCInst(NPCDef.Get(def.Definition));
            }

            // add inventory items
            if (def.ItemDefs != null)
                foreach (var invItem in def.ItemDefs)
                {
                    var item = new ItemInst(ItemDef.Get(invItem.DefName));
                    item.SetAmount(invItem.Amount);
                    npc.Inventory.AddItem(item);
                    npc.EffectHandler.TryEquipItem(item);
                }

            // add overlays
            if (def.Overlays != null)
                foreach (var overlay in def.Overlays)
                {
                    if (npc.ModelDef.TryGetOverlay(overlay, out ScriptOverlay ov))
                        npc.ModelInst.ApplyOverlay(ov);
                }

            npc.TeamID = teamID;

            npc.Protection = def.Protection;
            npc.Damage = def.Damage;
            npc.SetHealth(def.HP, def.HP);
            return npc;
        }

        protected void SpawnCharacter(ArenaClient client, Vec3f position, float range)
        {
            SpawnCharacter(client, new PosAng(Randomizer.GetVec3fRad(position, range), Randomizer.GetYaw()));
        }

        protected virtual void SpawnCharacter(ArenaClient client, PosAng spawnPoint)
        {
            // only spawn if player has joined the game mode and chosen a class
            if (client == null || !client.GMJoined || client.GMClass == null)
                return;

            // get rid of old character if there is one
            client.KillCharacter();
            
            NPCInst npc = CreateNPC(client.GMClass, (int)client.GMTeamID, client.CharInfo);
            npc.Spawn(World, spawnPoint.Position, spawnPoint.Angles);
            client.SetControl(npc);

            // start the warm up phase as soon as the first player joins
            if (Phase == GamePhase.None && players.Count(p => p.IsCharacter) == 1)
            {
                SetPhase(GamePhase.WarmUp);
                phaseTimer.SetInterval(Scenario.WarmUpDuration);
                phaseTimer.SetCallback(Fight);
                phaseTimer.Restart();
            }
        }

        public virtual void SelectClass(ArenaClient client, int index) { }
        public virtual void OnSuicide(ArenaClient client) { }

        protected NPCInst SpawnNPC(NPCClass classDef, Vec3f pos, float posOffset = 100, int teamID = 1, bool giveAI = true)
        {
            return SpawnNPC(classDef, pos, Randomizer.GetYaw(), posOffset, teamID, giveAI);
        }

        protected NPCInst SpawnNPC(NPCClass classDef, Vec3f pos, Angles ang, float posOffset = 100, int teamID = 1, bool giveAI = true)
        {
            NPCInst npc = CreateNPC(classDef, teamID);

            Vec3f spawnPos = posOffset > 0 ? Randomizer.GetVec3fRad(pos, posOffset) : pos;

            npc.BaseInst.SetNeedsClientGuide(giveAI);
            npc.Spawn(World, spawnPos, ang);
            return npc;
        }

        protected AIAgent CreateAgent(float aggressionRad = 800)
        {
            var pers = new SimpleAIPersonality(aggressionRad, 1);
            var agent = new AIAgent(new List<VobInst>(), pers);
            AIManager.aiManagers[0].SubscribeAIAgent(agent);
            agents.Add(agent);
            pers.Init(null, null);
            return agent;
        }

        List<AIAgent> agents = new List<AIAgent>(100);

        void ClearAgents()
        {
            var aiMan = AIManager.aiManagers[0];
            agents.ForEach(a => aiMan.UnsubscribeAIAgent(a));
            agents.Clear();
        }
    }
}
