using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GUI;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    class TDMScoreBoard : ScoreBoardScreen
    {
        public static readonly TDMScoreBoard Instance = new TDMScoreBoard();

        GUCVisual countdown;

        private TDMScoreBoard() : base(ScriptMessages.TDMScoreBoard)
        {
            OnOpen += UpdateScenario;


            countdown = new GUCVisual();
            var textVis = countdown.CreateTextCenterX("", 200);
            textVis.Font = GUCView.Fonts.Menu_Hi;

            OnOpen += StartCountdown;
            OnClose += StopCountdown;
        }

        void StartCountdown()
        {
            GUCScripts.OnUpdate += UpdateCountdown;
            countdown.Show();
        }

        void UpdateCountdown(long now)
        {
            countdown.Texts[0].Text = TDMMode.ActiveMode.Phase == GamePhase.None ? "" : new TimeSpan(TDMMode.ActiveMode.PhaseEndTime - now).ToString(@"mm\:ss");
        }

        void StopCountdown()
        {
            GUCScripts.OnUpdate -= UpdateCountdown;
            countdown.Hide();
        }

        public override void ReadMessage(PacketReader stream)
        {
            if (!TDMMode.IsActive)
                return;

            UpdateScenario();

            int teamCount = stream.ReadByte();
            for (int t = 0; t < teamCount; t++)
            {
                if (t >= BoardCount)
                    return;

                ScoreBoard board = Boards[t];
                board.Reset();

                int teamScore = stream.ReadShort();
                board.SetTitle(string.Format("{0} ({1}/{2})", activeScenario.Teams[t].Name, teamScore, TDMMode.ScoreLimit));

                board.ReadEntries(stream, false);
            }

            int spectators = stream.ReadByte();
            for (int i = 0; i < spectators; i++)
            {
                var lowest = Boards.Aggregate((c1, c2) => c1.EntryCount < c2.EntryCount ? c1 : c2);
                lowest.AddEntry(new ScoreBoard.Entry(stream), true);
            }
        }

        TDMScenario activeScenario;
        void UpdateScenario()
        {
            if (activeScenario == TDMMode.ActiveMode.Scenario)
                return;

            activeScenario = TDMMode.ActiveMode.Scenario;

            SetBoardCount(activeScenario.Teams.Length);
            for (int i = 0; i < BoardCount; i++)
            {
                var team = activeScenario.Teams[i];
                Boards[i].SetTitle(team.Name, team.Color);
            }
        }
    }

}
