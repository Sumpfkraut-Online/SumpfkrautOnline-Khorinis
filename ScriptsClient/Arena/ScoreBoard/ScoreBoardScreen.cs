using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Network;
using GUC.Scripting;
using GUC.Utilities;

namespace GUC.Scripts.Arena
{
    abstract class ScoreBoardScreen : GUCMenu
    {
        const long MinOpenDuration = 750 * TimeSpan.TicksPerMillisecond;

        GUCTimer closeTimer;
        ScriptMessages msgID;
        List<ScoreBoard> boards = new List<ScoreBoard>();
        protected ReadOnlyList<ScoreBoard> Boards { get { return boards; } }

        int boardCount;
        public int BoardCount { get { return boardCount; } }
        protected void SetBoardCount(int num)
        {
            if (this.boardCount == num)
                return;

            this.boardCount = num;
            UpdateBoardPositions();
            for (int i = boardCount; i < boards.Count; i++)
                boards[i].Hide();
        }

        void UpdateBoardPositions()
        {
            var screenSize = GUCView.GetScreenSize();
            for (int i = 0; i < boardCount; i++)
            {
                ScoreBoard board;
                if (i >= boards.Count)
                {
                    board = new ScoreBoard();
                    boards.Add(board);
                }
                else
                {
                    board = boards[i];
                }
                board.SetPos((screenSize.X - ScoreBoard.Width * boardCount) / 2 + i * ScoreBoard.Width, ScoreBoard.YDistance);
            }
        }

        protected ScoreBoardScreen(ScriptMessages messageID)
        {
            this.msgID = messageID;
            this.closeTimer = new GUCTimer(DoClose);
        }

        bool shown = false;
        public bool Shown { get { return shown; } }

        public event Action OnOpen;
        public override void Open()
        {
            if (shown)
                return;

            SendToggleMessage(true);
            OnOpen?.Invoke();
            for (int i = 0; i < boardCount; i++)
                boards[i].Show();
            openTime = GameTime.Ticks;
            closeTimer.Stop();
            shown = true;
        }

        long openTime;
        public override void Close()
        {
            if (!shown)
                return;

            long diff = GameTime.Ticks - openTime;
            if (diff > MinOpenDuration)
            {
                DoClose();
            }
            else
            {
                closeTimer.Stop();
                closeTimer.SetInterval(MinOpenDuration - diff);
                closeTimer.Start();
            }
        }

        public event Action OnClose;
        void DoClose()
        {
            SendToggleMessage(false);
            boards.ForEach(b => b.Hide());
            OnClose?.Invoke();
            closeTimer.Stop();
            shown = false;
        }

        void SendToggleMessage(bool open)
        {
            var stream = ArenaClient.GetStream(msgID);
            stream.Write(open);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        public abstract void ReadMessage(PacketReader stream);
    }
}