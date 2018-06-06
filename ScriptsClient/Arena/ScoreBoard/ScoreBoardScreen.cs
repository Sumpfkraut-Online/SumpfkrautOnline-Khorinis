using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Network;
using GUC.Scripting;

namespace GUC.Scripts.Arena
{
    abstract class ScoreBoardScreen : GUCMenu
    {
        const long MinOpenDuration = 750 * TimeSpan.TicksPerMillisecond;

        GUCTimer closeTimer;
        ScriptMessages msgID;
        List<ScoreBoard> boards;
        protected ScoreBoard GetBoard(int index)
        {
            return index >= usedCount ? null : boards[index];
        }

        int usedCount;
        protected int UsedCount { get { return usedCount; } }
        protected void SetUsedCount(int value)
        {
            if (this.usedCount == value)
                return;

            this.usedCount = value;
            UpdateBoardPositions();
            for (int i = usedCount; i < boards.Count; i++)
                boards[i].Hide();
        }

        void UpdateBoardPositions()
        {
            var screenSize = GUCView.GetScreenSize();
            for (int i = 0; i < usedCount; i++)
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
                board.SetPos((screenSize.X - ScoreBoard.Width * usedCount) / 2 + i * ScoreBoard.Width, ScoreBoard.YDistance);
            }
        }

        protected ScoreBoardScreen(ScriptMessages messageID)
        {
            this.msgID = messageID;
            this.closeTimer = new GUCTimer(DoClose);
            this.boards = new List<ScoreBoard>();
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
            for (int i = 0; i < usedCount; i++)
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