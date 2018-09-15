using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Menus;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    partial class TDMMode
    {
        public override Action OpenJoinMenu { get { return MenuTeamSelect.Instance.Open; } }
        public override ScoreBoardScreen ScoreBoard { get { return TDMScoreBoard.Instance; } }

        public static TDMScenario.TeamDef HeroTeam { get { return (IsActive && PlayerInfo.HeroInfo.TeamID >= TeamIdent.GMPlayer) ? ActiveMode.Scenario.Teams[(int)PlayerInfo.HeroInfo.TeamID] : null; } }

        public static void HeroTeamChange()
        {
            if (PlayerInfo.HeroInfo.ID >= 0)
                MenuClassSelect.Instance.Open();
        }

        protected override void Start(GameScenario scenario)
        {
            base.Start(scenario);
            RespawnNote.Toggle(true);
        }

        protected override void End()
        {
            RespawnNote.Toggle(false);
            base.End();
        }

        public static void ReadWinMessage(PacketReader stream)
        {
            var teams = ActiveMode.Scenario.Teams;

            int winnerCount = stream.ReadByte();
            List<int> winners = new List<int>(winnerCount);
            for (int i = 0; i < winnerCount; i++)
                winners.Add(stream.ReadByte());

            if (winnerCount == teams.Length)
            {
                DoVictoryStuff(true, "UNENTSCHIEDEN!");
                return;
            }
            else
            {
                if (PlayerInfo.HeroInfo.TeamID >= TeamIdent.GMPlayer)
                    DoVictoryStuff(winners.Contains((int)PlayerInfo.HeroInfo.TeamID));

                if (winners.Count > 1)
                    foreach (int i in winners)
                        if (i < teams.Length)
                            ScreenScrollText.AddText(teams[i].Name + " hat gewonnen.", GUI.GUCView.Fonts.Menu, teams[i].Color, 20 * TimeSpan.TicksPerSecond);
            }
        }
    }
}
