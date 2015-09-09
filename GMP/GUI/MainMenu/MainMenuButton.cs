using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Client.GUI.MainMenu
{
    class MainMenuButton : MainMenuItem
    {
        GUCVisual vis;
        GUCVisualText visText;

        public MainMenuButton(string text, string help, int y, Action action)
            : this(text, help, 0, y, action)
        {
            visText.CenteredX = true;
        }

        public MainMenuButton(string text, string help, int x, int y, Action action)
        {
            HelpText = help;
            vis = GUCVisualText.Create(text, x, y);
            visText = vis.Texts[0];
            visText.Format = GUCVisualText.TextFormat.Center;
            vis.Font = GUCVisual.Fonts.Menu;
            OnActivate = action;
        }

        public string Text
        {
            get
            {
                return visText.Text;
            }
            set
            {
                visText.Text = value;
            }
        }

        public override void Show()
        {
            vis.Show();
        }

        public override void Hide()
        {
            vis.Hide();
        }

        public override void Select()
        {
            vis.Font = GUCVisual.Fonts.Menu_Hi;
        }

        public override void Deselect()
        {
            vis.Font = GUCVisual.Fonts.Menu;
        }

        public override bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                if (value)
                {
                    visText.SetColor(ColorRGBA.White);
                }
                else
                {
                    visText.SetColor(ColorRGBA.Grey);
                }
            }
        }
    }
}
