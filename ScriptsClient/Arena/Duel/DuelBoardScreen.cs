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
            vis.SetPosX((screenSize.Width - Width) / 2);
            vis.SetPosY(yScreenDist);
        }

        public override void ReadMessage(PacketReader stream)
        {
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
