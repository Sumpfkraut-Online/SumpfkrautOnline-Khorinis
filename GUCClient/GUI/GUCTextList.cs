using System.Collections.Generic;
using GUC.Types;

using System;
using System.Linq;
using System.Text;
using WinApi;
using WinApi.User.Enumeration;
using GUC.Client.GUI;
//using Gothic.zClasses;

namespace GUC.Client.GUI
{
    class GUCTextList : GUCView
    {
        List<TextLine> TextList;
        GUCVisual bgTexture;
        string basicTex;
        int x, y, width, height, lines;
        int scrollShift; // anchor for scrolling in the textlist
        public bool Scrolled = false;
        public bool IsShown = false;
        public bool Highlighted = false;

        public GUCTextList(int x, int y, int width, int height, string bgTex, int maxTextLines, int lineDistances)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.lines = maxTextLines;

            bgTexture = new GUCVisual(x, y, width, height);
            bgTexture.SetBackTexture(bgTex);
            this.basicTex = bgTex;

            for (int i = 0; i < lines; i++)
            {
                bgTexture.CreateText("", 0, lineDistances * i);
            }

            TextList = new List<TextLine>();
        }

        public override void Show()
        {
            bgTexture.Show();
            IsShown = true;
        }

        public override void Hide()
        {
            bgTexture.Hide();
            IsShown = false;
            Scrolled = false;
            Highlight(false);
        }

        public void AddText(string text, ColorRGBA color)
        {
            int size = 0;
            int counter = 0;
            foreach (char c in text)
            {
                if (size < width - 80)
                {
                    size += InputHandler.GetCharPixelWidth(c);
                    counter++;
                }
                else
                {
                    zERROR.GetZErr(Program.Process).Report(2, 'G', "adding text: " + text.Substring(0, counter), 0, "hGame.cs", 0);
                    TextList.Add(new TextLine(text.Substring(0, counter), color));
                    text = "     " + text.Substring(counter + 1);
                    size = 0;
                    counter = 0;
                }
            }
            zERROR.GetZErr(Program.Process).Report(2, 'G', "adding text: " + text, 0, "hGame.cs", 0);
            TextList.Add(new TextLine(text, color));

            if (!Scrolled)
            {
                Scroll(0);
            }
        }

        public void Scroll(int mode)
        {
            // 1 => DOWN | -1 => UP | 0 => reset
            switch (mode)
            {
                case 1:
                    if (scrollShift == TextList.Count - lines)
                    {
                        Scrolled = false;
                        if (Highlighted)
                        {
                            Highlight(false);
                        }
                    }
                    else if (scrollShift < TextList.Count - lines)
                    {
                        scrollShift++;
                    }
                    break;
                case -1:
                    if (scrollShift > 0)
                    {
                        scrollShift--;
                        Scrolled = true;
                    }
                    break;
                case 0:
                    scrollShift = TextList.Count > lines ? TextList.Count - lines : 0;
                    break;
            }
            UpdateTextLines();
        }

        public void UpdateTextLines()
        {
            for (int i = 0; i < lines; i++)
            {
                if ((scrollShift + i) < TextList.Count)
                {
                    bgTexture.Texts[i].Text = TextList[scrollShift + i].Text;
                    bgTexture.Texts[i].SetColor(TextList[scrollShift + i].Color);
                }
            }
        }

        public void ChangeBGTex(string texture)
        {
            bgTexture.SetBackTexture(texture);
        }

        public void Highlight(bool mode)
        {
            // anything else? display new tex for e.g...??
            if (mode)
                bgTexture.SetBackTexture("Inv_Back_Sell.tga");
            else
                bgTexture.SetBackTexture(basicTex);

            Highlighted = mode;
        }
    }

    class TextLine
    {
        public ColorRGBA Color;
        public string Text;

        public TextLine(string text, ColorRGBA color)
        {
            this.Text = text;
            this.Color = color;
        }
    }

}