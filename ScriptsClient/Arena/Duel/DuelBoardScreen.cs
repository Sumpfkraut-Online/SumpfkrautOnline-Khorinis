using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena.Duel
{
    class DuelBoardScreen : ScoreBoardScreen
    {
        public static readonly DuelBoardScreen Instance = new DuelBoardScreen();

        ScoreBoard board;

        private DuelBoardScreen() : base(ScriptMessages.DuelScoreBoard)
        {
            SetBoardCount(1);
            this.board = Boards[0];
            this.board.SetTitle("Duellpunkte");
        }

        public override void ReadMessage(PacketReader stream)
        {
            this.board.Reset();

            // players
            this.board.ReadEntries(stream, false);

            // spectators
            this.board.ReadEntries(stream, true);
        }
    }
}
