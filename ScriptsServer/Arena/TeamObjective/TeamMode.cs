using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Utilities;

namespace GUC.Scripts.Arena
{
    static class TeamMode
    {
        public const int MinClientsToStart = 1;
        const long WarmUpDuration = 30 * TimeSpan.TicksPerSecond;
        const long FinishDuration = 30 * TimeSpan.TicksPerSecond;

        static List<TOTeamInst> teams = new List<TOTeamInst>(3);
        public static ReadOnlyList<TOTeamInst> Teams { get { return teams; } }

        static GUCTimer timer = new GUCTimer();

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
            PhaseWarmup();
        }

        static void PhaseWarmup()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOWarmup);
            stream.Write(activeTODef.Name);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));

            timer.SetInterval(WarmUpDuration);
            timer.SetCallback(PhaseStart);
            timer.Start();
        }

        static void PhaseStart()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOStart);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));

            timer.SetInterval(activeTODef.Duration * TimeSpan.TicksPerMinute);
            timer.SetCallback(PhaseFinish);
        }

        static void PhaseFinish()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOFinish);
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));

            timer.SetInterval(FinishDuration);
            timer.SetCallback(EndTO);
        }

        static void EndTO()
        {
            teams.ForEach(team => team.Reset());
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
            ArenaClient.ForEach(c => c.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));
        }

        public static void ChooseClass(ArenaClient client, TOClassDef classDef)
        {

        }
    }
}
