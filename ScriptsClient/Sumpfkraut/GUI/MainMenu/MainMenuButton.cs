using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.GUI;

namespace GUC.Scripts.Sumpfkraut.GUI.MainMenu
{
    class MainMenuButton : MainMenuItem
    {
        GUCVisual vis;
        GUCVisualText visText;

        public MainMenuButton(string text, string help, int y, Action action, Fonts font = Fonts.Menu)
            : this(text, help, 0, y, action, font)
        {
            visText.CenteredX = true;
        }

        public MainMenuButton(string text, string help, int x, int y, Action action, Fonts font = Fonts.Menu)
        {
            HelpText = help;
            vis = GUCVisualText.Create(text, x, y);
            visText = vis.Texts[0];
            visText.Format = GUCVisualText.TextFormat.Center;
            vis.Font = font;
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
            if (vis.Font == Fonts.Menu)
                vis.Font = Fonts.Menu_Hi;
            else if (vis.Font == Fonts.Default)
                vis.Font = Fonts.Default_Hi;
        }

        public override void Deselect()
        {
            if (vis.Font == Fonts.Menu_Hi)
                vis.Font = Fonts.Menu;
            else if (vis.Font == Fonts.Default_Hi)
                vis.Font = Fonts.Default;
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
