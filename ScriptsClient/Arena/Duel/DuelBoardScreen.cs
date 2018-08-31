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

        private DuelBoardScreen() : base(ScriptMessages.DuelScore)
        {
            SetBoardCount(1);
            this.board = Boards[0];
            this.board.SetTitle("Duellpunkte");
        }

        public override void ReadMessage(PacketReader stream)
        {
            board.Reset();

            // players
            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                board.AddEntry(new ScoreBoard.Entry()
                {
                    ID = stream.ReadByte(),
                    Score = stream.ReadShort(),
                    Kills = stream.ReadShort(),
                    Deaths = stream.ReadShort(),
                    Ping = stream.ReadShort()
                }, false);
            }

            // spectators
            count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                board.AddEntry(new ScoreBoard.Entry()
                {
                    ID = stream.ReadByte(),
                    Score = stream.ReadShort(),
                    Kills = stream.ReadShort(),
                    Deaths = stream.ReadShort(),
                    Ping = stream.ReadShort()
                }, true);
            }
        }
    }
}
