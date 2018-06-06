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

        private TDMScoreBoard() : base(ScriptMessages.TDMScoreMessage)
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
            countdown.Texts[0].Text = new TimeSpan(TDMMode.ActiveMode.PhaseEndTime - now).ToString(@"mm\:ss");
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

            var list = new List<ScoreBoard.Entry>(10);

            int teamCount = stream.ReadByte();
            for (int t = 0; t < teamCount; t++)
            {
                if (t >= UsedCount)
                    return;

                list.Clear();
                int teamScore = stream.ReadShort();
                int count = stream.ReadByte();
                if (count > list.Capacity)
                    list.Capacity = count;

                for (int i = 0; i < count; i++)
                {
                    list.Add(new ScoreBoard.Entry()
                    {
                        ID = stream.ReadByte(),
                        Score = stream.ReadShort(),
                        Kills = stream.ReadShort(),
                        Deaths = stream.ReadShort(),
                        Ping = stream.ReadShort()
                    });
                }

                var board = GetBoard(t);
                board.Fill(list);
                board.SetTitle(string.Format("{0} ({1}/{2})", activeScenario.Teams[t].Name, teamScore, TDMMode.ScoreLimit));
            }
        }

        TDMScenario activeScenario;
        void UpdateScenario()
        {
            if (activeScenario == TDMMode.ActiveMode.Scenario)
                return;

            activeScenario = TDMMode.ActiveMode.Scenario;

            SetUsedCount(activeScenario.Teams.Length);
            for (int i = 0; i < UsedCount; i++)
            {
                var team = activeScenario.Teams[i];
                GetBoard(i).SetTitle(team.Name, team.Color);
            }
        }
    }

}
