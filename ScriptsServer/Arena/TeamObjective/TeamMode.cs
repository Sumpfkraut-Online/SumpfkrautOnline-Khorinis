using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Utilities;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Arena
{
    static class TeamMode
    {
        public const int MinClientsToStart = 1;
        const long WarmUpDuration = 30 * TimeSpan.TicksPerSecond;
        const long FinishDuration = 30 * TimeSpan.TicksPerSecond;
        const long RespawnInterval = 10 * TimeSpan.TicksPerSecond;

        static List<TOTeamInst> teams = new List<TOTeamInst>(3);
        public static ReadOnlyList<TOTeamInst> Teams { get { return teams; } }

        static TOPhases phase;
        public static TOPhases Phase { get { return phase; } }
        static GUCTimer phaseTimer = new GUCTimer();
        static GUCTimer respawnTimer = new GUCTimer(RespawnInterval, RespawnPlayers);

        static void RespawnPlayers()
        {
            foreach (var team in teams)
                foreach (var player in team.Players)
                {
                    if (player.Character == null || player.Character.IsDead)
                    {
                        SpawnCharacter(player);
                    }
                }
        }

        static void SpawnCharacter(ArenaClient player)
        {
            if (player.Team == null || player.ClassDef == null)
                return;

            var classDef = player.ClassDef;
            NPCInst npc;
            if (classDef.NPCDef == null)
            {
                var charInfo = player.CharInfo;
                npc = new NPCInst(NPCDef.Get(charInfo.BodyMesh == HumBodyMeshs.HUM_BODY_NAKED0 ? "maleplayer" : "femaleplayer"));
                npc.UseCustoms = true;
                npc.CustomBodyTex = charInfo.BodyTex;
                npc.CustomHeadMesh = charInfo.HeadMesh;
                npc.CustomHeadTex = charInfo.HeadTex;
                npc.CustomVoice = charInfo.Voice;
                npc.CustomFatness = charInfo.Fatness;
                npc.CustomScale = new Vec3f(charInfo.BodyWidth, 1.0f, charInfo.BodyWidth);
                npc.CustomName = charInfo.Name;
            }
            else
            {
                npc = new NPCInst(NPCDef.Get(classDef.NPCDef));
            }

            foreach (var eqPair in classDef.ItemDefs)
            {
                var item = new ItemInst(ItemDef.Get(eqPair.Item1));
                item.SetAmount(eqPair.Item2);
                npc.Inventory.AddItem(item);
                npc.EquipItem(item);
            }

            foreach (var overlay in classDef.Overlays)
            {
                ScriptOverlay ov;
                if (npc.ModelDef.TryGetOverlay(overlay, out ov))
                    npc.ModelInst.ApplyOverlay(ov);
            }

            var spawnPoint = player.Team.GetSpawnPoint();
            npc.Spawn(WorldInst.Current, spawnPoint.Item1, spawnPoint.Item2);
            player.SetControl(npc);
        }

        public static bool CheckStartTO()
        {
            if (activeTODef != null)
                return false;

            if (ArenaClient.GetCount() < MinClientsToStart)
                return false;

            StartNextTO();
            return true;
        }

        static int toLoopIndex = 0;
        public static void StartNextTO()
        {
            var todef = TODef.GetAll().ElementAtOrDefault(toLoopIndex++);

            if (toLoopIndex >= TODef.GetAll().Count())
                toLoopIndex = 0;

            StartTO(todef);
        }

        public static void StartTO(string name)
        {
            StartTO(TODef.TryGet(name));
        }

        static TODef activeTODef;
        public static bool IsRunning { get { return activeTODef != null; } }
        public static void StartTO(TODef def)
        {
            if (def == null)
                return;

            activeTODef = def;
            foreach (var teamDef in activeTODef.Teams)
                teams.Add(new TOTeamInst(teamDef));

            PhaseWarmup();
        }

        static void PhaseWarmup()
        {
            phase = TOPhases.Warmup;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOWarmup);
            stream.Write(activeTODef.Name);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));

            phaseTimer.SetInterval(WarmUpDuration);
            phaseTimer.SetCallback(PhaseStart);
            phaseTimer.Start();

            respawnTimer.Start();
        }

        static void PhaseStart()
        {
            phase = TOPhases.Battle;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOStart);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));

            phaseTimer.SetInterval(activeTODef.Duration * TimeSpan.TicksPerMinute);
            phaseTimer.SetCallback(PhaseFinish);
        }

        static void PhaseFinish()
        {
            phase = TOPhases.Finish;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOFinish);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));

            phaseTimer.SetInterval(FinishDuration);
            phaseTimer.SetCallback(EndTO);
            
            respawnTimer.Stop();
        }

        static void EndTO()
        {
            phase = TOPhases.None;

            teams.ForEach(team => team.Players.ForEach(p => p.Team = null));
            teams.Clear();

            activeTODef = null;
            if (!CheckStartTO())
            {
                var stream = ArenaClient.GetScriptMessageStream();
                stream.Write((byte)ScriptMessages.TOEnd);
                ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));
            }
        }

        public static void JoinTeam(ArenaClient client, TOTeamInst team)
        {
            if (team == null || activeTODef == null || client.Team == team)
                return;

            int index = teams.IndexOf(team);
            if (index < 0)
                return; // team is not from this TO

            // don't join a team which has already more players than the others
            if (!teams.TrueForAll(t => team.Players.Count <= t.Players.Count - (t == client.Team ? 1 : 0)))
                return;

            if (client.Team != null)
                client.Team.Players.Remove(client);

            client.Team = team;
            team.Players.Add(client);

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOJoinTeam);
            stream.Write((byte)index);
            client.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }
    }
}
