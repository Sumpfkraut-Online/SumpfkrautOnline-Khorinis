using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.GUI;

namespace GUC.Scripts.Sumpfkraut.GUI.MainMenu
{
    class MainMenuChoice : MainMenuItem, InputReceiver
    {
        const int width = 200;
        const int height = 20;

        GUCVisual vis;
        GUCVisualText choiceText { get { return vis.Texts[0]; } }
        GUCVisual titleVis;

        Dictionary<int, string> original;
        List<KeyValuePair<int, string>> choices;
        public Dictionary<int, string> Choices
        {
            get { return original; }
            set
            {
                if (value == original)
                    return;

                if (sorted)
                { //sort by alphabet
                    choices = value.OrderBy(x => x.Value).ToList();
                }
                else
                {
                    choices = value.ToList();
                }

                if (cursor >= value.Count)
                {
                    cursor = value.Count - 1;
                }

                original = value;
                UpdateChoiceText();
            }
        }
        public int Choice { get { return choices[cursor].Key; } }

        bool sorted;

        int cursor = 0;

        Action OnChange;

        public MainMenuChoice(string title, string help, int x, int y, Dictionary<int,string> choices, bool sorted, Action OnActivate, Action OnChange)
        {
            HelpText = help;
            this.sorted = sorted;
            this.OnActivate = OnActivate;
            this.OnChange = OnChange;

            titleVis = GUCVisualText.Create(title, x + (width - (int)StringPixelWidth(title)) / 2, y - 15);

            vis = new GUCVisual(x, y, width, height);
            vis.SetBackTexture("Menu_Choice_Back.tga");
            vis.CreateText("");

            sorted = false;
            Choices = choices;
        }

        void UpdateChoiceText()
        {
            choiceText.Text = sorted ? string.Format("{0} ({1})", choices[cursor].Value, cursor+1) : choices[cursor].Value;
        }

        public override void Show()
        {
            vis.Show();
            titleVis.Show();
        }

        public override void Hide()
        {
            vis.Hide();
            titleVis.Hide();
        }

        public override void Select()
        {
            vis.Font = Fonts.Default_Hi;
            titleVis.Font = Fonts.Default_Hi;
        }

        public override void Deselect()
        {
            vis.Font = Fonts.Default;
            titleVis.Font = Fonts.Default;
        }

        public void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Left)
            {
                cursor--;
                if (cursor < 0)
                {
                    cursor = choices.Count - 1;
                }
            }
            else if (key == VirtualKeys.Right)
            {
                cursor++;
                if (cursor >= choices.Count)
                {
                    cursor = 0;
                }
            }
            else return;

            UpdateChoiceText();
            if (OnChange != null)
                OnChange();
        }
    }
}
