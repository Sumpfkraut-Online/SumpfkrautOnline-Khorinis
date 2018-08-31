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
