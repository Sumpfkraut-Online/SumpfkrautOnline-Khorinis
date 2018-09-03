using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    class HordeScoreBoard : ScoreBoardScreen
    {
        public static readonly HordeScoreBoard Instance = new HordeScoreBoard();
        
        ScoreBoard board;

        private HordeScoreBoard() : base(ScriptMessages.HordeScoreBoard)
        {
            SetBoardCount(1);
            this.board = Boards[0];
            
            OnOpen += StartCountdown;
            OnClose += StopCountdown;
        }

        void StartCountdown()
        {
            GUCScripts.OnUpdate += UpdateCountdown;
        }

        void UpdateCountdown(long now)
        {
            this.board.SetTitle(HordeMode.ActiveMode.Phase == GamePhase.None ? "" : new TimeSpan(HordeMode.ActiveMode.PhaseEndTime - now).ToString(@"mm\:ss"));
        }

        void StopCountdown()
        {
            GUCScripts.OnUpdate -= UpdateCountdown;
        }

        public override void ReadMessage(PacketReader stream)
        {
            if (!HordeMode.IsActive)
                return;

            this.board.Reset();

            // players
            this.board.ReadEntries(stream, false);

            // spectators
            this.board.ReadEntries(stream, true);
        }
    }
}
