using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    partial class TDMMode
    {
        const long MenuIdleDuration = 120 * TimeSpan.TicksPerSecond;
        const long RespawnInterval = 10 * TimeSpan.TicksPerSecond;

        List<TDMTeamInst> teams = new List<TDMTeamInst>(2);
        public List<TDMTeamInst> Teams { get { return teams; } }

        #region Respawn

        GUCTimer respawnTimer = new GUCTimer(RespawnInterval);

        void RespawnWave()
        {
            foreach (var team in teams)
                foreach (var player in team.Players)
                {
                    if (player.Character != null && player.Character.IsDead)
                        SpawnCharacter(player, World, team.GetSpawnPoint());
                }
        }

        #endregion

        protected override void Start(GameScenario scenario)
        {
            if (!(scenario is TDMScenario))
                throw new ArgumentException("Scenario is no TDMScenario!");

            base.Start(scenario);

            TDMScenario tdmScen = (TDMScenario)scenario;
            foreach (var teamDef in tdmScen.Teams)
            {
                teams.Add(new TDMTeamInst() { Definition = teamDef });
            }

            respawnTimer.SetCallback(RespawnWave);
            respawnTimer.Start();
            NPCInst.sOnHit += OnHit;
        }

        public override void OnSuicide(ArenaClient client)
        {
            if (Phase < GamePhase.Fight)
                return;

            client.GMScore--;
            client.GMDeaths++;
            client.TDMTeam.Score--;
        }

        static void OnHit(NPCInst attacker, NPCInst target, int damage)
        {
            if (!IsActive || ActiveMode.Phase < GamePhase.Fight)
                return;

            if (target.HP <= 0 
                && attacker.Client is ArenaClient attClient && target.Client is ArenaClient tarClient
                && attClient.GMTeamID >= 0 && tarClient.GMTeamID >= 0)
            {
                if (attClient.GMTeamID != tarClient.GMTeamID)
                {
                    attClient.GMScore++;
                    attClient.GMKills++;
                    attClient.TDMTeam.Score++;
                    attClient.SendPointsMessage(+1);

                    tarClient.GMDeaths++;

                    if (attClient.TDMTeam.Score >= ScoreLimit)
                    {
                        ActiveMode.FadeOut();
                    }
                }
                else // teamkill
                {
                    attClient.GMScore--;
                    attClient.TDMTeam.Score--;
                    attClient.SendPointsMessage(-1);

                    tarClient.GMDeaths++;
                }
            }
        }

        #region Phases
        
        protected override void FadeOut()
        {
            int max = teams.Max(t => t.Score);
            int count = teams.Count(t => t.Score == max);
            var stream = ArenaClient.GetStream(ScriptMessages.TDMWin);
            stream.Write((byte)count);
            for (byte i = 0; i < teams.Count; i++)
                if (teams[i].Score == max)
                    stream.Write(i);

            players.ForEach(p => p.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable));

            base.FadeOut();

            respawnTimer.Stop();
            NPCInst.sOnHit -= OnHit;
        }

        protected override void End()
        {
            teams.ForEach(t => t.Players.ForEach(c => c.TDMTeam = null));
            teams.Clear();
            respawnTimer.Stop();
            NPCInst.sOnHit -= OnHit;
            TDMScoreBoard.Instance.RemoveAll();
            base.End();
        }

        #endregion

        #region Join, Leave & Class selection

        public void JoinTeam(ArenaClient client, int index)
        {
            if (Phase == GamePhase.FadeOut)
                return;

            if (index < 0 || index >= teams.Count)
                return;
            
            var team = teams[index];
            if (client.TDMTeam == team)
                return;

            if (client.TDMTeam != null)
            {
                client.KillCharacter();
                client.TDMTeam.Players.Remove(client);
                client.GMClass = null;
            }

            client.TDMTeam = team;
            team.Players.Add(client);
            client.SetTeamID((TeamIdent)index);
        }

        public override void SelectClass(ArenaClient client, int index)
        {
            if (client.TDMTeam == null || client.GMTeamID < 0)
                return;

            if (!client.TDMTeam.Definition.Classes.TryGet(index, out NPCClass pc))
                return;

            client.GMClass = pc;

            if (client.Character == null || Phase == GamePhase.WarmUp)
                SpawnCharacter(client, World, client.TDMTeam.GetSpawnPoint());
        }

        public override bool Leave(ArenaClient client)
        {
            if (!base.Leave(client))
                return false;

            if (client.TDMTeam != null)
            {
                client.TDMTeam.Players.Remove(client);
                client.TDMTeam = null;
            }
            return true;
        }

        #endregion
    }
}
