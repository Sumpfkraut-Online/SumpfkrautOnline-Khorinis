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
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static partial class TeamMode
    {
        public const int MinClientsToStart = 1;

        static List<TOTeamInst> teams = new List<TOTeamInst>(3);
        public static ReadOnlyList<TOTeamInst> Teams { get { return teams; } }

        static GUCTimer phaseTimer = new GUCTimer();

        static WorldInst World = null;

        public static uint RemainingPhaseMsec { get { return (uint)(phaseTimer.GetRemainingTicks() / TimeSpan.TicksPerMillisecond); } }

        public static void AddScore(ArenaClient client)
        {
            if (client == null || client.Team == null)
                return;

            var team = client.Team;
            team.Score++;
            if (team.Score >= activeTODef.ScoreToWin)
            {
                PhaseFinish();
            }
        }

        public static void SpawnCharacter(ArenaClient player)
        {
            if (player.Team == null || player.ClassDef == null)
                return;

            player.KillCharacter();

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

            npc.TeamPlayer = true;

            var spawnPoint = player.Team.GetSpawnPoint();
            npc.Spawn(World, spawnPoint.Item1, spawnPoint.Item2);
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

        public static void StartTO(TODef def)
        {
            if (def == null)
                return;

            World = WorldInst.List.Find(w => w.Path == def.WorldPath);
            if (World == null)
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
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));

            phaseTimer.SetInterval(WarmUpDuration);
            phaseTimer.SetCallback(PhaseStart);
            phaseTimer.Start();

            Log.Logger.Log(phase.ToString());
        }

        static void PhaseStart()
        {
            phase = TOPhases.Battle;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOStart);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));

            phaseTimer.SetInterval(activeTODef.Duration * TimeSpan.TicksPerMinute);
            phaseTimer.SetCallback(PhaseFinish);
            Log.Logger.Log(phase.ToString());

            foreach (var team in teams)
                foreach (var player in team.Players)
                    if (player.Character == null || player.Character.IsDead)
                        SpawnCharacter(player);
        }

        static void PhaseFinish()
        {
            phase = TOPhases.Finish;

            // first, check which teams beat the score limit
            List<int> winIndices = new List<int>(teams.Count);
            for (int i = 0; i < teams.Count; i++)
                if (teams[i].Score >= activeTODef.ScoreToWin)
                    winIndices.Add(i);

            // no teams beat the score limit? Select highest scores.
            if (winIndices.Count == 0)
            {
                int max = teams.Max(t => t.Score);
                for (int i = 0; i < teams.Count; i++)
                    if (teams[i].Score >= max)
                        winIndices.Add(i);
            }

            for (int i = 0; i < teams.Count; i++)
                if (!winIndices.Contains(i))
                    teams[i].Players.ForEach(c =>
                    {
                        var inv = c.Character.Inventory;
                        inv.ForEachItem(item =>
                        {
                            if (item.IsWeapon)
                            {
                                c.Character.UnequipItem(item);
                                inv.RemoveItem(item);
                            }
                        });
                    });

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOFinish);
            stream.Write((byte)winIndices.Count); // write the winners
            winIndices.ForEach(i => stream.Write((byte)i));
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));

            phaseTimer.SetInterval(FinishDuration);
            phaseTimer.SetCallback(EndTO);

            Log.Logger.Log(phase.ToString());
        }

        static void EndTO()
        {
            phase = TOPhases.None;

            ArenaClient.ForEach(c => { if (((ArenaClient)c).Team != null) ((ArenaClient)c).Spectate(); });
            teams.Clear();

            activeTODef = null;
            World = null;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOEnd);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));

            CheckStartTO();
        }
        public static void ReadSelectClass(ArenaClient client, PacketReader stream)
        {
            if (!IsRunning || client.Team == null || Phase == TOPhases.None || Phase == TOPhases.Finish)
                return;

            int index = stream.ReadByte();
            var classDef = client.Team.Def.ClassDefs.ElementAtOrDefault(index);
            if (classDef != null)
                client.ClassDef = classDef;

            if (Phase == TOPhases.Warmup)
                SpawnCharacter(client);
        }

        public static void ReadJoinTeam(ArenaClient client, PacketReader stream)
        {
            if (!IsRunning)
                return;

            int index = stream.ReadByte();
            var team = Teams.ElementAtOrDefault(index);
            JoinTeam(client, team);
        }

        public static Action OnChangeTeamComposition;
        public static void JoinTeam(ArenaClient client, TOTeamInst team)
        {
            if (client.Team == team)
                return;

            if (team != null && (!IsRunning || Phase == TOPhases.None || Phase == TOPhases.Finish))
                return;

            client.KillCharacter();

            int index = teams.IndexOf(team);
            if (index >= 0)
            {
                // don't join a team which has already more players than the others
                if (!teams.TrueForAll(t => team.Players.Count <= t.Players.Count - (t == client.Team ? 1 : 0)))
                    return;

                if (client.Team != null)
                    client.Team.Players.Remove(client);

                client.Team = team;
                team.Players.Add(client);
            }
            else if (client.Team != null)
            {
                client.Team.Players.Remove(client);
                client.Team = null;
            }
            client.ClassDef = null;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOJoinTeam);
            stream.Write((sbyte)index);
            client.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);

            SpawnCharacter(client);

            OnChangeTeamComposition?.Invoke();
        }

        public static void WriteGameInfo(PacketWriter stream)
        {
            stream.Write((byte)TeamMode.phase);
            if (TeamMode.Phase != TOPhases.None)
            {
                stream.Write(TeamMode.ActiveTODef.Name);
                stream.Write(TeamMode.RemainingPhaseMsec);
            }
        }

        public static void Kill(ArenaClient attacker, ArenaClient target)
        {
            if (Phase != TOPhases.Battle)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.PointsMessage);
            attacker.SendScriptMessage(stream, NetPriority.Low, NetReliability.Unreliable);

            attacker.TOScore++;
            attacker.TOKills++;
            target.TODeaths++;
            attacker.Team.Score++;
            if (attacker.Team.Score >= activeTODef.ScoreToWin)
                PhaseFinish();
        }
    }
}
