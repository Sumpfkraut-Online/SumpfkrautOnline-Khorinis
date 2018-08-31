using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GUI;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    class HordeScoreBoard : ScoreBoardScreen
    {
        public static readonly HordeScoreBoard Instance = new HordeScoreBoard();
        
        ScoreBoard board;

        private HordeScoreBoard() : base(ScriptMessages.HordeScore)
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
            this.board.SetTitle(new TimeSpan(HordeMode.ActiveMode.PhaseEndTime - now).ToString(@"mm\:ss"));
        }

        void StopCountdown()
        {
            GUCScripts.OnUpdate -= UpdateCountdown;
        }

        public override void ReadMessage(PacketReader stream)
        {
            if (!HordeMode.IsActive)
                return;
            
        }
    }
}
