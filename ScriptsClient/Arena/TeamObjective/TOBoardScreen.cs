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

            int teamCount = stream.ReadByte();
            for (int t = 0; t < teamCount; t++)
            {
                if (t >= boards.Count)
                    return;
                var vis = boards[t];

                int count = stream.ReadByte();
                for (int i = 1; i <= count; i++)
                {
                    if (i >= vis.Texts.Count / 5)
                        return;

                    int id = stream.ReadByte();
                    int score = stream.ReadShort();
                    int kills = stream.ReadShort();
                    int deaths = stream.ReadShort();
                    int ping = stream.ReadShort();

                    SetText(vis.Texts[5 * i], PlayerInfo.TryGetInfo(id, out PlayerInfo pi) ? pi.Name : "!Unknown Player!", id);
                    SetText(vis.Texts[5 * i + 1], score, id);
                    SetText(vis.Texts[5 * i + 2], kills, id);
                    SetText(vis.Texts[5 * i + 3], deaths, id);
                    SetText(vis.Texts[5 * i + 4], ping, id);
                }

                for (int i = 5 * (count + 1); i < vis.Texts.Count; i++)
                    vis.Texts[i].Text = "";
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

                board.SetPosX((screenSize.Width - Width * teamCount) / 2 + i * Width);
                board.SetPosY(yScreenDist);
            }
        }
    }
}
