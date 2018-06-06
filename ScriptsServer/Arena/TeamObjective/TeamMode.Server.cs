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
       
        
        public static void StartTO (TODef def)
        {
            if (def == null)
                return;

            if (activeTODef != null)
            {
                ArenaClient.ForEach(c =>
                {
                    ArenaClient client = (ArenaClient)c;
                    if (client.Team != null || client.BaseClient.SpecWorld == world.BaseWorld)
                        client.Spectate();

                    client.TODeaths = client.TOKills = client.TOScore = 0;
                });
                teams.Clear();

                var stream = ArenaClient.GetScriptMessageStream();
                stream.Write((byte)ScriptMessages.TOEnd);
                ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered));
            }

            world = WorldInst.List.Find(w => w.Path == def.WorldPath);
            if (world == null)
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
                        if (c.Character != null && !c.Character.IsDead && c.Character.TeamID != -1)
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
                        }
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

            ArenaClient.ForEach(c => 
            {
                ArenaClient client = (ArenaClient)c;
                if (client.Team != null || client.BaseClient.SpecWorld == world.BaseWorld)
                    client.Spectate();

                client.TODeaths = client.TOKills = client.TOScore = 0;
            });
            teams.Clear();

            activeTODef = null;
            world = null;

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
                client.TOClass = classDef;

            client.KillCharacter();
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
                //if (!teams.TrueForAll(t => team.Players.Count <= t.Players.Count - (t == client.Team ? 1 : 0)))
                //    return;

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
            client.TOClass = null;

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

            target.TODeaths++;
            if (attacker.Team == target.Team)
            {
                attacker.TOScore--;
                attacker.Team.Score--;
                attacker.SendPointsMessage(-1);
            }
            else
            {
                attacker.TOScore++;
                attacker.TOKills++;
                attacker.Team.Score++;
                attacker.SendPointsMessage(+1);
                if (attacker.Team.Score >= activeTODef.ScoreToWin)
                    PhaseFinish();

            }

        }
    }
}
