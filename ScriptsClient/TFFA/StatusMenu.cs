using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;
using GUC.Scripts.TFFA;

namespace GUC.Client.Scripts.TFFA
{
    class StatusMenu
    {
        public static readonly StatusMenu Menu = new StatusMenu();

        bool statusShow = false;
        bool scoreShow = false;
        public bool StatusShow
        {
            get { return this.statusShow; }
            set
            {
                this.statusShow = value;
                UpdateShow();
            }
        }
        public bool ScoreShow
        {
            get { return this.scoreShow; }
            set
            {
                this.scoreShow = value;
                UpdateShow();
            }
        }

        void UpdateShow()
        {
            if (statusShow || scoreShow)
            {
                if (TFFAClient.Status == TFFAPhase.Waiting)
                {
                    status.Text = "WARTEN...";
                }
                else if (TFFAClient.Status == TFFAPhase.Fight)
                {
                    status.Text = "";
                }
                else if (TFFAClient.Status == TFFAPhase.End)
                {
                    status.Text = "ENDE";
                }

                if (TFFAClient.Winner == Team.AL)
                {
                    this.winner.Text = "!!! TEAM GOMEZ HAT GEWONNEN !!!";
                }
                else if (TFFAClient.Winner == Team.NL)
                {
                    this.winner.Text = "!!! TETRIANDOCH HAT GEWONNEN !!!";
                }
                else
                {
                    this.winner.Text = "";
                }

                back.Show();
            }
            else
            {
                back.Hide();
            }
        }

        GUCVisual back;

        GUCVisualText status;
        GUCVisualText winner;

        public StatusMenu()
        {
            var res = GUCView.GetScreenSize();

            back = new GUCVisual();
            back.Font = GUCView.Fonts.Menu;

            status = back.CreateTextCenterX("", res[1] / 2 - 320);
            status.Format = GUCVisualText.TextFormat.Center;
            status.SetColor(Types.ColorRGBA.Green);

            winner = back.CreateTextCenterX("", res[1] / 2 - 290);
            winner.Format = GUCVisualText.TextFormat.Center;
        }
    }
}
