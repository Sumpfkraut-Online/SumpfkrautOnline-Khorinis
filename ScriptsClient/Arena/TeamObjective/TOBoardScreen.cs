using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GUI;

namespace GUC.Scripts.Arena
{
    class TOBoardScreen : ScoreBoardScreen
    {
        public static readonly TOBoardScreen Instance = new TOBoardScreen();

        List<GUCVisual> boards = new List<GUCVisual>(3);

        private TOBoardScreen() : base(ScriptMessages.ScoreTOMessage, "Team Objective")
        {
        }

        public override void ReadMessage(PacketReader stream)
        {
            if (TeamMode.ActiveTODef == null)
                return;

            UpdateBoards();

            List<BoardEntry> list = new List<BoardEntry>();

            int teamCount = stream.ReadByte();
            for (int t = 0; t < teamCount; t++)
            {
                if (t >= boards.Count)
                    return;

                int count = stream.ReadByte();
                if (count > list.Capacity)
                    list.Capacity = count;

                for (int i = 0; i < count; i++)
                    list.Add(new BoardEntry()
                    {
                        ID = stream.ReadByte(),
                        Score = stream.ReadShort(),
                        Kills = stream.ReadShort(),
                        Deaths = stream.ReadShort(),
                        Ping = stream.ReadShort()
                    });

                FillBoard(boards[t], list);
                list.Clear();
            }
        }

        protected override void HideBoard()
        {
            boards.ForEach(b => b.Hide());
        }

        protected override void ShowBoard()
        {
            if (TeamMode.ActiveTODef == null)
                return;

            UpdateBoards();
            for (int i = 0; i < activeDef.Teams.Count; i++)
                boards[i].Show();
        }

        TODef activeDef = null;
        void UpdateBoards()
        {
            if (activeDef == TeamMode.ActiveTODef)
                return;
            activeDef = TeamMode.ActiveTODef;

            var screenSize = GUCView.GetScreenSize();
            int teamCount = activeDef.Teams.Count;
            for (int i = 0; i < teamCount; i++)
            {
                GUCVisual board;
                if (i >= boards.Count)
                {
                    board = CreateBoard();
                    boards.Add(board);
                }
                else
                {
                    board = boards[i];
                }

                board.SetPosX((screenSize.X - Width * teamCount) / 2 + i * Width);
                board.SetPosY(yScreenDist);
            }
        }
    }
}
