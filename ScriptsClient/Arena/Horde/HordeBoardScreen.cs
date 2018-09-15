using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    class HordeBoardScreen : ScoreBoardScreen
    {
        public static readonly HordeBoardScreen Instance = new HordeBoardScreen();

        ScoreBoard board;

        private HordeBoardScreen() : base(ScriptMessages.ScoreHordeMessage)
        {
            SetUsedCount(1);
            this.board = GetBoard(0);
            this.board.SetTitle("Horde-Modus");
            HordeMode.OnPhaseChange += SetTitle;
        }

        void SetTitle(HordePhase phase)
        {
            this.board.SetTitle(phase.ToString());
        }
        
        public override void ReadMessage(PacketReader stream)
        {
            SetTitle(HordeMode.Phase);

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
