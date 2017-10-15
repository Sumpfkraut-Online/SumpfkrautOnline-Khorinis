using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GUI;

namespace GUC.Scripts.Arena
{
    class DuelBoardScreen : ScoreBoardScreen
    {
        public static readonly DuelBoardScreen Instance = new DuelBoardScreen();

        GUCVisual vis;

        private DuelBoardScreen() : base(ScriptMessages.ScoreDuelMessage, "Duellpunkte")
        {
            vis = CreateBoard();

            var screenSize = GUCView.GetScreenSize();
            vis.SetPosX((screenSize.X - Width) / 2);
            vis.SetPosY(yScreenDist);
        }

        public override void ReadMessage(PacketReader stream)
        {
            int count = stream.ReadByte();
            List<BoardEntry> list = new List<BoardEntry>(count);
            for (int i = 0; i < count; i++)
                list.Add(new BoardEntry()
                {
                    ID = stream.ReadByte(),
                    Score = stream.ReadShort(),
                    Kills = stream.ReadShort(),
                    Deaths = stream.ReadShort(),
                    Ping = stream.ReadShort()
                });

            FillBoard(vis, list);
        }

        protected override void HideBoard()
        {
            vis.Hide();
        }

        protected override void ShowBoard()
        {
            vis.Show();
        }
    }
}
