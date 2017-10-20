using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    class DuelBoardScreen : ScoreBoardScreen
    {
        public static readonly DuelBoardScreen Instance = new DuelBoardScreen();

        ScoreBoard board;

        private DuelBoardScreen() : base(ScriptMessages.ScoreDuelMessage)
        {
            SetUsedCount(1);
            this.board = GetBoard(0);
            this.board.SetTitle("Duellpunkte");
        }

        public override void ReadMessage(PacketReader stream)
        {
            int count = stream.ReadByte();
            List<ScoreBoard.Entry> list = new List<ScoreBoard.Entry>(count);
            for (int i = 0; i < count; i++)
                list.Add(new ScoreBoard.Entry()
                {
                    ID = stream.ReadByte(),
                    Score = stream.ReadShort(),
                    Kills = stream.ReadShort(),
                    Deaths = stream.ReadShort(),
                    Ping = stream.ReadShort()
                });

            board.Fill(list);
        }
    }
}
