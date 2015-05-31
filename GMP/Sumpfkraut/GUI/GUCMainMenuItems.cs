using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GUC.Types;
using GUC.Sumpfkraut;

namespace GUC.Sumpfkraut.GUI
{
    interface GUCMainMenuItem
    {
        void Center();
        string GetHelpText();
        void Enable();
        void Disable();
        void Show();
        void Hide();
        void Func();
    }

    class GUCMainMenuButton : GUCMainMenuItem
    {
        GUCMenuText titleText;
        string helpText;
        bool menuFont;

        public EventHandler func;

        public Login.LoginMessage.CharInfo charInfo;

        public bool ignore;

        public GUCMainMenuButton(string text, string helpText, int x, int y, bool menuFont)
        {
            this.helpText = helpText;
            titleText = new GUCMenuText(text, x, y,menuFont);
            this.menuFont = menuFont;
            ignore = false;
        }

        public void Func()
        {
            if (func != null)
                func(null,null);
        }

        public void Show()
        {
            titleText.Show();
        }

        public void Hide()
        {
            titleText.Hide();
        }

        public void Enable()
        {
            if (menuFont) titleText.SetFont("FONT_OLD_20_WHITE_HI.TGA");
            else titleText.SetFont("FONT_OLD_10_WHITE_HI.TGA");
        }

        public void Disable()
        {
            if (menuFont) titleText.SetFont("FONT_OLD_20_WHITE.TGA");
            else titleText.SetFont("FONT_OLD_10_WHITE.TGA");
        }

        public string GetHelpText()
        {
            return helpText;
        }

        public void Center()
        {
            titleText.CenterText();
        }

        public void SetText(string text)
        {
            titleText.Text = text;
        }

        public void SetHelpText(string text)
        {
            helpText = text;
        }

        public void SetColor(ColorRGBA color)
        {
            titleText.SetColor(color);
        }
    }

    class GUCMainMenuTextBox : GUCMainMenuItem
    {
        string helpText;

        GUCMenuText titleText;
        GUCMenuTexture backTex;
        GUCMenuTextBox tb;
        int width;

        public EventHandler func;

        public GUCMenuTextBox TextBox
        {
            get { return tb; }
            private set { }
        }

        public GUCMainMenuTextBox(string text, string helpText, int x, int y, int w)
        {
            this.helpText = helpText;
            width = w;
            titleText = new GUCMenuText(text, x, y, true);
            backTex = new GUCMenuTexture("Menu_Choice_Back.tga", x + titleText.pixelLen + 20 , y, width, 30);
            tb = new GUCMenuTextBox(x + titleText.pixelLen + 35, y + 5, width - 30, true);
            tb.allowWhiteSpaces = false;
        }

        public void Func()
        {
            if (func != null)
                func(null, null);
        }

        public void Show()
        {
            titleText.Show();
            backTex.Show();
            tb.Show();
        }

        public void Hide()
        {
            titleText.Hide();
            backTex.Hide();
            tb.Hide();
        }

        public void Enable()
        {
            titleText.SetFont("FONT_OLD_20_WHITE_HI.TGA");
            tb.Enabled = true;
        }

        public void Disable()
        {
            titleText.SetFont("FONT_OLD_20_WHITE.TGA");
            tb.Enabled = false;
        }

        public string GetHelpText()
        {
            return helpText;
        }

        public void Center()
        {
            int[] screenSize = InputHooked.GetScreenSize(Process.ThisProcess());
            titleText.SetPos((screenSize[0] - 640) / 2 + 100, -1);
            backTex.SetPos((screenSize[0] + 640) / 2 - width - 100, -1);
            tb.SetXSize((screenSize[0] + 640) / 2 - 100 - width + 15, width - 30);
        }
    }

    class GUCMainMenuChoice : GUCMainMenuItem
    {
        string helpText;
        GUCMenuText titleText;
        GUCMenuTexture backTex;
        GUCMenuText choiceText;
        Dictionary<int, string> choices;
        
        int cursor;
        int[] pos;
        const int width = 200;

        public EventHandler func;
        public event EventHandler OnChange;

        private bool sorted;
        public bool Sorted
        {
            get { return sorted; }
            set
            {
                sorted = value;
                SetChoices(choices);
            }
        }

        public int Choice
        {
            private set { }
            get { return choices.Keys.ToArray()[cursor]; }
        }

        public GUCMainMenuChoice(string title, string helpText, int x, int y, Dictionary<int, string> choices)
        {
            cursor = 0;
            
            this.helpText = helpText;
            pos = new int[] { x, y };

            titleText = new GUCMenuText(title, x + (width - InputHandler.StringPixelWidth(title))/2, y - 15, false);
            backTex = new GUCMenuTexture("Menu_Choice_Back.tga", x, y, width, 20);
            choiceText = new GUCMenuText("", 0, 0, false);

            sorted = false;
            SetChoices(choices);
        }

        public void Func()
        {
            if (func != null)
                func(null, null);
        }

        public void ChangeChoice(bool next)
        {
            if (next)
            {
                cursor++;
                if (cursor >= choices.Count)
                    cursor = 0;
            }
            else
            {
                cursor--;
                if (cursor < 0)
                {
                    cursor = choices.Count - 1;
                }
            }

            choiceText.Text = choices.Values.ToArray()[cursor];
            if (sorted) choiceText.Text += " (" + (cursor + 1) + ")";
            choiceText.SetPos(pos[0] + (width - InputHandler.StringPixelWidth(choiceText.Text)) / 2, pos[1] + 2);

            if (OnChange != null)
                OnChange(this, null);
        }

        public void SetChoices(Dictionary<int,string> newChoices)
        {
            if (sorted)
            {
                Dictionary<int,string> dict = newChoices.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                if (dict == choices)
                    return;
                choices = dict;
            }
            else
            {
                if (newChoices == choices)
                    return;
                choices = newChoices;
            }

            if (cursor >= newChoices.Count)
            {
                cursor = newChoices.Count - 1;
            }

            choiceText.Text = choices.Values.ToArray()[cursor];
            if (sorted) choiceText.Text += " (" + (cursor + 1) + ")";
            choiceText.SetPos(pos[0] + (width - InputHandler.StringPixelWidth(choiceText.Text)) / 2, pos[1] + 2);

            if (OnChange != null)
                OnChange(this, null);
        }

        public void Show()
        {
            titleText.Show();
            backTex.Show();
            choiceText.Show();
        }

        public void Hide()
        {
            titleText.Hide();
            backTex.Hide();
            choiceText.Hide();
        }

        public string GetHelpText()
        {
            return helpText;
        }

        public void Enable()
        {
            titleText.SetFont("Font_Old_10_White_Hi.tga");
            choiceText.SetFont("Font_Old_10_White_Hi.tga");
        }

        public void Disable()
        {
            titleText.SetFont("Font_Old_10_White.tga");
            choiceText.SetFont("Font_Old_10_White.tga");
        }

        public void Center()
        {

        }
    }

    class GUCMainMenuCharacter : GUCMainMenuItem
    {
        zCVob charVob;
        zCView charView;
        oCItem charItem;
        oCNpc charNpc;

        private int bodyMesh;
        private int bodyTex;
        private int headMesh;
        private int headTex;
        private float fatness;
        private zVec3 bodyScale;

        public EventHandler func;

        public int BodyMesh
        {
            set
            {
                bodyMesh = value;
                UpdateCharacterVisual();
            }
            get { return bodyMesh; }
        }

        public int BodyTex
        {
            set
            {
                bodyTex = value;
                UpdateCharacterVisual();
            }
            get { return bodyTex; }
        }

        public int HeadMesh
        {
            set
            {
                headMesh = value;
                UpdateCharacterVisual();
            }
            get { return headMesh; }
        }

        public int HeadTex
        {
            set
            {
                headTex = value;
                UpdateCharacterVisual();
            }
            get { return headTex; }
        }

        public float Fatness
        {
            set
            {
                fatness = value;
                charNpc.SetFatness(value);
            }
            get { return fatness; }
        }
        
        public float BodyHeight
        {
            set
            {
                bodyScale.Y = value;
                charNpc.SetModelScale(bodyScale);
            }
            get { return bodyScale.Y; }
        }

        public float BodyWidth
        {
            set
            {
                bodyScale.X = value;
                bodyScale.Z = value;
                charNpc.SetModelScale(bodyScale);
            }
            get { return bodyScale.X; }
        }

        GUCMenuTexture rightArrow;
        GUCMenuTexture leftArrow;
        bool shown;

        public GUCMainMenuCharacter(int x, int y, int w, int h)
        {
            Process proc = Process.ThisProcess();
            charVob = zCVob.Create(proc);

            charNpc = new oCNpc(proc, charVob.Address);
            charNpc.SetVisual("HUMANS.MDS");
            bodyMesh = 0;
            bodyTex = 1;
            headMesh = 0;
            headTex = 58;
            UpdateCharacterVisual();

            fatness = 0;
            bodyScale = zVec3.Create(proc);
            bodyScale.X = 1.0f;
            bodyScale.Y = 1.0f;
            bodyScale.Z = 1.0f;

            rotation = 180;
            proc.Write(140, charVob.Address + (int)oCItem.Offsets.inv_zbias);
            proc.Write(rotation, charVob.Address + (int)oCItem.Offsets.inv_roty);

            int[] vpos = InputHooked.PixelToVirtual(proc, new int[] { x, y });
            int[] vsize = InputHooked.PixelToVirtual(proc, new int[] { w, h });
            charView = zCView.Create(proc, vpos[0], vpos[1], vpos[0] + vsize[0], vpos[1] + vsize[1]);
            charView.FillZ = true;
            charView.Blit();

            charItem = new oCItem(proc, charVob.Address);

            rightArrow = new GUCMenuTexture("R.TGA", x + w - 170, y + h / 2 - 40, 15, 20);
            leftArrow = new GUCMenuTexture("L.TGA", x + 150, y + h / 2 - 40, 15, 20);

            shown = false;
        }

        public void Func()
        {
            if (func != null)
                func(null, null);
        }

        private void UpdateCharacterVisual()
        {
            charNpc.SetAdditionalVisuals(((GUC.Enumeration.BodyMesh)bodyMesh).ToString(), bodyTex, 0, ((GUC.Enumeration.HeadMesh)headMesh).ToString(), headTex, 0, -1);
        }

        public void Show()
        {
            if (!shown)
            {
                ItemRenderer.renderList.Add(charView, charItem);
                shown = true;
            }
        }

        public void Hide()
        {
            if (shown)
            {
                ItemRenderer.renderList.Remove(charView);
                rightArrow.Hide();
                leftArrow.Hide();
                shown = false;
            }
        }

        public void Enable()
        {
            rightArrow.Show();
            leftArrow.Show();
        }

        public void Disable()
        {
            rightArrow.Hide();
            leftArrow.Hide();
        }

        private int rotation;
        public void Rotate(bool right)
        {
            rotation += right ? -5 : 5;   
            Process.ThisProcess().Write(rotation, charVob.Address + (int)oCItem.Offsets.inv_roty);
        }

        public string GetHelpText()
        {
            return "Drücke links und rechts um den Charakter zu drehen";
        }

        public void Center()
        {

        }
    }
}
