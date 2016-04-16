using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;
using GUC.Network;
using GUC.Scripts.TFFA;
using GUC.Scripting;

namespace GUC.Client.Scripts.TFFA
{
    class PhaseInfo
    {
        public static readonly PhaseInfo info = new PhaseInfo();

        GUCVisual back;

        public PhaseInfo()
        {
            var res = GUCView.GetScreenSize();

            back = new GUCVisual();

            back.CreateTextCenterX("", res[1] / 2 - 320);
            back.Font = GUCView.Fonts.Menu;
            back.Texts[0].Format = GUCVisualText.TextFormat.Center;
            back.Texts[0].SetColor(Types.ColorRGBA.Green);
        }

        public void SetState(TFFAPhase state)
        {
            this.back.Texts[0].Text = state.ToString().ToUpper();
        }

        public void Open()
        {
            back.Show();
        }

        public void Close()
        {
            back.Hide();
        }
    }
}
